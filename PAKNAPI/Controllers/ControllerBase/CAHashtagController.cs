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
using Microsoft.AspNetCore.Http;
using PAKNAPI.Models.ModelBase;

namespace PAKNAPI.Controllers.ControllerBase
{
    [Route("api/CAHashtag")]
    [ApiController]
    public class CAHashtagController : BaseApiController
	{
        private readonly IAppSetting _appSetting;
        private readonly IClient _bugsnag;

        public CAHashtagController(IAppSetting appSetting, IClient bugsnag)
        {
            _appSetting = appSetting;
            _bugsnag = bugsnag;
        }


		[HttpGet]
		[Authorize]
		[Route("CAHashtagGetAllOnPage")]
		public async Task<ActionResult<object>> CAHashtagGetAllOnPage(int PageSize, int PageIndex, string Name, int? QuantityUser, bool? IsActived)
		{
			try
			{
				List<CAHashtagListPage> rsCAHashtagOnPage = await new CAHashtagListPage(_appSetting).CAHashtagGetAllOnPage(PageSize, PageIndex, Name, QuantityUser, IsActived);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAHashtag", rsCAHashtagOnPage},
						{"TotalCount", rsCAHashtagOnPage != null && rsCAHashtagOnPage.Count > 0 ? rsCAHashtagOnPage[0].RowNumber : 0},
						{"PageIndex", rsCAHashtagOnPage != null && rsCAHashtagOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsCAHashtagOnPage != null && rsCAHashtagOnPage.Count > 0 ? PageSize : 0},
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
		[Route("CAHashtagGetAll")]
		public async Task<ActionResult<object>> CAHashtagGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new CAHashtag(_appSetting).CAHashtagGetAll() };
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
		[Route("CAHashtagInsert")]
		public async Task<ActionResult<object>> CAHashtagInsert(CAHashtag _cAHashtag)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CAHashtag(_appSetting).CAHashtagInsert(_cAHashtag) };
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
		[Route("CAHashtagUpdate")]
		public async Task<ActionResult<object>> CAHashtagUpdate(CAHashtag _cAHashtag)
		{
			try
			{
				int count = await new CAHashtag(_appSetting).CAHashtagUpdate(_cAHashtag);
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, new Exception());

					return new ResultApi { Success = ResultCode.ORROR, Result = count, Message = ResultMessage.ORROR };
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
		[Route("CAHashtagDelete")]
		public async Task<ActionResult<object>> CAHashtagDelete(CAHashtag _cAHashtag)
		{
			try
			{
				int count = await new CAHashtag(_appSetting).CAHashtagDelete(_cAHashtag);
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
	}
}
