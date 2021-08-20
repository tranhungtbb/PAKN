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
using Microsoft.AspNetCore.Hosting;
using PAKNAPI.Models.BusinessIndividual;
using System.Text.RegularExpressions;

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
		private readonly IWebHostEnvironment _hostingEnvironment;
		private Microsoft.Extensions.Configuration.IConfiguration _config;
		public UserController(IFileService fileService, IAppSetting appSetting, IClient bugsnag,
			IHttpContextAccessor httpContextAccessor, Microsoft.Extensions.Configuration.IConfiguration config, IWebHostEnvironment IWebHostEnvironment)
		{
			_fileService = fileService;
			_appSetting = appSetting;
			_bugsnag = bugsnag;
			_httpContextAccessor = httpContextAccessor;
			_config = config;
			_hostingEnvironment = IWebHostEnvironment;
		}

		[HttpGet]
		[Authorize]
		[Route("UsersGetDataForCreate")]
		public async Task<ActionResult<object>> UsersGetDataForCreate()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYUserGetAllOnPageList(_appSetting).UsersGetDataForCreate() };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
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
				else {
					model.Avatar = null;
				}

				//generate mật khẩu
				byte[] salt = new byte[128 / 8];
				using (var rng = RandomNumberGenerator.Create())
				{
					rng.GetBytes(salt);
				}
				// derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
				string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
					password: "12345a@@",
					salt: salt,
					prf: KeyDerivationPrf.HMACSHA1,
					iterationCount: 10000,
					numBytesRequested: 256 / 8));

				// thêm người dùng vào db
				model.Avatar = filePath;
				model.Password = hashed;
				model.Salt = Convert.ToBase64String(salt);
				model.IsSuperAdmin = false;

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
				model.IsSuperAdmin = false;
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
		[Route("UserSystemCreate")]
		[Authorize]
		public async Task<object> UserSystemCreate([FromForm] SYUserInsertIN model, [FromForm] IFormFileCollection files = null)
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
				else
				{
					model.Avatar = null;
				}

				//generate mật khẩu
				byte[] salt = new byte[128 / 8];
				using (var rng = RandomNumberGenerator.Create())
				{
					rng.GetBytes(salt);
				}
				// derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
				string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
					password: "12345a@@",
					salt: salt,
					prf: KeyDerivationPrf.HMACSHA1,
					iterationCount: 10000,
					numBytesRequested: 256 / 8));

				// thêm người dùng vào db
				model.Avatar = filePath;
				model.Password = hashed;
				model.Salt = Convert.ToBase64String(salt);
				model.IsSuperAdmin = false;

				var unitMainId = (await new SYUnitGetMainId(_appSetting).SYUnitGetMainIdDAO()).FirstOrDefault();
				if (unitMainId == null) {
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
					return new ResultApi { Success = ResultCode.ORROR, Message = "Không tồn tại đơn vị trung tâm" };
				}
				model.UnitId = unitMainId.Id;

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
		[Route("UserSystemUpdate")]
		[Authorize]
		public async Task<object> UserSystemUpdate([FromForm] SYUserSystemUpdateIN model, [FromForm] IFormFileCollection files = null)
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
					if (!string.IsNullOrEmpty(modelOld[0].Avatar))
						await _fileService.Remove(modelOld[0].Avatar);
					if (!string.IsNullOrEmpty(filePath))
					{
						model.Avatar = filePath;
					}
				}
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
				var result = await new SYUserUpdate(_appSetting).SYUserSystemUpdateDAO(model);

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
					if (modelOld != null && modelOld.Avatar != null)
					{
						string _imageToBeDeleted = Path.Combine(_hostingEnvironment.WebRootPath, modelOld.Avatar);
						//string _imageToBeDeleted = Path.Combine(_hostingEnvironment.WebRootPath, "Invitation\\", fname);
						if ((System.IO.File.Exists(_imageToBeDeleted)))
						{
							System.IO.File.Delete(_imageToBeDeleted);
						}
					}
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
					return new Models.Results.ResultApi { Success = ResultCode.OK, Result = result };
				}
				else {
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
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

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("SYUserSystemGetAllOnPageList")]
		public async Task<ActionResult<object>> SYUserSystemGetAllOnPageList(int? PageSize, int? PageIndex, string UserName, string FullName, string Phone, bool? IsActived)
		{
			try
			{
				List<SYUserSystemGetAllOnPageList> rsSYUserGetAllOnPage = await new SYUserSystemGetAllOnPageList(_appSetting).SYUserSystemGetAllOnPageDAO(PageSize, PageIndex, UserName, FullName, Phone, IsActived);
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

		[HttpGet]
		[Authorize]
		[Route("UserGetByID")]
		public async Task<ActionResult<object>> UserGetByID(long? Id)
		{
			try
			{
				List<SYUserGetByID> rsSYUserGetByID = await new SYUserGetByID(_appSetting).SYUserGetByIDDAO(Id);
				List<int> lstPermissionUserSelected = await new SYPermissionUserGetByID(_appSetting).SYPermissionUserGetByIDDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYUserGetByID", rsSYUserGetByID},
						{"lstPermissionUserSelected", lstPermissionUserSelected},
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


		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("GetOrderByUnit")]
		public async Task<ActionResult<object>> SYUserGetAllOrderByUnit()
		{
			try
			{
				List <DropListTreeView> result = new List<DropListTreeView>();
				// lst đơn vị
				List<SYUnitGetDropdown> lstUnit = await new SYUnitGetDropdown(_appSetting).SYUnitGetDropdownDAO();
				// lst người dùng theo đơn vị từng đơn vị
				DropListTreeView tinh = new DropListTreeView();
				foreach (var province in lstUnit.Where(x=>x.UnitLevel == 1).ToList()) {
					tinh = new DropListTreeView(province.Text,province.Value, new List<DropListTreeView>());
					List<SYUserGetByUnitId> usersProvice = await new SYUserGetByUnitId(_appSetting).SYUserGetByUnitIdDAO(province.Value);
					foreach (var userProvice in usersProvice) {
						tinh.children.Add(new DropListTreeView(userProvice.FullName, userProvice.Id));
					}
					// list chilldren của chil
					DropListTreeView huyen = new DropListTreeView();
					foreach (var district in lstUnit.Where(x => x.ParentId == province.Value && x.UnitLevel == 2).ToList()) {
						huyen = new DropListTreeView(district.Text, district.Value, new List<DropListTreeView>());
						List<SYUserGetByUnitId> usersDistrict = await new SYUserGetByUnitId(_appSetting).SYUserGetByUnitIdDAO(district.Value);
						foreach (var userDistrict in usersDistrict)
						{
							huyen.children.Add(new DropListTreeView(userDistrict.FullName, userDistrict.Id));
						}
						// list children của child1
						DropListTreeView xa = new DropListTreeView();
						foreach (var commune in lstUnit.Where(x => x.ParentId == district.Value && x.UnitLevel == 3).ToList())
						{
							xa = new DropListTreeView(commune.Text,commune.Value, new List<DropListTreeView>());
							List<SYUserGetByUnitId> usersCommune = await new SYUserGetByUnitId(_appSetting).SYUserGetByUnitIdDAO(commune.Value);
							foreach (var userCommune in usersCommune)
							{
								xa.children.Add(new DropListTreeView(userCommune.FullName, userCommune.Id));
							}
							if (xa.children.Count > 0)
							{
								huyen.children.Add(xa);
							}
						}
						if (huyen.children.Count > 0) {
							tinh.children.Add(huyen);
						}
						
					}
					if (tinh.children.Count > 0) { result.Add(tinh); }
					
				}
				

				return new ResultApi { Success = ResultCode.OK, Result = result };
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
		public async Task<object> OrganizationRegister(
			[FromBody] BusinessRegisterModel model)
        {
			try
			{
				if (model.Password != model.RePassword)
				{
					return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Mật khẩu không khớp" };
				}
				//var hasOne = await new SYUserGetByUserName(_appSetting).SYUserGetByUserNameDAO(model.Phone);
				//if (hasOne != null && hasOne.Any()) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại tài khoản đăng nhập đã tồn tại" };


				var accCheckExist = await new SYUserCheckExists(_appSetting).SYUserCheckExistsDAO("Phone", model.Phone, 0);
				if (accCheckExist[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại đã tồn tại" };
				accCheckExist = await new SYUserCheckExists(_appSetting).SYUserCheckExistsDAO("Email", model.Email, 0);
				if (accCheckExist[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Email đã tồn tại" };

				

				DateTime birdDay, dateOfIssue;
				if (!DateTime.TryParseExact(model._RepresentativeBirthDay, "dd/MM/yyyy", null, DateTimeStyles.None, out birdDay))
				{
					//return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Định dạng ngày sinh không hợp lệ" };
				}
				if (!DateTime.TryParseExact(model._DateOfIssue, "dd/MM/yyyy", null, DateTimeStyles.None, out dateOfIssue))
				{
					//return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Định dạng ngày cấp không hợp lệ" };
				}

				///Phone,Email,IDCard
				///check ton tai
				var checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("Phone", model.Phone, 0);
				if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại đã tồn tại" };
				if (!string.IsNullOrEmpty(model.Email))
				{
					checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("Email", model.Email, 0);
					if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Email người đại diện đã tồn tại" };
				}
				checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("IDCard", model.IDCard, 0);
				if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số CMND / CCCD đã tồn tại" };
				checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("OrgPhone", model.OrgPhone, 0);
				if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại doanh nghiệp đã tồn tại" };

				checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("Tax", model.Tax, 0);
				if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Mã số thuế đã tồn tại" };

				if (!string.IsNullOrEmpty(model.OrgEmail))
				{
					checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("OrgEmail", model.OrgEmail, 0);
					if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Email doanh nghiệp đã tồn tại" };
				}
				checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("BusinessRegistration", model.BusinessRegistration, 0);
				if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số đăng ký kinh doanh đã tồn tại" };

                if(!string.IsNullOrEmpty(model.DecisionOfEstablishing)){
					checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("DecisionOfEstablishing", model.DecisionOfEstablishing, 0);
					if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số quyết định thành lập đã tồn tại" };
				}

				//if(model.ProvinceId == 0)
    //            {
				//	model.ProvinceId = null;
				//	model.DistrictId = null;
				//	model.WardsId = null;
    //            }
				


				///mod loginInfo
				///
				var pwd = GeneratePassword.generatePassword(model.Password);
				var account = new SYUserInsertIN
				{
					Password = pwd["Password"],
					Salt = pwd["Salt"],
					Phone = model.Phone,
					Email = model.Email,
					UserName = model.Phone,
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
				if (string.IsNullOrEmpty(model._DateOfIssue)) model.DateOfIssue = null;
				else model.DateOfIssue = dateOfIssue;
				if (string.IsNullOrEmpty(model._RepresentativeBirthDay)) model.RepresentativeBirthDay = null;
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
		public async Task<object> InvididualRegister_Body([FromBody] IndivialRegisterModel model)
		{


			try
			{
				// validate

				if (!Regex.Match(model.Phone.ToString(), ConstantRegex.PHONE, RegexOptions.IgnoreCase).Success)
				{
					return new ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại không hợp lệ" };
				}

				if (model.Email != null && !ConstantRegex.EmailIsValid(model.Email))
				{
					return new ResultApi { Success = ResultCode.ORROR, Message = "Email không hợp lệ" };
				}

				if (!Regex.Match(model.IDCard.ToString(), ConstantRegex.CMT, RegexOptions.IgnoreCase).Success)
				{
					return new ResultApi { Success = ResultCode.ORROR, Message = "Số CMT/Hộ chiếu không hợp lệ" };
				}

				if (model.Password != model.RePassword)
				{
					return new ResultApi { Success = ResultCode.ORROR, Message = "Mật khẩu không khớp" };
				}

				//if(model.)



				var accCheckExist = await new SYUserCheckExists(_appSetting).SYUserCheckExistsDAO("Phone", model.Phone, 0);
				if (accCheckExist[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại đã tồn tại" };
				accCheckExist = await new SYUserCheckExists(_appSetting).SYUserCheckExistsDAO("Email", model.Email, 0);
				if (accCheckExist[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Email đã tồn tại" };

				DateTime birdDay, dateOfIssue;
				if (!DateTime.TryParseExact(model._BirthDay, "dd/MM/yyyy", null, DateTimeStyles.None, out birdDay))
				{
					//return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Định dạng ngày sinh không hợp lệ" };
				}
				if (!DateTime.TryParseExact(model._DateOfIssue, "dd/MM/yyyy", null, DateTimeStyles.None, out dateOfIssue))
				{
					//return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Định dạng ngày cấp không hợp lệ" };
				}

				///Phone,Email,IDCard
				///check ton tai
				var checkExists = await new BIIndividualCheckExists(_appSetting).BIIndividualCheckExistsDAO("Phone", model.Phone, 0);
				if (checkExists[0].Exists.Value)
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

				//if (model.ProvinceId == 0)
				//{
				//	model.ProvinceId = null;
				//	model.DistrictId = null;
				//	model.WardsId = null;
				//}

				///mod loginInfo
				///
				var pwd = GeneratePassword.generatePassword(model.Password);
				var account = new SYUserInsertIN
				{
					Password = pwd["Password"],
					Salt = pwd["Salt"],
					Phone = model.Phone,
					Email = model.Email,
					UserName = model.Phone,
					FullName = model.FullName,
					Gender = model.Gender,
					Address = model.Address,

					TypeId = 2,
					Type = 2,
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
				if (string.IsNullOrEmpty(model._DateOfIssue)) model.DateOfIssue = null;
				else model.DateOfIssue = dateOfIssue;
				if (string.IsNullOrEmpty(model._BirthDay)) model.BirthDay = null;
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
				var rs2 = await new ModelBase.BIIndividualInsert(_appSetting).BIIndividualInsertDAO(model);

			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
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
						DateOfBirth = info[0].BirthDay,
						Email = info[0].Email,
						Phone = info[0].Phone,
						Nation = info[0].Nation,
						ProvinceId = info[0].ProvinceId,
						DistrictId = info[0].DistrictId,
						WardsId = info[0].WardsId,
						Address = info[0].Address,
						IdCard = info[0].IDCard,
						IssuedPlace = info[0].IssuedPlace,
						IssuedDate = info[0].DateOfIssue,
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
					//List<BI_BusinessGetById> rsBusinessGetById = await new BI_BusinessGetById(_appSetting).BusinessGetByIdDAO(accInfo[0].Id);
					var rsBusinessGetById = await new BIBusinessGetByUserId(_appSetting).BIBusinessGetByUserIdDAO(accInfo[0].Id);
					if (rsBusinessGetById == null || !rsBusinessGetById.Any())
					{
						return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Thông tin tài khoản không tồn tại" };
					}



					//BusinessAccountInfoModel model = new BusinessAccountInfoModel(info[0]);

					//model.UserName = accInfo[0].UserName;
					//model.FullName = accInfo[0].FullName;
					//if (!model.ProvinceId.HasValue)
					//{
					//	model.DistrictId = null;
					//}
     //               if (!model.OrgProvinceId.HasValue)
     //               {
					//	model.OrgDistrictId = null;
					//	model.OrgWardsId = null;
     //               }

					return new Models.Results.ResultApi { Success = ResultCode.OK, Result = rsBusinessGetById.FirstOrDefault() };
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

				var newPwd = GeneratePassword.generatePassword(model.NewPassword);
				var _model = new SYUserChangePwdIN
				{
					Password = newPwd["Password"],
					Salt = newPwd["Salt"]
				};
				_model.Id = accInfo[0].Id;
				var rs = await new SYUserChangePwd(_appSetting).SYUserChangePwdDAO(_model);

				// cho trạng thái = false hết
				await new SYUserUserAgent(_appSetting).SYUserUserAgentUpdateStatusDAO(accInfo[0].Id);

				SYUserUserAgent sy_UserAgent = new SYUserUserAgent(accInfo[0].Id, Request.Headers["User-Agent"].ToString(), Request.Headers["ipAddress"].ToString(), true);
				await new SYUserUserAgent(_appSetting).SYUserUserAgentInsertDAO(sy_UserAgent);

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
				

				var newPwd = GeneratePassword.generatePassword(model.NewPassword);
				var _model = new SYUserChangePwdIN
				{
					Password = newPwd["Password"],
					Salt = newPwd["Salt"]
				};
				_model.Id = accInfo[0].Id;

				var rs = await new SYUserChangePwd(_appSetting).SYUserChangePwdDAO(_model);

				// cho trạng thái = false hết
				await new SYUserUserAgent(_appSetting).SYUserUserAgentUpdateStatusDAO(accInfo[0].Id);

				//SYUserUserAgent sy_UserAgent = new SYUserUserAgent(accInfo[0].Id, Request.Headers["User-Agent"].ToString(), Request.Headers["ipAddress"].ToString(), true);
				//await new SYUserUserAgent(_appSetting).SYUserUserAgentInsertDAO(sy_UserAgent);

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

				//DateTime? birdDay =null, dateOfIssue = null;
				//if (DateTime.TryParseExact(model.DateOfBirth, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime _birdDay))
				//{
				//	birdDay = _birdDay;
				//}
				//if (DateTime.TryParseExact(model.IssuedDate, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime _dateOfIssue))
				//{
				//	dateOfIssue = _dateOfIssue;
				//}

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


					//if(model.ProvinceId == 0)
     //               {
					//	model.ProvinceId = null;
					//	model.DistrictId = null;
					//	model.WardsId = null;
     //               }

					var _model = new BIInvididualUpdateInfoIN
					{
						Id = info[0].Id,
						FullName = model.FullName,
						DateOfBirth = model.DateOfBirth,
						Email = model.Email,
						Nation = model.Nation,
						ProvinceId = model.ProvinceId,
						DistrictId = model.DistrictId,
						WardsId = model.WardsId,
						Address = model.Address,
						IdCard = model.IdCard,
						IssuedPlace = model.IssuedPlace,
						IssuedDate = model.IssuedDate,
						Gender = model.Gender,
					};

					var rs = await new BIInvididualUpdateInfo(_appSetting).BIInvididualUpdateInfoDAO(_model);

					var rsUpdateAcc = new SYUserUpdateInfo(_appSetting).SYUserUpdateInfoDAO(new SYUserUpdateInfoIN
					{
						Id = accInfo[0].Id,
						FullName = accInfo[0].FullName,
						Address = accInfo[0].Address,
					});
				}
				else if (accInfo[0].TypeId == 3)
                {
					///check ton tai
					///
					var info = await new BIBusinessGetByUserId(_appSetting).BIBusinessGetByUserIdDAO(accInfo[0].Id);
					var checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("Phone", businessModel.Phone, info[0].Id);
					if (checkExists[0].Exists.Value && businessModel.Phone != null) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại đã tồn tại" };
					if (!string.IsNullOrEmpty(model.Email))
					{
						checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("Email", businessModel.Email, info[0].Id);
						if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Email người đại diện đã tồn tại" };
					}
					checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("IDCard", businessModel.IDCard, info[0].Id);
					if (checkExists[0].Exists.Value && businessModel.IDCard != null) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số CMND / CCCD đã tồn tại" };
					checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("OrgPhone", businessModel.OrgPhone, info[0].Id);
					if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại doanh nghiệp đã tồn tại" };
					if (!string.IsNullOrEmpty(businessModel.OrgEmail))
					{
						checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("OrgEmail", businessModel.OrgEmail, info[0].Id);
						if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Email doanh nghiệp đã tồn tại" };
					}
					checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("BusinessRegistration", businessModel.BusinessRegistration, info[0].Id);
					if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số đăng ký kinh doanh đã tồn tại" };

					checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("Tax", businessModel.Tax, info[0].Id);
					if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Mã số thuế đã tồn tại" };

					checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("DecisionOfEstablishing", businessModel.DecisionOfEstablishing, info[0].Id);
					if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số quyết định thành lập đã tồn tại" };


					accInfo[0].FullName = businessModel.RepresentativeName;
					//if (DateTime.TryParseExact(businessModel.RepresentativeBirthDay, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime outTime0))
					//{
					//	birdDay = outTime0;
					//}
					//if (DateTime.TryParseExact(businessModel.DateOfIssue, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime outTime1))
					//{
					//	dateOfIssue = outTime1;
					//}

					//if (model.ProvinceId == 0)
					//{
					//	model.ProvinceId = null;
					//	model.DistrictId = null;
					//	model.WardsId = null;
					//}


					var _model = new BIBusinessUpdateInfoIN
					{
						Id = info[0].Id,
						FullName = businessModel.RepresentativeName,
						DateOfBirth = businessModel.RepresentativeBirthDay,
						Email = model.Email,
						Nation = model.Nation,
						ProvinceId = model.ProvinceId,
						DistrictId = model.DistrictId,
						WardsId = model.WardsId,
						Address = model.Address,
						IdCard = model.IdCard,
						IssuedDate = businessModel.DateOfIssue,
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
					//if (model.IssuedDate != null) {
					//	_model.IssuedDate = DateTime.ParseExact(model.IssuedDate, "dd/MM/yyyy",
					//				   System.Globalization.CultureInfo.InvariantCulture);
					//}
					var rs = await new BIBusinessUpdateInfo(_appSetting).BIBusinessUpdateInfoDAO(_model);
					//update account

					var rsUpdateAcc = new SYUserUpdateInfo(_appSetting).SYUserUpdateInfoDAO(new SYUserUpdateInfoIN
					{
						Id = accInfo[0].Id,
						FullName = businessModel.Business,
						Address = accInfo[0].Address,
					});
				}

				
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


    }
}
