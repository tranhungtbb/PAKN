using Microsoft.AspNetCore.Http;
using PAKNAPI.Common;
using PAKNAPI.ModelBase;
using PAKNAPI.Models.Statistic;
using PAKNAPI.Models.SyncData;
using System;
using System.Collections.Generic;

namespace PAKNAPI.Models.Recommendation
{

    public class RecommendationGetDataForCreateResponse
    {
        public string Code { get; set; }
        public List<DropdownTree> lstUnit { get; set; }
        public List<DropdownObject> lstField { get; set; }
        public List<DropdownObject> lstIndividual { get; set; }
        public List<DropdownObject> lstBusiness { get; set; }
        public List<DropdownObject> lstHashTag { get; set; }
        public List<DropdownObject> lstGroupWord { get; set; }
        public List<DropdownObject> lstUnitChild { get; set; }
        public GeneralSetting generalSetting { get; set; }
    }

    public class RecommendationGetDataForForwardResponse
    {
        public List<DropdownObject> lstUnitNotMain { get; set; }
        public List<DropdownObject> lstUnitForward { get; set; }
    }

    public class RecommendationGetDataForProcessResponse
    {
        public List<DropdownObject> lstHashtag { get; set; }
        public List<DropdownObject> lstUsers { get; set; }
        public List<DropdownObject> lstGroupWord { get; set; }
        public List<DropdownObject> lstUsersProcess { get; set; }
    }
    public class RecommendationInsertRequest
    {
        public long? UserId { get; set; }
        public int? UserType { get; set; }
        public string UserFullName { get; set; }
        public MRRecommendationInsertIN Data { get; set; }
        public List<DropdownObject> ListHashTag { get; set; }
        public List<MRRecommendationFiles> LstXoaFile { get; set; }
        public IFormFileCollection Files { get; set; }
    }
    public class RecommendationUpdateRequest
    {
        public long? UserId { get; set; }
        public int? UserType { get; set; }
        public string UserFullName { get; set; }
        public MRRecommendationUpdateIN Data { get; set; }
        public List<DropdownObject> ListHashTag { get; set; }
        public List<MRRecommendationFiles> LstXoaFile { get; set; }
        public IFormFileCollection Files { get; set; }
    }
    public class RecommendationGetByIDResponse
    {
        public MRRecommendationGetByID Model { get; set; }
        public List<MRRecommendationHashtagGetByRecommendationId> lstHashtag { get; set; }
        public List<MRRecommendationFilesGetByRecommendationId> lstFiles { get; set; }
    }
    public class RecommendationGetByIDViewResponse
    {
        public MRRecommendationGetByIDView Model { get; set; }
        public MRRecommendationConclusionGetByRecommendationId ModelConclusion { get; set; }
        public List<MRRecommendationHashtagGetByRecommendationId> lstHashtag { get; set; }
        public List<MRRecommendationFilesGetByRecommendationId> lstFiles { get; set; }
        public List<MRRecommendationConclusionFilesGetByConclusionId> filesConclusion { get; set; }

        public List<MRRecommendationGetDenyContentsBase> denyContent { get; set; }
    }

    public class RecommendationForwardRequest
    {
        public MRRecommendationForwardInsertIN _mRRecommendationForwardInsertIN { get; set; }
        public List<DropdownObject> ListHashTag { get; set; }
        public byte RecommendationStatus { get; set; }
        public bool IsList { get; set; }
    }
    public class RecommendationForwardProcess
    {
        public MRRecommendationForwardProcessIN _mRRecommendationForwardProcessIN { get; set; }
        public List<DropdownObject> ListHashTag { get; set; }
        public byte RecommendationStatus { get; set; }
        public bool? ReactionaryWord { get; set; }
        public string ListGroupWordSelected { get; set; }
        public bool IsList { get; set; }
        public bool? IsForwardProcess { get; set; }
        public bool? IsForwardUnitChild { get; set; }
        public bool? IsFakeImage { get; set; }
        public bool? IsForwardMain { get; set; }
    }
    public class RecommendationOnProcessConclusionProcess
    {
        public MRRecommendationConclusionInsertIN DataConclusion { get; set; }
        public IFormFileCollection Files { get; set; }
        public byte RecommendationStatus { get; set; }
        public List<DropdownObject> ListHashTag { get; set; }
    }

