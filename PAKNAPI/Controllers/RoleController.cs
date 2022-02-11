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
using NSwag.Annotations;

namespace PAKNAPI.Controllers
{
	[Route("api/role")]
	[ApiController]
	[ValidateModel]
	[OpenApiTag("Vai trò", Description = "quản lý vai trò")]
	public class RoleController : BaseApiController
	{
		private readonly IFileService _fileService;
		private readonly IAppSetting _appSetting;
		private readonly IClient _bugsnag;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private Microsoft.Extensions.Configuration.IConfiguration _config;
		public RoleController(IFileService fileService, IAppSetting appSetting, IClient bugsnag,
			IHttpContextAccessor httpContextAccessor, Microsoft.Extensions.Configuration.IConfiguration config)
		{
			_fileService = fileService;
			_appSetting = appSetting;
			_bugsnag = bugsnag;
			_httpContextAccessor = httpContextAccessor;
			_config = config;
		}

		/// <summary>
		/// get data for create - Authorize
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Route("get-data-for-create")]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		public async Task<ActionResult<object>> GetDataForCreate()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new PermissionDAO(_appSetting).GetListPermission() };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// chi tiết vai trò - Authorize
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("get-by-id")]
		public async Task<ActionResult<object>> GetByID(int? Id)
		{
			try
			{
				SYRoleGetByID rsSYRoleGetByID = (await new SYRoleGetByID(_appSetting).SYRoleGetByIDDAO(Id)).FirstOrDefault();
				List<int> rsSYPermissionGroupUserGetByGroupId = await new PermissionDAO(_appSetting).SYPermissionGroupUserGetByGroupId(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"Data", rsSYRoleGetByID},
						{"ListPermission", rsSYPermissionGroupUserGetByGroupId},
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
		/// xóa vai trò - Authorize
		/// </summary>
		/// <param name="_sYRoleDeleteIN"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("delete")]
		public async Task<ActionResult<object>> SYRoleDeleteBase(SYRoleDeleteIN _sYRoleDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYRoleDelete(_appSetting).SYRoleDeleteDAO(_sYRoleDeleteIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// insert permission cho vai trò - Authorize
		/// </summary>
		/// <param name="_sYPermissionGroupUserInsertByListIN"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("permission-group-user-insert-by-list")]
		public async Task<ActionResult<object>> SYPermissionGroupUserInsertByListBase(SYPermissionGroupUserInsertByListIN _sYPermissionGroupUserInsertByListIN)
		{
			try
			{

				return new ResultApi { Success = ResultCode.OK, Result = await new SYPermissionGroupUserInsertByList(_appSetting).SYPermissionGroupUserInsertByListDAO(_sYPermissionGroupUserInsertByListIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// danh sách vao trò (all) - Authorize
		/// </summary>
		/// <returns></returns>

		[HttpGet]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("get-list-role-base")]
		public async Task<ActionResult<object>> SYRoleGetAllBase()
		{
			try
			{
				List<SYRoleGetAll> rsSYRoleGetAll = await new SYRoleGetAll(_appSetting).SYRoleGetAllDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYRoleGetAll", rsSYRoleGetAll},
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
		/// danh sách vai trò (on page) - Authorize
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("get-list-role-on-page")]
		public async Task<ActionResult<object>> SYRoleGetAllOnPageBase()
		{
			try
			{
				List<SYRoleGetAllOnPage> rsSYRoleGetAllOnPage = await new SYRoleGetAllOnPage(_appSetting).SYRoleGetAllOnPageDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYRoleGetAllOnPage", rsSYRoleGetAllOnPage},
						{"TotalCount", rsSYRoleGetAllOnPage != null && rsSYRoleGetAllOnPage.Count > 0 ? rsSYRoleGetAllOnPage[0].RowNumber : 0}
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
		/// chi tiết vai trò - Authorize
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("SYRoleGetByIDBase")]
		public async Task<ActionResult<object>> SYRoleGetByIDBase(int? Id)
		{
			try
			{
				List<SYRoleGetByID> rsSYRoleGetByID = await new SYRoleGetByID(_appSetting).SYRoleGetByIDDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYRoleGetByID", rsSYRoleGetByID},
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
		/// cập nhập vai trò - Authorize
		/// </summary>
		/// <param name="_sYRoleUpdateIN"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("update")]
		public async Task<ActionResult<object>> SYRoleUpdateBase(SYRoleUpdateIN _sYRoleUpdateIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYRoleUpdate(_appSetting).SYRoleUpdateDAO(_sYRoleUpdateIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// thêm mới vai trò - Authorize
		/// </summary>
		/// <param name="_sYRoleInsertIN"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("insert")]
		public async Task<ActionResult<object>> SYRoleInsertBase(SYRoleInsertIN _sYRoleInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYRoleInsert(_appSetting).SYRoleInsertDAO(_sYRoleInsertIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// thêm mới người dùng theo vai trò
		/// </summary>
		/// <param name="_sYUserRoleMaps"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("user-role-map-insert-list")]
		public async Task<ActionResult<object>> SYUserRoleMapListInsert(SYUserRoleMapInsertObject _sYUserRoleMaps)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (SYUserRoleMapInsertIN _sYUserRoleMapInsertIN in _sYUserRoleMaps._sYUserRoleMaps)
				{
					int? result = await new SYUserRoleMapInsert(_appSetting).SYUserRoleMapInsertDAO(_sYUserRoleMapInsertIN);
					if (result != null)
					{
						count++;
					}
					else
					{
						errcount++;
					}
				}

				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CountSuccess", count},
						{"CountError", errcount}
					};
				if (_sYUserRoleMaps.isCreated == false) {
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
				}
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

	}
}
