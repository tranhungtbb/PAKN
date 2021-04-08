export class hoSoThanhTraObject {
  constructor() {
    this.id = 0;
    this.nam = null;
    this.idKeHoachNam = null;
    this.idQuyetDinh = null;
    this.idDot = null;
    this.idCuocThanhTra = null;
    this.tenHoSo = "";
    this.maHoSo = "";
    this.ngayTao = null;
    this.moTa = "";
    this.status = true;
    this.createdBy = null;
    this.createdDate = null;

  }
  id: number;
  nam: any;
  idKeHoachNam: number;
  idQuyetDinh: number;
  idDot: number;
  idCuocThanhTra: number;
  tenHoSo: string;
  maHoSo: string;
  ngayTao: Date;
  moTa: string;
  status: any;
  createdBy: number;
  createdDate: Date;
}
export class hoSoThanhTraTaiLieuObject {
  constructor() {
    this.id = 0;
    this.idHoSoThanhTra = null;
    this.soHieu = "";
    this.tenTaiLieu = "";
    this.trichYeu = "";
    this.fileType = null;
    this.filePath = "";
    this.thoiGian = null;
    this.name = "";

  }
  id: number;
  idHoSoThanhTra: number;
  soHieu: string;
  tenTaiLieu: string;
  trichYeu: string;
  fileType: number;
  filePath: string;
  thoiGian: Date;
  name: string;
}
