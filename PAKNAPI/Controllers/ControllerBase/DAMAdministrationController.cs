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

		[HttpGet]
		[Route("DAMAdministrationForward")]
		[Authorize]
		public async Task<ActionResult<object>> DAMAdministrationForward(int UnitId, int AdministrationId, string Content)
		{
			try
			{
				var lstUser = await new SYUserGetByUnitId(_appSetting).SYUserGetByUnitIdDAO(UnitId);
				var lstUserId = new List<long>();
				lstUser.ForEach(item => {
					lstUserId.Add(item.Id);
				});
				DAMAdministrationForward modelInsert = new DAMAdministrationForward();
				modelInsert.AdministrationId = AdministrationId;
				modelInsert.UnitId = UnitId;
				modelInsert.CreateBy = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
				modelInsert.CreatedDate = DateTime.Now;
				modelInsert.Content = Content;
				modelInsert.LstUserReceive = String.Join(",", lstUserId);
				await new DAMAdministrationForward(_appSetting).DAMAdministrationForwardInsertDAO(modelInsert);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
				return new ResultApi { Success = ResultCode.OK};
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		// DAMAdministrationForward
	}
}
