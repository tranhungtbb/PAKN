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
	[Route("api/HITableBase")]
	[ApiController]
	public class HITableBaseController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		private readonly IClient _bugsnag;

		public HITableBaseController(IAppSetting appSetting, IClient bugsnag)
		{
			_appSetting = appSetting;
			_bugsnag = bugsnag;
		}

		#region HISRecommendation

		[HttpGet]
		[Authorize]
		[Route("HISRecommendationGetByID")]
		public async Task<ActionResult<object>> HISRecommendationGetByID(int? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new HISRecommendation(_appSetting).HISRecommendationGetByID(Id) };
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
		[Route("HISRecommendationGetAll")]
		public async Task<ActionResult<object>> HISRecommendationGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new HISRecommendation(_appSetting).HISRecommendationGetAll() };
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
		[Route("HISRecommendationGetAllOnPage")]
		public async Task<ActionResult<object>> HISRecommendationGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<HISRecommendationOnPage> rsHISRecommendationOnPage = await new HISRecommendation(_appSetting).HISRecommendationGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"HISRecommendation", rsHISRecommendationOnPage},
						{"TotalCount", rsHISRecommendationOnPage != null && rsHISRecommendationOnPage.Count > 0 ? rsHISRecommendationOnPage[0].RowNumber : 0},
						{"PageIndex", rsHISRecommendationOnPage != null && rsHISRecommendationOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsHISRecommendationOnPage != null && rsHISRecommendationOnPage.Count > 0 ? PageSize : 0},
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
		[Route("HISRecommendationInsert")]
		public async Task<ActionResult<object>> HISRecommendationInsert(HISRecommendation _hISRecommendation)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new HISRecommendation(_appSetting).HISRecommendationInsert(_hISRecommendation) };
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
		[Route("HISRecommendationListInsert")]
		public async Task<ActionResult<object>> HISRecommendationListInsert(List<HISRecommendation> _hISRecommendations)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (HISRecommendation _hISRecommendation in _hISRecommendations)
				{
					int? result = await new HISRecommendation(_appSetting).HISRecommendationInsert(_hISRecommendation);
					if (result != null)
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
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("HISRecommendationUpdate")]
		public async Task<ActionResult<object>> HISRecommendationUpdate(HISRecommendation _hISRecommendation)
		{
			try
			{
				int count = await new HISRecommendation(_appSetting).HISRecommendationUpdate(_hISRecommendation);
				if (count > 0)
				{
					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("HISRecommendationDelete")]
		public async Task<ActionResult<object>> HISRecommendationDelete(HISRecommendation _hISRecommendation)
		{
			try
			{
				int count = await new HISRecommendation(_appSetting).HISRecommendationDelete(_hISRecommendation);
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("HISRecommendationListDelete")]
		public async Task<ActionResult<object>> HISRecommendationListDelete(List<HISRecommendation> _hISRecommendations)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (HISRecommendation _hISRecommendation in _hISRecommendations)
				{
					var result = await new HISRecommendation(_appSetting).HISRecommendationDelete(_hISRecommendation);
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
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("HISRecommendationDeleteAll")]
		public async Task<ActionResult<object>> HISRecommendationDeleteAll()
		{
			try
			{
				int count = await new HISRecommendation(_appSetting).HISRecommendationDeleteAll();
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("HISRecommendationCount")]
		public async Task<ActionResult<object>> HISRecommendationCount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new HISRecommendation(_appSetting).HISRecommendationCount() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion HISRecommendation
	}
}
