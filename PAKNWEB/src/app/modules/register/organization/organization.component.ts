import { Component, OnInit, ViewChild } from '@angular/core'

import { ToastrService } from 'ngx-toastr'
import { FormGroup, FormBuilder, Validators, ValidatorFn, AbstractControl } from '@angular/forms'
import { Router } from '@angular/router'
import { viLocale } from 'ngx-bootstrap/locale'
import { defineLocale } from 'ngx-bootstrap/chronos'
import { BsLocaleService } from 'ngx-bootstrap/datepicker'

import { OrgFormAddressComponent } from './org-form-address/org-form-address.component'
import { OrgRepreFormComponent } from './org-repre-form/org-repre-form.component'

import { RegisterService } from 'src/app/services/register.service'

import { COMMONS } from 'src/app/commons/commons'
import { OrganizationObject } from 'src/app/models/RegisterObject'
import { MESSAGE_COMMON, PROCESS_STATUS_RECOMMENDATION, RECOMMENDATION_STATUS, RESPONSE_STATUS, STEP_RECOMMENDATION } from 'src/app/constants/CONSTANTS'
import { AuthenticationService } from 'src/app/services/authentication.service'

declare var $: any

@Component({
	selector: 'app-organization',
	templateUrl: './organization.component.html',
	styleUrls: ['./organization.component.css'],
})
export class OrganizationComponent implements OnInit {
	@ViewChild(OrgRepreFormComponent, { static: true })
	private child_OrgRepreForm: OrgRepreFormComponent
	@ViewChild(OrgFormAddressComponent, { static: true })
	private child_OrgAddressForm: OrgFormAddressComponent

	constructor(
		private localeService: BsLocaleService,
		private toast: ToastrService,
		private formBuilder: FormBuilder,
		private registerService: RegisterService,
		private router: Router,
		private authenticationService: AuthenticationService
	) {
		defineLocale('vi', viLocale)
	}

	dateNow: Date = new Date()

	formLogin: FormGroup
	formOrgInfo: FormGroup
	listNation: any[] = [{ id: 'Việt Nam', name: 'Việt Nam' }]
	model: OrganizationObject = new OrganizationObject()
	isOtherNation = false
	ngOnInit() {
		this.localeService.use('vi')

		this.child_OrgAddressForm.model = this.model
		this.child_OrgRepreForm.model = this.model
		this.loadFormBuilder()
	}

	serverMsg = {}
	closeModalOtp() {
		$('#modal-otp').modal('hide');
	}
	onReset() {
		this.formLogin.reset()
		this.formOrgInfo.reset()
		this.child_OrgRepreForm.formInfo.reset()
		this.child_OrgRepreForm.resetObject()
		this.child_OrgAddressForm.formOrgAddress.reset()
		this.child_OrgRepreForm.formInfo.get('Gender').setValue(true)

		this.fLoginSubmitted = false
		this.child_OrgRepreForm.fInfoSubmitted = false
		this.fOrgInfoSubmitted = false
		this.child_OrgAddressForm.fOrgAddressSubmitted = false
		this.model = new OrganizationObject()
		this.model._RepresentativeBirthDay = ''
		this.model._DateOfIssue = ''
		this.model.representativeGender = true
	}

	phoneHide: any = ''
	otp_1: any = ''
	otp_2: any = ''
	otp_3: any = ''
	otp_4: any = ''
	otp_5: any = ''
	otp_6: any = ''
	otp: string = "";

	onPreShowOtp() {

		this.clearOTP()
		this.fLoginSubmitted = true
		this.child_OrgRepreForm.fInfoSubmitted = true
		this.fOrgInfoSubmitted = true
		this.child_OrgAddressForm.fOrgAddressSubmitted = true

		if (
			this.checkExists['Phone'] ||
			this.checkExists['BusinessRegistration'] ||
			this.child_OrgAddressForm.checkExists['OrgEmail'] ||
			this.child_OrgAddressForm.checkExists['OrgPhone'] ||
			this.child_OrgRepreForm.checkExists['Email']
		) {
			//this.toast.error('Dữ liệu không hợp lệ')
			return
		}

		if (this.formLogin.invalid || this.formOrgInfo.invalid || this.child_OrgRepreForm.formInfo.invalid || this.child_OrgAddressForm.formOrgAddress.invalid) {
			//this.toast.error('Dữ liệu không hợp lệ')
			return
		}
		this.phoneHide = this.model.phone
			.split('')
			.map((item, index) => {
				if (index > 3 && index < 7) {
					return '*'
				}
				return item
			})
			.join('')
		$('#modal-otp').modal('show')
		setTimeout(() => {
			$('#input_1').focus()
		}, 400);
	}

