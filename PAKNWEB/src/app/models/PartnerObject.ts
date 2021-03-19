export class PartnerObject {
  constructor() {
    this.id = 0;
    this.ten = "";
    this.vietTat = "";
    this.ma = "";
    this.diaChi = " ";
    this.dienThoai = " ";
    this.email = " ";
    this.website = " ";
    this.fax = " ";
    this.nguoiTiepCan = null;
    this.ghiChu = " ";
    this.donViId = null;
    this.trangThai = 0;
    this.ngayBatDauHopTac = null;
    this.loai = 1;
    this.tenNguoiTiepCan = "";
    this.peopleApproach = null;
  }
  id: number;
  ten: string;
  vietTat: string;
  ma: string;
  diaChi: string;
  dienThoai: string;
  email: string;
  website: string;
  fax: string;
  nguoiTiepCan: number;
  ghiChu: string;
  donViId: number;
  trangThai: number;
  ngayBatDauHopTac: Date;
  loai: number;
  tenNguoiTiepCan: string;
  peopleApproach: Array<any>;
}
