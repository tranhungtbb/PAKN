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
using PAKNAPI.Models.ModelBase;
using PAKNAPI.Models.Remind;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using PAKNAPI.Models.Recommendation;

namespace PAKNAPI.Controllers
{
    [Route("api/SYNotification")]
    [ApiController]

    public class SYNotificationController : BaseApiController
    {
        private readonly IAppSetting _appSetting;
        private readonly IClient _bugsnag;

        public SYNotificationController(IAppSetting appSetting, IClient bugsnag)
        {
            _appSetting = appSetting;
            _bugsnag = bugsnag;
        }

        #region SYNotificationInsertTypeNews

        [HttpPost]
        [Authorize]
        [Route("SYNotificationInsertTypeNews")]
        public async Task<object> SYNotificationInsertTypeNews(int Id, string Title, bool isCreateNews)
        {
            try
            {
                //lấy tất cả danh sách người dùng
                List<SYUser> lstUser = await new SYUser(_appSetting).SYUserGetAll();
                if (lstUser.Count > 0)
                {
                    string senderName = new LogHelper(_appSetting).GetFullNameFromRequest(HttpContext);
                    foreach (var user in lstUser) {
                        var model = new SYNotificationModel();
                        model.SenderId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
                        model.SendOrgId = new LogHelper(_appSetting).GetUnitIdFromRequest(HttpContext);
                        model.ReceiveId = user.Id;
                        model.ReceiveOrgId = user.UnitId;
                        model.DataId = Id;
                        model.SendDate = DateTime.Now;
                        model.Type = 1;
                        model.Title = isCreateNews == true ? senderName + " vừa đăng một bài viết mới" : senderName + " vừa cập nhập một bài viết";
                        model.Content = Title;
                        model.IsViewed = true;
                        // insert vào db-
                        var count = await new SYNotificationInsert(_appSetting).SYNotificationInsertDAO(model);
                    }
                    return new ResultApi { Success = ResultCode.OK, Message = "Success" };
                }
                else {
                    return new ResultApi { Success = ResultCode.ORROR, Message = "Users null" };
                }
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }


        #endregion SYNotificationInsertTypeNews


    }
}
