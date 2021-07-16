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
		public async Task<ActionResult<object>> DAMAdministrationForward(string LstUnitId, int AdministrationId, string Content)
		{
			try
			{
				List<int> lstUnitId = new List<int>();
				lstUnitId = LstUnitId.Split(',').Select(Int32.Parse).Where(x => x != 0).ToList();
				if (lstUnitId.Count == 0) {
					return new ResultApi { Success = ResultCode.ORROR, Message = "Unit is not null" };
				}
				DAMAdministrationForward modelInsert = new DAMAdministrationForward();
				modelInsert.AdministrationId = AdministrationId;
				modelInsert.CreateBy = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
				modelInsert.CreatedDate = DateTime.Now;
				modelInsert.Content = Content;

				foreach (var item in lstUnitId) {
					var lstUser = await new SYUserGetByUnitId(_appSetting).SYUserGetByUnitIdDAO(item);
					if (lstUser.Count() == 0) { continue; }
					var lstUserId = new List<long>();
					lstUser.ForEach(item => {
						lstUserId.Add(item.Id);
					});
					modelInsert.UnitId = item;
					modelInsert.LstUserReceive = String.Join(",", lstUserId);
					var id = await new DAMAdministrationForward(_appSetting).DAMAdministrationForwardInsertDAO(modelInsert);
					// tạo thông báo
					var model = new SYNotificationModel();
					model.SenderId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
					model.SendOrgId = new LogHelper(_appSetting).GetUnitIdFromRequest(HttpContext);

					lstUser.ForEach(async item =>
					{
						model.ReceiveId = item.Id;
						model.DataId = Convert.ToInt32(id);
						model.SendDate = DateTime.Now;
						model.Type = TYPENOTIFICATION.ADMINISTRATIVE;
						model.Title = "Tiếp nhận thủ tục hành chính";
						model.Content = "Bạn vừa nhận được một thủ tục hành chính từ " + new LogHelper(_appSetting).GetFullNameFromRequest(HttpContext);
						model.IsViewed = true;
						model.IsReaded = true;
						// insert vào db-
						await new SYNotification(_appSetting).SYNotificationInsertDAO(model);
					});
				}
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
