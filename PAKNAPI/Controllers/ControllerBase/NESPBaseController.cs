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
	[Route("api/NESPBase")]
	[ApiController]
	public class NESPBaseController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		private readonly IClient _bugsnag;

		public NESPBaseController(IAppSetting appSetting, IClient bugsnag)
		{
			_appSetting = appSetting;
			_bugsnag = bugsnag;
		}

		[HttpGet]
		[Authorize]
		[Route("NENewsGetAllOnPageBase")]
		public async Task<ActionResult<object>> NENewsGetAllOnPageBase(string NewsIds, int? PageSize, int? PageIndex, string Title, int? NewsType, int? Status)
		{
			try
			{
				List<NENewsGetAllOnPage> rsNENewsGetAllOnPage = await new NENewsGetAllOnPage(_appSetting).NENewsGetAllOnPageDAO(NewsIds, PageSize, PageIndex, Title, NewsType, Status);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"NENewsGetAllOnPage", rsNENewsGetAllOnPage},
						{"TotalCount", rsNENewsGetAllOnPage != null && rsNENewsGetAllOnPage.Count > 0 ? rsNENewsGetAllOnPage[0].RowNumber : 0},
						{"PageIndex", rsNENewsGetAllOnPage != null && rsNENewsGetAllOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsNENewsGetAllOnPage != null && rsNENewsGetAllOnPage.Count > 0 ? PageSize : 0},
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
		[Route("NENewsGetListHomePage")]
		public async Task<ActionResult<object>> NENewsGetListHome()
		{
			try
			{
				List<NENewsGetListHomePage> rsPUNewsGetListHomePage = await new NENewsGetListHomePage(_appSetting).PU_NewsGetListHomePage(true);
				if (rsPUNewsGetListHomePage.Count < 4)
				{
					rsPUNewsGetListHomePage = await new NENewsGetListHomePage(_appSetting).PU_NewsGetListHomePage(false);
				}
				return new ResultApi { Success = ResultCode.OK, Result = rsPUNewsGetListHomePage };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}


		[HttpGet]
		[Authorize]
		[Route("NENewsGetByIDBase")]
		public async Task<ActionResult<object>> NENewsGetByIDBase(int? Id)
		{
			try
			{
				List<NENewsGetByID> rsNENewsGetByID = await new NENewsGetByID(_appSetting).NENewsGetByIDDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"NENewsGetByID", rsNENewsGetByID},
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
		[Route("NENewsGetByIDOnJoinBase")]
		public async Task<ActionResult<object>> NENewsGetByIDOnJoinBase(int? Id)
		{
			try
			{
				List<NENewsGetByIDOnJoin> rsNENewsGetByIDOnJoin = await new NENewsGetByIDOnJoin(_appSetting).NENewsGetByIDOnJoinDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"NENewsGetByIDOnJoin", rsNENewsGetByIDOnJoin},
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
		[Route("NENewsInsertBase")]
		public async Task<ActionResult<object>> NENewsInsertBase(NENewsInsertIN _nENewsInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new NENewsInsert(_appSetting).NENewsInsertDAO(_nENewsInsertIN) };
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
		[Route("NENewsInsertListBase")]
		public async Task<ActionResult<object>> NENewsInsertListBase(List<NENewsInsertIN> _nENewsInsertINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _nENewsInsertIN in _nENewsInsertINs)
				{
					var result = await new NENewsInsert(_appSetting).NENewsInsertDAO(_nENewsInsertIN);
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
		[Route("NENewsUpdateBase")]
		public async Task<ActionResult<object>> NENewsUpdateBase(NENewsUpdateIN _nENewsUpdateIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new NENewsUpdate(_appSetting).NENewsUpdateDAO(_nENewsUpdateIN) };
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
		[Route("NENewsUpdateListBase")]
		public async Task<ActionResult<object>> NENewsUpdateListBase(List<NENewsUpdateIN> _nENewsUpdateINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _nENewsUpdateIN in _nENewsUpdateINs)
				{
					var result = await new NENewsUpdate(_appSetting).NENewsUpdateDAO(_nENewsUpdateIN);
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

		[HttpGet]
		[Authorize]
		[Route("NERelateGetAllBase")]
		public async Task<ActionResult<object>> NERelateGetAllBase(int? NewsId)
		{
			try
			{
				List<NERelateGetAll> rsNERelateGetAll = await new NERelateGetAll(_appSetting).NERelateGetAllDAO(NewsId);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"NERelateGetAll", rsNERelateGetAll},
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
	}
}
