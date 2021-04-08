export class DocumentTypeObject {
  constructor() {
    this.id = 0;
    this.code = "";
    this.tenDuLieu = "";
    this.moTa = "";
    this.xoa = null;
    this.maDonVi = null;
    this.giaTri = "";
    this.thoiGianNhac = null;
    this.giuongId = null;
    this.loai = 1;
  }
  id: number;
  code: string;
  tenDuLieu: string;
  moTa: string;
  trangThai: boolean = true;
  xoa: boolean;
  maDonVi: number;
  stt: number;
  loai: number;

  //payment method
  thoiGianNhac: number;

  //form
  nam: number;

  //equipping student
  gia: number

  //tu cach luu tru
  giaTri: string;

  //cap cha
  capChaId: number;

  soGiuong:string;
  soTu:string;
  kyTucXaId: number;

  giuongId = null;
}
