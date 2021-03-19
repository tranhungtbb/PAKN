import { ToastrService } from 'ngx-toastr';
import { Component, OnInit } from '@angular/core';
import { UserInfoStorageService } from '../../../commons/user-info-storage.service';
import { Router } from '@angular/router';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { RegionObject } from '../../../models/RegionObject';
import { RegionService } from '../../../services/region.service';
import { MenuItem } from 'primeng/api';
import { debug } from 'util';
import { Parterms } from 'src/app/constants/parterm';
import { DataService } from '../../../services/sharedata.service';
import { saveAs as importedSaveAs } from "file-saver";
import { CatalogService } from 'src/app/services/catalog.service';

// declare var jquery: any;
declare var $: any;

@Component({
  selector: 'region-catalog',
  templateUrl: './region-catalog.component.html',
  styleUrls: ['./region-catalog.component.css']
})

export class RegionCatalogComponent implements OnInit {

  model: RegionObject = new RegionObject();

  constructor(private service: RegionService, private _router: Router, private _notifi: ToastrService,
    private _shareData: DataService,
    private storeageService: UserInfoStorageService,private _serviceCatalog: CatalogService) {
  }
  RegionForm: FormGroup;

  public newtrenode: any[];
  selectedNode: any;
  Menuitems: MenuItem[];

  public lstTinhTP: any = [];
  public lstQuanHuyen: any = [];
  public typeMode: string = '';

  private currentUserPermissions;
  private currentUserFuntions;
  private currentUserPermissionCategories;
  private permissions;
  private mode: string = "";

  submitted: boolean = false;

  ngOnInit() {
    this.getDepartmentTree();
    this.getdataDropDown();
    this.buildForm();
  }

  ngAfterViewInit() {
    this._shareData.seteventnotificationDropdown();
  }

  public inputValidator(event: any) {
    const pattern = /^[0-9]*$/;
    //let inputChar = String.fromCharCode(event.charCode)
    if (!pattern.test(event.target.value)) {
      event.target.value = event.target.value.replace(/[^0-9]/g, "");
      // invalid character, prevent input
    }
  }

  getDepartmentTree(): void {
    this.service.getRegionFirstNode()
      .subscribe(data => {
        this.newtrenode = this.getDataByParentId(data.treehanhchinh);
      }, error => {
      });
  }

  getdataDropDown() {
    this.service.getDropDown().subscribe(data => {
      this.lstTinhTP = data.treeCapTinh;
    })
  }

  onChangeDropDown(data) {

    if (data == null) {
      this.lstQuanHuyen = [];
      this.model.quanHuyenId = null;
    } else {
      this.service.getDropDownQuanHuyen({ id: data }).subscribe(result => {
        this.lstQuanHuyen = result.treeCapQuanHuyen;
      });
    }

  }

  selectDepartment(data?: any): void {
    this.getdataById(data.node.id);
    this.rebuildForm(true);
    this.typeMode = 'view';
  }

  getDataByParentId(data) {
    if (!data || data.length == 0) {
      return null;
    }
    if (data[0].cap != 3) {
      return data.map(({ id, name, cap, description, tinhId, quanHuyenId, trangThai, code }) =>
        ({ id, label: name, cap, description, tinhId, quanHuyenId, trangThai, code, children: [], leaf: false }))
    } else {
      return data.map(({ id, name, cap, description, tinhId, quanHuyenId, trangThai, code }) =>
        ({ id, label: name, cap, description, tinhId, quanHuyenId, trangThai, code, children: [], leaf: true }))
    }
  }

  loadNode(event) {

    if (event.node) {
      var request = {
        id: event.node.id,
        cap: event.node.cap
      }
      this.service.getRegionChildNote(request).subscribe(data => {
        event.node.children = this.getDataByParentId(data.treehanhchinh);
      }
      );
    }
  }

  public specialCharacter(control: FormControl): { [key: string]: any } {
    debugger
    let ptDatePattern = Parterms.specialCharacter;///^[a-zA-Z0-9_.]+$/;
    let value = control.value;
    if (value != null) {
      if (control.value.match(ptDatePattern))
        return { "specialCharacter": true };
    }
    return null;
  }

