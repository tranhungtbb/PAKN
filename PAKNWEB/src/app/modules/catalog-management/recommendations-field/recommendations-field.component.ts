import { Component, OnInit, ViewChild } from '@angular/core';
import { CatalogService } from '../../../services/catalog.service';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserInfoStorageService } from '../../../commons/user-info-storage.service';
import { RecommendationsFieldObject } from '../../../models/recommendationsFieldObject';
import { DataService } from '../../../services/sharedata.service';
import { COMMONS } from 'src/app/commons/commons';

declare var $: any;

@Component({
  selector: 'app-recommendations-field',
  templateUrl: './recommendations-field.component.html',
  styleUrls: ['./recommendations-field.component.css']
})
export class RecommendationsFieldComponent implements OnInit {

  constructor(private _service: CatalogService,
    private _toastr: ToastrService,
    private _fb: FormBuilder,
    private _shareData: DataService,
    private localStorage: UserInfoStorageService) { }

  listRecommendationsField = new Array<RecommendationsFieldObject>();
  form: FormGroup;
  model: any = new RecommendationsFieldObject();
  submitted: boolean = false;
  status: boolean;
  title: string = "";
  code: string = "";
  ten: string = "";
  //stt: number = null;
  trangThai: boolean = null;
  pageIndex: number = 1;
  pageSize: number = 20;
  totalRecords: number = 0;

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
      code: [this.model.code, Validators.required],
      ten: [this.model.tenDuLieu, Validators.required],
      moTa: [this.model.moTa],
      trangThai: [this.model.trangThai, Validators.required],
      stt: [this.model.stt],
    });
  }

  rebuilForm() {
    this.form.reset({
      code: this.model.code,
      ten: this.model.tenDuLieu,
      trangThai: this.model.trangThai,
      moTa: this.model.moTa,
      stt: this.model.stt,
    });
  }

  getList() {
    this.code = this.code.trim();
    this.ten = this.ten.trim();
    let request = {
      Code: this.code,
      Ten: this.ten,
      //Stt: this.stt,
      TrangThai: this.trangThai,
      PageIndex: this.pageIndex,
      PageSize: this.pageSize,
    }
    this._service.recommendationsFieldGetList(request).subscribe(response => {
      if (response.status == 1) {
        this.listRecommendationsField = [];
        this.listRecommendationsField = response.listRecommendationsField;
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
      this.pageSize = 20;
      this.getList();
    }
  }

  preCreate() {
    this.model = new RecommendationsFieldObject();
    this.rebuilForm();
    this.submitted = false;
    this.title = "Thêm mới lĩnh vực";
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
      Type: 7
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
      Type: 7,
    }
    this._service.CatalogGetById(request).subscribe(response => {
      if (response.status == 1) {
        this.rebuilForm();
        this.title = "Chỉnh sửa lĩnh vực";
        this.model = response.catalog;
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
      Type: 7,
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
    }
  }

  onUpdateStatus(data) {
    var status = data.trangThai;
    let request = {
      Type: 7,
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
    $('#modalDetail').modal('show');
  }

  onEdit() {
    $('#modalDetail').modal('hide');
    this.preUpdate(this.model);
  }

}
