import { Component, OnInit, ViewChild } from '@angular/core';
import { DepartmentMenuType } from './departmentMenuType';
import { MenuPassingObject } from './menuPassingObject';
import { DepartmentTree } from '../../../../models/departmentTree';
import { DepartmentsInfoComponent } from './department-info/department-info.component';
import { DepartmentsListComponent } from './departments-list/departments-list.component';
import { EmployeesListComponent } from './employees-list/employees-list.component';
import { DepartmentService } from 'src/app/services/department.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { DepartmentObject } from 'src/app/models/departmentObject';
import { UserService } from '../../../../services/user.service';
import { UserObject } from '../../../../models/UserObject';
import { Router } from '@angular/router';
import { DataService } from '../../../../services/sharedata.service';

// // declare var jquery: any;
declare var $: any;

@Component({
  selector: 'app-orgnization',
  templateUrl: './orgnization.component.html',
  styleUrls: ['./orgnization.component.css']
})
export class OrgnizationComponent implements OnInit {
  FormDepartment: FormGroup;
  type: number;
  departmentInfo: DepartmentTree;
  modelPhongBan: any = new DepartmentObject();
  modelDepartment: any = new DepartmentObject();
  @ViewChild(DepartmentsInfoComponent, {static: false})
  private deptInfoComponent: DepartmentsInfoComponent;
  @ViewChild(EmployeesListComponent, {static: false})
  private employeelist: EmployeesListComponent;
  @ViewChild(DepartmentsListComponent, {static: false})
  private departmentlist: DepartmentsListComponent;
  lstDepartments: any[] = [];
  lstUserThamGia: any = [];
  checkViewDepartment: boolean = false;
  depId: number = -1;
  lstData: any = [];
  lstIsDonVi: any = [{ value: true, text: 'Là Đơn Vị' }, { value: false, text: 'Không là đơn vị' }]
  modelDonVi: any = {};
  gioiTinhText: string = "";
  submitted: boolean = false;
  userInfo: UserObject = new UserObject();;
  isOpenList: boolean = false;
  idDonVi: number = 0;
  lstUser: any = [];
  selectedUser: any = [];
  @ViewChild('table', {static: false}) public table: any;
  lstmaCapCha: any = [];
  searchModel = {
    phongBan: '',
    ten: '',
    departmentId: -1,
    pageIndex: 1,
    pageSize: 20,
    totalRecords: 0,
  }
  isUnit: boolean = false;

  constructor(private _router: Router,
    private service: DepartmentService,
    private _toastr: ToastrService,
    private _shareData: DataService,
    private userService: UserService) { }

  ngOnInit() {
    this.type = DepartmentMenuType.DanhSachNguoiDung;
    this.getDepartmentTree();
    this.buildForm();
  }

  ngAfterViewInit() {
    this._shareData.seteventnotificationDropdown();
  }

  buildForm() {
    this.FormDepartment = new FormGroup({
      'ten': new FormControl(this.modelDepartment.ten, [Validators.required]),
      'sdt': new FormControl(this.modelDepartment.soDienThoai, [Validators.minLength(9)]),
      'code': new FormControl(this.modelDepartment.code, [Validators.required]),
      'email': new FormControl(this.modelDepartment.email, [Validators.email]),
      'description': new FormControl(this.modelDepartment.description, []),
      'nguoiPhuTrachId': new FormControl(this.modelDepartment.nguoiPhuTrachId, []),
      'isDonVi': new FormControl(this.modelDepartment.isDonVi, []),
      'maCapCha': new FormControl(this.modelDepartment.maCapCha, []),
    });
  }

  get f() { return this.FormDepartment.controls; }

