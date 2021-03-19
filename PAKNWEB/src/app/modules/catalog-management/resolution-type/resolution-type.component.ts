import { Component, OnInit, ViewChild} from '@angular/core';
// import {TreeTableModule} from 'primeng/treetable';
import {TreeNode} from 'primeng/api';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { CatalogService } from '../../../services/catalog.service';
import { resolutionTypeObject, modelSearchResolutionTypeObject } from 'src/app/models/resolutionTypeObject';
import { COMMONS } from 'src/app/commons/commons';

declare var $: any;

@Component({
  selector: 'app-resolution-type',
  templateUrl: './resolution-type.component.html',
  styleUrls: ['./resolution-type.component.css']
})
export class ResolutionTypeComponent implements OnInit {
  listresolutionTypeObject: TreeNode[];
  form: FormGroup;
  model: any = new resolutionTypeObject();
  submitted: boolean = false;
  @ViewChild("tableResolutionType", {static: false}) tableResolutionType: any;
  totalRecords: number = 0;
  listcapChaId: any = [];
  title: string = "";
  modelSearch: any = new modelSearchResolutionTypeObject();
  listTrangThai = COMMONS.LIST_TRANG_THAI;
  totalRecordsForPaginator: number = 0;

  constructor(private _service: CatalogService,
    private _toastr: ToastrService,
    private _fb: FormBuilder,
  ) { }
  ngOnInit() {
    this.buildForm();
    this.GetlistcapChaId(0);
  }


  getDataByParentId(data, parentId, expaned): any {
    const result = data
      .filter(d => d.parentId === parentId);

    if (!result && !result.length) {
      return null;
    }
    return result.map(({ id, name, parentId, code, description, status }) =>
      ({

        id,
        label: name,
        capChaId: parentId, expanded: expaned,
        data: { id, name, parentId, code, description, status, expanded: expaned },
        children: this.getDataByParentId(data, id, expaned)
      }));
  };

  GetlistcapChaId(id) {
    this.listcapChaId = [];
    this._service.GetlistcapChaId({Id: id, Type: 1}).subscribe(response => {
      if (response.status == 1) {
        this.listcapChaId = response.listcapChaId;
      }
      else {
        this._toastr.error(response.message);
      }
    }), error => {
      console.log(error);
      alert(error);
    }
    
  }
  

  onNodeExpand(event) {
   // this._systemConfigService.getChildNode(event.node.id).subscribe(success => {
      //if (success.length > 0) {
      //  for (var ct of success) {
      //    ct.children = [];
      //    ct.label = ct.tenDuLieu;
      //    ct.leaf = false;
      //    ct.data = { id: ct.id, tenDuLieu: ct.tenDuLieu, capChaId: event.node.id, code: ct.code, stt: ct.stt, moTa: ct.moTa, trangThai: ct.trangThai, type: ct.type, expanded: false, leaf: true, children: [] }
      //  }
      //  event.node.children = success;
      //  this.lstCatalogValue = [...this.lstCatalogValue];
      //}

   // });
  }


  get f() { return this.form.controls; }

  buildForm() {
    this.form = this._fb.group({
      code: [this.model.code, Validators.required],
      name: [this.model.name, Validators.required],
      description: [this.model.description],
      status: [this.model.status, Validators.required],
      parentId: [this.model.parentId],
    });
  }

  rebuilForm() {
    this.form.reset({
      code: this.model.code,
      name: this.model.name,
      status: this.model.status,
      description: this.model.description,
      parentId: this.model.parentId,
    });
  }

  getList(expaned) {
    if (this.modelSearch.code != null && this.modelSearch.code != "" && this.modelSearch.code != undefined) {
      this.modelSearch.code = this.modelSearch.code.trim();
    }
    if (this.modelSearch.name != null && this.modelSearch.name != "" && this.modelSearch.name != undefined) {
      this.modelSearch.name = this.modelSearch.name.trim();
    }
    let request = {
      Name: this.modelSearch.name,
      Code: this.modelSearch.code,
      Status: this.modelSearch.status,
      PageIndex: this.modelSearch.pageIndex,
      PageSize: this.modelSearch.pageSize,
    }
    this.listresolutionTypeObject = [];
    this._service.resolutionTypeGetList(request).subscribe(response => {
      if (response.status == 1) {
        this.listresolutionTypeObject = this.getDataByParentId(response.listResolutionType, null, expaned);
        this.totalRecords = response.totalRecords;
        this.totalRecordsForPaginator = response.totalRecordsForPaginatorl;
      }
      else {
        this._toastr.error(response.message);
      }
    }), error => {
      console.log(error);
      alert(error);
    }
  }

