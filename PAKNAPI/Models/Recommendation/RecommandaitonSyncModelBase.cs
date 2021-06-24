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
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
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
            DP.Add("questioner", questioner);
            DP.Add("Question", question);
            DP.Add("pageindex", pageIndex);
            DP.Add("pageSize", pageSize);

            var rs = await _sQLCon.ExecuteListDapperAsync<GopYKienNghiPagedListModel>("[MR_Sync_CongThongTinDienTuTinh_GetPagedList]", DP);
            return rs;
        }
        /// <summary>
        /// hòm thư góp ý  https://thongtin.hanhchinhcong.khanhhoa.gov.vn/module/hop-thu-gop-y
        /// </summary>
        /// <param name="questioner"></param>
        /// <param name="question"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<IEnumerable<GopYKienNghi>> CongThongTinDichVuHCCGetPagedList(
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

            var rs = await _sQLCon.ExecuteListDapperAsync<GopYKienNghiPagedListModel>("[MR_Sync_HanhChinhCongKhanhHoa_GetPagedList]", DP);
            return rs;
        }

        /// <summary>
        ///  https://dichvucong.gov.vn/p/phananhkiennghi/pakn-search.html
        /// </summary>
        /// <param name="questioner"></param>
        /// <param name="question"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PANKChinhPhuPagedListModel>> HeThongPANKChinhPhuGetPagedList(
            string title,
            string status,
            string createdBy,
            int pageIndex = 1,
            int pageSize = 20)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("title", title);
            DP.Add("status", status);
            DP.Add("CreatedBy", createdBy);
            DP.Add("pageindex", pageIndex);
            DP.Add("pageSize", pageSize);

            var rs = await _sQLCon.ExecuteListDapperAsync<PANKChinhPhuPagedListModel>("[MR_Sync_CongDichVuCongQuocGia_GetPagedList]", DP);
            return rs;
        }

        /// <summary>
        /// https://kiennghicutri.khanhhoa.gov.vn/request-search-publish
        /// </summary>
        /// <param name="questioner"></param>
        /// <param name="question"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<IEnumerable<CuTriTinhKhanhHoaPagedListModel>> HeThongQuanLyKienNghiCuTriGetPagedList(
            string fields,
            string status,
            int pageIndex = 1,
            int pageSize = 20)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("fields", fields);
            DP.Add("status", status);
            DP.Add("pageindex", pageIndex);
            DP.Add("pageSize", pageSize);

            var rs = await _sQLCon.ExecuteListDapperAsync<CuTriTinhKhanhHoaPagedListModel>("[MR_Sync_CuTriTinhKhanhHoa_GetPagedList]", DP);
            return rs;
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

}
