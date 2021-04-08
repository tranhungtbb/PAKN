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
            DP.Add("CreateDate", _rMRemind.CreateDate);
            DP.Add("UnitId", _rMRemind.UnitId);
            DP.Add("Name", _rMRemind.Name);

            return (await _sQLCon.ExecuteScalarDapperAsync<decimal?>("[RM_RemindInsert]", DP));
        }
        //public int? RMRemindInsert(RMRemindModel _rMRemind)
        //{
        //    DynamicParameters DP = new DynamicParameters();
        //    DP.Add("Content", _rMRemind.Content);
        //    DP.Add("RecommendationId", _rMRemind.RecommendationId);

        //    return  _sQLCon.ExecuteScalarDapper<int?>("[RM_RemindInsert]", DP);
        //}
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
            DP.Add("SenderId", _rMForward.SenderId);
            DP.Add("SendOrgId", _rMForward.SendOrgId);
            DP.Add("ReceiveOrgId", _rMForward.ReceiveOrgId);
            DP.Add("DateSend", _rMForward.DateSend);
            DP.Add("IsView", _rMForward.IsView);

            return (await _sQLCon.ExecuteNonQueryDapperAsync("[RM_ForwardInsert]", DP));
        }
    }
}
