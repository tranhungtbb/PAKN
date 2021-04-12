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

namespace PAKNAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : BaseApiController
	{
		private readonly IFileService _fileService;
		private readonly IAppSetting _appSetting;
		private readonly IClient _bugsnag;
		public UserController(IFileService fileService, IAppSetting appSetting, IClient bugsnag)
		{
			_fileService = fileService;
			_appSetting = appSetting;
			_bugsnag = bugsnag;
		}

		[HttpPost, DisableRequestSizeLimit]
		[Route("Create")]
		[Authorize]
		public async Task<object> Create([FromForm] SYUserInsertIN model, [FromForm] IFormFileCollection files = null)
		{
			// tải file
			string filePath = "";
			//try
			//{
			//	if (files != null && files.Any())
			//	{
			//		var info = await _fileService.Save(files, "User");
			//		filePath = info[0].Path;
			//	}
			//}
			//catch (Exception e)
			//{
			//	new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Xảy ra lỗi trong quá trình tải file" };
			//}

			try
			{
				if (files != null && files.Any())
				{
					var info = await _fileService.Save(files, "User");
					filePath = info[0].Path;
				}

				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
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
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);
				//xóa file đã tải
				if(!string.IsNullOrEmpty(filePath))
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

				// xóa avatar cũ
				//if (!string.IsNullOrEmpty(modelOld[0].Avatar) && !string.IsNullOrEmpty(filePath))
				//{
				//	await _fileService.Remove(modelOld[0].Avatar);
				//}

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
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);
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



		#region dang ky nguoi dan, doanh nghiep
		[HttpPost]
		[Route("OrganizationRegister")]
		public async Task<object> OrganizationRegister([FromForm] RegisterModel loginInfo, [FromForm] QLDoanhNghiepInsertIN model)
        {
			try
			{
				if (loginInfo.Password != loginInfo.RePassword)
				{
					return null;
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
					UserName = model.Email,
					FullName = model.RepresentativeName,
					Gender = model.Gender,
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
				model.CreatedDate = DateTime.Now;
				model.CreatedBy = 0;
				model.UpdatedBy = 0;
				model.UpdatedDate = DateTime.Now;
				model.Status = 1;

				var rs2 = await new QLDoanhNghiepInsert(_appSetting).QLDoanhNghiepInsertDAO(model);

			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);

				return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}

			return new Models.Results.ResultApi { Success = ResultCode.OK };
		}

		[HttpPost]
		[Route("InvididualRegister")]
		public async Task<object> InvididualRegister([FromForm] RegisterModel loginInfo, [FromForm] QLNguoiDanInsertIN model)
		{


            try
            {
				if (loginInfo.Password != loginInfo.RePassword)
				{
					return null;
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
					UserName = model.Email,
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
				model.CreatedDate = DateTime.Now;
				model.CreatedBy = 0;
				model.UpdatedBy = 0;
				model.UpdatedDate = DateTime.Now;
				model.Status = 1;

				var rs2 = await new QLNguoiDanInsert(_appSetting).QLNguoiDanInsertDAO(model);

			}
			catch (Exception ex)
            {
				_bugsnag.Notify(ex);

				return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = ex.Message};
			}

			return new Models.Results.ResultApi { Success = ResultCode.OK };
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
