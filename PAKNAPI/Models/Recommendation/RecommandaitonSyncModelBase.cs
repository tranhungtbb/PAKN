using Dapper;
using PAKNAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Models.Recommendation
{
    public class RecommandaitonSyncModelBase
    {
    }
    public class GopYKienNghiPagedListModel : GopYKienNghi
    {
        public int? RowNumber { get; set; }
    }

    public class PANKChinhPhuModel
    {
        public int? ObjectId { get; set; }
        public string Questioner { get; set; }
        public string Question { get; set; }
        public string CreatedDate { get; set; }
        public string Status { get; set; }

        public string QuestionContent { get; set; }

    }
    public class PANKChinhPhuPagedListModel : PANKChinhPhuModel
    {
        public int? RowNumber { get; set; }
    }


    public class CuTriTinhKhanhHoaModel
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Fields { get; set; }
        public string Status { get; set; }
        public string Result { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
    }
    public class CuTriTinhKhanhHoaPagedListModel : CuTriTinhKhanhHoaModel
    {
        public int? RowNumber { get; set; }
    }

    public class PURecommandationSyncModel
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string Fields { get; set; }
        public string Reply { get; set; }
        public string Status { get; set; }
    }
    public class PURecommandationSyncPagedListModel : PURecommandationSyncModel
    {
        public int? RowNumber { get; set; }
    }

    public class RecommandationSyncDAO
    {
        private SQLCon _sQLCon;
        public RecommandationSyncDAO(IAppSetting appSetting)
        {
            _sQLCon = new SQLCon(appSetting.GetConnectstring());
        }
        public RecommandationSyncDAO()
        {
        }

        /// <summary>
        /// Danh sách góp ý kiến nghị
        /// </summary>
        /// <param name="questioner"></param>
        /// <param name="question"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<IEnumerable<GopYKienNghiPagedListModel>> CongThongTinDienTuTinhGetPagedList(
            string questioner,
            string question,
            int pageIndex = 1,
            int pageSize = 20)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("Questioner", questioner);
            DP.Add("Question", question);
            DP.Add("PageIndex", pageIndex);
            DP.Add("PageSize", pageSize);

            var rs = await _sQLCon.ExecuteListDapperAsync<GopYKienNghiPagedListModel>("[MR_Sync_CongThongTinDienTuTinh_GetPagedList]", DP);
            return rs;
        }

        public async Task<List<GopYKienNghi>> CongThongTinDienTuTinhGetByIdDAO(int Id)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("Id", Id);

            return (await _sQLCon.ExecuteListDapperAsync<GopYKienNghi>("[MR_Sync_CongThongTinDienTuTinh_GetById]", DP)).ToList();

        }

        /// <summary>
        /// hòm thư góp ý  https://thongtin.hanhchinhcong.khanhhoa.gov.vn/module/hop-thu-gop-y
        /// </summary>
        /// <param name="questioner"></param>
        /// <param name="question"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<List<GopYKienNghiPagedListModel>> CongThongTinDichVuHCCGetPagedList(
            string questioner,
            string question,
            int pageIndex = 1,
            int pageSize = 20)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("questioner", questioner);
            DP.Add("Question", question);
            DP.Add("pageindex", pageIndex);
            DP.Add("pageSize", pageSize);

            var rs = (await _sQLCon.ExecuteListDapperAsync<GopYKienNghiPagedListModel>("[MR_Sync_HanhChinhCongKhanhHoa_GetPagedList]", DP)).ToList();
            return rs;
        }

        public async Task<List<GopYKienNghi>> CongThongTinDichVuHCCGetById(int Id)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("Id", Id);

            return (await _sQLCon.ExecuteListDapperAsync<GopYKienNghi>("[MR_Sync_HanhChinhCongKhanhHoa_GetById]", DP)).ToList();
            
        }


        /// <summary>
        ///  https://dichvucong.gov.vn/p/phananhkiennghi/pakn-search.html
        /// </summary>
        /// <param name="questioner"></param>
        /// <param name="question"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<List<PANKChinhPhuPagedListModel>> HeThongPANKChinhPhuGetPagedList(
            string Questioner,
            string Question,
            int PageIndex = 1,
            int PageSize = 20)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("Questioner", Questioner);
            DP.Add("Question", Question);
            DP.Add("PageIndex", PageIndex);
            DP.Add("PageSize", PageSize);

            return (await _sQLCon.ExecuteListDapperAsync<PANKChinhPhuPagedListModel>("[MR_Sync_CongDichVuCongQuocGia_GetPagedList]", DP)).ToList();
            
        }

       



        public async Task<IEnumerable<PURecommandationSyncPagedListModel>> PURecommandationSyncGetPagedList(
            string keyword,
            int src = 1,
            int pageIndex = 1,
            int pageSize = 20)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("keyword", keyword);
            DP.Add("src", src);
            DP.Add("pageindex", pageIndex);
            DP.Add("pageSize", pageSize);

            var rs = await _sQLCon.ExecuteListDapperAsync<PURecommandationSyncPagedListModel>("[PU_Recomendation_Sync_GetAllPagedList]", DP);
            return rs;
        }
        
        public async Task<PURecommandationSyncModel> PURecommandationSyncGetDetail(
            long id,
            int src = 1)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("id", id);
            DP.Add("src", src);

            var rs = await _sQLCon.ExecuteListDapperAsync<PURecommandationSyncModel>("[PU_Recomendation_Sync_GetDetail]", DP);
            return rs.FirstOrDefault();
        }
    }

    /// <summary>
    /// https://kiennghicutri.khanhhoa.gov.vn/request-search-publish
    /// </summary>
    /// 

    public class MR_CuTriTinhKhanhHoa
    {
        private SQLCon _sQLCon;

        public MR_CuTriTinhKhanhHoa(IAppSetting appSetting)
        {
            _sQLCon = new SQLCon(appSetting.GetConnectstring());
        }



        public MR_CuTriTinhKhanhHoa()
        {
        }
        public long Id { get; set; }
        public long ElectorId { get; set; }
        public string Content { get; set; }
        public string Result { get; set; }
        public int Status { get; set; }
        public string CategoryName { get; set; }
        public int FieldId { get; set; }
        public string FieldName { get; set; }
        public string RecommendationPlace { get; set; }
        public string Term { get; set; }
        public string UserProcess { get; set; }
        public string UnitPreside { get; set; }
        public string UnitCombination { get; set; }
        public string Progress { get; set; }
        public string Response { get; set; }
        public DateTime? EndDate { get; set; }

        public int? TotalCount { get; set; }



        public async Task<int?> MR_Sync_CuTriTinhKhanhHoaInsertDAO(MR_CuTriTinhKhanhHoa mr_CuTriTinhKhanhHoa)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("@ElectorId", mr_CuTriTinhKhanhHoa.ElectorId);
            DP.Add("@Content", mr_CuTriTinhKhanhHoa.Content);
            DP.Add("@Result", mr_CuTriTinhKhanhHoa.Result);
            DP.Add("@Status", mr_CuTriTinhKhanhHoa.Status);
            DP.Add("@CategoryName", mr_CuTriTinhKhanhHoa.CategoryName);
            DP.Add("@FieldId", mr_CuTriTinhKhanhHoa.FieldId);
            DP.Add("@RecommendationPlace", mr_CuTriTinhKhanhHoa.RecommendationPlace);
            DP.Add("@Term", mr_CuTriTinhKhanhHoa.Term);
            DP.Add("@UserProcess", mr_CuTriTinhKhanhHoa.UserProcess);
            DP.Add("@UnitPreside", mr_CuTriTinhKhanhHoa.UnitPreside);
            DP.Add("@UnitCombination", mr_CuTriTinhKhanhHoa.UnitCombination);
            DP.Add("@Progress", mr_CuTriTinhKhanhHoa.Progress);
            DP.Add("@Response", mr_CuTriTinhKhanhHoa.Response);
            DP.Add("@EndDate", mr_CuTriTinhKhanhHoa.EndDate);

            return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_Sync_CuTriTinhKhanhHoaInsert", DP));
        }


        public async Task<List<MR_CuTriTinhKhanhHoa>> MR_Sync_CuTriTinhKhanhHoaGetByIdDAO(int id)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("Id", id);
            return (await _sQLCon.ExecuteListDapperAsync<MR_CuTriTinhKhanhHoa>("MR_Sync_CuTriTinhKhanhHoaGetByKNCTId", DP)).ToList();
        }

        public async Task<MR_CuTriTinhKhanhHoa> MR_Sync_CuTriTinhKhanhHoaGetTopOrderbyElectorId()
        {
            DynamicParameters DP = new DynamicParameters();
            return (await _sQLCon.ExecuteListDapperAsync<MR_CuTriTinhKhanhHoa>("[MR_Sync_CuTriTinhKhanhHoa_GetTopOrderbyElectorId]", DP)).FirstOrDefault();
        }

        // delete

        public async Task<int> MR_Sync_CuTriTinhKhanhHoaDeleteAllDAO()
        {
            DynamicParameters DP = new DynamicParameters();

            return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_Sync_CuTriTinhKhanhHoaDeleteAll", DP));
        }
    }

    public class MR_CuTriTinhKhanhHoaGetPage
    {
        private SQLCon _sQLCon;

        public MR_CuTriTinhKhanhHoaGetPage(IAppSetting appSetting)
        {
            _sQLCon = new SQLCon(appSetting.GetConnectstring());
        }



        public MR_CuTriTinhKhanhHoaGetPage()
        {
        }
        public long? RowNumber { get; set; }
        public long Id { get; set; }
        public long ElectorId { get; set; }
        public string Content { get; set; }
        public string UnitPreside { get; set; }
        public int Status { get; set; }
        public string RecommendationPlace { get; set; }
        public string FieldName { get; set; }

        public string Term { get; set; }

        public async Task<List<MR_CuTriTinhKhanhHoaGetPage>> MR_Sync_CuTriTinhKhanhHoaGetListDAO(string Content, string UnitPreside, string Place, int? Field, int? Status, int? PageSize, int? PageIndex)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("Content", Content);
            DP.Add("UnitPreside", UnitPreside);
            DP.Add("RecommendationPlace", Place);
            DP.Add("Field", Field);
            DP.Add("Status", Status);
            DP.Add("PageSize", PageSize);
            DP.Add("PageIndex", PageIndex);

            return (await _sQLCon.ExecuteListDapperAsync<MR_CuTriTinhKhanhHoaGetPage>("MR_Sync_CuTriTinhKhanhHoa_GetPagedList", DP)).ToList();
        }
    }

    


    public class MR_SyncFileAttach
    {
        private SQLCon _sQLCon;

        public MR_SyncFileAttach(IAppSetting appSetting)
        {
            _sQLCon = new SQLCon(appSetting.GetConnectstring());
        }
        public MR_SyncFileAttach()
        {
        }

        public int Id { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public int Type { get; set; }
        public int ObjectId { get; set; }

        public async Task<int?> MR_Sync_CuTriTinhKhanhHoaFileAttachInsertDAO(MR_SyncFileAttach mr_CuTriTinhKhanhHoaFile)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("@FilePath", mr_CuTriTinhKhanhHoaFile.FilePath);
            DP.Add("@FileName", mr_CuTriTinhKhanhHoaFile.FileName);
            DP.Add("@Type", mr_CuTriTinhKhanhHoaFile.Type);
            DP.Add("@ObjectId", mr_CuTriTinhKhanhHoaFile.ObjectId);
            return (await _sQLCon.ExecuteNonQueryDapperAsync("[MR_Sync_CuTriTinhKhanhHoa_FileAttachInsert]", DP));
        }

        public async Task<List<MR_SyncFileAttach>> MR_Sync_CuTriTinhKhanhHoaFileAttachGetByKNCTIdDAO(long ElectorId)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("@ObjectId", ElectorId);
            return (await _sQLCon.ExecuteListDapperAsync<MR_SyncFileAttach>("[MR_Sync_CuTriTinhKhanhHoa_FileAttachGetListByKNCTId]", DP)).ToList();
        }


        public async Task<int?> MR_Sync_DichVuCongQuocGiaFileAttachInsertDAO(MR_SyncFileAttach mr_CuTriTinhKhanhHoaFile)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("@FilePath", mr_CuTriTinhKhanhHoaFile.FilePath);
            DP.Add("@FileName", mr_CuTriTinhKhanhHoaFile.FileName);
            DP.Add("@Type", mr_CuTriTinhKhanhHoaFile.Type);
            DP.Add("@ObjectId", mr_CuTriTinhKhanhHoaFile.ObjectId);
            return (await _sQLCon.ExecuteNonQueryDapperAsync("[MR_Sync_CongDichVuCongQuocGiaFileAttachInsert]", DP));
        }

        public async Task<List<MR_SyncFileAttach>> MR_Sync_DichVuCongQuocGiaFileAttachGetByDVCIdDAO(long ObjectId)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("@ObjectId", ObjectId);
            return (await _sQLCon.ExecuteListDapperAsync<MR_SyncFileAttach>("[MR_Sync_CongDichVuCongQuocGia_FileAttachGetListByObjectId]", DP)).ToList();
        }

    }

}
