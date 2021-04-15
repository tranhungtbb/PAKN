import { Component, OnInit } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { Router } from '@angular/router'
import { FormGroup, FormBuilder, Validators, ValidatorFn, AbstractControl } from '@angular/forms'
import { viLocale } from 'ngx-bootstrap/locale'
import { defineLocale } from 'ngx-bootstrap/chronos'

import { RegisterService } from 'src/app/services/register.service'
import { DiadanhService } from 'src/app/services/diadanh.service'

import { COMMONS } from 'src/app/commons/commons'
import { IndividualObject } from 'src/app/models/RegisterObject'

declare var $: any
@Component({
	selector: 'app-individual',
	templateUrl: './individual.component.html',
	styleUrls: ['./individual.component.css'],
})
export class IndividualComponent implements OnInit {
	constructor(
		private toast: ToastrService,
		private formBuilder: FormBuilder,
		private router: Router,
		private registerService: RegisterService,
		private diadanhService: DiadanhService
	) {
		defineLocale('vi', viLocale)
	}
	dateNow: Date = new Date()

	formLogin: FormGroup
	formInfo: FormGroup
	model: IndividualObject = new IndividualObject()

	listNation: any[] = [{ id: 'Việt Nam', name: 'Việt Nam' }]
	listProvince: any[] = []
	listDistrict: any[] = []
	listVillage: any[] = []

	listGender: any[] = [
		{ value: true, text: 'Nam' },
		{ value: false, text: 'Nữ' },
	]

	nation_enable_type = false

	ngOnInit() {
		this.loadFormBuilder()
	}

	//get req

	//event
	onChangeNation() {
		this.listProvince = []
		this.listDistrict = []
		this.listVillage = []

		this.model.provinceId = ''
		if (this.model.nation == 'Việt Nam') {
			this.diadanhService.getAllProvince().subscribe((res) => {
				if (res.success == 'OK') {
					this.listProvince = res.result.CAProvinceGetAll
				}
			})
		} else {
			if (this.model.nation == '#') {
				this.nation_enable_type = true
				this.model.nation = ''
				this.model.provinceId = 0
				this.model.districtId = 0
				this.model.wardsId = 0
			}
		}
	}
	onChangeProvince() {
		this.listDistrict = []
		this.listVillage = []

		this.model.districtId = ''
		this.model.wardsId = ''
		if (this.model.provinceId != null && this.model.provinceId != '') {
			this.diadanhService.getAllDistrict(this.model.provinceId).subscribe((res) => {
				if (res.success == 'OK') {
					this.listDistrict = res.result.CADistrictGetAll
				}
			})
		} else {
		}
	}

	onChangeDistrict() {
		this.listVillage = []

		this.model.wardsId = ''
		if (this.model.districtId != null && this.model.districtId != '') {
			this.diadanhService.getAllVillage(this.model.provinceId, this.model.districtId).subscribe((res) => {
				if (res.success == 'OK') {
					this.listVillage = res.result.CAVillageGetAll
				}
			})
		} else {
		}
	}

	onReset() {
		this.fLoginSubmitted = false
		this.fInfoSubmitted = false
		this.model = new IndividualObject()
		this.model._birthDay = ''
		this.model._dateOfIssue = ''
		this.model.fullName = ''
		this.formLogin.reset()
		this.formInfo.reset()
	}

	onSave() {
		this.fLoginSubmitted = true
		this.fInfoSubmitted = true

		let fDob: any = document.querySelector('#_dob')
		let fDateIssue: any = document.querySelector('#_dateIssue')
		this.model._birthDay = fDob.value
		this.model._dateOfIssue = fDateIssue.value

		if (this.formLogin.invalid || this.formInfo.invalid) {
			this.toast.error('Dữ liệu không hợp lệ')
			return
		}

		// req to server
		this.registerService.registerIndividual(this.model).subscribe((res) => {
			if (res.success != 'OK') {
				let msg = res.message
				if (msg.includes(`UNIQUE KEY constraint 'UC_SY_User_Email'`)) {
					this.toast.error('Email đã tồn tại')
				}

				this.toast.error(msg)
				return
			}
			this.toast.success('Đăng ký tài khoản thành công')
			this.router.navigate(['/dang-nhap'])
		})
	}

	fLoginSubmitted = false
	fInfoSubmitted = false

	get fLogin() {
		return this.formLogin.controls
	}

	get fInfo() {
		return this.formInfo.controls
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

		//form thông tin
		this.formInfo = this.formBuilder.group({
			fullName: [this.model.fullName, [Validators.required, Validators.maxLength(100)]],
			gender: [this.model.gender, [Validators.required]],
			dob: [this.model._birthDay, [Validators.required]],
			nation: [this.model.nation, [Validators.required]],
			province: [this.model.provinceId, [Validators.required]],
			district: [this.model.districtId, [Validators.required]],
			village: [this.model.wardsId, [Validators.required]],

			email: [this.model.email, [Validators.email]],
			address: [this.model.address, [Validators.required]],
			iDCard: [this.model.iDCard, [Validators.required, Validators.pattern(/^([0-9]){9,12}$/g)]],
			placeIssue: [this.model.issuedPlace, []],
			dateIssue: [this.model._dateOfIssue, []],
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
