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
	[Route("api/MRTableBase")]
	[ApiController]
	public class MRTableBaseController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		private readonly IClient _bugsnag;

		public MRTableBaseController(IAppSetting appSetting, IClient bugsnag)
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

		#region MRRecommendation

		[HttpGet]
		[Authorize]
		[Route("MRRecommendationGetByID")]
		public async Task<ActionResult<object>> MRRecommendationGetByID(int? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendation(_appSetting).MRRecommendationGetByID(Id) };
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
		[Route("MRRecommendationGetAll")]
		public async Task<ActionResult<object>> MRRecommendationGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendation(_appSetting).MRRecommendationGetAll() };
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
		[Route("MRRecommendationGetAllOnPage")]
		public async Task<ActionResult<object>> MRRecommendationGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<MRRecommendationOnPage> rsMRRecommendationOnPage = await new MRRecommendation(_appSetting).MRRecommendationGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"MRRecommendation", rsMRRecommendationOnPage},
						{"TotalCount", rsMRRecommendationOnPage != null && rsMRRecommendationOnPage.Count > 0 ? rsMRRecommendationOnPage[0].RowNumber : 0},
						{"PageIndex", rsMRRecommendationOnPage != null && rsMRRecommendationOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsMRRecommendationOnPage != null && rsMRRecommendationOnPage.Count > 0 ? PageSize : 0},
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
		[Route("MRRecommendationInsert")]
		public async Task<ActionResult<object>> MRRecommendationInsert(MRRecommendation _mRRecommendation)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendation(_appSetting).MRRecommendationInsert(_mRRecommendation) };
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
		[Route("MRRecommendationListInsert")]
		public async Task<ActionResult<object>> MRRecommendationListInsert(List<MRRecommendation> _mRRecommendations)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (MRRecommendation _mRRecommendation in _mRRecommendations)
				{
					int? result = await new MRRecommendation(_appSetting).MRRecommendationInsert(_mRRecommendation);
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
		[Route("MRRecommendationUpdate")]
		public async Task<ActionResult<object>> MRRecommendationUpdate(MRRecommendation _mRRecommendation)
		{
			try
			{
				int count = await new MRRecommendation(_appSetting).MRRecommendationUpdate(_mRRecommendation);
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
		[Route("MRRecommendationDelete")]
		public async Task<ActionResult<object>> MRRecommendationDelete(MRRecommendation _mRRecommendation)
		{
			try
			{
				int count = await new MRRecommendation(_appSetting).MRRecommendationDelete(_mRRecommendation);
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
		[Route("MRRecommendationListDelete")]
		public async Task<ActionResult<object>> MRRecommendationListDelete(List<MRRecommendation> _mRRecommendations)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (MRRecommendation _mRRecommendation in _mRRecommendations)
				{
					var result = await new MRRecommendation(_appSetting).MRRecommendationDelete(_mRRecommendation);
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
		[Route("MRRecommendationDeleteAll")]
		public async Task<ActionResult<object>> MRRecommendationDeleteAll()
		{
			try
			{
				int count = await new MRRecommendation(_appSetting).MRRecommendationDeleteAll();
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
		[Route("MRRecommendationCount")]
		public async Task<ActionResult<object>> MRRecommendationCount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendation(_appSetting).MRRecommendationCount() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion MRRecommendation

		#region MRRecommendationConclusion

		[HttpGet]
		[Authorize]
		[Route("MRRecommendationConclusionGetByID")]
		public async Task<ActionResult<object>> MRRecommendationConclusionGetByID(int? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationConclusion(_appSetting).MRRecommendationConclusionGetByID(Id) };
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
		[Route("MRRecommendationConclusionGetAll")]
		public async Task<ActionResult<object>> MRRecommendationConclusionGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationConclusion(_appSetting).MRRecommendationConclusionGetAll() };
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
		[Route("MRRecommendationConclusionGetAllOnPage")]
		public async Task<ActionResult<object>> MRRecommendationConclusionGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<MRRecommendationConclusionOnPage> rsMRRecommendationConclusionOnPage = await new MRRecommendationConclusion(_appSetting).MRRecommendationConclusionGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"MRRecommendationConclusion", rsMRRecommendationConclusionOnPage},
						{"TotalCount", rsMRRecommendationConclusionOnPage != null && rsMRRecommendationConclusionOnPage.Count > 0 ? rsMRRecommendationConclusionOnPage[0].RowNumber : 0},
						{"PageIndex", rsMRRecommendationConclusionOnPage != null && rsMRRecommendationConclusionOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsMRRecommendationConclusionOnPage != null && rsMRRecommendationConclusionOnPage.Count > 0 ? PageSize : 0},
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
		[Route("MRRecommendationConclusionInsert")]
		public async Task<ActionResult<object>> MRRecommendationConclusionInsert(MRRecommendationConclusion _mRRecommendationConclusion)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationConclusion(_appSetting).MRRecommendationConclusionInsert(_mRRecommendationConclusion) };
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
		[Route("MRRecommendationConclusionListInsert")]
		public async Task<ActionResult<object>> MRRecommendationConclusionListInsert(List<MRRecommendationConclusion> _mRRecommendationConclusions)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (MRRecommendationConclusion _mRRecommendationConclusion in _mRRecommendationConclusions)
				{
					int? result = await new MRRecommendationConclusion(_appSetting).MRRecommendationConclusionInsert(_mRRecommendationConclusion);
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
		[Route("MRRecommendationConclusionUpdate")]
		public async Task<ActionResult<object>> MRRecommendationConclusionUpdate(MRRecommendationConclusion _mRRecommendationConclusion)
		{
			try
			{
				int count = await new MRRecommendationConclusion(_appSetting).MRRecommendationConclusionUpdate(_mRRecommendationConclusion);
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
		[Route("MRRecommendationConclusionDelete")]
		public async Task<ActionResult<object>> MRRecommendationConclusionDelete(MRRecommendationConclusion _mRRecommendationConclusion)
		{
			try
			{
				int count = await new MRRecommendationConclusion(_appSetting).MRRecommendationConclusionDelete(_mRRecommendationConclusion);
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
		[Route("MRRecommendationConclusionListDelete")]
		public async Task<ActionResult<object>> MRRecommendationConclusionListDelete(List<MRRecommendationConclusion> _mRRecommendationConclusions)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (MRRecommendationConclusion _mRRecommendationConclusion in _mRRecommendationConclusions)
				{
					var result = await new MRRecommendationConclusion(_appSetting).MRRecommendationConclusionDelete(_mRRecommendationConclusion);
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
		[Route("MRRecommendationConclusionDeleteAll")]
		public async Task<ActionResult<object>> MRRecommendationConclusionDeleteAll()
		{
			try
			{
				int count = await new MRRecommendationConclusion(_appSetting).MRRecommendationConclusionDeleteAll();
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
		[Route("MRRecommendationConclusionCount")]
		public async Task<ActionResult<object>> MRRecommendationConclusionCount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationConclusion(_appSetting).MRRecommendationConclusionCount() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion MRRecommendationConclusion

		#region MRRecommendationConclusionFiles

		[HttpGet]
		[Authorize]
		[Route("MRRecommendationConclusionFilesGetByID")]
		public async Task<ActionResult<object>> MRRecommendationConclusionFilesGetByID(int? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationConclusionFiles(_appSetting).MRRecommendationConclusionFilesGetByID(Id) };
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
		[Route("MRRecommendationConclusionFilesGetAll")]
		public async Task<ActionResult<object>> MRRecommendationConclusionFilesGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationConclusionFiles(_appSetting).MRRecommendationConclusionFilesGetAll() };
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
		[Route("MRRecommendationConclusionFilesGetAllOnPage")]
		public async Task<ActionResult<object>> MRRecommendationConclusionFilesGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<MRRecommendationConclusionFilesOnPage> rsMRRecommendationConclusionFilesOnPage = await new MRRecommendationConclusionFiles(_appSetting).MRRecommendationConclusionFilesGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"MRRecommendationConclusionFiles", rsMRRecommendationConclusionFilesOnPage},
						{"TotalCount", rsMRRecommendationConclusionFilesOnPage != null && rsMRRecommendationConclusionFilesOnPage.Count > 0 ? rsMRRecommendationConclusionFilesOnPage[0].RowNumber : 0},
						{"PageIndex", rsMRRecommendationConclusionFilesOnPage != null && rsMRRecommendationConclusionFilesOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsMRRecommendationConclusionFilesOnPage != null && rsMRRecommendationConclusionFilesOnPage.Count > 0 ? PageSize : 0},
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
		[Route("MRRecommendationConclusionFilesInsert")]
		public async Task<ActionResult<object>> MRRecommendationConclusionFilesInsert(MRRecommendationConclusionFiles _mRRecommendationConclusionFiles)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationConclusionFiles(_appSetting).MRRecommendationConclusionFilesInsert(_mRRecommendationConclusionFiles) };
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
		[Route("MRRecommendationConclusionFilesListInsert")]
		public async Task<ActionResult<object>> MRRecommendationConclusionFilesListInsert(List<MRRecommendationConclusionFiles> _mRRecommendationConclusionFiless)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (MRRecommendationConclusionFiles _mRRecommendationConclusionFiles in _mRRecommendationConclusionFiless)
				{
					int? result = await new MRRecommendationConclusionFiles(_appSetting).MRRecommendationConclusionFilesInsert(_mRRecommendationConclusionFiles);
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
		[Route("MRRecommendationConclusionFilesUpdate")]
		public async Task<ActionResult<object>> MRRecommendationConclusionFilesUpdate(MRRecommendationConclusionFiles _mRRecommendationConclusionFiles)
		{
			try
			{
				int count = await new MRRecommendationConclusionFiles(_appSetting).MRRecommendationConclusionFilesUpdate(_mRRecommendationConclusionFiles);
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
		[Route("MRRecommendationConclusionFilesDelete")]
		public async Task<ActionResult<object>> MRRecommendationConclusionFilesDelete(MRRecommendationConclusionFiles _mRRecommendationConclusionFiles)
		{
			try
			{
				int count = await new MRRecommendationConclusionFiles(_appSetting).MRRecommendationConclusionFilesDelete(_mRRecommendationConclusionFiles);
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
		[Route("MRRecommendationConclusionFilesListDelete")]
		public async Task<ActionResult<object>> MRRecommendationConclusionFilesListDelete(List<MRRecommendationConclusionFiles> _mRRecommendationConclusionFiless)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (MRRecommendationConclusionFiles _mRRecommendationConclusionFiles in _mRRecommendationConclusionFiless)
				{
					var result = await new MRRecommendationConclusionFiles(_appSetting).MRRecommendationConclusionFilesDelete(_mRRecommendationConclusionFiles);
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
		[Route("MRRecommendationConclusionFilesDeleteAll")]
		public async Task<ActionResult<object>> MRRecommendationConclusionFilesDeleteAll()
		{
			try
			{
				int count = await new MRRecommendationConclusionFiles(_appSetting).MRRecommendationConclusionFilesDeleteAll();
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
		[Route("MRRecommendationConclusionFilesCount")]
		public async Task<ActionResult<object>> MRRecommendationConclusionFilesCount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationConclusionFiles(_appSetting).MRRecommendationConclusionFilesCount() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion MRRecommendationConclusionFiles

		#region MRRecommendationFiles

		[HttpGet]
		[Authorize]
		[Route("MRRecommendationFilesGetByID")]
		public async Task<ActionResult<object>> MRRecommendationFilesGetByID(int? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationFiles(_appSetting).MRRecommendationFilesGetByID(Id) };
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
		[Route("MRRecommendationFilesGetAll")]
		public async Task<ActionResult<object>> MRRecommendationFilesGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationFiles(_appSetting).MRRecommendationFilesGetAll() };
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
		[Route("MRRecommendationFilesGetAllOnPage")]
		public async Task<ActionResult<object>> MRRecommendationFilesGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<MRRecommendationFilesOnPage> rsMRRecommendationFilesOnPage = await new MRRecommendationFiles(_appSetting).MRRecommendationFilesGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"MRRecommendationFiles", rsMRRecommendationFilesOnPage},
						{"TotalCount", rsMRRecommendationFilesOnPage != null && rsMRRecommendationFilesOnPage.Count > 0 ? rsMRRecommendationFilesOnPage[0].RowNumber : 0},
						{"PageIndex", rsMRRecommendationFilesOnPage != null && rsMRRecommendationFilesOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsMRRecommendationFilesOnPage != null && rsMRRecommendationFilesOnPage.Count > 0 ? PageSize : 0},
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
		[Route("MRRecommendationFilesInsert")]
		public async Task<ActionResult<object>> MRRecommendationFilesInsert(MRRecommendationFiles _mRRecommendationFiles)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationFiles(_appSetting).MRRecommendationFilesInsert(_mRRecommendationFiles) };
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
		[Route("MRRecommendationFilesListInsert")]
		public async Task<ActionResult<object>> MRRecommendationFilesListInsert(List<MRRecommendationFiles> _mRRecommendationFiless)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (MRRecommendationFiles _mRRecommendationFiles in _mRRecommendationFiless)
				{
					int? result = await new MRRecommendationFiles(_appSetting).MRRecommendationFilesInsert(_mRRecommendationFiles);
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
		[Route("MRRecommendationFilesUpdate")]
		public async Task<ActionResult<object>> MRRecommendationFilesUpdate(MRRecommendationFiles _mRRecommendationFiles)
		{
			try
			{
				int count = await new MRRecommendationFiles(_appSetting).MRRecommendationFilesUpdate(_mRRecommendationFiles);
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
		[Route("MRRecommendationFilesDelete")]
		public async Task<ActionResult<object>> MRRecommendationFilesDelete(MRRecommendationFiles _mRRecommendationFiles)
		{
			try
			{
				int count = await new MRRecommendationFiles(_appSetting).MRRecommendationFilesDelete(_mRRecommendationFiles);
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
		[Route("MRRecommendationFilesListDelete")]
		public async Task<ActionResult<object>> MRRecommendationFilesListDelete(List<MRRecommendationFiles> _mRRecommendationFiless)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (MRRecommendationFiles _mRRecommendationFiles in _mRRecommendationFiless)
				{
					var result = await new MRRecommendationFiles(_appSetting).MRRecommendationFilesDelete(_mRRecommendationFiles);
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
		[Route("MRRecommendationFilesDeleteAll")]
		public async Task<ActionResult<object>> MRRecommendationFilesDeleteAll()
		{
			try
			{
				int count = await new MRRecommendationFiles(_appSetting).MRRecommendationFilesDeleteAll();
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
		[Route("MRRecommendationFilesCount")]
		public async Task<ActionResult<object>> MRRecommendationFilesCount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationFiles(_appSetting).MRRecommendationFilesCount() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion MRRecommendationFiles

		#region MRRecommendationForward

		[HttpGet]
		[Authorize]
		[Route("MRRecommendationForwardGetByID")]
		public async Task<ActionResult<object>> MRRecommendationForwardGetByID(int? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationForward(_appSetting).MRRecommendationForwardGetByID(Id) };
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
		[Route("MRRecommendationForwardGetAll")]
		public async Task<ActionResult<object>> MRRecommendationForwardGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationForward(_appSetting).MRRecommendationForwardGetAll() };
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
		[Route("MRRecommendationForwardGetAllOnPage")]
		public async Task<ActionResult<object>> MRRecommendationForwardGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<MRRecommendationForwardOnPage> rsMRRecommendationForwardOnPage = await new MRRecommendationForward(_appSetting).MRRecommendationForwardGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"MRRecommendationForward", rsMRRecommendationForwardOnPage},
						{"TotalCount", rsMRRecommendationForwardOnPage != null && rsMRRecommendationForwardOnPage.Count > 0 ? rsMRRecommendationForwardOnPage[0].RowNumber : 0},
						{"PageIndex", rsMRRecommendationForwardOnPage != null && rsMRRecommendationForwardOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsMRRecommendationForwardOnPage != null && rsMRRecommendationForwardOnPage.Count > 0 ? PageSize : 0},
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
		[Route("MRRecommendationForwardInsert")]
		public async Task<ActionResult<object>> MRRecommendationForwardInsert(MRRecommendationForward _mRRecommendationForward)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationForward(_appSetting).MRRecommendationForwardInsert(_mRRecommendationForward) };
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
		[Route("MRRecommendationForwardListInsert")]
		public async Task<ActionResult<object>> MRRecommendationForwardListInsert(List<MRRecommendationForward> _mRRecommendationForwards)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (MRRecommendationForward _mRRecommendationForward in _mRRecommendationForwards)
				{
					int? result = await new MRRecommendationForward(_appSetting).MRRecommendationForwardInsert(_mRRecommendationForward);
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
		[Route("MRRecommendationForwardUpdate")]
		public async Task<ActionResult<object>> MRRecommendationForwardUpdate(MRRecommendationForward _mRRecommendationForward)
		{
			try
			{
				int count = await new MRRecommendationForward(_appSetting).MRRecommendationForwardUpdate(_mRRecommendationForward);
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
		[Route("MRRecommendationForwardDelete")]
		public async Task<ActionResult<object>> MRRecommendationForwardDelete(MRRecommendationForward _mRRecommendationForward)
		{
			try
			{
				int count = await new MRRecommendationForward(_appSetting).MRRecommendationForwardDelete(_mRRecommendationForward);
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
		[Route("MRRecommendationForwardListDelete")]
		public async Task<ActionResult<object>> MRRecommendationForwardListDelete(List<MRRecommendationForward> _mRRecommendationForwards)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (MRRecommendationForward _mRRecommendationForward in _mRRecommendationForwards)
				{
					var result = await new MRRecommendationForward(_appSetting).MRRecommendationForwardDelete(_mRRecommendationForward);
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
		[Route("MRRecommendationForwardDeleteAll")]
		public async Task<ActionResult<object>> MRRecommendationForwardDeleteAll()
		{
			try
			{
				int count = await new MRRecommendationForward(_appSetting).MRRecommendationForwardDeleteAll();
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
		[Route("MRRecommendationForwardCount")]
		public async Task<ActionResult<object>> MRRecommendationForwardCount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationForward(_appSetting).MRRecommendationForwardCount() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion MRRecommendationForward

		#region MRRecommendationGenCode

		[HttpGet]
		[Authorize]
		[Route("MRRecommendationGenCodeGetByID")]
		public async Task<ActionResult<object>> MRRecommendationGenCodeGetByID(int? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationGenCode(_appSetting).MRRecommendationGenCodeGetByID(Id) };
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
		[Route("MRRecommendationGenCodeGetAll")]
		public async Task<ActionResult<object>> MRRecommendationGenCodeGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationGenCode(_appSetting).MRRecommendationGenCodeGetAll() };
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
		[Route("MRRecommendationGenCodeGetAllOnPage")]
		public async Task<ActionResult<object>> MRRecommendationGenCodeGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<MRRecommendationGenCodeOnPage> rsMRRecommendationGenCodeOnPage = await new MRRecommendationGenCode(_appSetting).MRRecommendationGenCodeGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"MRRecommendationGenCode", rsMRRecommendationGenCodeOnPage},
						{"TotalCount", rsMRRecommendationGenCodeOnPage != null && rsMRRecommendationGenCodeOnPage.Count > 0 ? rsMRRecommendationGenCodeOnPage[0].RowNumber : 0},
						{"PageIndex", rsMRRecommendationGenCodeOnPage != null && rsMRRecommendationGenCodeOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsMRRecommendationGenCodeOnPage != null && rsMRRecommendationGenCodeOnPage.Count > 0 ? PageSize : 0},
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
		[Route("MRRecommendationGenCodeInsert")]
		public async Task<ActionResult<object>> MRRecommendationGenCodeInsert(MRRecommendationGenCode _mRRecommendationGenCode)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationGenCode(_appSetting).MRRecommendationGenCodeInsert(_mRRecommendationGenCode) };
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
		[Route("MRRecommendationGenCodeListInsert")]
		public async Task<ActionResult<object>> MRRecommendationGenCodeListInsert(List<MRRecommendationGenCode> _mRRecommendationGenCodes)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (MRRecommendationGenCode _mRRecommendationGenCode in _mRRecommendationGenCodes)
				{
					int? result = await new MRRecommendationGenCode(_appSetting).MRRecommendationGenCodeInsert(_mRRecommendationGenCode);
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
		[Route("MRRecommendationGenCodeUpdate")]
		public async Task<ActionResult<object>> MRRecommendationGenCodeUpdate(MRRecommendationGenCode _mRRecommendationGenCode)
		{
			try
			{
				int count = await new MRRecommendationGenCode(_appSetting).MRRecommendationGenCodeUpdate(_mRRecommendationGenCode);
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
		[Route("MRRecommendationGenCodeDelete")]
		public async Task<ActionResult<object>> MRRecommendationGenCodeDelete(MRRecommendationGenCode _mRRecommendationGenCode)
		{
			try
			{
				int count = await new MRRecommendationGenCode(_appSetting).MRRecommendationGenCodeDelete(_mRRecommendationGenCode);
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
		[Route("MRRecommendationGenCodeListDelete")]
		public async Task<ActionResult<object>> MRRecommendationGenCodeListDelete(List<MRRecommendationGenCode> _mRRecommendationGenCodes)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (MRRecommendationGenCode _mRRecommendationGenCode in _mRRecommendationGenCodes)
				{
					var result = await new MRRecommendationGenCode(_appSetting).MRRecommendationGenCodeDelete(_mRRecommendationGenCode);
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
		[Route("MRRecommendationGenCodeDeleteAll")]
		public async Task<ActionResult<object>> MRRecommendationGenCodeDeleteAll()
		{
			try
			{
				int count = await new MRRecommendationGenCode(_appSetting).MRRecommendationGenCodeDeleteAll();
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
		[Route("MRRecommendationGenCodeCount")]
		public async Task<ActionResult<object>> MRRecommendationGenCodeCount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationGenCode(_appSetting).MRRecommendationGenCodeCount() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion MRRecommendationGenCode

		#region MRRecommendationHashtag

		[HttpGet]
		[Authorize]
		[Route("MRRecommendationHashtagGetByID")]
		public async Task<ActionResult<object>> MRRecommendationHashtagGetByID(long? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationHashtag(_appSetting).MRRecommendationHashtagGetByID(Id) };
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
		[Route("MRRecommendationHashtagGetAll")]
		public async Task<ActionResult<object>> MRRecommendationHashtagGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationHashtag(_appSetting).MRRecommendationHashtagGetAll() };
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
		[Route("MRRecommendationHashtagGetAllOnPage")]
		public async Task<ActionResult<object>> MRRecommendationHashtagGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<MRRecommendationHashtagOnPage> rsMRRecommendationHashtagOnPage = await new MRRecommendationHashtag(_appSetting).MRRecommendationHashtagGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"MRRecommendationHashtag", rsMRRecommendationHashtagOnPage},
						{"TotalCount", rsMRRecommendationHashtagOnPage != null && rsMRRecommendationHashtagOnPage.Count > 0 ? rsMRRecommendationHashtagOnPage[0].RowNumber : 0},
						{"PageIndex", rsMRRecommendationHashtagOnPage != null && rsMRRecommendationHashtagOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsMRRecommendationHashtagOnPage != null && rsMRRecommendationHashtagOnPage.Count > 0 ? PageSize : 0},
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
		[Route("MRRecommendationHashtagInsert")]
		public async Task<ActionResult<object>> MRRecommendationHashtagInsert(MRRecommendationHashtag _mRRecommendationHashtag)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationHashtag(_appSetting).MRRecommendationHashtagInsert(_mRRecommendationHashtag) };
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
		[Route("MRRecommendationHashtagListInsert")]
		public async Task<ActionResult<object>> MRRecommendationHashtagListInsert(List<MRRecommendationHashtag> _mRRecommendationHashtags)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (MRRecommendationHashtag _mRRecommendationHashtag in _mRRecommendationHashtags)
				{
					int? result = await new MRRecommendationHashtag(_appSetting).MRRecommendationHashtagInsert(_mRRecommendationHashtag);
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
		[Route("MRRecommendationHashtagUpdate")]
		public async Task<ActionResult<object>> MRRecommendationHashtagUpdate(MRRecommendationHashtag _mRRecommendationHashtag)
		{
			try
			{
				int count = await new MRRecommendationHashtag(_appSetting).MRRecommendationHashtagUpdate(_mRRecommendationHashtag);
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
		[Route("MRRecommendationHashtagDelete")]
		public async Task<ActionResult<object>> MRRecommendationHashtagDelete(MRRecommendationHashtag _mRRecommendationHashtag)
		{
			try
			{
				int count = await new MRRecommendationHashtag(_appSetting).MRRecommendationHashtagDelete(_mRRecommendationHashtag);
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
		[Route("MRRecommendationHashtagListDelete")]
		public async Task<ActionResult<object>> MRRecommendationHashtagListDelete(List<MRRecommendationHashtag> _mRRecommendationHashtags)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (MRRecommendationHashtag _mRRecommendationHashtag in _mRRecommendationHashtags)
				{
					var result = await new MRRecommendationHashtag(_appSetting).MRRecommendationHashtagDelete(_mRRecommendationHashtag);
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
		[Route("MRRecommendationHashtagDeleteAll")]
		public async Task<ActionResult<object>> MRRecommendationHashtagDeleteAll()
		{
			try
			{
				int count = await new MRRecommendationHashtag(_appSetting).MRRecommendationHashtagDeleteAll();
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
		[Route("MRRecommendationHashtagCount")]
		public async Task<ActionResult<object>> MRRecommendationHashtagCount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationHashtag(_appSetting).MRRecommendationHashtagCount() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion MRRecommendationHashtag
	}
}
