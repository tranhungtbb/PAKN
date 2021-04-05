using Dapper;
using PAKNAPI.Common;
using PAKNAPI.ModelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Models.ModelBase
{
    public class PURecommendation
    {

        private SQLCon _sQLCon;

        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Hour { get; set; }
        public string Date { get; set; }

        public int RowNumber { get; set; }

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


        public async Task<PURecommendation> PURecommendationGetById(int? id, int? status)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("Id", id);
            DP.Add("Status", status);
            return (await _sQLCon.ExecuteListDapperAsync<PURecommendation>("PU_RecommendationGetByID", DP)).ToList().FirstOrDefault();
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
