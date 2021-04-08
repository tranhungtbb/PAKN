import { Component, OnInit, ViewChild } from '@angular/core';
import { MenuPassingObject } from '../orgnization/menuPassingObject';
import { DepartmentTree } from '../../../../models/departmentTree';
import { SystemConfigListComponent } from './system-config-list/system-config-list.component';

@Component({
  selector: 'app-system-config',
  templateUrl: './system-config.component.html',
  styleUrls: ['./system-config.component.css']
})
export class SystemConfigComponent implements OnInit {

  type: number;
  departmentId: number;
  departmentInfo: DepartmentTree;
  @ViewChild(SystemConfigListComponent, {static: false})
  private systemConffigListComponent: SystemConfigListComponent;

  constructor() { }

  ngOnInit() {
  }
  onMainDepartmentMenuClick(data: MenuPassingObject) {
    this.type = data.type;
    this.departmentInfo = data.data;
    this.departmentId = this.departmentInfo.ma;
  }

}
