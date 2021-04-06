export class PaymentRequestObject {
  constructor() {
    this.id = 0;
    this.ten = "";
    this.vietTat = "";
    this.ma = "";
    this.diaChi = " ";
    this.dienThoai = " ";
    this.email = " ";
    this.website = " ";
    this.nguoiTiepCan = null;
    this.ghiChu = " ";
    this.donViId = null;
    this.trangThai = 0;
    this.loai = 1;
  }
  id: number;
  ten: string;
  vietTat: string;
  ma: string;
  diaChi: string;
  dienThoai: string;
  email: string;
  website: string;
  nguoiTiepCan: number;
  ghiChu: string;
  donViId: number;
  trangThai: number;
  loai: number;

}
