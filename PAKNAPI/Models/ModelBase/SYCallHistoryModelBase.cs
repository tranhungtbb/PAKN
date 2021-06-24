using Dapper;
using PAKNAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Models.ModelBase
{
    public class SYCallHistoryModelBase
    {

    }
    enum SYCallHistoryType
    {
        call_in,call_out, Missed
    }
    public class SYCallHistoryPagedList
    {
        private SQLCon _sQLCon;
        public SYCallHistoryPagedList(IAppSetting appSetting)
        {
            _sQLCon = new SQLCon(appSetting.GetConnectstring());
        }
        public SYCallHistoryPagedList()
        {
        }
        public long? RowNumber { get; set; }
        public long Id { get; set; }
        public string Phone { get; set; }
        public int Type { get; set; }
        public DateTime? StartDate { get; set; }
        public long? CallDuration { get; set; }

        public async Task<List<SYCallHistoryPagedList>> GetData(int? type, string phone, int pageIndex =1, int pageSize = 20)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("Type", type);
            DP.Add("Phone",phone);
            DP.Add("PageIndex", pageIndex);
            DP.Add("PageSize", pageSize);
            return (await _sQLCon.ExecuteListDapperAsync<SYCallHistoryPagedList>("[SY_CallHistory_GetPageList]", DP)).ToList();
        }

        public async Task<int> Delete(long id)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("id", id);
            return await _sQLCon.ExecuteNonQueryDapperAsync("[SY_CallHistory_Delete]", DP);
            
        }
    }
}
