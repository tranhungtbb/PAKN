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
using PAKNAPI.ModelBase;

namespace PAKNAPI.Controllers.ControllerBase
{
    [Route("api/DAMAdministration")]
    [ApiController]
    public class DAMAdministrationController : BaseApiController
	{
        private readonly IAppSetting _appSetting;
        private readonly IClient _bugsnag;

        public DAMAdministrationController(IAppSetting appSetting, IClient bugsnag)
        {
            _appSetting = appSetting;
            _bugsnag = bugsnag;
        }

		[HttpGet]
		[Route("DAMAdministrationGetListHomePage")]
		public async Task<ActionResult<object>> DAMAdministrationGetListHomePage()
		{
			try
			{
				List<DAMAdministrationGetListHomePage> rsDAMAdministrationGetList = await new DAMAdministrationGetListHomePage(_appSetting).DAMAdministrationGetListHomePageDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"DAMAdministrationGetList", rsDAMAdministrationGetList.Take(4).ToList()},
						{"TotalCount", rsDAMAdministrationGetList != null && rsDAMAdministrationGetList.Count > 0 ? rsDAMAdministrationGetList[0].RowNumber : 0},

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