    public class RecommendationSendProcess
    {
        public int? id { get; set; }
        public byte? status { get; set; }
    }
    public class GopYKienNghi
    {
        public int Id { get; set; }
        public string Questioner { get; set; }
        public string Question { get; set; }
        public string QuestionContent { get; set; }
        public string Reply { get; set; }
        public string CreatedDate { get; set; }
        public string ReplyDate { get; set; }

        public GopYKienNghi() { }

    }

    public class CongThongTinTinh
    {
        public int Id { get; set; }
        public string Questioner { get; set; }
        public string Question { get; set; }
        public string QuestionContent { get; set; }
        public string Reply { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ReplyDate { get; set; }
        public long? ObjectId { get; set; }

        public CongThongTinTinh() { }

        public CongThongTinTinh(FeedBackModel model)
        {
            this.Questioner = model.NameSender;
            this.Question = model.FeedBackTitle;
            this.QuestionContent = model.FeedBackContent;
            this.Reply = model.FeedBackContentReply;
            this.CreatedDate = model.CreatedOn != null ? model.CreatedOn : model.ChangedOn;
            this.ReplyDate = model.FeedBackDate;
            this.ObjectId = model.Id;

        }
    }

    public class DichViCongQuocGia
    {
        public int Id { get; set; }
        public string Questioner { get; set; }
        public string Question { get; set; }
        public string QuestionContent { get; set; }
        public string Reply { get; set; }
        public string CreatedDate { get; set; }
        public string Status { get; set; }
        public int ObjectId { get; set; }
        public int? TotalCount { get; set; }

    }

    public class RequestKienNghiCuTri
    {
        private bool isSuperAdmin = false;
        private bool isTongHop = false;
        private string keyWord = "";
        private int tinhTrangGiaiQuyet = -1;
        private int trangThai = 3;
        private int pageIndex = 0;
        private int pageSize = 40;
        public int AccountId { get; set; }
        public int DeparmentId { get; set; }
        public string DiaPhuong { get; set; }
        public string DonVi { get; set; }
        public string IpAddress { get; set; }
        public bool IsSuperAdmin
        {
            get { return this.isSuperAdmin; }
            set { this.isSuperAdmin = value; }
        }
        public bool IsTongHop
        {
            get { return this.isTongHop; }
            set { this.isTongHop = value; }
        }
        public string KeyWord
        {
            get { return this.keyWord; }
            set { this.keyWord = value; }
        }
        public string LinhVucId { get; set; }
        public string Nguon { get; set; }
        public string NhiemKy { get; set; }
        public int PageIndex {
            get { return this.pageIndex; }
            set { this.pageIndex = value; }
        }
        public int PageSize
        {
            get { return this.pageSize; }
            set { this.pageSize = value; }
        }
        public string PhanLoai { get; set; }
        public string PhuongXaId { get; set; }
        public int TinhTrangGiaiQuyet
        {
            get { return this.tinhTrangGiaiQuyet; }
            set { this.tinhTrangGiaiQuyet = value; }
        }
        public int TrangThai
        {
            get { return this.trangThai; }
            set { this.trangThai = value; }
        }
        public string UnitId { get; set; }
        public string UserId { get; set; }
    }

    public class ResponseKienNghiCuTri
    {
        public List<KienNghiCuTriObject> dataGrid = new List<KienNghiCuTriObject>();
        public string message { get; set; }
        public int status { get; set; }

    }

