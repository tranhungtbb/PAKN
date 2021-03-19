export class FoldersObject {
  constructor() {
    this.id = 0,
      this.tieuDe = '',
      this.maCapCha = null,
      this.maDonVi = null;
    this.nguoiTao = null;
    this.ngayTao = null;
    this.daXoa = false
  }
  id: number;
  tieuDe: string;
  maCapCha: number;
  maDonVi: number;
  nguoiTao: number;
  ngayTao: Date;
  daXoa: boolean
}
