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
using PAKNAPI.Models.Remind;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using PAKNAPI.Models.Recommendation;
using PAKNAPI.Models.ModelBase;

namespace PAKNAPI.Controllers
{
    [Route("api/SYNotification")]
    [ApiController]

    public class NotificationController : BaseApiController
    {
        private readonly IAppSetting _appSetting;
        private readonly IClient _bugsnag;

        public NotificationController(IAppSetting appSetting, IClient bugsnag)
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
                //lấy tất cả danh sách người dùng là cá nhân, doanh nghiệp
                List<SYUserGetNonSystem> lstUser = await new SYUserGetNonSystem(_appSetting).SYUserGetNonSystemDAO();
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
                        model.Type = TYPENOTIFICATION.NEWS;
                        model.Title = isCreateNews == true ? senderName + " vừa đăng một bài viết mới" : senderName + " vừa cập nhập một bài viết";
                        model.Content = Title;
                        model.IsViewed = true;
                        model.IsReaded = true;
                        // insert vào db-
                        var count = await new SYNotification(_appSetting).SYNotificationInsertDAO(model);
                    }
                    return new ResultApi { Success = ResultCode.OK, Message = "Success" };
                }
                else {
                    return new ResultApi { Success = ResultCode.ORROR, Message = "Users null" };
                }
            }
            catch (Exception ex)
            {
                //new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }


        #endregion SYNotificationInsertTypeNews


        #region SYNotificationInsertTypeRecommendation

        [HttpGet]
        [Authorize]
        [Route("SYNotificationInsertTypeRecommendation")]
        public async Task<object> SYNotificationInsertTypeRecommendation(int? recommendationId)
        {
            try
            {
                // thông tin PAKN
                var recommendation = new RecommendationDAO(_appSetting).RecommendationGetByID(recommendationId).Result.Model;

                //thông tin người gửi PAKN
                SYUser sender = await new SYUser(_appSetting).SYUserGetByID(recommendation.CreatedBy);

                // danh sách người thuộc đơn vị mà PAKN gửi đến
                List<SYUserGetByUnitId> lstUser = await new SYUserGetByUnitId(_appSetting).SYUserGetByUnitIdDAO((int)recommendation.UnitId);

                List<RecommendationForward> lstRMForward = new List<RecommendationForward>();
                int unitReceiveId, receiveId;

                // danh sách người dùng thuộc đơn vị mà PAKN gửi đến đơn vị đó
                List<SYUserGetByUnitId> listUserReceiveResolve = new List<SYUserGetByUnitId>();

                // thông tin người giải quyết
                SYUser approver = new SYUser();

                // thông tin đơn vị giải quyết
                SYUnit unitReceive = new SYUnit();

                // thông tin đơn vị tiếp nhận PAKN
                SYUnit unit = await new SYUnit(_appSetting).SYUnitGetByID(recommendation.UnitId);

                // lấy thông tin đơn vị người đăng nhập
                SYUserGetByID userInfo = (await new SYUserGetByID(_appSetting).SYUserGetByIDDAO(new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext))).FirstOrDefault();

                // obj thông báo
                SYNotificationModel notification = new SYNotificationModel();
                notification.SenderId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
                if (sender.Id != userInfo.Id) {
                    notification.SendOrgId = new LogHelper(_appSetting).GetUnitIdFromRequest(HttpContext);
                }
                notification.DataId = recommendation.Id;
                notification.SendDate = DateTime.Now;
                notification.Type = TYPENOTIFICATION.RECOMMENDATION;
                notification.TypeSend = recommendation.Status;
                notification.IsViewed = true;
                notification.IsReaded = true;


                switch (recommendation.Status)
                {
                    case STATUS_RECOMMENDATION.RECEIVE_WAIT: //2 Chờ xử lý

                        foreach (var item in lstUser) {

                            notification.ReceiveId = item.Id;
                            notification.ReceiveOrgId = item.UnitId;
                            notification.Title = "PAKN CHỜ XỬ LÝ";
                            notification.Content =
                                recommendation.SendId != item.Id ?
                                sender.FullName + " vừa gửi một PAKN." : "Bạn vừa tạo một PAKN.";
                            // insert notification
                            await new SYNotification(_appSetting).SYNotificationInsertDAO(notification);
                        }

                        // người gửi PAKN
                        
                        break;
                    case STATUS_RECOMMENDATION.RECEIVE_DENY: //3 Từ chối xử lý

                        //foreach (var item in lstUser)
                        //{
                        //    notification.ReceiveId = item.Id;
                        //    notification.ReceiveOrgId = item.UnitId;
                        //    notification.Title = "PAKN số " + recommendation.Code + " đã bị từ chối xử lý";
                        //    notification.Content =
                        //        recommendation.SendId != item.Id ?
                        //        sender.FullName + " vừa gửi một PAKN." : "Bạn vừa tạo một PAKN.";
                        //    // insert notification
                        //    await new SYNotification(_appSetting).SYNotificationInsertDAO(notification);
                        //}

                        // người gửi PAKN

                        notification.ReceiveId = sender.Id;
                        notification.Title = "PAKN BỊ TỪ CHỐI";
                        notification.Content = "Phản ánh kiến nghị số " + recommendation.Code + " của bạn đã bị từ chối.";
                        await new SYNotification(_appSetting).SYNotificationInsertDAO(notification);

                        break;
                    case STATUS_RECOMMENDATION.RECEIVE_APPROVED: //4 Đã tiếp nhận

                        notification.Title = "PAKN ĐÃ TIẾP NHẬN";
                        notification.Content = "PAKN " + recommendation.Code + " đã được tiếp nhận giải quyết: đơn vị được yêu cầu giải quyết đã tiếp nhận PAKN";
                        foreach (var item in lstUser)
                        {
                            notification.ReceiveId = item.Id;
                            notification.ReceiveOrgId = item.UnitId;
                            // insert notification
                            await new SYNotification(_appSetting).SYNotificationInsertDAO(notification);
                        }

                        // người gửi PAKN
                        notification.ReceiveId = sender.Id;
                        await new SYNotification(_appSetting).SYNotificationInsertDAO(notification);

                        break;
                    case STATUS_RECOMMENDATION.PROCESS_WAIT: //5 Chờ giải quyết

                        lstRMForward = (await new MR_RecommendationForward(_appSetting).MRRecommendationForwardGetByRecommendationId(recommendationId)).ToList();

                        var check = lstRMForward.Where(x => x.Step == 1).FirstOrDefault();
                        if (check == null)
                        {
                            // người dân doanh nghiệp gửi luôn PAKN cho đơn vị đã xác định thì phải tạo thông báo của status trước đó
                            // status 4
                            notification.Title = "PAKN ĐÃ TIẾP NHẬN";
                            notification.Content = "PAKN " + recommendation.Code + " đã được tiếp nhận giải quyết: đơn vị được yêu cầu giải quyết đã tiếp nhận PAKN";
                            foreach (var item in lstUser)
                            {
                                notification.ReceiveId = item.Id;
                                notification.ReceiveOrgId = item.UnitId;
                                // insert notification
                                await new SYNotification(_appSetting).SYNotificationInsertDAO(notification);
                            }

                            // người gửi PAKN
                            notification.ReceiveId = sender.Id;
                            await new SYNotification(_appSetting).SYNotificationInsertDAO(notification);
                        }

                        unitReceiveId = lstRMForward.FirstOrDefault(x => x.Step == 2).UnitReceiveId;
                        listUserReceiveResolve = await new SYUserGetByUnitId(_appSetting).SYUserGetByUnitIdDAO(unitReceiveId);
                        notification.Title = "PAKN ĐANG CHỜ GIẢI QUYẾT";
                        notification.Content = "PAKN " + recommendation.Code + " từ đơn vị " + unit.Name + " được gửi tới yêu cầu giải quyết";

                        foreach (var item in listUserReceiveResolve)
                        {
                            notification.ReceiveId = item.Id;
                            notification.ReceiveOrgId = item.UnitId;
                            // insert notification
                            await new SYNotification(_appSetting).SYNotificationInsertDAO(notification);
                        }

                        break;
                    case STATUS_RECOMMENDATION.PROCESS_DENY: //6 Từ chối giải quyết

                        lstRMForward = (await new MR_RecommendationForward(_appSetting).MRRecommendationForwardGetByRecommendationId(recommendationId)).ToList();
                        unitReceiveId = lstRMForward.FirstOrDefault(x => x.Step == 2).UnitReceiveId;
                        unitReceive = await new SYUnit(_appSetting).SYUnitGetByID(unitReceiveId);
                        // gửi cho đơn vị tiếp nhận ban đầu
                        notification.Title = "PAKN BỊ TỪ CHỐI GIẢI QUYẾT";
                        notification.Content = "PAKN số " + recommendation.Code +" đã bị " + unitReceive.Name + " từ chối giải quyết";
                        foreach (var item in lstUser)
                        {
                            notification.ReceiveId = item.Id;
                            notification.ReceiveOrgId = item.UnitId;
                            // insert notification
                            await new SYNotification(_appSetting).SYNotificationInsertDAO(notification);
                        }

                        // người gửi PAKN
                        
                        notification.Content = "PAKN của bạn đã bị " + unitReceive.Name + " từ chối giải quyết";
                        notification.ReceiveId = sender.Id;
                        notification.ReceiveOrgId = null;
                        await new SYNotification(_appSetting).SYNotificationInsertDAO(notification);

                        break;
                    case STATUS_RECOMMENDATION.PROCESSING: //7 Đang giải quyết
                        lstRMForward = (await new MR_RecommendationForward(_appSetting).MRRecommendationForwardGetByRecommendationId(recommendationId)).ToList();
                        unitReceiveId = lstRMForward.FirstOrDefault(x => x.Step == 2).UnitReceiveId;
                        unitReceive = await new SYUnit(_appSetting).SYUnitGetByID(unitReceiveId);

                        notification.Title = "PAKN ĐANG GIẢI QUYẾT";
                        notification.Content = "PAKN của bạn đã được gửi cho " + unitReceive.Name + " giải quyết";
                        notification.ReceiveId = sender.Id;
                        await new SYNotification(_appSetting).SYNotificationInsertDAO(notification);
                        break;
                    case STATUS_RECOMMENDATION.APPROVE_WAIT: //8 Chờ phê duyệt
                        // bạn có 1 PAKN chờ phê duyệt
                        lstRMForward = (await new MR_RecommendationForward(_appSetting).MRRecommendationForwardGetByRecommendationId(recommendationId)).ToList();
                        receiveId = lstRMForward.FirstOrDefault(x => x.Step == 3).ReceiveId;
                        approver = await new SYUser(_appSetting).SYUserGetByID(receiveId);

                        // gửi cho lãnh đạo
                        notification.Title = "PAKN CHỜ PHÊ DUYỆT";
                        notification.Content = "Bạn có PAKN số " + recommendation.Code + " chờ phê duyệt";
                        notification.ReceiveId = approver.Id;
                        await new SYNotification(_appSetting).SYNotificationInsertDAO(notification);

                        break;
                    case STATUS_RECOMMENDATION.APPROVE_DENY: //9 Từ chối phê duyệt

                        lstRMForward = (await new MR_RecommendationForward(_appSetting).MRRecommendationForwardGetByRecommendationId(recommendationId)).ToList();
                        unitReceiveId = lstRMForward.FirstOrDefault(x => x.Step == 3).UnitReceiveId;
                        unitReceive = await new SYUnit(_appSetting).SYUnitGetByID(unitReceiveId);

                        notification.Title = "PAKN ĐÃ BỊ TỪ CHỐI PHÊ DUYỆT";
                        notification.Content = "Lãnh đạo đơn vị " + unitReceive.Name + " đã từ chối kết quả giải quyết PAKN số" + recommendation.Code;
                        foreach (var item in lstUser)
                        {
                            notification.ReceiveId = item.Id;
                            notification.ReceiveOrgId = item.UnitId;
                            // insert notification
                            await new SYNotification(_appSetting).SYNotificationInsertDAO(notification);
                        }
                        // gửi cho người tiếp nhận PAKN -chưa chắc là người giải quyết nhá

                        notification.ReceiveId = lstRMForward.FirstOrDefault(x => x.Step == 2).ReceiveId;
                        notification.ReceiveOrgId = lstRMForward.FirstOrDefault(x => x.Step == 2).UnitReceiveId;
                        await new SYNotification(_appSetting).SYNotificationInsertDAO(notification);

                        //người gửi PAKN
                        notification.ReceiveId = sender.Id;
                        notification.ReceiveOrgId = null;
                        notification.Content = "Lãnh đạo đơn vị " + unitReceive.Name + " đã từ chối phê duyệt PAKN số " + recommendation.Code + " của bạn";
                        await new SYNotification(_appSetting).SYNotificationInsertDAO(notification);

                        break;
                    case STATUS_RECOMMENDATION.FINISED: //10 Đã giải quyết

                        lstRMForward = (await new MR_RecommendationForward(_appSetting).MRRecommendationForwardGetByRecommendationId(recommendationId)).ToList();
                        unitReceiveId = lstRMForward.FirstOrDefault(x => x.Step == 2).UnitReceiveId;
                        unitReceive = await new SYUnit(_appSetting).SYUnitGetByID(unitReceiveId);

                        notification.Title = "PAKN ĐÃ GIẢI QUYẾT XONG";
                        notification.Content = "Lãnh đạo đơn vị " + unitReceive.Name + " đã giải quyết PAKN số " + recommendation.Code;
                        foreach (var item in lstUser)
                        {
                            notification.ReceiveId = item.Id;
                            notification.ReceiveOrgId = item.UnitId;
                            // insert notification
                            await new SYNotification(_appSetting).SYNotificationInsertDAO(notification);
                        }

                        // gửi cho người tiếp nhận PAKN -chưa chắc là người giải quyết nhá

                        notification.ReceiveId = lstRMForward.FirstOrDefault(x => x.Step == 2).ReceiveId;
                        notification.ReceiveOrgId = lstRMForward.FirstOrDefault(x => x.Step == 2).UnitReceiveId;
                        await new SYNotification(_appSetting).SYNotificationInsertDAO(notification);


                        // người gửi PAKN
                        notification.ReceiveId = sender.Id;
                        notification.ReceiveOrgId = null;
                        await new SYNotification(_appSetting).SYNotificationInsertDAO(notification);
                        break;
                }
                return new ResultApi { Success = ResultCode.OK, Message = "Success" };
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }


        #endregion SYNotificationInsertTypeRecommendation


        #region SYNotificationGetAll
        [HttpPost]
        [Authorize]
        [Route("SYNotificationGetListOnPage")]
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
        [HttpGet]
        [Authorize]
        [Route("SYNotificationUpdateIsViewed")]
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

        [HttpPost]
        [Authorize]
        [Route("SYNotificationDelete")]
        public async Task<ActionResult<object>> SYNotificationDelete(SYNotificationModel _syNotification)
        {
            try
            {
                int count = await new SYNotification(_appSetting).SYNotificationDelete(_syNotification);
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
