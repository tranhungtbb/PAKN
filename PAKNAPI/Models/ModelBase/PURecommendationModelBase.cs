﻿using Dapper;
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
        public int QuantityLike { get; set; }
        public int QuantityDislike { get; set; }

        public int CountClick { get; set; }

        public int RowNumber { get; set; }

        public string ProcessUnitName { get; set; }

        public string UnitName { get; set; }

        public string FieldName { get; set; }

        public PURecommendation(IAppSetting appSetting)
        {
            _sQLCon = new SQLCon(appSetting.GetConnectstring());
        }

        public PURecommendation()
        {
        }

        public async Task<List<PURecommendation>> PURecommendationAllOnPage(string? KeySearch, int Status, int PageSize, int PageIndex)
        {
            DynamicParameters DP = new DynamicParameters();

            DP.Add("KeySearch", KeySearch);
            DP.Add("Status", Status);
            DP.Add("PageSize", PageSize);
            DP.Add("PageIndex", PageIndex);

            return (await _sQLCon.ExecuteListDapperAsync<PURecommendation>("PU_RecommendationGetAllOnPage", DP)).ToList();
        }

        public async Task<List<MyRecommendation>> MyRecommendationAllOnPage(int?CreateBy, string LtsStatus, string Title,int PageSize, int PageIndex)
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

        public async Task<int?> PURecommendationCountClick(int? recommendationId)
        {
            DynamicParameters DP = new DynamicParameters();

            DP.Add("Id", recommendationId);

            return (await _sQLCon.ExecuteNonQueryDapperAsync("[PU_RecommendationUpdateCountClick]", DP));
        }



        public async Task<PURecommendation> PURecommendationGetById(int? id, int? status)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("Id", id);
            DP.Add("Status", status);
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
        public MRRecommendationConclusionGetByRecommendationId lstConclusion {get; set;}
        //ConclusionFile - file đính kèm giải quyết
        public List<MRRecommendationConclusionFilesGetByConclusionId> lstConclusionFiles { get; set; }
        
    }



}
