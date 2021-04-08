export class BienBienLamViec {
  id: number = 0;
  idKeHoachNam: number;
  idDot: number;
  idCuocThanhTra: number;
  idNhomThanhVien: number;
  idNhomThanhVienCt: number;
  idDoiTuong: number;
  idDaiDienDoiTuong: number;
  diaDiem: string = "";
  status: number = 1;
  nam: number;
  thoiGian: Date;
  createdBy: number;
  createdDate: Date;
  soVb: string = "";
  namHienThi: any;
}

export class BBLVNDGiaiTrinh {
  id: number = 0;
  idBienBanLamViec: number;
  noiDung: string = "";
  ketQua: string = "";
  noiDungGiaiTrinh: string = "";
  ycgiaiTrinh: boolean = false;
  createdBy: number;
  createdDate: Date;
  index: number = 0;
  files: Array<any> = Array<any>();
  lstXoaFiles: Array<any> = Array<any>();
}
