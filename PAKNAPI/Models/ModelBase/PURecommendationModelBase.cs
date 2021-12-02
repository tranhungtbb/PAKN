using Dapper;
using PAKNAPI.Common;
using PAKNAPI.ModelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Models.ModelBase
{
    public class MyRecommendation
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public int RowNumber { get; set; }
        public int Status { get; set; }
        public bool? IsForwardProcess { get; set; }
    }
    public class PURecommendation
    {

        private SQLCon _sQLCon;

        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? QuantityLike { get; set; }
        public int? QuantityDislike { get; set; }
        public int? QuantityAccept { get; set; }
        public int? QuantityType { get; set; }

        public int Status { get; set; }
        public string ContentConclusion { get; set; }

        public int RowNumber { get; set; }

        public string ProcessUnitName { get; set; }

        public string UnitName { get; set; }

        public string FieldName { get; set; }
        public int TotalComment { get; set; }

        public PURecommendation(IAppSetting appSetting)
        {
            _sQLCon = new SQLCon(appSetting.GetConnectstring());
        }

        public PURecommendation()
        {
        }

        public async Task<List<PURecommendation>> PURecommendationAllOnPage(string KeySearch, int? FieldId, int? UnitId, int PageSize, int PageIndex)
        {
            DynamicParameters DP = new DynamicParameters();

            DP.Add("KeySearch", KeySearch);
            DP.Add("FieldId", FieldId);
            DP.Add("UnitId", UnitId);
            DP.Add("Status", STATUS_RECOMMENDATION.FINISED);
            DP.Add("PageSize", PageSize);
            DP.Add("PageIndex", PageIndex);

            return (await _sQLCon.ExecuteListDapperAsync<PURecommendation>("PU_RecommendationGetAllOnPage", DP)).ToList();
        }
        public async Task<List<PURecommendation>> PURecommendationReceiveDeny(string KeySearch, int? FieldId, int? UnitId, int PageSize, int PageIndex)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("KeySearch", KeySearch);
            DP.Add("FieldId", FieldId);
            DP.Add("UnitId", UnitId);
            DP.Add("PageSize", PageSize);
            DP.Add("PageIndex", PageIndex);

            return (await _sQLCon.ExecuteListDapperAsync<PURecommendation>("PU_Recommendation_ReceiveDeny", DP)).ToList();
        }


        public async Task<List<PURecommendation>> PURecommendationByField(int fieldId)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("FieldId", fieldId);
            return (await _sQLCon.ExecuteListDapperAsync<PURecommendation>("PU_RecommendationGetByField", DP)).ToList();
        }

        public async Task<List<MyRecommendation>> MyRecommendationAllOnPage(int? CreateBy, string LtsStatus, string Title, int PageSize, int PageIndex)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("CreateBy", CreateBy);
            DP.Add("ltsStatus", LtsStatus);
            DP.Add("Title", Title);
            DP.Add("PageSize", PageSize);
            DP.Add("PageIndex", PageIndex);

            return (await _sQLCon.ExecuteListDapperAsync<MyRecommendation>("My_RecommendationAllOnPage", DP)).ToList();
        }

        public async Task<List<PURecommendation>> PURecommendationGetListOrderByCountClick(int? Status)
        {
            DynamicParameters DP = new DynamicParameters();

            DP.Add("Status", Status);

            return (await _sQLCon.ExecuteListDapperAsync<PURecommendation>("PU_RecommendationGetListOrderByCountClick", DP)).ToList();
        }

        public async Task<List<PURecommendation>> PURecommendationProcessing()
        {
            DynamicParameters DP = new DynamicParameters();

            return (await _sQLCon.ExecuteListDapperAsync<PURecommendation>("PU_RecommendationProcessingGetList", DP)).ToList();
        }

        public async Task<PublishNotificationObject> PUNotificationGetDashboard()
        {
            DynamicParameters DP = new DynamicParameters();
            return (await _sQLCon.ExecuteListDapperAsync<PublishNotificationObject>("SY_PublishNotificationGetPublish", DP)).FirstOrDefault();
        }

        public async Task<int?> PURecommendationCountClick(int? recommendationId)
        {
            DynamicParameters DP = new DynamicParameters();

            DP.Add("Id", recommendationId);

            return (await _sQLCon.ExecuteNonQueryDapperAsync("[PU_RecommendationUpdateCountClick]", DP));
        }

        public async Task<int?> PURecommendationSatisfationInsert(long? recommendationId, long? userId , int satisfaction )
        {
            DynamicParameters DP = new DynamicParameters();

            DP.Add("RecommendationId", recommendationId);
            DP.Add("UserId", userId);
            DP.Add("Satisfaction", satisfaction);

            return (await _sQLCon.ExecuteNonQueryDapperAsync("[MR_Recommendation_SatisfactionInsert]", DP));
        }



        public async Task<PURecommendation> PURecommendationGetById(int? id, int? status, long? userId)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("Id", id);
            DP.Add("Status", status);
            DP.Add("UserId", userId);
            return (await _sQLCon.ExecuteListDapperAsync<PURecommendation>("PU_RecommendationGetByID", DP)).ToList().FirstOrDefault();
        }


        public async Task<int?> MR_RecommendationUpdateSatisfaction(int? RecommendationId, bool? Satisfaction)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("RecommendationId", RecommendationId);
            DP.Add("Satisfaction", Satisfaction);

            return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_RecommendationUpdateSatisfaction", DP));
        }
    }


    // detail Recommendation trang công bố
    public class PURecommendationGetByIdViewResponse {
        public PURecommendation Model { get; set; }
        // files
        public List<MRRecommendationFilesGetByRecommendationId> lstFiles { get; set; }
        //Conclusion
        public MRRecommendationConclusionGetByRecommendationId lstConclusion { get; set; }
        //ConclusionFile - file đính kèm giải quyết
        public List<MRRecommendationConclusionFilesGetByConclusionId> lstConclusionFiles { get; set; }

        

    }


    public class PURecommendationByField
    {


        private SQLCon _sQLCon;
        public PURecommendationByField(IAppSetting appSetting)
        {
            _sQLCon = new SQLCon(appSetting.GetConnectstring());
        }


        public async Task<List<PURecommendationByFieldModel>> RecommendationGetByField(int? fieldId)
        {
            List<PURecommendationByFieldModel> data = new List<PURecommendationByFieldModel>();
            DynamicParameters DP = new DynamicParameters();
            DP.Add("FieldId", fieldId);
            List<PURecommendation> recommendations = (await _sQLCon.ExecuteListDapperAsync<PURecommendation>("PU_RecommendationGetByField", DP)).ToList();

            recommendations.ForEach( item =>
            {
                var dataItem = new PURecommendationByFieldModel();
                dataItem.Recommendation = item;
                DP = new DynamicParameters();
                DP.Add("Id", item.Id);
                var file = (_sQLCon.ExecuteListDapperAsync<MRRecommendationFilesGetByRecommendationId>("MR_Recommendation_FilesGetByRecommendationId", DP)).Result
                .Where(x => x.FileType == 4).FirstOrDefault();
                if (file != null) {
                    dataItem.filePath = file.FilePath;
                }
                data.Add(dataItem);
            });
            return data;
        }

    }

    public class PURecommendationByFieldModel{
        public PURecommendation Recommendation { get; set; }
        public string filePath { get; set; }
    }

    public class RecommendationGroupByFieldResponse {
        public int FieldId { get; set; }
        public string FieldName { get; set; }
        public string FilePath { get; set; }
        public List<PURecommendationByFieldModel> ListRecommendation { get; set; }

        public RecommendationGroupByFieldResponse(int fieldId, string fieldName, string FilePath, List<PURecommendationByFieldModel> pURecommendation) {
            this.FieldId = fieldId;
            this.FieldName = fieldName;
            this.FilePath = FilePath;
            this.ListRecommendation = pURecommendation;
        }
    }


    public class UnitDissatisfactionRateOnPage
    {

        private SQLCon _sQLCon;

        public int RowNumber { get; set; }
        public int Id { get; set; }
        public string UnitName { get; set; }
        public int Percent { get; set; }

        public UnitDissatisfactionRateOnPage(IAppSetting appSetting)
        {
            _sQLCon = new SQLCon(appSetting.GetConnectstring());
        }

        public UnitDissatisfactionRateOnPage()
        {
        }

        public async Task<List<UnitDissatisfactionRateOnPage>> UnitDissatisfactionRateOnPageDAO(string KeySearch, int? PageSize, int PageIndex)
        {
            DynamicParameters DP = new DynamicParameters();

            DP.Add("KeySearch", KeySearch);
            DP.Add("PageSize", PageSize);
            DP.Add("PageIndex", PageIndex);

            return (await _sQLCon.ExecuteListDapperAsync<UnitDissatisfactionRateOnPage>("PU_UnitDissatisfactionRateOnPage", DP)).ToList();
        }
    }

    public class LateProcessingUnitOnPage
    {

        private SQLCon _sQLCon;

        public int RowNumber { get; set; }
        public int Id { get; set; }
        public string UnitName { get; set; }
        public int Quantity { get; set; }

        public LateProcessingUnitOnPage(IAppSetting appSetting)
        {
            _sQLCon = new SQLCon(appSetting.GetConnectstring());
        }

        public LateProcessingUnitOnPage()
        {
        }

        public async Task<List<LateProcessingUnitOnPage>> LateProcessingUnitOnPageDAO(string KeySearch, int? PageSize, int PageIndex)
        {
            DynamicParameters DP = new DynamicParameters();

            DP.Add("KeySearch", KeySearch);
            DP.Add("PageSize", PageSize);
            DP.Add("PageIndex", PageIndex);

            return (await _sQLCon.ExecuteListDapperAsync<LateProcessingUnitOnPage>("PU_LateProcessingUnitOnPage", DP)).ToList();
        }
    }
}
