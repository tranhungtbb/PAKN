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
			data.lstUnit = (await _sQLCon.ExecuteListDapperAsync<DropdownTree>("SY_UnitGetDropdownLevel", DP)).ToList();
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

		public async Task<RecommendationGetDataForProcessResponse> RecommendationGetDataForProcess(int? UnitId)
		{
			RecommendationGetDataForProcessResponse data = new RecommendationGetDataForProcessResponse();
			DynamicParameters DP = new DynamicParameters();
			data.lstHashtag = (await _sQLCon.ExecuteListDapperAsync<DropdownObject>("CA_HashtagGetDropdown", DP)).ToList();
			data.lstGroupWord = (await _sQLCon.ExecuteListDapperAsync<DropdownObject>("CA_GroupWordGetListSuggest", DP)).ToList();
			DP.Add("UnitId", UnitId);
			data.lstUsers = (await _sQLCon.ExecuteListDapperAsync<DropdownObject>("SY_UsersGetDropdownByUnitId", DP)).ToList();
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

		public async Task<RecommendationGetByIDViewResponse> RecommendationGetByIDView(int? Id)
		{
			RecommendationGetByIDViewResponse data = new RecommendationGetByIDViewResponse();
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);
			data.Model = (await _sQLCon.ExecuteListDapperAsync<MRRecommendationGetByIDView>("MR_RecommendationGetByIDView", DP)).FirstOrDefault();
				data.lstHashtag = (await _sQLCon.ExecuteListDapperAsync<MRRecommendationHashtagGetByRecommendationId>("MR_Recommendation_HashtagGetByRecommendationId", DP)).ToList();
			data.lstFiles = (await _sQLCon.ExecuteListDapperAsync<MRRecommendationFilesGetByRecommendationId>("MR_Recommendation_FilesGetByRecommendationId", DP)).ToList();
			Base64EncryptDecryptFile decrypt = new Base64EncryptDecryptFile();
			foreach (var item in data.lstFiles)
			{
				item.FilePath = decrypt.EncryptData(item.FilePath);
			}

			if (data.Model.Status == STATUS_RECOMMENDATION.APPROVE_DENY || data.Model.Status == STATUS_RECOMMENDATION.PROCESS_DENY || data.Model.Status == STATUS_RECOMMENDATION.RECEIVE_DENY)
			{
				DynamicParameters  DPdeny = new DynamicParameters();
				DPdeny.Add("RecommendationId", Id);
				data.denyContent =(await _sQLCon.ExecuteListDapperAsync<MRRecommendationGetDenyContentsBase>("[MR_Recommendation_GetDenyContents]", DPdeny)).Where(x=>x.Status == Id).ToList();
			}

			if (data.Model.Status > STATUS_RECOMMENDATION.PROCESSING)
			{
				data.ModelConclusion = (await _sQLCon.ExecuteListDapperAsync<MRRecommendationConclusionGetByRecommendationId>("MR_Recommendation_ConclusionGetByRecommendationId", DP)).FirstOrDefault();
				DP = new DynamicParameters();
				DP.Add("Id", data.ModelConclusion.Id);
				if (data.ModelConclusion != null)
				{
					data.filesConclusion = (await _sQLCon.ExecuteListDapperAsync<MRRecommendationConclusionFilesGetByConclusionId>("MR_Recommendation_Conclusion_FilesGetByConclusionId", DP)).ToList();
					foreach (var item in data.filesConclusion)
					{
						item.FilePath = decrypt.EncryptData(item.FilePath);
					}
				}
			}
			return data;
		}
		public void SyncKhanhHoa(List<GopYKienNghi> lstData)
		{
			DynamicParameters DP = new DynamicParameters();
			_sQLCon.ExecuteNonQueryDapper("MR_RecommendationSync_DeleteAll", DP);
            foreach (var item in lstData)
			{
				DP = new DynamicParameters();
				DP.Add("Questioner", item.Questioner);
				DP.Add("Question", item.Question);
				DP.Add("QuestionContent", item.QuestionContent);
				DP.Add("Reply", item.Reply);
				DP.Add("CreatedDate", item.CreatedDate);
				_sQLCon.ExecuteNonQueryDapper("MR_RecommendationSync_Insert", DP);
			}
		}
	}
}