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
using PAKNAPI.Models.Statistic;

namespace PAKNAPI.ControllerBase
{
	[Route("api/pu-recommendation")]
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
		/// <summary>
		/// danh sách pakn đã giải quyết
		/// </summary>
		/// <param name="KeySearch"></param>
		/// <param name="FieldId"></param>
		/// <param name="UnitId"></param>
		/// <param name="PageSize"></param>
		/// <param name="PageIndex"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("get-list-recommentdation-on-page")]
		public async Task<ActionResult<object>> PURecommendationAllOnPage(string KeySearch, int? FieldId , int? UnitId, int PageSize, int PageIndex)
		{
			try
			{
				var rsPURecommendationOnPage = await new PURecommendation(_appSetting).PURecommendationAllOnPage(KeySearch, FieldId, UnitId, PageSize, PageIndex);
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
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// danh sách pakn đã giải quyết trang chủ group by field
		/// </summary>
		/// <returns></returns>


		[HttpGet]
		[Route("get-list-recommentdation-home-page")]
		public async Task<ActionResult<object>> PURecommendationHomePage()
		{
			try
			{
				IDictionary<string, object> json = new Dictionary<string, object>{ };
				// check setting
				SYConfig rsSYConfigGetByType = (await new SYConfig(_appSetting).SYConfigGetByTypeDAO(TYPECONFIG.VIEWHOME)).FirstOrDefault();

				if (rsSYConfigGetByType == null || rsSYConfigGetByType.Content == "1") {

					var rsPURecommendationOnPage = await new PURecommendation(_appSetting).PURecommendationAllOnPage("", null, null, 20, 1);
					json = new Dictionary<string, object>
					{
						{"IsHomeDefault", true},
						{"PURecommendation", rsPURecommendationOnPage},
					};
					return new ResultApi { Success = ResultCode.OK, Result = json };
				}

				// list filed show home
				List<CAFieldGetAllOnPage> lstFieldHome = await new CAFieldGetAllOnPage(_appSetting).CAFieldGetAllShowHome();

				List<RecommendationGroupByFieldResponse> result = new List<RecommendationGroupByFieldResponse>();

				lstFieldHome.ForEach((field) =>
				{
					var lstRecommendation = new PURecommendationByField(_appSetting).RecommendationGetByField(field.Id).Result;
					result.Add(new RecommendationGroupByFieldResponse(field.Id, field.Name, field.FilePath, lstRecommendation));
				});
				json = new Dictionary<string, object>
					{
						{"IsHomeDefault", false},
						{"PURecommendation", result},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		/// <summary>
		/// danh sách pakn của tôi
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="LtsStatus"></param>
		/// <param name="Title"></param>
		/// <param name="PageSize"></param>
		/// <param name="PageIndex"></param>
		/// <returns></returns>

		[HttpGet]
		[Route("get-list-my-recommentdation-on-page")]
		public async Task<ActionResult<object>> MyRecommendationAllOnPage(int? userId, string LtsStatus, string Title, int PageSize, int PageIndex)
		{
			try
			{
				var rsMyRecommendationOnPage = await new PURecommendation(_appSetting).MyRecommendationAllOnPage(userId, LtsStatus, Title, PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"MyRecommendation", rsMyRecommendationOnPage},
						{"TotalCount", rsMyRecommendationOnPage != null && rsMyRecommendationOnPage.Count > 0 ? rsMyRecommendationOnPage[0].RowNumber : 0},
						{"PageIndex", rsMyRecommendationOnPage != null && rsMyRecommendationOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsMyRecommendationOnPage != null && rsMyRecommendationOnPage.Count > 0 ? PageSize : 0},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}



		#endregion PURecommendationAllOnPage
		/// <summary>
		/// thống kê pakn của tôi
		/// </summary>
		/// <returns></returns>

		[HttpGet]
		[Route("recommendation-statistics-get-by-user-id")]
		public async Task<ActionResult<object>> PURecommendationStatisticsGetByUserIdBase()
		{
			try
			{
				var UserId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
				List<PURecommendationStatisticsGetByUserId> rsPURecommendationStatisticsGetByUserId = await new PURecommendationStatisticsGetByUserId(_appSetting).PURecommendationStatisticsGetByUserIdDAO((int)UserId);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"PURecommendationStatisticsGetByUserId", rsPURecommendationStatisticsGetByUserId},
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
		

		#region PURecommendationgetById
		/// <summary>
		/// chi tiết pakn trang công bố
		/// </summary>
		/// <param name="Id"></param>
		/// <param name="Status"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("get-by-id")]
		public async Task<ActionResult<object>> PURecommendationGetById(int? Id, int? Status)
		{
			try
			{
				Base64EncryptDecryptFile decrypt = new Base64EncryptDecryptFile();
				PURecommendationGetByIdViewResponse result = new PURecommendationGetByIdViewResponse();
				// detail
				long UserId = 0;
				try {
					UserId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
				}
				catch (Exception ex) {

				}

				result.Model = await new PURecommendation(_appSetting).PURecommendationGetById(Id, Status, UserId);

				// file đính kèm
				result.lstFiles = await new MRRecommendationFilesGetByRecommendationId(_appSetting).MRRecommendationFilesGetByRecommendationIdDAO(Id);
				foreach (var item in result.lstFiles)
				{
					item.FilePath = decrypt.EncryptData(item.FilePath);
				}
				var file = result.lstFiles.Where(x => x.FileType == 4).FirstOrDefault();
				if (file != null)
				{
					result.Model.FilePath = file.FilePathUrl;
				}
				// 

				// nội dung phản hồi
				result.lstConclusion = (await new MRRecommendationConclusionGetByRecommendationId(_appSetting).MRRecommendationConclusionGetByRecommendationIdDAO(Id)).ToList().FirstOrDefault();
				// file đính kèm nội dung phản hồi
				if (result.lstConclusion != null) {
					result.lstConclusionFiles = (await new MRRecommendationConclusionFilesGetByConclusionId(_appSetting).MRRecommendationConclusionFilesGetByConclusionIdDAO(result.lstConclusion.Id)).ToList();
					foreach (var item in result.lstConclusionFiles)
					{
						item.FilePath = decrypt.EncryptData(item.FilePath);
					}
				}
				return new ResultApi { Success = ResultCode.OK, Result = result };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion PURecommendationgetById

		#region ChangeSatisfaction
		/// <summary>
		/// cập nhập số lượng hài lòng, không hài lòng pakn
		/// </summary>
		/// <param name="RecommendationId"></param>
		/// <param name="Satisfaction"></param>
		/// <returns></returns>
		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("change-satisfaction")]
		public async Task<ActionResult<object>> PURecommendationSactifaction(long RecommendationId, int Satisfaction)
		{
			try
			{
				var userId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
				int? count = await new PURecommendation(_appSetting).PURecommendationSatisfationInsert(RecommendationId, userId, Satisfaction);

				if (count > 0)
				{
					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					return new ResultApi { Success = ResultCode.ORROR, Result = count };
				}
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion ChangeSatisfaction

		/// <summary>
		/// tk phản ánh kiến nghị toàn tỉnh
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Route("recommendation-statistic-by-province")]
		public async Task<ActionResult<object>> RecommendationStatisticByProvince()
		{
			try
			{
				List<PU_Statistic> statisticByProvinces = await new PU_Statistic(_appSetting).StatisticByProvinceDAO();
				var titles = new List<string>();
				var values = new List<int?>();
				statisticByProvinces.ForEach(item => {
					values.Add(item.Value);
					titles.Add(item.Title);
				});

				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"RecommendationStatisticByProvince", statisticByProvinces},
						{"Titles", titles },
						{"Values", values }
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
		/// <summary>
		/// danh sách PAKN bị từ chối tiếp nhận
		/// </summary>
		/// <param name="KeySearch"></param>
		/// <param name="FieldId"></param>
		/// <param name="UnitId"></param>
		/// <param name="PageSize"></param>
		/// <param name="PageIndex"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("get-list-recommentdation-receive-deny")]
		public async Task<ActionResult<object>> PURecommendationReceiveDeny(string KeySearch, int? FieldId, int? UnitId, int PageSize, int PageIndex)
		{
			try
			{
				var pURecommendation_ReceiveDeny = await new PURecommendation(_appSetting).PURecommendationReceiveDeny(KeySearch,FieldId, UnitId,PageSize,PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"RecommendationReceiveDeny", pURecommendation_ReceiveDeny},
						{"TotalCount", pURecommendation_ReceiveDeny != null && pURecommendation_ReceiveDeny.Count > 0 ? pURecommendation_ReceiveDeny[0].RowNumber : 0},
						{"PageIndex", pURecommendation_ReceiveDeny != null && pURecommendation_ReceiveDeny.Count > 0 ? PageIndex : 0},
						{"PageSize", pURecommendation_ReceiveDeny != null && pURecommendation_ReceiveDeny.Count > 0 ? PageSize : 0},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}


		/// <summary>
		/// danh sách PAKN dang xu ly
		/// </summary>
		/// <param name="KeySearch"></param>
		/// <param name="FieldId"></param>
		/// <param name="UnitId"></param>
		/// <param name="PageSize"></param>
		/// <param name="PageIndex"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("get-list-recommentdation-processing")]
		public async Task<ActionResult<object>> PURecommendationProcessing(string KeySearch, int? FieldId, int? UnitId, int PageSize, int PageIndex)
		{
			try
			{
				var pURecommendations = await new PURecommendation(_appSetting).PURecommendationProcessing(KeySearch, FieldId, UnitId, PageSize, PageIndex);


				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"RecommendationProcessing", pURecommendations},
						{"TotalCount", pURecommendations != null && pURecommendations.Count > 0 ? pURecommendations[0].RowNumber : 0},
						{"PageIndex", pURecommendations != null && pURecommendations.Count > 0 ? PageIndex : 0},
						{"PageSize", pURecommendations != null && pURecommendations.Count > 0 ? PageSize : 0},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpGet]
		[Route("recommendations-hight-light")]
		public async Task<ActionResult<object>> PURecommendationGetListOrderByCountClick(string KeySearch, int? FieldId, int? UnitId, int PageSize, int PageIndex)
		{
			try
			{
				var rsPURecommendationOnPage = await new PURecommendation(_appSetting).PURecommendationGetListOrderByCountClick(KeySearch, FieldId, UnitId, PageSize, PageIndex);
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
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpGet]
		[Route("unit-dissatisfaction-rate-on-page")]
		public async Task<ActionResult<object>> PUUnitDissatisfactionRateOnPage(string KeySearch , int? PageSize, int PageIndex)
		{
			try
			{
				var listUnit = await new UnitDissatisfactionRateOnPage(_appSetting).UnitDissatisfactionRateOnPageDAO(KeySearch, PageSize, PageIndex);

				IDictionary<string, object> json = new Dictionary<string, object>
				{
					{"listUnit", listUnit},
					{"TotalCount", listUnit != null && listUnit.Count > 0 ? listUnit[0].RowNumber : 0},
					{"PageIndex", listUnit != null && listUnit.Count > 0 ? PageIndex : 0},
					{"PageSize", listUnit != null && listUnit.Count > 0 ? PageSize : 0},
				};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpGet]
		[Route("late-processing-unit-on-page")]
		public async Task<ActionResult<object>> PULateProcessingUnitOnPage(string KeySearch, int? PageSize, int PageIndex)
		{
			try
			{
				var listUnit = await new LateProcessingUnitOnPage(_appSetting).LateProcessingUnitOnPageDAO(KeySearch, PageSize, PageIndex);

				IDictionary<string, object> json = new Dictionary<string, object>
				{
					{"listUnit", listUnit},
					{"TotalCount", listUnit != null && listUnit.Count > 0 ? listUnit[0].RowNumber : 0},
					{"PageIndex", listUnit != null && listUnit.Count > 0 ? PageIndex : 0},
					{"PageSize", listUnit != null && listUnit.Count > 0 ? PageSize : 0},
				};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		


		[HttpGet]
		[Route("notification-getdashboard")]
		public async Task<ActionResult<object>> PUNotificationGetDashboard()
		{
			try
			{
				var data = await new PURecommendation(_appSetting).PUNotificationGetDashboard();
				return new ResultApi { Success = ResultCode.OK, Result = data };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpGet]
		[Route("statistics-satisfaction-recommentdation")]
		public async Task<ActionResult<object>> PURecommendationStatisticsSatisfaction()
		{
			try
			{
				var statisticSatisfaction = await new PU_Statistic(_appSetting).StatisticSatisfactionDAO();
				var values = new List<int?>();
				statisticSatisfaction.ForEach(item => {
					values.Add(item.Value); 
				});
				var StatisticExpire = await new PU_Statistic(_appSetting).StatisticExpireDAO();

				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"RecommendationStatisticBySatisfaction", statisticSatisfaction},
						{"Values", values },
						{"Expire", StatisticExpire}
					};

				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}



	}
}
