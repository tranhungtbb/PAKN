export class resolutionTypeObject {
  constructor() {
    this.id = 0;
    this.name = "";
    this.code = "";
    this.description = "";
    this.parentId = null;

  }
  id: number;
  name: string;
  code: string;
  description: string; 
  status: boolean = true;  
  xoa: boolean = false;
  parentId: number;
}


export class modelSearchResolutionTypeObject {
  constructor() {
    this.name = null;
    this.code = null
    this.description = null
    this.status = null;
    this.pageIndex = 1;
    this.pageSize = 20;

  }
  name: string;
  code: string;
  description: string;
  status: boolean;
  parentId: number = null;
  pageSize: number;
  pageIndex: number;
}
