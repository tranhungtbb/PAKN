import { Component, OnInit, ViewChild } from '@angular/core';
import { CatalogService } from '../../../services/catalog.service';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserInfoStorageService } from '../../../commons/user-info-storage.service';
import { PositionObject } from '../../../models/positionObject';
import { DataService } from '../../../services/sharedata.service';
import { saveAs as importedSaveAs } from "file-saver";
import { COMMONS } from 'src/app/commons/commons';

declare var $: any;

@Component({
  selector: 'app-position',
  templateUrl: './position.component.html',
  styleUrls: ['./position.component.css']
})
export class PositionComponent implements OnInit {

  constructor(private _service: CatalogService,
    private _toastr: ToastrService,
    private _fb: FormBuilder,
    private _shareData: DataService,
    private localStorage: UserInfoStorageService) { }

  listPosition = new Array<PositionObject>();
  listTrangThai: any = [
    { value: true, text: "Hiệu lực" },
    { value: false, text: "Hết hiệu lực" }
  ];
  form: FormGroup;
  model: any = new PositionObject();
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
    this._service.positionGetList(request).subscribe(response => {
      if (response.status == 1) {
        this.listPosition = [];
        this.listPosition = response.listPosition;
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

  changeType(event: any) {
    if (event) {
      this.pageIndex = 1;
      this.pageSize = 20;
      this.getList();
    }
  }

  // preCreate() {
  //   this.model = new PositionObject();
  //   this.rebuilForm();
  //   this.submitted = false;
  //   this.title = "Thêm mới chức vụ";
  //   $('#modal').modal('show');
  // }

  // onSave() {
  //   this.submitted = true;
  //   if (this.form.invalid) {
  //     return;
  //   }
  //   if (this.model.stt <= 0 && this.model.stt != null && this.model.stt.toString() != '') {
  //     this.model.stt = 1;
  //   }
  //   if (this.validateModel() == false) {
  //     return;
  //   }
  //   let request = {
  //     CatalogObject: this.model,
  //     Type: 1
  //   };
  //   if (this.model.id == 0 || this.model.id == null) {
  //     this._service.CatalogCreate(request).subscribe(response => {
  //       if (response.status == 1) {
  //         $("#modal").modal("hide");
  //         this._toastr.success("Thêm mới thành công");
  //         this.getList();
  //       } else {
  //         this._toastr.error(response.message);
  //       }
  //     }), error => {
  //       console.error(error);
  //       alert(error);
  //     }
  //   }
  //   else {
  //     this._service.CatalogUpdate(request).subscribe(response => {
  //       if (response.status == 1) {
  //         $("#modal").modal("hide");
  //         this._toastr.success("Cập nhật thành công");
  //         this.getList();
  //       } else {
  //         this._toastr.error(response.message);
  //       }
  //     }), error => {
  //       console.error(error);
  //       alert(error);
  //     }
  //   }
  // }

  // preUpdate(data) {
  //   let request = {
  //     Id: data.id,
  //     Type: 1,
  //   }
  //   this._service.CatalogGetById(request).subscribe(response => {
  //     if (response.status == 1) {
  //       this.rebuilForm();
  //       this.title = "Chỉnh sửa chức vụ";
  //       this.model = response.catalog;
  //       $('#modal').modal('show');
  //     }
  //     else {
  //       this._toastr.error(response.message);
  //     }
  //   }), error => {
  //     console.error(error);
  //     alert(error);
  //   }
  // }

  // onDelete(data) {
  //   let request = {
  //     Type: 1,
  //     Id: data.id
  //   }
  //   this._service.DeleteCatalog(request).subscribe(response => {
  //     if (response.status == 1) {
  //       this._toastr.success("Xóa thành công");
  //       this.getList();
  //     }
  //     else {
  //       this._toastr.error(response.message);
  //     }
  //   }), error => {
  //     console.error(error);
  //   }
  // }

  onUpdateStatus(data) {
    var status = data.trangThai;
    let request = {
      Type: 1,
      Id: data.id
    }
    this._service.UpdateStatus(request).subscribe(res => {
      if (res.status == 1) {
        if (data.trangThai == true) {
          this._toastr.success(COMMONS.UNLOCK_SUCCESS);
        } else {
          this._toastr.success(COMMONS.LOCK_SUCCESS);
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
  exportExcel() {
    let request = {
      type: 3,
      Code: this.code,
      Name: this.ten,
      //Stt: this.stt,
      Status: this.trangThai
    }

    this._service.ExportExcel(request).subscribe(
      response => {
        var today = new Date();
        var dd = String(today.getDate()).padStart(2, '0');
        var mm = String(today.getMonth() + 1).padStart(2, '0');
        var yyyy = today.getFullYear();
        var hh = String(today.getHours()).padStart(2, '0');
        var minute = String(today.getMinutes()).padStart(2, '0');
        var fileName = "DM_ChucVuHanhChinh_" + yyyy + mm + dd+hh+minute;
        var blob = new Blob([response], { type: response.type });
        importedSaveAs(blob, fileName);
      }
    );
  }
  // onEdit() {
  //   $('#modalDetail').modal('hide');
  //   this.preUpdate(this.model);
  // }

  // validateModel() {
  //   this.model.code = this.model.code.trim();
  //   this.model.tenDuLieu = this.model.tenDuLieu.trim();
  //   this.model.moTa = this.model.moTa.trim();
  //   if (this.model.code == '') {
  //     //this._toastr.error("Mã chức vụ không để trống");
  //     $('#code').focus();
  //     return false;
  //   }
  //   if (this.model.tenDuLieu == '') {
  //     //this._toastr.error("Tên chức vụ không để trống");
  //     $('#ten').focus();
  //     return false;
  //   }
  //   return true;
  // }
}
