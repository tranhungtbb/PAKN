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
	[Route("api/SYCaptChaSPBase")]
	[ApiController]
	public class SYCaptChaSPBaseController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		private readonly IClient _bugsnag;

		public SYCaptChaSPBaseController(IAppSetting appSetting, IClient bugsnag)
		{
			_appSetting = appSetting;
			_bugsnag = bugsnag;
		}

		[HttpPost]
		[Authorize]
		[Route("SYCaptChaDeleteBase")]
		public async Task<ActionResult<object>> SYCaptChaDeleteBase(SYCaptChaDeleteIN _sYCaptChaDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYCaptChaDelete(_appSetting).SYCaptChaDeleteDAO(_sYCaptChaDeleteIN) };
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
		[Route("SYCaptChaInsertDataBase")]
		public async Task<ActionResult<object>> SYCaptChaInsertDataBase(SYCaptChaInsertDataIN _sYCaptChaInsertDataIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYCaptChaInsertData(_appSetting).SYCaptChaInsertDataDAO(_sYCaptChaInsertDataIN) };
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
		[Route("SYCaptChaValidatorBase")]
		public async Task<ActionResult<object>> SYCaptChaValidatorBase(string Code)
		{
			try
			{
				List<SYCaptChaValidator> rsSYCaptChaValidator = await new SYCaptChaValidator(_appSetting).SYCaptChaValidatorDAO(Code);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYCaptChaValidator", rsSYCaptChaValidator},
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
