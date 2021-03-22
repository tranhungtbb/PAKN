import { Component, OnInit, HostListener } from '@angular/core';
import { UserInfoStorageService } from '../../commons/user-info-storage.service';
import { AuthenticationService } from '../../services/authentication.service';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { ChangePasswordUserObject } from '../../models/changePasswordUserObject';
import { ToastrService } from 'ngx-toastr';
import { UserObject } from '../../models/UserObject';
import { UserService } from '../../services/user.service';
import { DataService } from '../../services/sharedata.service';
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS';

declare var $: any;
@HostListener("window:scroll", ["$event"])

@Component({
  selector: 'app-appheader',
  templateUrl: './appheader.component.html',
  styleUrls: ['./appheader.component.css']
})
export class AppheaderComponent implements OnInit {
  totalThongBao: number = 0;
  myDate: any;
  myHours: any;
  pageindex: number;
  listPageIndex: any[] = [];
  pageSizeGrid: number = 10;
  files: any;
  updateForm: FormGroup;
  userForm: FormGroup;
  userName: string;
  errorMessage: any;
  year: Date = new Date();
  notifications: any[] = [];
  mindateUyQuyen: Date = new Date();
  exchange: FormGroup;
  lstUserbyDep: Array<any> = [];
  lstTimline: Array<any> = [];
  lstTimlineMore: Array<any> = [];
  model: UserObject = new UserObject();
  submitted: boolean = false;
  listSexs: any[] = [{ text: 'Nam', value: true }, { text: 'Nữ', value: false }];
  lstDropDownLayout = [{ 'value': 1, 'Text': 'Thành công' }, { 'value': 0, 'Text': 'Thất bại' }]
  public timeOut: number = 1;
  public exchangedata: any = {};
  listThongBao: any = [];
  formData = new FormData();
  showLoader: boolean = true;
  tenDonVi: string = "";
  remindWork: any = {};
  minDate: Date = new Date();

  lstChucVu: any = [];
  lstPhongBan: any = [];

  constructor(private formBuilder: FormBuilder,
    private router: Router,
    private userService: UserService,
    private storageService: UserInfoStorageService,
    private authenService: AuthenticationService,
    private _router: Router,
    private _fb: FormBuilder,
    private toastr: ToastrService,
    private sharedataService: DataService,
  ) {
  }

  user: ChangePasswordUserObject = {
    OldPassword: '',
    NewPassword: '',
    ConfirmPassword: '',
    LoginId: ''
  };

  ngOnInit() {
    this.buildForm();

    this.userName = this.storageService.getFullName();
    this.userForm = new FormGroup({
      'oldpassword': new FormControl(this.user.OldPassword, [
        Validators.required,
      ]),
      'newpassword': new FormControl(this.user.NewPassword, [
        Validators.required,
      ]),
      'confirmpassword': new FormControl(this.user.ConfirmPassword, [
        Validators.required,
      ])
    });
    this.sharedataService.getnotificationDropdown.subscribe((data) => {
      if (data) {
        var result: any = data;
        this.listThongBao = [];
        this.listThongBao = result.notifications;
        this.totalThongBao = result.totalRecords;
      }
    })

  }

  get f() { return this.updateForm.controls; }

  buildForm() {
    this.updateForm = this._fb.group({
      hoTen: ['', [Validators.required]],
      tenDangNhap: [''],
      dienThoai: [''],
      gioiTinh: [true, Validators.required],
      homThu: ['', [Validators.required, Validators.email]],
      diaChi: [''],
      isHaveToken: [''],
      maChucVu: [this.model.maChucVu, [Validators.required]],
      listPhongBan: ['', [Validators.required]]
    });
  }

  reBuildForm() {
    this.submitted = false;
    this.updateForm = this._fb.group({
      hoTen: [this.model.hoTen, Validators.required],
      tenDangNhap: [this.model.tenDangNhap, Validators.required],
      dienThoai: this.model.dienThoai,
      gioiTinh: [this.model.gioiTinh, Validators.required],
      homThu: [this.model.homThu, Validators.required],
      diaChi: this.model.diaChi,
      isHaveToken: this.model.isHaveToken,
      maChucVu: [this.model.maChucVu, Validators.required],
      listPhongBan: [this.model.listPhongBan, Validators.required],
    });
  }

  Show() {
    this.submitted = false;
    this.userForm = new FormGroup({
      'oldpassword': new FormControl(this.user.OldPassword, [
        Validators.required,
      ]),
      'newpassword': new FormControl(this.user.NewPassword, [
        Validators.required,
      ]),
      'confirmpassword': new FormControl(this.user.ConfirmPassword, [
        Validators.required,
      ])
    });
  }

  Save() {
    this.submitted = true;
    if (this.userForm.controls.oldpassword.status == "INVALID") {
      $("#oldpassword").focus();
    }
    else if (this.userForm.controls.newpassword.status == "INVALID") {
      $("#newpassword").focus();
    }
    else {
      $("#confirmpassword").focus();
    }
    if (this.userForm.invalid) {
      return;
    }
    var data = this.userForm.value;
    this.user = data;
    this.authenService.chagepassword(this.user).subscribe((data) => {
      if (data.status === 1) {
        $("#myModal").modal("hide");
        this.toastr.success('Thay đổi mật khẩu thành công');
      } else {
        this.toastr.error(data.message);
      }
    }), err => {
      console.error(err);
    }
  }

