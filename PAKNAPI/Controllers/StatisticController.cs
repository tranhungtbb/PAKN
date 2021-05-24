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
using PAKNAPI.Models.Remind;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using PAKNAPI.Models.Recommendation;
using PAKNAPI.Models.Invitation;
using PAKNAPI.Models.EmailSMSModel;
using PAKNAPI.Models.Statistic;

namespace PAKNAPI.Controllers
{
    [Route("api/Statistic")]
    [ApiController]
   
    public class StatisticController : BaseApiController
    {
        private readonly IAppSetting _appSetting;
        private readonly IClient _bugsnag;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public StatisticController(IAppSetting appSetting, IClient bugsnag, IWebHostEnvironment hostEnvironment)
        {
            _appSetting = appSetting;
            _bugsnag = bugsnag;
            _hostingEnvironment = hostEnvironment;
        }

		[HttpGet]
		[Authorize]
		[Route("STT_RecommendationByUnit")]
		public async Task<ActionResult<object>> STT_RecommendationByUnitGetAllOnPageBase(string LtsUnitId, int? Year, int? Timeline, DateTime? FromDate, DateTime? ToDate, int? PageSize, int? PageIndex)
		{
			try
			{
				List<StatisticRecommendationByUnitGetAllOnPage> mrrRecommendationByUnit = await new StatisticRecommendationByUnitGetAllOnPage(_appSetting).StatisticRecommendationByUnitGetAllOnPageDAO(LtsUnitId,Year,Timeline,FromDate,ToDate,PageSize,PageIndex);
				mrrRecommendationByUnit.ForEach((item)=> {
					item.Total = item.ReceiveWait + item.ReceiveApproved + item.ReceiveDeny + item.ProcessWait + item.Processing + item.Finised;
				});

				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"StatisticRecommendationByUnitGetAllOnPage", mrrRecommendationByUnit},
						{"PageIndex", mrrRecommendationByUnit != null && mrrRecommendationByUnit.Count > 0 ? PageIndex : 0},
						{"PageSize", mrrRecommendationByUnit != null && mrrRecommendationByUnit.Count > 0 ? PageSize : 0},
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
		[Route("STT_RecommendationByGroupWord")]
		public async Task<ActionResult<object>> STT_RecommendationByGroupWordGetAllOnPageBase(string LtsUnitId, DateTime? FromDate, DateTime? ToDate)
		{
			try
			{
				List<StatisticRecommendationByGroupWordGetAllOnPage> mrrRecommendationByGroupWord = await new StatisticRecommendationByGroupWordGetAllOnPage(_appSetting).StatisticRecommendationByGroupWordGetAllOnPageDAO(LtsUnitId, FromDate, ToDate);
				List<SYUnitGetDropdownByListId> rsSYUnitGetDropdownByListId= await new SYUnitGetDropdownByListId(_appSetting).SYUnitGetDropdownByListIdDAO(LtsUnitId);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"ListData", mrrRecommendationByGroupWord},
						{"ListUnits", rsSYUnitGetDropdownByListId},
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
		[Route("STT_RecommendationByGroupWordDetail")]
		public async Task<ActionResult<object>> RecommendationByGroupWordDetail(string Code, string SendName, string Title, string Content, int? UnitId, int? GroupWordId, DateTime? FromDate, DateTime? ToDate, int? PageSize, int? PageIndex)
		{
			try
			{
				List<StatisticRecommendationByGroupWordDetail> rsStatisticRecommendationByGroupWordDetail = await new StatisticRecommendationByGroupWordDetail(_appSetting).StatisticRecommendationByGroupWordDetailDAO(Code, SendName, Title, Content, UnitId, GroupWordId, FromDate, ToDate, PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"ListData", rsStatisticRecommendationByGroupWordDetail},
						{"TotalCount", rsStatisticRecommendationByGroupWordDetail != null && rsStatisticRecommendationByGroupWordDetail.Count > 0 ? rsStatisticRecommendationByGroupWordDetail[0].RowNumber : 0},
						{"PageIndex", rsStatisticRecommendationByGroupWordDetail != null && rsStatisticRecommendationByGroupWordDetail.Count > 0 ? PageIndex : 0},
						{"PageSize", rsStatisticRecommendationByGroupWordDetail != null && rsStatisticRecommendationByGroupWordDetail.Count > 0 ? PageSize : 0},
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
		[Route("STT_RecommendationByField")]
		public async Task<ActionResult<object>> STT_RecommendationByFieldGetAllOnPageBase(string LtsUnitId, int? Year, int? Timeline, DateTime? FromDate, DateTime? ToDate, int? PageSize, int? PageIndex)
		{
			try
			{
				List<StatisticRecommendationByFieldGetAllOnPage> mrrRecommendationByField = await new StatisticRecommendationByFieldGetAllOnPage(_appSetting).StatisticRecommendationByFieldGetAllOnPageDAO(LtsUnitId, Year, Timeline, FromDate, ToDate, PageSize, PageIndex);
				mrrRecommendationByField.ForEach((item) => {
					item.Total = item.ReceiveWait + item.ReceiveApproved + item.ReceiveDeny + item.ProcessWait + item.Processing + item.Finised;
				});

				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"StatisticRecommendationByFieldGetAllOnPage", mrrRecommendationByField},
						{"TotalCount", mrrRecommendationByField != null && mrrRecommendationByField.Count > 0 ? mrrRecommendationByField[0].RowNumber : 0},
						{"PageIndex", mrrRecommendationByField != null && mrrRecommendationByField.Count > 0 ? PageIndex : 0},
						{"PageSize", mrrRecommendationByField != null && mrrRecommendationByField.Count > 0 ? PageSize : 0},
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
		[Route("SY_UnitGetChildrenDropdown")]
		public async Task<ActionResult<object>> SY_UnitGetChildrenDropdown()
		{
			try
			{
				var unitId = new LogHelper(_appSetting).GetUnitIdFromRequest(HttpContext);
				List<UnitGetChildrenDropdown> data = await new UnitGetChildrenDropdown(_appSetting).UnitGetChildrenDropdownDAO(unitId);
				
				return new ResultApi { Success = ResultCode.OK, Result = data };
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
