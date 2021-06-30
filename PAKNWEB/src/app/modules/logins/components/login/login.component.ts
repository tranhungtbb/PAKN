import { Component, OnInit } from '@angular/core'
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms'
import { HttpClient } from '@angular/common/http'
import { ActivatedRoute, Router } from '@angular/router'
import { AuthenticationService } from '../../../../services/authentication.service'
import { UserInfoStorageService } from '../../../../commons/user-info-storage.service'
import { LoginUserObject } from '../../../../models/loginUserObject'
import { ToastrService } from 'ngx-toastr'
import { DataService } from '../../../../services/sharedata.service'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { AppSettings } from 'src/app/constants/app-setting'
import { Api } from 'src/app/constants/api'
import { CaptchaService } from 'src/app/services/captcha-service'
import {Parterms} from 'src/app/constants/parterm'

declare var $: any

@Component({
	selector: 'app-login',
	templateUrl: './login.component.html',
	styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
	user: LoginUserObject = {
		UserName: '',
		Password: '',
	}
	userProduct: LoginUserObject = {
		UserName: '',
		Password: '',
	}
	isShowPassword : boolean = false
	typeInput : any = 'text'
	isAbleCaptcha: any = ''
	isSaveLogin: boolean = true
	loginForm: FormGroup
	loginFormProduct: FormGroup
	lang: any = 'vi'
	theme: any = 'light'
	size: any = 'normal'
	type: any = 'image'
	siteKey: any = '6LdcUHgUAAAAAOoPO7q4P2s4YFU2N3khp4DBf3Dh'
	submitted: boolean = false
	submittedProduct: boolean = false
	configcaptcha: any = {
		theme: this.theme,
		type: this.type,
		size: this.size,
		tabindex: 3,
	}

	constructor(
		private _fb: FormBuilder,
		private _avRoute: ActivatedRoute,
		private authenService: AuthenticationService,
		private _router: Router,
		private storeageService: UserInfoStorageService,
		private captchaService: CaptchaService,
		private toastr: ToastrService,
		private http: HttpClient,
		private shareData: DataService
	) {
		this.loginForm = new FormGroup({
			name: new FormControl(this.user.UserName, [Validators.email]),
			pass: new FormControl(this.user.Password, [Validators.required]),
			captcha: new FormControl(this.captchaCode, [Validators.required]),
			isRemember: new FormControl(this.isSaveLogin, []),
		})
		this.loginFormProduct = new FormGroup({
			name: new FormControl(this.userProduct.UserName, [Validators.required]),
			pass: new FormControl(this.userProduct.Password, [Validators.required]),
			captcha: new FormControl(this.captchaCodeProduct, [Validators.required]),
			isRemember: new FormControl(this.isSaveLogin, []),
		})
		this.http.get<{ ip: string }>('https://jsonip.com/').subscribe((data) => {
			if (data != null) {
				this.storeageService.setIpAddress(data.ip)
			}
		})
		if (this.storeageService.getAccessToken()) {
			var ReturnlUrl = this.storeageService.getReturnUrl()
			if (ReturnlUrl != undefined && ReturnlUrl != '' && ReturnlUrl != null && ReturnlUrl.includes('business')) {
				this._router.navigateByUrl(ReturnlUrl)
			} else {
				if (this.storeageService.getTypeObject() && this.storeageService.getTypeObject() == 1) {
					this._router.navigate(['/quan-tri'])
				} else {
					this._router.navigate(['/cong-bo'])
				}
			}
			return
		}
	}

	ngOnInit(): void {
		this.submitted = false
		this.submittedProduct = false
		this.reloadImage()
	}

	get floginForm() {
		return this.loginForm.controls
	}

	rebuildFormProduct(){
		this.loginFormProduct.reset({
			name: this.userProduct.UserName,
			pass: this.userProduct.Password,
			captcha: this.captchaCodeProduct,
			isRemember: this.isSaveLogin
		})
	}
	rebuildForm(){
		this.loginForm.reset({
			name: this.user.UserName,
			pass: this.user.Password,
			captcha: this.captchaCode,
			isRemember: this.isSaveLogin
		})
	}

	login() {
		this.submitted = true
		this.user.UserName = this.user.UserName.trim()
		this.user.Password = this.user.Password.trim()
		if (this.loginForm.invalid) {
			if (this.loginForm.controls.name.status == 'INVALID') {
				$("input[id='username']").focus()
				return
			}
			if (this.loginForm.controls.pass.status == 'INVALID') {
				$("input[id='userpass']").focus()
				return
			}

			return
		} else {
			var constdata = {
				CaptchaCode: this.captchaCode,
			}
			this.captchaService.send(constdata).subscribe((result) => {
				if (result.success === RESPONSE_STATUS.success) {
					this.authenService.login(this.user).subscribe(
						(data) => {
							if (data.success === RESPONSE_STATUS.success) {
								if (data.isActive == false) {
									this.reloadImage()
									this.captchaCodeProduct = ''
									this.submitted = false
									this.rebuildForm()
									this.toastr.error('Tài khoản của bạn đang hết hiệu lực')
									return
								}
								this.storeageService.clear()
								this.shareData.setIsLogin(true)
								this.storeageService.setAccessToken(data.accessToken)
								this.storeageService.setUserId(data.userId)
								this.storeageService.setPermissionCategories(data.permissionCategories)
								this.storeageService.setFunctions(data.permissionFunctions)
								this.storeageService.setPermissions(data.permissions)
								this.storeageService.setUnitId(data.unitId)
								this.storeageService.setUnitName(data.unitName)
								this.storeageService.setIsMain(data.isMain)
								this.storeageService.setSaveLogin(this.isSaveLogin)
								this.storeageService.setIsSession(true)
								this.storeageService.setIsHaveToken(data.isHaveToken)
								this.storeageService.setRole(data.role)
								this.storeageService.setFullName(data.fullName)
								this.storeageService.setTypeObject(data.typeObject)
								this.http.get<{ ip: string }>('https://jsonip.com/').subscribe((dataIP) => {
									if (dataIP != null) {
										this.storeageService.setIpAddress(dataIP.ip)
									}
								})
								if (this.isSaveLogin) {
									this.storeageService.setKeyRemember(btoa(this.user.Password))
								} else {
									this.storeageService.setKeyRemember('')
								}
								//this._router.navigate(['/quan-tri'])
								// if (data.typeObject && data.typeObject == 1) {
								// 	location.href = '/quan-tri'
								// } else {
								// 	location.href = '/quan-tri'
								// 	// this.toastr.error(data.message, 'Tài khoản cá nhân, doanh nghiệp không thể đăng nhập hệ thống dành cho cán bộ quản lý')
								// 	// localStorage.clear();
								// }
								if(this.storeageService.getRecommentdationObjectRemember() != null){
									location.href='/cong-bo/them-moi-kien-nghi'
								}
								else{
									location.href = '/quan-tri'
								}
							} else if (data.success === RESPONSE_STATUS.incorrect) {
								this.toastr.error(data.message, 'Tên tài khoản hoặc mật khẩu không chính xác')
								this.reloadImage()
								this.captchaCode = ''
								this.submitted = false
								this.rebuildForm()
							}
						},
						(error) => {
							console.error(error)
							this.reloadImage()
							this.captchaCode = ''
							this.submitted = false
							this.rebuildForm()
						}
					)
				} else {
					this.toastr.error('Vui lòng nhập lại mã xác thực')
					this.reloadImage()
					this.submitted = false
					this.rebuildForm()
					this.captchaCode = ''
					//   this.captchaEl.nativeElement.focus();
				}
			})
		}
	}
	get floginFormProduct() {
		return this.loginFormProduct.controls
	}
	loginProduct() {
		this.submittedProduct = true
		this.userProduct.UserName = this.userProduct.UserName.trim()
		this.userProduct.Password = this.userProduct.Password.trim()
		if (this.loginFormProduct.invalid) {
			if (this.loginFormProduct.controls.name.status == 'INVALID') {
				$("input[id='productname']").focus()
				return
			}
			if (this.loginFormProduct.controls.pass.status == 'INVALID') {
				$("input[id='productpass']").focus()
				return
			}
			return
		} else {
			var constdata = {
				CaptchaCode: this.captchaCodeProduct,
			}
			this.captchaService.send(constdata).subscribe((result) => {
				if (result.success === RESPONSE_STATUS.success) {
					this.authenService.login(this.userProduct).subscribe(
						(data) => {
							if (data.success === RESPONSE_STATUS.success) {
								debugger
								if (data.isActive == false) {
									this.reloadImage()
									this.captchaCodeProduct = ''
									this.submittedProduct = false
									this.rebuildFormProduct()
									this.toastr.error('Tài khoản của bạn đang hết hiệu lực')
									return
								}
								this.storeageService.clear()
								this.shareData.setIsLogin(true)
								this.storeageService.setAccessToken(data.accessToken)
								this.storeageService.setUserId(data.userId)
								this.storeageService.setPermissionCategories(data.permissionCategories)
								this.storeageService.setFunctions(data.permissionFunctions)
								this.storeageService.setPermissions(data.permissions)
								this.storeageService.setUnitName(data.unitName)
								this.storeageService.setSaveLogin(this.isSaveLogin)
								this.storeageService.setIsHaveToken(data.isHaveToken)
								this.storeageService.setRole(data.role)
								this.storeageService.setIsSession(true)
								this.storeageService.setFullName(data.fullName)
								this.storeageService.setTypeObject(data.typeObject)
								this.http.get<{ ip: string }>('https://jsonip.com/').subscribe((dataIP) => {
									if (dataIP != null) {
										this.storeageService.setIpAddress(dataIP.ip)
									}
								})
								if (this.isSaveLogin) {
									this.storeageService.setKeyRemember(btoa(this.user.Password))
								} else {
									this.storeageService.setKeyRemember('')
								}
								//this._router.navigate(['/quan-tri'])
								if (data.typeObject && data.typeObject == 1) {
									this.submittedProduct = false
									this.reloadImage()
									this.captchaCodeProduct = ''
									this.rebuildFormProduct()
									this.toastr.error(data.message, 'Tài khoản cán bộ quản lý không thể đăng nhập hệ thống dành cho cá nhân, doanh nghiệp')
									this.storeageService.clear()

								} else {
									if(this.storeageService.getRecommentdationObjectRemember() != null){
										location.href='/cong-bo/them-moi-kien-nghi'
									}else{
										location.href = '/cong-bo'
									}
									
								}
							} else if (data.success === RESPONSE_STATUS.incorrect) {
								this.reloadImage()
								this.captchaCodeProduct = ''
								this.submittedProduct = false
								this.rebuildFormProduct()
								this.toastr.error(data.message, 'Tên tài khoản hoặc mật khẩu không chính xác')
							}
						},
						(error) => {
							console.error(error)
							this.reloadImage()
							this.submittedProduct = false
							this.rebuildFormProduct()
							this.captchaCodeProduct = ''
						}
					)
				} else {
					this.toastr.error('Vui lòng nhập lại mã xác thực')
					this.reloadImage()
					this.submittedProduct = false
					this.captchaCodeProduct = ''
					//   this.captchaEl.nativeElement.focus();
				}
			})
		}
	}

	showPassword(){
		this.isShowPassword = !this.isShowPassword
		if(this.isShowPassword== true){
			this.typeInput = 'password'
		}else{
			this.typeInput = 'text'
		}
	}

	get name() {
		return this.loginForm.get('name')
	}

	get pass() {
		return this.loginForm.get('pass')
	}

	get captcha() {
		return this.loginForm.get('captcha')
	}

	get nameProduct() {
		return this.loginFormProduct.get('name')
	}

	get passProduct() {
		return this.loginFormProduct.get('pass')
	}

	get captchaProduct() {
		return this.loginFormProduct.get('captcha')
	}

	forgetPassWordUser(): void {
		this._router.navigate(['/quen-mat-khau-quan-tri'])
	}
	forgetPassWord(): void {
		this._router.navigate(['/quen-mat-khau'])
	}

	register(): void {
		this._router.navigate(['/dang-ky/ca-nhan'])
	}

	handleExpire() {
		console.log('Hết hạn')
	}

	handleLoad() {
		console.log('đang load')
	}

	handleSuccess(data) {
		this.isAbleCaptcha = data
	}

	captchaCode: string = null
	captchaImage: any = ''
	reloadImage() {
		this.captchaImage = AppSettings.API_ADDRESS + Api.getImageCaptcha + '?' + Math.random() * 100000000000000000000
	}

	captchaCodeProduct: string = null
	captchaImageProduct: any = ''
	reloadImageProduct() {
		this.captchaImageProduct = AppSettings.API_ADDRESS + Api.getImageCaptcha + '?' + Math.random() * 100000000000000000000
	}
}
