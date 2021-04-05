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
	[Route("api/BISPBase")]
	[ApiController]
	public class BISPBaseController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		private readonly IClient _bugsnag;

		public BISPBaseController(IAppSetting appSetting, IClient bugsnag)
		{
			_appSetting = appSetting;
			_bugsnag = bugsnag;
		}

		[HttpGet]
		[Authorize]
		[Route("BIBusinessGetDropdownBase")]
		public async Task<ActionResult<object>> BIBusinessGetDropdownBase()
		{
			try
			{
				List<BIBusinessGetDropdown> rsBIBusinessGetDropdown = await new BIBusinessGetDropdown(_appSetting).BIBusinessGetDropdownDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"BIBusinessGetDropdown", rsBIBusinessGetDropdown},
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
		[Route("BIIndividualGetDropdownBase")]
		public async Task<ActionResult<object>> BIIndividualGetDropdownBase()
		{
			try
			{
				List<BIIndividualGetDropdown> rsBIIndividualGetDropdown = await new BIIndividualGetDropdown(_appSetting).BIIndividualGetDropdownDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"BIIndividualGetDropdown", rsBIIndividualGetDropdown},
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