    public class KienNghiCuTriObject
    {
        public int Id { get; set; }
        public string noiDungKienNghi { get; set; }
        public string ketQua { get; set; }
        public int trangThai { get; set; }
        public string phanLoai { get; set; }
        public string linhVuc { get; set; }
        public string noiCoKienNghi { get; set; }
        public string nhiemKy { get; set; }
        public string tenCaNhanDonVi { get; set; }
        public string coQuanChuTri { get; set; }
        public string coQuanPhoiHop { get; set; }
        public string tienDoGiaiQuyet { get; set; }
        public DateTime? ngayKetThuc { get; set; }
        public string noiDungTraLoi { get; set; }
        public int total { get; set; }

        public List<FileAttackForKienNghiCuTri> tepDinhKem = new List<FileAttackForKienNghiCuTri>();
    }
    public class FileAttackForKienNghiCuTri
    {
        public string duongDan { get; set; }
        public string name { get; set; }
    }

    public class ResponseFieldKienNghiCuTri {
        public List<ResponseDropdownKienNghiCuTri> lstLinhVuc = new List<ResponseDropdownKienNghiCuTri>();
    }

    public class ResponseDropdownKienNghiCuTri {
        public int value { get; set; }
        public string text { get; set; }
        public int code { get; set; }
    }

    public class RequestAdministrative {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public RequestAdministrative() {
            this.PageIndex = 0;
            this.PageSize = 20;
        }
    }
    public class ResponseListAdministrative
    {
        public List<thutuc> ThuTucs { get; set; }
    }
    public class thutuc
    {
        public int Id { get; set; }

    }
    public class DetailAdministrative
    {
        public int Id { get; set; }
        public string Ten { get; set; }
        public string Ma { get; set; }
        public string TenCoQuanBanHanh { get; set; }
        public string SoHoSo { get; set; }
        public string CachThucThucHien { get; set; }
        public double ThoiHanGiaiQuyet { get; set; }
        public string MoTaThoiHanGiaiQuyet { get; set; }
        public string DoiTuongThucHien { get; set; }
        public string DiaChiTiepNhanHoSo { get; set; }
        public string KetQuaThucHien { get; set; }
        public string CoQuanCoThamQuyenQuyetDinh { get; set; }
        public int CoQuanCoThamQuyenQuyetDinhId { get; set; }
        public string CoQuanDuocUyQuyen { get; set; }
        public string CoQuanGiaiQuyet { get; set; }
        public string LuuY { get; set; }
        public string ThongTinLienHe { get; set; }
        public string CoQuanPhoiHop { get; set; }
        public object CoPhi { get; set; }
        public int TinhTrangId { get; set; }
        public TinhTrang TinhTrang { get; set; }
        public object HinhThuc { get; set; }
        public int IdThayTheHoacBoSung { get; set; }
        public object NgayCoHieuLuc { get; set; }
        public object NgayHetHieuLuc { get; set; }
        public object PhamViApDung { get; set; }
        public object TrinhTuThucHien { get; set; }
        public object ThanhPhanHoSo { get; set; }
        public string SoBoHoSo { get; set; }
        public string YeuCauDieuKien { get; set; }
        public object HoSoLuu { get; set; }
        public string CanCuPhapLy { get; set; }
        public int MucDo { get; set; }
        public bool? ThanhToanTrucTuyen { get; set; }
        public bool? NopQuaBuuChinh { get; set; }
        public int LinhVucId { get; set; }
        public string MaQuanLy_QuocGia { get; set; }
        public LinhVuc LinhVuc { get; set; }
        public object LinhVucs { get; set; }
        public List<CacBuocThucHien> CacBuocThucHiens { get; set; }
        public List<object> GopYThuTucs { get; set; }
        public List<HoSo> HoSoes { get; set; }
        public List<PhiLePhi> PhiLePhis { get; set; }
        public List<ThuTucDonViTiepNhan> ThuTucDonViTiepNhans { get; set; }
        public List<DonViTiepNhan> DonViTiepNhans { get; set; }
        public List<DanhSachCoQuanTiepNhan> DanhSachCoQuanTiepNhan { get; set; }
        public List<object> ThuTucLienQuans { get; set; }
        public List<object> ThuTucLienThongs { get; set; }
        public List<object> ThuTucSuaDoiBoSungs { get; set; }
        public List<HinhThucNop> HinhThucNops { get; set; }
        public List<AllHinhThucNop> AllHinhThucNops { get; set; }
        public List<object> DanhSachTepThuTuc { get; set; }
        public bool? CheckSMS { get; set; }
        public object CheckNhanDienThuTucLienThong { get; set; }
        public bool? CheckKhongApDungMotCua { get; set; }
        public string TenDonViTiepNhan { get; set; }
        public string TenLinhVuc { get; set; }
        public string TenCapTiepNhan { get; set; }
        public object CheckThanhToanKhiNopHS { get; set; }
        public object MaCSDLQuocGia { get; set; }
        public object MaDVC_QuocGia { get; set; }
        public string Href { get; set; }
    }
    public class TinhTrang
    {
        public int Ma { get; set; }
        public string Ten { get; set; }
        public List<object> ThuTucs { get; set; }
        public string Href { get; set; }
        public Errors Errors { get; set; }
        public int Id { get; set; }
        public bool IsValid { get; set; }
    }

