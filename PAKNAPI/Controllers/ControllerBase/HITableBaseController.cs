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

		#region HISIndividual

		[HttpGet]
		[Authorize]
		[Route("HISIndividualGetByID")]
		public async Task<ActionResult<object>> HISIndividualGetByID(int? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new HISIndividual(_appSetting).HISIndividualGetByID(Id) };
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
		[Route("HISIndividualGetAll")]
		public async Task<ActionResult<object>> HISIndividualGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new HISIndividual(_appSetting).HISIndividualGetAll() };
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
		[Route("HISIndividualGetAllOnPage")]
		public async Task<ActionResult<object>> HISIndividualGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<HISIndividualOnPage> rsHISIndividualOnPage = await new HISIndividual(_appSetting).HISIndividualGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"HISIndividual", rsHISIndividualOnPage},
						{"TotalCount", rsHISIndividualOnPage != null && rsHISIndividualOnPage.Count > 0 ? rsHISIndividualOnPage[0].RowNumber : 0},
						{"PageIndex", rsHISIndividualOnPage != null && rsHISIndividualOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsHISIndividualOnPage != null && rsHISIndividualOnPage.Count > 0 ? PageSize : 0},
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
		[Route("HISIndividualInsert")]
		public async Task<ActionResult<object>> HISIndividualInsert(HISIndividual _hISIndividual)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new HISIndividual(_appSetting).HISIndividualInsert(_hISIndividual) };
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
		[Route("HISIndividualListInsert")]
		public async Task<ActionResult<object>> HISIndividualListInsert(List<HISIndividual> _hISIndividuals)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (HISIndividual _hISIndividual in _hISIndividuals)
				{
					int? result = await new HISIndividual(_appSetting).HISIndividualInsert(_hISIndividual);
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
		[Route("HISIndividualUpdate")]
		public async Task<ActionResult<object>> HISIndividualUpdate(HISIndividual _hISIndividual)
		{
			try
			{
				int count = await new HISIndividual(_appSetting).HISIndividualUpdate(_hISIndividual);
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
		[Route("HISIndividualDelete")]
		public async Task<ActionResult<object>> HISIndividualDelete(HISIndividual _hISIndividual)
		{
			try
			{
				int count = await new HISIndividual(_appSetting).HISIndividualDelete(_hISIndividual);
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
		[Route("HISIndividualListDelete")]
		public async Task<ActionResult<object>> HISIndividualListDelete(List<HISIndividual> _hISIndividuals)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (HISIndividual _hISIndividual in _hISIndividuals)
				{
					var result = await new HISIndividual(_appSetting).HISIndividualDelete(_hISIndividual);
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
		[Route("HISIndividualDeleteAll")]
		public async Task<ActionResult<object>> HISIndividualDeleteAll()
		{
			try
			{
				int count = await new HISIndividual(_appSetting).HISIndividualDeleteAll();
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
		[Route("HISIndividualCount")]
		public async Task<ActionResult<object>> HISIndividualCount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new HISIndividual(_appSetting).HISIndividualCount() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion HISIndividual

		#region HISInvitation

		[HttpGet]
		[Authorize]
		[Route("HISInvitationGetByID")]
		public async Task<ActionResult<object>> HISInvitationGetByID(int? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new HISInvitation(_appSetting).HISInvitationGetByID(Id) };
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
		[Route("HISInvitationGetAll")]
		public async Task<ActionResult<object>> HISInvitationGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new HISInvitation(_appSetting).HISInvitationGetAll() };
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
		[Route("HISInvitationGetAllOnPage")]
		public async Task<ActionResult<object>> HISInvitationGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<HISInvitationOnPage> rsHISInvitationOnPage = await new HISInvitation(_appSetting).HISInvitationGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"HISInvitation", rsHISInvitationOnPage},
						{"TotalCount", rsHISInvitationOnPage != null && rsHISInvitationOnPage.Count > 0 ? rsHISInvitationOnPage[0].RowNumber : 0},
						{"PageIndex", rsHISInvitationOnPage != null && rsHISInvitationOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsHISInvitationOnPage != null && rsHISInvitationOnPage.Count > 0 ? PageSize : 0},
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
		[Route("HISInvitationInsert")]
		public async Task<ActionResult<object>> HISInvitationInsert(HISInvitation _hISInvitation)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new HISInvitation(_appSetting).HISInvitationInsert(_hISInvitation) };
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
		[Route("HISInvitationListInsert")]
		public async Task<ActionResult<object>> HISInvitationListInsert(List<HISInvitation> _hISInvitations)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (HISInvitation _hISInvitation in _hISInvitations)
				{
					int? result = await new HISInvitation(_appSetting).HISInvitationInsert(_hISInvitation);
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
		[Route("HISInvitationUpdate")]
		public async Task<ActionResult<object>> HISInvitationUpdate(HISInvitation _hISInvitation)
		{
			try
			{
				int count = await new HISInvitation(_appSetting).HISInvitationUpdate(_hISInvitation);
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
		[Route("HISInvitationDelete")]
		public async Task<ActionResult<object>> HISInvitationDelete(HISInvitation _hISInvitation)
		{
			try
			{
				int count = await new HISInvitation(_appSetting).HISInvitationDelete(_hISInvitation);
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
		[Route("HISInvitationListDelete")]
		public async Task<ActionResult<object>> HISInvitationListDelete(List<HISInvitation> _hISInvitations)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (HISInvitation _hISInvitation in _hISInvitations)
				{
					var result = await new HISInvitation(_appSetting).HISInvitationDelete(_hISInvitation);
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
		[Route("HISInvitationDeleteAll")]
		public async Task<ActionResult<object>> HISInvitationDeleteAll()
		{
			try
			{
				int count = await new HISInvitation(_appSetting).HISInvitationDeleteAll();
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
		[Route("HISInvitationCount")]
		public async Task<ActionResult<object>> HISInvitationCount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new HISInvitation(_appSetting).HISInvitationCount() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion HISInvitation

		#region HISNews

		[HttpGet]
		[Authorize]
		[Route("HISNewsGetByID")]
		public async Task<ActionResult<object>> HISNewsGetByID(int? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new HISNews(_appSetting).HISNewsGetByID(Id) };
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
		[Route("HISNewsGetAll")]
		public async Task<ActionResult<object>> HISNewsGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new HISNews(_appSetting).HISNewsGetAll() };
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
		[Route("HISNewsGetAllOnPage")]
		public async Task<ActionResult<object>> HISNewsGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<HISNewsOnPage> rsHISNewsOnPage = await new HISNews(_appSetting).HISNewsGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"HISNews", rsHISNewsOnPage},
						{"TotalCount", rsHISNewsOnPage != null && rsHISNewsOnPage.Count > 0 ? rsHISNewsOnPage[0].RowNumber : 0},
						{"PageIndex", rsHISNewsOnPage != null && rsHISNewsOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsHISNewsOnPage != null && rsHISNewsOnPage.Count > 0 ? PageSize : 0},
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
		[Route("HISNewsInsert")]
		public async Task<ActionResult<object>> HISNewsInsert(HISNews _hISNews)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new HISNews(_appSetting).HISNewsInsert(_hISNews) };
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
		[Route("HISNewsListInsert")]
		public async Task<ActionResult<object>> HISNewsListInsert(List<HISNews> _hISNewss)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (HISNews _hISNews in _hISNewss)
				{
					int? result = await new HISNews(_appSetting).HISNewsInsert(_hISNews);
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
		[Route("HISNewsUpdate")]
		public async Task<ActionResult<object>> HISNewsUpdate(HISNews _hISNews)
		{
			try
			{
				int count = await new HISNews(_appSetting).HISNewsUpdate(_hISNews);
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
		[Route("HISNewsDelete")]
		public async Task<ActionResult<object>> HISNewsDelete(HISNews _hISNews)
		{
			try
			{
				int count = await new HISNews(_appSetting).HISNewsDelete(_hISNews);
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
		[Route("HISNewsListDelete")]
		public async Task<ActionResult<object>> HISNewsListDelete(List<HISNews> _hISNewss)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (HISNews _hISNews in _hISNewss)
				{
					var result = await new HISNews(_appSetting).HISNewsDelete(_hISNews);
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
		[Route("HISNewsDeleteAll")]
		public async Task<ActionResult<object>> HISNewsDeleteAll()
		{
			try
			{
				int count = await new HISNews(_appSetting).HISNewsDeleteAll();
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
		[Route("HISNewsCount")]
		public async Task<ActionResult<object>> HISNewsCount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new HISNews(_appSetting).HISNewsCount() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion HISNews

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

		#region HISSMS

		[HttpGet]
		[Authorize]
		[Route("HISSMSGetByID")]
		public async Task<ActionResult<object>> HISSMSGetByID(int? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new HISSMS(_appSetting).HISSMSGetByID(Id) };
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
		[Route("HISSMSGetAll")]
		public async Task<ActionResult<object>> HISSMSGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new HISSMS(_appSetting).HISSMSGetAll() };
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
		[Route("HISSMSGetAllOnPage")]
		public async Task<ActionResult<object>> HISSMSGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<HISSMSOnPage> rsHISSMSOnPage = await new HISSMS(_appSetting).HISSMSGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"HISSMS", rsHISSMSOnPage},
						{"TotalCount", rsHISSMSOnPage != null && rsHISSMSOnPage.Count > 0 ? rsHISSMSOnPage[0].RowNumber : 0},
						{"PageIndex", rsHISSMSOnPage != null && rsHISSMSOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsHISSMSOnPage != null && rsHISSMSOnPage.Count > 0 ? PageSize : 0},
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
		[Route("HISSMSInsert")]
		public async Task<ActionResult<object>> HISSMSInsert(HISSMS _hISSMS)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new HISSMS(_appSetting).HISSMSInsert(_hISSMS) };
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
		[Route("HISSMSListInsert")]
		public async Task<ActionResult<object>> HISSMSListInsert(List<HISSMS> _hISSMSs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (HISSMS _hISSMS in _hISSMSs)
				{
					int? result = await new HISSMS(_appSetting).HISSMSInsert(_hISSMS);
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
		[Route("HISSMSUpdate")]
		public async Task<ActionResult<object>> HISSMSUpdate(HISSMS _hISSMS)
		{
			try
			{
				int count = await new HISSMS(_appSetting).HISSMSUpdate(_hISSMS);
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
		[Route("HISSMSDelete")]
		public async Task<ActionResult<object>> HISSMSDelete(HISSMS _hISSMS)
		{
			try
			{
				int count = await new HISSMS(_appSetting).HISSMSDelete(_hISSMS);
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
		[Route("HISSMSListDelete")]
		public async Task<ActionResult<object>> HISSMSListDelete(List<HISSMS> _hISSMSs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (HISSMS _hISSMS in _hISSMSs)
				{
					var result = await new HISSMS(_appSetting).HISSMSDelete(_hISSMS);
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
		[Route("HISSMSDeleteAll")]
		public async Task<ActionResult<object>> HISSMSDeleteAll()
		{
			try
			{
				int count = await new HISSMS(_appSetting).HISSMSDeleteAll();
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
		[Route("HISSMSCount")]
		public async Task<ActionResult<object>> HISSMSCount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new HISSMS(_appSetting).HISSMSCount() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion HISSMS
	}
}
