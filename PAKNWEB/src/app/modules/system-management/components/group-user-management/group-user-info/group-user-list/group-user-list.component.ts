import { Component, OnInit, Renderer2, Inject, Input, OnChanges, SimpleChanges, ViewChild } from '@angular/core';
import { DatePipe,DOCUMENT } from '@angular/common';
import { GroupUserService } from '../../../../../../services/groupuser.service';
import { ActivatedRoute, Router } from '@angular/router';
// import { DOCUMENT } from '@angular/platform-browser';
import { GroupUserObject } from '../../../../../../models/groupUserObject';
import { RequestGroupUserModel } from '../../../../../../models/requestGroupUserModel';
// import { Observable } from 'rxjs/Observable';
import { MenuPassingObject } from '../../../orgnization/menuPassingObject';
import { DepartmentTree } from '../../../../../../models/departmentTree';
import { ToastrService } from 'ngx-toastr';
import { UserGroupUser } from '../../../../../../models/userGroupUser';
import { UserService } from '../../../../../../services/user.service';
// import { switchMap, debounceTime, tap, finalize } from 'rxjs/operators';
import { UserObject } from '../../../../../../models/UserObject';
import { UserInfoStorageService } from '../../../../../../commons/user-info-storage.service';
import { DataService } from '../../../../../../services/sharedata.service';

// // declare var jquery: any;
declare var $: any;
// declare var data: any;

@Component({
  selector: 'app-group-user-list',
  templateUrl: './group-user-list.component.html',
  styleUrls: ['./group-user-list.component.css'],
  providers: [DatePipe],
})
export class GroupUserListComponent implements OnInit, OnChanges {

  Id: number;
  public groupuserlst: GroupUser[];
  public groupUser: GroupUserObject;
  public userGroupUser: UserGroupUser;
  public lstUserGroupUser: UserGroupUser[];
  public item: any;
  listUserToAdd: Array<any> = [];
  lstTrangThai: Array<any> = [
    { value: true, text: "Đang sử dụng" },
    { value: false, text: "Không sử dụng" }
  ];

  groupUserId: number = 0;
  userDel: GroupUser = new GroupUser;
  filteredUsers: UserObject[] = [];
  isLoading = false;
  userIdDel: number = 0;
  public totalRecords = 0;
  isFilterable: boolean = false;

  @ViewChild('table', {static: false}) table: any;
  @Input() departmentInfo: DepartmentTree;
  @Input() departmentId: number;
  object: MenuPassingObject = new MenuPassingObject();

  model: RequestGroupUserModel = {
    userLoginId: '',
    trangThai: 0,
    maDonVi: 0,
  };

  errorMessage: any;
  keyword: string = '';
  pageindex: number = 1;
  pagesize: number = 20;

  pageCount: number = 0;
  total: number = 0;
  lstPage: number[] = [];
  TrangThai: number = null;
  userLoginId: string = '';

  public buttonCount = 5;
  public info = true;
  public type: 'numeric' | 'input' = 'numeric';
  public pageSizes = true;
  public previousNext = true;

  tenNhom: string = "";
  maNhom: string = "";

  constructor(private _avRoute: ActivatedRoute,
    private _groupUserService: GroupUserService,
    private myPipe: DatePipe,
    private _renderer2: Renderer2,
    @Inject(DOCUMENT) private _document,
    private _router: Router,
    private _toast: ToastrService,
    private _userService: UserService,
    private _shareData: DataService,
    private localStorage: UserInfoStorageService) {
  }

  ngOnInit() {
    this.getGroupUserByDepartment();
    $('.select2').select2({
      width: 'resolve',
      language: {
        noResults: function (params) {
          return "không tìm thấy kết quả.";
        }
      }
    });
  }

  ngAfterViewInit() {
    this._shareData.seteventnotificationDropdown();
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.getGroupUserByDepartment();
  }

  filterCount: number = 0;

  public dataStateChange(): void {
    this.pageindex = 1;
    this.table.first = 0;
    this.getGroupUserByDepartment();
  }