	onChange = (event, index) => {
		if (event.target.value) {
			setTimeout(() => {
				$('#input_' + String(index + 1)).focus()
			}, 1);
		} else {
			setTimeout(() => {
				$('#input_' + String(index - 1)).focus()
			}, 1);

		}
	}
	clearOTP = () => {
		this.authenticationService.getTokenByEmail({ Email: this.model.orgEmail, Type: 1 }).subscribe((res) => {
			if (res.success == 'OK') {
				this.otp = res.result
			}
		})
		this.otp_1 = null
		this.otp_2 = null
		this.otp_3 = null
		this.otp_4 = null
		this.otp_5 = null
		this.otp_6 = null
	}

	onSave() {
		debugger
		if (!this.otp_1 || !this.otp_2 || !this.otp_3 || !this.otp_4 || !this.otp_5 || !this.otp_6) {
			this.toast.error('Vui lòng nhập otp!')
			return
		} else {
			var otpInput = "" + this.otp_1 + this.otp_2 + this.otp_3 + this.otp_4 + this.otp_5 + this.otp_6;
			if (otpInput != this.otp) {
				this.toast.error('Mã otp bạn nhập không chính xác!')
				return
			}
		}
		$('#modal-otp').modal('hide')

		this.registerService.registerOrganization(this.model).subscribe((res) => {
			if (res.success != 'OK') {
				let msg = res.message
				if (msg.includes(`UNIQUE KEY constraint 'UC_SY_User_Email'`)) {
					this.toast.error('Email Người đại diện đã tồn tại')
					return
				}
				if (msg.includes(`UNIQUE KEY constraint 'UK_BI_Business_OrgEmail'`)) {
					this.toast.error('Email Văn phòng đại diện đã tồn tại')
					return
				}
				if (msg.includes(`UNIQUE KEY constraint 'UK_BI_Business_Email'`)) {
					this.toast.error('Email Người đại diện đã tồn tại')
					return
				}
				this.toast.error(msg)
				return
			}
			this.toast.success('Đăng ký tài khoản thành công')
			this.router.navigate(['/dang-nhap'])
		})
		//req to server
	}

	fLoginSubmitted = false
	fOrgInfoSubmitted = false

	get fLogin() {
		return this.formLogin.controls
	}
	get fOrgInfo() {
		return this.formOrgInfo.controls
	}

	private loadFormBuilder() {
		//form thông tin đăng nhập
		this.formLogin = this.formBuilder.group(
			{
				businessRegistration: [this.model.businessRegistration, [Validators.required]],
				password: [this.model.password, [Validators.required]],
				rePassword: [this.model.rePassword, [Validators.required]],
			},
			{ validator: MustMatch('password', 'rePassword') }
		)

		this.formOrgInfo = this.formBuilder.group({
			//---thông tin doanh nghiệp
			business: [this.model.business, [Validators.required, Validators.maxLength(200)]], // tên tổ chức
			orgPhone: [this.model.orgPhone, [Validators.maxLength(20)]], //Số ĐKKD
		})
	}

	//kiểm tra dữ liệu đã tồn tại
	checkExists = {
		Phone: false,
		BusinessRegistration: false,
		DecisionOfEstablishing: false,
		OrgPhone: false,
	}
	onCheckExist(field: string, value: string) {
		if (value == null || value == '') {
			this.checkExists[field] = false
			return
		}
		this.registerService
			.businessCheckExists({
				field,
				value,
				id: 0,
			})
			.subscribe((res) => {
				if (res.success == RESPONSE_STATUS.success) {
					// if (field == 'Phone') this.phone_exists = res.result.BIBusinessCheckExists[0].exists
					// else if (field == 'BusinessRegistration') this.busiRegis_exists = res.result.BIBusinessCheckExists[0].exists
					// else if (field == 'DecisionOfEstablishing') this.busiDeci_exists = res.result.BIBusinessCheckExists[0].exists
					this.checkExists[field] = res.result.BIBusinessCheckExists[0].exists
				}
			})
	}
	backToHome() {
		window.open('/cong-bo/trang-chu', '_self')
	}
}
function MustMatch(controlName: string, matchingControlName: string) {
	return (formGroup: FormGroup) => {
		const control = formGroup.controls[controlName]
		const matchingControl = formGroup.controls[matchingControlName]

		if (matchingControl.errors && !matchingControl.errors.mustMatch) {
			// return if another validator has already found an error on the matchingControl
			return
		}

		// set error on matchingControl if validation fails
		if (control.value !== matchingControl.value) {
			matchingControl.setErrors({ mustMatch: true })
		} else {
			matchingControl.setErrors(null)
		}
	}
}
