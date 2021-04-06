export class NotifyObject {
  constructor() {
    this.ma = 0,
    this.noiDung = '',
    this.nguoiTao = null,
    this.ngayTao = null,
    this.maDonVi = null,
    this.trangThai = 1;
  }
  ma: number;
  noiDung: string;
  nguoiTao: number;
  ngayTao: Date;
  maDonVi: number;
  trangThai: number;
}
export class NotifySearch {
  constructor() {
    this.noiDung = '';
    this.tuNgay = null;
    this.denNgay = null;
    this.pageSize = null;
    this.pageIndex = null;
    this.trangThai = -1;
    this.totalFilter = null;
    this.totalRecord = null;
  }
  noiDung: string;
  tuNgay: string;
  denNgay: string;
  pageSize: number;
  pageIndex: number;
  trangThai: number;
  totalFilter: number;
  totalRecord: number; 
}
