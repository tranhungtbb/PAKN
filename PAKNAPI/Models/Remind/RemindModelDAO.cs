using Dapper;
using PAKNAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Models.Remind
{
    public class RMRemindModelDAO
    {
        private SQLCon _sQLCon;
        public RMRemindModelDAO(IAppSetting appSetting)
        {
            _sQLCon = new SQLCon(appSetting.GetConnectstring());
        }

        public async Task<decimal?> RMRemindInsert(RMRemind _rMRemind)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("Content", _rMRemind.Content);
            DP.Add("PetitionId", _rMRemind.PetitionId);

            return (await _sQLCon.ExecuteScalarDapperAsync<decimal?>("[RM_RemindInsert]", DP));
        }
    }

    public class RMFileAttachDAO
    {
        private SQLCon _sQLCon;
        public RMFileAttachDAO(IAppSetting appSetting)
        {
            _sQLCon = new SQLCon(appSetting.GetConnectstring());
        }

        public async Task<int?> RMFileAttachInsert(RMFileAttach _rMFileAttach)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("RemindId", _rMFileAttach.RemindId);
            DP.Add("FileAttach", _rMFileAttach.FileAttach);
            DP.Add("Name", _rMFileAttach.Name);
            DP.Add("FileType", _rMFileAttach.FileType);

            return (await _sQLCon.ExecuteNonQueryDapperAsync("[RM_FileAttachInsert]", DP));
        }
    }

    public class RMForwardDAO
    {
        private SQLCon _sQLCon;
        public RMForwardDAO(IAppSetting appSetting)
        {
            _sQLCon = new SQLCon(appSetting.GetConnectstring());
        }

        public async Task<int?> RMFileAttachInsert(RMForward _rMForward)
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
