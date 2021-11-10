using Dapper;
using Microsoft.Extensions.Configuration;
using PAKNAPI.App_Helper;
using PAKNAPI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Models.ModelBase
{
	public class SYNotificationModel
	{
		public long? Id { get; set; }
		public long? SenderId { get; set; }
		public int? SendOrgId { get; set; }
		public long? ReceiveId { get; set; }
		public int? ReceiveOrgId { get; set; }
		public long? DataId { get; set; }
		public DateTime SendDate { get; set; }
		public int Type { get; set; }
		public int? TypeSend { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public bool IsViewed { get; set; }
		public bool IsReaded { get; set; }
		public int RowNumber { get; set; }
		public int ViewedCount { get; set; }
		public SYNotificationModel() { }
	}

	public class SYNotificationGetList {
		public int PageSize { get; set; }
		public int PageIndex { get; set; }
		public string? Title { get; set; }
		public string? Content { get; set; }
		public int? Type { get; set; }
		public DateTime? SendDate { get; set; }

		public SYNotificationGetList(){}
	}
    public class SYNotification
    {
		private SQLCon _sQLCon;
		private readonly IConfiguration _configuration;

		public SYNotification(IAppSetting appSetting, IConfiguration configuration)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
			_configuration = configuration;
		}

		public SYNotification()
		{
		}

		public async Task<int> UpdateTokenFireBaseDAO(long userId, UpdateTokenFireBaseRequest request)
		{
			DynamicParameters DP = new DynamicParameters();
			if (!request.Remove)
			{
				DP.Add("UserId", userId);
				DP.Add("TokenFireBase", request.Token);
				return (await _sQLCon.ExecuteListDapperAsync<int>("SY_TokenNotification_Insert", DP)).First();
			}
			else
			{
				DP.Add("TokenFireBase", request.Token);
				return (await _sQLCon.ExecuteListDapperAsync<int>("SY_TokenNotification_Delete", DP)).First();
			}
		}

		public async Task<int> InsertNotification(SYNotificationModel _syNotificationModel)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("@SenderId", _syNotificationModel.SenderId);
			DP.Add("@SendOrgId", _syNotificationModel.SendOrgId);
			DP.Add("@ReceiveId", _syNotificationModel.ReceiveId);
			DP.Add("@ReceiveOrgId", _syNotificationModel.ReceiveOrgId);
			DP.Add("@DataId", _syNotificationModel.DataId);
			DP.Add("@SendDate", _syNotificationModel.SendDate);
			DP.Add("@Type", _syNotificationModel.Type);
			DP.Add("@TypeSend", _syNotificationModel.TypeSend);
			DP.Add("@Title", _syNotificationModel.Title);
			DP.Add("@Content", _syNotificationModel.Content);
			DP.Add("@IsViewed", _syNotificationModel.IsViewed);
			DP.Add("@IsReaded", _syNotificationModel.IsReaded);
			await _sQLCon.ExecuteNonQueryDapperAsync("[SY_NotificationInsert]", DP);
			if (true)
			{
				DP = new DynamicParameters();
				DP.Add("UserId", _syNotificationModel.ReceiveId);
				var lstUserFireBase = (await _sQLCon.ExecuteListDapperAsync<string>("SY_TokenNotification_GetToken", DP)).ToList();
				if (lstUserFireBase != null && lstUserFireBase.Count > 0)
				{
					NotifiDocumentJob(lstUserFireBase, _syNotificationModel);
				}
			}
			return 1;
		}

		//public async Task<int> InsertNotificationBase(SYNotificationModel _syNotificationModel, UpdateTokenFireBaseRequest request)
		//{
		//	DynamicParameters DP = new DynamicParameters();
		//	DP.Add("@SenderId", _syNotificationModel.SenderId);
		//	DP.Add("@SendOrgId", _syNotificationModel.SendOrgId);
		//	DP.Add("@ReceiveId", _syNotificationModel.ReceiveId);
		//	DP.Add("@ReceiveOrgId", _syNotificationModel.ReceiveOrgId);
		//	DP.Add("@DataId", _syNotificationModel.DataId);
		//	DP.Add("@SendDate", _syNotificationModel.SendDate);
		//	DP.Add("@Type", _syNotificationModel.Type);
		//	DP.Add("@TypeSend", _syNotificationModel.TypeSend);
		//	DP.Add("@Title", _syNotificationModel.Title);
		//	DP.Add("@Content", _syNotificationModel.Content);
		//	DP.Add("@IsViewed", _syNotificationModel.IsViewed);
		//	DP.Add("@IsReaded", _syNotificationModel.IsReaded);
		//	await _sQLCon.ExecuteNonQueryDapperAsync("[SY_NotificationInsert]", DP);
		//	List<string> token = new List<string>();
		//	token.Add(request.Token);
		//	NotifiDocumentJob(token, _syNotificationModel);
		//	return 1;
		//}

		public void SendNotificationWithTopData(List<string> tokenNotifies, SYNotificationModel notificationData)
		{
			List<string> lstDataAdd = new List<string>();
			var count = 0;
			for (int i = 0; i < tokenNotifies.Count; i++)
			{
				if (count == 1000)
				{
					NotifiDocumentJob(lstDataAdd, notificationData);
					lstDataAdd.Clear();
					count = 0;
				}
				else
				{
					lstDataAdd.Add(tokenNotifies[i]);
					count++;
				}
			}
			if (lstDataAdd.Count > 0)
			{
				NotifiDocumentJob(lstDataAdd, notificationData);
			}
		}
		public void NotifiDocumentJob(List<string> tokenNotifies, SYNotificationModel notificationData)
		{
			DateTime Created = DateTime.Now;
			string url = "https://fcm.googleapis.com/fcm/send";
			var notification = new NotificationMobileV2(new NotificationMobile
			{
				collapse_key = "type_a",
				notification = new NotificationHeader
				{
					title = notificationData.Title,
					body = notificationData.Content,
				},
				data = new NotificationData
				{
					Created = Created,
					IdElement = notificationData.DataId == null ? 0 : (int)notificationData.DataId,
					Type = (int)notificationData.Type,
					DatasNotification = notificationData
				}
			}, tokenNotifies);
			new WebRequestHelper(_configuration).SendTextFirebaseRequest(url, notification);
		}

		// delete

		public async Task<int> SYNotificationDelete(SYNotificationModel _syNotification)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _syNotification.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SYNotificationDelete", DP));
		}
	}

	public class SYNotificationGetListOnPageByReceiveId
	{
		private SQLCon _sQLCon;

		public SYNotificationGetListOnPageByReceiveId(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}



		public SYNotificationGetListOnPageByReceiveId()
		{
		}

		public async Task<List<SYNotificationModel>> SYNotificationGetListOnPageByReceiveIdDAO(int? ReceiveId, int PageSize, int PageIndex, string? Title, string? Content, int? Type, DateTime? SendDate)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", ReceiveId);
			DP.Add("Title", Title);
			DP.Add("Content", Content);
			DP.Add("Type", Type);
			DP.Add("SendDate", SendDate);
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);

			return (await _sQLCon.ExecuteListDapperAsync<SYNotificationModel>("SY_NotificationGetListByReceiveId", DP)).ToList();
		}

		public async Task<int> SYNotificatioUpdateIsViewedByReceiveIdDAO(int? ReceiveId)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("ReceiveId", ReceiveId);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_NotificationUpdateIsViewedByReceiveId", DP));
		}

		public async Task<int> SYNotificatioUpdateIsReadedDAO(int? ObjectId ,int? ReceiveId, DateTime WatchedDate)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("ObjectId", ObjectId);
			DP.Add("ReceiveId", ReceiveId);
			DP.Add("WatchedDate", WatchedDate);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_NotificationUpdateIsReaded", DP));
		}
	}

	public class SYNotificationGetById
	{
		private SQLCon _sQLCon;

		public SYNotificationGetById(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYNotificationGetById()
		{
		}

		public int Id { get; set; }
		public int? ReceiveId { get; set; }
		public int? SenderId { get; set; }

		public string SenderName { get; set; }
		public int? ReceiveOrgId { get; set; }
		public int? DataId { get; set; }
		public DateTime SendDate { get; set; }

		public int Type { get; set; }
		public int TypeSend { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public DateTime WatchedDate { get; set; }

		public async Task<List<SYNotificationGetById>> SYNotificationGetByIdDAO(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<SYNotificationGetById>("SY_NotificationGetById", DP)).ToList();
		}
	}

	public class UpdateTokenFireBase
	{
		private SQLCon _sQLCon;

		public UpdateTokenFireBase(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public UpdateTokenFireBase()
		{
		}

		public async Task<int> UpdateTokenFireBaseDAO(long userId, UpdateTokenFireBaseRequest request)
		{
			DynamicParameters DP = new DynamicParameters();
			if (!request.Remove)
			{
				DP.Add("UserId", userId);
				DP.Add("TokenFireBase", request.Token);
				return (await _sQLCon.ExecuteListDapperAsync<int>("SY_TokenNotification_Insert", DP)).First();
			}
			else
			{
				DP.Add("TokenFireBase", request.Token);
				return (await _sQLCon.ExecuteListDapperAsync<int>("SY_TokenNotification_Delete", DP)).First();
			}
		}
	}
	public class UpdateTokenFireBaseRequest
	{
		[Required]
		public string Token { set; get; }
		public bool Remove { set; get; }
	}

	public class NotificationMobileV2
	{
		public string collapse_key { set; get; }
		public NotificationHeader notification { set; get; }
		public NotificationData data { set; get; }
		public List<string> registration_ids { set; get; }
		public string priority = "high";
		public NotificationMobileV2(NotificationMobile notification, List<string> tokenNotifies)
		{
			this.notification = notification.notification;
			this.collapse_key = notification.collapse_key;
			this.data = notification.data;
			this.registration_ids = tokenNotifies;
		}

	}
	public class NotificationMobile
	{
		public string to { set; get; }
		public string collapse_key { set; get; }
		public bool content_available { get; set; }
		public NotificationHeader notification { set; get; }
		public NotificationData data { set; get; }
	}
	public class NotificationHeader
	{
		public string title { set; get; }
		public string body { set; get; }
	}
	public class NotificationData
	{
		public int? IdElement { set; get; }
		public int Type { set; get; }
		public Dictionary<string, object> Datas { set; get; }
		public int UserId { set; get; }
		public DateTime Created { set; get; }
		public SYNotificationModel DatasNotification { get; set; }
	}
}
