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
using static PAKNAPI.Models.Login.BusinessRegisterModel;
using System.Text;
using PAKNAPI.Job;
using Newtonsoft.Json;
using System.Threading;

namespace PAKNAPI.Controllers
{
	[Route("api/user")]
	[ApiController]
	[ValidateModel]
	public class UserController : BaseApiController
	{
		private readonly IFileService _fileService;
		private readonly IAppSetting _appSetting;
		private readonly IClient _bugsnag;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IWebHostEnvironment _hostingEnvironment;
		private Microsoft.Extensions.Configuration.IConfiguration _config;

		private static string textRamdom = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
		private static string numberRamdom = "1234567890";
		private static string specialCharacters = "!@#$%^&*";
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
		[Authorize("ThePolicy")]
		[Route("get-data-for-create")]
		public async Task<ActionResult<object>> UsersGetDataForCreate()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYUserGetAllOnPageList(_appSetting).UsersGetDataForCreate() };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// thêm mới người dùng
		/// </summary>
		/// <param name="model"></param>
		/// <param name="files"></param>
		/// <returns></returns>
		[HttpPost, DisableRequestSizeLimit]
		[Route("create")]
		[Authorize("ThePolicy")]
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

				int length = 3;
				StringBuilder res = new StringBuilder();
				Random rnd = new Random();
				while (0 < length--)
				{
					res.Append(textRamdom[rnd.Next(textRamdom.Length)]);
				}
				length = 3;
				while (0 < length--)
				{
					res.Append(numberRamdom[rnd.Next(numberRamdom.Length)]);
				}
				length = 2;
				while (0 < length--)
				{
					res.Append(specialCharacters[rnd.Next(specialCharacters.Length)]);
				}

				// lưu db

