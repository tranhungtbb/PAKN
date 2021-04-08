export class FileStorage {
  ma: number = 0;
  soVanBan: number;
  tieuDe: string;
  ghiChu: string;
  nam: number;
  hopSo: number;
  mucDoTruyCap: number;
  capSo: number;
  namChinhLy: number;
  phongSo: number;
  nguoiNhap: string;
  tenPhong: string;
  hoSoSo: number;
  maThoiGianBaoQuan: number = 1;
  thoiHanBaoQuan: Date ; 
  hoSoSoCu: number;
  nguoiTao: number;
  maKho: number;
  maLoaiHoSo: number;
  nguoiChinhLy: number;
  ngayChinhLy: Date;
  kichHoat:boolean = false;
  xoa: boolean = false;
  ngayTao: string;
  maDonVi: number;
}
export class FileStorageSearch {
  constructor() { 
    this.tieuDe = '';
    this.hoSoSo = null;  
    this.tuNgay = null;  
    this.denNgay = null;  
    this.phongSo = null;
    this.capSo = null;  
    this.maKho = null;
    this.maThoiGianBaoQuan = null;
    this.pageSize = null;
    this.pageIndex = null;
    this.nguoiTao = null;
    this.totalFilter = null;
    this.totalRecord = null;
  } 
  tieuDe: string;
  hoSoSo: number;
  tuNgay: string;
  denNgay: string;
  phongSo: number;
  capSo: number;
  maKho: number;
  maThoiGianBaoQuan: number;
  pageSize: number;
  pageIndex: number;
  nguoiTao: number;
  totalFilter: number; 
  totalRecord: number; 
}
export class FileStorageGrid {
  ma: number;
  soVanBan: number;
  tieuDe: number;
  hoSoSo: number;
  tenKho: string;
  tenPhong: string;
  tenCap: string; 
  maThoiGianBaoQuan: number; 
}
