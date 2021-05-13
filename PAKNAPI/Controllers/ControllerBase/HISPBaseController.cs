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
	[Route("api/HISPBase")]
	[ApiController]
	public class HISPBaseController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		private readonly IClient _bugsnag;

		public HISPBaseController(IAppSetting appSetting, IClient bugsnag)
		{
			_appSetting = appSetting;
			_bugsnag = bugsnag;
		}

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("HISSMSDeleteBySMSIdBase")]
		public async Task<ActionResult<object>> HISSMSDeleteBySMSIdBase(HISSMSDeleteBySMSIdIN _hISSMSDeleteBySMSIdIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new HISSMSDeleteBySMSId(_appSetting).HISSMSDeleteBySMSIdDAO(_hISSMSDeleteBySMSIdIN) };
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
		[Route("HISSMSDeleteBySMSIdListBase")]
		public async Task<ActionResult<object>> HISSMSDeleteBySMSIdListBase(List<HISSMSDeleteBySMSIdIN> _hISSMSDeleteBySMSIdINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _hISSMSDeleteBySMSIdIN in _hISSMSDeleteBySMSIdINs)
				{
					var result = await new HISSMSDeleteBySMSId(_appSetting).HISSMSDeleteBySMSIdDAO(_hISSMSDeleteBySMSIdIN);
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
		[Authorize("ThePolicy")]
		[Route("HISSMSGetBySMSIdOnPageBase")]
		public async Task<ActionResult<object>> HISSMSGetBySMSIdOnPageBase(int? PageSize, int? PageIndex, int? SMSId, string Content, string UserName, DateTime? CreateDate, int? Status)
		{
			try
			{
				List<HISSMSGetBySMSIdOnPage> rsHISSMSGetBySMSIdOnPage = await new HISSMSGetBySMSIdOnPage(_appSetting).HISSMSGetBySMSIdOnPageDAO(PageSize, PageIndex, SMSId, Content, UserName, CreateDate, Status);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"HISSMSGetBySMSIdOnPage", rsHISSMSGetBySMSIdOnPage},
						{"TotalCount", rsHISSMSGetBySMSIdOnPage != null && rsHISSMSGetBySMSIdOnPage.Count > 0 ? rsHISSMSGetBySMSIdOnPage[0].RowNumber : 0},
						{"PageIndex", rsHISSMSGetBySMSIdOnPage != null && rsHISSMSGetBySMSIdOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsHISSMSGetBySMSIdOnPage != null && rsHISSMSGetBySMSIdOnPage.Count > 0 ? PageSize : 0},
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
		[Route("HISSMSInsertBase")]
		public async Task<ActionResult<object>> HISSMSInsertBase(HISSMSInsertIN _hISSMSInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new HISSMSInsert(_appSetting).HISSMSInsertDAO(_hISSMSInsertIN) };
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
		[Route("HISSMSInsertListBase")]
		public async Task<ActionResult<object>> HISSMSInsertListBase(List<HISSMSInsertIN> _hISSMSInsertINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _hISSMSInsertIN in _hISSMSInsertINs)
				{
					var result = await new HISSMSInsert(_appSetting).HISSMSInsertDAO(_hISSMSInsertIN);
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
	}
}
