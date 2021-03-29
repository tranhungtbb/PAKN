using Dapper;
using PAKNAPI.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace PAKNAPI.Models.Recommendation
{

	public class RecommendationDAO
    {
		private SQLCon _sQLCon;
		public RecommendationDAO(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public async Task<RecommendationGetDataForCreateResponse> RecommendationGetDataForCreate()
		{
			RecommendationGetDataForCreateResponse data = new RecommendationGetDataForCreateResponse();
			DynamicParameters DP = new DynamicParameters();
			data.lstField = (await _sQLCon.ExecuteListDapperAsync<DropdownObject>("CA_PositionGetDropdown", DP)).ToList();
			data.lstUnit = (await _sQLCon.ExecuteListDapperAsync<DropdownObject>("SY_UnitGetDropdown", DP)).ToList();
			data.lstBusiness = (await _sQLCon.ExecuteListDapperAsync<DropdownObject>("BI_BusinessGetDropdown", DP)).ToList();
			data.lstIndividual = (await _sQLCon.ExecuteListDapperAsync<DropdownObject>("BI_IndividualGetDropdown", DP)).ToList();
			data.lstHashTag = (await _sQLCon.ExecuteListDapperAsync<DropdownObject>("CA_HashtagGetDropdown", DP)).ToList();
			data.Code = await _sQLCon.ExecuteScalarDapperAsync<string>("MR_Recommendation_GenCode_GetCode", DP);
			return data;
		}
	}
}