export class BienBanLamViecObject {
  id: number = 0;
  idKeHoachNam: number;
  idDot: number;
  idCuocThanhTra: number;
  idDoiTuong: number;
  diaDiem: string = "";
  status: number = 1;
  namHienThi: any;
  nam: number;
  thoiGian: Date;
  soVB: string = "";
  idQuyetDinh: number;
}

export class BBLV_NDGiaiTrinh {
  id: number = 0;
  idBienBanLamViec: number;
  noiDung: string = "";
  ketQua: string = "";
  noiDungGiaiTrinh: string = "";
}
