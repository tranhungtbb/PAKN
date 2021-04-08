export class ResolutionObject {
  constructor() {
    this.id = 0;
    this.trangThai = 0;
    this.thoiGian = new Date;
    this.thoiGianCoHieuLuc = null;
    this.thoiGianHetHieuLuc = null;
    this.thoiGianCoHieuLuc = new Date;
    this.thoiGianHetHieuLuc = new Date;
    this.coQuanBanHanh = null;
    this.nguoiKy = '';
    this.loaiNghiQuyet = null;
  }
  id: number;
  maKyHop: number = null;
  so: string;
  ten: string;
  thoiGian: Date;
  noiDung: string;
  fileDinhKem: string;
  thoiGianCoHieuLuc: Date;
  thoiGianHetHieuLuc: Date
  trangThai: number;
  fileType: number;
  name: string;
  nguoiTao: number;
  linhVucId: number;
  coQuanBanHanh: string;
  nguoiKy: string;
  loaiNghiQuyet: number;
}