  rebuilForm() {
    this.FormDepartment.reset({
      'ten': { value: this.modelDepartment.ten, disabled: this.checkViewDepartment },
      'sdt': { value: this.modelDepartment.soDienThoai, disabled: this.checkViewDepartment },
      'code': { value: this.modelDepartment.code, disabled: this.checkViewDepartment },
      'email': { value: this.modelDepartment.email, disabled: this.checkViewDepartment },
      'description': { value: this.modelDepartment.moTa, disabled: this.checkViewDepartment },
      'nguoiPhuTrachId': { value: this.modelDepartment.nguoiPhuTrachId, disabled: this.checkViewDepartment },
      'isDonVi': { value: this.modelDepartment.isDonVi, disabled: this.checkViewDepartment },
      'maCapCha': { value: this.modelDepartment.maCapCha, disabled: this.checkViewDepartment },
    });
  }

  getDepartmentTree(): void {
    this.service.getTreeDepartment().subscribe(data => {
      data.departmentTree[0].maCapCha = null;
      this.lstDepartments = data.departmentTree;
      this.modelDonVi = data.modelDonVi;
      this.lstmaCapCha = data.lstmaCapCha;
      if (data.modelDonVi.anhDaiDien) {
        this.onLoadImage(data.modelDonVi.anhDaiDien);
      }
      if (this.depId != null && this.depId != -1) {
        this.GetListUser();
      } else {
        this.depId = data.modelDonVi.ma;
        this.GetListUser();
      }
      this.lstUserThamGia = data.lstUserThamGia;
      for (var item of this.lstDepartments) {
        if (item.isDonVi) {
          this.idDonVi = item.ma;
        }
        if (item.level === 2) {
          item.departmentChildren = this.lstDepartments.filter(x => x.maCapCha === item.ma);
        }
      }
    }, error => {
      alert(error);
    });
  }

  preAddDepartment() {
    this.GetDepartments();
    this.isUnit = false;
    this.modelDepartment = new DepartmentObject();
    this.modelDepartment.maCapCha = this.modelDonVi.ma;
    this.checkViewDepartment = false;
    this.submitted = false;
    this.buildForm();
  }

  OnCreateDepartment() {
    this.submitted = true;
    if (this.FormDepartment.invalid) {
      return;
    }
    if (this.validateModel() == false) {
      return;
    }
    var request = {
      Department: this.modelDepartment
    }
    this.service.createDepartment(request).subscribe(data => {
      if (data.status == 1) {
        $("#modalDepartment").modal('hide');
        this._toastr.success("Thêm mới thành công");
        this.getDepartmentTree();
      } else {
        this._toastr.error(data.message);
      }
    }, error => {
      alert(error);
    });
  }

  validateModel() {
    this.modelDepartment.ten = this.modelDepartment.ten.trim();
    this.modelDepartment.code = this.modelDepartment.code.trim();
    if (this.modelDepartment.moTa) {
      this.modelDepartment.moTa = this.modelDepartment.moTa.trim();
    }
    if (this.modelDepartment.email) {
      this.modelDepartment.email = this.modelDepartment.email.trim();
    }
    if (this.modelDepartment.soDienThoai) {
      if (this.modelDepartment.soDienThoai.length < 9) {
        this._toastr.error('Số điện thoại không đúng định dạng!');
        return false;
      }
    }
    if (!this.modelDepartment.isDonVi) {
      if (!this.modelDepartment.maCapCha) {
        this._toastr.error("Đơn vị cấp cha không được để trống");
        return false;
      }
    }
    return true;
  }

  PreDeleteDepartment(ma) {
    this.service.DeleteDepartment(ma).subscribe(data => {
      if (data.status == 1) {
        this._toastr.success("Xóa thành công!");
        this.getDepartmentTree();
      } else {
        this._toastr.error(data.message);
      }
    }, error => {
      alert(error);
    });
  }

  OnUpDateDepartment() {
    this.submitted = true;
    if (this.FormDepartment.invalid) {
      return;
    }
    if (!this.validateModel()) {
      return;
    }
    var request = {
      Department: this.modelDepartment
    }
    this.service.UpdateDepartment(request).subscribe(data => {
      if (data.status == 1) {
        $("#modalDepartment").modal('hide');
        this._toastr.success("Cập nhật thành công!");
        this.getDepartmentTree();
      } else {
        this._toastr.error(data.message);
      }
    }, error => {
      alert(error);
    });
  }