    public class LinhVucTTHCResponse
    {
        public string totalRowLinhVuc { get; set; }
        public List<LinhVuc> DanhSachLinhVuc { get; set; }
        public List<DanhSachCoQuanTiepNhan> DanhSachCoQuanTiepNhan { get; set; }
    }

    public class LinhVuc
    {
        public int? Id { get; set; }
        public string Ten { get; set; }
        public int ThuTu { get; set; }
        public int? LinhVucChaId { get; set; }
        public object MaDinhDanh { get; set; }
        public object DonViPhuTrachLinhVuc { get; set; }
        public List<object> NguoiPhuTrachLinhVucs { get; set; }
        public List<object> DonViPhuTrachLinhVucs { get; set; }
        public object MaQuanLy_QuocGia { get; set; }
        public object Href { get; set; }
    }

    public class CacBuocThucHien
    {
        public int Id { get; set; }
        public double Buoc { get; set; }
        public string TenBuoc { get; set; }
        public object CoQuanCaNhanThucHien { get; set; }
        public double? ThoiGian { get; set; }
        public string TrachNhiem { get; set; }
        public string KetQua { get; set; }
        public object ThuTuc { get; set; }
        public int ThuTucId { get; set; }
        public object TongThoiGianXuLyDuocGiao { get; set; }
        public string Href { get; set; }
    }

    public class TepMauDonToKhai
    {
        public int Id { get; set; }
        public object TenTep { get; set; }
        public object MoRong { get; set; }
        public object KichThuocTep { get; set; }
        public object LoaiTep { get; set; }
        public object DuongDan { get; set; }
        public object ThumbnailURL { get; set; }
        public object MauDonToKhaiId { get; set; }
        public List<object> MauDonToKhais { get; set; }
        public object Href { get; set; }
    }
    public class FileTepMauDonToKhai
    {
        public Errors Errors { get; set; }
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public string FileType { get; set; }
        public string Size { get; set; }
        public string Path { get; set; }
        public object WeekOfYear { get; set; }
        public object Year { get; set; }
        public int ObjectId { get; set; }
        public string ObjectType { get; set; }
        public int Loai { get; set; }
        public string ServerId { get; set; }
        public bool IsValid { get; set; }
    }

    public class DonViPhuTrachLinhVuc
    {
        public int Id { get; set; }
        public int LinhVucId { get; set; }
        public int DonViPhuTrachId { get; set; }
        public object LinhVuc { get; set; }
        public string Href { get; set; }
    }

