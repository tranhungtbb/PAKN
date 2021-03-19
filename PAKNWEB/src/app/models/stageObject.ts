export class StageObject {
  constructor() {
    this.id = 0;
    this.xoa = false;
    this.trangThai = true;
    this.ngayBd = new Date;
    this.ngayKt = new Date;
    this.moTa = '';
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
