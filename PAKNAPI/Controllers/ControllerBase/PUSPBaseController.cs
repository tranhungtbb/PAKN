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
	[Route("api/PUSPBase")]
	[ApiController]
	public class PUSPBaseController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		private readonly IClient _bugsnag;

		public PUSPBaseController(IAppSetting appSetting, IClient bugsnag)
		{
			_appSetting = appSetting;
			_bugsnag = bugsnag;
		}

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("PURecommendationStatisticsGetByUserIdBase")]
		public async Task<ActionResult<object>> PURecommendationStatisticsGetByUserIdBase(int? UserId)
		{
			try
			{
				List<PURecommendationStatisticsGetByUserId> rsPURecommendationStatisticsGetByUserId = await new PURecommendationStatisticsGetByUserId(_appSetting).PURecommendationStatisticsGetByUserIdDAO(UserId);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"PURecommendationStatisticsGetByUserId", rsPURecommendationStatisticsGetByUserId},
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
		[Route("PURecommendationGetAllOnPageBase")]
		public async Task<ActionResult<object>> PURecommendationGetAllOnPageBase(string KeySearch, int? Status, int? PageSize, int? PageIndex)
		{
			try
			{
				List<PURecommendationGetAllOnPage> rsPURecommendationGetAllOnPage = await new PURecommendationGetAllOnPage(_appSetting).PURecommendationGetAllOnPageDAO(KeySearch, Status, PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"PURecommendationGetAllOnPage", rsPURecommendationGetAllOnPage},
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