				var config = (await new SYConfig(_appSetting).SYConfigGetByTypeDAO(TYPECONFIG.CONFIG_EMAIL));
				if (config.Count == 0)
				{
					return new ResultApi { Success = ResultCode.ORROR, Result = -1, Message = "Config email not exits" };
				}
				else
				{
					// gửi mail
					var configEmail = JsonConvert.DeserializeObject<ConfigEmail>(config[0].Content);
					string content =
						"<p><span style = 'font-family:times new roman,times,serif'><span style='font-size:16px'>Dear {FullName},</span></span></p>" +
						"<p style ='font-family:times new roman,times,serif'> Đây là mật khẩu mới. Vui lòng đăng nhập lại với tài khoản của bạn</p>" +
						"<br/><strong>Mật khẩu mới</strong><span style = 'font-size:14px'>: {NewPassword}</span>";
					content = content.Replace("{FullName}", model.FullName);
					content = content.Replace("{NewPassword}", res.ToString());
					Thread t = new Thread(async () => {
						MailHelper.SendMail(configEmail, model.Email, "Mật khẩu của bạn", content, null);
					});
					t.Start();

					// lưu db
					
					byte[] salt = new byte[128 / 8];
					using (var rng = RandomNumberGenerator.Create())
					{
						rng.GetBytes(salt);
					}
					// derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
					string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
						password: res.ToString(),
						salt: salt,
						prf: KeyDerivationPrf.HMACSHA1,
						iterationCount: 10000,
						numBytesRequested: 256 / 8));

					// thêm người dùng vào db
					model.Avatar = filePath;
					model.Password = hashed;
					model.Salt = Convert.ToBase64String(salt);
					model.IsSuperAdmin = false;
					model.IsAdmin = false;
					model.UserName = model.Email;

					var result = await new SYUserInsert(_appSetting).SYUserInsertDAO(model);
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
					return new ResultApi { Success = ResultCode.OK, Result = result };

					//generate mật khẩu
				}
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);
				//xóa file đã tải
				if (!string.IsNullOrEmpty(filePath))
					await _fileService.Remove(filePath);
				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}



		/// <summary>
		/// cập nhập người dùng
		/// </summary>
		/// <param name="model"></param>
		/// <param name="files"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("update")]
		[Authorize("ThePolicy")]
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
					if (!string.IsNullOrEmpty(modelOld[0].Avatar))
						await _fileService.Remove(modelOld[0].Avatar);
					if (!string.IsNullOrEmpty(filePath))
					{
						model.Avatar = filePath;
					}
				}


				model.Password = modelOld[0].Password;
				model.Salt = modelOld[0].Salt;
				model.IsSuperAdmin = false;
				model.IsAdmin = false;
				model.UserName = model.Email;
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
				var result = await new SYUserUpdate(_appSetting).SYUserUpdateDAO(model);

			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);
				//xóa file đã tải
				await _fileService.Remove(filePath);
				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
			new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
			return new ResultApi { Success = ResultCode.OK };
		}
		/// <summary>
		/// cập nhập profile
		/// </summary>
		/// <param name="model"></param>
		/// <param name="files"></param>
		/// <returns></returns>
		[HttpPost, DisableRequestSizeLimit]
		[Route("update-profile")]
		[Authorize("ThePolicy")]
		public async Task<object> Update([FromForm] SYUserUpdateProfile model, [FromForm] IFormFileCollection files = null)
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

				model.UserName = model.Email;
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
				var result = await new SYUserUpdate(_appSetting).SYUserUpdateProfileDAO(model);

			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);
				//xóa file đã tải
				await _fileService.Remove(filePath);
				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
			new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
			return new ResultApi { Success = ResultCode.OK };
		}




		/// <summary>
		/// thêm mới nv quản trị
		/// </summary>
		/// <param name="model"></param>
		/// <param name="files"></param>
		/// <returns></returns>
		[HttpPost, DisableRequestSizeLimit]
		[Route("user-system-create")]
		[Authorize("ThePolicy")]
		public async Task<object> UserSystemCreate([FromForm] SYUserSystemInsertIN model, [FromForm] IFormFileCollection files = null)
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

				int length = 3;
				StringBuilder res = new StringBuilder();
				Random rnd = new Random();
				while (0 < length--)
				{
					res.Append(textRamdom[rnd.Next(textRamdom.Length)]);
				}
				length = 3;
				while (0 < length--)
				{
					res.Append(numberRamdom[rnd.Next(numberRamdom.Length)]);
				}
				length = 2;
				while (0 < length--)
				{
					res.Append(specialCharacters[rnd.Next(specialCharacters.Length)]);
				}

				// lưu db

				var config = (await new SYConfig(_appSetting).SYConfigGetByTypeDAO(TYPECONFIG.CONFIG_EMAIL));
				if (config.Count == 0)
				{
					return new ResultApi { Success = ResultCode.ORROR, Result = -1, Message = "Config email not exits" };
				}
				else
				{
					// gửi mail
					var configEmail = JsonConvert.DeserializeObject<ConfigEmail>(config[0].Content);
					string content =
						"<p><span style = 'font-family:times new roman,times,serif'><span style='font-size:16px'>Dear {FullName},</span></span></p>" +
						"<p style ='font-family:times new roman,times,serif'> Đây là mật khẩu mới. Vui lòng đăng nhập lại với tài khoản của bạn</p>" +
						"<br/><strong>Mật khẩu mới</strong><span style = 'font-size:14px'>: {NewPassword}</span>";
					content = content.Replace("{FullName}", model.FullName);
					content = content.Replace("{NewPassword}", res.ToString());

					//generate mật khẩu
					byte[] salt = new byte[128 / 8];
					using (var rng = RandomNumberGenerator.Create())
					{
						rng.GetBytes(salt);
					}
					// derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
					string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
						password: res.ToString(),
						salt: salt,
						prf: KeyDerivationPrf.HMACSHA1,
						iterationCount: 10000,
						numBytesRequested: 256 / 8));

					// thêm người dùng vào db
					model.Avatar = filePath;
					model.Password = hashed;
					model.Salt = Convert.ToBase64String(salt);
					model.IsSuperAdmin = false;
					model.IsAdmin = true;
					model.TypeId = 1; // quản trị
					model.UserName = model.Email;

					var unitMainId = (await new SYUnitGetMainId(_appSetting).SYUnitGetMainIdDAO()).FirstOrDefault();
					if (unitMainId == null)
					{
						new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
						return new ResultApi { Success = ResultCode.ORROR, Message = "Không tồn tại đơn vị trung tâm" };
					}
					model.UnitId = unitMainId.Id;

					var result = await new SYUserInsert(_appSetting).SYUserSystemInsertDAO(model);
					Thread t = new Thread(async () => { 
						MailHelper.SendMail(configEmail, model.Email, "Mật khẩu của bạn", content, null); 
					});
					t.Start();
					
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
					return new ResultApi { Success = ResultCode.OK, Result = result };
				}
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);
				//xóa file đã tải
				if (!string.IsNullOrEmpty(filePath))
					await _fileService.Remove(filePath);
				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		/// <summary>
		/// cập nhập nv quản trị
		/// </summary>
		/// <param name="model"></param>
		/// <param name="files"></param>
		/// <returns></returns>

		[HttpPost, DisableRequestSizeLimit]
		[Route("user-system-update")]
		[Authorize("ThePolicy")]
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
				model.UserName = model.Email;
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
				var result = await new SYUserUpdate(_appSetting).SYUserSystemUpdateDAO(model);

			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);
				//xóa file đã tải
				await _fileService.Remove(filePath);
				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
			new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
			return new ResultApi { Success = ResultCode.OK };
		}
		/// <summary>
		/// xóa người dùng
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>

		[HttpPost, DisableRequestSizeLimit]
		[Route("delete")]
		[Authorize("ThePolicy")]
		public async Task<object> Delete(SYUserDeleteIN model)
		{
			try
			{
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
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
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
					return new ResultApi { Success = ResultCode.OK, Result = result };
				}
				else {
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
					return new ResultApi { Success = ResultCode.ORROR, Result = result };
				}


			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);
				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
			new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
			return new ResultApi { Success = ResultCode.OK };
		}

		//[HttpGet]
		//[Route("GetAvatar/{id}")]
		//[Authorize("ThePolicy")]
		//public async Task<byte[]> GetAvatar(int id)
		//{
		//	var modelOld = await new SYUserGetByID(_appSetting).SYUserGetByIDDAO(id);
		//	if (string.IsNullOrEmpty(modelOld[0].Avatar?.Trim())) return null;
		//	var data = await _fileService.GetBinary(modelOld[0].Avatar);

		//	return data;
		//}

		/// <summary>
		/// danh sách người dùng
		/// </summary>
		/// <param name="PageSize"></param>
		/// <param name="PageIndex"></param>
		/// <param name="UserName"></param>
		/// <param name="FullName"></param>
		/// <param name="Phone"></param>
		/// <param name="IsActived"></param>
		/// <param name="UnitId"></param>
		/// <param name="TypeId"></param>
		/// <param name="PositionId"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-list-user-on-page")]
		public async Task<ActionResult<object>> SYUserGetAllOnPageList()
		{
			try
			{
				List<SYUserGetAllOnPageList> rsSYUserGetAllOnPage = await new SYUserGetAllOnPageList(_appSetting).SYUserGetAllOnPageDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYUserGetAllOnPage", rsSYUserGetAllOnPage},
						{"TotalCount", rsSYUserGetAllOnPage != null && rsSYUserGetAllOnPage.Count > 0 ? rsSYUserGetAllOnPage[0].RowNumber : 0}
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// list user chat
		/// </summary>
		/// <param name="PageIndex"></param>
		/// <param name="UserName"></param>
		/// <param name="TextSearch"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-list-user-for-chat")]
		public async Task<ActionResult<object>> SYUserGetAllOnPageListForChat(int? PageIndex, string UserName, string TextSearch)
		{
			try
			{
				List<SYUserGetAllOnPageListForChat> rsSYUserGetAllOnPage = await new SYUserGetAllOnPageListForChat(_appSetting).SYUserGetAllOnPageListForChatDAO(PageIndex, UserName, TextSearch);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"users", rsSYUserGetAllOnPage},
						{"total", rsSYUserGetAllOnPage != null && rsSYUserGetAllOnPage.Count > 0 ? rsSYUserGetAllOnPage[0].RowNumber : 0},
						{"page", rsSYUserGetAllOnPage != null && rsSYUserGetAllOnPage.Count > 0 ? PageIndex : 0},
						{"perPage", 30},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		/// <summary>
		/// danh sách người dùng theo lst idQB
		/// </summary>
		/// <param name="lstId"></param>
		/// <returns></returns>
		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-list-user-by-lst-id-qb")]
		public async Task<ActionResult<object>> SYUserGetAllOnPageListByListIdQb(string lstId, string textSearch)
		{
			try
			{
				if (lstId == null || lstId.Trim() == "")
				{
					IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"users", new List<SYUserGetAllOnPageListForChat>()}
					};
					return new ResultApi { Success = ResultCode.OK, Result = json };
				}
				else {
					List<SYUserGetAllOnPageListForChat> rsSYUserGetAllOnPage = await new SYUserGetAllOnPageListForChat(_appSetting).SYUserGetAllOnPageListByListIdQb(lstId, textSearch);
					IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"users", rsSYUserGetAllOnPage}
					};
					return new ResultApi { Success = ResultCode.OK, Result = json };
				}
				
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("update-qb")]
		public async Task<ActionResult<object>> SYUserUpdateQB(int? Id, int? IdQB)
		{
			try
			{
				await new SYUserGetAllOnPageListForChat(_appSetting).SYUserUpdateIdQBDAO(Id, IdQB);
				return new ResultApi { Success = ResultCode.OK};
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		/// <summary>
		/// danh sách quản trị
		/// </summary>
		/// <param name="PageSize"></param>
		/// <param name="PageIndex"></param>
		/// <param name="UserName"></param>
		/// <param name="FullName"></param>
		/// <param name="Phone"></param>
		/// <param name="IsActived"></param>
		/// <param name="UnitId"></param>
		/// <param name="TypeId"></param>
		/// <param name="SortDir"></param>
		/// <param name="SortField"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-list-user-on-page-by-unit-id")]
		public async Task<ActionResult<object>> SYUserGetAllOnPageBase(int? UnitId)
		{
			try
			{
				List<SYUserGetAllOnPage> rsSYUserGetAllOnPage = await new SYUserGetAllOnPage(_appSetting).SYUserGetAllOnPageDAO(UnitId);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYUserGetAllOnPage", rsSYUserGetAllOnPage},
						{"TotalCount", rsSYUserGetAllOnPage != null && rsSYUserGetAllOnPage.Count > 0 ? rsSYUserGetAllOnPage[0].RowNumber : 0}
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		/// <summary>
		/// cập nhập trạng thái người dùng
		/// </summary>
		/// <param name="_sYUserChangeStatusIN"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("change-status")]
		public async Task<ActionResult<object>> SYUserChangeStatusBase(SYUserChangeStatusIN _sYUserChangeStatusIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYUserChangeStatus(_appSetting).SYUserChangeStatusDAO(_sYUserChangeStatusIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// check tồn tại người dùng
		/// </summary>
		/// <param name="Field"></param>
		/// <param name="Value"></param>
		/// <param name="Id"></param>
		/// <returns></returns>
		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("check-exists")]
		public async Task<ActionResult<object>> SYUserCheckExistsBase(string Field, string Value, long? Id)
		{
			try
			{
				List<SYUserCheckExists> rsSYUserCheckExists = await new SYUserCheckExists(_appSetting).SYUserCheckExistsDAO(Field, Value, Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYUserCheckExists", rsSYUserCheckExists},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// danh sách người dùng theo vai trò - all
		/// </summary>
		/// <param name="RoleId"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-list-user-on-page-base-by-role-id")]
		public async Task<ActionResult<object>> SYUserGetAllByRoleIdBase(int? RoleId)
		{
			try
			{
				List<SYUserGetAllByRoleId> rsSYUserGetAllByRoleId = await new SYUserGetAllByRoleId(_appSetting).SYUserGetAllByRoleIdDAO(RoleId);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYUserGetAllByRoleId", rsSYUserGetAllByRoleId},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// :D
		/// </summary>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-drop-down")]
		public async Task<ActionResult<object>> SYUsersGetDropdownBase()
		{
			try
			{
				List<SYUsersGetDropdown> rsSYUsersGetDropdown = await new SYUsersGetDropdown(_appSetting).SYUsersGetDropdownDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYUsersGetDropdown", rsSYUsersGetDropdown},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// danh sách người dùng theo vai trò
		/// </summary>
		/// <param name="PageSize"></param>
		/// <param name="PageIndex"></param>
		/// <param name="RoleId"></param>
		/// <returns></returns>
		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-list-user-on-page-by-role-id")]
		public async Task<ActionResult<object>> SYUserGetByRoleIdAllOnPageBase(int? PageSize, int? PageIndex, int? RoleId)
		{
			try
			{
				List<SYUserGetByRoleIdAllOnPage> rsSYUserGetByRoleIdAllOnPage = await new SYUserGetByRoleIdAllOnPage(_appSetting).SYUserGetByRoleIdAllOnPageDAO(PageSize, PageIndex, RoleId);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYUserGetByRoleIdAllOnPage", rsSYUserGetByRoleIdAllOnPage},
						{"TotalCount", rsSYUserGetByRoleIdAllOnPage != null && rsSYUserGetByRoleIdAllOnPage.Count > 0 ? rsSYUserGetByRoleIdAllOnPage[0].RowNumber : 0},
						{"PageIndex", rsSYUserGetByRoleIdAllOnPage != null && rsSYUserGetByRoleIdAllOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsSYUserGetByRoleIdAllOnPage != null && rsSYUserGetByRoleIdAllOnPage.Count > 0 ? PageSize : 0},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// chi tiết người dùng
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-by-id")]
		public async Task<ActionResult<object>> SYUserGetByIDBase(long? Id)
		{
			try
			{
				List<SYUserGetByID> rsSYUserGetByID = await new SYUserGetByID(_appSetting).SYUserGetByIDDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYUserGetByID", rsSYUserGetByID},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// danh sách người dùng( quản trị)
		/// </summary>
		/// <param name="PageSize"></param>
		/// <param name="PageIndex"></param>
		/// <param name="UserName"></param>
		/// <param name="FullName"></param>
		/// <param name="Phone"></param>
		/// <param name="IsActived"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-list-user-system-on-page")]
		public async Task<ActionResult<object>> SYUserSystemGetAllOnPageList()
		{
			try
			{
				List<SYUserSystemGetAllOnPageList> rsSYUserGetAllOnPage = await new SYUserSystemGetAllOnPageList(_appSetting).SYUserSystemGetAllOnPageDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYUserGetAllOnPage", rsSYUserGetAllOnPage},
						{"TotalCount", rsSYUserGetAllOnPage != null && rsSYUserGetAllOnPage.Count > 0 ? rsSYUserGetAllOnPage[0].RowNumber : 0}
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// thông tin người dùng
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-info")]
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// người dùng theo đơn vị
		/// </summary>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-user-order-by-unit")]
		public async Task<ActionResult<object>> SYUserGetAllOrderByUnit()
		{
			try
			{
				List<DropListTreeView> result = new List<DropListTreeView>();
				// lst đơn vị
				List<SYUnitGetDropdown> lstUnit = await new SYUnitGetDropdown(_appSetting).SYUnitGetDropdownDAO();
				// lst người dùng theo đơn vị từng đơn vị
				DropListTreeView tinh = new DropListTreeView();
				foreach (var province in lstUnit.Where(x => x.UnitLevel == 1).ToList()) {
					tinh = new DropListTreeView(province.Text, province.Value, new List<DropListTreeView>());
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
							xa = new DropListTreeView(commune.Text, commune.Value, new List<DropListTreeView>());
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// người dùng thuộc hệ thống
		/// </summary>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-user-is-system")]
		public async Task<ActionResult<object>> SYUserGetIsSystemBase()
		{
			try
			{
				List<SYUserGetIsSystem2> rsSYUserGetIsSystem2 = await new SYUserGetIsSystem2(_appSetting).SYUserGetIsSystem2DAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYUserGetIsSystem", rsSYUserGetIsSystem2},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		/// <summary>
		/// xóa người dùng theo vai trò
		/// </summary>
		/// <param name="_sYUserRoleMapDeleteIN"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("user-role-map-delete")]
		public async Task<ActionResult<object>> SYUserRoleMapDeleteBase(SYUserRoleMapDeleteIN _sYUserRoleMapDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYUserRoleMapDelete(_appSetting).SYUserRoleMapDeleteDAO(_sYUserRoleMapDeleteIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		/// <summary>
		/// danh sách người dùng ko thuộc vai trò
		/// </summary>
		/// <param name="RoleId"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("user-not-role")]
		public async Task<ActionResult<object>> SYUserGetIsNotRole(int? RoleId)
		{
			try
			{
				List<SYUserGetIsSystem> rsSYUserGetIsSystem = await new SYUserGetIsSystem(_appSetting).SYUserGetIsNotRole(RoleId);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYUserGetIsNotRole", rsSYUserGetIsSystem},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}


		/// <summary>
		/// đăng kí doanh nghiệp
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>

		#region nguoi dan, doanh nghiep
		[HttpPost]
		[Route("organization-register")]
		public async Task<object> OrganizationRegister([FromBody] BusinessRegisterModel model)
		{
			try
			{
				if (model.Password != model.RePassword)
				{
					return new ResultApi { Success = ResultCode.ORROR, Message = "Mật khẩu không khớp" };
				}
				
				//var accCheckExist = await new SYUserCheckExists(_appSetting).SYUserCheckExistsDAO("Phone", model.Phone, 0);
				//if (accCheckExist[0].Exists.Value) {
				//	return new ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại đã tồn tại" };
				//}
				//accCheckExist = await new SYUserCheckExists(_appSetting).SYUserCheckExistsDAO("Email", model.Email, 0);
				//if (accCheckExist[0].Exists.Value) {
				//	return new ResultApi { Success = ResultCode.ORROR, Message = "Email đã tồn tại" };
				//}

				//check ton tai
				var checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("Phone", model.Phone, 0);
				if (checkExists[0].Exists.Value) {
					return new ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại đã tồn tại" };
				}
				if (!string.IsNullOrEmpty(model.Email))
				{
					checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("Email", model.Email, 0);
					if (checkExists[0].Exists.Value) {
						return new ResultApi { Success = ResultCode.ORROR, Message = "Email người đại diện đã tồn tại" };
					}
				}
				if (!string.IsNullOrEmpty(model.OrgEmail))
				{
					checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("OrgEmail", model.OrgEmail, 0);
					if (checkExists[0].Exists.Value) {
						return new ResultApi { Success = ResultCode.ORROR, Message = "Email doanh nghiệp đã tồn tại" };
					}
				}
				checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("BusinessRegistration", model.BusinessRegistration, 0);
				if (checkExists[0].Exists.Value) {
					return new ResultApi { Success = ResultCode.ORROR, Message = "Số đăng ký kinh doanh đã tồn tại" };
				}

				//mod loginInfo
				//
				var pwd = GeneratePassword.generatePassword(model.Password);
				var account = new SYUserInsertIN
				{
					Password = pwd["Password"],
					Salt = pwd["Salt"],
					Phone = model.Phone,
					Email = model.Email,
					UserName = model.BusinessRegistration,
					FullName = model.Business,
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
				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}

			return new ResultApi { Success = ResultCode.OK };
		}

		/// <summary>
		/// đăng kí người dân
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("individual-register")]
		public async Task<object> InvididualRegister_Body([FromBody] IndivialRegisterModel model)
		{
			try
			{
				if (model.Password != model.RePassword) {
					return new ResultApi { Success = ResultCode.ORROR, Message = "Mật khẩu không khớp" };
				}

				if (model.BirthDay >= DateTime.Now) {
					return new ResultApi { Success = ResultCode.ORROR, Message = "Ngày sinh không được lớn hơn hoặc bằng ngày hiện tại" };
				}

				if (model.DateOfIssue != null && model.DateOfIssue < model.BirthDay) {
					return new ResultApi { Success = ResultCode.ORROR, Message = "Ngày sinh không được lớn hơn ngày thành lập" };
				}

				var accCheckExist = await new SYUserCheckExists(_appSetting).SYUserCheckExistsDAO("Phone", model.Phone, 0);
				if (accCheckExist[0].Exists.Value) return new ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại đã tồn tại" };
				//accCheckExist = await new SYUserCheckExists(_appSetting).SYUserCheckExistsDAO("Email", model.Email, 0);
				//if (accCheckExist[0].Exists.Value) return new ResultApi { Success = ResultCode.ORROR, Message = "Email đã tồn tại" };

				///Phone,Email,IDCard
				///check ton tai
				var checkExists = await new BIIndividualCheckExists(_appSetting).BIIndividualCheckExistsDAO("Phone", model.Phone, 0);
				if (checkExists[0].Exists.Value)
					return new ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại đã tồn tại" };
				if (!string.IsNullOrEmpty(model.Email))
				{
					checkExists = await new BIIndividualCheckExists(_appSetting).BIIndividualCheckExistsDAO("Email", model.Email, 0);
					if (checkExists[0].Exists.Value)
						return new ResultApi { Success = ResultCode.ORROR, Message = "Email đã tồn tại" };
				}
				checkExists = await new BIIndividualCheckExists(_appSetting).BIIndividualCheckExistsDAO("IDCard", model.IDCard, 0);
				if (checkExists[0].Exists.Value)
					return new ResultApi { Success = ResultCode.ORROR, Message = "Số CMND / CCCD đã tồn tại" };


				//mod loginInfo
				///
				var pwd = GeneratePassword.generatePassword(model.Password);
				var account = new SYUserInsertIN
				{
					Password = pwd["Password"],
					Salt = pwd["Salt"],
					Phone = model.Phone,
					Email = model.Email,
					UserName = model.IDCard,
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
				model.CreatedDate = DateTime.Now;
				model.CreatedBy = 0;
				model.UpdatedBy = 0;
				model.Status = 1;
				model.IsDeleted = false;
				model.UserId = accRs[0].Id;

				var rs2 = await new ModelBase.BIIndividualInsert(_appSetting).BIIndividualInsertDAO(model);

			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}

			return new ResultApi { Success = ResultCode.OK, Message = ResultMessage.OK };
		}


		//[HttpGet]
		//[Route("SendDemo")]
		//public string SendDemo()
		//      {
		//          try
		//	{
		//		string contentSMSOPT = "(Trung tam tiep nhan PAKN tinh Khanh Hoa) Xin chao: Trần Thanh Quyền. Mat khau dang nhap he thong la: abcAbc123123. Xin cam on!";
		//		SV.MailSMS.Model.SMTPSettings settings = new SV.MailSMS.Model.SMTPSettings();
		//		settings.COM = _config["SmsCOM"].ToString();
		//		SV.MailSMS.Control.SMSs sMSs = new SV.MailSMS.Control.SMSs(settings);
		//		string MessageResult = sMSs.SendOTPSMS("0984881580", contentSMSOPT);
		//		return "ok";
		//	}
		//          catch (Exception ex)
		//	{
		//		return ex.Message;
		//	}
		//}

		/// <summary>
		/// thông tin người dùng
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>

		[HttpGet]
		[Route("user-get-info")]
		[Authorize("ThePolicy")]
		public async Task<object> UserGetInfo(long? id)
		{
			try
			{
				var users = _httpContextAccessor.HttpContext.User.Identities.FirstOrDefault().Claims; //FindFirst(ClaimTypes.NameIdentifier);

				var userId = users.FirstOrDefault(c => c.Type.Equals("Id", StringComparison.OrdinalIgnoreCase)).Value;
				var accInfo = await new SYUserGetByID(_appSetting).SYUserGetByIDDAO(long.Parse(userId));

				if (accInfo == null || !accInfo.Any())
				{
					return new ResultApi { Success = ResultCode.ORROR, Message = "Tài khoản không tồn tại" };
				}

				if (accInfo[0].TypeId == 1)
				{
					return new ResultApi { Success = ResultCode.OK, Result = accInfo[0] };
				}
				else if (accInfo[0].TypeId == 2)
				{
					var info = await new BIIndividualGetByUserId(_appSetting).BIIndividualGetByUserIdDAO(accInfo[0].Id);
					if (info == null || !info.Any())
					{
						return new ResultApi { Success = ResultCode.ORROR, Message = "Thông tin tài khoản không tồn tại" };
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


					return new ResultApi { Success = ResultCode.OK, Result = model };

				}
				else if (accInfo[0].TypeId == 3)
				{
					var rsBusinessGetById = await new BIBusinessGetByUserId(_appSetting).BIBusinessGetByUserIdDAO(accInfo[0].Id);
					if (rsBusinessGetById == null || !rsBusinessGetById.Any())
					{
						return new ResultApi { Success = ResultCode.ORROR, Message = "Thông tin tài khoản không tồn tại" };
					}

					return new ResultApi { Success = ResultCode.OK, Result = rsBusinessGetById.FirstOrDefault() };
				}

			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);
				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}

			return new ResultApi { Success = ResultCode.ORROR, Message = "Thông tin tài khoản không tồn tại" };
		}


		/// <summary>
		/// thay mk người dùng
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("user-change-password")]
		[Authorize("ThePolicy")]
		public async Task<object> UserChagePwd([FromForm] ChangePwdModel model)
		{
			try
			{
				var users = _httpContextAccessor.HttpContext.User.Identities.FirstOrDefault().Claims; //FindFirst(ClaimTypes.NameIdentifier);

				var userId = users.FirstOrDefault(c => c.Type.Equals("Id", StringComparison.OrdinalIgnoreCase)).Value;
				var accInfo = await new SYUserGetByID(_appSetting).SYUserGetByIDDAO(long.Parse(userId));
				if (accInfo == null || !accInfo.Any())
				{
					return new ResultApi { Success = ResultCode.ORROR, Message = "Tài khoản không tồn tại" };
				}
				if (string.IsNullOrEmpty(model.OldPassword) || string.IsNullOrEmpty(model.NewPassword))
				{
					return new ResultApi { Success = ResultCode.ORROR, Message = "Dữ liệu không hợp lệ" };
				}
				if (model.NewPassword != model.RePassword)
				{
					return new ResultApi { Success = ResultCode.ORROR, Message = "Mật khẩu mới không trùng khớp" };
				}

				PasswordHasher hasher = new PasswordHasher();
				if (!hasher.AuthenticateUser(model.OldPassword, accInfo[0].Password, accInfo[0].Salt))
				{
					return new ResultApi { Success = ResultCode.ORROR, Message = "Mật khẩu cũ không đúng" };
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
				string token = Request.Headers["Authorization"].ToString().Split(' ')[1].Trim();

				await new SYUserUserAgent(_appSetting).SYUserUserAgentUpdateStatusDAO(accInfo[0].Id, token);

				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
				return new ResultApi { Success = ResultCode.OK };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);
				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		

		/// <summary>
		/// thay mk người dùng bởi quản trị
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("user-change-password-in-manage")]
		[Authorize("ThePolicy")]
		public async Task<object> UserChangePwdInManage(ChangePwdInManage model)
		{
			try
			{

				var accInfo = await new SYUserGetByID(_appSetting).SYUserGetByIDDAO(long.Parse(model.UserId.ToString()));
				if (accInfo == null || !accInfo.Any())
				{
					return new ResultApi { Success = ResultCode.ORROR, Message = "Tài khoản không tồn tại" };
				}

				if (model.NewPassword != model.RePassword)
				{
					return new ResultApi { Success = ResultCode.ORROR, Message = "Mật khẩu mới không trùng khớp" };
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
				if (accInfo[0].Id == new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext))
				{
					string token = Request.Headers["Authorization"].ToString().Split(' ')[1].Trim();
					await new SYUserUserAgent(_appSetting).SYUserUserAgentUpdateStatusDAO(accInfo[0].Id, token);
				}
				else {
					await new SYUserUserAgent(_appSetting).SYUserUserAgentUpdateStatusDAO(accInfo[0].Id, null);
				}
				

				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
				return new ResultApi { Success = ResultCode.OK };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);
				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		/// <summary>
		/// cập nhập người dùng - công bố
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>

		[HttpPost]
		[Route("update-current-info-individual")]
		[Authorize("ThePolicy")]
		public async Task<object> UpdateCurrentInfo( AccountInfoModel model)
        {
            try
            {
				var users = _httpContextAccessor.HttpContext.User.Identities.FirstOrDefault().Claims; //FindFirst(ClaimTypes.NameIdentifier);

				var userId = users.FirstOrDefault(c => c.Type.Equals("Id", StringComparison.OrdinalIgnoreCase)).Value;
				var accInfo = await new SYUserGetByID(_appSetting).SYUserGetByIDDAO(long.Parse(userId));

                if (accInfo == null || !accInfo.Any())
                {
					return new ResultApi { Success = ResultCode.ORROR, Message = "Tài khoản không tồn tại" };
				}

				

				if (model.DateOfBirth >= DateTime.Now)
				{
					return new ResultApi { Success = ResultCode.ORROR, Message = "Ngày sinh không được lớn hơn hoặc bằng ngày hiện tại" };
				}

				if (model.IssuedDate != null && model.IssuedDate < model.DateOfBirth)
				{
					return new ResultApi { Success = ResultCode.ORROR, Message = "Ngày sinh không được lớn hơn ngày thành lập" };
				}

				///Phone,Email,IDCard
				///check ton tai
				///
				accInfo[0].FullName = model.FullName;
				var info = await new BIIndividualGetByUserId(_appSetting).BIIndividualGetByUserIdDAO(accInfo[0].Id);

				var checkExists = await new BIIndividualCheckExists(_appSetting).BIIndividualCheckExistsDAO("Phone", model.Phone, info[0].Id);
				if (checkExists[0].Exists.Value)
					return new ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại đã tồn tại" };
				if (!string.IsNullOrEmpty(model.Email))
				{
					checkExists = await new BIIndividualCheckExists(_appSetting).BIIndividualCheckExistsDAO("Email", model.Email, info[0].Id);
					if (checkExists[0].Exists.Value)
						return new ResultApi { Success = ResultCode.ORROR, Message = "Email đã tồn tại" };
				}
				checkExists = await new BIIndividualCheckExists(_appSetting).BIIndividualCheckExistsDAO("IDCard", model.IdCard, info[0].Id);
				if (checkExists[0].Exists.Value)
					return new ResultApi { Success = ResultCode.ORROR, Message = "Số CMND / CCCD đã tồn tại" };

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

				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
				return new ResultApi { Success = ResultCode.OK, Message = ResultMessage.OK};
			}
            catch (Exception ex)
            {
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);
				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message};
			}
        }


		[HttpPost]
		[Route("update-current-info-business")]
		[Authorize("ThePolicy")]
		public async Task<object> UpdateCurrentInfoBusiness(BusinessAccountInfoModel businessModel)
		{
			try
			{
				var users = _httpContextAccessor.HttpContext.User.Identities.FirstOrDefault().Claims; //FindFirst(ClaimTypes.NameIdentifier);

				var userId = users.FirstOrDefault(c => c.Type.Equals("Id", StringComparison.OrdinalIgnoreCase)).Value;
				var accInfo = await new SYUserGetByID(_appSetting).SYUserGetByIDDAO(long.Parse(userId));

				if (accInfo == null || !accInfo.Any())
				{
					return new ResultApi { Success = ResultCode.ORROR, Message = "Tài khoản không tồn tại" };
				}
				if (businessModel.RepresentativeBirthDay != null && businessModel.RepresentativeBirthDay >= DateTime.Now)
				{
					return new ResultApi { Success = ResultCode.ORROR, Message = "Ngày sinh người đại diện không được lớn hơn ngày hiện tại" };
				}

				///check ton tai
				///
				var info = await new BIBusinessGetByUserId(_appSetting).BIBusinessGetByUserIdDAO(accInfo[0].Id);
				var checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("Phone", businessModel.Phone, info[0].Id);
				if (checkExists[0].Exists.Value && businessModel.Phone != null) return new ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại đã tồn tại" };
				if (!string.IsNullOrEmpty(businessModel.Email))
				{
					checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("Email", businessModel.Email, info[0].Id);
					if (checkExists[0].Exists.Value) return new ResultApi { Success = ResultCode.ORROR, Message = "Email người đại diện đã tồn tại" };
				}
				checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("IDCard", businessModel.IDCard, info[0].Id);
				if (checkExists[0].Exists.Value && businessModel.IDCard != null) return new ResultApi { Success = ResultCode.ORROR, Message = "Số CMND / CCCD đã tồn tại" };
				checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("OrgPhone", businessModel.OrgPhone, info[0].Id);
				if (checkExists[0].Exists.Value) return new ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại doanh nghiệp đã tồn tại" };
				if (!string.IsNullOrEmpty(businessModel.OrgEmail))
				{
					checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("OrgEmail", businessModel.OrgEmail, info[0].Id);
					if (checkExists[0].Exists.Value) return new ResultApi { Success = ResultCode.ORROR, Message = "Email doanh nghiệp đã tồn tại" };
				}
				checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("BusinessRegistration", businessModel.BusinessRegistration, info[0].Id);
				if (checkExists[0].Exists.Value) return new ResultApi { Success = ResultCode.ORROR, Message = "Số đăng ký kinh doanh đã tồn tại" };

				checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("Tax", businessModel.Tax, info[0].Id);
				if (checkExists[0].Exists.Value) return new ResultApi { Success = ResultCode.ORROR, Message = "Mã số thuế đã tồn tại" };

				checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("DecisionOfEstablishing", businessModel.DecisionOfEstablishing, info[0].Id);
				if (checkExists[0].Exists.Value) return new ResultApi { Success = ResultCode.ORROR, Message = "Số quyết định thành lập đã tồn tại" };


				accInfo[0].FullName = businessModel.RepresentativeName;


				var _model = new BIBusinessUpdateInfoIN
				{
					Id = info[0].Id,
					FullName = businessModel.RepresentativeName,
					DateOfBirth = businessModel.RepresentativeBirthDay,
					Email = businessModel.Email,
					Nation = businessModel.Nation,
					ProvinceId = businessModel.ProvinceId,
					DistrictId = businessModel.DistrictId,
					WardsId = businessModel.WardsId,
					Address = businessModel.Address,
					IdCard = businessModel.IDCard,
					IssuedDate = businessModel.DateOfIssue,
					Gender = businessModel.RepresentativeGender,
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
				//update account

				var rsUpdateAcc = new SYUserUpdateInfo(_appSetting).SYUserUpdateInfoDAO(new SYUserUpdateInfoIN
				{
					Id = accInfo[0].Id,
					FullName = businessModel.Business,
					Address = accInfo[0].Address,
				});


				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
				return new ResultApi { Success = ResultCode.OK, Message = ResultMessage.OK };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);
				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion


	}
}
