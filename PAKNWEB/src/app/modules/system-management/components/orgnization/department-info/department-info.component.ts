import { Component, OnInit, ViewChild, Input, ChangeDetectionStrategy, ChangeDetectorRef, OnChanges, SimpleChanges, Output, EventEmitter } from '@angular/core';
import { DepartmentService } from '../../../../../services/department.service';
// import { Observable, Observer } from 'rxjs';
import { Router, ActivatedRoute } from '@angular/router';
// import { switchMap } from 'rxjs/operators';
import 'rxjs/add/operator/filter';
import { DepartmentTree } from '../../../../../models/departmentTree';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { DepartmentObject } from '../../../../../models/departmentObject';
// import { MenuPassingObject } from '../menuPassingObject';
// import { error } from '@angular/compiler/src/util';
import { DepartmentsListComponent } from '../departments-list/departments-list.component';
import { ToastrService } from 'ngx-toastr';
import { ArrayUtilsService } from '../../../../../commons/array-utils.service';
import { DataService } from '../../../../../services/sharedata.service';



// declare var jquery: any;
declare var $: any;

@Component({
  selector: 'app-departments-info',
  templateUrl: './department-info.component.html',
  styles: [`
.dashboardContainer {
width: 100%;
height: 100%;
position: fixed;
}
.componentsContainer {
position: fixed;
bottom: 0;
top: 100px;
width: 100%; }
.componentContainer {
overflow: auto;
position: absolute; }

.danger{
color:red;
}
.padding-l5{
padding-left:12px;
}
.no-padding-left{
padding-left:0px;
}
.padding-right{
padding-right:30px;
}
textarea {
  resize: none;
}
`],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DepartmentsInfoComponent implements OnInit, OnChanges {

  Id: number;
  Type: any;
  ParentId: any;
  DepartmentForm: FormGroup;

  model: any = new DepartmentObject;

  lstUser: Array<any> = [];
  lstDepartment: Array<any> = [];
  lstSelectedUser: Array<any> = [];
  @Input() departmentInfo: DepartmentTree;
  @Input() type: number;

  @ViewChild(DepartmentsListComponent, {static: false})
  private deplist: DepartmentsListComponent;

  @Output() myEvent = new EventEmitter();

  @Output() changeData = new EventEmitter<any>();

  isEnable: boolean = true;

  constructor(private route: ActivatedRoute,
    private router: Router,
    private service: DepartmentService,
    private _fb: FormBuilder,
    private ref: ChangeDetectorRef,
    private _noity: ToastrService,
    private _shareData: DataService,
    private arrayUtilsService: ArrayUtilsService) {

  }

  ngOnInit() {
    this.Id = this.departmentInfo.ma;
    this.newForm();
    //this.Type = this.route.snapshot.params['type'];
    this.ParentId = this.departmentInfo.maCapCha;
    this.isEnable = true;
    if (this.type == 1 || this.type == 5) {
      // Thêm cùng cấp
      this.model.maCapCha = this.ParentId;
    } else if (this.type == 2 || this.type == 6) {
      this.model.maCapCha = this.Id;
    }
    if (this.type == 1 || this.type == 2) {
      this.model.isDonVi = true;
    } else if (this.type == 5 || this.type == 6) {
      this.model.isDonVi = false;
    }

    if (this.type == 7 || this.type == 8) {
      this.lstUser = [];
      if (this.type == 7) {
        this.isEnable = false;
      }
      this.service.GetUserbyDepId(this.Id).subscribe(data => {
        this.lstUser = data.gridDepartmentUser;
        this.GetbyId(this.Id);
      })
    }

    if (this.type != 7 && this.type != 8) {
      this.service.getDropDown(0).subscribe((data) => {
        this.lstDepartment = data;
        this.rebuildForm();
      });
    } else {
      this.service.getDropDown(this.Id).subscribe((data) => {
        this.lstDepartment = data;
        this.rebuildForm();
      });
    }

  }

  ngAfterViewInit() {
    this._shareData.seteventnotificationDropdown();
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.model = new DepartmentObject();
    this.model.kichHoat = true;
    this.model.nguoiDaiDien = null;
    this.Id = this.departmentInfo.ma;
    this.ParentId = this.departmentInfo.maCapCha;
    this.isEnable = true;
    if (this.type == 1 || this.type == 2 || this.type == 5 || this.type == 6) {
      if (this.type == 1 || this.type == 2) {
        this.model.isDonVi = true;
      } else if (this.type == 5 || this.type == 6) {
        this.model.isDonVi = false;
      }
      if (this.type == 1 || this.type == 5) {
        // Thêm cùng cấp
        this.model.maCapCha = this.ParentId;
      } else if (this.type == 2 || this.type == 6) {
        this.model.maCapCha = this.Id;
      }
      setTimeout(function () {
        let inputField: HTMLElement = <HTMLElement>document.querySelectorAll('#DepartmentName')[0];
        inputField && inputField.focus();
      });
    }
    else if (this.type == 7 || this.type == 8) {
      if (this.Id) {
        this.lstUser = [];
        if (this.type == 7) {
          this.isEnable = false;
        }
        this.service.GetUserbyDepId(this.Id).subscribe(data => {
          if (data.gridDepartmentUser) {
            this.lstUser = data.gridDepartmentUser;
          }
          this.GetbyId(this.Id);
        })
      }
    }
    if (this.type != 7 && this.type != 8) {
      this.service.getDropDown(0).subscribe((data) => {
        this.lstDepartment = data;
        this.rebuildForm();
      });
    } else {
      this.service.getDropDown(this.Id).subscribe((data) => {

        this.lstDepartment = data;
        this.rebuildForm();
      });
    }
  }

  newForm() {
    this.DepartmentForm = new FormGroup({
      'ma': new FormControl(this.model.ma),
      'ten': new FormControl(this.model.ten, [Validators.required]),
      'maCapCha': new FormControl(this.model.maCapCha),
      'moTa': new FormControl(this.model.moTa),
      'nguoiDaiDien': new FormControl(this.model.nguoiDaiDien),
      'kichHoat': new FormControl(this.model.kichHoat),
      'xoa': new FormControl(this.model.xoa),
      'code': new FormControl(this.model.code, [Validators.required, Validators.pattern('[0-9a-zA-Z_-]+')]),
      'isDonVi': new FormControl(this.model.isDonVi),
      'soDienThoai': new FormControl(this.model.soDienThoai, [Validators.minLength(9), Validators.maxLength(11)]),
      'loaiPhongBan': new FormControl(this.model.loaiPhongBan)
    });

    this.disableFormControll(this.type);
  }

  //GetbyId

  GetbyId(id: number) {
    this.service.DepartmentGetbyId(id)
      .subscribe(data => {
        this.model = data.department;
        if (this.model.nguoiDaiDien == 0) {
          this.model.nguoiDaiDien = null;
        }
        this.rebuildForm();
        setTimeout(function () {
          let inputField: HTMLElement = <HTMLElement>document.querySelectorAll('#DepartmentName')[0];
          inputField && inputField.focus();
        });
      }, error => {
        alert(error);
      });
  }

  rebuildForm() {

    if (this.DepartmentForm != undefined)

      this.DepartmentForm.reset({
        ma: { value: this.model.ma, disabled: !this.isEnable },
        ten: { value: this.model.ten, disabled: !this.isEnable },
        maCapCha: { value: this.model.maCapCha, disabled: !this.isEnable },
        moTa: { value: this.model.moTa, disabled: !this.isEnable },
        nguoiDaiDien: { value: this.model.nguoiDaiDien, disabled: !this.isEnable },
        kichHoat: { value: this.model.kichHoat, disabled: !this.isEnable },
        xoa: { value: this.model.xoa, disabled: !this.isEnable },
        code: { value: this.model.code, disabled: !this.isEnable },
        isDonVi: { value: this.model.isDonVi, disabled: !this.isEnable },
        soDienThoai: { value: this.model.soDienThoai, disabled: !this.isEnable },
        loaiPhongBan: { value: this.model.loaiPhongBan, disabled: !this.isEnable }
      });
    this.disableFormControll(this.type);
  }

  clearData() {
    // Lấy dữ liệu từ Form xuống
    this.model = new DepartmentObject();
    this.model.kichHoat = true;
    this.model.nguoiDaiDien = null;
    this.Id = this.departmentInfo.ma;
    if (this.type == 1 || this.type == 2 || this.type == 5 || this.type == 6) {
      if (this.type == 1 || this.type == 2) {
        this.model.isDonVi = true;
      } else if (this.type == 5 || this.type == 6) {
        this.model.isDonVi = false;
      }
      if (this.type == 1 || this.type == 5) {
        // Thêm cùng cấp
        this.model.maCapCha = this.ParentId;
      } else if (this.type == 2 || this.type == 6) {
        this.model.maCapCha = this.Id;
      }
      setTimeout(function () {
        let inputField: HTMLElement = <HTMLElement>document.querySelectorAll('#DepartmentName')[0];
        inputField && inputField.focus();
      });
      this.rebuildForm();
    }
    else if (this.type == 7 || this.type == 8) {
      if (this.Id) {
        this.lstUser = [];
        this.service.GetUserbyDepId(this.Id).subscribe(data => {
          if (data.gridDepartmentUser) {
            this.lstUser = data.gridDepartmentUser;
          }
          this.GetbyId(this.Id);
        })


      }
    }
  }

  onCreateNewDepartment(): void {
    if (this.DepartmentForm.invalid) {
      if (this.DepartmentForm.controls.ten.status == "INVALID") {
        $("#DepartmentName").focus();
        return;
      } else {
        if (this.model.ten.trim() == '') {
          this.model.ten = this.model.ten.trim();
          this.DepartmentForm.controls.ten.setValue(this.model.ten);
          $("#DepartmentName").focus();
          return;
        }
      }
      if (this.DepartmentForm.controls.code.status == "INVALID") {
        $("#code").focus();
        $("#code").blur();
        this.DepartmentForm.controls.code.setValue('');
        return;
      } else {
        if (this.model.code.trim() == '') {
          this.model.code = this.model.code.trim();
          this.DepartmentForm.controls.code.setValue(this.model.code);
          $("#code").focus();
          return;
        }
      }
      if (this.DepartmentForm.controls.soDienThoai.status == "INVALID") {
        $("#PhoneNumber").focus();
        return;
      }
    } else {
      if (this.model.ten.trim() == '') {
        this.model.ten = this.model.ten.trim();
        this.DepartmentForm.controls.ten.setValue(this.model.ten);
        $("#DepartmentName").focus();
        return;
      }
      if (this.model.code.trim() == '') {
        this.model.code = this.model.code.trim();
        this.DepartmentForm.controls.code.setValue(this.model.code);
        $("#code").focus();
        return;
      }
    }
    this.model.moTa = this.model.moTa != null ? this.model.moTa.trim() : "";
    var data = this.model;
    if (data.ma == undefined || data.ma == null) {
      data.ma = 0;
      data.xoa = false;
    }
    try {
      data.nguoiDaiDien = parseInt(data.nguoiDaiDien.toString());
    } catch{
      data.nguoiDaiDien = null;
    }
    try {
      data.maCapCha = parseInt(data.maCapCha.toString())
    } catch{
      data.maCapCha = null;
    }

    var request = {
      Department: data
    }
    if (data.ma == 0) {
      // thêm mới
      this.service.createDepartment(request)
        .subscribe(data => {
          if (data.status == 3) {
            if (data.message !== "Mã đã tồn tại vui lòng nhập mã khác!") {
              $("#DepartmentName").focus();
              this._noity.error("Tên đã tồn tại");
            } else {
              $("#code").focus();
              this._noity.error("Mã đã tồn tại");
            }
          } else if (data.status == 1) {
            this.type = 7;
            this.disableFormControll(7);
            this.model = data.department;
            this.myEvent.emit(null);
            var passingdata = {
              type: 7,
              data: this.model
            }
            this.changeData.emit(passingdata);
            this._noity.success("Thêm mới thành công");
          } else if (data.status == 2) {
            this._noity.error(data.message);
          }
        }, error => {
          this._noity.error("Thêm mới thất bại");
          console.log(error);
        });
    } else {
      // Cập nhật
      this.service.UpdateDepartment(request).subscribe(data => {
        if (data.status === 3) {
          if (data.message == "Mã đã tồn tại vui lòng nhập mã khác!") {
            $("#code").focus();
            $("#lblCodeError").show();
            $("#lblNameError").hide();
            this._noity.error("Mã đã tồn tại");
          } else {
            $("#lblCodeError").hide();
            $("#DepartmentName").focus();
            $("#lblNameError").show();
            this._noity.error("Tên đã tồn tại");
          }
        } else if (data.status === 1) {
          this.model = data.department;
          this.myEvent.emit(null);
          this._noity.success("Cập nhật thành công")
        } else {
          this._noity.error(data.message);
        }

      }, error => {
        this._noity.error("Cập nhật thất bại")
        console.log(error);
      });
    }
  }

  // Disable/Enable Form Controll
  disableFormControll(type: number) {

  }
  checkvalue(event: any) {
    if (/\D/g.test(event.target.value)) {
      this.model.soDienThoai = event.target.value.replace(/\D/g, '');
      this.DepartmentForm.controls.soDienThoai.setValue(this.model.soDienThoai);
    }
  }



  get ma() { return this.DepartmentForm.get('ma'); }
  get ten() { return this.DepartmentForm.get('ten'); }
  get code() { return this.DepartmentForm.get('code'); }
  get kichHoat() { return this.DepartmentForm.get('kichHoat'); }
  get nguoiDaiDien() { return this.DepartmentForm.get('nguoiDaiDien'); }
  get moTa() { return this.DepartmentForm.get('moTa'); }
  get soDienThoai() { return this.DepartmentForm.get('soDienThoai'); }


}
