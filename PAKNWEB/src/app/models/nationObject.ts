export class NationObject {
  constructor() {
    this.id = 0;
    this.xoa = false;
    this.trangThai = true;
    this.moTa = '';
  }
  id: number;
  stt: number;
  tenDuLieu: string;
  code: string;
  moTa: string;
  nhom: string;
  trangThai: boolean;
  xoa: boolean;
}
