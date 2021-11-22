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
    [Route("api/statistic")]
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
		/// <summary>
		/// tk pakn theo đơn vị
		/// </summary>
		/// <param name="LtsUnitId"></param>
		/// <param name="FromDate"></param>
		/// <param name="ToDate"></param>
		/// <returns></returns>
		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("recommendation-by-unit")]
		public async Task<ActionResult<object>> STT_RecommendationByUnitGetAllOnPageBase(string LtsUnitId, DateTime? FromDate, DateTime? ToDate)
		{
			try
			{
				int UnitProcessId = new LogHelper(_appSetting).GetUnitIdFromRequest(HttpContext);
				long UserProcessId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
				List<StatisticRecommendationByUnitGetAllOnPage> mrrRecommendationByUnit = 
					await new StatisticRecommendationByUnitGetAllOnPage(_appSetting).StatisticRecommendationByUnitGetAllOnPageDAO(LtsUnitId, UnitProcessId, UserProcessId, FromDate,ToDate);
				

				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"StatisticRecommendationByUnitGetAllOnPage", mrrRecommendationByUnit}
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				//_bugsnag.Notify(ex);
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// chi tiết pakn theo đơn vị
		/// </summary>
		/// <param name="UnitId"></param>
		/// <param name="Code"></param>
		/// <param name="CreateName"></param>
		/// <param name="Title"></param>
		/// <param name="Field"></param>
		/// <param name="Status"></param>
		/// <param name="FromDate"></param>
		/// <param name="ToDate"></param>
		/// <param name="PageSize"></param>
		/// <param name="PageIndex"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("recommendation-by-unit-detail")]
		public async Task<ActionResult<object>> RecommendationsByUnitDetailGetAllOnPageBase(int UnitId,string Code,string CreateName, string Title, int? Field, int? Status, DateTime? FromDate, DateTime? ToDate, int? PageSize, int? PageIndex)
		{
			try
			{
				int UnitProcessId = new LogHelper(_appSetting).GetUnitIdFromRequest(HttpContext);
				long UserProcessId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
			   List<StatisticRecommendationByUnitDetailGetAllOnPage> mrrRecommendationByUnit = await new StatisticRecommendationByUnitDetailGetAllOnPage(_appSetting).StatisticRecommendationByUnitDetailGetAllOnPageDAO(UnitId,UnitProcessId, UserProcessId, Code, CreateName, Title, Field, Status, FromDate,ToDate, PageSize,PageIndex);
				
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"RecommendationsByUnitDetailGetAllOnPage", mrrRecommendationByUnit},
						{"TotalCount", mrrRecommendationByUnit != null && mrrRecommendationByUnit.Count > 0 ? mrrRecommendationByUnit[0].RowNumber : 0},
						{"PageIndex", mrrRecommendationByUnit != null && mrrRecommendationByUnit.Count > 0 ? PageIndex : 0},
						{"PageSize", mrrRecommendationByUnit != null && mrrRecommendationByUnit.Count > 0 ? PageSize : 0},
					};
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				//_bugsnag.Notify(ex);
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// tk pakn theo từ ngữ 
		/// </summary>
		/// <param name="LtsUnitId"></param>
		/// <param name="FromDate"></param>
		/// <param name="ToDate"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("recommendation-by-group-word")]
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
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				//_bugsnag.Notify(ex);
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// chi tiết pakn theo nhóm từ ngữ
		/// </summary>
		/// <param name="Code"></param>
		/// <param name="SendName"></param>
		/// <param name="Title"></param>
		/// <param name="Content"></param>
		/// <param name="UnitId"></param>
		/// <param name="GroupWordId"></param>
		/// <param name="FromDate"></param>
		/// <param name="ToDate"></param>
		/// <param name="PageSize"></param>
		/// <param name="PageIndex"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("recommendation-by-group-word-detail")]
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
				//_bugsnag.Notify(ex);
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// tk pakn theo lĩnh vực
		/// </summary>
		/// <param name="LtsUnitId"></param>
		/// <param name="FromDate"></param>
		/// <param name="ToDate"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("recommendation-by-field")]
		public async Task<ActionResult<object>> STT_RecommendationByFieldGetAllOnPageBase(string LtsUnitId, DateTime? FromDate, DateTime? ToDate)
		{
			try
			{
				int UnitProcessId = new LogHelper(_appSetting).GetUnitIdFromRequest(HttpContext);
				long UserProcessId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
				List<StatisticRecommendationByFieldGetAllOnPage> mrrRecommendationByField = await new StatisticRecommendationByFieldGetAllOnPage(_appSetting).StatisticRecommendationByFieldGetAllOnPageDAO(LtsUnitId, UnitProcessId, UserProcessId, FromDate, ToDate);
				

				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"StatisticRecommendationByFieldGetAllOnPage", mrrRecommendationByField}
					};
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				//_bugsnag.Notify(ex);
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// danh sách pakn theo lĩnh vực
		/// </summary>
		/// <param name="FiledId"></param>
		/// <param name="Code"></param>
		/// <param name="CreateName"></param>
		/// <param name="Title"></param>
		/// <param name="LstUnitId"></param>
		/// <param name="Status"></param>
		/// <param name="FromDate"></param>
		/// <param name="ToDate"></param>
		/// <param name="PageSize"></param>
		/// <param name="PageIndex"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("recommendation-by-field-detail")]
		public async Task<ActionResult<object>> RecommendationsByFieldDetailGetAllOnPageBase(int FiledId, string Code, string CreateName, string Title, string? LstUnitId, int? Status, DateTime? FromDate, DateTime? ToDate, int? PageSize, int? PageIndex)
		{
			try
			{
				int UnitProcessId = new LogHelper(_appSetting).GetUnitIdFromRequest(HttpContext);
				long UserProcessId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
				List<StatisticRecommendationByFiledDetailGetAllOnPage> mrrRecommendationByField = await new StatisticRecommendationByFiledDetailGetAllOnPage(_appSetting).StatisticRecommendationByFieldDetailGetAllOnPageDAO(FiledId,UnitProcessId, UserProcessId, Code, CreateName, Title, LstUnitId, Status, FromDate, ToDate, PageSize, PageIndex);

				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"RecommendationsByFieldDetailGetAllOnPage", mrrRecommendationByField},
						{"TotalCount", mrrRecommendationByField != null && mrrRecommendationByField.Count > 0 ? mrrRecommendationByField[0].RowNumber : 0},
						{"PageIndex", mrrRecommendationByField != null && mrrRecommendationByField.Count > 0 ? PageIndex : 0},
						{"PageSize", mrrRecommendationByField != null && mrrRecommendationByField.Count > 0 ? PageSize : 0},
					};
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				//_bugsnag.Notify(ex);
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}


		/// <summary>
		/// thống kê tổng hợp trang công bố
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Route("recommendation-statistic-by-unit-parent")]
		public async Task<ActionResult<object>> RecommendationStatisticByUnitParent(int? ParentId = 0)
		{
			try
			{
				List<StatisticByByUnitParent> statisticByByUnitParent = await new StatisticByByUnitParent(_appSetting).StatisticByUnitParentDAO(ParentId);

				return new ResultApi { Success = ResultCode.OK, Result = statisticByByUnitParent };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, ex);
				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}


		/// <summary>
		/// thống kê tổng hợp cho biểu đồ
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Route("recommendation-statistic-for-chart")]
		public async Task<ActionResult<object>> RecommendationStatisticForChart()
		{
			try
			{
				List<StatisticByByUnitParent> statisticByUnitParent = await new StatisticByByUnitParent(_appSetting).StatisticByUnitParentDAO(0);

				
				var titles = new List<string>();
				var itemObjectResponse = new RecommendationStatisticForChart();
				var values = new List<RecommendationStatisticForChart>();


				for (int i = 0; i < statisticByUnitParent.Count; i++)
				{
					titles.Add(statisticByUnitParent[i].UnitName);
					switch (i)
					{
						case 0:
							// tổng
							values.Add(new RecommendationStatisticForChart("Tổng", statisticByUnitParent.Select(x => x.TotalResult).ToList()));
							break;
						case 1:
							values.Add(new RecommendationStatisticForChart("Đã xử lý", statisticByUnitParent.Select(x => x.Finised).ToList()));
							break;
						case 2:
							values.Add(new RecommendationStatisticForChart("Đang xử lý", statisticByUnitParent.Select(x => x.Processing).ToList()));
							break;
						case 3:
							values.Add(new RecommendationStatisticForChart("Quá hạn", statisticByUnitParent.Select(x => x.Expired).ToList()));
							break;
						default: 
							break;
					}

				}



				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"Titles", titles },
						{"Values", values },
						{"statisticForChart",statisticByUnitParent }
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, ex);
				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}


		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("recommendation-processing-status")]
		public async Task<ActionResult<object>> RecommendationProcessStatus(DateTime? FromDate , DateTime? ToDate, int? PageSize, int? PageIndex)
		{
			try
			{
				var unitId = new LogHelper(_appSetting).GetUnitIdFromRequest(HttpContext);
				List<StatisticRecommendationProcessStatus> result = await new StatisticRecommendationProcessStatus(_appSetting).StatisticRecommendationProcessStatusDAO(FromDate,ToDate, unitId,PageSize, PageIndex);

				return new ResultApi { Success = ResultCode.OK, Result = result };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, ex);
				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("recommendation-processing-results")]
		public async Task<ActionResult<object>> RecommendationProcessResults(DateTime? FromDate, DateTime? ToDate)
		{
			try
			{
				var unitId = new LogHelper(_appSetting).GetUnitIdFromRequest(HttpContext);
				List<StatisticRecommendationProcessResults> result = await new StatisticRecommendationProcessResults(_appSetting).StatisticRecommendationProcessResultsDAO(FromDate, ToDate, unitId);

				return new ResultApi { Success = ResultCode.OK, Result = result };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, ex);
				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}


	}
}
