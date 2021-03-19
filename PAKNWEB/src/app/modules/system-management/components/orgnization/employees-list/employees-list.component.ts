// import { Component, OnInit, Input, ChangeDetectionStrategy, ChangeDetectorRef, OnChanges, SimpleChanges, Output, EventEmitter } from '@angular/core';
import { Component, OnInit, Input, SimpleChanges, Output, EventEmitter } from '@angular/core';
// import { Observable, Observer } from 'rxjs';
import { DepartmentService } from '../../../../../services/department.service';
// import { forEach } from '@angular/router/src/utils/collection';
import { DepartmentTree } from '../../../../../models/departmentTree';
import { MenuPassingObject } from '../menuPassingObject';
import { ToastrService } from 'ngx-toastr';
import { DataService } from '../../../../../services/sharedata.service';

// declare var jquery: any;
declare var $: any;

@Component({
  selector: 'app-employees-list',
  templateUrl: './employees-list.component.html',
  styleUrls: ['./employees-list.component.css']
})
export class EmployeesListComponent implements OnInit {

  userlst: any = {};

  lstData: any = [];
  NameSearch: string = "";
  UserNameSearch: string = "";
  pageSize: number = 20;
  pageIndex: number = 1;
  depId: number = -1;
  request: any = {};
  userdelete: any = [];
  lstPage: any = [];
  pageCount: number = 0;
  total: number = 0;
  idDaiDien: number = 0;
  trangThai: number = -1;
  isenable: boolean = false;
  public modeselected: string = 'multiple';

  listState: any[] = [{ text: "Tất cả", value: -1 }, { value: 1, text: "Đang sử dụng" }, { value: 0, text: "Không sử dụng" }];

  defaulItem: any = { 'value': -1, 'text': '--Chọn trạng thái--' };

  public buttonCount = 5;
  public info = true;
  public type: 'numeric' | 'input' = 'numeric';
  public pageSizes = true;
  public previousNext = true;

  public filterusername = false;
  public filtername = false;
  @Input() departmentInfo: DepartmentTree;

  object: MenuPassingObject = new MenuPassingObject();
  @Output() onMaincheck = new EventEmitter<MenuPassingObject>();

  constructor(private service: DepartmentService,
    private _notifi: ToastrService,
    private _shareData: DataService) { }

  ngOnInit() {
    this.userdelete = [];
    this.getlistUser();
  }

  ngAfterViewInit() {
    this._shareData.seteventnotificationDropdown();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (this.departmentInfo != undefined) {
      if (this.depId != this.departmentInfo.ma) {
        $('#deleteSelected').prop('checked', false);
        this.userdelete = [];

        $("#tennguoidung").val('');
        $("#tendangnhap").val('');

      }

      this.depId = this.departmentInfo.ma;

      if (this.departmentInfo.isDonVi) {
        this.isenable = false;
        this.modeselected = 'single';
      } else {
        this.isenable = true;
        this.modeselected = 'multiple';
      }
    }
    this.getlistUser();
  }
  getlistUser() {

    this.request.DepartmentId = this.depId;
    this.request.Tennguoidung = this.NameSearch.trim();
    this.request.Tendangnhap = this.UserNameSearch.trim();
    this.request.Pagesize = this.pageSize;
    this.request.PageIndex = this.pageIndex;
    this.request.TrangThai = this.trangThai;

    this.service.DepartmentGetlstUserbyId(this.request).subscribe(data => {
      this.lstData = data.gridDepartmentUser;
      if (data.gridDepartmentUser.length > 0) {
        this.total = data.gridDepartmentUser[0].tongso;
      } else this.total = 0;

      //this.userlst = {
      //  data: data.gridDepartmentUser,
      //  total: this.total
      //}
    }, error => {
      alert(error);
    });
  }
  mainUserDepartment() {

    this.object.type = 3;
    this.object.data = this.userdelete;
    this.onMaincheck.emit(this.object);
  }
  deleteDepartmentUser() {
    let LstId: Array<any> = [];
    LstId = this.getListMaUser(this.userdelete);
    var request = {
      Request: {
        lstId: LstId.join(),
        DepId: this.depId
      }
    }
    this.service.DeleteDepartmentUser(request).subscribe(data => {

      if (data.status == 1) {
        $("#modal-confirm-delete").modal('hide');
        this._notifi.success("Xóa thành công");
        this.userdelete = [];
        $('#deleteSelected').prop('checked', false);
        this.mainUserDepartment();
        this.getlistUser();
      } else if (data.status == 2) {
        $("#modal-confirm-delete").modal('hide');
        this._notifi.error(data.message);
      }

    }, error => {
      this._notifi.error(error);
    })
  }
  confirmDaiDien(id: number) {
    this.idDaiDien = id;
    $("#modal-confirm-setperson").modal('show')
  }
  SetNguoiDaiDien() {
    var request = {
      DepId: this.depId,
      Representative: this.idDaiDien
    }
    this.service.UpdateRepresentative(request).subscribe(data => {
      if (data) {
        $("#modal-confirm-setperson").modal('hide');
        this._notifi.success("Cập nhật thành công");
        this.getlistUser();
      } else {
        $("#modal-confirm-setperson").modal('hide');
        this._notifi.error("Người dùng đã bị khóa");
      }

    }, error => {
      this._notifi.error(error);
    })
  }
  public onSelectedKeysChange() {
    this.object.type = 3;
    this.object.data = this.getListMaUser(this.userdelete);

    this.onMaincheck.emit(this.object);
  }
  public onSelectedHeaderChange(event) {
    if (event.checked) {
      this.object.type = 3;
      this.object.data = this.getListMaUser(this.userdelete);
      this.onMaincheck.emit(this.object);
    } else {

      this.object.type = 3;
      this.object.data = [];
      this.onMaincheck.emit(this.object);
    }
  }
  myMethod(data: any) {
    if (data.keyCode == 13) {
      this.getlistUser();
    }
  }
  public onPageChange(event: any): void {

    this.pageSize = event.rows;
    this.pageIndex = (event.first / event.rows) + 1;

    this.getlistUser();
  }
  changeState(event: any) {
    this.trangThai = event.value.value;
    this.getlistUser();
  }
  getListMaUser(data) {
    const result = data;

    if (!result && !result.length) {
      return null;
    }
    return result.map(obj => obj.ma)
  }
}
