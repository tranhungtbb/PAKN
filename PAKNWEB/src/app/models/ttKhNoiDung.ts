export class DonViThucHienTT {
  id: number = 0;
  idKeHoachNam: number;
  idDoiTuong: number;
  index: number = 0;
  lstKeHoachDoiTuong: Array<TTKHNoiDung> = Array<TTKHNoiDung>();
}

export class TTKHNoiDung {
  id: number = 0;
  idDVThucHien: number;
  idKeHoachNam: number;
  index: number = 0;
  noiDung: string = "";
  lstDoiTuongItems: Array<TTKHNDNDoiTuong> = Array<TTKHNDNDoiTuong>();
}

export class TTKHNDNDoiTuong {
  id: number = 0;
  idNoiDung: number;
  idDoiTuong: number;
  doiTuongText: string = "";
}
