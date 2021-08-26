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
	[Route("api/role")]
	[ApiController]
	[ValidateModel]
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
		/// get data for create
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Route("get-data-for-create")]
		[Authorize]
		public async Task<ActionResult<object>> GetDataForCreate()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new PermissionDAO(_appSetting).GetListPermission() };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// chi tiết vai trò
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize]
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// xóa vai trò
		/// </summary>
		/// <param name="_sYRoleDeleteIN"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize]
		[Route("delete")]
		public async Task<ActionResult<object>> SYRoleDeleteBase(SYRoleDeleteIN _sYRoleDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYRoleDelete(_appSetting).SYRoleDeleteDAO(_sYRoleDeleteIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// danh sách người dùng theo vai trò
		/// </summary>
		/// <param name="_sYPermissionGroupUserInsertByListIN"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("permission-group-user-insert-by-list")]
		public async Task<ActionResult<object>> SYPermissionGroupUserInsertByListBase(SYPermissionGroupUserInsertByListIN _sYPermissionGroupUserInsertByListIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYPermissionGroupUserInsertByList(_appSetting).SYPermissionGroupUserInsertByListDAO(_sYPermissionGroupUserInsertByListIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// danh sách vao trò (all)
		/// </summary>
		/// <returns></returns>

		[HttpGet]
		[Authorize]
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// danh sách vai trò (on page)
		/// </summary>
		/// <param name="PageSize"></param>
		/// <param name="PageIndex"></param>
		/// <param name="UserCount"></param>
		/// <param name="Name"></param>
		/// <param name="Description"></param>
		/// <param name="IsActived"></param>
		/// <returns></returns>
		[HttpGet]
		[Authorize]
		[Route("get-list-role-on-page")]
		public async Task<ActionResult<object>> SYRoleGetAllOnPageBase(int? PageSize, int? PageIndex, int? UserCount, string Name, string Description, bool? IsActived)
		{
			try
			{
				List<SYRoleGetAllOnPage> rsSYRoleGetAllOnPage = await new SYRoleGetAllOnPage(_appSetting).SYRoleGetAllOnPageDAO(PageSize, PageIndex, UserCount, Name, Description, IsActived);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYRoleGetAllOnPage", rsSYRoleGetAllOnPage},
						{"TotalCount", rsSYRoleGetAllOnPage != null && rsSYRoleGetAllOnPage.Count > 0 ? rsSYRoleGetAllOnPage[0].RowNumber : 0},
						{"PageIndex", rsSYRoleGetAllOnPage != null && rsSYRoleGetAllOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsSYRoleGetAllOnPage != null && rsSYRoleGetAllOnPage.Count > 0 ? PageSize : 0},
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
		/// <summary>
		/// thêm mới vai trò
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize]
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// cập nhập vai trò
		/// </summary>
		/// <param name="_sYRoleUpdateIN"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize]
		[Route("update")]
		public async Task<ActionResult<object>> SYRoleUpdateBase(SYRoleUpdateIN _sYRoleUpdateIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYRoleUpdate(_appSetting).SYRoleUpdateDAO(_sYRoleUpdateIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// thêm mới vai trò
		/// </summary>
		/// <param name="_sYRoleInsertIN"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize]
		[Route("insert")]
		public async Task<ActionResult<object>> SYRoleInsertBase(SYRoleInsertIN _sYRoleInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYRoleInsert(_appSetting).SYRoleInsertDAO(_sYRoleInsertIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// thêm mới người dùng theo vai trò
		/// </summary>
		/// <param name="_sYUserRoleMaps"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize]
		[Route("user-role-map-insert-list")]
		public async Task<ActionResult<object>> SYUserRoleMapListInsert(List<SYUserRoleMapInsertIN> _sYUserRoleMaps)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (SYUserRoleMapInsertIN _sYUserRoleMapInsertIN in _sYUserRoleMaps)
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

	}
}