  onPageChange(event: any) {
    this.modelSearch.pageSize = event.rows;
    this.modelSearch.pageIndex = (event.first / event.rows) + 1;
    this.getList(true);
  }

  dataStateChange() {
    this.modelSearch.pageIndex = 1;
    this.tableResolutionType.first = 0;
    this.getList(true);
  }

  preCreate() {
    this.model = new resolutionTypeObject();
    this.rebuilForm();
    this.submitted = false;
    this.GetlistcapChaId(0);
    this.title = "Thêm mới loại nghị quyết";
    $('#modal').modal('show');
  }

  validateModel() {
    if (this.model.code != "" && this.model.code != null && this.model.code != undefined) {
      this.model.code = this.model.code.trim();
    }
    if (this.model.name != "" && this.model.name != null && this.model.name != undefined) {
      this.model.name = this.model.name.trim();
    }
    if (this.model.description) {
      this.model.description = this.model.description.trim();
    }
    //if (this.model.code == '') {
    //  $('#code').focus();
    //  return false;
    //}
    //if (this.model.name == '') {
    //  $('#ten').focus();
    //  return false;
    //}
    return true;
  }

  onSave() {
    this.submitted = true;
    if (this.form.invalid) {
      return;
    }
    if (this.model.stt != null && this.model.stt <= 0 && this.model.stt.toString() != '') {
      this.model.stt = 1;
    }
    if (this.validateModel() == false) {
      return;
    }
    let request = {
      CatalogObject: this.model,
      Type: 11
    };
    if (this.model.id == 0 || this.model.id == null) {
      this._service.CatalogCreate(request).subscribe(response => {
        if (response.status == 1) {
          $("#modal").modal("hide");
          this._toastr.success("Thêm mới thành công");
          this.getList(true);
        } else {
          this._toastr.error(response.message);
        }
      }), error => {
        console.error(error);
        alert(error);
      }
    }
    else {
      this._service.CatalogUpdate(request).subscribe(response => {
        if (response.status == 1) {
          $("#modal").modal("hide");
          this._toastr.success("Cập nhật thành công");
          this.getList(true);
        } else {
          this._toastr.error(response.message);
        }
      }), error => {
        console.error(error);
        alert(error);
      }
    }
  }

  preUpdate(data) {
  this.GetlistcapChaId(0);
    let request = {
      Id: data.id,
      Type: 11,
    }
    this._service.CatalogGetById(request).subscribe(response => {
      if (response.status == 1) {
        this.rebuilForm();
        this.title = "Chỉnh sửa loại nghị quyết";
        this.model = response.catalog;
        $('#modal').modal('show');
      }
      else {
        this._toastr.error(response.message);
      }
    }), error => {
      console.error(error);
      alert(error);
    }
  }

  onDelete(data) {
    let request = {
      Type: 11,
      Id: data.id
    }
    this._service.DeleteCatalog(request).subscribe(response => {
      if (response.status == 1) {
        this._toastr.success("Xóa thành công");
        this.getList(true);
      }
      else {
        this._toastr.error(response.message);
      }
    }), error => {
      console.error(error);
    }
  }

  preCreateInList(data){
    this.model = new resolutionTypeObject();
    this.model.parentId = data.id;
    this.GetlistcapChaId(0);
    this.rebuilForm();
    this.submitted = false;
    this.title = "Thêm mới loại nghị quyết";
    $('#modal').modal('show');
}

  onUpdateStatus(data) {
    var status = data.status;
    let request = {
      Type: 11,
      Id: data.id,
      Status: data.status,
    }
    this._service.UpdateStatus(request).subscribe(res => {
      if (res.status == 1) {

        this.getList(true);
        if (status) {
          this._toastr.success("Mở khóa thành công");
        }
        else {
          this._toastr.success("Khóa thành công");
        }
      }
      else {
        this._toastr.error(res.message);
      }
    }), error => {
      console.error(error);
    }
  }

  preView(data) {
    this.model = data;
    $('#modalDetail').modal('show');
  }

  onEdit() {
    $('#modalDetail').modal('hide');
    this.preUpdate(this.model);
  }

}
