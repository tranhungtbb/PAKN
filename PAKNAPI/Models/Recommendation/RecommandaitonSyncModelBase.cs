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

            var rs = await _sQLCon.ExecuteListDapperAsync<GopYKienNghiPagedListModel>("[MR_Sync_CongThongTinDienTuTinh_GetPagedList]", DP);
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
        public async Task<IEnumerable<GopYKienNghi>> HeThongPANKChinhPhuGetPagedList(
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
        /// https://kiennghicutri.khanhhoa.gov.vn/request-search-publish
        /// </summary>
        /// <param name="questioner"></param>
        /// <param name="question"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<IEnumerable<GopYKienNghi>> HeThongQuanLyKienNghiCuTriGetPagedList(
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

    }

}
