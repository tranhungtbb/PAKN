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
		private router: Router
	) {
		defineLocale('vi', viLocale)
	}

	dateNow: Date = new Date()

	formLogin: FormGroup
	formOrgInfo: FormGroup
	listNation: any[] = [{ id: 'Việt Nam', name: 'Việt Nam' }]
	model: OrganizationObject = new OrganizationObject()
	nation_enable_type = false
	ngOnInit() {
		this.localeService.use('vi')

		this.child_OrgAddressForm.model = this.model
		this.child_OrgRepreForm.model = this.model
		this.loadFormBuilder()
	}

	serverMsg = {}

	onReset() {
		this.formLogin.reset()
		this.formOrgInfo.reset()
		this.child_OrgRepreForm.formInfo.reset()
		this.child_OrgAddressForm.formOrgAddress.reset()

		this.fLoginSubmitted = false
		this.child_OrgRepreForm.fInfoSubmitted = false
		this.fOrgInfoSubmitted = false
		this.child_OrgAddressForm.fOrgAddressSubmitted = false
		this.model = new OrganizationObject()
		this.model._RepresentativeBirthDay = ''
		this.model._DateOfIssue = ''
		this.model.RepresentativeGender = true
	}
	onSave() {
		this.fLoginSubmitted = true
		this.child_OrgRepreForm.fInfoSubmitted = true
		this.fOrgInfoSubmitted = true
		this.child_OrgAddressForm.fOrgAddressSubmitted = true

		let fDob: any = document.querySelector('#_dob')
		let fIsDate: any = document.querySelector('#_IsDate')
		this.model._RepresentativeBirthDay = fDob.value
		this.model._DateOfIssue = fIsDate.value

		if (
			this.phone_exists ||
			this.busiDeci_exists ||
			this.busiDeci_exists ||
			this.child_OrgAddressForm.orgEmail_exists ||
			this.child_OrgAddressForm.orgPhone_exists ||
			this.child_OrgRepreForm.email_exists ||
			this.child_OrgRepreForm.idCard_exists
		) {
			this.toast.error('Dữ liệu không hợp lệ')
			return
		}

		if (this.formLogin.invalid || this.formOrgInfo.invalid || this.child_OrgRepreForm.formInfo.invalid || this.child_OrgAddressForm.formOrgAddress.invalid) {
			this.toast.error('Dữ liệu không hợp lệ')
			return
		}

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
				phone: [this.model.phone, [Validators.required, Validators.pattern(/^(84|0[3|5|7|8|9])+([0-9]{8})$/)]],
				password: [this.model.password, [Validators.required, Validators.pattern(/^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$/)]],
				rePassword: [this.model.rePassword, [Validators.required]],
			},
			{ validator: MustMatch('password', 'rePassword') }
		)

		this.formOrgInfo = this.formBuilder.group({
			//---thông tin doanh nghiệp
			Business: [this.model.Business, [Validators.required, Validators.maxLength(200)]], // tên tổ chức
			RegistrationNum: [this.model.BusinessRegistration, [Validators.maxLength(20)]], //Số ĐKKD
			DecisionFoundation: [this.model.DecisionOfEstablishing, [Validators.maxLength(20)]], //Quyết định thành lập
			DateIssue: [this.model._DateOfIssue, []], //Ngày cấp/thành lập
			Tax: [this.model.Tax, [Validators.required, Validators.maxLength(13)]], //Mã số thuế
		})
	}

	//kiểm tra dữ liệu đã tồn tại
	phone_exists: boolean = false
	busiRegis_exists = false
	busiDeci_exists = false
	onCheckExist(field: string, value: string) {
		this.registerService
			.businessCheckExists({
				field,
				value,
			})
			.subscribe((res) => {
				if (res.success == RESPONSE_STATUS.success) {
					if (field == 'Phone') this.phone_exists = res.result.BIBusinessCheckExists[0].exists
					else if (field == 'BusinessRegistration') this.busiRegis_exists = res.result.BIBusinessCheckExists[0].exists
					else if (field == 'DecisionOfEstablishing') this.busiDeci_exists = res.result.BIBusinessCheckExists[0].exists
				}
			})
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
