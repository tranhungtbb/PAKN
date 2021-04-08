import { Component, OnInit, ViewChild } from '@angular/core';
// import { Component, OnInit, Input, OnChanges, SimpleChanges, ViewChild } from '@angular/core';
// import { sampleProducts } from './products';
import { UserService } from '../../../../../services/user.service';
import { DepartmentService } from '../../../../../services/department.service';
import { UserInfoStorageService } from '../../../../../commons/user-info-storage.service';
import { AuthenticationService } from '../../../../../services/authentication.service';
// import { ChangePasswordUserObject } from '../../../../../models/changePasswordUserObject';
import { ToastrService } from 'ngx-toastr';
import { MatDialog } from '@angular/material';
import { SystemLogService } from '../../../../../services/systemlog.service';
import { DataService } from '../../../../../services/sharedata.service';

// declare var jquery: any;
declare var $: any;

@Component({
  selector: 'app-users-list',
  templateUrl: './users-list.component.html',
  styleUrls: ['./users-list.component.css']
})
export class UsersListComponent implements OnInit {
  listUsers: any[];
  listDepartments: any[];
  lstTimline: Array<any> = [];
  lstTimlineMore: Array<any> = [];
  usernameselected: string;
  newPassword: string = '';
  user: any = {};
  constructor(
    private userService: UserService,
    private depService: DepartmentService,
    private userStorageService: UserInfoStorageService,
    private authenService: AuthenticationService,
    private toastr: ToastrService,
    public dialog: MatDialog,
    private _shareData: DataService,
    private _systemlogService: SystemLogService
  ) {
  }
  lstDropDown = [{ 'value': 1, 'Text': 'Thành công' }, { 'value': 0, 'Text': 'Thất bại' }]
  defaultItem2 = { Text: "--Chọn trạng thái--", value: -1 };
  noidung: string = "";
  noixayraloi: string = "";
  trangthai: number = -1;
  _fromdate: any = "";
  _todate: any = "";
  total: number = 0;

  searchObject = {
    tenNguoiDung: "",
    tenDangNhap: "",
    departmentId: null,
    pageIndex: 1,
    pageSize: 20,
    totalRecords: 0,
  }
  @ViewChild("table", {static: false}) table: any;
  @ViewChild("table2", {static: false}) table2: any;

  ngOnInit(): void {
    this.getDepartment();
  }

  ngAfterViewInit() {
    this._shareData.seteventnotificationDropdown();
  }

  getDepartment() {
    this.depService.getDepartmentByUnit({
      ParentUnitId: 1
    }).subscribe(data => {
      if (data.status == 1) {
        this.listDepartments = data.departments;
      }
    });
  }

  pageindexTimeline: number = 1;
  pageSizeTimeline: number = 20;


  public buttonCount = 5;
  public info = true;
  public type: 'numeric' | 'input' = 'numeric';
  listState: any[] = [{ text: "Tất cả", value: null }, { value: true, text: "Đang sử dụng" }, { value: false, text: "Không sử dụng" }];
  public defaultItem: { text: string, value: number } = { text: "Tất cả", value: null };

  lstDayhis: Array<timlineUser> = [];

  userTimeLineGrid: Array<any> = [];

  public onPageChange(event: any): void {
    if (event) {
      this.searchObject.pageSize = event.rows;
      this.searchObject.pageIndex = (event.first / event.rows) + 1;
      this.getUserByDepartment();
    }
  }

  public dataStateChange(): void {
    this.searchObject.pageIndex = 1;
    this.table.first = 0;
    this.getUserByDepartment();
  }

  public getUserByDepartment() {
    this.searchObject.tenDangNhap = this.searchObject.tenDangNhap.trim();
    this.searchObject.tenNguoiDung = this.searchObject.tenNguoiDung.trim();
    var request = {
      DepartmentId: this.searchObject.departmentId,
      Name: this.searchObject.tenNguoiDung,
      LoginName: this.searchObject.tenDangNhap,
      PageSize: this.searchObject.pageSize,
      PageIndex: this.searchObject.pageIndex,
    }
    this.userService.getUserByDepartment(request).subscribe(data => {
      if (data.status == 1) {
        this.listUsers = [];
        this.listUsers = data.users;
        this.searchObject.totalRecords = data.totalRecords;
      }
    });
  }

