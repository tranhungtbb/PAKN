export class StudentObject {

  constructor() {
    this.id = -1;
    this.ma = "";
    this.loai = 1;
    this.nguonId = "";
    this.anhDaiDien = "";
    this.hoTen = "";
    this.cmnd = "";
    this.ngayCap = null;
    this.noiCap = "";
    this.ngaySinh = null;
    this.gioiTinh = true;
    this.diaChi = "";
    this.tinhTrangHonNhan = 1;
    this.dienThoai = "";
    this.email = "";
    this.facebook = "";
    this.nguoiLienHeKhiCan = "";
    this.quanHeVoiHocVien = "";
    this.dienThoaiLienHe = "";
    this.dienThoaiLienHe1 = "";
    this.diaChiLienHe = "";
    this.trangThai = -1;
    this.tenNvtv = "";
    this.nguoiTuyenSinh = -1;
    this.congTacVien = "";
    this.nguoiTao = -1;
    this.ngayTao = null
    this.chietKhau = null;
    this.nguon = 1;
    this.ghiChu = "";
    this.fileDinhKem = "";
    this.donViId = null;
    this.kyDuHocThang = new Date().getMonth()+1;
    this.kyDuHocNam = new Date().getFullYear();
    this.chiPhiHoSo = null;
  }
  id: number;
  ma: string;
  loai: number;//
  nguonId: string; //
  anhDaiDien: string;
  hoTen: string;
  cmnd: string;
  ngayCap: Date;
  noiCap: string;
  ngaySinh: Date;
  gioiTinh: boolean;
  diaChi: string;
  tinhTrangHonNhan: number;
  dienThoai: string;
  email: string;
  facebook: string;
  nguoiLienHeKhiCan: string;
  quanHeVoiHocVien: string;
  dienThoaiLienHe: string;
  dienThoaiLienHe1: string;
  diaChiLienHe: string;
  trangThai: number;
  tenNvtv: string;
  nguoiTuyenSinh: number;
  congTacVien: string;
  nguoiTao: number;
  ngayTao: Date
  chietKhau: number;
  ghiChu: string;
  nguon: number; //
  fileDinhKem: string;
  donViId: number;
  kyDuHocThang: number;
  kyDuHocNam: number;
  chiPhiHoSo: number;
}

export class StudentQTHTObject {
  constructor() {
    this.id = -1;
    this.ma = "";
    this.tuThang = 1;
    this.tuNam = new Date().getFullYear();
    this.denThang = 1;
    this.denNam = new Date().getFullYear();
    this.capBac = 1;
    this.tenTruong = "";
    this.diaChi = "";
    this.tinhTrangHoc = "";
    this.tinhTrangKhiNao = "";
    this.doiTacId = "";
    this.daLamHoSoNhat = false;
    this.hoSoNhatKhiNao = "";
    this.daLamHoChieu = false;
    this.daHocTiengNhat = false;
    this.hocNhatKhiNao = "";
    this.hocNhatOdau = "";
    this.trinhDoTiengNhat = "";
    this.chungChiTiengNhat = "";
    //check required
    this.dateError = false;
    this.tenTruongError = false;
    this.diaChiError = false;
  }
  id: number;
  ma: string;
  tuThang: number;
  tuNam: number;
  denThang: number;
  denNam: number;
  capBac: number;
  tenTruong: string;
  diaChi: string;
  tinhTrangHoc: string;//number
  tinhTrangKhiNao: string;
  doiTacId: string;
  daLamHoSoNhat: boolean;
  hoSoNhatKhiNao: string;
  daLamHoChieu: boolean;
  daHocTiengNhat: boolean;
  hocNhatKhiNao: string;
  hocNhatOdau: string;
  trinhDoTiengNhat: string;
  chungChiTiengNhat: string;

  dateError: boolean;
  tenTruongError: boolean;
  diaChiError: boolean;
}