  preUpdate() {
    this.GetListChucVuAndListPhongBan();
    let request = {
    }
    this.userService.getUserById(request).subscribe(response => {
      if (response.status == 1) {
        //this.buildForm();
        this.reBuildForm();
        this.submitted = false;
        this.model = response.user;
        if (this.model.anhDaiDien) {
          this.loadImage(this.model.anhDaiDien);
        }
        $('#modalUpdate').modal('show');
      }
      else {
        this.toastr.error(response.message);
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
          var avatar = document.getElementById("anhDaiDien");
          avatar.setAttribute("src", url);
        }
      }
    })
  }

  selectImage(event: any) {
    if (event) {
      const file = event.target.files[0];
      this.files = file;
      const reader = new FileReader();
      reader.onload = () => {
        var avatar = document.getElementById("anhDaiDien");
        avatar.setAttribute("src", reader.result.toString());
      }
      reader.readAsDataURL(file);
    }
  }

  onUpdate() {
    this.submitted = true;
    this.model.hoTen = this.model.hoTen.trim();
    this.model.diaChi = this.model.diaChi.trim();
    this.updateForm.controls.hoTen.setValue(this.updateForm.controls.hoTen.value.trim());
    if (this.updateForm.invalid && this.updateForm.controls['tenDangNhap'].valid != false) {
      return;
    }
    if (this.model.gioiTinh == null) {
      this.toastr.error("Giới tính không để trống");
      return;
    }
    let fullName = this.model.hoTen;
    var request = {
      User: this.model
    }
    this.userService.updateUserLogin(request, this.files).subscribe(success => {
      if (success.status == 1) {
        $("#modalUpdate").modal('hide');
        this.storageService.setIsHaveToken(this.model.isHaveToken);
        this.storageService.setFullName(fullName);
        this.toastr.success("Cập nhật thành công");
        this.userName = fullName;
      } else if (success.status == 3) {
        this.toastr.error("Email đã tồn tại");
      }
      else {
        this.toastr.error(success.message);
      }
    });
  }

  signOut(): void {
      this.userService.logOut({
        UserId: this.model.ma
      }).subscribe(success => {
        if (success.success == RESPONSE_STATUS.success) {
          this.sharedataService.setIsLogin(false);
          this.storageService.setReturnUrl('');
          this.storageService.clearStoreage();
          this._router.navigate(['/login']);
        }
      });
  }

  get newpassword() { return this.userForm.get('newpassword'); }
  get oldpassword() { return this.userForm.get('oldpassword'); }
  get confirmpassword() { return this.userForm.get('confirmpassword'); }

  expandMenu() {
    if ($("body").hasClass("sidebar-collapse") || $("body").hasClass("sidebar-open")) {
      $("body").removeClass("sidebar-collapse sidebar-open");
    } else {
      $("body").addClass("sidebar-collapse sidebar-open");
    }
  }

  get controlform() { return this.exchange.controls; }

  // getNotification() {
  //   var request = {
  //     PageSize: this.pageSizeGrid
  //   }
  //   this.notificationService.GetNotification(request).subscribe(response => {
  //     if (response.status == 1) {
  //       this.listThongBao = [];
  //       this.listThongBao = response.listThongBao;
  //       this.totalThongBao = response.total;
  //     } else {
  //       this.toastr.error(response.message);
  //     }
  //   }), err => {
  //     console.error(err);
  //   }
  // }

  
  goLink(type, id) {
    if (type == 1 || type == 2 || type == 3) {
      this.router.navigate([`/business/business-management/view-meeting/${id}`]);
    }
    else if (type == 4) {
      this.router.navigate([`/business/business-management/contact-view/${id}`]);
    }
    else if (type == 5 || type == 6) {
      this.router.navigate([`/business/business-management/resolution/list/${id}`]);
    }
    else if (type == 7 || type == 8) {
      this.router.navigate([`/business/business-management/letters-view/${id}`]);
    }
    else if (type == 9 || type == 10 || type == 11) {
      this.router.navigate([`/business/business-management/monitoring/view/${id}`]);
    }
    else if (type == 12) {
      this.router.navigate([`/business/business-management/feedback/meeting/view/${id}`]);
    }
    else if (type == 15) {
      this.router.navigate([`/business/business-management/feedback/petition/view/${id}`]);
    }
    else if (type == 16) {
      this.router.navigate([`/business/business-management/feedback/letters/view/${id}`]);
    }
    else if (type == 13 || type == 14) {
      this.router.navigate([`/business/business-management/records-view/${id}`]);
    }
    else if (type == 17) {
      this.sharedataService.setQuestionId(id);
      this.router.navigate(['/business/business-management/question/list']);
    }
    else if (type == 18 || type == 19 || type == 20) {
      this.router.navigate([`/business/business-management/session/view/${id}`]);
    }
  }

  onScroll(event: any) {
    if (event.target.offsetHeight + event.target.scrollTop >= event.target.scrollHeight) {
      if (this.pageSizeGrid < this.totalThongBao) {
        this.pageSizeGrid = this.pageSizeGrid + 10;
        // this.getNotification();
      }
    }
  }

  GetListChucVuAndListPhongBan() {
    this.userService.GetListChucVuAndListPhongBan({}).subscribe((res) => {
      if (res.status == 1) {
        this.lstChucVu = [];
        this.lstPhongBan = [];
        this.lstChucVu = res.lstChucVu;
        this.lstPhongBan = res.lstPhongBan;
      }
      else {
        this.toastr.error(res.message);
      }
    }), (err) => {
      console.error(err);
    }
  }

  isInvalidNam(event) {
    let test = event;

    if (test == 'Invalid Date') {
      event = new Date();
    }
  }

}
