import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators, FormArray } from '@angular/forms';
import { UserObject } from '../../../../../models/UserObject';
import { UserService } from '../../../../../services/user.service';
import { ActivatedRoute, Router } from '@angular/router';
import { UserInfoStorageService } from '../../../../../commons/user-info-storage.service';
import { ToastrService } from 'ngx-toastr';
import { UploadFileService } from '../../../../../services/uploadfiles.service';
import { DataService } from '../../../../../services/sharedata.service';

@Component({
  selector: 'app-create-user',
  templateUrl: './create-user.component.html',
  styleUrls: ['./create-user.component.css']
})
export class CreateUserComponent implements OnInit {

  createUserForm: FormGroup;
  deptForm: FormArray;
  model: UserObject = new UserObject();
  submitted: boolean = false;
  listUnits: any;
  listPosistions: any;
  listDepartments: any;
  listGroupUsers: any[];
  listPermissionCategories: any[];
  listPermissionUserSelected: any[] = [];
  unitIdCapSo: any;
  listUserDepts: any[] = [];
  listSexs: any[] = [{ text: 'Nam', value: true }, { text: 'Nữ', value: false }];
  listTrangThai: any[] = [{ text: 'Đang Hoạt Động', value: true }, { text: 'Không Hoạt Động', value: false }];
  listVaiTro: any[] = [{ text: 'Chuyên viên', value: 0 }, { text: 'Lãnh đạo', value: 1 }, { text: 'Cán bộ', value: 2 }];
  listPhongBan: any[];
  imgURL: any = "../../../../../../assets/dist/img/image/DefaultAvatar.png";
  departmentId: number = 1;
  //isSuperAdmin: boolean = false;

  @ViewChild('dinhKem', {static: false}) public fileDK: ElementRef;
  listFile: any = [];
  file: File = null;
  checkFileChange = false;

  constructor(private formBuilder: FormBuilder,
    private userService: UserService,
    private route: ActivatedRoute,
    private localStorage: UserInfoStorageService,
    private toastr: ToastrService,
    private router: Router,
    private _shareData: DataService,
    private fileService: UploadFileService) { }

  ngOnInit() {
    this.buildForm();
    this.submitted = false;
    //this.isSuperAdmin = this.localStorage.getIsSuperAdmin();
    //this.isSuperAdmin ? this.createUserForm.get('unit').enable() : this.createUserForm.get('unit').disable();
    this.route.params.subscribe(
      params => {
        this.model.unitId = 1;
        this.userService.getCreateUserDatas({ DepartmentId: this.model.unitId }).subscribe(success => {
          this.listUnits = success.units;
          this.listPosistions = success.posistions;
          this.listDepartments = success.departments;
          this.listGroupUsers = success.groupUsers;
          this.listPermissionCategories = success.permissionCategories;
          let isDonvi: boolean = true;
          this.model.unitId = success.currentUnitId;
        }, err => {
        });
        return null;
      }
    );
  }

  ngAfterViewInit() {
    this._shareData.seteventnotificationDropdown();
  }

  buildForm() {
    this.createUserForm = new FormGroup({
      'mail': new FormControl(this.model.homThu, [Validators.required, Validators.email]),
      'gioiTinh': new FormControl(this.model.gioiTinh, [Validators.required]),
      'userName': new FormControl(this.model.hoTen, [Validators.required]),
      'role': new FormControl(this.model.role, [Validators.required]),
      'phone': new FormControl(this.model.dienThoai, [Validators.maxLength(20), Validators.minLength(9), Validators.pattern('[0-9]+')]),
      'state': new FormControl(this.model.kichHoat, [Validators.required]),
      'address': new FormControl(this.model.diaChi, []),
      'positionId': new FormControl(this.model.maChucVu, [Validators.required]),
      'unit': new FormControl(this.model.listPhongBan, [Validators.required]),
    });
  }

  onFileChange(event) {
    if (event.target.files.length == 0) {
      return;
    }
    else {
      const check = this.fileService.checkFileWasExitsted(event, this.listFile);
      if (check === 1) {
        this.listFile = [];
        this.file = event.target.files[0];
        this.checkFileChange = true;
        this.listFile.push(this.file);
        this.fileDK.nativeElement.value = "";
        var reader = new FileReader();
        reader.readAsDataURL(this.file);
        reader.onload = (_event) => {
          this.imgURL = reader.result;
        }
      } else if (check === 2) {
        this.toastr.error('Không được phép đẩy trùng tên file lên hệ thống');
      } else {
        this.toastr.error('File tải lên vượt quá dung lượng cho phép 10MB');
      }
    }
  }

  get f() { return this.createUserForm.controls; }

  onSave(): void {
    this.submitted = true;
    if (this.createUserForm.invalid) {
      return;
    }
    this.listUserDepts = [];
    if (this.model.listPhongBan != null && this.model.listPhongBan.length > 0) {
      for (var i = 0; i < this.model.listPhongBan.length; i++) {
        this.listUserDepts.push({
          DepartmentId: this.model.listPhongBan[i],
          PosistionId: this.model.maChucVu,
          UserId: this.model.ma
        });
      }
    }

    this.model.checkFileChange = this.checkFileChange;

    this.userService.createUser({
      User: this.model,
      PermissionCategories: this.listPermissionCategories,
      GroupUsers: this.listGroupUsers,
      UserDepartments: this.listUserDepts,
      listFile: this.listFile,
    }).subscribe(success => {
      if (success.status == 1) {
        this.router.navigate(['../business/system-management/user']);
        this.toastr.success("Thêm mới thành công.");
      } else {
        this.toastr.error(success.message);
      }
    });
  }

  onGroupUserChange(event, group): void {
    this.clearPermisison();
    let listGroup: any[] = [];
    for (var i = 0; i < this.listGroupUsers.length; i++) {
      if (this.listGroupUsers[i].selected)
        listGroup.push(this.listGroupUsers[i]);
    }

    for (var i = 0; i < this.listPermissionUserSelected.length; i++) {
      this.checkPermission(this.listPermissionUserSelected[i], true);
    }

    for (var j = 0; j < listGroup.length; j++) {
      for (var i = 0; i < listGroup[j].permissionIds.length; i++) {
        this.checkPermission(listGroup[j].permissionIds[i], false);
      }
    }
  }

  onCategoryChange(ev, permisionCategory): void {
    for (var i = 0; i < permisionCategory.htFunction.length; i++) {
      permisionCategory.htFunction[i].selected = ev.checked;
      for (var j = 0; j < permisionCategory.htFunction[i].htPermission.length; j++) {
        permisionCategory.htFunction[i].htPermission[j].selected = ev.checked;
        var permissionId = permisionCategory.htFunction[i].htPermission[j].id;
        if (ev.checked) {
          this.listPermissionUserSelected.push(permissionId);
        } else {
          var index = this.listPermissionUserSelected.indexOf(permissionId, 0);
          if (index > -1) {
            this.listPermissionUserSelected.splice(index, 1);
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
        this.listPermissionUserSelected.push(permissionId);
      } else {
        var index = this.listPermissionUserSelected.indexOf(permissionId, 0);
        if (index > -1) {
          this.listPermissionUserSelected.splice(index, 1);
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
      this.listPermissionUserSelected.push(permission.id);
    } else {
      var index = this.listPermissionUserSelected.indexOf(permission.id, 0);
      if (index > -1) {
        this.listPermissionUserSelected.splice(index, 1);
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

  isSelected(deptId) {
    return deptId == null || deptId == 0;
  }

  onHuy() {
    this.model = new UserObject();
    this.clearPermisison();
  }
}
