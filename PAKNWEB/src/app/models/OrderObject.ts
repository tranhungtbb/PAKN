export class OrderObject {
  constructor() {
    this.id = -1;
    this.loaiYeuCau = null; //1. Thực tập sinh; 2.Kỹ thuật viên
    this.ma = ""
    this.congTyId = null
    this.maCongTy = "";
    this.tenCongTy = "";
    this.giamDoc = "";
    this.diaChi = "";
    this.noiLamViec = "";
    this.website = "";
    this.email = "";
    this.khoaTuyen = null;
    this.doiTacId = null;
    this.ngayPhongVan = null;
    this.hanGuiCV = "";
    this.ngayNhapCanh = "";
    this.nganhNgheId = null;
    this.soNguoiTiepNhan = null;
    this.soNamTiepNhan = null;
    this.soNuTiepNhan = null;
    this.doTuoiTu = 18;
    this.doTuoiDen = 99;
    this.doTuoi =  [this.doTuoiTu, this.doTuoiDen];
    this.tayThuan = 3;
    this.thiLuc = "";
    this.hocVan = 1;
    this.noiDungCongViec = "";
    this.nguyenVongKhac = "";
    this.kyThiThichUng = 0;
    this.kiemTraIQ = 0;
    this.kiemTraTheLuc = 0;
    this.kiemTraKyNang = 0;
    this.noiDungKiemTraKyNang = "";
    this.noiDungKhac = "";
    this.troCap = null;
    this.thoiGianHoc = null;
    this.luongCoBan = null;
    this.thueThuNhapCaNhan = null;
    this.bhtn = null;
    this.cacKhoanBaoHiem = null;
    this.kyTuc = "";
    this.dienNuocGas = "";
    this.hoTroKhac = null;
    this.luongLamThem = null;
    this.thucLinh = null;
    this.soNgayDiLam = null;
    this.soNgayNghi = null;
    this.trangThai = null;
    this.nguoiTiepCan = null;
    this.ngayHenKyHD = null;
    this.donViId = -1;
    this.tenDoiTac = "";
    this.tenLoaiYeuCau = "";
    this.tenNganhNghe = "";
    this.tenNguoiTiepCan = "";
    this.peopleApproach = null;
  }
  id: number
  loaiYeuCau: number
  ma: string
  congTyId: number
  maCongTy: string
  tenCongTy: string
  giamDoc: string
  diaChi: string
  noiLamViec: string
  website: string
  email: string
  khoaTuyen: number
  doiTacId: number
  ngayPhongVan: Date
  hanGuiCV: string
  ngayNhapCanh: string
  nganhNgheId: number
  soNguoiTiepNhan: number
  soNamTiepNhan: number
  soNuTiepNhan: number
  doTuoiTu: number
  doTuoiDen: number
  doTuoi:  number[]
  tayThuan: number
  thiLuc: string
  hocVan: number
  noiDungCongViec: string
  nguyenVongKhac: string
  kyThiThichUng: number
  kiemTraIQ: number
  kiemTraTheLuc: number
  kiemTraKyNang: number
  noiDungKiemTraKyNang: string
  noiDungKhac: string
  troCap: string
  thoiGianHoc: string
  luongCoBan: string
  thueThuNhapCaNhan: string
  bhtn: string
  cacKhoanBaoHiem: string
  kyTuc: string
  dienNuocGas: string
  hoTroKhac: string
  luongLamThem: string
  thucLinh: string
  soNgayDiLam: string
  soNgayNghi: string
  trangThai: number
  nguoiTiepCan: number
  ngayHenKyHD: Date
  donViId: number
  tenDoiTac: string
  tenLoaiYeuCau: string
  tenNganhNghe: string
  tenNguoiTiepCan: string
  peopleApproach:Array<any>;
}
