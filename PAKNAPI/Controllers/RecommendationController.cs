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
using PAKNAPI.Models.Recommendation;

namespace PAKNAPI.Controller
{
	[Route("api/Recommendation")]
	[ApiController]
	public class RecommendationController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		public RecommendationController(IAppSetting appSetting)
		{
			_appSetting = appSetting;
		}



		[HttpGet]
		[Authorize]
		[Route("RecommendationGetDataForCreate")]
		public async Task<ActionResult<object>> RecommendationGetDataForCreate()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new RecommendationDAO(_appSetting).RecommendationGetDataForCreate() };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}


		[HttpPost]
		[Authorize]
		[Route("RecommendationInsert")]
		public async Task<ActionResult<object>> RecommendationInsert(MRRecommendation _mRRecommendation)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendation(_appSetting).MRRecommendationInsert(_mRRecommendation) };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
	}
}
