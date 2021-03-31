using Dapper;
using PAKNAPI.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using PAKNAPI.ModelBase;

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
			data.lstField = (await _sQLCon.ExecuteListDapperAsync<DropdownObject>("CA_FieldGetDropdown", DP)).ToList();
			data.lstUnit = (await _sQLCon.ExecuteListDapperAsync<DropdownObject>("SY_UnitGetDropdown", DP)).ToList();
			data.lstBusiness = (await _sQLCon.ExecuteListDapperAsync<DropdownObject>("BI_BusinessGetDropdown", DP)).ToList();
			data.lstIndividual = (await _sQLCon.ExecuteListDapperAsync<DropdownObject>("BI_IndividualGetDropdown", DP)).ToList();
			data.lstHashTag = (await _sQLCon.ExecuteListDapperAsync<DropdownObject>("CA_HashtagGetDropdown", DP)).ToList();
			data.Code = await _sQLCon.ExecuteScalarDapperAsync<string>("MR_Recommendation_GenCode_GetCode", DP);
			return data;
		}

		public async Task<RecommendationGetDataForForwardResponse> RecommendationGetDataForForward()
		{
			RecommendationGetDataForForwardResponse data = new RecommendationGetDataForForwardResponse();
			DynamicParameters DP = new DynamicParameters();
			data.lstUnitNotMain = (await _sQLCon.ExecuteListDapperAsync<DropdownObject>("SY_UnitGetDropdownNotMain", DP)).ToList();
			return data;
		}

		public async Task<RecommendationGetByIDResponse> RecommendationGetByID(int? Id)
		{
			RecommendationGetByIDResponse data = new RecommendationGetByIDResponse();
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);
			data.Model = (await _sQLCon.ExecuteListDapperAsync<MRRecommendationGetByID>("MR_RecommendationGetByID", DP)).FirstOrDefault();
			data.lstHashtag = (await _sQLCon.ExecuteListDapperAsync<MRRecommendationHashtagGetByRecommendationId>("MR_Recommendation_HashtagGetByRecommendationId", DP)).ToList();
			data.lstFiles = (await _sQLCon.ExecuteListDapperAsync<MRRecommendationFilesGetByRecommendationId>("MR_Recommendation_FilesGetByRecommendationId", DP)).ToList();
			Base64EncryptDecryptFile decrypt = new Base64EncryptDecryptFile();
			foreach (var item in data.lstFiles)
			{
				item.FilePath = decrypt.EncryptData(item.FilePath);
			}
			return data;
		}

		public async Task<int> RecommendationForwardProcess(RecommendationForwardProcess _recommendationForwardProcess)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _recommendationForwardProcess.Id);
			DP.Add("RecommendationId", _recommendationForwardProcess.RecommendationId);
			DP.Add("Status", _recommendationForwardProcess.Status);
			DP.Add("ReasonDeny", _recommendationForwardProcess.ReasonDeny);
			DP.Add("ProcessingDate", _recommendationForwardProcess.ProcessingDate);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_Recommendation_ForwardProcess", DP));
		}
	}
}