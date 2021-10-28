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
using PAKNAPI.Models.Remind;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using PAKNAPI.Models.Recommendation;
using PAKNAPI.Models.ModelBase;
using Microsoft.Extensions.Configuration;
using Bugsnag;

namespace PAKNAPI.Controllers
{
    [Route("api/notification")]
    [ApiController]

    public class NotificationController : BaseApiController
    {
        private readonly IAppSetting _appSetting;
        private readonly IClient _bugsnag;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;

        public NotificationController(IAppSetting appSetting, IClient bugsnag, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _appSetting = appSetting;
            _bugsnag = bugsnag;
            _configuration = configuration;
        }


        /// <summary>
        /// chi tiết thông báo
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize("ThePolicy")]
        [Route("get-by-id")]
        public async Task<ActionResult<object>> NotificationGetByIdBase(int? Id)
        {
            try
            {
                List<SYNotificationGetById> rsSYNotificationGetByID = await new SYNotificationGetById(_appSetting).SYNotificationGetByIdDAO(Id);
                IDictionary<string, object> json = new Dictionary<string, object>
                    {
                        {"SYNotificationGetByID", rsSYNotificationGetByID.FirstOrDefault()},
                    };
                return new ResultApi { Success = ResultCode.OK, Result = json };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }


        /// <summary>
        /// danh sách thông báo
        /// </summary>
        /// <returns></returns>

        #region SYNotificationGetAll
        [HttpPost]
        [Route("get-list-notification-on-page")]
        public async Task<object> SYNotificationGetListOnPage() {
            try{
                var jss = new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    DateTimeZoneHandling = DateTimeZoneHandling.Local,
                    DateParseHandling = DateParseHandling.DateTimeOffset,
                };
                var model = JsonConvert.DeserializeObject<SYNotificationGetList>(Request.Form["model"].ToString(), jss);
                var s = (int)new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
                var syNotifications = await new SYNotificationGetListOnPageByReceiveId(_appSetting).SYNotificationGetListOnPageByReceiveIdDAO((int)new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext), model.PageSize, model.PageIndex, model.Title , model.Content , model.Type , model.SendDate);
                IDictionary<string, object> json = new Dictionary<string, object>
                    {
                        {"syNotifications", syNotifications},
                        {"TotalCount", syNotifications != null && syNotifications.Count > 0 ? syNotifications[0].RowNumber : 0},
                        {"PageIndex", syNotifications != null && syNotifications.Count > 0 ? model.PageIndex : 0},
                        {"PageSize", syNotifications != null && syNotifications.Count > 0 ? model.PageSize : 0}
                    };
                return new ResultApi { Success = ResultCode.OK, Result = json };
            } catch (Exception ex) {
                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }
        #endregion



        #region SYNotificationUpdateIsViewed
        /// <summary>
        /// cập nhập trạng thái đã xem
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("update-is-viewed")]
        public async Task<object> SYNotificationUpdateIsViewed()
        {
            try
            {
                var count = await new SYNotificationGetListOnPageByReceiveId(_appSetting).SYNotificatioUpdateIsViewedByReceiveIdDAO((int)new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext));
                if (count > 0)
                {
                    return new ResultApi { Success = ResultCode.OK, Result = count };
                }
                else
                {
                    return new ResultApi{ Success = ResultCode.ORROR};
                }
            }
            catch (Exception ex)
            {
                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }
        #endregion
        /// <summary>
        /// cập nhập trạng thái đã đọc
        /// </summary>
        /// <param name="ObjectId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("update-is-readed")]
        public async Task<object> SYNotificationUpdateReaded(int? ObjectId)
        {
            try
            {
                var count = await new SYNotificationGetListOnPageByReceiveId(_appSetting).SYNotificatioUpdateIsReadedDAO(ObjectId,(int)new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext), DateTime.Now);
                if (count > 0)
                {
                    return new ResultApi { Success = ResultCode.OK, Result = count };
                }
                else
                {
                    return new ResultApi { Success = ResultCode.ORROR };
                }
            }
            catch (Exception ex)
            {
                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }
        /// <summary>
        /// xóa  thông báo
        /// </summary>
        /// <param name="_syNotification"></param>
        /// <returns></returns>

        [HttpPost]
        [Authorize("ThePolicy")]
        [Route("delete")]
        public async Task<ActionResult<object>> SYNotificationDelete(SYNotificationModel _syNotification)
        {
            try
            {
                int count = await new SYNotification(_appSetting,_configuration).SYNotificationDelete(_syNotification);
                if (count > 0)
                {
                    new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);

                    return new ResultApi { Success = ResultCode.OK, Result = count };
                }
                else
                {
                    new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, "Không tồn tại thông báo",new Exception());

                    return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
                }
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        [HttpPost]
        [Authorize("ThePolicy")]
        [Route("update-token-fire-base")]
        public async Task<ActionResult<object>> UpdateTokenFireBase(UpdateTokenFireBaseRequest request)
        {
            try
            {
                var UserId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
                var result = await new UpdateTokenFireBase(_appSetting).UpdateTokenFireBaseDAO(UserId, request);
                return new ResultApi { Success = ResultCode.OK, Result = result };
            }
            catch (Exception ex)
            {
                //_bugsnag.Notify(ex);
                //new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, ex);
                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }



    }
}
