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
using PAKNAPI.Models.ModelBase;

namespace PAKNAPI.ControllerBase
{
	[Route("api/PURecommendation")]
	[ApiController]
	public class PURecommendationController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		private readonly IClient _bugsnag;

		public PURecommendationController(IAppSetting appSetting, IClient bugsnag)
		{
			_appSetting = appSetting;
			_bugsnag = bugsnag;
		}

		#region PURecommendationAllOnPage

		[HttpGet]
		[Authorize]
		[Route("PURecommendationAllOnPage")]
		public async Task<ActionResult<object>> PURecommendationAllOnPage(string? KeySearch, int Code, int PageSize, int PageIndex)
		{
			try
			{
				var rsPURecommendationOnPage = await new PURecommendation(_appSetting).PURecommendationAllOnPage(KeySearch, Code, PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"PURecommendation", rsPURecommendationOnPage},
						{"TotalCount", rsPURecommendationOnPage != null && rsPURecommendationOnPage.Count > 0 ? rsPURecommendationOnPage[0].RowNumber : 0},
						{"PageIndex", rsPURecommendationOnPage != null && rsPURecommendationOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsPURecommendationOnPage != null && rsPURecommendationOnPage.Count > 0 ? PageSize : 0},
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

		#endregion PURecommendationAllOnPage


		#region PURecommendationgetById

		[HttpGet]
		[Authorize]
		[Route("PURecommendationGetById")]
		public async Task<ActionResult<object>> PURecommendationgetById(string? KeySearch, int Code, int PageSize, int PageIndex)
		{
			try
			{
				var rsPURecommendationOnPage = await new PURecommendation(_appSetting).PURecommendationAllOnPage(KeySearch, Code, PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"PURecommendation", rsPURecommendationOnPage},
						{"TotalCount", rsPURecommendationOnPage != null && rsPURecommendationOnPage.Count > 0 ? rsPURecommendationOnPage[0].RowNumber : 0},
						{"PageIndex", rsPURecommendationOnPage != null && rsPURecommendationOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsPURecommendationOnPage != null && rsPURecommendationOnPage.Count > 0 ? PageSize : 0},
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

		#endregion PURecommendationgetById


	}
}
