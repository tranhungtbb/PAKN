﻿using Bugsnag;
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
					return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Định dạng ngày sinh không hợp lệ" };
				}
				if (!DateTime.TryParseExact(_DateOfIssue, "dd/MM/yyyy", null, DateTimeStyles.None, out dateOfIssue))
				{
					return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Định dạng ngày cấp không hợp lệ" };
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

				DateTime birdDay, dateOfIssue;
				if (!DateTime.TryParseExact(_BirthDay, "dd/MM/yyyy", null, DateTimeStyles.None, out birdDay))
				{
					return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Định dạng ngày sinh không hợp lệ" };
				}
				if (!DateTime.TryParseExact(_DateOfIssue, "dd/MM/yyyy", null, DateTimeStyles.None, out dateOfIssue))
				{
					return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Định dạng ngày cấp không hợp lệ" };
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

				return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = ex.Message};
			}

			return new Models.Results.ResultApi { Success = ResultCode.OK };
		}


		[HttpGet]
		[Route("UserGetInfo")]
		[Authorize]
		public async Task<object> UserGetInfo(long id)
        {
			try
            {
				var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
				var accInfo = await new SYUserGetByID(_appSetting).SYUserGetByIDDAO(id);
				
				if(accInfo == null || !accInfo.Any())
                {
					return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Tài khoản không tồn tại" };
				}

                if (accInfo[0].TypeId == 1)
                {

                }
                else if (accInfo[0].TypeId == 2)
                {
					//var info = await new 
                }
				else if (accInfo[0].TypeId == 3)
                {

                }

			}
			catch(Exception ex)
            {
				_bugsnag.Notify(ex);
				return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}

			return null;
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
