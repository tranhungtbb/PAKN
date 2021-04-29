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
		public async Task<ActionResult<object>> SYSystemLogGetAllOnPageAdminBase(int? UserId, int? PageSize, int? PageIndex, DateTime? FromDate, DateTime? ToDate, int? Status, string Description)
		{
			try
			{
				List<SYSystemLogGetAllOnPageAdmin> rsSYSystemLogGetAllOnPageAdmin = await new SYSystemLogGetAllOnPageAdmin(_appSetting).SYSystemLogGetAllOnPageAdminDAO(UserId, PageSize, PageIndex, FromDate, ToDate, Status, Description);
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
	}
}
