import { ToastrService } from 'ngx-toastr';
// import { Component, OnInit, ViewChild, Output, EventEmitter } from '@angular/core';
import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { TreeNode } from 'primeng/api';
import { DepartmentService } from '../../../../../services/department.service';
import { MenuPassingObject } from '../../orgnization/menuPassingObject';
import { DepartmentMenuType } from '../../orgnization/departmentMenuType';
// declare var jquery: any;
declare var $: any;

@Component({
  selector: 'app-department-tree',
  templateUrl: './department-tree.component.html',
  styleUrls: ['./department-tree.component.css']
})
export class DepartmentTreeComponent implements OnInit {
  constructor(private toastr: ToastrService, private service: DepartmentService) {

  }
  @Output() onMainDepartmentMenuClick = new EventEmitter<any>();

  @Output() setDefaultValueDate = new EventEmitter();

  public newtrenode: TreeNode[];
  selectedNode: TreeNode[];
  depid: number = 0;
  object: MenuPassingObject = new MenuPassingObject();

  ngOnInit() {
    this.getDepartmentTree();
  }

  getDepartmentTree(): void {
    this.service.getTreeDepartment()
      .subscribe(data => {
        if (data.departmentTree.length > 0) {
          data.departmentTree[0].maCapCha = null;
          this.depid = data.departmentTree[0].ma;

          this.object.type = DepartmentMenuType.DanhSachNhomNguoiDung;
          this.object.data = data.departmentTree[0];
          this.onMainDepartmentMenuClick.emit(this.object);

          this.newtrenode = this.getDataByParentId(data.departmentTree, null);
          this.newtrenode[0].expanded = true;
        }
      }, error => {
        alert(error);
      });
  }

  selectDepartment(data?: any): void {
    this.depid = data.node.ma;
    this.object.type = DepartmentMenuType.DanhSachNhomNguoiDung;
    this.object.data = data.node;
    this.onMainDepartmentMenuClick.emit(this.object);
    if (this.setDefaultValueDate != null && this.setDefaultValueDate != undefined) {
      this.setDefaultValueDate.emit();
    }
  }

  getDataByParentId(data, parent) {
    const result = data
      .filter(d => d.maCapCha === parent);

    if (!result && !result.length) {
      return null;
    }
    return result.map(({ ma, ten, kichHoat, isDonVi, level, icon }) =>
      ({
        ma,
        label: ten,
        kichHoat,
        isDonVi,
        level,
        data: { ma, ten, kichHoat, isDonVi, level, icon },
        icon,
        children: this.getDataByParentId(data, ma)
      }));
  }
}