  public getGroupUserByDepartment() {
    this.tenNhom = this.tenNhom.trim();
    this.maNhom = this.maNhom.trim();
    var pageSize = this.pagesize;
    var pageIndex = this.pageindex;
    this._groupUserService.getListUserByUnitIdAndPage({
      DepartmentId: this.departmentId,
      PageSize: pageSize,
      PageIndex: pageIndex,
      GroupName: this.tenNhom,
      GroupCode: this.maNhom,
      UserLoginId: 1,
      TrangThai: this.TrangThai
    }).subscribe(data => {
      if (data.groupUsers.length > 0) {
        for (var i = 0; i < data.groupUsers.length; i++) {
          data.groupUsers[i].ngayTao = new Date(Date.parse(data.groupUsers[i].ngayTao))
        }
      }
      this.groupuserlst = data.groupUsers;
      this.totalRecords = data.totalRecords;
    });
  }

  getListUserByGroupId(groupid: number) {
    this._userService.getUserByGroupId(groupid).subscribe((data) => {
      if (data.status === 1) {
        this.lstUserGroupUser = data.userModel;
        this.groupUserId = groupid;
      } else if (data.status === 2) {
      }
    }, error => {
      console.error(error);
    });
    let request: any = {
      DepartmentId: this.departmentId,
      GroupUserId: groupid,
      Filter: ''
    };
    this.listUserToAdd = [];
    this._groupUserService.getListUserByUnitIdAndFilter(request).subscribe((data) => {

      this.listUserToAdd = data.listUsers;
    }, error => {
      console.error(error);
    });
  }

  preDelete(groupid: number) {
    var request = {
      GroupUserId: groupid
    }
    this._groupUserService.DeleteGroupUser(request).subscribe((data) => {
      if (data.status === 1) {
        this.getGroupUserByDepartment();
        this._toast.success("Xóa thành công!");
      } else if (data.status === 2) {
        this._toast.error(data.message);
      }
    }, error => {
      console.error(error);
    });
  }

  confirmDeleteUserFromGroup(id: number) {
    this.userIdDel = id;
    $("#confirmModal").modal('show');
  }

  preDeleteUserToGroup() {
    var request = {
      DelUserToGroup: {
        UserId: this.userIdDel,
        GroupUserId: this.groupUserId
      }
    }
    this._groupUserService.delUserFromGroupUser(request).subscribe((data) => {
      if (data.status === 1) {
        this.getListUserByGroupId(this.groupUserId);
        this._toast.success("Xóa thành công");
        this.getGroupUserByDepartment();
      } else if (data.status === 2) {
        this._toast.error(data.message);
      }
    }, error => {
      console.error(error);
    });
    $("#confirmModal").modal('hide');
  }

  addUserToGroup() {
    let lstuId: Array<any> = $("#addusertogroup").val();
    if (lstuId.length == 0) {
      this._toast.error('Bạn chưa chọn người dùng!');
      return;
    }
    lstuId.join();
    var request = {
      AddUserToGroup: {
        ListId: lstuId.join(),
        GroupUserId: this.groupUserId
      }
    }
    this._groupUserService.addListUserToGroupUser(request).subscribe((data) => {
      if (data.status === 1) {
        this._toast.success("Thêm người dùng thành công");
        this.getListUserByGroupId(this.groupUserId);
        this.getGroupUserByDepartment();
      } else {
        this._toast.error("Bạn chưa nhập người dùng");
      }
    });
  }

  changeState(event: any) {
    if (event.target.value == "null") {
      this.TrangThai = null;
      this.getGroupUserByDepartment();
    } else {
      this.TrangThai = event.target.value;
      this.getGroupUserByDepartment();
    }
  }

  public onPageChange(event: any): void {
    this.pagesize = event.rows;
    this.pageindex = (event.first / event.rows) + 1;
    this.getGroupUserByDepartment();
  }
}

export class GroupUser {
  Ma: number;
  TenNhom: string;
  MaNhom: string;
  MaDonVi: number;
  NguoiTao: string;
  NgayTao: Date;
  TrangThai: boolean;
  Xoa: boolean;
}
