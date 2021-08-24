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
    [Route("api/news-type")]
    [ApiController]
    public class NewsTypeController : BaseApiController
	{
        private readonly IAppSetting _appSetting;
        private readonly IClient _bugsnag;

        public NewsTypeController(IAppSetting appSetting, IClient bugsnag)
        {
            _appSetting = appSetting;
            _bugsnag = bugsnag;
        }


		[HttpPost]
		[Authorize]
		[Route("delete")]
		public async Task<ActionResult<object>> CANewsTypeDeleteBase(CANewsTypeDeleteIN _cANewsTypeDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CANewsTypeDelete(_appSetting).CANewsTypeDeleteDAO(_cANewsTypeDeleteIN) };
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
		[Route("get-list-news-type-on-page")]
		public async Task<ActionResult<object>> CANewsTypeGetAllOnPageBase(int? PageSize, int? PageIndex, string Name, string Description, bool? IsActived)
		{
			try
			{
				List<CANewsTypeGetAllOnPage> rsCANewsTypeGetAllOnPage = await new CANewsTypeGetAllOnPage(_appSetting).CANewsTypeGetAllOnPageDAO(PageSize, PageIndex, Name, Description, IsActived);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CANewsTypeGetAllOnPage", rsCANewsTypeGetAllOnPage},
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
		[Authorize]
		[Route("get-by-id")]
		public async Task<ActionResult<object>> CANewsTypeGetByIDBase(int? Id)
		{
			try
			{
				List<CANewsTypeGetByID> rsCANewsTypeGetByID = await new CANewsTypeGetByID(_appSetting).CANewsTypeGetByIDDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CANewsTypeGetByID", rsCANewsTypeGetByID},
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
		[Route("insert")]
		public async Task<ActionResult<object>> CANewsTypeInsertBase(CANewsTypeInsertIN _cANewsTypeInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CANewsTypeInsert(_appSetting).CANewsTypeInsertDAO(_cANewsTypeInsertIN) };
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
		[Route("update")]
		public async Task<ActionResult<object>> CANewsTypeUpdateBase(CANewsTypeUpdateIN _cANewsTypeUpdateIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CANewsTypeUpdate(_appSetting).CANewsTypeUpdateDAO(_cANewsTypeUpdateIN) };
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
