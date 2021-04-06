export class InspectionSearchObject {
  constructor() {
    this.idDot = null;
    this.idQuyetDinh = null;
    this.so = "";
    this.tieuDe = "";
  }

  idDot: number;
  idQuyetDinh: number;
  so: string;
  tieuDe: string;
}
export class InspectionListObject {
  constructor() {
    this.id = 0;
    this.stt = null;
    this.title = null;
    this.inspectionPlan = null;
    this.inspectedUnit = null;
    this.leader = null;
    this.fromDate = null;
    this.toDate = null;
  }

  id: number;
  stt: number;
  inspectionPlan: string;
  title: string;
  inspectedUnit: string;
  leader: string;
  fromDate: string;
  toDate: string;
}


export class InspectionObject {
  constructor() {
    this.id = 0;
    this.idDot = null;
    this.idQuyetDinh = null;
    this.idKeHoachNam = null;
    this.tieuDe = '';
    this.mucDich = '';
    this.yeuCau = '';
    this.phamVi = '';
    this.thoiKy = [];
    this.phuongPhapTH = '';
    this.cheDoTinBao = '';
    this.dieuKienVatChat = '';
    this.so = '';
    this.thoiHan = null;
    this.trangThai = 1;
    this.nam = new Date().getFullYear();
    this.tuNgay = null;
    this.denNgay = null;
  }

  id: number;
  idDot: number;
  idQuyetDinh: number;
  idKeHoachNam: number;
  tieuDe: string;
  mucDich: string;
  yeuCau: string;
  phamVi: string;
  thoiKy:dropDown[];
  phuongPhapTH: string;
  cheDoTinBao: string;
  dieuKienVatChat: string;
  so: string;
  thoiHan: number;
  trangThai: number;
  nam: number;
  tuNgay: Date;
  denNgay: Date;
  tieuDeKH: string;
  tenDot: string;
  soQD: string;
}
export class dropDown {
  value: number;
  text: string;
}

export class InspectionGroupObject {
  constructor() {
    this.id = 0;
    this.idCuocThanhTra = null;
    this.ten = "";
    this.type = null;
    this.createdBy = null;
    this.createdDate = null;
    this.listThanhVien = [];
  }

  id: number;
  idCuocThanhTra: number;
  ten: string;
  type: number;
  createdBy: number;
  createdDate: Date;
  listThanhVien: Array<InspectionGroupMemberObject>;
}

export class InspectionGroupMemberObject {
  constructor() {
    this.id = 0;
    this.idToTDGS = null;
    this.soHieuThe = "";
    this.hoVaTen = "";
    this.chucVu = null;
    this.chucVuTrongNhom = null;
    this.idThanhVien = null;
    this.noiDung = "";
    this.idCuocThanhTra = null;

  }

  id: number;
  idToTDGS: number;
  soHieuThe: string;
  hoVaTen: string;
  chucVu: number;
  chucVuTrongNhom: number;
  idThanhVien: number;
  noiDung: string;
  idCuocThanhTra: number;
}

export class InspectionMissionObject {
  constructor() {
    this.id = 0;
    this.idCuocThanhTra = null;
    this.idDot = null;
    this.idDotKeHoach = null;
    this.noiDung = "";
    this.diaDiem = "";
    this.cachThuc = "";
    this.ghiChu = "";
    this.listDoiTuong = [];
    this.listThoiGian = [];
  }

  id: number;
  idCuocThanhTra: number;
  idDot: number;
  idDotKeHoach: number;
  noiDung: string;
  diaDiem: string;
  cachThuc: string;
  ghiChu: string;
  listDoiTuong:dropDown[];
  listThoiGian:any[];
}

export class InspectionDiartListObject {
  constructor() {
    this.id = 0;
    this.nam = null;
    this.idDot = null;
    this.idKeHoachNam = null;
    this.idCuocThanhTra = null;
    this.idNhomThanhVien = null;
    this.IdDaiDienDoiTuong = null;
    this.ngay = new Date();
    this.soPhieu = '';
  }

  id: number;
  nam: number;
  idDot: number;
  idKeHoachNam: number;
  idCuocThanhTra: number;
  idNhomThanhVien: number;
  IdDaiDienDoiTuong: number;
  ngay: Date;
  soPhieu: string;
}

export class InspectionDiartResultObject {
  constructor() {
    this.id = 0;
    this.thanhVienId = null;
    this.idNhatKyThanhTra = null;
    this.idDoiTuong = null;
    this.noiDung = '';
    this.ketQua = '';
    this.ghiChu = '';
  }

  id: number;
  thanhVienId: number;
  idDoiTuong: number;
  noiDung: string;
  ketQua: string;
  ghiChu: string;
  idNhatKyThanhTra: number;
}

export class InspectionPeriodResultObject {
  constructor() {
    this.id = 0;
    this.code = '';
    this.name = '';
    this.inspeactionPlan = null;
    this.year = null;
    this.fromDate = null;
    this.toDate = null;
    this.unitImpID = null;
    this.yearDate = new Date();
  }

  id: number;
  code: string;
  name: string;
  inspeactionPlan: number;
  unitImpID: number;
  year: number;
  fromDate: Date;
  toDate: Date;
  yearDate: Date;
}

export class InspectionDiartObject {
  constructor() {
    this.soPhieu = '';
  }
  id: number = 0;
  idKeHoachNam: number;
  idDot: number;
  idCuocThanhTra: number;
  idNhomThanhVien: number;
  idNhomThanhVienCT: number;
  idDoiTuong: number;
  idDaiDienDoiTuong: number;
  diaDiem: string = "";
  trangThai: number = 1;
  nam: number;
  namHienThi: any;
  thoiGian: Date;
  soPhieu: string = "";
}

export class InspectionInclusionObject {
  id: number = 0;
  idKeHoachNam: number;
  idDot: number;
  idCuocThanhTra: number;
  trangThai: number = 1;
  nam: number;
  namHienThi: any;
  ngayKetLuan: Date;
  ngayQuyetDinh: Date;
  soKetLuan: string="";
  khaiQuatChung:string="";
  nhungMatLamDuoc:string="";
  tonTai:string="";
  xacDinhTrachNhiem:string="";
  kienNghi:string="";
}