  PreUpDateDepartment(unit) {
    if ((unit.level && unit.level == 1) || unit.isDonVi) {
      this.isUnit = true;
    }
    else {
      this.isUnit = false;
    }
    this.checkViewDepartment = false;
    this.submitted = false;
    this.DepartmentGetbyId(unit.ma);
  }

  PreViewDepartment(ma) {
    this.checkViewDepartment = true;
    this.DepartmentGetbyId(ma);
  }

  DepartmentGetbyId(ma) {
    this.service.DepartmentGetbyId(ma).subscribe(data => {
      if (data.status == 1) {
        this.modelDepartment = data.department;
        this.rebuilForm();
        $("#modalDepartment").modal('show');
      } else {
        this._toastr.error(data.message);
      }
    }, error => {
      alert(error);
    });
  }

  onLoadImage(anhDaiDien) {
    var request = {
      filePath: anhDaiDien,
      Name: anhDaiDien,
    }
    this.service.LoadImage(request).subscribe(data => {
      if (data != undefined) {
        var blob = new Blob([data], { type: data.type });
        var blob_url = URL.createObjectURL(blob);
        var t = document.getElementById('userImageOfUnit');
        t.setAttribute('src', blob_url);
      }
    });
  }

  GetListUserByDepartment(depIdSelected) {
    this.depId = depIdSelected;
    this.searchModel.pageIndex = 1;
    this.searchModel.pageSize = 20;
    this.GetListUser();
    this.getListUserAdd();
  }

  public onPageChange(event: any) {
    this.searchModel.pageSize = event.rows;
    this.searchModel.pageIndex = (event.first / event.rows) + 1;
    this.GetListUser();
  }

  dataStateChange() {
    this.searchModel.pageIndex = 1;
    this.table.first = 0;
    this.GetListUser();
  }

  GetListUser() {
    if (this.modelDonVi.ma) {
      this.searchModel.phongBan = this.searchModel.phongBan.trim();
      this.searchModel.ten = this.searchModel.ten.trim();
      let tenPhongBan = '';
      if (this.depId == this.idDonVi) {
        tenPhongBan = this.searchModel.phongBan;
      }
      var request = {
        PhongBan: tenPhongBan,
        DepartmentId: this.depId,
        Ten: this.searchModel.ten,
        PageIndex: this.searchModel.pageIndex,
        PageSize: this.searchModel.pageSize,
      }
      this.service.DepartmentGetlstUserbyId(request).subscribe(data => {
        if (data.status == 1) {
          this.lstData = [];
          this.lstData = data.lstData;
          this.searchModel.totalRecords = data.totalRecords;
          if (data.departmentInfo) {
            this.modelPhongBan = data.departmentInfo;
          }
        }
        else {
          this._toastr.error(data.message);
        }
      }), error => {
        console.error(error);
      }
    }
  }

  onSaveUserDepartment(data: MenuPassingObject) {
    this.type = data.type;
    this.departmentInfo = data.data;
    this.departmentlist.selectedId = this.departmentInfo.ma;
    this.employeelist.getlistUser();
  }

  previewInfo() {
    var url = "#" + this._router.url + "/report-view/" + "0" + "/DepartmentUser";
    $("#modalPrint").modal('hide');
    window.open(url, '_blank');
  }

  GetUserInfo(ma) {
    var request = {
      UserId: ma,
    }
    this.userService.getUserById(request).subscribe(response => {
      if (response.status == 1) {
        this.userInfo = response.user;
        if (this.userInfo.anhDaiDien != null && this.userInfo.anhDaiDien != "") {
          this.loadImage(this.userInfo.anhDaiDien);
        }
        if (response.user.gioiTinh == true) {
          this.gioiTinhText = "Nam";
        } else {
          this.gioiTinhText = "Nữ";
        }
        $('#modalShowUserInfo').modal('show');
      }
      else {
        this._toastr.error(response.message);
      }
    }, error => {
      console.error(error);
      alert(error);
    });
  }

