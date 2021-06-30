using Dapper;
using PAKNAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Models.ModelBase
{
	public class MrGraphCountByStatusModel
    {
		public int? Total { get; set; }
		public int Status { get; set; }
    }
    public class MrRecommendationGetGraphBase
    {
		private SQLCon _sQLCon;

		public MrRecommendationGetGraphBase(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MrRecommendationGetGraphBase()
		{
		}

		public async Task<List<MrGraphCountByStatusModel>> Get7DayGraphData(int? UnitProcessId, long? UserProcessId)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UnitProcessId", UnitProcessId);
			DP.Add("UserProcessId", UserProcessId);

			return (await _sQLCon.ExecuteListDapperAsync<MrGraphCountByStatusModel>("[MR_RecommendationGet7DayDataGraph]", DP)).ToList();
		}
		public async Task<List<MrGraphCountByStatusModel>> GetGraphData(int? UnitProcessId, long? UserProcessId)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UnitProcessId", UnitProcessId);
			DP.Add("UserProcessId", UserProcessId);

			return (await _sQLCon.ExecuteListDapperAsync<MrGraphCountByStatusModel>("[MR_RecommendationGetDataGraph]", DP)).ToList();
		}
	}
}
