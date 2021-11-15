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
		[Route("get-list-recommentdation-group-by-field")]
		public async Task<ActionResult<object>> PURecommendationAllOnPageByField()
		{
			try
			{
				// list filed show home
				List<CAFieldGetAllOnPage> lstFieldHome = await new CAFieldGetAllOnPage(_appSetting).CAFieldGetAllShowHome();

				List<RecommendationGroupByFieldResponse> result = new List<RecommendationGroupByFieldResponse>();

				foreach (var field in lstFieldHome) {
					var lstRecommendation =  await new PURecommendationByField(_appSetting).RecommendationGetByField(field.Id);
 					result.Add(new RecommendationGroupByFieldResponse(field.Id, field.Name, lstRecommendation));
				}
				IDictionary<string, object> json = new Dictionary<string, object>
					{
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
		/// <summary>
		/// pakn phổ biến
		/// </summary>
		/// <param name="Status"></param>
		/// <returns></returns>

		#region PURecommendationGetListOrderByCountClick
		[HttpGet]
		[Route("recommendation-get-list-order-by-count-click")]
		public async Task<ActionResult<object>> PURecommendationGetListOrderByCountClick(int? Status)
		{
			try
			{
				var rsPURecommendationOnPage = await new PURecommendation(_appSetting).PURecommendationGetListOrderByCountClick(Status);
				return new ResultApi { Success = ResultCode.OK, Result = rsPURecommendationOnPage.Take(3).ToList() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion PURecommendationGetListOrderByCountClick
		/// <summary>
		/// cập nhập lượt click pakn
		/// </summary>
		/// <param name="RecommendationId"></param>
		/// <returns></returns>
		#region
		[HttpGet]
		[Route("recommendation-count-click")]
		public async Task<ActionResult<object>> PURecommendationCountClick(int? RecommendationId)
		{
			try
			{
				int? count = await new PURecommendation(_appSetting).PURecommendationCountClick(RecommendationId);
				if (count > 0)
				{
					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else {
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
		#endregion



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
				result.Model = await new PURecommendation(_appSetting).PURecommendationGetById(Id, Status);
				// file đính kèm
				result.lstFiles = await new MRRecommendationFilesGetByRecommendationId(_appSetting).MRRecommendationFilesGetByRecommendationIdDAO(Id);
				foreach (var item in result.lstFiles)
				{
					item.FilePath = decrypt.EncryptData(item.FilePath);
				}
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
		[Route("change-satisfaction")]
		public async Task<object> ChangeSatisfaction(int? RecommendationId, bool? Satisfaction)
		{
			try {
				var result = await new PURecommendation(_appSetting).MR_RecommendationUpdateSatisfaction(RecommendationId, Satisfaction);
				if (result > 0)
				{
					return new ResultApi { Success = ResultCode.OK };
				}
				else {
					return new ResultApi { Success = ResultCode.ORROR, Message = "Error" };
				}
			}
			catch (Exception ex) {
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion ChangeSatisfaction
	}
}
