import { Component, OnInit, ViewChild } from '@angular/core';
import { MenuPassingObject } from '../orgnization/menuPassingObject';
import { DepartmentMenuType } from '../orgnization/departmentMenuType';
import { DepartmentTree } from '../../../../models/departmentTree';
import { Router } from '@angular/router';
import { GroupUserListComponent } from './group-user-info/group-user-list/group-user-list.component';
import { UserInfoStorageService } from '../../../../commons/user-info-storage.service';

// declare var jquery: any;
declare var $: any;
@Component({
  selector: 'app-group-user-management',
  templateUrl: './group-user-management.component.html',
  styleUrls: ['./group-user-management.component.css']
})
export class GroupUserManagementComponent implements OnInit {
  type: number;
  departmentId: number;
  departmentInfo: DepartmentTree;
  @ViewChild(GroupUserListComponent, {static: false})
  private groupUserListComponent: GroupUserListComponent;

  constructor(private _router: Router,
    private userStorageService: UserInfoStorageService) { }

  ngOnInit() {
    this.type = DepartmentMenuType.DanhSachNhomNguoiDung;
  }

  onMainDepartmentMenuClick(data: MenuPassingObject) {
    this.type = data.type;
    this.departmentInfo = data.data;
    this.departmentId = this.departmentInfo.ma;
  }
  onAdd() {
    this._router.navigate(['/create-group-user']);
  }

  onCreateNewUser(): void {
    this._router.navigate(['/create-group-user', { id: this.departmentInfo.ma }]);
  }


  previewInfo() {
    var url = "#" + "/business/system-management/organization/report-view/" + "0" + "/GroupUserReport";
    $("#modalPrint").modal('hide');
    window.open(url, '_blank');

  }

}