  onRestoreAccount(name: string): void {
    var param = {
      UserName: name,
      Password: ''
    }
    var request = {
      Userlg: param
    }
    this.authenService.restoreAccount(param).subscribe(data => {
      if (data.status == 1) {
        this.getUserByDepartment();
        this.toastr.success("Khôi phục thành công");
      } else {
        this.toastr.error(data.message);
      }
    });
  }

  onChangePass(type, data): void {
    if (type == 1) {
      this.user = {};
      this.user = data;
      this.newPassword = "";
      $("#modal-change-password").modal("show");
    }
    else {
      this.newPassword = this.newPassword.trim();
      if (this.newPassword == "") {
        this.toastr.error("Mật khẩu không được để trống");
        return;
      }
      this.authenService.chagepassword({
        OldPassword: "",
        NewPassword: this.newPassword,
        ConfirmPassword: this.newPassword,
        IsAdminChanged: true,
        LoginId: this.user.loginId,
        UserChange: this.user.tenDangNhap,
      }).subscribe(data => {
        if (data.status == 1) {
          this.toastr.success("Đổi mật khẩu thành công");
          $("#modal-change-password").modal("hide");
        } else {
          this.toastr.error(data.message);
        }
      }), err => {
        console.error(err);
      }
    }
  }

  onLock(id: number): void {
    this.userService.lockUser({ Id: id, IsActive: false }).subscribe(data => {
      if (data.status == 1) {
        this.toastr.success("Khóa tài khoản thành công");
        this.getUserByDepartment();
      } else {
        this.toastr.error("Khóa tài khoản thất bại");
      }
    });
  }

  onUnLock(id: number): void {
    this.userService.lockUser({ Id: id, IsActive: true }).subscribe(data => {
      if (data.status == 1) {
        this.toastr.success("Mở khóa tài khoản thành công");
        this.getUserByDepartment();
      } else {
        this.toastr.error("Mở khóa tài khoản thất bại");
      }
    });
  }

  onDelete(id: number): void {
    this.userService.delete({ Id: id }).subscribe(data => {
      if (data.status == 1) {
        this.toastr.success("Xóa tài khoản thành công");
        this.getUserByDepartment();
      } else if (data.status == 3) {
        this.toastr.error(data.message);
      }
      else {
        this.toastr.error("Xóa tài khoản thất bại");
      }
    });
  }

  // Hàm Convert Datetime sang dạng String V
  parseDateV = function (value: any) {
    if (value) {
      var currentTime = new Date(value);
      var month = (currentTime.getMonth() + 1).toString();
      var day = currentTime.getDate().toString();
      var year = currentTime.getFullYear();
      if (parseInt(day) < 10) { day = "0" + day; }
      if (parseInt(month) < 10) { month = "0" + month; }
      var date = day + "/" + month + "/" + year;
      return date;
    }
    return null;
  }
  // Show Lịch sử
  ShowHistories(username: string) {
    this.noidung = "";
    this.noixayraloi = "";
    if (this.lstDayhis.length == 0) {
      var CurentDate = new Date();
      for (var i = 0; i <= 6; i++) {
        if (i == 0) {
          this.lstDayhis.push({ time: this.parseDateV(CurentDate), history: [] });
        } else {
          this.lstDayhis.push({ time: this.parseDateV(CurentDate.setDate(CurentDate.getDate() - 1)), history: [] });
        }
      };
    } else {
      for (var j = 0; j < this.lstDayhis.length; j++) {
        this.lstDayhis[j].history = [];
        if (j != 0) {
          $("#today" + j).removeClass("in");
        }
      }
    }

    this.usernameselected = username;
    this.lstTimline = [];
    this.lstTimlineMore = [];
    this._systemlogService.getTimeLineData(username, this.lstDayhis[0].time).subscribe(data => {
      this.lstDayhis[0].history = data.timeline;
      this._fromdate = "";
      this._todate = "";
      $("#modalHistories").modal("show");
    })
    this.pageindexTimeline = 1;
    this.pageSizeTimeline = 20;
    this.userSearchInTimeLine();
  }

