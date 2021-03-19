export class RecordsObject {
  constructor() {
    this.id = 0;
    this.trangThai = 0;
    this.gioiTinhNguoiKhieuNai = true;
    this.gioiTinhDtkn = true;
    this.ten = '';
    this.noiDung = '';
    this.thoiGian = new Date;
    this.quocTichNkn = "Việt Nam";
    this.quocTichDtkn = "Việt Nam";
    this.hoTenNguoiKhieuNai = '';
    this.hoTenDoiTuongKhieuNai = '';
    this.ngheNghiepDtkn = '';
    this.ngheNghiepNguoiKhieuNai = '';
    this.chiTietDiaDanhDtkn = '';
    this.chiTietDiaDanhNkn = '';
    this.chucVuDtkn = '';
    this.noiCongTac = '';
    this.isHopLe = false;
    this.thongTinBoXung = '';
  }
  id: number;
  thoiGian: Date;
  nguonId: number;
  hoTenNguoiKhieuNai: string;
  gioiTinhNguoiKhieuNai: boolean;
  ngheNghiepNguoiKhieuNai: string;
  tinhThanhPhoNkn: string;
  quanHuyenNkn: string;
  xaPhuongNkn: string;
  chiTietDiaDanhNkn: string;
  tenCaNhanDonVi: string;
  quocTichNkn: string;
  danToc: string;
  nguoiTao: number;
  noiDung: string;
  hoTenDoiTuongKhieuNai: string;
  gioiTinhDtkn: boolean;
  ngheNghiepDtkn: string;
  chucVuDtkn: string;
  tinhThanhPhoDtkn: string;
  quanHuyenDtkn: string;
  xaPhuongDtkn: string;
  chiTietDiaDanhDtkn: string;
  noiCongTac: string;
  quocTichDtkn: string;
  danTocDtkn: string;
  nguoiXuLy: number;
  ketLuan: string;
  trangThai: number;
  ten: string;
  donThuId: number;
  nguonDonThu: string;
  congBo: boolean;
  nguoiCapNhat: number;
  ngayCapNhat: Date;
  countView: number;
  isView: string;
  loaiDonThu: number;
  loaiNkn: number;
  loaiDtkn: number;
  isHopLe: boolean;
  thongTinBoXung: string;
}
