using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PAKNAPI.Common;
using PAKNAPI.ModelBase;
using PAKNAPI.Models.ModelBase;
using PAKNAPI.Models.Remind;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Services
{
    public class Notification
    {
        private readonly IAppSetting _appSetting;
        private readonly HttpContext _context;
        private readonly IConfiguration _configuration;
        public Notification(IAppSetting appSetting, HttpContext context, IConfiguration configuration)
        {
            _appSetting = appSetting;
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// insert thong bao cho tham muu nhieu don vi
        /// </summary>
        /// <param name="RecommendationId"></param>
        /// <param name="ListUnit"></param>
        public async Task<bool> NotificationInsertForCombine(long? RecommendationId, List<int> ListUnit)
        {
            try
            {
                var mr = (await new MRRecommendationGetByID(_appSetting).MRRecommendationGetByIDDAO((int)RecommendationId)).FirstOrDefault();
                var lstRMForward = (await new MR_RecommendationForward(_appSetting).MRRecommendationForwardGetByRecommendationId((int)RecommendationId)).ToList();
                var unitReceive = lstRMForward.Where(x => x.Status == 2).FirstOrDefault();//
                var unitMain = (await new SYUnitGetNameById(_appSetting).SYUnitGetNameByIdDAO(new LogHelper(_appSetting).GetUnitIdFromRequest(_context))).FirstOrDefault().Name;
                if (unitReceive != null)
                {
                    SYNotificationModel notification = new SYNotificationModel();
                    notification.SenderId = new LogHelper(_appSetting).GetUserIdFromRequest(_context);
                    notification.SendOrgId = new LogHelper(_appSetting).GetUnitIdFromRequest(_context);
                    notification.DataId = (int)RecommendationId;
                    notification.SendDate = DateTime.Now;
                    notification.Type = TYPENOTIFICATION.RECOMMENDATION;
                    notification.TypeSend = STATUS_RECOMMENDATION.COMBINE;
                    notification.IsViewed = true;
                    notification.IsReaded = true;
                    var tasks = new List<Task>();

                    foreach (var unit in ListUnit)
                    {
                        List<SYUserGetByUnitId> lstUser = await new SYUserGetByUnitId(_appSetting).SYUserGetByUnitIdDAO(unit);
                        var unitName = (await new SYUnitGetNameById(_appSetting).SYUnitGetNameByIdDAO(unit)).FirstOrDefault().Name;
                        foreach (var item in lstUser)
                        {
                            notification.ReceiveId = item.Id;
                            notification.ReceiveOrgId = item.UnitId ?? 0;
                            notification.Title = "PAKN ĐƯỢC YÊU CẦU PHỐI HỢP/THAM MƯU";
                            notification.SendOrgId = 0;
                            notification.Content = "PAKN số " + mr.Code + " từ " + unitMain + " được yêu cầu phối hợp giải quyết";
                            // insert notification
                            tasks.Add(new SYNotification(_appSetting, _configuration).InsertNotification(notification));
                        }
                    }

                    Task.WaitAll(tasks.ToArray());
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public async Task<bool> NotificationInsertForCombineProcess(long? RecommendationId, int Status)
        {
            try
            {
                SYNotificationModel notification = new SYNotificationModel();
                notification.SenderId = new LogHelper(_appSetting).GetUserIdFromRequest(_context);
                notification.SendOrgId = new LogHelper(_appSetting).GetUnitIdFromRequest(_context);
                notification.DataId = (int)RecommendationId;
                notification.SendDate = DateTime.Now;
                notification.Type = TYPENOTIFICATION.RECOMMENDATION;
                notification.TypeSend = Status;
                notification.IsViewed = true;
                notification.IsReaded = true;
                var tasks = new List<Task>();

                List<SYUserGetByUnitId> lstUser = new List<SYUserGetByUnitId>();
                var lstRMForward = (await new MR_RecommendationForward(_appSetting).MRRecommendationForwardGetByRecommendationId((int)RecommendationId)).ToList();
                var unitReceive = lstRMForward.Where(x => x.Status == 2).FirstOrDefault();//

                var mr = (await new MRRecommendationGetByID(_appSetting).MRRecommendationGetByIDDAO((int)RecommendationId)).FirstOrDefault();
                string unitNameReceive = (await new SYUnitGetNameById(_appSetting).SYUnitGetNameByIdDAO(notification.SendOrgId)).FirstOrDefault().Name;



                if (unitReceive != null && (Status == STATUS_RECOMMENDATION.PROCESS_DENY || Status == STATUS_RECOMMENDATION.PROCESSING || Status == STATUS_RECOMMENDATION.FINISED))
                {
                    switch (Status)
                    {

                        case STATUS_RECOMMENDATION.PROCESS_DENY:
                            notification.Title = "PAKN BỊ TỪ CHỐI PHỐI HỢP/THAM MƯU GIẢI QUYẾT";
                            notification.Content = "PAKN số " + mr.Code + " đã bị đơn vị " + unitNameReceive + " từ chối giải quyết";
                            break;
                        case STATUS_RECOMMENDATION.PROCESSING:
                            notification.Title = "PAKN PHỐI HỢP/THAM MƯU ĐÃ ĐƯỢC TIẾP NHẬN GIẢI QUYẾT";
                            notification.Content = "PAKN số " + mr.Code + " đã được đơn vị " + unitNameReceive + " tiếp nhận giải quyết";
                            break;
                        case STATUS_RECOMMENDATION.FINISED:
                            notification.Title = "PAKN ĐÃ ĐƯỢC PHỐI HỢP/THAM MƯU";
                            notification.Content = "PAKN số " + mr.Code + " đã được đơn vị " + unitNameReceive + " giải quyết";
                            break;
                    }
                    lstUser = await new SYUserGetByUnitId(_appSetting).SYUserGetByUnitIdDAO(unitReceive.UnitReceiveId);
                    foreach (var item in lstUser)
                    {
                        notification.ReceiveId = item.Id;
                        notification.ReceiveOrgId = item.UnitId ?? 0;
                        notification.SendOrgId = 0;
                        tasks.Add(new SYNotification(_appSetting, _configuration).InsertNotification(notification));
                    }
                }
                else
                {
                    return false;
                }
                Task.WaitAll(tasks.ToArray());

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public async Task<bool> NotificationInsertForCombineApprove(long? RecommendationId, long? ReceiveId)
        {
            try
            {
                var mr = (await new MRRecommendationGetByID(_appSetting).MRRecommendationGetByIDDAO((int)RecommendationId)).FirstOrDefault();
                SYNotificationModel notification = new SYNotificationModel();
                notification.SenderId = new LogHelper(_appSetting).GetUserIdFromRequest(_context);
                notification.SendOrgId = new LogHelper(_appSetting).GetUnitIdFromRequest(_context);
                notification.DataId = (int)RecommendationId;
                notification.SendDate = DateTime.Now;
                notification.Type = TYPENOTIFICATION.RECOMMENDATION;
                notification.TypeSend = STATUS_RECOMMENDATION.APPROVE_WAIT;
                notification.IsViewed = true;
                notification.IsReaded = true;
                notification.Title = "PAKN CHỜ PHÊ DUYỆT";
                notification.Content = "Bạn có PAKN số " + mr.Code + " chờ phê duyệt";
                notification.ReceiveId = ReceiveId; // nguoi phe duyet
                notification.ReceiveOrgId = notification.SendOrgId;
                await new SYNotification(_appSetting, _configuration).InsertNotification(notification);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
