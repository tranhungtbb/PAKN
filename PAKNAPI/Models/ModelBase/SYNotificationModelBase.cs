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

		public SYNotificationModel() { }
	}
    public class SYNotificationInsert
    {
		private SQLCon _sQLCon;

		public SYNotificationInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		

		public SYNotificationInsert()
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

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_NotificationInsert", DP));
		}
	}

	public class SYNotificationGetListByUserId
	{
		private SQLCon _sQLCon;

		public SYNotificationGetListByUserId(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}



		public SYNotificationGetListByUserId()
		{
		}

		public async Task<List<SYNotificationModel>> SYPermissionCheckByUserIdDAO(int? UserId)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UserId", UserId);

			return (await _sQLCon.ExecuteListDapperAsync<SYNotificationModel>("SY_PermissionCheckByUserId", DP)).ToList();
		}



	}
}
