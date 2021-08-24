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
    [Route("api/word")]
    [ApiController]
    public class WordController : BaseApiController
	{
        private readonly IAppSetting _appSetting;
        private readonly IClient _bugsnag;

        public WordController(IAppSetting appSetting, IClient bugsnag)
        {
            _appSetting = appSetting;
            _bugsnag = bugsnag;
        }


		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-list-word-on-page")]
		public async Task<ActionResult<object>> CAWordGetAllOnPageBase(int? PageSize, int? PageIndex, int? GroupId, string Name, string Description, bool? IsActived)
		{
			try
			{
				List<CAWordGetAllOnPage> rsCAWordGetAllOnPage = await new CAWordGetAllOnPage(_appSetting).CAWordGetAllOnPageDAO(PageSize, PageIndex, GroupId, Name, Description, IsActived);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAWordGetAllOnPage", rsCAWordGetAllOnPage},
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
		[Route("get-list-word-on-page-by-group-id")]
		public async Task<ActionResult<object>> CAWordGetAllOnPageByGroupIdBase(int GroupId)
		{
			try
			{
				List<CAWordGetAllOnPage> rsCAWordGetAllOnPage = await new CAWordGetAllOnPage(_appSetting).CAWordGetAllOnPageByGroupIdDAO(GroupId);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAWordGetAllOnPageByGroupId", rsCAWordGetAllOnPage},
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
		[Route("get-by-id")]
		public async Task<ActionResult<object>> CAWordGetByIDBase(int? Id)
		{
			try
			{
				List<CAWordGetByID> rsCAWordGetByID = await new CAWordGetByID(_appSetting).CAWordGetByIDDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAWordGetByID", rsCAWordGetByID},
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
		public async Task<ActionResult<object>> CAWordInsertBase(CAWordInsertIN _cAWordInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CAWordInsert(_appSetting).CAWordInsertDAO(_cAWordInsertIN) };
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
		public async Task<ActionResult<object>> CAWordUpdateBase(CAWordUpdateIN _cAWordUpdateIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CAWordUpdate(_appSetting).CAWordUpdateDAO(_cAWordUpdateIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		[HttpGet]
		[Route("get-list-suggest")]
		public async Task<ActionResult<object>> CA_WordGetListSuggestBase()
		{
			try
			{
				List<CA_WordGetListSuggest> rsCA_WordGetListSuggest = await new CA_WordGetListSuggest(_appSetting).CA_WordGetListSuggestDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAWordGetListSuggest", rsCA_WordGetListSuggest},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("delete")]
		public async Task<ActionResult<object>> CAWordDeleteBase(CADepartmentGroupDeleteIN _cADepartmentGroupDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CADepartmentGroupDelete(_appSetting).CAWordDeleteDAO(_cADepartmentGroupDeleteIN) };
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
