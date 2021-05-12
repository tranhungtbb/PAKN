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
	[Route("api/MRSPBase")]
	[ApiController]
	public class MRSPBaseController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		private readonly IClient _bugsnag;

		public MRSPBaseController(IAppSetting appSetting, IClient bugsnag)
		{
			_appSetting = appSetting;
			_bugsnag = bugsnag;
		}

		[HttpGet]
		[Authorize]
		[Route("HISRecommendationGetByObjectIdBase")]
		public async Task<ActionResult<object>> HISRecommendationGetByObjectIdBase(int? Id)
		{
			try
			{
				List<HISRecommendationGetByObjectId> rsHISRecommendationGetByObjectId = await new HISRecommendationGetByObjectId(_appSetting).HISRecommendationGetByObjectIdDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"HISRecommendationGetByObjectId", rsHISRecommendationGetByObjectId},
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

		[HttpPost]
		[Authorize]
		[Route("HISRecommendationInsertBase")]
		public async Task<ActionResult<object>> HISRecommendationInsertBase(HISRecommendationInsertIN _hISRecommendationInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new HISRecommendationInsert(_appSetting).HISRecommendationInsertDAO(_hISRecommendationInsertIN) };
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
		[Route("MRCommnentGetAllOnPageBase")]
		public async Task<ActionResult<object>> MRCommnentGetAllOnPageBase(int? PageSize, int? PageIndex, long? RecommendationId)
		{
			try
			{
				List<MRCommnentGetAllOnPage> rsMRCommnentGetAllOnPage = await new MRCommnentGetAllOnPage(_appSetting).MRCommnentGetAllOnPageDAO(PageSize, PageIndex, RecommendationId);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"MRCommnentGetAllOnPage", rsMRCommnentGetAllOnPage},
						{"TotalCount", rsMRCommnentGetAllOnPage != null && rsMRCommnentGetAllOnPage.Count > 0 ? rsMRCommnentGetAllOnPage[0].RowNumber : 0},
						{"PageIndex", rsMRCommnentGetAllOnPage != null && rsMRCommnentGetAllOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsMRCommnentGetAllOnPage != null && rsMRCommnentGetAllOnPage.Count > 0 ? PageSize : 0},
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

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("MRCommnentInsertBase")]
		public async Task<ActionResult<object>> MRCommnentInsertBase(MRCommnentInsertIN _mRCommnentInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRCommnentInsert(_appSetting).MRCommnentInsertDAO(_mRCommnentInsertIN) };
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
		[Route("MRRecommendationCheckExistedCodeBase")]
		public async Task<ActionResult<object>> MRRecommendationCheckExistedCodeBase(string Code)
		{
			try
			{
				List<MRRecommendationCheckExistedCode> rsMRRecommendationCheckExistedCode = await new MRRecommendationCheckExistedCode(_appSetting).MRRecommendationCheckExistedCodeDAO(Code);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"MRRecommendationCheckExistedCode", rsMRRecommendationCheckExistedCode},
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

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationConclusionFilesDeleteBase")]
		public async Task<ActionResult<object>> MRRecommendationConclusionFilesDeleteBase(MRRecommendationConclusionFilesDeleteIN _mRRecommendationConclusionFilesDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationConclusionFilesDelete(_appSetting).MRRecommendationConclusionFilesDeleteDAO(_mRRecommendationConclusionFilesDeleteIN) };
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
		[Route("MRRecommendationConclusionFilesGetByConclusionIdBase")]
		public async Task<ActionResult<object>> MRRecommendationConclusionFilesGetByConclusionIdBase(int? Id)
		{
			try
			{
				List<MRRecommendationConclusionFilesGetByConclusionId> rsMRRecommendationConclusionFilesGetByConclusionId = await new MRRecommendationConclusionFilesGetByConclusionId(_appSetting).MRRecommendationConclusionFilesGetByConclusionIdDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"MRRecommendationConclusionFilesGetByConclusionId", rsMRRecommendationConclusionFilesGetByConclusionId},
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

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationConclusionFilesInsertBase")]
		public async Task<ActionResult<object>> MRRecommendationConclusionFilesInsertBase(MRRecommendationConclusionFilesInsertIN _mRRecommendationConclusionFilesInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationConclusionFilesInsert(_appSetting).MRRecommendationConclusionFilesInsertDAO(_mRRecommendationConclusionFilesInsertIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationConclusionDeleteBase")]
		public async Task<ActionResult<object>> MRRecommendationConclusionDeleteBase(MRRecommendationConclusionDeleteIN _mRRecommendationConclusionDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationConclusionDelete(_appSetting).MRRecommendationConclusionDeleteDAO(_mRRecommendationConclusionDeleteIN) };
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
		[Route("MRRecommendationConclusionGetByRecommendationIdBase")]
		public async Task<ActionResult<object>> MRRecommendationConclusionGetByRecommendationIdBase(int? Id)
		{
			try
			{
				List<MRRecommendationConclusionGetByRecommendationId> rsMRRecommendationConclusionGetByRecommendationId = await new MRRecommendationConclusionGetByRecommendationId(_appSetting).MRRecommendationConclusionGetByRecommendationIdDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"MRRecommendationConclusionGetByRecommendationId", rsMRRecommendationConclusionGetByRecommendationId},
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

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationConclusionInsertBase")]
		public async Task<ActionResult<object>> MRRecommendationConclusionInsertBase(MRRecommendationConclusionInsertIN _mRRecommendationConclusionInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationConclusionInsert(_appSetting).MRRecommendationConclusionInsertDAO(_mRRecommendationConclusionInsertIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationFilesDeleteBase")]
		public async Task<ActionResult<object>> MRRecommendationFilesDeleteBase(MRRecommendationFilesDeleteIN _mRRecommendationFilesDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationFilesDelete(_appSetting).MRRecommendationFilesDeleteDAO(_mRRecommendationFilesDeleteIN) };
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
		[Route("MRRecommendationFilesGetByRecommendationIdBase")]
		public async Task<ActionResult<object>> MRRecommendationFilesGetByRecommendationIdBase(int? Id)
		{
			try
			{
				List<MRRecommendationFilesGetByRecommendationId> rsMRRecommendationFilesGetByRecommendationId = await new MRRecommendationFilesGetByRecommendationId(_appSetting).MRRecommendationFilesGetByRecommendationIdDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"MRRecommendationFilesGetByRecommendationId", rsMRRecommendationFilesGetByRecommendationId},
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

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationFilesInsertBase")]
		public async Task<ActionResult<object>> MRRecommendationFilesInsertBase(MRRecommendationFilesInsertIN _mRRecommendationFilesInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationFilesInsert(_appSetting).MRRecommendationFilesInsertDAO(_mRRecommendationFilesInsertIN) };
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
		[Route("MRRecommendationForwardGetByIDBase")]
		public async Task<ActionResult<object>> MRRecommendationForwardGetByIDBase(int? Id)
		{
			try
			{
				List<MRRecommendationForwardGetByID> rsMRRecommendationForwardGetByID = await new MRRecommendationForwardGetByID(_appSetting).MRRecommendationForwardGetByIDDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"MRRecommendationForwardGetByID", rsMRRecommendationForwardGetByID},
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

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationForwardInsertBase")]
		public async Task<ActionResult<object>> MRRecommendationForwardInsertBase(MRRecommendationForwardInsertIN _mRRecommendationForwardInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationForwardInsert(_appSetting).MRRecommendationForwardInsertDAO(_mRRecommendationForwardInsertIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationForwardProcessBase")]
		public async Task<ActionResult<object>> MRRecommendationForwardProcessBase(MRRecommendationForwardProcessIN _mRRecommendationForwardProcessIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationForwardProcess(_appSetting).MRRecommendationForwardProcessDAO(_mRRecommendationForwardProcessIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationForwardUpdateBase")]
		public async Task<ActionResult<object>> MRRecommendationForwardUpdateBase(MRRecommendationForwardUpdateIN _mRRecommendationForwardUpdateIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationForwardUpdate(_appSetting).MRRecommendationForwardUpdateDAO(_mRRecommendationForwardUpdateIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("MRRecommendationFullTextDeleteByRecommendationIdBase")]
		public async Task<ActionResult<object>> MRRecommendationFullTextDeleteByRecommendationIdBase(MRRecommendationFullTextDeleteByRecommendationIdIN _mRRecommendationFullTextDeleteByRecommendationIdIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationFullTextDeleteByRecommendationId(_appSetting).MRRecommendationFullTextDeleteByRecommendationIdDAO(_mRRecommendationFullTextDeleteByRecommendationIdIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("MRRecommendationFullTextInsertBase")]
		public async Task<ActionResult<object>> MRRecommendationFullTextInsertBase(MRRecommendationFullTextInsertIN _mRRecommendationFullTextInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationFullTextInsert(_appSetting).MRRecommendationFullTextInsertDAO(_mRRecommendationFullTextInsertIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationGenCodeGetCodeBase")]
		public async Task<ActionResult<object>> MRRecommendationGenCodeGetCodeBase()
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationGenCodeGetCode(_appSetting).MRRecommendationGenCodeGetCodeDAO() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationGenCodeUpdateNumberBase")]
		public async Task<ActionResult<object>> MRRecommendationGenCodeUpdateNumberBase()
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationGenCodeUpdateNumber(_appSetting).MRRecommendationGenCodeUpdateNumberDAO() };
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
		[Route("MRRecommendationGetSendUserDataGraphBase")]
		public async Task<ActionResult<object>> MRRecommendationGetSendUserDataGraphBase(long? SendId, DateTime? SendDateFrom, DateTime? SendDateTo)
		{
			try
			{
				List<MRRecommendationGetSendUserDataGraph> rsMRRecommendationGetSendUserDataGraph = await new MRRecommendationGetSendUserDataGraph(_appSetting).MRRecommendationGetSendUserDataGraphDAO(SendId, SendDateFrom, SendDateTo);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"MRRecommendationGetSendUserDataGraph", rsMRRecommendationGetSendUserDataGraph},
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
		[Route("MRRecommendationGetSuggestCreateBase")]
		public async Task<ActionResult<object>> MRRecommendationGetSuggestCreateBase(string Title)
		{
			try
			{
				List<MRRecommendationGetSuggestCreate> rsMRRecommendationGetSuggestCreate = await new MRRecommendationGetSuggestCreate(_appSetting).MRRecommendationGetSuggestCreateDAO(Title);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"MRRecommendationGetSuggestCreate", rsMRRecommendationGetSuggestCreate},
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
		[Route("MRRecommendationGetSuggestReplyBase")]
		public async Task<ActionResult<object>> MRRecommendationGetSuggestReplyBase(string ListIdHashtag, int? PageSize, int? PageIndex)
		{
			try
			{
				List<MRRecommendationGetSuggestReply> rsMRRecommendationGetSuggestReply = await new MRRecommendationGetSuggestReply(_appSetting).MRRecommendationGetSuggestReplyDAO(ListIdHashtag, PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"MRRecommendationGetSuggestReply", rsMRRecommendationGetSuggestReply},
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

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("MRRecommendationGroupWordInsertByListBase")]
		public async Task<ActionResult<object>> MRRecommendationGroupWordInsertByListBase(MRRecommendationGroupWordInsertByListIN _mRRecommendationGroupWordInsertByListIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationGroupWordInsertByList(_appSetting).MRRecommendationGroupWordInsertByListDAO(_mRRecommendationGroupWordInsertByListIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationHashtagDeleteByRecommendationIdBase")]
		public async Task<ActionResult<object>> MRRecommendationHashtagDeleteByRecommendationIdBase(MRRecommendationHashtagDeleteByRecommendationIdIN _mRRecommendationHashtagDeleteByRecommendationIdIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationHashtagDeleteByRecommendationId(_appSetting).MRRecommendationHashtagDeleteByRecommendationIdDAO(_mRRecommendationHashtagDeleteByRecommendationIdIN) };
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
		[Route("MRRecommendationHashtagGetByRecommendationIdBase")]
		public async Task<ActionResult<object>> MRRecommendationHashtagGetByRecommendationIdBase(long? Id)
		{
			try
			{
				List<MRRecommendationHashtagGetByRecommendationId> rsMRRecommendationHashtagGetByRecommendationId = await new MRRecommendationHashtagGetByRecommendationId(_appSetting).MRRecommendationHashtagGetByRecommendationIdDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"MRRecommendationHashtagGetByRecommendationId", rsMRRecommendationHashtagGetByRecommendationId},
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

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationHashtagInsertBase")]
		public async Task<ActionResult<object>> MRRecommendationHashtagInsertBase(MRRecommendationHashtagInsertIN _mRRecommendationHashtagInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationHashtagInsert(_appSetting).MRRecommendationHashtagInsertDAO(_mRRecommendationHashtagInsertIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationDeleteBase")]
		public async Task<ActionResult<object>> MRRecommendationDeleteBase(MRRecommendationDeleteIN _mRRecommendationDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationDelete(_appSetting).MRRecommendationDeleteDAO(_mRRecommendationDeleteIN) };
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
		[Route("MRRecommendationGetAllOnPageBase")]
		public async Task<ActionResult<object>> MRRecommendationGetAllOnPageBase(string Code, string SendName, string Content, int? UnitId, int? Field, int? Status, int? PageSize, int? PageIndex)
		{
			try
			{
				List<MRRecommendationGetAllOnPage> rsMRRecommendationGetAllOnPage = await new MRRecommendationGetAllOnPage(_appSetting).MRRecommendationGetAllOnPageDAO(Code, SendName, Content, UnitId, Field, Status, PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"MRRecommendationGetAllOnPage", rsMRRecommendationGetAllOnPage},
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
		[Route("MRRecommendationGetAllReactionaryWordBase")]
		public async Task<ActionResult<object>> MRRecommendationGetAllReactionaryWordBase(string Code, string SendName, string Content, int? UnitId, int? Field, int? Status, int? UnitProcessId, long? UserProcessId, int? PageSize, int? PageIndex)
		{
			try
			{
				List<MRRecommendationGetAllReactionaryWord> rsMRRecommendationGetAllReactionaryWord = await new MRRecommendationGetAllReactionaryWord(_appSetting).MRRecommendationGetAllReactionaryWordDAO(Code, SendName, Content, UnitId, Field, Status, UnitProcessId, UserProcessId, PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"MRRecommendationGetAllReactionaryWord", rsMRRecommendationGetAllReactionaryWord},
						{"TotalCount", rsMRRecommendationGetAllReactionaryWord != null && rsMRRecommendationGetAllReactionaryWord.Count > 0 ? rsMRRecommendationGetAllReactionaryWord[0].RowNumber : 0},
						{"PageIndex", rsMRRecommendationGetAllReactionaryWord != null && rsMRRecommendationGetAllReactionaryWord.Count > 0 ? PageIndex : 0},
						{"PageSize", rsMRRecommendationGetAllReactionaryWord != null && rsMRRecommendationGetAllReactionaryWord.Count > 0 ? PageSize : 0},
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
		[Route("MRRecommendationGetAllWithProcessBase")]
		public async Task<ActionResult<object>> MRRecommendationGetAllWithProcessBase(string Code, string SendName, string Content, int? UnitId, int? Field, int? Status, int? UnitProcessId, long? UserProcessId, int? PageSize, int? PageIndex)
		{
			try
			{
				List<MRRecommendationGetAllWithProcess> rsMRRecommendationGetAllWithProcess = await new MRRecommendationGetAllWithProcess(_appSetting).MRRecommendationGetAllWithProcessDAO(Code, SendName, Content, UnitId, Field, Status, UnitProcessId, UserProcessId, PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"MRRecommendationGetAllWithProcess", rsMRRecommendationGetAllWithProcess},
						{"TotalCount", rsMRRecommendationGetAllWithProcess != null && rsMRRecommendationGetAllWithProcess.Count > 0 ? rsMRRecommendationGetAllWithProcess[0].RowNumber : 0},
						{"PageIndex", rsMRRecommendationGetAllWithProcess != null && rsMRRecommendationGetAllWithProcess.Count > 0 ? PageIndex : 0},
						{"PageSize", rsMRRecommendationGetAllWithProcess != null && rsMRRecommendationGetAllWithProcess.Count > 0 ? PageSize : 0},
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
		[Route("MRRecommendationGetByHashtagAllOnPageBase")]
		public async Task<ActionResult<object>> MRRecommendationGetByHashtagAllOnPageBase(string Code, string SendName, string Title, string Content, int? Status, int? UnitId, int? HashtagId, int? PageSize, int? PageIndex)
		{
			try
			{
				List<MRRecommendationGetByHashtagAllOnPage> rsMRRecommendationGetByHashtagAllOnPage = await new MRRecommendationGetByHashtagAllOnPage(_appSetting).MRRecommendationGetByHashtagAllOnPageDAO(Code, SendName, Title, Content, Status, UnitId, HashtagId, PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"MRRecommendationGetByHashtagAllOnPage", rsMRRecommendationGetByHashtagAllOnPage},
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
		[Route("MRRecommendationGetByIDBase")]
		public async Task<ActionResult<object>> MRRecommendationGetByIDBase(int? Id)
		{
			try
			{
				List<MRRecommendationGetByID> rsMRRecommendationGetByID = await new MRRecommendationGetByID(_appSetting).MRRecommendationGetByIDDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"MRRecommendationGetByID", rsMRRecommendationGetByID},
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
		[Route("MRRecommendationGetByIDViewBase")]
		public async Task<ActionResult<object>> MRRecommendationGetByIDViewBase(int? Id)
		{
			try
			{
				List<MRRecommendationGetByIDView> rsMRRecommendationGetByIDView = await new MRRecommendationGetByIDView(_appSetting).MRRecommendationGetByIDViewDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"MRRecommendationGetByIDView", rsMRRecommendationGetByIDView},
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
		[Route("MRRecommendationGetDataGraphBase")]
		public async Task<ActionResult<object>> MRRecommendationGetDataGraphBase(int? UnitProcessId, long? UserProcessId)
		{
			try
			{
				List<MRRecommendationGetDataGraph> rsMRRecommendationGetDataGraph = await new MRRecommendationGetDataGraph(_appSetting).MRRecommendationGetDataGraphDAO(UnitProcessId, UserProcessId);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"MRRecommendationGetDataGraph", rsMRRecommendationGetDataGraph},
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

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationInsertBase")]
		public async Task<ActionResult<object>> MRRecommendationInsertBase(MRRecommendationInsertIN _mRRecommendationInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationInsert(_appSetting).MRRecommendationInsertDAO(_mRRecommendationInsertIN) };
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
		[Route("MRRecommendationKNCTCheckExistedIdBase")]
		public async Task<ActionResult<object>> MRRecommendationKNCTCheckExistedIdBase(int? RecommendationKNCTId)
		{
			try
			{
				List<MRRecommendationKNCTCheckExistedId> rsMRRecommendationKNCTCheckExistedId = await new MRRecommendationKNCTCheckExistedId(_appSetting).MRRecommendationKNCTCheckExistedIdDAO(RecommendationKNCTId);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"MRRecommendationKNCTCheckExistedId", rsMRRecommendationKNCTCheckExistedId},
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

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("MRRecommendationKNCTFilesDeleteBase")]
		public async Task<ActionResult<object>> MRRecommendationKNCTFilesDeleteBase(MRRecommendationKNCTFilesDeleteIN _mRRecommendationKNCTFilesDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationKNCTFilesDelete(_appSetting).MRRecommendationKNCTFilesDeleteDAO(_mRRecommendationKNCTFilesDeleteIN) };
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
		[Route("MRRecommendationKNCTFilesGetByRecommendationIdBase")]
		public async Task<ActionResult<object>> MRRecommendationKNCTFilesGetByRecommendationIdBase(int? Id)
		{
			try
			{
				List<MRRecommendationKNCTFilesGetByRecommendationId> rsMRRecommendationKNCTFilesGetByRecommendationId = await new MRRecommendationKNCTFilesGetByRecommendationId(_appSetting).MRRecommendationKNCTFilesGetByRecommendationIdDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"MRRecommendationKNCTFilesGetByRecommendationId", rsMRRecommendationKNCTFilesGetByRecommendationId},
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

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("MRRecommendationKNCTFilesInsertBase")]
		public async Task<ActionResult<object>> MRRecommendationKNCTFilesInsertBase(MRRecommendationKNCTFilesInsertIN _mRRecommendationKNCTFilesInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationKNCTFilesInsert(_appSetting).MRRecommendationKNCTFilesInsertDAO(_mRRecommendationKNCTFilesInsertIN) };
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
		[Route("MRRecommendationKNCTGetAllWithProcessBase")]
		public async Task<ActionResult<object>> MRRecommendationKNCTGetAllWithProcessBase(string Code, string Content, string Unit, string Place, int? Field, int? Status, int? PageSize, int? PageIndex)
		{
			try
			{
				List<MRRecommendationKNCTGetAllWithProcess> rsMRRecommendationKNCTGetAllWithProcess = await new MRRecommendationKNCTGetAllWithProcess(_appSetting).MRRecommendationKNCTGetAllWithProcessDAO(Code, Content, Unit, Place, Field, Status, PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"MRRecommendationKNCTGetAllWithProcess", rsMRRecommendationKNCTGetAllWithProcess},
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
		[Route("MRRecommendationKNCTGetByIdBase")]
		public async Task<ActionResult<object>> MRRecommendationKNCTGetByIdBase(int? Id)
		{
			try
			{
				List<MRRecommendationKNCTGetById> rsMRRecommendationKNCTGetById = await new MRRecommendationKNCTGetById(_appSetting).MRRecommendationKNCTGetByIdDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"MRRecommendationKNCTGetById", rsMRRecommendationKNCTGetById},
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

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("MRRecommendationKNCTInsertBase")]
		public async Task<ActionResult<object>> MRRecommendationKNCTInsertBase(MRRecommendationKNCTInsertIN _mRRecommendationKNCTInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationKNCTInsert(_appSetting).MRRecommendationKNCTInsertDAO(_mRRecommendationKNCTInsertIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("MRRecommendationKNCTUpdateBase")]
		public async Task<ActionResult<object>> MRRecommendationKNCTUpdateBase(MRRecommendationKNCTUpdateIN _mRRecommendationKNCTUpdateIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationKNCTUpdate(_appSetting).MRRecommendationKNCTUpdateDAO(_mRRecommendationKNCTUpdateIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationUpdateBase")]
		public async Task<ActionResult<object>> MRRecommendationUpdateBase(MRRecommendationUpdateIN _mRRecommendationUpdateIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationUpdate(_appSetting).MRRecommendationUpdateDAO(_mRRecommendationUpdateIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationUpdateReactionaryWordBase")]
		public async Task<ActionResult<object>> MRRecommendationUpdateReactionaryWordBase(MRRecommendationUpdateReactionaryWordIN _mRRecommendationUpdateReactionaryWordIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationUpdateReactionaryWord(_appSetting).MRRecommendationUpdateReactionaryWordDAO(_mRRecommendationUpdateReactionaryWordIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationUpdateStatusBase")]
		public async Task<ActionResult<object>> MRRecommendationUpdateStatusBase(MRRecommendationUpdateStatusIN _mRRecommendationUpdateStatusIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationUpdateStatus(_appSetting).MRRecommendationUpdateStatusDAO(_mRRecommendationUpdateStatusIN) };
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
