import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { DepartmentService } from '../../../../../services/department.service';
import { DepartmentTree } from '../../../../../models/departmentTree';
import { Observable } from 'rxjs';
// import { forEach } from '@angular/router/src/utils/collection';
// import { RouterModule } from '@angular/router';
import { Router } from '@angular/router';
import { MenuPassingObject } from '../menuPassingObject';
import { DepartmentMenuType } from '../departmentMenuType';
import { DualistDepartmentUserObject } from '../../../../../models/DualistDepartmentUserObject';
import { ToastrService } from 'ngx-toastr';
import { UserInfoStorageService } from '../../../../../commons/user-info-storage.service';
import { TreeNode } from 'primeng/api';
import { MenuItem } from 'primeng/api';
import { DataService } from '../../../../../services/sharedata.service';

// declare var jquery: any;
declare var $: any;

@Component({
  selector: 'app-departments-list',
  templateUrl: './departments-list.component.html',
  styleUrls: ['./departments-list.component.css']
})
export class DepartmentsListComponent implements OnInit {
  public treeNodes: Observable<any[]>;
  public queryParams: {};
  public treeAddUser: TreeNode[];

  public newtrenode: TreeNode[];
  selectedNode: TreeNode[];

  selectedTreeAddUser: TreeNode[];

  private currentUserPermissions;
  private currentUserFuntions;
  private currentUserPermissionCategories;
  private permissions;
  private mode: string = "";

  currentNode: any = {};


  object: MenuPassingObject = new MenuPassingObject();
  @Output() onMainDepartmentMenuClick = new EventEmitter<MenuPassingObject>();

  @Output() onLoadInfoClick = new EventEmitter<MenuPassingObject>();

  @Output() reLoadUser = new EventEmitter();

  DepId: number;
  userlstadd: Array<DualistDepartmentUserObject> = [];
  confirmed: Array<DualistDepartmentUserObject> = [];
  request: any = {};
  searchuser: string = '';
  selectedId: number = 0;
  changeDepId: number = 0;

  isAdmin = false;

  Menuitems: MenuItem[];

  format = {
    add: 'Chọn', remove: 'Bỏ chọn', all: 'Chọn tất cả', none: 'Bỏ chọn tất cả'
  };


  constructor(private service: DepartmentService, private _router: Router, private _notifi: ToastrService,
    private _shareData: DataService,
    private storeageService: UserInfoStorageService) {
  }

  ngOnInit() {
    this.getDepartmentTree();
    this.isAdmin = this.storeageService.getIsSuperAdmin();

    $("#isAdmin").prop('checked', this.isAdmin);
  }

  ngAfterViewInit() {
    this._shareData.seteventnotificationDropdown();
  }

  getDepartmentTree(): void {
    this.service.getTreeDepartment()
      .subscribe(data => {
        data.departmentTree[0].maCapCha = null;
        this.treeNodes = data.departmentTree;

        this.newtrenode = this.getDataByParentId(data.departmentTree, null);
        if (this.selectedId == 0) {
          this.firstLoadUser(this.treeNodes[0]);
        }
      }, error => {
        alert(error);
      });
  }

  firstLoadUser(data) {
    this.selectedId = data.ma;
    this.object.type = DepartmentMenuType.DanhSachNguoiDung;
    this.object.data = data;
    this.onMainDepartmentMenuClick.emit(this.object);
  }

  showMessage(data?: any): void {
    console.log(data.item);
  }

  selectDepartment(data?: any): void {
    this.selectedId = data.node.ma;
    this.object.type = DepartmentMenuType.DanhSachNguoiDung;
    this.object.data = data.node;
    this.onMainDepartmentMenuClick.emit(this.object);
  }


  onDepartmentMenuClick(type: number, data?: any) {

    this.object.type = type;
    this.object.data = data;
    this.object.data.maCapCha = data.parent != null ? data.parent.ma : null;
    this.DepId = data.ma;
    if (type != 3 && type != 9) {
      this.onMainDepartmentMenuClick.emit(this.object);
    } else if (type == 3) {
      //Thêm người dùng
      this.GetTreeUserToAdd(this.DepId);
      this.confirmed = [];
      this.searchuser = '';
      this.GetUserToAdd(this.DepId, data.ma);
    } else if (type == 9) {
      //Xóa phòng ban
      $("#modal-confirm").modal('show');
    }
  }

  public displayMenu(item: any): boolean {
    if (!item.kichHoat) {
      return false;
    } else {
      return true;
    }
  }

  public isMenuItemType1(item: any): boolean {
    return item.isDonVi;
  }

  DeleteDePartment(id: number) {
    this.service.DeleteDepartment(id).subscribe(data => {
      if (data.status == 1) {
        $("#modal-confirm").modal('hide');
        this.selectedId = 0;
        this.getDepartmentTree();
        this._notifi.success("Xóa thành công!");
      } else if (data.status == 2) {
        $("#modal-confirm").modal('hide');
        this._notifi.error(data.message);
      }
    }, error => {
      this._notifi.error(error);
    })
  }

  public isMenuItemType2(item: any): boolean {

    return !item.isDonVi;
  }

  // Thêm mới người dùng vào đơn vị
  SaveDepartmentUser(depid: number) {

    if (this.confirmed.length == 0) {
      this._notifi.error("Bạn chưa chọn người dùng");
      return;
    } else {

      let LstId: Array<any> = [];
      for (var i = 0; i < this.confirmed.length; i++) {
        LstId.push(this.confirmed[i].ma)

        LstId.join();
      }

      var request = {
        Request: {
          lstId: LstId.join(),
          DepId: depid
        }
      }
      this.service.AddDepartmentUser(request).subscribe(data => {
        $("#modal-adduser").modal('hide');
        this._notifi.success("Thêm người dùng thành công!");
        this.reLoadUser.emit(this.object);

      }, error => {
        alert(error)
      })
    }
  }

