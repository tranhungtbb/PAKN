import { Component, OnInit, ViewChild } from '@angular/core';
import { CatalogService } from '../../../services/catalog.service';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserInfoStorageService } from '../../../commons/user-info-storage.service';
import { SessionObject } from '../../../models/sessionObject';
import { DataService } from '../../../services/sharedata.service';
import { COMMONS } from 'src/app/commons/commons';

declare var $: any;

@Component({
  selector: 'app-session',
  templateUrl: './session.component.html',
  styleUrls: ['./session.component.css']
})
export class SessionComponent implements OnInit {

  constructor(private _service: CatalogService,
    private _toastr: ToastrService,
    private _fb: FormBuilder,
    private _shareData: DataService,
    private localStorage: UserInfoStorageService) { }

  list: any[] = [];
  listTrangThai: any[] = COMMONS.LIST_TRANG_THAI;
  form: FormGroup;
  model: any = new SessionObject();
  submitted: boolean = false;
  status: boolean;
  title: string = "";
  code: string = "";
  ten: string = "";
  //stt: number = null;
  ngayBd: Date = null;
  ngayKt: Date = null;
  trangThai: boolean = null;
  pageIndex: number = 1;
  pageSize: number = 20;
  totalRecords: number = 0;
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
      code: [this.model.code, Validators.required],
      ten: [this.model.tenDuLieu, Validators.required],
      moTa: [this.model.moTa],
      ngayBd: [this.model.ngayBd, Validators.required],
      ngayKt: [this.model.ngayKt, Validators.required],
      trangThai: [this.model.trangThai, Validators.required],
      stt: [this.model.stt],
    });
  }

  rebuilForm() {
    this.form.reset({
      code: this.model.code,
      ten: this.model.tenDuLieu,
      trangThai: this.model.trangThai,
      ngayBd: this.model.ngayBd,
      ngayKt: this.model.ngayKt,
      moTa: this.model.moTa,
      stt: this.model.stt,
    });
  }

  getList() {
    this.code = this.code.trim();
    this.ten = this.ten.trim();
    let request = {
      Code: this.code,
      TenDuLieu: this.ten,
      //Stt: this.stt,
      NgayBD: this.ngayBd,
      NgayKT: this.ngayKt,
      TrangThai: this.trangThai,
      PageIndex: this.pageIndex,
      PageSize: this.pageSize,
    }
    this._service.KhoaHdndGetList(request).subscribe(response => {
      if (response.status == 1) {
        this.list = [];
        this.list = response.listKhoaHdnd;
        for (var i = 0; i < this.list.length; i++) {
          this.list[i].ngayBd = new Date(this.list[i].ngayBd);
          this.list[i].ngayKt = new Date(this.list[i].ngayKt);
        }
        this.totalRecords = response.totalRecords;
      }
      else {
        this._toastr.error(response.message);
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
      this.getList();
    }
  }

  preCreate() {
    this.model = new SessionObject();
    this.rebuilForm();
    this.submitted = false;
    this.title = "Thêm mới khóa HĐND";
    $('#modal').modal('show');
  }

  validateModel() {
    this.model.code = this.model.code.trim();
    this.model.tenDuLieu = this.model.tenDuLieu.trim();
    this.model.moTa = this.model.moTa.trim();
    if (this.model.code == '') {
      $('#code').focus();
      return false;
    }
    if (this.model.tenDuLieu == '') {
      $('#ten').focus();
      return false;
    }
    this.model.ngayBd.setHours(0, 0, 0, 0);
    if (this.model.ngayBd > this.model.ngayKt) {
      this._toastr.error("Ngày bắt đầu không lớn hơn ngày kết thúc");
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
    let request = {
      CatalogObject: this.model,
      Type: 9
    };
    if (this.model.id == 0 || this.model.id == null) {
      this._service.CatalogCreate(request).subscribe(response => {
        if (response.status == 1) {
          $("#modal").modal("hide");
          this._toastr.success("Thêm mới thành công");
          this.getList();
        } else {
          this._toastr.error(response.message);
        }
      }), error => {
        console.error(error);
        alert(error);
      }
    }
    else {
      this._service.CatalogUpdate(request).subscribe(response => {
        if (response.status == 1) {
          $("#modal").modal("hide");
          this._toastr.success("Cập nhật thành công");
          this.getList();
        } else {
          this._toastr.error(response.message);
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
      Type: 9,
    }
    this._service.CatalogGetById(request).subscribe(response => {
      if (response.status == 1) {
        this.title = "Chỉnh sửa khóa HĐND";
        this.model = response.catalog;
        this.model.ngayBd = new Date(this.model.ngayBd);
        this.model.ngayKt = new Date(this.model.ngayKt);
        this.buildForm();
        $('#modal').modal('show');
      }
      else {
        this._toastr.error(response.message);
      }
    }), error => {
      console.error(error);
      alert(error);
    }
  }

  onDelete(data) {
    let request = {
      Type: 9,
      Id: data.id
    }
    this._service.DeleteCatalog(request).subscribe(response => {
      if (response.status == 1) {
        this._toastr.success("Xóa thành công");
        this.getList();
      }
      else {
        this._toastr.error(response.message);
      }
    }), error => {
      console.error(error);
      alert(error);
    }
  }

  onUpdateStatus(data) {
    var status = data.trangThai;
    let request = {
      Type: 9,
      Id: data.id
    }
    this._service.UpdateStatus(request).subscribe(res => {
      if (res.status == 1) {
        if (status) {
          this._toastr.success("Mở khóa thành công");
        }
        else {
          this._toastr.success("Khóa thành công");
        }
      }
      else {
        this._toastr.error(res.message);
      }
    }), error => {
      console.error(error);
    }
  }

  preView(data) {
    this.model = data;
    this.model.ngayBd = new Date(this.model.ngayBd);
    this.model.ngayKt = new Date(this.model.ngayKt);
    $('#modalDetail').modal('show');
  }

  onEdit() {
    $('#modalDetail').modal('hide');
    this.preUpdate(this.model);
  }

  setTime(type) {
    if (type) {
      if (this.form.controls.ngayBd.status == "INVALID") {
        this.model.ngayBd = new Date;
      }
    }
    else {
      if (this.form.controls.ngayKt.status == "INVALID") {
        if (this.form.controls.ngayBd.status != "INVALID") {
          this.model.ngayKt = this.model.ngayBd;
        }
        else {
          this.model.ngayKt = new Date;
        }
      }
    }
  }
}
