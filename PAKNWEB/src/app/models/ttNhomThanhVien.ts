export class TTNhomThanhVien {
  id: number = 0;
  idKeHoachNam: number = 0;
  idQuyetDinh: number = 0;
  type: number = 0;
  createdBy: number;
  createdDate: Date;
  ten: string = "";
}

export class TTNhomThanhVienCT {
  id: number = 0;
  idKeHoachNam: number = 0;
  idNhomThanhVien: number = 0;
  soHieuThe: string = "";
  hoVaTen: string = "";
  chucVu: number;
  chucVuTrongNhom: number;
  chucVuText: string = "";
  chucVuTrongNhomText: string = "";
  idThanhVien: number = 0;
}
