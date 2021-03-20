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

namespace PAKNAPI.ControllerBase
{
	[Route("api/SYCategorySPBase")]
	[ApiController]
	public class SYCategorySPBaseController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		public SYCategorySPBaseController(IAppSetting appSetting)
		{
			_appSetting = appSetting;
		}

		[HttpGet]
		[Authorize]
		[Route("SYCAPositionGetOnPageBase")]
		public async Task<ActionResult<object>> SYCAPositionGetOnPageBase(int? PageSize, int? PageIndex, string Search)
		{
			try
			{
				List<SYCAPositionGetOnPage> rsSYCAPositionGetOnPage = await new SYCAPositionGetOnPage(_appSetting).SYCAPositionGetOnPageDAO(PageSize, PageIndex, Search);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYCAPositionGetOnPage", rsSYCAPositionGetOnPage},
					};
				return Ok(json);
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultSuccess.ORROR, Message = ex.Message };
			}
		}

		[HttpGet]
		[Authorize]
		[Route("SYCAUnitGetOnPageBase")]
		public async Task<ActionResult<object>> SYCAUnitGetOnPageBase(int? PageSize, int? PageIndex, string Search)
		{
			try
			{
				List<SYCAUnitGetOnPage> rsSYCAUnitGetOnPage = await new SYCAUnitGetOnPage(_appSetting).SYCAUnitGetOnPageDAO(PageSize, PageIndex, Search);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYCAUnitGetOnPage", rsSYCAUnitGetOnPage},
					};
				return Ok(json);
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultSuccess.ORROR, Message = ex.Message };
			}
		}
	}
}
