export class RecommendationsTypeObject {
  constructor() {
    this.id = 0;
    this.xoa = false;
    this.trangThai = true;
    this.moTa = '';
    this.code = '';
    this.tenDuLieu = '';
  }
  id: number;
  stt: number;
  tenDuLieu: string;
  code: string;
  moTa: string;
  trangThai: boolean;
  xoa: boolean;
}
