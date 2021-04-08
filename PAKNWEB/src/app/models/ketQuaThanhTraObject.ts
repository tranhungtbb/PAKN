export class ketQuaThanhTraObject {
  constructor() {
    this.id = 0;
    this.nam = null;
    this.idKeHoachNam = null;
    this.idDot = null;
    this.idQuyetDinh = null;
    this.idCuocThanhTra = null;
    this.idNhomThanhVien = null;
    this.so = "";
    this.ngay = null;
    this.soQuyetDinh = "";
    this.tuNgay = null;
    this.denNgay = null;
    this.tenNhomThanhVien = "";
    this.diaDiem = "";
    this.status = 0;
    this.tinhHinh = "";
    this.ketQuaThanhTra = "";
    this.nhanXetKetLuan = "";
    this.bienPhapXuLy = "";
    this.kienNghi = "";
    this.quyetDinhText = "";
    this.createdBy = null;
    this.createdDate = null;

  }
  id: number;
  nam: any;
  idKeHoachNam: number;
  idDot: number;
  idQuyetDinh: number;
  idCuocThanhTra: number;
  idNhomThanhVien: number;
  so: string;
  ngay: Date;
  soQuyetDinh: string;
  tuNgay: Date;
  denNgay: Date;
  tenNhomThanhVien: string;
  diaDiem: string; 
  status: number;
  tinhHinh: string;
  ketQuaThanhTra: string;
  nhanXetKetLuan: string;
  bienPhapXuLy: string;
  kienNghi: string;
  quyetDinhText: string;
  createdBy: number;
  createdDate: Date;
}
