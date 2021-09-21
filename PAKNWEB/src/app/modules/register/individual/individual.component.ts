import { Component, OnInit } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { Router } from '@angular/router'
import { FormGroup, FormBuilder, Validators, ValidatorFn, AbstractControl } from '@angular/forms'
import { viLocale } from 'ngx-bootstrap/locale'
import { defineLocale } from 'ngx-bootstrap/chronos'
import { BsLocaleService } from 'ngx-bootstrap/datepicker'

import { RegisterService } from 'src/app/services/register.service'
import { DiadanhService } from 'src/app/services/diadanh.service'

import { COMMONS } from 'src/app/commons/commons'
import { MESSAGE_COMMON, PROCESS_STATUS_RECOMMENDATION, RECOMMENDATION_STATUS, RESPONSE_STATUS, STEP_RECOMMENDATION } from 'src/app/constants/CONSTANTS'
import { IndividualObject } from 'src/app/models/RegisterObject'

declare var $: any
@Component({
	selector: 'app-individual',
	templateUrl: './individual.component.html',
	styleUrls: ['./individual.component.css'],
})
export class IndividualComponent implements OnInit {
	constructor(
		private localeService: BsLocaleService,
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

	isOtherNation = false

	ngOnInit() {
		this.localeService.use('vi')
		this.loadFormBuilder()
		this.onChangeNation()
	}

	//get req

	//event
	onResetNationValue(event: any) {
		console.log(event)
		if (event.target.value == 'Nhập...') {
			event.target.value = ''
		}
	}
	backDefaultValue() {
		this.isOtherNation = false
		this.model.nation = null
		this.model.provinceId = null
		this.model.districtId = null
		this.model.wardsId = null
		this.formInfo.controls['province'].setValue(null)
		this.formInfo.controls['district'].setValue(null)
		this.formInfo.controls['village'].setValue(null)
		this.formInfo.controls['nation'].setValue(null)
	}
	onChangeNation() {
		this.listProvince = []
		this.listDistrict = []
		this.listVillage = []

		this.model.provinceId = null
		this.model.districtId = null
		this.model.wardsId = null

		if (this.model.nation == 'Việt Nam') {
			debugger
			this.diadanhService.getAllProvince().subscribe((res) => {
				if (res.success == 'OK') {
					this.listProvince = res.result.CAProvinceGetAll
					this.model.provinceId = 37
					this.onChangeProvince()
				}
			})
		} else {
			if (this.model.nation == '#') {
				this.isOtherNation = true
				this.model.nation = "Nhập..."
				this.formInfo.controls['province'].setValue(0)
				this.formInfo.controls['district'].setValue(0)
				this.formInfo.controls['village'].setValue(0)
				//
				// this.formInfo.controls.province.disable()
				// this.formInfo.controls.district.disable()
				// this.formInfo.controls.village.disable()
			}
		}
	}
	onChangeProvince() {
		this.listDistrict = []
		this.listVillage = []
		this.model.districtId = null
		this.model.wardsId = null
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
		this.model.wardsId = null
		if (this.model.districtId != null && this.model.districtId != '') {
			this.diadanhService.getAllVillage(this.model.provinceId, this.model.districtId).subscribe((res) => {
				if (res.success == 'OK') {
					this.listVillage = res.result.CAVillageGetAll
				}
			})
		} else {
		}
	}
	validateDateOfIssue : any = true

	onReset() {
		this.formLogin.reset()
		this.formInfo.reset()

		this.fLoginSubmitted = false
		this.fInfoSubmitted = false
		this.model = new IndividualObject()
		this.model.gender = true
		this.formInfo.get('gender').setValue(this.model.gender)
	}

	onSave() {
		this.fLoginSubmitted = true
		this.fInfoSubmitted = true

		if (!this.model.email) this.model.email = ''
		if (this.model.nation == 'Nhập...') this.model.nation = ''
		if (this.checkExists['Phone'] || this.checkExists['Email'] || this.checkExists['IDCard']) {
			return
		}
		if (this.formLogin.invalid || this.formInfo.invalid) {
			return
		}

		if (this.model.dateOfIssue && this.model.birthDay > this.model.dateOfIssue) {
			this.validateDateOfIssue = false
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
				password: [this.model.password, [Validators.required]], //, Validators.pattern(/^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$/)
				rePassword: [this.model.rePassword, [Validators.required]],
			},
			{ validator: MustMatch('password', 'rePassword') }
		)

		//form thông tin
		this.formInfo = this.formBuilder.group({
			fullName: [this.model.fullName, [Validators.required, Validators.maxLength(100)]],
			gender: [this.model.gender, [Validators.required]],
			dob: [this.model.birthDay, [Validators.required]],
			nation: [this.model.nation, [Validators.required]],
			province: [this.model.provinceId, [Validators.required]],
			district: [this.model.districtId, [Validators.required]],
			village: [this.model.wardsId, [Validators.required]],

			email: [this.model.email, [Validators.email]],
			address: [this.model.address, [Validators.required]],
			iDCard: [this.model.iDCard, [Validators.required]], //, Validators.pattern(/^([0-9]){8,12}$/g)
			placeIssue: [this.model.issuedPlace, []],
			dateIssue: [this.model.dateOfIssue, []],
		})
	}

	// server exists
	checkExists = {
		Phone: false,
		Email: false,
		IDCard: false,
	}
	onCheckExist(field: string, value: string) {
		if (value == null || value == '') {
			this.checkExists[field] = false
			return
		}
		this.registerService
			.individualCheckExists({
				field,
				value,
				id: 0,
			})
			.subscribe((res) => {
				if (res.success == RESPONSE_STATUS.success) {
					this.checkExists[field] = res.result.BIIndividualCheckExists[0].exists
				}
			})
	}
	backToHome() {
		this.router.navigate(['/dang-nhap'])
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
