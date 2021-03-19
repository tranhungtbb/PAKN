export class categoryObject {
  constructor() {
    this.id = 0;
    this.code = "";
    this.tenDuLieu = "";
    this.moTa = "";
    //this.trangThai = "";
    this.xoa = null;
    this.maDonVi = null;
    this.stt = null;
    this.sTT = null;
  }
  id: number;
  code: string;
  tenDuLieu: string;
  moTa: string; 
  trangThai: boolean = true;  
  xoa: boolean;
  maDonVi: number;
  stt: number;
  sTT: number;
}
