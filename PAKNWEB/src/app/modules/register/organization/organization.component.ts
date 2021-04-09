import { Component, OnInit } from '@angular/core'

import { ToastrService } from 'ngx-toastr'
import { FormGroup, FormBuilder, Validators, ValidatorFn, AbstractControl } from '@angular/forms'

import { COMMONS } from 'src/app/commons/commons'
import { OrganizationObject } from 'src/app/models/RegisterObject'

declare var $: any

@Component({
	selector: 'app-organization',
	templateUrl: './organization.component.html',
	styleUrls: ['./organization.component.css'],
})
export class OrganizationComponent implements OnInit {
	constructor(private toast: ToastrService, private formBuilder: FormBuilder) {}

	formLogin: FormGroup
	formInfo: FormGroup
	formOrgInfo: FormGroup
	formOrgAddress: FormGroup

	model: OrganizationObject = new OrganizationObject()

	ngOnInit() {
		this.loadFormBuilder()
	}

	listNation: any[] = [
		{ id: 1, name: 'Việt Nam' },
		{ id: 2, name: 'Lào' },
		{ id: 3, name: 'Thái Lan' },
		{ id: 4, name: 'Campuchia' },
	]
	listProvince: any[] = []
	listDistrict: any[] = []
	listVillage: any[] = []

	listGender: any[] = [
		{ value: true, text: 'Nam' },
		{ value: false, text: 'Nữ' },
	]

	onSave() {
		this.fLoginSubmitted = true
		this.fInfoSubmitted = true
		this.fOrgInfoSubmitted = true
		this.fOrgAddressSubmitted = true

		console.log(this.model);

		if (this.formLogin.invalid || this.formInfo.invalid || this.formOrgInfo.invalid || this.formOrgAddress.invalid) {
			this.toast.error('Dữ liệu không hợp lệ')
			return
		}
		//req to server
	}

	fLoginSubmitted = false
	fInfoSubmitted = false
	fOrgInfoSubmitted = false
	fOrgAddressSubmitted = false

	get fLogin() {
		return this.formLogin.controls
	}
	get fInfo() {
		return this.formInfo.controls
	}
	get fOrgInfo() {
		return this.formOrgInfo.controls
	}
	get fOrgAdr() {
		return this.formOrgAddress.controls
	}

	private loadFormBuilder() {
		//form thông tin đăng nhập
		this.formLogin = this.formBuilder.group(
			{
				phone: [this.model.phone, [Validators.required, Validators.pattern(/^(84|0[3|5|7|8|9])+([0-9]{8})$/g)]],
				password: [this.model.password, [Validators.required, Validators.pattern(/^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$/g)]],
				rePassword: [this.model.rePassword, [Validators.required]],
			},
			{ validator: MustMatch('password', 'rePassword') }
		)

		//form thông tin nguoi dai dien
		this.formInfo = this.formBuilder.group({
			//----thông tin người đại diện
			RepresentativeName: [this.model.RepresentativeName, [Validators.required]], // tên người đại diện
			Email: [this.model.Email, [Validators.required, Validators.email]],
			Gender: [this.model.Gender, [Validators.required]],
			DOB: [this.model.DOB, [Validators.required]],
			Nation: [this.model.Nation, [Validators.required]],
			Province: [this.model.Province, [Validators.required]], //int
			District: [this.model.District, [Validators.required]], // int
			Village: [this.model.Village, [Validators.required]], // int
			Address: [this.model.Address, [Validators.required]],
		})

		this.formOrgInfo = this.formBuilder.group({
			//---thông tin doanh nghiệp
			Business: [this.model.Business, [Validators.required]], // tên tổ chức
			RegistrationNum: [this.model.RegistrationNum, [Validators.required]], //Số ĐKKD
			DecisionFoundation: [this.model.DecisionFoundation, [Validators.required]], //Quyết định thành lập
			DateIssue: [this.model.DateIssue, [Validators.required]], //Ngày cấp/thành lập
			Tax: [this.model.Tax, [Validators.required]], //Mã số thuế
		})

		this.formOrgAddress = this.formBuilder.group({
			OrgProvince: [this.model.OrgDistrict, [Validators.required]], //int
			OrgDistrict: [this.model.OrgDistrict, [Validators.required]], //int
			OrgVillage: [this.model.OrgVillage, [Validators.required]], //int
			OrgAddress: [this.model.OrgAddress, [Validators.required]],
			OrgPhone: [this.model.OrgPhone, [Validators.required, Validators.pattern(/^(84|0[3|5|7|8|9])+([0-9]{8})$/g)]],
			OrgEmail: [this.model.OrgEmail, [Validators.required, Validators.email]],
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
