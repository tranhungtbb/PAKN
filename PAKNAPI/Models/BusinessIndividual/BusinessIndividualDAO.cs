using Dapper;
using PAKNAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Models.BusinessIndividual
{
    public class BusinessIndividualDAO
    {
        private SQLCon _sQLCon;
        public BusinessIndividualDAO(IAppSetting appSetting)
        {
            _sQLCon = new SQLCon(appSetting.GetConnectstring());
        }

		public async Task<BusinessIndividualGetDataForCreateResponse> BusinessIndividualGetDataForCreate()
		{
			BusinessIndividualGetDataForCreateResponse data = new BusinessIndividualGetDataForCreateResponse();
			DynamicParameters DP = new DynamicParameters();
			data.lstField = (await _sQLCon.ExecuteListDapperAsync<DropdownObject>("CA_FieldGetDropdown", DP)).ToList();
			data.lstUnit = (await _sQLCon.ExecuteListDapperAsync<DropdownTree>("SY_UnitGetDropdownLevel", DP)).ToList();
			data.lstBusiness = (await _sQLCon.ExecuteListDapperAsync<DropdownObject>("BI_BusinessGetDropdown", DP)).ToList();
			data.lstIndividual = (await _sQLCon.ExecuteListDapperAsync<DropdownObject>("BI_IndividualGetDropdown", DP)).ToList();
			data.lstHashTag = (await _sQLCon.ExecuteListDapperAsync<DropdownObject>("CA_HashtagGetDropdown", DP)).ToList();
			data.Code = await _sQLCon.ExecuteScalarDapperAsync<string>("MR_Recommendation_GenCode_GetCode", DP);
			return data;
		}

		public async Task<BusinessIndividualGetDataForForwardResponse> BusinessIndividualGetDataForForward()
		{
			BusinessIndividualGetDataForForwardResponse data = new BusinessIndividualGetDataForForwardResponse();
			DynamicParameters DP = new DynamicParameters();
			data.lstUnitNotMain = (await _sQLCon.ExecuteListDapperAsync<DropdownObject>("SY_UnitGetDropdownNotMain", DP)).ToList();
			return data;
		}

		public async Task<BusinessIndividualGetDataForProcessResponse> BusinessIndividualGetDataForProcess(int? UnitId)
		{
			BusinessIndividualGetDataForProcessResponse data = new BusinessIndividualGetDataForProcessResponse();
			DynamicParameters DP = new DynamicParameters();
			data.lstHashtag = (await _sQLCon.ExecuteListDapperAsync<DropdownObject>("CA_HashtagGetDropdown", DP)).ToList();
			data.lstGroupWord = (await _sQLCon.ExecuteListDapperAsync<DropdownObject>("CA_GroupWordGetListSuggest", DP)).ToList();
			DP.Add("UnitId", UnitId);
			data.lstUsers = (await _sQLCon.ExecuteListDapperAsync<DropdownObject>("SY_UsersGetDropdownByUnitId", DP)).ToList();
			return data;
		}
	}
}
