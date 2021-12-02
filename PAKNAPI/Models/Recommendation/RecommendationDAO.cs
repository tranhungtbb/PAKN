using Dapper;
using PAKNAPI.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using PAKNAPI.ModelBase;
using PAKNAPI.Models.Statistic;
using Newtonsoft.Json;

namespace PAKNAPI.Models.Recommendation
{

	public class RecommendationDAO
    {
		private SQLCon _sQLCon;
		public RecommendationDAO(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public async Task<RecommendationGetDataForCreateResponse> RecommendationGetDataForCreate(int? unitId)
		{
			RecommendationGetDataForCreateResponse data = new RecommendationGetDataForCreateResponse();
			DynamicParameters DP = new DynamicParameters();
			data.lstField = (await _sQLCon.ExecuteListDapperAsync<DropdownObject>("CA_FieldGetDropdown", DP)).ToList();
			data.lstUnit = (await _sQLCon.ExecuteListDapperAsync<DropdownTree>("SY_UnitGetDropdownLevel", DP)).ToList();
			data.lstBusiness = (await _sQLCon.ExecuteListDapperAsync<DropdownObject>("BI_BusinessGetDropdown", DP)).ToList();
			data.lstIndividual = (await _sQLCon.ExecuteListDapperAsync<DropdownObject>("BI_IndividualGetDropdown", DP)).ToList();
			data.lstHashTag = (await _sQLCon.ExecuteListDapperAsync<DropdownObject>("CA_HashtagGetDropdown", DP)).ToList();
			data.lstGroupWord = (await _sQLCon.ExecuteListDapperAsync<DropdownObject>("[CA_GroupWordGetListSuggest]", DP)).ToList();
			data.Code = await _sQLCon.ExecuteScalarDapperAsync<string>("MR_Recommendation_GenCode_GetCode", DP);
			DP.Add("Id", unitId);
			data.lstUnitChild = (await _sQLCon.ExecuteListDapperAsync<DropdownObject>("[SY_UnitGetChildDropdown]", DP)).ToList();

			DP = new DynamicParameters();
			DP.Add("Type", TYPECONFIG.GENERAL);
			var config = (await _sQLCon.ExecuteListDapperAsync<SYConfig>("SY_ConfigGetByType", DP)).FirstOrDefault();
			if (config != null)
			{
				data.generalSetting = JsonConvert.DeserializeObject<GeneralSetting>(config.Content);
			}
			return data;
		}


		public async Task<RecommendationGetDataForCreateResponse> RecommendationGetDataForSearch()
		{
			RecommendationGetDataForCreateResponse data = new RecommendationGetDataForCreateResponse();
			DynamicParameters DP = new DynamicParameters();
			data.lstField = (await _sQLCon.ExecuteListDapperAsync<DropdownObject>("CA_FieldGetDropdown", DP)).ToList();
			data.lstUnit = (await _sQLCon.ExecuteListDapperAsync<DropdownTree>("SY_UnitGetDropdownLevel", DP)).ToList();
			return data;
		}

		public async Task<RecommendationGetDataForForwardResponse> RecommendationGetDataForForward(int? unitId)
		{
			RecommendationGetDataForForwardResponse data = new RecommendationGetDataForForwardResponse();
			DynamicParameters DP = new DynamicParameters();
			data.lstUnitNotMain = (await _sQLCon.ExecuteListDapperAsync<DropdownObject>("SY_UnitGetDropdownNotMain", DP)).ToList();
			DP.Add("UnitId", unitId);
			data.lstUnitForward = (await _sQLCon.ExecuteListDapperAsync<DropdownObject>("[SY_UnitGetDropdownForward]", DP)).ToList();
			return data;
		}

		public async Task<RecommendationGetDataForProcessResponse> RecommendationGetDataForProcess(int? UnitId, long userId)
		{
			RecommendationGetDataForProcessResponse data = new RecommendationGetDataForProcessResponse();
			DynamicParameters DP = new DynamicParameters();
			data.lstHashtag = (await _sQLCon.ExecuteListDapperAsync<DropdownObject>("CA_HashtagGetDropdown", DP)).ToList();
			data.lstGroupWord = (await _sQLCon.ExecuteListDapperAsync<DropdownObject>("CA_GroupWordGetListSuggest", DP)).ToList();
			DP.Add("UnitId", UnitId);
			data.lstUsers = (await _sQLCon.ExecuteListDapperAsync<DropdownObject>("SY_UsersGetDropdownByUnitId", DP)).ToList();
			data.lstUsersProcess = (await _sQLCon.ExecuteListDapperAsync<DropdownObject>("[SY_UsersProcessDropdownByUnitId]", DP)).Where(x=>x.Value != userId).ToList();
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
				item.FilePathUrl = item.FilePath;
				item.FilePath = decrypt.EncryptData(item.FilePath);
			}
			return data;
		}

		public async Task<RecommendationGetByIDViewResponse> RecommendationGetByIDView(int? Id,long userProcessId ,long unitProcessId)
		{
			RecommendationGetByIDViewResponse data = new RecommendationGetByIDViewResponse();
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);
			DP.Add("UserprocessId", userProcessId);
			DP.Add("UnitProcessId", unitProcessId);
			data.Model = (await _sQLCon.ExecuteListDapperAsync<MRRecommendationGetByIDView>("[MR_RecommendationGetByIDView]", DP)).FirstOrDefault();

