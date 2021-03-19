export class DepartmentObject {
  constructor() {
    this.ten = '';
    this.email = '';
    this.moTa = '';
    this.code = '';
    this.soDienThoai = '';
    this.maCapCha = null;
  }
  ma: number;
  ten: string;
  maCapCha: number;
  code: string;
  moTa: string;
  nguoiDaiDien: number;
  kichHoat: boolean;
  isDonVi: boolean;
  xoa: boolean;
  soDienThoai: string;
  loaiPhongBan: number;
  email: string;
  lstNguoiThamGia: any = [];
  nguoiPhuTrachId: number;
}
