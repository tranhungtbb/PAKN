﻿using PAKNAPI.Common;
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


        #region SYNotificationInsertTypeRecommendation

        [HttpPost]
        [Authorize]
        [Route("SYNotificationInsertTypeRecommendation")]
        public async Task<object> SYNotificationInsertTypeRecommendation(int? recommendationId)
        {
            try
            {
                // thông tin PAKN
                var recommendation = new RecommendationDAO(_appSetting).RecommendationGetByID(recommendationId).Result.Model;

                //thông tin người gửi PAKN
                SYUser sender = await new SYUser(_appSetting).SYUserGetByID(recommendation.SendId);

                // danh sách người thuộc đơn vị mà PAKN gửi đến
                List<SYUser> lstUser = await new SYUser(_appSetting).SYUserGetByUnitId((int)recommendation.UnitId);

                List<RecommendationForward> lstRMForward = new List<RecommendationForward>();
                int unitReceiveId ,receiveId;

                // danh sách người dùng thuộc đơn vị mà PAKN gửi đến đơn vị đó
                List<SYUser> listUserReceiveResolve = new List<SYUser>();

                // thông tin người giải quyết
                SYUser approver = new SYUser();

                // thông tin đơn vị giải quyết
                SYUnit unitReceive = new SYUnit();

                // thông tin đơn vị tiếp nhận PAKN
                SYUnit unit = await new SYUnit(_appSetting).SYUnitGetByID(recommendation.UnitId); 

                // lấy thông tin đơn vị người đăng nhập
                //SYUserGetByID userInfo = (await new SYUserGetByID(_appSetting).SYUserGetByIDDAO(new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext))).FirstOrDefault();

                // obj thông báo
                SYNotificationModel notification = new SYNotificationModel();
                notification.SenderId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
                notification.SendOrgId = new LogHelper(_appSetting).GetUnitIdFromRequest(HttpContext); ;
                notification.DataId = recommendation.Id;
                notification.SendDate = DateTime.Now;
                notification.Type = 2;
                notification.TypeSend = recommendation.Status;
                notification.IsViewed = true;


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
                            await new SYNotificationInsert(_appSetting).SYNotificationInsertDAO(notification);
                        }
                        
                        // người gửi PAKN

                        notification.ReceiveId = sender.Id;
                        notification.Title = "PAKN của bạn đã được tiếp nhận.";
                        await new SYNotificationInsert(_appSetting).SYNotificationInsertDAO(notification);
                        break;
                    case STATUS_RECOMMENDATION.RECEIVE_DENY: //3 Từ chối xử lý

                        // người gửi PAKN

                        notification.ReceiveId = sender.Id;
                        notification.Title = "PAKN BỊ TỪ CHỐI";
                        notification.Content = "Phản ánh kiến nghị của bạn đã bị từ chối.";
                        await new SYNotificationInsert(_appSetting).SYNotificationInsertDAO(notification);

                        break;
                    case STATUS_RECOMMENDATION.RECEIVE_APPROVED: //4 Đã tiếp nhận

                        notification.Title = "PAKN ĐÃ TIẾP NHẬN";
                        notification.Content = "PAKN đã được tiếp nhận giải quyết: đơn vị được yêu cầu giải quyết đã tiếp nhận PAKN";
                        foreach (var item in lstUser)
                        {
                            notification.ReceiveId = item.Id;
                            notification.ReceiveOrgId = item.UnitId;
                            // insert notification
                            await new SYNotificationInsert(_appSetting).SYNotificationInsertDAO(notification);
                        }

                        // người gửi PAKN
                        notification.ReceiveId = sender.Id;
                        await new SYNotificationInsert(_appSetting).SYNotificationInsertDAO(notification);

                        break;
                    case STATUS_RECOMMENDATION.PROCESS_WAIT: //5 Chờ giải quyết

                        notification.Title = "PAKN ĐANG CHỜ GIẢI QUYẾT";
                        notification.Content = "PAKN từ đơn vị " + unit.Name + " được gửi tới yêu cầu giải quyết";

                        lstRMForward = (await new MR_RecommendationForward(_appSetting).MRRecommendationForwardGetByRecommendationId(recommendationId)).ToList();
                        unitReceiveId = lstRMForward.FirstOrDefault(x => x.Step == 2).UnitReceiveId;
                        listUserReceiveResolve = await new SYUser(_appSetting).SYUserGetByUnitId(unitReceiveId);

                        foreach (var item in listUserReceiveResolve)
                        {
                            notification.ReceiveId = item.Id;
                            notification.ReceiveOrgId = item.UnitId;
                            // insert notification
                            await new SYNotificationInsert(_appSetting).SYNotificationInsertDAO(notification);
                        }

                        break;
                    case STATUS_RECOMMENDATION.PROCESS_DENY: //6 Từ chối giải quyết

                        lstRMForward = (await new MR_RecommendationForward(_appSetting).MRRecommendationForwardGetByRecommendationId(recommendationId)).ToList();
                        unitReceiveId = lstRMForward.FirstOrDefault(x => x.Step == 2).UnitReceiveId;
                        unitReceive = await new SYUnit(_appSetting).SYUnitGetByID(unitReceiveId);

                        // người gửi PAKN
                        notification.Title = "PAKN BỊ TỪ CHỐI GIẢI QUYẾT";
                        notification.Content = "PAKN của bạn đã bị " + unitReceive.Name + " từ chối giải quyết";
                        notification.ReceiveId = sender.Id;
                        await new SYNotificationInsert(_appSetting).SYNotificationInsertDAO(notification);

                        break;
                    //case STATUS_RECOMMENDATION.PROCESSING: //7 Đang giải quyết
                    //    break;
                    case STATUS_RECOMMENDATION.APPROVE_WAIT: //8 Chờ phê duyệt
                        // bạn có 1 PAKN chờ phê duyệt
                        lstRMForward = (await new MR_RecommendationForward(_appSetting).MRRecommendationForwardGetByRecommendationId(recommendationId)).ToList();
                        receiveId = lstRMForward.FirstOrDefault(x => x.Step == 2).ReceiveId;
                        approver = await new SYUser(_appSetting).SYUserGetByID(receiveId);

                        // gửi cho lãnh đạo
                        notification.Title = "PAKN CHỜ PHÊ DUYỆT";
                        notification.Content = "Bạn có một PAKN chờ phê duyệt";
                        notification.ReceiveId = approver.Id;
                        await new SYNotificationInsert(_appSetting).SYNotificationInsertDAO(notification);

                        break;
                    case STATUS_RECOMMENDATION.APPROVE_DENY: //9 Từ chối phê duyệt

                        lstRMForward = (await new MR_RecommendationForward(_appSetting).MRRecommendationForwardGetByRecommendationId(recommendationId)).ToList();
                        unitReceiveId = lstRMForward.FirstOrDefault(x => x.Step == 2).UnitReceiveId;
                        unitReceive = await new SYUnit(_appSetting).SYUnitGetByID(unitReceiveId);

                        notification.Title = "PAKN ĐÃ BỊ TỪ CHỐI PHÊ DUYỆT";
                        notification.Content = "Lãnh đạo đơn vị "+ unitReceive.Name + " đã từ chối kết quả giải quyết PAKN";
                        foreach (var item in lstUser)
                        {
                            notification.ReceiveId = item.Id;
                            notification.ReceiveOrgId = item.UnitId;
                            // insert notification
                            await new SYNotificationInsert(_appSetting).SYNotificationInsertDAO(notification);
                        }

                        //người gửi PAKN
                        notification.ReceiveId = sender.Id;
                        notification.Content = "Lãnh đạo đơn vị " + unitReceive.Name + " đã từ chối phê duyệt PAKN";
                        await new SYNotificationInsert(_appSetting).SYNotificationInsertDAO(notification);

                        break;
                    case STATUS_RECOMMENDATION.FINISED: //10 Đã giải quyết

                        lstRMForward = (await new MR_RecommendationForward(_appSetting).MRRecommendationForwardGetByRecommendationId(recommendationId)).ToList();
                        unitReceiveId = lstRMForward.FirstOrDefault(x => x.Step == 2).UnitReceiveId;
                        unitReceive = await new SYUnit(_appSetting).SYUnitGetByID(unitReceiveId);

                        notification.Title = "PAKN ĐÃ GIẢI QUYẾT XONG";
                        notification.Content = "Lãnh đạo đơn vị " + unitReceive.Name + " đã giải quyết PAKN";
                        foreach (var item in lstUser)
                        {
                            notification.ReceiveId = item.Id;
                            notification.ReceiveOrgId = item.UnitId;
                            // insert notification
                            await new SYNotificationInsert(_appSetting).SYNotificationInsertDAO(notification);
                        }

                        // người gửi PAKN
                        notification.ReceiveId = sender.Id;
                        await new SYNotificationInsert(_appSetting).SYNotificationInsertDAO(notification);
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


    }
}
