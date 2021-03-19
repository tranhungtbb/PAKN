import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from '../../../../services/authentication.service';
import { UserInfoStorageService } from '../../../../commons/user-info-storage.service';
import { LoginUserObject } from '../../../../models/loginUserObject';
import { ToastrService } from 'ngx-toastr';
import { DataService } from '../../../../services/sharedata.service';

declare var $: any;

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})

export class LoginComponent implements OnInit {

  user: LoginUserObject = {
    UserName: '',
    Password: ''
  };
  isAbleCaptcha: any = '';
  isSaveLogin: boolean = false;
  loginForm: FormGroup;
  lang: any = 'vi';
  theme: any = 'light';
  size: any = 'normal';
  type: any = 'image';
  siteKey: any = '6LdcUHgUAAAAAOoPO7q4P2s4YFU2N3khp4DBf3Dh';
  submitted: boolean = false;
  configcaptcha: any = {
    theme: this.theme,
    type: this.type,
    size: this.size,
    tabindex: 3
  };


  constructor(
    private _fb: FormBuilder,
    private _avRoute: ActivatedRoute,
    private authenService: AuthenticationService,
    private _router: Router,
    private storeageService: UserInfoStorageService,
    private toastr: ToastrService, private http: HttpClient,
    private shareData: DataService
  ) {
    this.loginForm = new FormGroup({
      'name': new FormControl(this.user.UserName, [
        Validators.required,
      ]),
      'pass': new FormControl(this.user.Password, [
        Validators.required
      ]),
      'isRemember': new FormControl(this.isSaveLogin, [])
    });
    if (this.storeageService.getAccessToken()) {
      var ReturnlUrl = this.storeageService.getReturnUrl();
      if (ReturnlUrl != undefined && ReturnlUrl != '' && ReturnlUrl != null && ReturnlUrl.includes("business")) {
        this._router.navigateByUrl(ReturnlUrl);
      } else {
        this._router.navigate(['/business']);
      }
      return;
    }
  }

  ngOnInit(): void {
    this.submitted = false;
  }

  login() {
    localStorage.clear();
    this.submitted = true;
    if (this.loginForm.invalid) {
      if (this.loginForm.controls.name.status == "INVALID") {
        $("#name").focus();
        return;
      }
      if (this.loginForm.controls.pass.status == "INVALID") {
        $("#pass").focus();
        return;
      }

      return;
    } else {
      var model = {
        Userlg: this.user
      }
      this.authenService.login(model).subscribe((data) => {
        if (data.status === 1) {
          this.shareData.setIsLogin(true);
          this.storeageService.setUserName(data.userName);
          this.storeageService.setAccessToken(data.accessToken);
          this.storeageService.setUserId(data.userId);
          this.storeageService.setIsSuperAdmin(data.isSuperAdmin);
          this.storeageService.setDeparmentId(data.deparmentId);
          this.storeageService.setUnitId(data.unitId);
          this.storeageService.setAccountId(data.accountId);
          this.storeageService.setPermissionCategories(data.permissionCategories);
          this.storeageService.setFunctions(data.functions);
          this.storeageService.setPermissions(data.permissions);
          this.storeageService.setUnitName(data.unitName);
          this.storeageService.setSaveLogin(this.isSaveLogin);
          this.storeageService.setIsHaveToken(data.isHaveToken);
          this.storeageService.setRole(data.role);
          this.storeageService.setFullName(data.fullName);
          this.storeageService.setEmail(data.email);
          localStorage.setItem('anhDaiDien', data.anhDaiDien);
          if (this.isSaveLogin) {
            this.storeageService.setKeyRemember(btoa(this.user.Password));
          } else {
            this.storeageService.setKeyRemember("");
          }
          this.http.get<{ ip: string }>('https://jsonip.com/')
            .subscribe(data => {
              if (data != null) {
                this.storeageService.setIpAddress(data.ip);
              }
            })
          this._router.navigate(['/business']);

        } else if (data.status === 2) {
          this.toastr.error(data.message, "Đăng nhập thất bại");
        }
      }, error => {
        console.error(error);
        alert(error);
      });
    }
  }

  get name() { return this.loginForm.get('name'); }

  get pass() { return this.loginForm.get('pass'); }

  forgetPassWord(): void {
    this._router.navigate(['/forgotPass']);
  }

  handleExpire() {
    console.log("Hết hạn");
  }

  handleLoad() {
    console.log("đang load");
  }

  handleSuccess(data) {
    this.isAbleCaptcha = data;
  }
}
