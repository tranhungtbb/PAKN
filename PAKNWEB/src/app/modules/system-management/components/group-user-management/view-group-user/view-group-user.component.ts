import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { GroupUserService } from '../../../../../services/groupuser.service';
import { DepartmentService } from '../../../../../services/department.service';
// import { Observable, Observer } from 'rxjs';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
// import { switchMap } from 'rxjs/operators';
import 'rxjs/add/operator/filter';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { GroupUserObject } from '../../../../../models/groupUserObject';
// import { DepartmentTree } from '../../../../../models/departmentTree';
import { Location } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { AuthenticationService } from '../../../../../services/authentication.service';
import { UserInfoStorageService } from '../../../../../commons/user-info-storage.service';
// import { forEach } from '@angular/router/src/utils/collection';
import { DataService } from '../../../../../services/sharedata.service';
@Component({
  selector: 'app-view-group-user',
  templateUrl: './view-group-user.component.html',
  styleUrls: ['./view-group-user.component.css']
  //changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ViewGroupUserComponent implements OnInit {

  updateGroupUserForm: FormGroup;
  GroupUserId: number = 1;
  Type: any;
  model: GroupUserObject = new GroupUserObject();
  submitted: boolean = false;
  listUnits: any = [];
  listPermissionCategories: any[];
  listPermissionGroupUserSelected: any[] = [];
  departmentId: number = 1;
  isSuperAdmin: boolean = false;

  constructor(private route: ActivatedRoute,
    private router: Router,
    private groupUserservice: GroupUserService,
    private departmentService: DepartmentService,
    private _fb: FormBuilder,
    private ref: ChangeDetectorRef,
    private location: Location,
    private toastr: ToastrService,
    private formBuilder: FormBuilder,
    private _shareData: DataService,
    private authenService: AuthenticationService,
    private localStorage: UserInfoStorageService, ) { }

  ngOnInit() {
    this.updateGroupUserForm = this.formBuilder.group({
      ma: ['', []],
      tenNhom: ['', Validators.required],
      maNhom: ['', [Validators.required]],
      moTa: ['', []],
      nguoiTao: ['', []],
      trangThai: ['', []],
      xoa: ['', []],
      //maDonVi: ['', [Validators.required]],
    }); 
    //this.isSuperAdmin ? this.updateGroupUserForm.get('maDonVi').enable() : this.updateGroupUserForm.get('maDonVi').disable();

    this.route.params.subscribe(
      params => {
        this.model.ma = params['id'];
        this.groupUserservice.getCreateGroupUserDatas({ GroupUserUpdateId: this.model.ma }).subscribe(success => {
          this.model = success.groupUser;
          if (success.units) {
            for (var item of success.units) {
              item.value = +item.value;
            }
          }
          this.listUnits = success.units;
          this.listPermissionCategories = success.permissionCategories;
          this.listPermissionGroupUserSelected = success.listGroupUserSelectedPermissions;
          this.model.maDonVi = success.currentUnitId;
          this.onGroupUserLoadPermission(this.listPermissionGroupUserSelected);
        }, err => {
          console.error(err);
        });
        return null;
      }
    );
  }

  ngAfterViewInit() {
    this._shareData.seteventnotificationDropdown();
  }

  get f() { return this.updateGroupUserForm.controls; }

  onGroupUserLoadPermission(listPermissionGroupUserSelected): void {
    this.clearPermisison();
    for (var i = 0; i < this.listPermissionGroupUserSelected.length; i++) {
      this.checkPermission(this.listPermissionGroupUserSelected[i], true);
    }
  }

  onCategoryChange(ev, permisionCategory): void {
    for (var i = 0; i < permisionCategory.htFunction.length; i++) {
      permisionCategory.htFunction[i].selected = ev.checked;
      for (var j = 0; j < permisionCategory.htFunction[i].htPermission.length; j++) {
        permisionCategory.htFunction[i].htPermission[j].selected = ev.checked;
        var permissionId = permisionCategory.htFunction[i].htPermission[j].id;
        if (ev.checked) {
          this.listPermissionGroupUserSelected.push(permissionId);
        } else {
          var index = this.listPermissionGroupUserSelected.indexOf(permissionId, 0);
          if (index > -1) {
            this.listPermissionGroupUserSelected.splice(index, 1);
          }
        }
      }
    }
  }

  onFunctionChange(ev, funct, permisionCategory): void {
    for (var j = 0; j < funct.htPermission.length; j++) {
      funct.htPermission[j].selected = ev.checked;
      var permissionId = funct.htPermission[j].id;
      if (ev.checked) {
        this.listPermissionGroupUserSelected.push(permissionId);
      } else {
        var index = this.listPermissionGroupUserSelected.indexOf(permissionId, 0);
        if (index > -1) {
          this.listPermissionGroupUserSelected.splice(index, 1);
        }
      }
    }
    if (ev.checked) {
      permisionCategory.selected = ev.checked;
    } else {
      this.checkCategorySelected(permisionCategory);
    }
  }

  onPermissionChange(event, permission, funct, permisionCategory): void {
    this.checkFunctionSelected(permisionCategory, funct);
    if (event.checked) {
      this.listPermissionGroupUserSelected.push(permission.id);
    } else {
      var index = this.listPermissionGroupUserSelected.indexOf(permission.id, 0);
      if (index > -1) {
        this.listPermissionGroupUserSelected.splice(index, 1);
      }
    }
  }

  private checkCategorySelected(permisionCategory: any) {
    var hasSelectedChild = false;
    for (var j = 0; j < permisionCategory.htFunction.length; j++) {
      if (permisionCategory.htFunction[j].selected) {
        hasSelectedChild = true;
        break;
      }
    }
    permisionCategory.selected = hasSelectedChild;
  }

  private checkFunctionSelected(permisionCategory, funct) {
    var hasSelectedChild = false;
    for (var j = 0; j < funct.htPermission.length; j++) {
      if (funct.htPermission[j].selected) {
        hasSelectedChild = true;
        break;
      }
    }
    funct.selected = hasSelectedChild;
    this.checkCategorySelected(permisionCategory);
  }

  private clearPermisison() {
    for (var i = 0; i < this.listPermissionCategories.length; i++) {
      var permissioncategory = this.listPermissionCategories[i];
      permissioncategory.selected = false;
      permissioncategory.disabled = false;
      for (var j = 0; j < permissioncategory.htFunction.length; j++) {
        var funct = permissioncategory.htFunction[j];
        funct.selected = false;
        funct.disabled = false;
        for (var k = 0; k < funct.htPermission.length; k++) {
          funct.htPermission[k].selected = false;
          funct.htPermission[k].disabled = false;
        }
      }
    }
  }

  private checkPermission(permissionId, isUserSelected) {
    for (var i = 0; i < this.listPermissionCategories.length; i++) {
      var permissioncategory = this.listPermissionCategories[i];
      var isCategorySelected = this.listPermissionCategories[i].selected;
      var isCategoryDisabled = this.listPermissionCategories[i].disabled;
      for (var j = 0; j < permissioncategory.htFunction.length; j++) {
        var funct = permissioncategory.htFunction[j];
        var isFunctSelected = permissioncategory.htFunction[j].selected;
        var isFunctDisabled = permissioncategory.htFunction[j].disabled;
        for (var k = 0; k < funct.htPermission.length; k++) {
          if (funct.htPermission[k].id == permissionId) {
            funct.htPermission[k].selected = true;
            funct.htPermission[k].disabled = isUserSelected ? false : true;
            isCategorySelected = true;
            isFunctSelected = true;
            isCategoryDisabled = isUserSelected ? isCategoryDisabled : true;
            isFunctDisabled = isUserSelected ? isFunctDisabled : true;
          }
        }
        funct.selected = isFunctSelected;
        funct.disabled = isFunctDisabled;
      }
      permissioncategory.selected = isCategorySelected;
      permissioncategory.disabled = isCategoryDisabled;
    }
  }

  private checkPermissionSellected() {
    var resull: boolean = false;

    for (var i = 0; i < this.listPermissionCategories.length; i++) {
      var permissioncategory = this.listPermissionCategories[i];
      if (permissioncategory.selected) {
        resull = true;
      }
    }
    return resull;
  }

}