    public class MauDonToKhai
    {
        public int Id { get; set; }
        public string Ten { get; set; }
        public object KyHieu { get; set; }
        public object VanBanQuyDinh { get; set; }
        public object DuongDanTepMau { get; set; }
        public object ToKhaiOnline { get; set; }
        public object ChoPhepKhaiOnline { get; set; }
        public object TrangThaiToKhaiOnline { get; set; }
        public int? LinhVucId { get; set; }
        public object CapThucHien { get; set; }
        public LinhVuc LinhVuc { get; set; }
        public TepMauDonToKhai TepMauDonToKhai { get; set; }
        public List<object> ApiHoSos { get; set; }
        public List<object> ApiLinhVucs { get; set; }
        public List<object> TepMauDonToKhais { get; set; }
        public List<FileTepMauDonToKhai> FileTepMauDonToKhais { get; set; }
        public DonViPhuTrachLinhVuc DonViPhuTrachLinhVuc { get; set; }
        public object IdFormOnline { get; set; }
        public object Href { get; set; }
    }

    public class HoSo
    {
        public int Id { get; set; }
        public string Ten { get; set; }
        public string BanChinh { get; set; }
        public string BanSao { get; set; }
        public bool BatBuoc { get; set; }
        public object MoTa { get; set; }
        public int ThuTu { get; set; }
        public object ThuTuc { get; set; }
        public MauDonToKhai MauDonToKhai { get; set; }
        public int ThuTucId { get; set; }
        public int? MauDonToKhaiId { get; set; }
        public string Href { get; set; }
    }

    public class PhiLePhi
    {
        public int Id { get; set; }
        public int ThuTu { get; set; }
        public string Ten { get; set; }
        public double? MucPhi { get; set; }
        public object TenSoKyHieuNgayThangNamVanBanQuyDinh { get; set; }
        public string MoTa { get; set; }
        public object Ma { get; set; }
        public object ThuTuc { get; set; }
        public int ThuTucId { get; set; }
        public string Href { get; set; }
    }

    public class ThuTucDonViTiepNhan
    {
        public int Id { get; set; }
        public object ThuTuc { get; set; }
        public int ThuTucId { get; set; }
        public int DonViTiepNhanId { get; set; }
        public object CapTiepNhanId { get; set; }
        public string Href { get; set; }
    }

    public class DonViTiepNhan
    {
        public int Id { get; set; }
        public string Ten { get; set; }
        public int? ThuTu { get; set; }
        public string DiaChi { get; set; }
        public string SoDT { get; set; }
        public string Email { get; set; }
        public object Photo { get; set; }
        public int? PhongBanParentId { get; set; }
        public string Loai { get; set; }
        public object ThuocDonViId { get; set; }
        public int CapTiepNhan { get; set; }
        public object NhanViens { get; set; }
        public object DonViPhuTrachLinhVucs { get; set; }
        public object Href { get; set; }
    }

    public class DanhSachCoQuanTiepNhan
    {
        public int Id { get; set; }
        public string Ten { get; set; }
        public int? ThuTu { get; set; }
        public string DiaChi { get; set; }
        public string SoDT { get; set; }
        public string Email { get; set; }
        public string Photo { get; set; }
        public int? PhongBanParentId { get; set; }
        public string Loai { get; set; }
        public object ThuocDonViId { get; set; }
        public int CapTiepNhan { get; set; }
        public object NhanViens { get; set; }
        public object DonViPhuTrachLinhVucs { get; set; }
        public object Href { get; set; }
    }

    public class HinhThucNop
    {
        public int Id { get; set; }
        public string Ten { get; set; }
        public object Href { get; set; }
    }
    public class Errors
    {
    }
    public class AllHinhThucNop
    {
        public int Id { get; set; }
        public string Ten { get; set; }
        public object Href { get; set; }
    }
}