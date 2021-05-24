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
		[Route("PURecommendationAllOnPage")]
		public async Task<ActionResult<object>> PURecommendationAllOnPage(string? KeySearch, int Status, int PageSize, int PageIndex)
		{
			try
			{
				var rsPURecommendationOnPage = await new PURecommendation(_appSetting).PURecommendationAllOnPage(KeySearch, Status, PageSize, PageIndex);
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
		[Route("MyRecommendationAllOnPage")]
		public async Task<ActionResult<object>> MyRecommendationAllOnPage(int? userId ,string LtsStatus, int PageSize, int PageIndex)
		{
			try
			{
				var rsMyRecommendationOnPage = await new PURecommendation(_appSetting).MyRecommendationAllOnPage(userId, LtsStatus, PageSize, PageIndex);
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


		[HttpGet]
		[Authorize]
		[Route("PURecommendationStatisticsGetByUserIdBase")]
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}


		#region PURecommendationGetListOrderByCountClick
		[HttpGet]
		[Route("PURecommendationGetListOrderByCountClick")]
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

		#region
		[HttpGet]
		[Route("PURecommendationCountClick")]
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

		[HttpGet]
		[Route("PURecommendationGetById")]
		public async Task<ActionResult<object>> PURecommendationGetById(int? Id, int?Status)
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

		[HttpGet]
		[Route("ChangeSatisfaction")]
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion ChangeSatisfaction



	}
}
