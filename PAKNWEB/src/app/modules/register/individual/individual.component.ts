import { Component, OnInit } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { Router } from '@angular/router'
import { FormGroup, FormBuilder, Validators, ValidatorFn, AbstractControl } from '@angular/forms'

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
	) {}

	// datePickerConfig:DatepickerOptions={
	// 	inputClass: 'form-control border-brown',
	// 	placeholder:'Nhập...',
	// 	formatTitle: 'MM yyyy',
	// 	format: 'dd/MM/yyyy',
	// 	calendarClass: 'datepicker-container datepicker-dark',
	// }
	date: Date = new Date()

	formLogin: FormGroup
	formInfo: FormGroup
	model: IndividualObject = new IndividualObject()

	listNation: any[] = [{ id: 1, name: 'Việt Nam' }]
	listProvince: any[] = []
	listDistrict: any[] = []
	listVillage: any[] = []

	listGender: any[] = [
		{ value: true, text: 'Nam' },
		{ value: false, text: 'Nữ' },
	]

	ngOnInit() {
		this.loadFormBuilder()
	}

	//get req

	//event
	onChangeNation() {
		this.listProvince = []
		this.listDistrict = []
		this.listVillage = []

		this.model.province = ''
		if (this.model.nation == 1) {
			this.diadanhService.getAllProvince().subscribe((res) => {
				if (res.success == 'OK') {
					this.listProvince = res.result.CAProvinceGetAll
				}
			})
		} else {
		}
	}
	onChangeProvince() {
		this.listDistrict = []
		this.listVillage = []

		this.model.district = ''
		this.model.village = ''
		if (this.model.province != null && this.model.province != '') {
			this.diadanhService.getAllDistrict(this.model.province).subscribe((res) => {
				if (res.success == 'OK') {
					this.listDistrict = res.result.CADistrictGetAll
				}
			})
		} else {
		}
	}

	onChangeDistrict() {
		this.listVillage = []

		this.model.village = ''
		if (this.model.district != null && this.model.district != '') {
			this.diadanhService.getAllVillage(this.model.province, this.model.district).subscribe((res) => {
				if (res.success == 'OK') {
					this.listVillage = res.result.CAVillageGetAll
				}
			})
		} else {
		}
	}

	onSave() {
		this.fLoginSubmitted = true
		this.fInfoSubmitted = true

		let fDob: any = document.querySelector('ng-datepicker#_dob input')
		let fDateIssue: any = document.querySelector('ng-datepicker#_dateIssue input')
		this.model.dob = fDob.value
		this.model.dateIssue = fDateIssue.value

		if (this.formLogin.invalid || this.formInfo.invalid) {
			this.toast.error('Dữ liệu không hợp lệ')
			return
		}

		// req to server
		this.registerService.registerIndividual(this.model).subscribe((res) => {
			if (res.success != 'OK') {
				this.toast.error(res.message)
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
			fullName: [this.model.fullName, [Validators.required]],
			gender: [this.model.gender, [Validators.required]],
			dob: [this.model.dob, [Validators.required]],
			nation: [this.model.nation, [Validators.required]],
			province: [this.model.province, [Validators.required]],
			district: [this.model.district, [Validators.required]],
			village: [this.model.village, [Validators.required]],

			email: [this.model.email, [Validators.required, Validators.email]],
			address: [this.model.address, [Validators.required]],
			identity: [this.model.identity, [Validators.required, Validators.pattern(/^([0-9]){9,12}$/g)]],
			placeIssue: [this.model.placeIssue, [Validators.required]],
			dateIssue: [this.model.dateIssue, [Validators.required]],
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