  GetUserToAdd(id: number, changeId: number) {

    this.request.HoTen = this.searchuser.trim();
    this.request.SelectDepId = id;
    this.request.ChangeDepId = changeId;

    this.service.DepartmentGetUserToAdd(this.request).subscribe(data => {
      if (data.gridDepartmentUser != null)
        this.userlstadd = data.gridDepartmentUser;
      $("#modal-adduser").modal('show');
    }, error => {
      alert(error)
    })
  }

  changeDepartment(data?: any) {
    this.changeDepId = data.node.ma;
    if (data.node.ma == this.DepId) {
      this.userlstadd = [];
      return;
    } else {

      this.GetUserToAdd(this.DepId, data.node.ma)
    }
  }

  public activeClassSelectUser(data: any) {
    var active = false;
    if (data.ma == this.changeDepId) {
      active = true
    }
    return {
      'treenodeactive': active
    };
  }

  SearchUser() {
    if (this.request.ChangeDepId) {
      this.GetUserToAdd(this.request.SelectDepId, this.request.ChangeDepId);
    } else {
      this.GetUserToAdd(this.request.SelectDepId, this.request.SelectDepId);
    }
  }

  GetTreeUserToAdd(id: number) {
    this.service.DepartmentTreeAddUser(id).subscribe(data => {
      if (data.departmentTree != null) {
        data.departmentTree[0].maCapCha = null;
        this.treeAddUser = this.getDataByParentId(data.departmentTree, null);
        this.changeDepId = data.departmentTree[0].ma;
      }
      
    },
      error => {
        alert(error);
      });
  }

  myMethod(data: any) {
    if (data.charCode == 13) {
      this.SearchUser();
    }
  }

  public checkIsActive(item: any): boolean {
    return item.kichHoat;
  }

  public checkAddUser(item: any): boolean {
    if ((!item.isDonVi) && item.kichHoat) {
      return true;
    } else {
      return false;
    }
  }
  getDataByParentId(data, parent) {
    const result = data
      .filter(d => d.maCapCha === parent);

    if (!result && !result.length) {
      return null;
    }

    return result.map(({ ma, ten, kichHoat, isDonVi, level, icon }) =>
      ({ ma, label: ten, kichHoat, isDonVi, level, icon, expanded: true, children: this.getDataByParentId(data, ma) }))
  }

  CheckContextMenu(event: any) {
    // build Contextmenu for DepartmentTree
    this.Menuitems = [];

    this.currentNode = this.selectedNode;

    if (event.node.isDonVi) {
      this.Menuitems = [
        //{
        //  label: 'Thêm đơn vị cùng cấp', command: (event) => {
        //    console.log();
        //    this.onDepartmentMenuClick(1, this.currentNode);
        //  }
        //},
        {
          label: 'Thêm phòng ban cấp con', command: (event) => {
            this.onDepartmentMenuClick(6, this.currentNode);
          }
        }, {
          label: 'Xem đơn vị', command: (event) => {
            this.onDepartmentMenuClick(7, this.currentNode);
          }
        }, {
          label: 'Sửa đơn vị', command: (event) => {
            this.onDepartmentMenuClick(8, this.currentNode);
          }
        }
        //, {
        //  label: 'Xóa đơn vị', command: (event) => {
        //    this.onDepartmentMenuClick(9, this.currentNode);
        //  }
        //}
      ]
    } else {
      this.Menuitems = [
        {
          label: 'Thêm phòng ban cùng cấp', command: (event) => {
            this.onDepartmentMenuClick(5, this.currentNode);
          }
        }, {
          label: 'Thêm phòng ban cấp con', command: (event) => {
            this.onDepartmentMenuClick(6, this.currentNode);
          }
        }, {
          label: 'Xem phòng ban', command: (event) => {
            this.onDepartmentMenuClick(7, this.currentNode);
          }
        }, {
          label: 'Sửa phòng ban', command: (event) => {
            this.onDepartmentMenuClick(8, this.currentNode);
          }
        }, {
          label: 'Xóa phòng ban', command: (event) => {
            this.onDepartmentMenuClick(9, this.currentNode);
          }
        }, {
          label: 'Thêm người dùng', command: (event) => {
            this.onDepartmentMenuClick(3, this.currentNode);
          }
        }
      ]
    }
  }

  // Check Permission on ContextMenu
  private checkPermission(val) {

    this.mode = val[0];
    this.permissions = val.slice(1);

    let hasPermission = false;
    let isAdmin = this.storeageService.getIsSuperAdmin();

    let isLogin = this.storeageService.getAccessToken();
    if (isLogin == '' || isLogin == null || isLogin == undefined) {
    } else {
      if (this.mode === "1") // Category
      {
        this.currentUserPermissionCategories = this.storeageService.getPermissionCategories().split(',');
        for (let permission of this.permissions) {
          hasPermission = this.currentUserPermissionCategories.includes(permission);
          if (hasPermission) break;
        }
      } else if (this.mode === "2") {
        this.currentUserFuntions = this.storeageService.getFunctions().split(',');
        for (let permission of this.permissions) {
          hasPermission = this.currentUserFuntions.includes(permission);
          if (hasPermission) break;
        }
      } else if (this.mode === "3") {
        this.currentUserPermissions = this.storeageService.getPermissions().split(',');
        for (let permission of this.permissions) {
          hasPermission = this.currentUserPermissions.includes(permission);
          if (hasPermission) break;
        }
      }
    }


    return isAdmin || hasPermission;
  }


}
