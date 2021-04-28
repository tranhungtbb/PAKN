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
		[Authorize("ThePolicy")]
		[Route("BIBusinessCheckExistsBase")]
		public async Task<ActionResult<object>> BIBusinessCheckExistsBase(string Field, string Value)
		{
			try
			{
				List<BIBusinessCheckExists> rsBIBusinessCheckExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO(Field, Value);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"BIBusinessCheckExists", rsBIBusinessCheckExists},
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
		[Authorize("ThePolicy")]
		[Route("BIBusinessGetRepresentativeByIdBase")]
		public async Task<ActionResult<object>> BIBusinessGetRepresentativeByIdBase(long? Id)
		{
			try
			{
				List<BIBusinessGetRepresentativeById> rsBIBusinessGetRepresentativeById = await new BIBusinessGetRepresentativeById(_appSetting).BIBusinessGetRepresentativeByIdDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"BIBusinessGetRepresentativeById", rsBIBusinessGetRepresentativeById},
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
