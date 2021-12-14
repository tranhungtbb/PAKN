using Dapper;
using Microsoft.AspNetCore.Mvc;
using PAKNAPI.Common;
using PAKNAPI.ModelBase;
using PAKNAPI.Models.Recommendation;
using PAKNAPI.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Models.SyncData
{
    public class FeedBackSync
    {
        private readonly IAppSetting _appSetting;

        public FeedBackSync(IAppSetting appSetting)
        {
            _appSetting = appSetting;
        }

        public async Task<ActionResult<object>> SyncFeedBack()
        {
            try
            {
                var listFeedBack = await new FeedBackDAO(_appSetting).FeedBackGetAll();
                //if (listFeedBack.Count > 0) {
                //    //await new RecommendationDAO(_appSetting).SyncKhanhHoaDeleteAll();
                //    //await new MRRecommendationDelete(_appSetting).MRRecommendationDeleteByIsCloneDAO();
                //}
                // insert vào db

                CongThongTinTinh modelInsert = new CongThongTinTinh();
                foreach (var item in listFeedBack)
                {
                    modelInsert = new CongThongTinTinh(item);
                    await new RecommendationDAO(_appSetting).SyncKhanhHoaInsert(modelInsert);
                }

                return new ResultApi { Success = ResultCode.OK};
            }
            catch (Exception ex) {
                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }
    }


    public class FeedBackDAO
    {
        private SQLCon _sQLCon;
        public FeedBackDAO(IAppSetting appSetting)
        {
            _sQLCon = new SQLCon(appSetting.GetConnectstringFeedBack());
        }
        public async Task<List<FeedBackModel>> FeedBackGetAll() {
            string stringQuery = @"select * from FeedBack where IsPublish = 1 and IsDelete = 0";
            DynamicParameters DP = new DynamicParameters();
            return (await _sQLCon.ExecuteListDapperTextAsync<FeedBackModel>(stringQuery, DP)).ToList();
        }
    }

    public class FeedBackModel{
        public int Id { get; set; }
        public Guid FeedBackId { get; set; }
        public string NameSender { get; set; }
        public string EmailSender { get; set; }
        public string FeedBackTitle { get; set; }
        public string FeedBackAdress { get; set; }
        public string FeedBackPhone { get; set; }
        public string FeedBackContent { get; set; }
        public string FeedBackContentReply { get; set; }
        public DateTime? FeedBackDate { get; set; }
        public bool IsPublish { get; set; }
        public string FeedBackLink { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ChangedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? ChangedBy { get; set; }
        public string Image { get; set; }
        public string Linkfile { get; set; }
        public string UserPublish { get; set; }
        public string LinkFileTraLoi { get; set; }
        public int? Type { get; set; }
    }
}
