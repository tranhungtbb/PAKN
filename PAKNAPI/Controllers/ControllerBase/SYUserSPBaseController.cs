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
	[Route("api/SYUserSPBase")]
	[ApiController]
	public class SYUserSPBaseController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		private readonly IClient _bugsnag;

		public SYUserSPBaseController(IAppSetting appSetting, IClient bugsnag)
		{
			_appSetting = appSetting;
			_bugsnag = bugsnag;
		}

		[HttpPost]
		[Authorize]
		[Route("SYUserDeleteBase")]
		public async Task<ActionResult<object>> SYUserDeleteBase(SYUserDeleteIN _sYUserDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYUserDelete(_appSetting).SYUserDeleteDAO(_sYUserDeleteIN) };
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
		[Route("SYUserDeleteListBase")]
		public async Task<ActionResult<object>> SYUserDeleteListBase(List<SYUserDeleteIN> _sYUserDeleteINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _sYUserDeleteIN in _sYUserDeleteINs)
				{
					var result = await new SYUserDelete(_appSetting).SYUserDeleteDAO(_sYUserDeleteIN);
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
		[Route("SYUserGetAllOnPageBase")]
		public async Task<ActionResult<object>> SYUserGetAllOnPageBase(int? PageSize, int? PageIndex, string UserName, string FullName, string Phone, bool? IsActive, int? UnitId, int? TypeId)
		{
			try
			{
				List<SYUserGetAllOnPage> rsSYUserGetAllOnPage = await new SYUserGetAllOnPage(_appSetting).SYUserGetAllOnPageDAO(PageSize, PageIndex, UserName, FullName, Phone, IsActive, UnitId, TypeId);
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
		[Route("SYUserGetByIDBase")]
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("SYUserInsertBase")]
		public async Task<ActionResult<object>> SYUserInsertBase(SYUserInsertIN _sYUserInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYUserInsert(_appSetting).SYUserInsertDAO(_sYUserInsertIN) };
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
		[Route("SYUserInsertListBase")]
		public async Task<ActionResult<object>> SYUserInsertListBase(List<SYUserInsertIN> _sYUserInsertINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _sYUserInsertIN in _sYUserInsertINs)
				{
					var result = await new SYUserInsert(_appSetting).SYUserInsertDAO(_sYUserInsertIN);
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
		[Route("SYUserUpdateBase")]
		public async Task<ActionResult<object>> SYUserUpdateBase(SYUserUpdateIN _sYUserUpdateIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYUserUpdate(_appSetting).SYUserUpdateDAO(_sYUserUpdateIN) };
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
		[Route("SYUserUpdateListBase")]
		public async Task<ActionResult<object>> SYUserUpdateListBase(List<SYUserUpdateIN> _sYUserUpdateINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _sYUserUpdateIN in _sYUserUpdateINs)
				{
					var result = await new SYUserUpdate(_appSetting).SYUserUpdateDAO(_sYUserUpdateIN);
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
		[Route("SYUSRGetPermissionByUserIdBase")]
		public async Task<ActionResult<object>> SYUSRGetPermissionByUserIdBase(long? UserId)
		{
			try
			{
				List<SYUSRGetPermissionByUserId> rsSYUSRGetPermissionByUserId = await new SYUSRGetPermissionByUserId(_appSetting).SYUSRGetPermissionByUserIdDAO(UserId);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYUSRGetPermissionByUserId", rsSYUSRGetPermissionByUserId},
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