			DP = new DynamicParameters();
			DP.Add("Id", Id);
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
				data.denyContent =(await _sQLCon.ExecuteListDapperAsync<MRRecommendationGetDenyContentsBase>("[MR_Recommendation_GetDenyContents]", DPdeny)).OrderByDescending(x=>x.Status).Take(1).ToList();
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

		public async Task<RecommendationGetByIDViewResponse> RecommendationGetByIDView(int? Id)
		{
			RecommendationGetByIDViewResponse data = new RecommendationGetByIDViewResponse();
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);
			data.Model = (await _sQLCon.ExecuteListDapperAsync<MRRecommendationGetByIDView>("[MR_RecommendationGetByIDViewPublic]", DP)).FirstOrDefault();

			DP = new DynamicParameters();
			DP.Add("Id", Id);
			data.lstHashtag = (await _sQLCon.ExecuteListDapperAsync<MRRecommendationHashtagGetByRecommendationId>("MR_Recommendation_HashtagGetByRecommendationId", DP)).ToList();
			data.lstFiles = (await _sQLCon.ExecuteListDapperAsync<MRRecommendationFilesGetByRecommendationId>("MR_Recommendation_FilesGetByRecommendationId", DP)).ToList();
			Base64EncryptDecryptFile decrypt = new Base64EncryptDecryptFile();
			foreach (var item in data.lstFiles)
			{
				item.FilePath = decrypt.EncryptData(item.FilePath);
			}

			if (data.Model.Status == STATUS_RECOMMENDATION.APPROVE_DENY || data.Model.Status == STATUS_RECOMMENDATION.PROCESS_DENY || data.Model.Status == STATUS_RECOMMENDATION.RECEIVE_DENY)
			{
				DynamicParameters DPdeny = new DynamicParameters();
				DPdeny.Add("RecommendationId", Id);
				data.denyContent = (await _sQLCon.ExecuteListDapperAsync<MRRecommendationGetDenyContentsBase>("[MR_Recommendation_GetDenyContents]", DPdeny)).OrderByDescending(x => x.Status).Take(1).ToList();
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

		public async Task<int?> SyncKhanhHoaInsert(CongThongTinTinh item)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Questioner", item.Questioner);
			DP.Add("Question", item.Question);
			DP.Add("QuestionContent", item.QuestionContent);
			DP.Add("Reply", item.Reply);
			DP.Add("CreatedDate", item.CreatedDate);
			DP.Add("ReplyDate", item.ReplyDate);
			DP.Add("ObjectId", item.ObjectId);
			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_RecommendationSync_Insert", DP));
		}

		public async Task<int?> SyncKhanhHoaDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();
			return await _sQLCon.ExecuteNonQueryDapperAsync("MR_RecommendationSync_DeleteAll", DP);
		}

		public void SyncHopThuGopYKhanhHoa(List<GopYKienNghi> lstData)
		{
			DynamicParameters DP = new DynamicParameters();
			_sQLCon.ExecuteNonQueryDapper("MR_Sync_HanhChinhCongKhanhHoaDeleteAll", DP);
			foreach (var item in lstData)
			{
				DP = new DynamicParameters();
				DP.Add("Questioner", item.Questioner);
				DP.Add("Question", item.Question);
				DP.Add("QuestionContent", item.QuestionContent);
				DP.Add("Reply", item.Reply);
				DP.Add("CreatedDate", item.CreatedDate);
				_sQLCon.ExecuteNonQueryDapper("MR_Sync_HanhChinhCongKhanhHoaInsert", DP);
			}
		}
		public async Task<int> SyncDichVuCongQuocGiaInsert(DichViCongQuocGia mr_DichVuCong)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Questioner", mr_DichVuCong.Questioner);
			DP.Add("Question", mr_DichVuCong.Question);
			DP.Add("QuestionContent", mr_DichVuCong.QuestionContent);
			DP.Add("Reply", mr_DichVuCong.Reply);
			DP.Add("CreatedDate", mr_DichVuCong.CreatedDate);
			DP.Add("Status", mr_DichVuCong.Status);
			DP.Add("ObjectId", mr_DichVuCong.ObjectId);
			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_Sync_CongDichVuCongQuocGia_Insert", DP));
		}

		public async Task<List<DichViCongQuocGia>> SyncDichVuCongQuocGiaGetByObjecId(int ObjectId)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("ObjectId", ObjectId);
			return (await _sQLCon.ExecuteListDapperAsync<DichViCongQuocGia>("MR_Sync_CongDichVuCongQuocGia_GetByObjectId", DP)).ToList();
		}

		public async Task<int> SyncDichVuCongQuocGiaGetTotalCount()
		{
			DynamicParameters DP = new DynamicParameters();
			return (await _sQLCon.ExecuteListDapperAsync<int>("MR_Sync_CongDichVuCongQuocGia_TotalCount", DP)).FirstOrDefault();
		}

		public async Task<int> SyncDichVuCongQuocGiaDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();
			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_Sync_CongDichVuCongQuocGia_DeleteAll", DP));
		}
	}
}