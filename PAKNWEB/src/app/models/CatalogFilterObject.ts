export class CatalogFilterObject {
  constructor() {
    this.departmentId = null;
    this.name = null;
    this.code = null;
    this.trangthai = null;
    this.maloaivanban = null;
    this.tendonvi = null;
    this.tenphongban = null;
    this.type = null;
    this.pageSize = null;
    this.pageIndex = null;
  }

  departmentId: number;
  name: string;
  code: string;
  trangthai: number;
  maloaivanban: number;
  tendonvi: string;
  tenphongban: string;
  type: number;
  pageSize: number;
  pageIndex: number;
}