  buildForm() {
    this.RegionForm = new FormGroup({
      'cap': new FormControl(this.model.cap),
      'tinhTP': new FormControl(this.model.tinhId),
      'quanH': new FormControl(this.model.quanHuyenId),
      'ten': new FormControl(this.model.name, [Validators.required]),
      'code': new FormControl(this.model.code, [Validators.required]),
      'moTa': new FormControl(this.model.description),
      'trangthai': new FormControl(this.model.trangThai),
      'xoa': new FormControl(this.model.xoa)
    });
  }

  rebuildForm(type) {
    this.RegionForm.reset({
      cap: { value: this.model.cap, disabled: type },
      tinhTP: { value: this.model.tinhId, disabled: type },
      quanH: { value: this.model.quanHuyenId, disabled: type },
      ten: { value: this.model.name, disabled: type },
      code: { value: this.model.code, disabled: type },
      moTa: { value: this.model.description, disabled: type },
      trangthai: { value: this.model.trangThai, disabled: type },
      xoa: { value: this.model.xoa, disabled: type }
    });
  }

  resetData() {
    if (this.model.id == 0) {
      this.model = new RegionObject();
      this.rebuildForm(false);
    } else {
      this.getdataById(this.model.id);
      this.rebuildForm(false);
      this.typeMode = 'edit';
    }
  }
  get ctrl() { return this.RegionForm.controls; }

  preCreate() {

    this.submitted = true;
    if (this.model.cap == 2 && this.model.tinhId == null) {
      $("#capTinhcontroll").show();
      return;
    } else {
      $("#capTinhcontroll").hide();
    }
    if (this.model.cap == 3) {
      if (this.model.tinhId == null) {
        $("#capTinhcontroll").show();
        return;
      } else {
        $("#capTinhcontroll").hide();
      }

      if (this.model.quanHuyenId == null) {
        $("#quanhuyenControll").show();
        return;
      } else {
        $("#quanhuyenControll").hide();
      }
    }
    if (this.RegionForm.invalid) {
      return;
    } else {
      //this.model.code = this.model.code.trim();
      this.model.name = this.model.name.trim();

      if (this.model.code != null) {
        this.model.code = this.model.code.trim().replace(/[^0-9.,]/g, "");
      }

      var request = {
        region: this.model
      }
      if (this.model.id == 0) {
        this.service.createRegion(request).subscribe(data => {
          if (data.status == 1) {
            this.getDepartmentTree();
            this.model = new RegionObject();
            this.rebuildForm(false);
            this._notifi.success("Thêm mới thành công!");
            this.getdataDropDown();
            this.submitted = false;
          } else {
            this._notifi.error(data.message);
          }
        });
      } else {
        this.service.updateRegion(request).subscribe(data => {
          if (data.status == 1) {
            this.getDepartmentTree();
            this._notifi.success("Cập nhật thành công!");
            this.getdataDropDown();
            this.submitted = false;
          } else {
            this._notifi.error(data.message);
          }
        });
      }

    }
  }

  preDelete() {
    $("#modalDelete").modal('show');
  }

  onDelete() {
    this.model.xoa = true;
    var request = {
      region: this.model
    }
    this.service.deleteRegion(request).subscribe(data => {
      if (data.status == 1) {
        this.getDepartmentTree();
        this._notifi.success("Xóa thành công!");
        this.getdataDropDown();
        this.submitted = false;
        $("#modalDelete").modal('hide');
        this.model = new RegionObject;
      } else {
        this._notifi.error(data.message);
      }
    });
  }

  getdataById(id) {
    var request = {
      id: id
    }
    this.service.getRegionbyId(request).subscribe(data => {
      this.model = data;

      if (data.tinhId != null) {
        this.onChangeDropDown(data.tinhId);
      }
    });
  }

  onChangeCap() {
    if (this.model.cap == 3 && this.model.tinhId != null) {
      this.onChangeDropDown(this.model.tinhId);
    }
  }