export class StudentQTLVObject {
  constructor() {
    this.id = -1;
    this.tuThang = 1;
    this.tuNam = new Date().getFullYear();
    this.denThang = 1;
    this.denNam = new Date().getFullYear();
    this.noiLamViec = "";
    this.diaChi = "";
    this.chucVu = "";
    this.linhVuc = "";
    this.mucLuong = "";
    this.noiDungCongViec = "";

    this.dateError = false;
    this.mucLuongError = false;
    this.noiLamViecError = false;
    this.chucVuError = false;
  }
  id: number;
  tuThang: number;
  tuNam: number;
  denThang: number;
  denNam: number;
  noiLamViec: string;
  diaChi: string;
  chucVu: string;
  linhVuc: string;
  mucLuong: string;
  noiDungCongViec: string;

  dateError: boolean;
  mucLuongError: boolean;
  noiLamViecError: boolean;
  chucVuError: boolean;
}

export class StudentVBCCObject {
  constructor() {
    this.id = -1;
    this.ten = "";
    this.nganhNghe = "";
    this.xepLoai = "";
    this.ngayCap = null;

    this.tenError = false;
    this.xepLoaiError = false;
    this.ngayCapError = false;
    this.nganhNgheError = false;
  }
  id: number;
  ten: string;
  nganhNghe: string;// ;
  xepLoai: string;
  ngayCap: Date;

  tenError: boolean;
  xepLoaiError: boolean;
  ngayCapError: boolean;
  nganhNgheError: boolean;
}

export class StudenTPGDObject {
  constructor() {
    this.id = -1;
    this.hoTen = "";
    this.nganhNghe = "";
    this.moiQuanHe = "";
    this.namSinh = "";
    this.diaChiThuongTru = "";
    this.tinhTrang = "";
    this.thuNhapHangThangGiaDinh = null;

    this.hoTenError = false;
    this.moiQuanHeError = false;
    this.namSinhError = false;
    this.nganhNgheError = false;
  }
  id: number;
  hoTen: string;
  moiQuanHe: string;
  namSinh: string;
  nganhNghe: string;// number;
  diaChiThuongTru: string;
  tinhTrang: string;
  thuNhapHangThangGiaDinh: number;

  hoTenError: boolean;
  moiQuanHeError: boolean;
  namSinhError: boolean;
  nganhNgheError: boolean;

}

export class StudenNTTNObject {
  constructor() {
    this.id = -1;
    this.hoTen = "";
    this.tuCachLuuTruId = null;
    this.moiQuanHe = "";
    this.diaChiTaiNhat = "";
    this.ghiChu = "";
    this.dienThoai = "";
    this.facebook = "";

    this.hoTenError = false;
    this.moiQuanHeError = false;
    this.diaChiTaiNhatError = false;
  }
  id: number;
  hoTen: string;
  moiQuanHe: string;
  diaChiTaiNhat: string;
  tuCachLuuTruId: number;// number;
  ghiChu: string;
  dienThoai: string;
  facebook: string;

  hoTenError: boolean;
  moiQuanHeError: boolean;
  diaChiTaiNhatError: boolean;
}

export class StudenTTKObject {
  constructor() {
    this.id = -1;
    this.soThich = "";
    this.tinhCach = "";
    this.soTruong = "";
    this.lyDoMuonDiNhat = "";
    this.muonLamGiKhiVeVn = "";
    this.muonTietKiemBaoNhieuTien = "";
    this.matTrai = "";
    this.matPhai = "";
    this.diUngManCam = "";
    this.muMau = null;
    this.tayThuan = null;
    this.chieuCao = null;
    this.canNang = null;
    this.xamHinh = null;
    this.nhomMau = null;
    this.iq = "";
    this.hutThuoc = null;
    this.uongRuou = 0;
  }
  id: number;
  soThich: string;
  tinhCach: string;
  soTruong: string;
  lyDoMuonDiNhat: string;
  muonLamGiKhiVeVn: string;
  muonTietKiemBaoNhieuTien: string;
  matTrai: string;
  matPhai: string;
  diUngManCam: string;
  muMau: boolean;
  tayThuan: number;
  chieuCao: number;
  canNang: number;
  xamHinh: boolean;
  nhomMau: number;
  iq: string;
  hutThuoc: boolean;
  uongRuou: number;
}