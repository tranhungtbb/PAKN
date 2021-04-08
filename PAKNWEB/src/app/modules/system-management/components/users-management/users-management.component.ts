import { Component, OnInit } from '@angular/core';
import { DepartmentTree } from '../../../../models/departmentTree';
import { MenuPassingObject } from '../orgnization/menuPassingObject';
import { Router } from '@angular/router';
import { UserInfoStorageService } from '../../../../commons/user-info-storage.service';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
// import { defineLocale } from 'ngx-bootstrap/chronos'; 
// declare var jquery: any;
declare var $: any;
@Component({
  selector: 'app-users-management',
  templateUrl: './users-management.component.html',
  styleUrls: ['./users-management.component.css']
})
export class UsersManagementComponent implements OnInit {
  type: number;
  departmentInfo: DepartmentTree;
  departmentId: number;

  constructor(private router: Router, private userStorageService: UserInfoStorageService, private localeService: BsLocaleService) { }

  ngOnInit() {
    this.localeService.use('vi'); 
  }

  onMainDepartmentMenuClick(data: MenuPassingObject) {
    this.type = data.type;
    this.departmentInfo = data.data;
    this.departmentId = this.departmentInfo.ma;
  }

  onCreateNewUser(): void {
    this.router.navigate(['./create-user', { id: this.departmentInfo.ma }]);
  }

  previewInfo() {
    var url = "#" + "/business/system-management/organization/report-view/" + "0" + "/UserReport";
    $("#modalPrint").modal('hide');
    window.open(url, '_blank');

  }
}