  loadImage(path: string) {
    let request = {
      filePath: path
    }
    this.userService.LoadImage(request).subscribe(response => {
      if (response != undefined && response != null) {
        var blob = new Blob([response], { type: response.type });
        var url = URL.createObjectURL(blob);
        if (url != null) {
          var avatar = document.getElementById("anhDaiDienNguoiDungAtDepartment");
          avatar.setAttribute("src", url);
        }
      }
    })
  }

  RemoveUserFromDepartment(ma) {
    this.depId = this.modelPhongBan.ma;
    var request = {
      MaNguoiDung: ma,
      MaDonVi: this.modelPhongBan.ma,
    }
    this.service.RemoveUserFromDepartment(request).subscribe(response => {
      if (response.status == 1) {
        this._toastr.success("Xóa thành công!");
        this.GetListUser();
      }
      else {
        this._toastr.error(response.message);
      }
    }, error => {
      console.error(error);
      alert(error);
    });
  }

  NavagateToUser(maUser) {
    $('#modalShowUserInfo').modal('hide');
    this._router.navigate(['business/system-management/user/edit-user/' + maUser]);
  }

  openList() {
    var elemnt = document.getElementById('department-list');
    var content = document.getElementById('department-content');
    if (!this.isOpenList) {
      elemnt.style.width = '100%';
      elemnt.style.transition = 'width 1s ease';
      content.style.width = '0';
      content.style.transition = 'width 1s ease';
      this.isOpenList = true;
    }
    else {
      elemnt.style.width = '0';
      elemnt.style.transition = 'width 1s ease';
      content.style.width = '100%';
      content.style.transition = 'width 1s ease';
      this.isOpenList = false;
    }
  }

  UpdateStatus(nguoiDung) {
    const kickHoat = nguoiDung.kichHoat;
    var request = {
      Ma: nguoiDung.ma
    }
    this.userService.UserUpdateStatus(request).subscribe(res => {
      if (res.status == 1) {
        if (kickHoat) {
          this._toastr.success('Kích hoạt thành công!');
        }
        else {
          this._toastr.success('Khóa thành công!');
        }
      }
      else {
        this._toastr.error(res.message);
      }
    }), err => {
      console.error(err);
    }
  }

  getListUserAdd() {
    var request = {
      Id: this.depId
    }
    this.service.GetListUserOverDepartmentId(request).subscribe(res => {
      if (res.status == 1) {
        this.lstUser = [];
        this.lstUser = res.listDropDown;
      }
      else {
        this._toastr.error(res.message);
      }
    }), err => {
      console.error(err);
    }
  }

  onAddUser() {
    this.selectedUser = [];
    $("#modal-add-user").modal("show");
  }

  onSaveAdd() {
    if (this.lstUser.length == 0) {
      $("#modal-add-user").modal("hide");
      return;
    }
    if (this.selectedUser.length == 0) {
      this._toastr.error('Người dùng không được để trống!');
      return;
    }
    var request = {
      DepartmentId: this.depId,
      Users: this.selectedUser,
    }
    this.service.DepartmentAddUser(request).subscribe(res => {
      if (res.status == 1) {
        this._toastr.success('Thêm thành công!');
        $("#modal-add-user").modal("hide");
        this.GetListUser();
        this.getListUserAdd();
      }
      else {
        this._toastr.error(res.message);
      }
    }), err => {
      console.error(err);
    }
  }

  public GetDepartments() {
    this.service.GetDepartments({}).subscribe((res) => {
      if (res.status == 1) {
        this.lstmaCapCha = [];
        this.lstmaCapCha = res.listDropDown;
      }
      else {
        this._toastr.error(res.message);
      }
    }), (err) => {
      console.error(err);
    }
  }
}
