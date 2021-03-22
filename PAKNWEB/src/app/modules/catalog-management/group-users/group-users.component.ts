import { Component, OnInit, ViewChild } from '@angular/core';
import { DepartmentTree } from '../../../models/departmentTree';
import { UserInfoStorageService } from '../../../commons/user-info-storage.service';
import { DocumentTypeObject } from '../../../models/documentTypeObject';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { SystemconfigService } from '../../../services/systemconfig.service';
import { ToastrService } from 'ngx-toastr';
import { GroupUsersListComponent } from './group-users-list/group-users-list.component';
import { DataService } from '../../../services/sharedata.service';
declare var $: any;
@Component({
  selector: 'app-group-users',
  templateUrl: './group-users.component.html',
  styleUrls: ['./group-users.component.css']
})
export class GroupUsersComponent implements OnInit {

  createForm: FormGroup;
  data: DocumentTypeObject = new DocumentTypeObject();
  type: number;
  departmentId: number;
  departmentInfo: DepartmentTree;
  submitted: boolean = false;
  @ViewChild(GroupUsersListComponent, {static: false})
  private _loadView: GroupUsersListComponent;
  constructor(private _fb: FormBuilder,
    private _systemConfigService: SystemconfigService,
    private _toast: ToastrService,
    private _shareData: DataService,
    private localStorage: UserInfoStorageService) { }

  ngOnInit() {
    this.buildForm();
  }
  ngAfterViewInit() {
    this._shareData.seteventnotificationDropdown();
  }

  get f() { return this.createForm.controls; }

  buildForm() {
    this.createForm = this._fb.group({
      code: ['', Validators.required],
      tenDuLieu: ['', Validators.required],
      moTa: [''],
      trangThai: true,
      loai: true,
      stt: ['', Validators.required],
    });
  }

  rebuilForm() {
    this.createForm.reset({
      code: '',
      tenDuLieu: '',
      trangThai: true,
      loai: true,
      moTa: '',
      stt: '',
    });
  }


  preCreate() {
    //$("#modalCreate").modal('show');
    this.data = new DocumentTypeObject();
    this.rebuilForm();
  }
  onCreate() {
    this.submitted = true;
    if (this.data.tenDuLieu.trim() == '') {
      this.data.tenDuLieu = this.data.tenDuLieu.trim();
      return;
    }
    if (this.data.code.trim() == '') {
      this.data.code = this.data.code.trim();
      return;
    }


    this.data.code = this.data.code.trim();
    this.data.tenDuLieu = this.data.tenDuLieu.trim();
    this.data.moTa = this.data.moTa.trim();
    this.data.trangThai = this.data.trangThai;
    this.data.loai = this.data.loai;
    this.data.stt = this.data.stt;
    if (this.createForm.invalid) {
      return;
    }
    this.data.id = 0;
    this.data.xoa = false;
    this._systemConfigService.createCatalogValue(this.data, 14).subscribe(success => {
      if (success.status == 1) {
        this._toast.success("Thêm mới thành công");
        $("#modalCreate").modal("hide");
        this._loadView.pageindex = 1;
        this._loadView.dataTable.first = 0;
        this._loadView.GetDocumentTypeDepartmentId();
        this.buildForm();
      } else {
        this._toast.error(success.message);
      }
    });
  }
  onMainDepartmentMenuClick(data: any) {

    this.departmentId = data;
  }

  previewInfo() {
    var url = "#" + "/business/system-management/organization/report-view/" + "0" + "/GroupUsersReport";
    $("#modalPrint").modal('hide');
    window.open(url, '_blank');
  }

}
