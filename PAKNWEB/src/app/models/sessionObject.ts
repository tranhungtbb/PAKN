export class SessionObject {
  constructor() {
    this.id = 0;
    this.trangThai = true;
    this.xoa = false;
    this.moTa = '';
    this.ngayBd = new Date;
    this.ngayKt = new Date;
  }
  id: number;
  stt: number;
  tenDuLieu: string;
  code: string;
  moTa: string;
  ngayBd: Date;
  ngayKt: Date;
  trangThai: boolean;
  xoa: boolean;
}
