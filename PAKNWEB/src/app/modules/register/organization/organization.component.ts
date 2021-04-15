import { Component, OnInit, ViewChild } from '@angular/core'

import { ToastrService } from 'ngx-toastr'
import { FormGroup, FormBuilder, Validators, ValidatorFn, AbstractControl } from '@angular/forms'
import { Router } from '@angular/router'

import { OrgFormAddressComponent } from './org-form-address/org-form-address.component'
import { OrgRepreFormComponent } from './org-repre-form/org-repre-form.component'

import { RegisterService } from 'src/app/services/register.service'

import { COMMONS } from 'src/app/commons/commons'
import { OrganizationObject } from 'src/app/models/RegisterObject'

declare var $: any

@Component({
	selector: 'app-organization',
	templateUrl: './organization.component.html',
	styleUrls: ['./organization.component.css'],
})
export class OrganizationComponent implements OnInit {
	constructor(private toast: ToastrService, private formBuilder: FormBuilder, private registerService: RegisterService, private router: Router) {}

	dateNow: Date = new Date()

	@ViewChild(OrgRepreFormComponent, { static: false }) child_OrgRepreForm: OrgRepreFormComponent
	@ViewChild(OrgFormAddressComponent, { static: false }) child_OrgAddressForm: OrgFormAddressComponent

	formLogin: FormGroup
	formOrgInfo: FormGroup

	model: OrganizationObject = new OrganizationObject()
	nation_enable_type = false
	ngOnInit() {
		// this.child_OrgAddressForm.parentCompo = this
		// this.child_OrgRepreForm.parentCompo = this
		this.loadFormBuilder()
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

		if (this.formLogin.invalid || this.formOrgInfo.invalid || this.child_OrgRepreForm.formInfo.invalid || this.child_OrgAddressForm.formOrgAddress.invalid) {
			this.toast.error('Dữ liệu không hợp lệ')
			return
		}

		this.registerService.registerOrganization(this.model).subscribe((res) => {
			if (res.success != 'OK') {
				this.toast.error(res.message)
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
			Business: [this.model.Business, [Validators.required]], // tên tổ chức
			RegistrationNum: [this.model.BusinessRegistration, [Validators.required]], //Số ĐKKD
			DecisionFoundation: [this.model.DecisionOfEstablishing, [Validators.required]], //Quyết định thành lập
			DateIssue: [this.model._DateOfIssue, [Validators.required]], //Ngày cấp/thành lập
			Tax: [this.model.Tax, [Validators.required]], //Mã số thuế
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