  onEdit() {
    this.rebuildForm(false);
    this.typeMode = 'edit';
  }

  CheckContextMenu(event: any) {
    // build Contextmenu for DepartmentTree
    this.Menuitems = [];

    this.selectedNode;

    this.getdataById(this.selectedNode.id);


    if (event.node.cap == 1) {
      if (this.checkPermission(['3', 'B_X_1']) == true) {
        this.Menuitems.push({
          label: 'Thêm mới tỉnh/ thành phố', command: (event) => {
            this.model = new RegionObject();
            this.model.cap = 1;
            this.model.tinhId = null;
            this.model.quanHuyenId = null;
            this.typeMode = "edit";
            this.rebuildForm(false);
          }
        },
          {
            label: 'Thêm mới quận/ huyện', command: (event) => {
              this.model = new RegionObject();
              this.model.cap = 2;
              this.model.tinhId = this.selectedNode.id;
              this.model.quanHuyenId = null;
              this.typeMode = "edit";
              this.rebuildForm(false);
            }
          })
      }
      if (this.checkPermission(['3', 'B_X_2']) == true) {
        this.Menuitems.push({
          label: 'Sửa địa phương', command: (event) => {
            this.model = new RegionObject();
            this.getdataById(this.selectedNode.id);
            this.rebuildForm(false);
            this.typeMode = "edit";
          }
        })
      }
      if (this.checkPermission(['3', 'B_X_3']) == true) {
        this.Menuitems.push({
          label: 'Xóa địa phương', command: (event) => {
            this.preDelete();
          }
        })
      }

      //this.Menuitems = [
      //  {
      //    label: 'Thêm mới tỉnh/thành phố', command: (event) => {
      //      this.model = new RegionObject();
      //      this.model.cap = 1;
      //      this.model.tinhId = null;
      //      this.model.quanHuyenId = null;
      //      this.typeMode = "edit";
      //      this.rebuildForm(false);
      //    }
      //  },
      //  {
      //    label: 'Thêm mới quận huyện', command: (event) => {
      //      this.model = new RegionObject();
      //      this.model.cap = 2;
      //      this.model.tinhId = this.selectedNode.id;
      //      this.model.quanHuyenId = null;
      //      this.typeMode = "edit"; 
      //      this.rebuildForm(false);
      //    }
      //  }, {
      //    label: 'Sửa địa phương', command: (event) => {
      //      this.model = new RegionObject();
      //      this.getdataById(this.selectedNode.id);
      //      this.rebuildForm(false);
      //      this.typeMode = "edit";
      //    }
      //  }, {
      //    label: 'Xóa địa phương', command: (event) => {
      //      this.preDelete();
      //    }
      //  }
      //]
    } else if (event.node.cap == 2) {
      if (this.checkPermission(['3', 'B_X_1']) == true) {
        this.Menuitems.push({
          label: 'Thêm mới quận/ huyện', command: (event) => {
            this.model = new RegionObject();
            this.model.cap = 2;
            this.model.tinhId = this.selectedNode.tinhId;
            this.model.quanHuyenId = null;
            this.typeMode = "edit";
            this.rebuildForm(false);
          }
        },
          {
            label: 'Thêm mới phường/ xã', command: (event) => {
              this.model = new RegionObject();
              this.model.cap = 3;
              this.model.tinhId = this.selectedNode.tinhId;
              this.onChangeDropDown(this.selectedNode.tinhId);
              this.model.quanHuyenId = this.selectedNode.id;
              this.typeMode = "edit";
              this.rebuildForm(false);
            }
          })
      }
      if (this.checkPermission(['3', 'B_X_2']) == true) {
        this.Menuitems.push({
          label: 'Sửa địa phương', command: (event) => {
            this.model = new RegionObject();
            this.getdataById(this.selectedNode.id);
            this.rebuildForm(false);
            this.typeMode = "edit";
          }
        })
      }
      if (this.checkPermission(['3', 'B_X_3']) == true) {
        this.Menuitems.push({
          label: 'Xóa địa phương', command: (event) => {
            this.preDelete();
          }
        })
      }


      //this.Menuitems = [
      //  {
      //    label: 'Thêm mới quận huyện', command: (event) => {
      //      this.model = new RegionObject();
      //      this.model.cap = 2;
      //      this.model.tinhId = this.selectedNode.tinhId;
      //      this.model.quanHuyenId = null;
      //      this.typeMode = "edit";
      //      this.rebuildForm(false);
      //    }
      //  },
      //  {
      //    label: 'Thêm mới phường xã', command: (event) => {
      //      this.model = new RegionObject();
      //      this.model.cap = 3;
      //      this.model.tinhId = this.selectedNode.tinhId;
      //      this.onChangeDropDown(this.selectedNode.tinhId);
      //      this.model.quanHuyenId = this.selectedNode.id;
      //      this.typeMode = "edit";
      //      this.rebuildForm(false);
      //    }
      //  }, {
      //    label: 'Sửa địa phương', command: (event) => {
      //      this.model = new RegionObject();
      //      this.getdataById(this.selectedNode.id);
      //      this.rebuildForm(false);
      //      this.typeMode = "edit";
      //    }


      //  }, {
      //    label: 'Xóa địa phương', command: (event) => {
      //      this.preDelete();
      //    }
      //  }
      //]
    } else if (event.node.cap == 3) {

      if (this.checkPermission(['3', 'B_X_1']) == true) {
        this.Menuitems.push(
          {
            label: 'Thêm mới phường/ xã', command: (event) => {
              this.model = new RegionObject();
              this.model.cap = 3;
              this.model.tinhId = this.selectedNode.tinhId;
              this.onChangeDropDown(this.selectedNode.tinhId);
              this.model.quanHuyenId = this.selectedNode.quanHuyenId;
              this.typeMode = "edit";
              this.rebuildForm(false);
            }
          })
      }
      if (this.checkPermission(['3', 'B_X_2']) == true) {
        this.Menuitems.push({
          label: 'Sửa địa phương', command: (event) => {
            this.model = new RegionObject();
            this.getdataById(this.selectedNode.id);
            this.rebuildForm(false);
            this.typeMode = "edit";
          }
        })
      }
      if (this.checkPermission(['3', 'B_X_3']) == true) {
        this.Menuitems.push({
          label: 'Xóa địa phương', command: (event) => {
            this.preDelete();
          }
        })
      }


      //this.Menuitems = [
      //  {
      //    label: 'Thêm mới phường xã', command: (event) => {
      //      this.model = new RegionObject();
      //      this.model.cap = 3;
      //      this.model.tinhId = this.selectedNode.tinhId;
      //      this.onChangeDropDown(this.selectedNode.tinhId);
      //      this.model.quanHuyenId = this.selectedNode.quanHuyenId;
      //      this.typeMode = "edit";
      //      this.rebuildForm(false);
      //    }
      //  }, {
      //    label: 'Sửa địa phương', command: (event) => {
      //      this.model = new RegionObject();
      //      this.getdataById(this.selectedNode.id);
      //      this.rebuildForm(false);
      //      this.typeMode = "edit";
      //    }
      //  }, {
      //    label: 'Xóa địa phương', command: (event) => {
      //      this.preDelete();
      //    }
      //  }
      //]
    }
  }

  onCreate(model) {

  }


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

  exportExcel() {
    let request = {
      type: 4,
      Code: '',
      Name: '',
      Status:''
    }

    this._serviceCatalog.ExportExcel(request).subscribe(
      response => {
        var today = new Date();
        var dd = String(today.getDate()).padStart(2, '0');
        var mm = String(today.getMonth() + 1).padStart(2, '0');
        var yyyy = today.getFullYear();
        var hh = String(today.getHours()).padStart(2, '0');
        var minute = String(today.getMinutes()).padStart(2, '0');
        var fileName = "DM_DiaBanHanhChinh_" + yyyy + mm + dd+hh+minute;
        var blob = new Blob([response], { type: response.type });
        importedSaveAs(blob, fileName);
      }
    );
  }
}
