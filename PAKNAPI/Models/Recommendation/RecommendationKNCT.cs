using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Models.Recommendation
{
    public class RecommendationKNCT
    {
        public string coQuanChuTri { get; set; }
        public string coQuanKiemDuyet { get; set; }
        public string coQuanPhoiHop { get; set; }
        public int id { get; set; }
        public string ketQua { get; set; }
        public string linhVuc { get; set; }
        public string loai { get; set; }
        public string maKienNghi { get; set; }
        public DateTime? ngayHetHanXuLy { get; set; }
        public DateTime? ngayKetThuc { get; set; }
        public DateTime? ngayTao { get; set; }
        public DateTime? ngayTiepNhan { get; set; }
        public DateTime? ngayXuLy { get; set; }
        public string nguon { get; set; }
        public string nhiemKy { get; set; }
        public string noiCoKienNghi { get; set; }
        public string noiDungKienNghi { get; set; }
        public string noiDungTraLoi { get; set; }
        public string phanLoai { get; set; }
        public string phuongXa { get; set; }
        public int soNgayGiaiQuyet { get; set; }
        public string tenCaNhanDonVi { get; set; }
        public List<FileAttach> tepDinhKem { get; set; }
        public string thoiGian { get; set; }
        public string tienDoGiaiQuyet { get; set; }
        public string tinhTrangXuLy { get; set; }
        public int total { get; set; }
        public int trangThai { get; set; }
        public string trangThaiGiaiQuyet { get; set; }
    }
    public class FileAttach
    {
        public string duongDan { get; set; }
        public string name { get; set; }
    }
    public class gridRecommendation
    {
        public List<RecommendationKNCT> gridData { get; set; }
    }
    public class numRecord
    {
        public int soLuongKNChoXuLy { get; set; }
    }
}
