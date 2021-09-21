using Dapper;
using PAKNAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Models.Remind
{
    public class RMRemind
    {
        private SQLCon _sQLCon;
        public RMRemind(IAppSetting appSetting)
        {
            _sQLCon = new SQLCon(appSetting.GetConnectstring());
        }

        public async Task<decimal?> RMRemindInsert(RMRemindModel _rMRemind)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("Content", _rMRemind.Content);
            DP.Add("RecommendationId", _rMRemind.RecommendationId);

            return (await _sQLCon.ExecuteScalarDapperAsync<decimal?>("[RM_RemindInsert]", DP));
        }

        public async Task<List<RMRemindObject>> RMRemindGetList(int? RecommendationID,int? OrgId,bool? IsSenderOrg)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("RecommendationId", RecommendationID);
            DP.Add("OrgId", OrgId);
            DP.Add("IsSenderOrg", IsSenderOrg);
            return (await _sQLCon.ExecuteListDapperAsync<RMRemindObject>("[RM_RemindGetList]", DP)).ToList();
        }
        public async Task<List<RMRemindObject>> RMRemindGetListDashBoard(int? OrgId)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("OrgId", OrgId);
            return (await _sQLCon.ExecuteListDapperAsync<RMRemindObject>("RM_RemindGetListDashBoard", DP)).ToList();
        }
    }

    public class RMFileAttach
    {
        private SQLCon _sQLCon;
        public RMFileAttach(IAppSetting appSetting)
        {
            _sQLCon = new SQLCon(appSetting.GetConnectstring());
        }

        public async Task<int?> RMFileAttachInsert(RMFileAttachModel _rMFileAttach)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("RemindId", _rMFileAttach.RemindId);
            DP.Add("FileAttach", _rMFileAttach.FileAttach);
            DP.Add("Name", _rMFileAttach.Name);
            DP.Add("FileType", _rMFileAttach.FileType);

            return (await _sQLCon.ExecuteNonQueryDapperAsync("[RM_FileAttachInsert]", DP));
        }

        public async Task<List<RMFileAttachModel>> RMFileAttachGetByRemindID(int? Id)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("Id", Id);
            return (await _sQLCon.ExecuteListDapperAsync<RMFileAttachModel>("[RM_FileAttachGetByRemindID]", DP)).ToList();
        }
    }

    public class RMForward
    {
        private SQLCon _sQLCon;
        public RMForward(IAppSetting appSetting)
        {
            _sQLCon = new SQLCon(appSetting.GetConnectstring());
        }

        public async Task<int?> RMFileAttachInsert(RMForwardModel _rMForward)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("RemindId", _rMForward.RemindId);
            DP.Add("SenderId", _rMForward.SenderId);
            DP.Add("SenderName", _rMForward.SenderName);
            DP.Add("SendOrgId", _rMForward.SendOrgId);
            DP.Add("ReceiveOrgId", _rMForward.ReceiveOrgId);
            DP.Add("DateSend", _rMForward.DateSend);
            DP.Add("IsView", _rMForward.IsView);

            return (await _sQLCon.ExecuteNonQueryDapperAsync("[RM_ForwardInsert]", DP));
        }
    }

    public class MR_RecommendationForward
    {
        private SQLCon _sQLCon;

        public MR_RecommendationForward(IAppSetting appSetting)
        {
            _sQLCon = new SQLCon(appSetting.GetConnectstring());
        }


        public async Task<List<RecommendationForward>> MRRecommendationForwardGetByRecommendationId(int? RecommendationId)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("RecommendationId", RecommendationId);
            return (await _sQLCon.ExecuteListDapperAsync<RecommendationForward>("MR_Recommendation_ForwardGetByRecommendationId", DP)).ToList();
        }
    }

    public class RecommendationForward
    {
        public int Id { get; set; }
        public int RecommendationId { get; set; }
        public int UserSendId { get; set; }
        public int ReceiveId { get; set; }
        public int UnitReceiveId { get; set; }
        public int UnitSendId { get; set; }
        public int Step { get; set; }
    }


}
