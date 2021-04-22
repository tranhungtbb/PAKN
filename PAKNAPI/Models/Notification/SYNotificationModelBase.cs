using Dapper;
using PAKNAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Models.ModelBase
{
	public class SYNotificationModel
	{
		public long Id { get; set; }
		public long SenderId { get; set; }
		public int SendOrgId { get; set; }
		public long ReceiveId { get; set; }
		public int? ReceiveOrgId { get; set; }
		public int DataId { get; set; }
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

		public SYNotification(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		

		public SYNotification()
		{
		}

		public async Task<int?> SYNotificationInsertDAO(SYNotificationModel _syNotificationModel) {
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

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_NotificationInsert", DP));
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



	}
}
