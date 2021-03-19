import { Component, OnInit, ViewChild } from '@angular/core';
import { DepartmentGroupService } from '../../../services/department-group.service';
import { ToastrService } from 'ngx-toastr';
import { DepartmentGroupObject } from '../../../models/departmentGroupObject';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { UserInfoStorageService } from '../../../commons/user-info-storage.service';
import { CatalogService } from '../../../services/catalog.service';
import { DataService } from '../../../services/sharedata.service';

import { COMMONS } from 'src/app/commons/commons';
declare var $: any;

@Component({
  selector: 'app-department-group',
  templateUrl: './department-group.component.html',
  styleUrls: ['./department-group.component.css']
})
export class DepartmentGroupComponent implements OnInit {

  constructor(private service: DepartmentGroupService,
    private toastr: ToastrService,
    private _fb: FormBuilder,
    private catalogService: CatalogService,
    private _shareData: DataService,
    private localStorage: UserInfoStorageService) { }

  listDepartmentGroup = new Array<DepartmentGroupObject>();
  form: FormGroup;
  model: any = new DepartmentGroupObject();
  departmentGroupInfo: DepartmentGroupObject;
  submitted: boolean = false;
  status: boolean;
  title: string = "";
  ma: string = "";
  ten: string = "";
  //stt: number = null;
  trangThai: boolean = null;
  pageIndex: number = 1;
  pageSize: number = 20;
  totalRecords: number = 0;
  catalogObject: any = {};

  listTrangThai = COMMONS.LIST_TRANG_THAI;
  @ViewChild("table", {static: false}) table: any;
  ngOnInit() {
    this.buildForm();
  }

  ngAfterViewInit() {
    this._shareData.seteventnotificationDropdown();
  }

  get f() { return this.form.controls; }

  buildForm() {
    this.form = this._fb.group({
      ma: [this.model.ma, Validators.required],
      ten: [this.model.ten, Validators.required],
      moTa: [this.model.moTa],
      trangThai: [this.model.trangThai, Validators.required],
      stt: [this.model.stt],
    });
  }

  rebuilForm() {
    this.form.reset({
      ma: this.model.ma,
      ten: this.model.ten,
      trangThai: this.model.trangThai,
      moTa: this.model.moTa,
      stt: this.model.stt,
    });
  }

  getList() {
    this.ma = this.ma.trim();
    this.ten = this.ten.trim();
    let request = {
      Ma: this.ma,
      Ten: this.ten,
      //Stt: this.stt,
      TrangThai: this.trangThai,
      PageIndex: this.pageIndex,
      PageSize: this.pageSize,
    }
    this.service.getList(request).subscribe(response => {
      if (response.status == 1) {
        this.listDepartmentGroup = [];
        this.listDepartmentGroup = response.listDepartmentGroup;
        this.totalRecords = response.totalRecords;
      }
      else {
        this.toastr.error(response.message);
      }
    }), error => {
      console.log(error);
      alert(error);
    }
  }

  onPageChange(event: any) {
    this.pageSize = event.rows;
    this.pageIndex = (event.first / event.rows) + 1;
    this.getList();
  }

  dataStateChange() {
    this.pageIndex = 1;
    this.table.first = 0;
    this.getList();
  }

  changeState(event: any) {
    if (event) {
      if (event.target.value == "null") {
        this.trangThai = null;
      }
      else {
        this.trangThai = event.target.value;
      }
      this.pageIndex = 1;
      this.pageSize = 20;
      this.getList();
    }
  }

  preCreate() {
    this.model = new DepartmentGroupObject();
    this.rebuilForm();
    this.submitted = false;
    this.title = "Thêm mới nhóm sở ngành";
    $('#modal').modal('show');
  }

  validateModel() {
    this.model.ma = this.model.ma.trim();
    this.model.ten = this.model.ten.trim();
    this.model.moTa = this.model.moTa.trim();
    if (this.model.ma == '') {
      //this.toastr.error("Mã nhóm sở ngành không để trống");
      $('#code').focus();
      return false;
    }
    if (this.model.ten == '') {
      //this.toastr.error("Tên nhóm sở ngành không để trống");
      $('#ten').focus();
      return false;
    }
    return true;
  }

  onSave() {
    this.submitted = true;
    if (this.form.invalid) {
      return;
    }
    if (this.model.stt != null && this.model.stt <= 0 && this.model.stt.toString() != '') {
      this.model.stt = 1;
    }
    if (this.validateModel() == false) {
      return;
    }
    this.catalogObject = {};
    this.catalogObject = this.model;
    this.catalogObject.TenDuLieu = this.model.ten;
    this.catalogObject.Code = this.model.ma;
    let request = {
      CatalogObject: this.catalogObject,
      Type: 2
    };
    if (this.model.id == 0 || this.model.id == null) {
      this.catalogService.CatalogCreate(request).subscribe(response => {
        if (response.status == 1) {
          $("#modal").modal("hide");
          this.toastr.success("Thêm mới thành công");
          this.getList();
        } else {
          this.toastr.error(response.message);
        }
      }), error => {
        console.error(error);
        alert(error);
      }
    }
    else {
      this.catalogService.CatalogUpdate(request).subscribe(response => {
        if (response.status == 1) {
          $("#modal").modal("hide");
          this.toastr.success("Cập nhật thành công");
          this.getList();
        } else {
          this.toastr.error(response.message);
        }
      }), error => {
        console.error(error);
        alert(error);
      }
    }
  }

  preUpdate(data) {
    let request = {
      Id: data.id,
      Type: 2,
    }
    this.catalogService.CatalogGetById(request).subscribe(response => {
      if (response.status == 1) {
        this.rebuilForm();
        this.title = "Chỉnh sửa nhóm sở ngành";
        this.model = response.catalog;
        $('#modal').modal('show');
      }
      else {
        this.toastr.error(response.message);
      }
    }), error => {
      console.error(error);
      alert(error);
    }
  }

  onDelete(data) {
    let request = {
      Type: 2,
      Id: data.id
    }
    this.catalogService.DeleteCatalog(request).subscribe(response => {
      if (response.status == 1) {
        this.toastr.success("Xóa thành công");
        this.getList();
      }
      else {
        this.toastr.error(response.message);
      }
    }), error => {
      console.error(error);
    }
  }

  onUpdateStatus(data) {
    var status = data.trangThai;
    let request = {
      Type: 2,
      Id: data.id
    }
    this.catalogService.UpdateStatus(request).subscribe(res => {
      if (res.status == 1) {
        if (status) {
          this.toastr.success("Mở khóa thành công");
        }
        else {
          this.toastr.success("Khóa thành công");
        }
      }
      else {
        this.toastr.error(res.message);
      }
    }), error => {
      console.error(error);
    }
  }

  preView(data) {
    this.model = data;
    $('#modalDetail').modal('show');
  }

  onEdit() {
    $('#modalDetail').modal('hide');
    this.preUpdate(this.model);
  }

}
