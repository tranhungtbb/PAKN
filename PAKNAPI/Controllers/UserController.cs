using Bugsnag;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PAKNAPI.Common;
using PAKNAPI.ModelBase;
using PAKNAPI.Services.FileUpload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using PAKNAPI.App_Helper;
using PAKNAPI.Models.Results;
using PAKNAPI.Models.Login;
using System.Security.Claims;
using System.Globalization;
using PAKNAPI.Models;
using PAKNAPI.Models.User;
using System.IO;

namespace PAKNAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : BaseApiController
	{
		private readonly IFileService _fileService;
		private readonly IAppSetting _appSetting;
		private readonly IClient _bugsnag;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private Microsoft.Extensions.Configuration.IConfiguration _config;
		public UserController(IFileService fileService, IAppSetting appSetting, IClient bugsnag,
			IHttpContextAccessor httpContextAccessor, Microsoft.Extensions.Configuration.IConfiguration config)
		{
			_fileService = fileService;
			_appSetting = appSetting;
			_bugsnag = bugsnag;
			_httpContextAccessor = httpContextAccessor;
			_config = config;
		}

		[HttpPost, DisableRequestSizeLimit]
		[Route("Create")]
		[Authorize]
		public async Task<object> Create([FromForm] SYUserInsertIN model, [FromForm] IFormFileCollection files = null)
		{
			// tải file
			string filePath = "";
			

			try
			{
				if (files != null && files.Any())
				{
					var info = await _fileService.Save(files, "User");
					filePath = info[0].Path;
				}

				//generate mật khẩu
				byte[] salt = new byte[128 / 8];
				using (var rng = RandomNumberGenerator.Create())
				{
					rng.GetBytes(salt);
				}
				// derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
				string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
					password: "12345",
					salt: salt,
					prf: KeyDerivationPrf.HMACSHA1,
					iterationCount: 10000,
					numBytesRequested: 256 / 8));

				// thêm người dùng vào db
				model.Avatar = filePath;
				model.Password = hashed;
				model.Salt = Convert.ToBase64String(salt);

				var result = await new SYUserInsert(_appSetting).SYUserInsertDAO(model);
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);
				//xóa file đã tải
				if (!string.IsNullOrEmpty(filePath))
					await _fileService.Remove(filePath);
				return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
			new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
			return new Models.Results.ResultApi { Success = ResultCode.OK };
		}

		[HttpPost, DisableRequestSizeLimit]
		[Route("Update")]
		[Authorize]
		public async Task<object> Update([FromForm] SYUserUpdateIN model, [FromForm] IFormFileCollection files = null)
		{
			// tải file
			string filePath = "";
			try
			{
				var modelOld = await new SYUserGetByID(_appSetting).SYUserGetByIDDAO(model.Id);
				if (files != null && files.Any())
				{
					var info = await _fileService.Save(files, "User");
					filePath = info[0].Path;

					//xóa avatar cũ
					if(!string.IsNullOrEmpty(modelOld[0].Avatar))
						await _fileService.Remove(modelOld[0].Avatar);
					if (!string.IsNullOrEmpty(filePath))
					{
						model.Avatar = filePath;
					}
				}


				model.Password = modelOld[0].Password;
				model.Salt = modelOld[0].Salt;
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
				var result = await new SYUserUpdate(_appSetting).SYUserUpdateDAO(model);

			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);
				//xóa file đã tải
				await _fileService.Remove(filePath);
				return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
			new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
			return new Models.Results.ResultApi { Success = ResultCode.OK };
		}

		[HttpPost, DisableRequestSizeLimit]
		[Route("Delete")]
		[Authorize]
		public async Task<object> Delete(SYUserDeleteIN model)
		{
			try
			{
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
				var modelOld = (await new SYUserGetByID(_appSetting).SYUserGetByIDDAO(model.Id)).FirstOrDefault();

				var result = await new SYUserDelete(_appSetting).SYUserDeleteDAO(model);
				if (result > 0)
				{
					// xóa avatar cũ
					if (modelOld != null && string.IsNullOrEmpty(modelOld.Avatar))
					{
						await _fileService.Remove(modelOld.Avatar);
					}
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
					return new Models.Results.ResultApi { Success = ResultCode.OK, Result = result };
				}
				else {
					return new Models.Results.ResultApi { Success = ResultCode.ORROR, Result = result };
				}
				

			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);
				return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
			new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
			return new Models.Results.ResultApi { Success = ResultCode.OK };
		}

		[HttpGet]
		[Route("GetAvatar/{id}")]
		[Authorize]
		public async Task<byte[]> GetAvatar(int id)
		{
			var modelOld = await new SYUserGetByID(_appSetting).SYUserGetByIDDAO(id);
			if (string.IsNullOrEmpty(modelOld[0].Avatar?.Trim())) return null;
			var data = await _fileService.GetBinary(modelOld[0].Avatar);

			return data;
		}

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("SYUserGetAllOnPageList")]
		public async Task<ActionResult<object>> SYUserGetAllOnPageList(int? PageSize, int? PageIndex, string UserName, string FullName, string Phone, bool? IsActived, int? UnitId, int? TypeId, int? PositionId)
		{
			try
			{
				List<SYUserGetAllOnPageList> rsSYUserGetAllOnPage = await new SYUserGetAllOnPageList(_appSetting).SYUserGetAllOnPageDAO(PageSize, PageIndex, UserName, FullName, Phone, IsActived, UnitId, TypeId, PositionId);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYUserGetAllOnPage", rsSYUserGetAllOnPage},
						{"TotalCount", rsSYUserGetAllOnPage != null && rsSYUserGetAllOnPage.Count > 0 ? rsSYUserGetAllOnPage[0].RowNumber : 0},
						{"PageIndex", rsSYUserGetAllOnPage != null && rsSYUserGetAllOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsSYUserGetAllOnPage != null && rsSYUserGetAllOnPage.Count > 0 ? PageSize : 0},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}


		#region nguoi dan, doanh nghiep
		[HttpPost]
		[Route("OrganizationRegister")]
		public async Task<object> OrganizationRegister([FromForm] RegisterModel loginInfo, [FromForm] BIBusinessInsertIN model,
			[FromForm] string _RepresentativeBirthDay,
			[FromForm] string _DateOfIssue)
        {
			try
			{
				if (loginInfo.Password != loginInfo.RePassword)
				{
					return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Mật khẩu không khớp" };
				}
				//var hasOne = await new SYUserGetByUserName(_appSetting).SYUserGetByUserNameDAO(loginInfo.Phone);
				//if (hasOne != null && hasOne.Any()) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại tài khoản đăng nhập đã tồn tại" };


				var accCheckExist = await new SYUserCheckExists(_appSetting).SYUserCheckExistsDAO("Phone", loginInfo.Phone, 0);
				if (accCheckExist[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại đã tồn tại" };
				accCheckExist = await new SYUserCheckExists(_appSetting).SYUserCheckExistsDAO("Email", model.Email, 0);
				if (accCheckExist[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Email đã tồn tại" };

				DateTime birdDay, dateOfIssue;
				if (!DateTime.TryParseExact(_RepresentativeBirthDay, "dd/MM/yyyy", null, DateTimeStyles.None, out birdDay))
				{
					//return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Định dạng ngày sinh không hợp lệ" };
				}
				if (!DateTime.TryParseExact(_DateOfIssue, "dd/MM/yyyy", null, DateTimeStyles.None, out dateOfIssue))
				{
					//return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Định dạng ngày cấp không hợp lệ" };
				}

				///Phone,Email,IDCard
				///check ton tai
				var checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("Phone",loginInfo.Phone,0);
				if(checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại đã tồn tại" };
                if (!string.IsNullOrEmpty(model.Email))
                {
					checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("Email", model.Email, 0);
					if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Email người đại diện đã tồn tại" };
				}
				checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("IDCard", model.IDCard, 0);
				if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số CMND / CCCD đã tồn tại" };
				checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("OrgPhone", model.OrgPhone, 0);
				if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại doanh nghiệp đã tồn tại" };
                if (!string.IsNullOrEmpty(model.OrgEmail))
                {
					checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("OrgEmail", model.OrgEmail, 0);
					if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Email doanh nghiệp đã tồn tại" };
				}
				checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("BusinessRegistration", model.BusinessRegistration, 0);
				if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số đăng ký kinh doanh đã tồn tại" };
				checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("DecisionOfEstablishing", model.DecisionOfEstablishing, 0);
				if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số quyết định thành lập đã tồn tại" };


				///mod loginInfo
				///
				var pwd = generatePassword(loginInfo.Password);
				var account = new SYUserInsertIN
				{
					Password = pwd["Password"],
					Salt = pwd["Salt"],
					Phone = loginInfo.Phone,
					Email = model.Email,
					UserName = loginInfo.Phone,
					FullName = model.RepresentativeName,
					Gender = model.RepresentativeGender,
					Address = model.Address,

					TypeId = 3,
					Type = 3,
					IsActived = true,
					IsDeleted = false,
					CountLock = 0,
					LockEndOut = DateTime.Now,
					IsSuperAdmin = false,

				};
				var rs1 = await new SYUserInsert(_appSetting).SYUserInsertDAO(account);

				var accRs = await new SYUserGetByUserName(_appSetting).SYUserGetByUserNameDAO(account.UserName);

				///mod model
				///
				if (string.IsNullOrEmpty(_DateOfIssue)) model.DateOfIssue = null;
				else model.DateOfIssue = dateOfIssue;
				if (string.IsNullOrEmpty(_RepresentativeBirthDay)) model.RepresentativeBirthDay = null;
				else model.RepresentativeBirthDay = birdDay;
				model.CreatedDate = DateTime.Now;
				model.CreatedBy = 0;
				model.UpdatedBy = 0;
				model.UpdatedDate = DateTime.Now;
				model.Status = 1;
				model.IsDeleted = false;
				model.UserId = accRs[0].Id;

				var rs2 = await new BIBusinessInsert(_appSetting).BIBusinessInsertDAO(model);

			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);
				return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}

			return new Models.Results.ResultApi { Success = ResultCode.OK };
		}

		[HttpPost]
		[Route("InvididualRegister")]
		public async Task<object> InvididualRegister(
			[FromForm] RegisterModel loginInfo, 
			[FromForm] BIIndividualInsertIN model,
			[FromForm] string _BirthDay,
			[FromForm] string _DateOfIssue)
		{


            try
            {
				if (loginInfo.Password != loginInfo.RePassword)
				{
					return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Mật khẩu không khớp" };
				}

				//var hasOne = await new SYUserGetByUserName(_appSetting).SYUserGetByUserNameDAO(loginInfo.Phone);
				//if(hasOne != null && hasOne.Any()) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại đã tồn tại" };

				var accCheckExist = await new SYUserCheckExists(_appSetting).SYUserCheckExistsDAO("Phone", loginInfo.Phone,0);
				if (accCheckExist[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại đã tồn tại" };
				accCheckExist = await new SYUserCheckExists(_appSetting).SYUserCheckExistsDAO("Email", model.Email, 0);
				if (accCheckExist[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Email đã tồn tại" };

				DateTime birdDay, dateOfIssue;
				if (!DateTime.TryParseExact(_BirthDay, "dd/MM/yyyy", null, DateTimeStyles.None, out birdDay))
				{
					//return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Định dạng ngày sinh không hợp lệ" };
				}
				if (!DateTime.TryParseExact(_DateOfIssue, "dd/MM/yyyy", null, DateTimeStyles.None, out dateOfIssue))
				{
					//return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Định dạng ngày cấp không hợp lệ" };
				}

				///Phone,Email,IDCard
				///check ton tai
				var checkExists = await new BIIndividualCheckExists(_appSetting).BIIndividualCheckExistsDAO("Phone", loginInfo.Phone, 0);
				if(checkExists[0].Exists.Value)
					return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại đã tồn tại" };
                if (!string.IsNullOrEmpty(model.Email))
                {
					checkExists = await new BIIndividualCheckExists(_appSetting).BIIndividualCheckExistsDAO("Email", model.Email, 0);
					if (checkExists[0].Exists.Value)
						return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Email đã tồn tại" };
				}
				checkExists = await new BIIndividualCheckExists(_appSetting).BIIndividualCheckExistsDAO("IDCard", model.IDCard, 0);
				if (checkExists[0].Exists.Value)
					return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số CMND / CCCD đã tồn tại" };


				///mod loginInfo
				///
				var pwd = generatePassword(loginInfo.Password);
				var account = new SYUserInsertIN 
				{
					Password = pwd["Password"],
					Salt = pwd["Salt"],
					Phone = loginInfo.Phone,
					Email = model.Email,
					UserName = loginInfo.Phone,
					FullName = model.FullName,
					Gender = model.Gender,
					Address = model.Address,

					TypeId = 2,
					Type = 2,
					IsActived = true,
					IsDeleted= false,
					CountLock = 0,
					LockEndOut = DateTime.Now,
					IsSuperAdmin=false,
					
				};
					var rs1 = await new SYUserInsert(_appSetting).SYUserInsertDAO(account);

				var accRs = await new SYUserGetByUserName(_appSetting).SYUserGetByUserNameDAO(account.UserName);
				///mod model
				///
				if (string.IsNullOrEmpty(_DateOfIssue)) model.DateOfIssue = null;
				else model.DateOfIssue = dateOfIssue;
				if (string.IsNullOrEmpty(_BirthDay)) model.BirthDay = null;
				else model.BirthDay = birdDay;
				model.CreatedDate = DateTime.Now;
				model.CreatedBy = 0;
				model.UpdatedBy = 0;
				model.UpdatedDate = DateTime.Now;
				model.Status = 1;
				model.IsDeleted = false;
				model.UserId = accRs[0].Id;
				//string contentSMSOPT = "(Trung tam tiep nhan PAKN tinh Khanh Hoa) Xin chao: " + model.FullName +". Mat khau dang nhap he thong la: " + account.Password + ". Xin cam on!";
				//SV.MailSMS.Model.SMTPSettings settings = new SV.MailSMS.Model.SMTPSettings();
				//settings.COM = _config["SmsCOM"].ToString();
				//SV.MailSMS.Control.SMSs sMSs = new SV.MailSMS.Control.SMSs(settings);
				//sMSs.SendOTPSMS(model.Phone,contentSMSOPT);
				var rs2 = await new BIIndividualInsert(_appSetting).BIIndividualInsertDAO(model);

			}
			catch (Exception ex)
            {
				_bugsnag.Notify(ex);
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);
				return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = ex.Message};
			}

			return new Models.Results.ResultApi { Success = ResultCode.OK };
		}

		[HttpGet]
		[Route("SendDemo")]
		public string SendDemo()
        {
            try
			{
				string contentSMSOPT = "(Trung tam tiep nhan PAKN tinh Khanh Hoa) Xin chao: Trần Thanh Quyền. Mat khau dang nhap he thong la: abcAbc123123. Xin cam on!";
				SV.MailSMS.Model.SMTPSettings settings = new SV.MailSMS.Model.SMTPSettings();
				settings.COM = _config["SmsCOM"].ToString();
				SV.MailSMS.Control.SMSs sMSs = new SV.MailSMS.Control.SMSs(settings);
				string MessageResult = sMSs.SendOTPSMS("0984881580", contentSMSOPT);
				return "ok";
			}
            catch (Exception ex)
			{
				return ex.Message;
			}
		}

		[HttpGet]
		[Route("UserGetInfo")]
		[Authorize]
		public async Task<object> UserGetInfo(long? id)
        {
			try
            {
				var users = _httpContextAccessor.HttpContext.User.Identities.FirstOrDefault().Claims; //FindFirst(ClaimTypes.NameIdentifier);

				var userId = users.FirstOrDefault(c=> c.Type.Equals("Id", StringComparison.OrdinalIgnoreCase)).Value;
				var accInfo = await new SYUserGetByID(_appSetting).SYUserGetByIDDAO(long.Parse(userId));
				
				if(accInfo == null || !accInfo.Any())
                {
					return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Tài khoản không tồn tại" };
				}

                if (accInfo[0].TypeId == 1)
                {
					return new Models.Results.ResultApi { Success = ResultCode.OK, Result = accInfo[0] };
				}
                else if (accInfo[0].TypeId == 2)
                {
					var info = await new BIIndividualGetByUserId(_appSetting).BIIndividualGetByUserIdDAO(accInfo[0].Id);
					if(info == null || !info.Any())
                    {
						return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Thông tin tài khoản không tồn tại" };
					}

					var model = new AccountInfoModel
					{
						UserName = accInfo[0].UserName,
						FullName = info[0].FullName,
						Gender = info[0].Gender.Value,
						DateOfBirth = info[0].BirthDay.Value.ToString("dd/MM/yyyy"),
						Email = info[0].Email,
						Phone = info[0].Phone,
						Nation = info[0].Nation,
						ProvinceId = info[0].ProvinceId,
						DistrictId = info[0].DistrictId,
						WardsId = info[0].WardsId,
						Address = info[0].Address,
						IdCard = info[0].IDCard,
						IssuedPlace = info[0].IssuedPlace,
						IssuedDate = info[0].DateOfIssue.Value.ToString("dd/MM/yyyy"),
					};
                    if (!model.ProvinceId.HasValue)
                    {
						model.DistrictId = null;
						model.WardsId = null;
                    }


					return new Models.Results.ResultApi { Success = ResultCode.OK, Result = model };

				}
				else if (accInfo[0].TypeId == 3)
                {
					var info = await new BIBusinessGetByUserId(_appSetting).BIBusinessGetByUserIdDAO(accInfo[0].Id);
					if (info == null || !info.Any())
					{
						return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Thông tin tài khoản không tồn tại" };
					}

					BusinessAccountInfoModel model = new BusinessAccountInfoModel(info[0]);

					model.UserName = accInfo[0].UserName;
					model.FullName = accInfo[0].FullName;
					if (!model.ProvinceId.HasValue)
					{
						model.DistrictId = null;
					}
                    if (!model.OrgProvinceId.HasValue)
                    {
						model.OrgDistrictId = null;
						model.OrgWardsId = null;
                    }

					return new Models.Results.ResultApi { Success = ResultCode.OK, Result = model };
				}

			}
			catch(Exception ex)
            {
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);
				return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}

			return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Thông tin tài khoản không tồn tại"};
		}

		[HttpPost]
		[Route("UserChagePwd")]
		[Authorize]
		public async Task<object> UserChagePwd([FromForm] ChangePwdModel model)
        {
            try
            {
				var users = _httpContextAccessor.HttpContext.User.Identities.FirstOrDefault().Claims; //FindFirst(ClaimTypes.NameIdentifier);

				var userId = users.FirstOrDefault(c => c.Type.Equals("Id", StringComparison.OrdinalIgnoreCase)).Value;
				var accInfo = await new SYUserGetByID(_appSetting).SYUserGetByIDDAO(long.Parse(userId));
				if (accInfo == null || !accInfo.Any())
				{
					return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Tài khoản không tồn tại" };
				}
                if (string.IsNullOrEmpty(model.OldPassword) || string.IsNullOrEmpty(model.NewPassword))
                {
					return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Dữ liệu không hợp lệ" };
				}
                if (model.NewPassword !=  model.RePassword)
                {
					return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Mật khẩu mới không trùng khớp" };
				}

				PasswordHasher hasher = new PasswordHasher();
				if (!hasher.AuthenticateUser(model.OldPassword, accInfo[0].Password, accInfo[0].Salt))
                {
					return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Mật khẩu cũ không đúng" };
				}

				var newPwd = generatePassword(model.NewPassword);
				var _model = new SYUserChangePwdIN
				{
					Password = newPwd["Password"],
					Salt = newPwd["Salt"]
				};
				_model.Id = accInfo[0].Id;

				var rs = await new SYUserChangePwd(_appSetting).SYUserChangePwdDAO(_model);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
				return new Models.Results.ResultApi { Success = ResultCode.OK};
			}
			catch(Exception ex)
            {
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);
				return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
        }


		[HttpPost]
		[Route("UserChangePwdInManage")]
		[Authorize]
		public async Task<object> UserChangePwdInManage(ChangePwdInManage model)
		{
			try
			{
				
				var accInfo = await new SYUserGetByID(_appSetting).SYUserGetByIDDAO(long.Parse(model.UserId.ToString()));
				if (accInfo == null || !accInfo.Any())
				{
					return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Tài khoản không tồn tại" };
				}
				
				if (model.NewPassword != model.RePassword)
				{
					return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Mật khẩu mới không trùng khớp" };
				}

				PasswordHasher hasher = new PasswordHasher();
				

				var newPwd = generatePassword(model.NewPassword);
				var _model = new SYUserChangePwdIN
				{
					Password = newPwd["Password"],
					Salt = newPwd["Salt"]
				};
				_model.Id = accInfo[0].Id;

				var rs = await new SYUserChangePwd(_appSetting).SYUserChangePwdDAO(_model);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
				return new Models.Results.ResultApi { Success = ResultCode.OK };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);
				return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}


		[HttpPost]
		[Route("UpdateCurrentInfo")]
		[Authorize]
		public async Task<object> UpdateCurrentInfo([FromForm] AccountInfoModel model,[FromForm] BusinessAccountInfoModel businessModel)
        {
            try
            {
				var users = _httpContextAccessor.HttpContext.User.Identities.FirstOrDefault().Claims; //FindFirst(ClaimTypes.NameIdentifier);

				var userId = users.FirstOrDefault(c => c.Type.Equals("Id", StringComparison.OrdinalIgnoreCase)).Value;
				var accInfo = await new SYUserGetByID(_appSetting).SYUserGetByIDDAO(long.Parse(userId));

                if (accInfo == null || !accInfo.Any())
                {
					return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Tài khoản không tồn tại" };
				}

				DateTime birdDay, dateOfIssue;
				if (!DateTime.TryParseExact(model.DateOfBirth, "dd/MM/yyyy", null, DateTimeStyles.None, out birdDay))
				{
					//return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Định dạng ngày sinh không hợp lệ" };
				}
				if (!DateTime.TryParseExact(model.IssuedDate, "dd/MM/yyyy", null, DateTimeStyles.None, out dateOfIssue))
				{
					//return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Định dạng ngày cấp không hợp lệ" };
				}

				if (accInfo[0].TypeId == 2)
                {
					///Phone,Email,IDCard
					///check ton tai
					///
					accInfo[0].FullName = model.FullName;
					var info = await new BIIndividualGetByUserId(_appSetting).BIIndividualGetByUserIdDAO(accInfo[0].Id);

					var checkExists = await new BIIndividualCheckExists(_appSetting).BIIndividualCheckExistsDAO("Phone", model.Phone, info[0].Id);
					if (checkExists[0].Exists.Value)
						return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại đã tồn tại" };
					if (!string.IsNullOrEmpty(model.Email))
					{
						checkExists = await new BIIndividualCheckExists(_appSetting).BIIndividualCheckExistsDAO("Email", model.Email, info[0].Id);
						if (checkExists[0].Exists.Value)
							return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Email đã tồn tại" };
					}
					checkExists = await new BIIndividualCheckExists(_appSetting).BIIndividualCheckExistsDAO("IDCard", model.IdCard, info[0].Id);
					if (checkExists[0].Exists.Value)
						return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số CMND / CCCD đã tồn tại" };

					var _model = new BIInvididualUpdateInfoIN
					{
						Id = info[0].Id,
						FullName = model.FullName,
						DateOfBirth = birdDay,
						Email = model.Email,
						Nation = model.Nation,
						ProvinceId = model.ProvinceId,
						DistrictId = model.DistrictId,
						WardsId = model.WardsId,
						Address = model.Address,
						IdCard = model.IdCard,
						IssuedPlace = model.IssuedPlace,
						IssuedDate = dateOfIssue,
						Gender = model.Gender,
					};

					var rs = await new BIInvididualUpdateInfo(_appSetting).BIInvididualUpdateInfoDAO(_model);
				}
				else if (accInfo[0].TypeId == 3)
                {
					///check ton tai
					///
					var info = await new BIBusinessGetByUserId(_appSetting).BIBusinessGetByUserIdDAO(accInfo[0].Id);
					var checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("Phone", businessModel.Phone, info[0].Id);
					if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại đã tồn tại" };
					if (!string.IsNullOrEmpty(model.Email))
					{
						checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("Email", businessModel.Email, info[0].Id);
						if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Email người đại diện đã tồn tại" };
					}
					checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("IDCard", businessModel.IDCard, info[0].Id);
					if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số CMND / CCCD đã tồn tại" };
					checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("OrgPhone", businessModel.OrgPhone, info[0].Id);
					if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại doanh nghiệp đã tồn tại" };
					if (!string.IsNullOrEmpty(businessModel.OrgEmail))
					{
						checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("OrgEmail", businessModel.OrgEmail, info[0].Id);
						if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Email doanh nghiệp đã tồn tại" };
					}
					checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("BusinessRegistration", businessModel.BusinessRegistration, info[0].Id);
					if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số đăng ký kinh doanh đã tồn tại" };
					checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("DecisionOfEstablishing", businessModel.DecisionOfEstablishing, info[0].Id);
					if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số quyết định thành lập đã tồn tại" };


					accInfo[0].FullName = businessModel.RepresentativeName;
					if (!DateTime.TryParseExact(businessModel.RepresentativeBirthDay, "dd/MM/yyyy", null, DateTimeStyles.None, out birdDay))
					{
						//return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Định dạng ngày sinh không hợp lệ" };
					}
					if (!DateTime.TryParseExact(businessModel.DateOfIssue, "dd/MM/yyyy", null, DateTimeStyles.None, out dateOfIssue))
					{
						//return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Định dạng ngày cấp không hợp lệ" };
					}

					
					
					var _model = new BIBusinessUpdateInfoIN
					{
						Id = info[0].Id,
						FullName = businessModel.RepresentativeName,
						DateOfBirth = birdDay,
						Email = model.Email,
						Nation = model.Nation,
						ProvinceId = model.ProvinceId,
						DistrictId = model.DistrictId,
						WardsId = model.WardsId,
						Address = model.Address,
						IdCard = model.IdCard,
						IssuedDate = dateOfIssue,
						Gender = model.Gender,
						BusinessRegistration = businessModel.BusinessRegistration,
						DecisionOfEstablishing = businessModel.DecisionOfEstablishing,
						Tax = businessModel.Tax,
						OrgProvinceId = businessModel.OrgProvinceId,
						OrgDistrictId = businessModel.OrgDistrictId,
						OrgWardsId = businessModel.OrgWardsId,
						OrgAddress = businessModel.OrgAddress,
						OrgPhone = businessModel.OrgPhone,
						OrgEmail = businessModel.OrgEmail,
						Business = businessModel.Business
					};
					var rs = await new BIBusinessUpdateInfo(_appSetting).BIBusinessUpdateInfoDAO(_model);
				}

				//update account

				var rsUpdateAcc = new SYUserUpdateInfo(_appSetting).SYUserUpdateInfoDAO(new SYUserUpdateInfoIN { 
					Id=accInfo[0].Id,
					FullName= accInfo[0].FullName,
					Address = accInfo[0].Address,
				});
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
				return new Models.Results.ResultApi { Success = ResultCode.OK};
			}
            catch (Exception ex)
            {
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);
				return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = ex.Message};
			}
        }

    //    public async Task<IActionResult> ExportExcelHisUser(int ? id)
    //    {
    //        using (var workbook = new XLWorkbook())
    //        {
				//List<SYSystemLogGetAllOnPage> data  = await new SYSystemLogGetAllOnPage(_appSetting).SYSystemLogGetAllOnPageDAO(id, 1000, 1, null, null);
				//var worksheet = workbook.Worksheets.Add("Users");
    //            var currentRow = 1;
    //            worksheet.Cell(currentRow, 1).Value = "Id";
    //            worksheet.Cell(currentRow, 2).Value = "Username";
				//worksheet.Cell(currentRow, 3).Value = "Id";
				//worksheet.Cell(currentRow, 4).Value = "Username";
				//worksheet.Cell(currentRow, 5).Value = "Id";
				//foreach (var item in data)
    //            {
    //                currentRow++;
    //                worksheet.Cell(currentRow, 1).Value = item.Id;
				//	worksheet.Cell(currentRow, 2).Value = item.CreatedDate;
				//	worksheet.Cell(currentRow, 3).Value = item.Action;
				//	worksheet.Cell(currentRow, 4).Value = item.Description;
				//	worksheet.Cell(currentRow, 5).Value = item.Status;
				//}

    //            using (var stream = new MemoryStream())
    //            {
    //                workbook.SaveAs(stream);
    //                var content = stream.ToArray();

    //                return File(
    //                    content,
    //                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
    //                    "users.xlsx");
    //            }
    //        }
    //    }


        #endregion


        #region private

        private Dictionary<string,string> generatePassword(string pwd)
        {
			byte[] salt = new byte[128 / 8];
			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(salt);
			}
			// derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
			string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
				password: pwd,
				salt: salt,
				prf: KeyDerivationPrf.HMACSHA1,
				iterationCount: 10000,
				numBytesRequested: 256 / 8));

			return new Dictionary<string,string>
			{
				{"Password",hashed},
				{"Salt",Convert.ToBase64String(salt) }
			};
		}

        #endregion

    }
}
