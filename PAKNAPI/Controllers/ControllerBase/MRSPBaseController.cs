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
	[Route("api/MRSPBase")]
	[ApiController]
	public class MRSPBaseController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		public MRSPBaseController(IAppSetting appSetting)
		{
			_appSetting = appSetting;
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("HISRecommendationInsertListBase")]
		public async Task<ActionResult<object>> HISRecommendationInsertListBase(List<HISRecommendationInsertIN> _hISRecommendationInsertINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _hISRecommendationInsertIN in _hISRecommendationInsertINs)
				{
					var result = await new HISRecommendationInsert(_appSetting).HISRecommendationInsertDAO(_hISRecommendationInsertIN);
					if (result > 0)
					{
						count++;
					}
					else
					{
						errcount++;
					}
				}

				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CountSuccess", count},
						{"CountError", errcount}
					};
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationDeleteListBase")]
		public async Task<ActionResult<object>> MRRecommendationDeleteListBase(List<MRRecommendationDeleteIN> _mRRecommendationDeleteINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _mRRecommendationDeleteIN in _mRRecommendationDeleteINs)
				{
					var result = await new MRRecommendationDelete(_appSetting).MRRecommendationDeleteDAO(_mRRecommendationDeleteIN);
					if (result > 0)
					{
						count++;
					}
					else
					{
						errcount++;
					}
				}

				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CountSuccess", count},
						{"CountError", errcount}
					};
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
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
						{"TotalCount", rsMRRecommendationGetAllOnPage != null && rsMRRecommendationGetAllOnPage.Count > 0 ? rsMRRecommendationGetAllOnPage[0].RowNumber : 0},
						{"PageIndex", rsMRRecommendationGetAllOnPage != null && rsMRRecommendationGetAllOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsMRRecommendationGetAllOnPage != null && rsMRRecommendationGetAllOnPage.Count > 0 ? PageSize : 0},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
	}
}
