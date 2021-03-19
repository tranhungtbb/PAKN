export class UserObject {
  constructor() {
    this.ma = -1;
    this.tenDangNhap = null;
    this.hoTen = "";
    this.homThu = "";
    this.gioiTinh = true;
    this.loginId = "";
    this.unitId = null;
    this.departmentId = null;
    this.kichHoat = true;
    this.isHaveToken = false;
    this.isLeader = false;
    this.unitIdCapSo = 0;
    this.accountType = 0;
    this.maChucVu = null;
    this.checkFileChange = false;
    this.listPhongBan = [];
    this.role = 2;
    this.diaChi = "";
    this.dienThoai = "";
  }
  ma: number;
  tenDangNhap: string;
  loginId: string;
  hoTen: string;
  homThu: string;
  dienThoai: string;
  ngaySinh: string;
  gioiTinh: boolean;
  unitId: number;
  departmentId: number;
  kichHoat: boolean;
  isHaveToken: boolean;
  isLeader: boolean;
  unitIdCapSo: number;
  accountType: number;
  donViId: number;
  anhDaiDien: string;
  phongBanId: number;
  xoa: boolean;
  maChucVu: boolean;
  matKhau: string;
  isSuperAdmin: boolean;
  checkFileChange: boolean;
  listPhongBan: any[];
  role: number;
  diaChi: string;

}
