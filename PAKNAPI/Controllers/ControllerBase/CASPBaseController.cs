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
	[Route("api/CASPBase")]
	[ApiController]
	public class CASPBaseController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		private readonly IClient _bugsnag;

		public CASPBaseController(IAppSetting appSetting, IClient bugsnag)
		{
			_appSetting = appSetting;
			_bugsnag = bugsnag;
		}

		[HttpGet]
		[Authorize]
		[Route("CAFieldGetDropdownBase")]
		public async Task<ActionResult<object>> CAFieldGetDropdownBase()
		{
			try
			{
				List<CAFieldGetDropdown> rsCAFieldGetDropdown = await new CAFieldGetDropdown(_appSetting).CAFieldGetDropdownDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAFieldGetDropdown", rsCAFieldGetDropdown},
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
		[Route("CAHashtagGetAllBase")]
		public async Task<ActionResult<object>> CAHashtagGetAllBase()
		{
			try
			{
				List<CAHashtagGetAll> rsCAHashtagGetAll = await new CAHashtagGetAll(_appSetting).CAHashtagGetAllDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAHashtagGetAll", rsCAHashtagGetAll},
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
		[Route("CAHashtagGetDropdownBase")]
		public async Task<ActionResult<object>> CAHashtagGetDropdownBase()
		{
			try
			{
				List<CAHashtagGetDropdown> rsCAHashtagGetDropdown = await new CAHashtagGetDropdown(_appSetting).CAHashtagGetDropdownDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAHashtagGetDropdown", rsCAHashtagGetDropdown},
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
