// import { Component, OnInit, Renderer2, Inject, Output, EventEmitter, Input, OnChanges, SimpleChanges } from '@angular/core';
import { Component, OnInit, Renderer2, Inject, Input, OnChanges, SimpleChanges } from '@angular/core';
import { DatePipe, DOCUMENT } from '@angular/common';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
// import { DOCUMENT } from '@angular/platform-browser'; 
// import { Observable } from 'rxjs/Observable'; 
import { MenuPassingObject } from '../../orgnization/menuPassingObject';
import { DepartmentTree } from '../../../../../models/departmentTree';
import { ToastrService } from 'ngx-toastr'; 
// import { switchMap, debounceTime, tap, finalize } from 'rxjs/operators';
import { UserInfoStorageService } from '../../../../../commons/user-info-storage.service';
import { SystemconfigService } from '../../../../../services/systemconfig.service';
import { SystemConfigObject } from '../../../../../models/SystemConfigObject';
import { DataService } from '../../../../../services/sharedata.service';

// declare var jquery: any;
declare var $: any;
// declare var data: any;
@Component({
  selector: 'app-system-config-list',
  templateUrl: './system-config-list.component.html',
  styleUrls: ['./system-config-list.component.css'],
  providers: [DatePipe],
})
export class SystemConfigListComponent implements OnInit, OnChanges {

  systemConfigForm: FormGroup;
  viewsystemConfigForm: FormGroup;
  model: SystemConfigObject = new SystemConfigObject();
  submitted: boolean = false;
  Id: number;

  public systemConfiglst: SystemConfigObject[]; 
  public item: any;

  isLoading = false;

  @Input() departmentInfo: DepartmentTree;
  @Input() departmentId: number;

  object: MenuPassingObject = new MenuPassingObject();
   
  listTrangThai: any = [
    { value: true, text: "Đang sử dụng" },
    { value: false, text: "Hết hiệu lực" }
  ];
  errorMessage: any;
  maCauhinh: string = '';
  pageindex: number = 1;
  pagesize: number = 20;
  tencauhinh: string = '';
     
  userLoginId: string = '';

  public totalRecords = 0;
  isFilterable: boolean = false;

  constructor(private route: ActivatedRoute, private _fb: FormBuilder, private _systemConfigService: SystemconfigService, private _renderer2: Renderer2, @Inject(DOCUMENT) private _document,
    private _shareData: DataService,
    private _toast: ToastrService,
    private localStorage: UserInfoStorageService) {
  }

  ngOnInit() { 
    this.buildForm();
    this.buildViewForm();
    //this.GetbyDepartmentId();
  }

  ngAfterViewInit() {
    this._shareData.seteventnotificationDropdown();
  }

  ngOnChanges(changes: SimpleChanges): void {
    //this.GetbyDepartmentId();
    this.buildForm();
  }

 buildForm(){
   this.systemConfigForm = this._fb.group({
     code: [this.model.code, Validators.required],
     trangThai: [this.model.trangThai, Validators.required],
     tenCauHinh: [this.model.tenCauHinh, Validators.required],
     giaTri: [this.model.giaTri],
     moTa: [this.model.moTa],
     maDonVi: [this.model.maDonVi],
   });
  }

  rebuilForm() {
    this.systemConfigForm.reset({
      code: this.model.code,
      trangThai: this.model.trangThai,
      tenCauHinh: this.model.tenCauHinh,
      giaTri: this.model.giaTri,
      moTa: this.model.moTa,
      maDonVi: this.model.maDonVi,
    });
  }

  buildViewForm() {
    this.viewsystemConfigForm = this._fb.group({
      code: [this.model.code, Validators.required],
      trangThai: [this.model.trangThai, Validators.required],
      tenCauHinh: [this.model.tenCauHinh, Validators.required],
      giaTri: [this.model.giaTri],
      moTa: [this.model.moTa],
      maDonVi: [this.model.maDonVi],
    });
  }

  public buttonCount = 5;
  public info = true;
  public type: 'numeric' | 'input' = 'numeric';
  public pageSizes = true;
  public previousNext = true;

  public dataStateChange(): void {
      this.GetbyDepartmentId();
  }

  public GetbyDepartmentId() {
    //new get adapper
    var request = {
      PageSize: this.pagesize,
      PageIndex: this.pageindex,
      TenCauHinh: this.tencauhinh,
      MaCauHinh: this.maCauhinh
    }

    this._systemConfigService.getSystemConfig(request).subscribe((data) => {
      if (data.status === 1) {
        this.systemConfiglst = data.systemConfigs;
        this.totalRecords = data.totalRecords;
      } else if (data.status === 2) {

      }
    }, error => {
      console.error(error);
      alert(error);
      });
    //this.buildForm();
  }
  passingData(id: number) {
    this.GetBySystemConfigId(id);
    this.buildForm();
  }

  getdata(id: number) {
    this.GetBySystemConfigIdforView(id);
  }

  GetBySystemConfigId(id: number) {
    this._systemConfigService.getSystemConfigById(id).subscribe((data) => {
      if (data.status === 1) {
        this.model = data.systemConfig;
        this.rebuilForm();
        $("#editSystemConfigModal").modal('show');
      } else if (data.status === 2) {

      }
    }, error => {
      console.error(error);
      alert(error);
    });
  }

  GetBySystemConfigIdforView(id: number) {
    this._systemConfigService.getSystemConfigById(id).subscribe((data) => {
      if (data.status === 1) {
        this.model = data.systemConfig;
        this.buildViewForm();
        $("#viewSystemConfigModal").modal('show');
      } else if (data.status === 2) {

      }
    }, error => {
      console.error(error);
      alert(error);
    });
  }
  // convenience getter for easy access to form fields
  get f() { return this.systemConfigForm.controls; }

  onSave() {
    //this.route.params.subscribe(param => console.log(param));
    var data = this.model;
    var request = {
      SystemConfig: data
    }
    this._systemConfigService.updateSystemConfig(request).subscribe(success => {
      if (success.status == 1) {

        this._toast.success("Cập nhật cấu hình hệ thống thành công");
        $("#editSystemConfigModal").modal("hide");
        this.GetbyDepartmentId();
      } else {
        this._toast.error(success.message);
      }
    });
  }

 
  changeDepartment(data: any) {
    this.departmentId = data.data.ma;
    this.dataStateChange();
  }


  public onPageChange(event: any): void {
    this.pagesize = event.rows;
    this.pageindex = (event.first / event.rows) + 1;
    this.GetbyDepartmentId();
  }
}