  LoadTimeLine(data: any, index: number) {
    if (data.history.length == 0) {
      this._systemlogService.getTimeLineData(this.usernameselected, data.time).subscribe(data => {
        this.lstDayhis[index].history = data.timeline;
      })
    }
  }

  LoadMoreTime(time: string) {
    if (this.lstTimlineMore.length == 0) {
      this._systemlogService.getTimeLineData(this.usernameselected, time).subscribe(data => {
        this.lstTimlineMore = data.timeline;
      })
    }
  }

  public SetClassTimeLine(data: any) {
    if (data.loaiNhatKy) {
      switch (data.tenHanhDong) {
        case "Add":
          return {
            'fa-plus': true,
            'bg-green': true
          };
        case "Edit":
          return {
            'fa-pencil': true,
            'bg-yellow': true
          };
        case "Delete":
          return {
            'fa-trash': true,
            'bg-red': true
          };
        case "Publish":
          return {
            'fa-external-link': true,
            'bg-yellow': true
          };
        case "Login":
          return {
            'fa-sign-in': true,
            'bg-aqua': true
          };
        case "Send":
          return {
            'fa-paper-plane': true,
            'bg-green': true
          };
        case "Recover":
          return {
            'fa-reply': true,
            'bg-yellow': true
          };
        case "Deny":
          return {
            'fa-ban': true,
            'bg-red': true
          };
        case "Approved":
          return {
            'fa-check-square': true,
            'bg-green': true
          };
        case "ChangePassWord":
          return {
            'fa-key': true,
            'bg-yellow': true
          };
        case "LogOut":
          return {
            'fa-power-off': true,
            'bg-red': true
          };
        case "Delegacy":
          return {
            'fa-legal': true,
            'bg-purple': true
          };
        case "Forward":
          return {
            'fa-paper-plane-o': true,
            'bg-green': true
          };
        case "VaoSo":
          return {
            'fa-check-circle': true,
            'bg-green': true
          };
        case "Support":
          return {
            'fa-check-square': true,
            'bg-yellow': true
          };
        case "Wait":
          return {
            'fa-external-link': true,
            'bg-yellow': true
          };
        case "ProvidedNumber":
          return {
            'fa-external-link': true,
            'bg-blue': true
          };
        case "Remove":
          return {
            'fa-trash': true,
            'bg-red': true
          };
        case "Accept":
          return {
            'fa-check-square': true,
            'bg-yellow': true
          };
        case "Signed":
          return {
            'fa-check-square': true,
            'bg-yellow': true
          };
        case "Reply":
          return {
            'fa-reply': true,
            'bg-green': true
          };
        case "EndProcess":
          return {
            'fa-check': true,
            'bg-blue': true
          };
        case "Lock":
          return {
            'fa-lock': true,
            'bg-purple': true
          };
        case "Unlock":
          return {
            'fa-unlock': true,
            'bg-aqua': true
          };
        case "Keep":
          return {
            'fa-check-circle-o': true,
            'bg-aqua': true
          };
        case "CanceKeep":
          return {
            'fa-trash': true,
            'bg-yellow': true
          };
      }
    } else {
      switch (data.tenHanhDong) {
        case "Add":
          return {
            'fa-plus': true,
            'bg-green': true
          };
        case "Edit":
          return {
            'fa-pencil': true,
            'bg-yellow': true
          };
        case "Delete":
          return {
            'fa-trash': true,
            'bg-red': true
          };
        case "Publish":
          return {
            'fa-external-link': true,
            'bg-yellow': true
          };
        case "Login":
          return {
            'fa-sign-in': true,
            'bg-aqua': true
          };
        case "Send":
          return {
            'fa-paper-plane': true,
            'bg-green': true
          };
        case "Recover":
          return {
            'fa-reply': true,
            'bg-yellow': true
          };
        case "Deny":
          return {
            'fa-ban': true,
            'bg-red': true
          };
        case "Approved":
          return {
            'fa-check-square': true,
            'bg-green': true
          };
        case "ChangePassWord":
          return {
            'fa-key': true,
            'bg-yellow': true
          };
        case "LogOut":
          return {
            'fa-power-off': true,
            'bg-red': true
          };
        case "Delegacy":
          return {
            'fa-legal': true,
            'bg-purple': true
          };
        case "Forward":
          return {
            'fa-paper-plane-o': true,
            'bg-green': true
          };
        case "VaoSo":
          return {
            'fa-check-circle': true,
            'bg-green': true
          };
        case "Support":
          return {
            'fa-check-square': true,
            'bg-yellow': true
          };
        case "Move":
          return {
            'fa-balance-scale': true,
            'bg-purple ': true
          };
        case "Wait":
          return {
            'fa-external-link': true,
            'bg-warning': true
          };
        case "ProvidedNumber":
          return {
            'fa-external-link': true,
            'bg-blue': true
          };
        case "Remove":
          return {
            'fa-trash': true,
            'bg-red': true
          };
        case "Accept":
          return {
            'fa-check-square': true,
            'bg-yellow': true
          };
        case "Signed":
          return {
            'fa-check-square': true,
            'bg-yellow': true
          };
        case "Reply":
          return {
            'fa-reply': true,
            'bg-green': true
          };
        case "EndProcess":
          return {
            'fa-check': true,
            'bg-blue': true
          };
        case "Lock":
          return {
            'fa-lock': true,
            'bg-purple': true
          };
        case "Unlock":
          return {
            'fa-unlock': true,
            'bg-aqua': true
          };
        case "Keep":
          return {
            'fa-check-circle-o': true,
            'bg-aqua': true
          };
        case "CanceKeep":
          return {
            'fa-trash': true,
            'bg-yellow': true
          };
      }
    }
  }

