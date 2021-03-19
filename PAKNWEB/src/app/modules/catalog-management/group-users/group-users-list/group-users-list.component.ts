import { Component, OnInit, Renderer2, Inject, Input, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
// import { DOCUMENT } from '@angular/platform-browser';
import { DOCUMENT } from '@angular/common';
import { MenuPassingObject } from '../../../system-management/components/orgnization/menuPassingObject';
import { DepartmentTree } from '../../../../models/departmentTree';
import { ToastrService } from 'ngx-toastr';
import { UserInfoStorageService } from '../../../../commons/user-info-storage.service';
import { SystemconfigService } from '../../../../services/systemconfig.service';
import { CatalogValueObject } from '../../../../models/catalogValueObject';
import { CatalogsValueRequest } from '../../../../models/catalogsValueRequest';
import { DocumentTypeObject } from '../../../../models/documentTypeObject';

// declare var jquery: any;
declare var $: any;

@Component({
  selector: 'app-group-users-list',
  templateUrl: './group-users-list.component.html',
  styleUrls: ['./group-users-list.component.css']
})
export class GroupUsersListComponent implements OnInit {

  updateForm: FormGroup;
  deleteForm: FormGroup;
  Id: number;
  nhomNguoiDungId: number;
  public data: DocumentTypeObject = new DocumentTypeObject();
  public lstCatalogValue: CatalogValueObject[];
  public item: any;
  public dataDelete = new DocumentTypeObject();

  public totalRecords = 0;
  isFilterable: boolean = false;

  submitted: boolean = false;
  @ViewChild('dataTable', {static: false}) public dataTable: any;

  isLoading = false;

  @Input() departmentInfo: DepartmentTree;
  @Input() departmentId: number;

  object: MenuPassingObject = new MenuPassingObject();

  model: CatalogsValueRequest = {
    departmentId: 0,
    type: 14,
  };


  errorMessage: any;
  nameSearch: string = '';
  codeSearch: string = '';
  stt: string = '';
  pageindex: number = 1;
  pagesize: number = 20;

  pageCount: number = 0;
  total: number = 0;
  lstPage: number[] = [];
  TrangThai: number = null;
  Loai: number = null;
  userLoginId: string = '';

  listPageIndex: any[] = [];
  pageSizeGrid: number;

  lstUsers: any;
  lstNguoiDungNhomNguoiDung: any;
  lstNguoiDungNhomNguoiDungSelected: any;

  constructor(private _fb: FormBuilder,
    private _avRoute: ActivatedRoute,
    private _renderer2: Renderer2,
    @Inject(DOCUMENT) private _document,
    private _router: Router,
    private _toast: ToastrService,
    private localStorage: UserInfoStorageService,
    private _systemConfigService: SystemconfigService) {
  }

  ngOnInit() {

    this.userLoginId = this.localStorage.getUserId();
    //this.GetDocumentTypeDepartmentId();
    this.buildForm();
    this.departmentId = this.localStorage.getUnitId();

  }

  getNhomNguoiDungNguoiDung() {
    var request = {
      NhomNguoiDungId: this.nhomNguoiDungId
    }
    this._systemConfigService.getNhomNguoiDungNguoiDung(request).subscribe((data) => {
      if (data.status === 1) {
        this.lstNguoiDungNhomNguoiDung = data.listUsers;
      }
    }, error => {
      console.error(error);
      alert(error);
    });
  }
  preCreateNguoiDungNhomNguoiDung(id:number) {
    this.lstNguoiDungNhomNguoiDungSelected = [];
    this.nhomNguoiDungId = id;
    this.getUserForCreate();
    this.getNhomNguoiDungNguoiDung();
  }

  onCreateNguoiDungNhomNguoiDung() {
    if (this.lstNguoiDungNhomNguoiDungSelected.length == 0) {
      this._toast.error('Vui lòng chọn người dùng!');
      return;
    }
    var request = {
      ListNguoiDung: this.lstNguoiDungNhomNguoiDungSelected,
      NhomNguoiDungId: this.nhomNguoiDungId
    }
    this._systemConfigService.createNguoiDungNhomNguoiDung(request).subscribe((data) => {
      if (data.status === 1) {
        this._toast.success("Thêm người dùng thành công");
        this.lstNguoiDungNhomNguoiDungSelected = [];
        this.getUserForCreate();
        this.getNhomNguoiDungNguoiDung();
      } else {
        this._toast.error("Anh/chị chưa nhập người dùng");
      }
    });
  }
  onDeleteNguoiDungNhomNguoiDung(id: number) {
    var request = {
      Id: id
    }
    this._systemConfigService.DeleteNguoiDungNhomNguoiDung(request).subscribe(success => {
      if (success.status == 1) {
        this._toast.success("Xóa thành công");
        this.getUserForCreate();
        this.getNhomNguoiDungNguoiDung();
      } else {
        this._toast.error(success.message);
      }
    });
  }

  getUserForCreate() {
    this.lstNguoiDungNhomNguoiDungSelected = [];
    var request = {
      NhomNguoiDungId : this.nhomNguoiDungId
    }
    this._systemConfigService.getUserForCreateByDepartment(request).subscribe((data) => {
      if (data.status === 1) {
        this.lstUsers = data.listUsers;
      }
    }, error => {
      console.error(error);
      alert(error);
    });
  }

  buildForm() {
    this.updateForm = this._fb.group({
      code: [this.data.code, Validators.required],
      tenDuLieu: [this.data.tenDuLieu, Validators.required],
      moTa: [this.data.moTa],
      trangThai: [this.data.trangThai],
      loai: [this.data.loai],
      stt: [this.data.stt, Validators.required]
    });
  }

  rebuilForm() {
    this.updateForm.reset({
      code: this.data.code,
      tenDuLieu: this.data.tenDuLieu,
      moTa: this.data.moTa,
      trangThai: this.data.trangThai,
      loai: this.data.loai,
      stt: this.data.stt
    });
  }

  preUpdate(dataUpdate: DocumentTypeObject) {
    this.GetByCatalogValueId(dataUpdate.id);
    this.submitted = false;
  }

  GetByCatalogValueId(id: number) {
    this._systemConfigService.getCatalogValueById(14, id).subscribe((data) => {
      if (data.status === 1) {
        this.data = data.nhomNguoiDung;
        this.rebuilForm();
        $("#modalEdit").modal('show');
      } else if (data.status === 2) {

      }
    }, error => {
      console.error(error);
      alert(error);
    });
  }

  onSave() {
    this.submitted = true;
    if (this.data.tenDuLieu.trim() == '') {
      this.data.tenDuLieu = this.data.tenDuLieu.trim();
      return;
    }
    if (this.data.code.trim() == '') {
      this.data.code = this.data.code.trim();
      return;
    }

    this.data.code = this.data.code.trim();
    this.data.tenDuLieu = this.data.tenDuLieu.trim();
    this.data.moTa = this.data.moTa.trim();
    this.data.trangThai = this.data.trangThai;
    this.data.loai = this.data.loai;
    this.data.stt = this.data.stt;
    if (this.updateForm.invalid) {
      return;
    }
    this._systemConfigService.updateCatalogValue(this.data, 14).subscribe(success => {
      if (success.status == 1) {

        this._toast.success("Cập nhật thành công");
        $("#modalEdit").modal("hide");
        this.GetDocumentTypeDepartmentId();
      } else {
        this._toast.error(success.message);
      }
    });
    this.TrangThai = null;
  }

  preDelete(dataDel: DocumentTypeObject) {
    this.dataDelete = dataDel;
    this.onDelete();
  }

  onDelete() {
    this.dataDelete.xoa = true;
    this._systemConfigService.deleteCatalogValue(this.dataDelete, 14).subscribe(success => {
      if (success.status == 1) {
        this._toast.success("Xóa thành công");
        this.pageindex = 1;
        this.dataTable.first = 0;
        this.GetDocumentTypeDepartmentId();

      } else {
        this._toast.error(success.message);
      }
    });
  }

  onPageChange(event: any): void {
    this.pagesize = event.rows;
    this.pageindex = (event.first / event.rows) + 1;
    this.GetDocumentTypeDepartmentId();
  }


  dataStateChange(): void {
    this.pageindex = 1;
    this.dataTable.first = 0;
    this.GetDocumentTypeDepartmentId();
  }

  get f() { return this.updateForm.controls; }
  GetDocumentTypeDepartmentId() {
    this.nameSearch = this.nameSearch.trim();
    this.codeSearch = this.codeSearch.trim();
    this.model.type = 14;

    var request = {
      DepartmentId: this.departmentId,
      Type: this.model.type,
      PageSize: this.pagesize,
      PageIndex: this.pageindex,
      Name: this.nameSearch,
      Code: this.codeSearch,
      TrangThai: this.TrangThai,
      Loai: this.Loai,
      stt: this.stt
    }
    this._systemConfigService.getCatalogValueByDepartmentId(request).subscribe((data) => {
      if (data.status === 1) {
        this.lstCatalogValue = data.listNhomNguoiDung;
        this.totalRecords = data.totalRecords;
      } else if (data.status === 2) {

      }
    }, error => {
      console.error(error);
      alert(error);
    });
  }

  changeState(event: any) {
    if (event.target.value == "null") {
      this.TrangThai = null;
    } else {
      this.TrangThai = event.target.value;
    }
    this.GetDocumentTypeDepartmentId();
  }

  changeLoai(event: any) {
    if (event.target.value == "null") {
      this.Loai = null;
    } else {
      this.Loai = event.target.value;
    }
    this.GetDocumentTypeDepartmentId();
  }

}
