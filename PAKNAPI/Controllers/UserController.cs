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
		public UserController(IFileService fileService, IAppSetting appSetting, IClient bugsnag,
			IHttpContextAccessor httpContextAccessor)
		{
			_fileService = fileService;
			_appSetting = appSetting;
			_bugsnag = bugsnag;
			_httpContextAccessor = httpContextAccessor;
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
				}


				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
				
				if (!string.IsNullOrEmpty(filePath))
				{
					model.Avatar = filePath;
				}
				model.Password = modelOld[0].Password;
				model.Salt = modelOld[0].Salt;

				var result = await new SYUserUpdate(_appSetting).SYUserUpdateDAO(model);

			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);
				//xóa file đã tải
				await _fileService.Remove(filePath);
				return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}

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
				var modelOld = await new SYUserGetByID(_appSetting).SYUserGetByIDDAO(model.Id);

				var result = await new SYUserDelete(_appSetting).SYUserDeleteDAO(model);

				// xóa avatar cũ
				if (string.IsNullOrEmpty(modelOld[0].Avatar))
				{
					await _fileService.Remove(modelOld[0].Avatar);
				}

			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);
				return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}

			return new Models.Results.ResultApi { Success = ResultCode.OK };
		}

		[HttpGet]
		[Route("GetAvatar/{id}")]
		[Authorize]
		public async Task<byte[]> GetAvatar(int id)
		{
			var modelOld = await new SYUserGetByID(_appSetting).SYUserGetByIDDAO(id);
			if (string.IsNullOrEmpty(modelOld[0].Avatar.Trim())) return null;
			var data = await _fileService.GetBinary(modelOld[0].Avatar);

			return data;
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

				DateTime birdDay, dateOfIssue;
				if (!DateTime.TryParseExact(_RepresentativeBirthDay, "dd/MM/yyyy", null, DateTimeStyles.None, out birdDay))
				{
					//return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Định dạng ngày sinh không hợp lệ" };
				}
				if (!DateTime.TryParseExact(_DateOfIssue, "dd/MM/yyyy", null, DateTimeStyles.None, out dateOfIssue))
				{
					//return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Định dạng ngày cấp không hợp lệ" };
				}

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

				///mod model
				///
				model.DateOfIssue = dateOfIssue;
				model.RepresentativeBirthDay = birdDay;
				model.CreatedDate = DateTime.Now;
				model.CreatedBy = 0;
				model.UpdatedBy = 0;
				model.UpdatedDate = DateTime.Now;
				model.Status = 1;
				model.IsDeleted = false;

				var rs2 = await new BIBusinessInsert(_appSetting).BIBusinessInsertDAO(model);

			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);
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

				var check_email = await new BIIndividualCheckExists().BIIndividualCheckExistsDAO("Email",model.Email);
                if (check_email[0].Exists.Value)
					return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Email đã tồn tại" };

				var check_phone = await new BIIndividualCheckExists().BIIndividualCheckExistsDAO("Phone", model.Phone);
				if (check_phone[0].Exists.Value)
					return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại đã tồn tại" };

				var check_idCard = await new BIIndividualCheckExists().BIIndividualCheckExistsDAO("IDCard", model.IDCard);
				if(check_idCard[0].Exists.Value)
					return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số CMND/CCCD đã tồn tại" };

				if (loginInfo.Password != loginInfo.RePassword)
				{
					return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Mật khẩu không khớp" };
				}

				DateTime birdDay, dateOfIssue;
				
				if (!DateTime.TryParseExact(_BirthDay, "dd/MM/yyyy", null, DateTimeStyles.None, out birdDay))
				{
					//return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Định dạng ngày sinh không hợp lệ" };
				}
				
				if (!DateTime.TryParseExact(_DateOfIssue, "dd/MM/yyyy", null, DateTimeStyles.None, out dateOfIssue))
				{
					//return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Định dạng ngày cấp không hợp lệ" };
				}

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


				///mod model
				///
				model.DateOfIssue = dateOfIssue;
				model.BirthDay = birdDay;
				model.CreatedDate = DateTime.Now;
				model.CreatedBy = 0;
				model.UpdatedBy = 0;
				model.UpdatedDate = DateTime.Now;
				model.Status = 1;
				model.IsDeleted = false;

				var rs2 = await new BIIndividualInsert(_appSetting).BIIndividualInsertDAO(model);

			}
			catch (Exception ex)
            {
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);
				return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = ex.Message};
			}

			return new Models.Results.ResultApi { Success = ResultCode.OK };
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

                }
                else if (accInfo[0].TypeId == 2)
                {
					var info = await new BIIndividualGetByEmail(_appSetting).BIIndividualGetByEmailDAO(accInfo[0].Email);
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
						ProvinceId = info[0].ProvinceId.Value,
						DistrictId = info[0].DistrictId.Value,
						WardsId = info[0].WardsId.Value,
						Address = info[0].Address,
						IdCard = info[0].IDCard,
						IssuedPlace = info[0].IssuedPlace,
						IssuedDate = info[0].DateOfIssue.Value.ToString("dd/MM/yyyy"),
					};



					return new Models.Results.ResultApi { Success = ResultCode.OK, Result = model };

				}
				else if (accInfo[0].TypeId == 3)
                {
					var info = await new BIBusinessGetRepresentativeEmail(_appSetting).BIBusinessGetRepresentativeEmailDAO(accInfo[0].Email);
					if (info == null || !info.Any())
					{
						return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Thông tin tài khoản không tồn tại" };
					}

					var model = new AccountInfoModel
					{
						UserName = accInfo[0].UserName,
						FullName = info[0].RepresentativeName,
						Gender = info[0].RepresentativeGender.Value,
						DateOfBirth = info[0].RepresentativeBirthDay.ToString("dd/MM/yyyy"),
						Email = info[0].Email,
						Phone = info[0].Phone,
						Nation = info[0].Nation,
						ProvinceId = info[0].ProvinceId.Value,
						DistrictId = info[0].DistrictId.Value,
						WardsId = info[0].WardsId.Value,
						Address = info[0].Address,
						IdCard = info[0].IDCard,
						//IssuedPlace = info[0].IssuedPlace,
						//IssuedDate = info[0].DateOfIssue.Value.ToString("dd/MM/yyyy"),
					};



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
		[Route("UpdateCurrentInfo")]
		[Authorize]
		public async Task<object> UpdateCurrentInfo([FromForm] AccountInfoModel model)
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
					return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Định dạng ngày sinh không hợp lệ" };
				}
				if (!DateTime.TryParseExact(model.IssuedDate, "dd/MM/yyyy", null, DateTimeStyles.None, out dateOfIssue))
				{
					return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Định dạng ngày cấp không hợp lệ" };
				}

				if (accInfo[0].TypeId == 2)
                {
					var info = await new BIIndividualGetByEmail(_appSetting).BIIndividualGetByEmailDAO(accInfo[0].Email);

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
					var info = await new BIBusinessGetRepresentativeEmail(_appSetting).BIBusinessGetRepresentativeEmailDAO(accInfo[0].Email);
					
					var _model = new BIBusinessUpdateInfoIN
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
					var rs = await new BIBusinessUpdateInfo(_appSetting).BIBusinessUpdateInfoDAO(_model);
				}
                

				return new Models.Results.ResultApi { Success = ResultCode.OK};
			}
            catch (Exception ex)
            {
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);
				return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = ex.Message};
			}
        }


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
