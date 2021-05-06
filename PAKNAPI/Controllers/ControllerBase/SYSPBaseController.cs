using PAKNAPI.Common;
using PAKNAPI.Controllers;
using PAKNAPI.Models;
using PAKNAPI.ModelBase;
using PAKNAPI.Models.Results;
using System;
using Dapper;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Bugsnag;

namespace PAKNAPI.ControllerBase
{
	[Route("api/SYSPBase")]
	[ApiController]
	public class SYSPBaseController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		private readonly IClient _bugsnag;

		public SYSPBaseController(IAppSetting appSetting, IClient bugsnag)
		{
			_appSetting = appSetting;
			_bugsnag = bugsnag;
		}

		[HttpPost]
		[Authorize]
		[Route("SYAPIInsertBase")]
		public async Task<ActionResult<object>> SYAPIInsertBase(SYAPIInsertIN _sYAPIInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYAPIInsert(_appSetting).SYAPIInsertDAO(_sYAPIInsertIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("SYAPIInsertListBase")]
		public async Task<ActionResult<object>> SYAPIInsertListBase(List<SYAPIInsertIN> _sYAPIInsertINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _sYAPIInsertIN in _sYAPIInsertINs)
				{
					var result = await new SYAPIInsert(_appSetting).SYAPIInsertDAO(_sYAPIInsertIN);
					if (result > 0)
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

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("SYEmailGetFirstBase")]
		public async Task<ActionResult<object>> SYEmailGetFirstBase()
		{
			try
			{
				List<SYEmailGetFirst> rsSYEmailGetFirst = await new SYEmailGetFirst(_appSetting).SYEmailGetFirstDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYEmailGetFirst", rsSYEmailGetFirst},
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

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("SYEmailInsertBase")]
		public async Task<ActionResult<object>> SYEmailInsertBase(SYEmailInsertIN _sYEmailInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYEmailInsert(_appSetting).SYEmailInsertDAO(_sYEmailInsertIN) };
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
		[Route("SYPermissionGetByFunctionBase")]
		public async Task<ActionResult<object>> SYPermissionGetByFunctionBase(int? Id)
		{
			try
			{
				List<SYPermissionGetByFunction> rsSYPermissionGetByFunction = await new SYPermissionGetByFunction(_appSetting).SYPermissionGetByFunctionDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYPermissionGetByFunction", rsSYPermissionGetByFunction},
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
		[Route("SYPermissionCategoryGetBase")]
		public async Task<ActionResult<object>> SYPermissionCategoryGetBase()
		{
			try
			{
				List<SYPermissionCategoryGet> rsSYPermissionCategoryGet = await new SYPermissionCategoryGet(_appSetting).SYPermissionCategoryGetDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYPermissionCategoryGet", rsSYPermissionCategoryGet},
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
		[Route("SYPermissionCheckByUserIdBase")]
		public async Task<ActionResult<object>> SYPermissionCheckByUserIdBase(int? UserId, string APIName)
		{
			try
			{
				List<SYPermissionCheckByUserId> rsSYPermissionCheckByUserId = await new SYPermissionCheckByUserId(_appSetting).SYPermissionCheckByUserIdDAO(UserId, APIName);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYPermissionCheckByUserId", rsSYPermissionCheckByUserId},
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
		[Route("SYPermissionFunctionGetByCategoryBase")]
		public async Task<ActionResult<object>> SYPermissionFunctionGetByCategoryBase(int? Id)
		{
			try
			{
				List<SYPermissionFunctionGetByCategory> rsSYPermissionFunctionGetByCategory = await new SYPermissionFunctionGetByCategory(_appSetting).SYPermissionFunctionGetByCategoryDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYPermissionFunctionGetByCategory", rsSYPermissionFunctionGetByCategory},
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
		[Route("SYPermissionGroupUserGetByGroupIdBase")]
		public async Task<ActionResult<object>> SYPermissionGroupUserGetByGroupIdBase(int? GroupUserId)
		{
			try
			{
				List<SYPermissionGroupUserGetByGroupId> rsSYPermissionGroupUserGetByGroupId = await new SYPermissionGroupUserGetByGroupId(_appSetting).SYPermissionGroupUserGetByGroupIdDAO(GroupUserId);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYPermissionGroupUserGetByGroupId", rsSYPermissionGroupUserGetByGroupId},
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

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("SYPermissionGroupUserInsertByListBase")]
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

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("SYPermissionGroupUserInsertByListListBase")]
		public async Task<ActionResult<object>> SYPermissionGroupUserInsertByListListBase(List<SYPermissionGroupUserInsertByListIN> _sYPermissionGroupUserInsertByListINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _sYPermissionGroupUserInsertByListIN in _sYPermissionGroupUserInsertByListINs)
				{
					var result = await new SYPermissionGroupUserInsertByList(_appSetting).SYPermissionGroupUserInsertByListDAO(_sYPermissionGroupUserInsertByListIN);
					if (result > 0)
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

		[HttpPost]
		[Authorize]
		[Route("SYRoleDeleteBase")]
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

		[HttpPost]
		[Authorize]
		[Route("SYRoleDeleteListBase")]
		public async Task<ActionResult<object>> SYRoleDeleteListBase(List<SYRoleDeleteIN> _sYRoleDeleteINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _sYRoleDeleteIN in _sYRoleDeleteINs)
				{
					var result = await new SYRoleDelete(_appSetting).SYRoleDeleteDAO(_sYRoleDeleteIN);
					if (result > 0)
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

		[HttpGet]
		[Authorize]
		[Route("SYRoleGetAllBase")]
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

		[HttpGet]
		[Authorize]
		[Route("SYRoleGetAllOnPageBase")]
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

		[HttpPost]
		[Authorize]
		[Route("SYRoleInsertBase")]
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

		[HttpPost]
		[Authorize]
		[Route("SYRoleUpdateBase")]
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

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("SYSMSGetFirstBase")]
		public async Task<ActionResult<object>> SYSMSGetFirstBase()
		{
			try
			{
				List<SYSMSGetFirst> rsSYSMSGetFirst = await new SYSMSGetFirst(_appSetting).SYSMSGetFirstDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYSMSGetFirst", rsSYSMSGetFirst},
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

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("SYSMSInsertBase")]
		public async Task<ActionResult<object>> SYSMSInsertBase(SYSMSInsertIN _sYSMSInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYSMSInsert(_appSetting).SYSMSInsertDAO(_sYSMSInsertIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("SYSystemLogDeleteBase")]
		public async Task<ActionResult<object>> SYSystemLogDeleteBase(SYSystemLogDeleteIN _sYSystemLogDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYSystemLogDelete(_appSetting).SYSystemLogDeleteDAO(_sYSystemLogDeleteIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("SYSystemLogDeleteListBase")]
		public async Task<ActionResult<object>> SYSystemLogDeleteListBase(List<SYSystemLogDeleteIN> _sYSystemLogDeleteINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _sYSystemLogDeleteIN in _sYSystemLogDeleteINs)
				{
					var result = await new SYSystemLogDelete(_appSetting).SYSystemLogDeleteDAO(_sYSystemLogDeleteIN);
					if (result > 0)
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

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("SYSystemLogGetAllOnPageBase")]
		public async Task<ActionResult<object>> SYSystemLogGetAllOnPageBase(int? UserId, int? PageSize, int? PageIndex, DateTime? FromDate, DateTime? ToDate)
		{
			try
			{
				List<SYSystemLogGetAllOnPage> rsSYSystemLogGetAllOnPage = await new SYSystemLogGetAllOnPage(_appSetting).SYSystemLogGetAllOnPageDAO(UserId, PageSize, PageIndex, FromDate, ToDate);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYSystemLogGetAllOnPage", rsSYSystemLogGetAllOnPage},
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
		[Route("SYSystemLogGetAllOnPageAdminBase")]
		public async Task<ActionResult<object>> SYSystemLogGetAllOnPageAdminBase(int? UserId, int? PageSize, int? PageIndex, DateTime? CreateDate, byte? Status, string Description)
		{
			try
			{
				List<SYSystemLogGetAllOnPageAdmin> rsSYSystemLogGetAllOnPageAdmin = await new SYSystemLogGetAllOnPageAdmin(_appSetting).SYSystemLogGetAllOnPageAdminDAO(UserId, PageSize, PageIndex, CreateDate, Status, Description);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYSystemLogGetAllOnPageAdmin", rsSYSystemLogGetAllOnPageAdmin},
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
		[Route("SYUnitGetDropdownBase")]
		public async Task<ActionResult<object>> SYUnitGetDropdownBase()
		{
			try
			{
				List<SYUnitGetDropdown> rsSYUnitGetDropdown = await new SYUnitGetDropdown(_appSetting).SYUnitGetDropdownDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYUnitGetDropdown", rsSYUnitGetDropdown},
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
		[Route("SYUnitGetDropdownLevelBase")]
		public async Task<ActionResult<object>> SYUnitGetDropdownLevelBase()
		{
			try
			{
				List<SYUnitGetDropdownLevel> rsSYUnitGetDropdownLevel = await new SYUnitGetDropdownLevel(_appSetting).SYUnitGetDropdownLevelDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYUnitGetDropdownLevel", rsSYUnitGetDropdownLevel},
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
		[Route("SYUnitGetDropdownNotMainBase")]
		public async Task<ActionResult<object>> SYUnitGetDropdownNotMainBase()
		{
			try
			{
				List<SYUnitGetDropdownNotMain> rsSYUnitGetDropdownNotMain = await new SYUnitGetDropdownNotMain(_appSetting).SYUnitGetDropdownNotMainDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYUnitGetDropdownNotMain", rsSYUnitGetDropdownNotMain},
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
		[Route("SYUnitGetMainIdBase")]
		public async Task<ActionResult<object>> SYUnitGetMainIdBase()
		{
			try
			{
				List<SYUnitGetMainId> rsSYUnitGetMainId = await new SYUnitGetMainId(_appSetting).SYUnitGetMainIdDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYUnitGetMainId", rsSYUnitGetMainId},
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
		[Route("SYUnitGetNameByIdBase")]
		public async Task<ActionResult<object>> SYUnitGetNameByIdBase(int? Id)
		{
			try
			{
				List<SYUnitGetNameById> rsSYUnitGetNameById = await new SYUnitGetNameById(_appSetting).SYUnitGetNameByIdDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYUnitGetNameById", rsSYUnitGetNameById},
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
		[Route("SYUserGetByUnitIdBase")]
		public async Task<ActionResult<object>> SYUserGetByUnitIdBase(int? UnitId)
		{
			try
			{
				List<SYUserGetByUnitId> rsSYUserGetByUnitId = await new SYUserGetByUnitId(_appSetting).SYUserGetByUnitIdDAO(UnitId);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYUserGetByUnitId", rsSYUserGetByUnitId},
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
		[Route("SYUserGetByUserNameBase")]
		public async Task<ActionResult<object>> SYUserGetByUserNameBase(string UserName)
		{
			try
			{
				List<SYUserGetByUserName> rsSYUserGetByUserName = await new SYUserGetByUserName(_appSetting).SYUserGetByUserNameDAO(UserName);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYUserGetByUserName", rsSYUserGetByUserName},
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
		[Route("SYUserGetNonSystemBase")]
		public async Task<ActionResult<object>> SYUserGetNonSystemBase()
		{
			try
			{
				List<SYUserGetNonSystem> rsSYUserGetNonSystem = await new SYUserGetNonSystem(_appSetting).SYUserGetNonSystemDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYUserGetNonSystem", rsSYUserGetNonSystem},
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

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("SYNotificationDeleteBase")]
		public async Task<ActionResult<object>> SYNotificationDeleteBase(SYNotificationDeleteIN _sYNotificationDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYNotificationDelete(_appSetting).SYNotificationDeleteDAO(_sYNotificationDeleteIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("SYNotificationDeleteListBase")]
		public async Task<ActionResult<object>> SYNotificationDeleteListBase(List<SYNotificationDeleteIN> _sYNotificationDeleteINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _sYNotificationDeleteIN in _sYNotificationDeleteINs)
				{
					var result = await new SYNotificationDelete(_appSetting).SYNotificationDeleteDAO(_sYNotificationDeleteIN);
					if (result > 0)
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