  userSearchInTimeLine() {
    var Request = {
      username: this.usernameselected,
      fromdate: this._fromdate,
      todate: this._todate,
      noidung: this.noidung,
      noixayraloi: this.noixayraloi,
      pageindex: this.pageindexTimeline,
      pagesize: this.pageSizeTimeline,
      trangthai: this.trangthai
    }
    this._systemlogService.timlineusersearch(Request).subscribe(data => {
      if (data) {
        this.userTimeLineGrid = [];
        this.userTimeLineGrid = data.listTimeLine;
        //for (var i = 0; i < data.listTimeLine.length; i++) {
        //  data.listTimeLine[i].ngayHanhDong = new Date(Date.parse(data.listTimeLine[i].ngayHanhDong));
        //}
        if (this.userTimeLineGrid.length > 0) {
          this.total = this.userTimeLineGrid[0].tongso;
        } else {
          this.total = 0;
        }
      }
    })
  }

  onSearch() {
    this.pageindexTimeline = 1;
    this.table2.first = 0;
    this.userSearchInTimeLine();
  }

  public onPageChangeTimeLine(event): void {
    if (event) {
      this.pageSizeTimeline = event.rows;
      this.pageindexTimeline = (event.first / event.rows) + 1;
      this.userSearchInTimeLine();
    }
  }

  UpdateStatus(nguoiDung) {
    const kickHoat = nguoiDung.kichHoat;
    var request = {
      Ma: nguoiDung.ma
    }
    this.userService.UserUpdateStatus(request).subscribe(res => {
      if (res.status == 1) {
        if (kickHoat) {
          this.toastr.success('Kích hoạt thành công');
        }
        else {
          this.toastr.success('Khóa thành công');
        }
      }
      else {
        this.toastr.error(res.message);
      }
    }), err => {
      console.error(err);
    }
  }

  setDate(type, e) {
    if (type == 1) {
      if (e.currentTarget.value == "Invalid date")
        this._fromdate = null;
    }
    else {
      if (e.currentTarget.value == "Invalid date")
        this._todate = null;
    }
  }
}

interface timlineUser {
  time: string;
  history: Array<any>;
}
