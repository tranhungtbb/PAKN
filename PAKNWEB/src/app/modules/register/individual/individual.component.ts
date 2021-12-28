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
import { UserService } from 'src/app/services/user.service'
import { AuthenticationService } from 'src/app/services/authentication.service'

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
		private authenticationService: AuthenticationService,
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
	otp: string = "";

	listGender: any[] = [
		{ value: true, text: 'Nam' },
		{ value: false, text: 'Nữ' },
	]

	isOtherNation = false

	ngOnInit() {
		this.localeService.use('vi')
		this.model = new IndividualObject()
		this.loadFormBuilder()
		// this.onChangeNation()
	}

	//get req

	//event
	onResetNationValue(event: any) {
		console.log(event)
		if (event.target.value == 'Nhập...') {
			event.target.value = ''
		}
	}
	closeModalOtp() {
		$('#modal-otp').modal('hide');
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
				this.model.nation = 'Nhập...'
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
	validateDateOfIssue: any = true

	onReset() {
		debugger
		this.model = new IndividualObject()
		this.formLogin.reset({
			iDCard: this.model.iDCard,
			password: this.model.password,
			rePassword: this.model.rePassword
		})
		this.formInfo.reset({
			fullName: this.model.fullName,
			gender: this.model.gender,
			dob: this.model.birthDay,
			nation: this.model.nation,
			province: this.model.provinceId,
			district: this.model.districtId,
			village: this.model.wardsId,

			email: this.model.email,
			address: this.model.address,
			phone: this.model.phone,
			placeIssue: this.model.issuedPlace,
			dateIssue: this.model.dateOfIssue,
		})
		this.fLoginSubmitted = false
		this.fInfoSubmitted = false

		this.model.gender = true
		this.formInfo.get('gender').setValue(this.model.gender)
	}
	phoneHide: any = ''
	otp_1: any = ''
	otp_2: any = ''
	otp_3: any = ''
	otp_4: any = ''
	otp_5: any = ''
	otp_6: any = ''
	onPreShowOtp() {


		this.clearOTP()
		this.fLoginSubmitted = true
		this.fInfoSubmitted = true

		this.model.email = this.model.email == null ? '' : this.model.email.trim()
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
		this.authenticationService.getTokenByEmail({ Email: this.model.email, Type: 1 }).subscribe((res) => {
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
				iDCard: [this.model.iDCard, [Validators.required]],
				password: [this.model.password, [Validators.required]],
				rePassword: [this.model.rePassword, [Validators.required]],
			},
			{ validator: MustMatch('password', 'rePassword') }
		)

		//form thông tin
		this.formInfo = this.formBuilder.group({
			fullName: [this.model.fullName, [Validators.required, Validators.maxLength(100)]],
			gender: [this.model.gender, [Validators.required]],
			dob: [this.model.birthDay],
			nation: [this.model.nation],
			province: [this.model.provinceId],
			district: [this.model.districtId],
			village: [this.model.wardsId],

			email: [this.model.email, [Validators.email, Validators.required]],
			address: [this.model.address, [Validators.required]],
			phone: [this.model.phone, [Validators.required, Validators.pattern(/^(84|0[3|5|7|8|9])+([0-9]{8})$/g)]], //, Validators.pattern(/^([0-9]){8,12}$/g)
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


