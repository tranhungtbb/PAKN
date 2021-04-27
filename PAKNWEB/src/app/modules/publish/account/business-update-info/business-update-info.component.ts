import { Component, OnInit, ViewChild } from '@angular/core'
import { Router } from '@angular/router'
import { ToastrService } from 'ngx-toastr'
import { AccountService } from 'src/app/services/account.service'
import { FormBuilder, FormControl, FormGroup, Validator, Validators } from '@angular/forms'
import { COMMONS } from 'src/app/commons/commons'
import { viLocale } from 'ngx-bootstrap/locale'
import { defineLocale } from 'ngx-bootstrap/chronos'
import { BsLocaleService } from 'ngx-bootstrap/datepicker'

import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { RESPONSE_STATUS, REGEX } from 'src/app/constants/CONSTANTS'
import { AuthenticationService } from 'src/app/services/authentication.service'
import { DataService } from 'src/app/services/sharedata.service'
import { DiadanhService } from 'src/app/services/diadanh.service'
import { AccountSideLeftComponent } from '../account-side-left/account-side-left.component'

@Component({
	selector: 'app-business-update-info',
	templateUrl: './business-update-info.component.html',
	styleUrls: ['./business-update-info.component.css'],
})
export class BusinessUpdateInfoComponent implements OnInit {
	constructor(
		private localeService: BsLocaleService,
		private formBuider: FormBuilder,
		private toast: ToastrService,
		private router: Router,
		private accountService: AccountService,
		private storageService: UserInfoStorageService,
		private authenService: AuthenticationService,
		private sharedataService: DataService,
		private diadanhService: DiadanhService
	) {
		defineLocale('vi', viLocale)
	}

	formUpdateAccountInfo: FormGroup

	model: any = {}
	editable = false
	@ViewChild(AccountSideLeftComponent, { static: false }) child_SideLeft: AccountSideLeftComponent

	listNation: any[] = [{ id: 'Việt Nam', name: 'Việt Nam' }]
	listProvince: any[] = []
	listDistrict: any[] = []
	listVillage: any[] = []

	listOrgDistrict: any[] = []
	listOrgVillage: any[] = []

	listGender: any[] = [
		{ value: true, text: 'Nam' },
		{ value: false, text: 'Nữ' },
	]

	//
	nation_enable_type = false
	orgnation_enable_type = false

	//regex
	regex_phone: string = REGEX.PHONE_VN

	ngOnInit() {
		this.getUserInfo()
		this.formUpdateAccountInfo = this.formBuider.group({
			representativeName: [this.model.representativeName, [Validators.required]],
			representativeBirthDay: [this.model.representativeBirthDay],
			email: [this.model.email, [Validators.email]],
			nation: [this.model.nation, [Validators.required]],
			provinceId: [this.model.provinceId],
			districtId: [this.model.districtId],
			wardsId: [this.model.wardsId],
			address: [this.model.address, [Validators.required]],
			representativeGender: [this.model.representativeGender],
			businessRegistration: [this.model.businessRegistration],
			decisionOfEstablishing: [this.model.decisionOfEstablishing],
			dateOfIssue: [this.model.dateOfIssue],
			tax: [this.model.tax, [Validators.required]],
			orgProvinceId: [this.model.orgProvinceId],
			orgDistrictId: [this.model.orgDistrictId],
			orgWardsId: [this.model.orgWardsId],
			orgAddress: [this.model.orgAddress, [Validators.required]],
			orgPhone: [this.model.orgPhone, [Validators.required]],
			orgEmail: [this.model.orgEmail, [Validators.email]],
			business: [this.model.business, [Validators.required]],
		})
	}

	get f() {
		return this.formUpdateAccountInfo.controls
	}

	getUserInfo() {
		this.accountService.getUserInfo().subscribe((res) => {
			if (res.success != 'OK') {
				this.toast.error(res.message)
				return
			}
			this.model = res.result
			this.onChangeNation()

			this.child_SideLeft.model = this.model
		})
	}

	submitted = false
	onSave() {
		this.submitted = true
		let fDob: any = document.querySelector('#_dateOfBirth')
		let fDateIssue: any = document.querySelector('#_dateOfIssue')

		this.model.dateOfBirth = fDob.value
		this.model.dateOfIssue = fDateIssue.value
		this.model.fullName = this.model.representativeName

		if (!this.model.wardsId) this.model.wardsId = ''
		if (!this.model.provinceId) this.model.provinceId = ''
		if (!this.model.districtId) this.model.districtId = ''
		if (!this.model.orgProvinceId) this.model.orgProvinceId = ''
		if (!this.model.orgDistrictId) this.model.orgDistrictId = ''
		if (!this.model.orgWardsId) this.model.orgWardsId = ''

		console.log(this.model)

		if (this.formUpdateAccountInfo.invalid) {
			this.toast.error('Dữ liệu không hợp lệ')
			return
		}

		this.accountService.updateInfoUserCurrent(this.model).subscribe((res) => {
			if (res.success != 'OK') {
				this.toast.error(res.message)
				return
			}

			this.toast.success(COMMONS.UPDATE_SUCCESS)
			this.router.navigate(['/cong-bo/tai-khoan/thong-tin'])
		})
	}

	onReset() {
		this.getUserInfo()
	}

	onChangeNation(clearable = false) {
		if (clearable) {
			this.listProvince = []
			this.listDistrict = []
			this.listVillage = []

			this.listOrgDistrict = []
			this.listOrgVillage = []

			this.model.provinceId = ''
			this.model.orgProvinceId = ''
		}
		if (this.model.nation == 'Việt Nam') {
			this.diadanhService.getAllProvince().subscribe((res) => {
				if (res.success == 'OK') {
					this.listProvince = res.result.CAProvinceGetAll
					//get province
					this.onChangeProvince()
					this.onChangeOrgProvince()
				}
			})
		} else {
			if (this.model.nation == '#') {
				this.nation_enable_type = true
				this.model.nation = ''
				//
				// this.formInfo.controls.province.disable()
				// this.formInfo.controls.district.disable()
				// this.formInfo.controls.village.disable()
			}
		}
	}
	onChangeProvince(clearable = false) {
		if (clearable) {
			this.listDistrict = []
			this.listVillage = []

			this.model.districtId = ''
			this.model.wardsId = ''
		}
		if (this.model.provinceId != null && this.model.provinceId != '') {
			this.diadanhService.getAllDistrict(this.model.provinceId).subscribe((res) => {
				if (res.success == 'OK') {
					this.listDistrict = res.result.CADistrictGetAll
					//get district
					this.onChangeDistrict()
				}
			})
		} else {
		}
	}

	onChangeDistrict(clearable = false) {
		if (clearable) {
			this.listVillage = []
			this.model.wardsId = ''
		}
		if (this.model.districtId != null && this.model.districtId != '') {
			this.diadanhService.getAllVillage(this.model.provinceId, this.model.districtId).subscribe((res) => {
				if (res.success == 'OK') {
					this.listVillage = res.result.CAVillageGetAll
				}
			})
		} else {
		}
	}

	onChangeOrgProvince(clearable = false) {
		if (clearable) {
			this.listOrgDistrict = []
			this.listOrgVillage = []

			this.model.orgProvinceId = ''
			this.model.orgWardsId = ''
		}
		if (this.model.orgProvinceId != null && this.model.orgProvinceId != '') {
			this.diadanhService.getAllDistrict(this.model.orgProvinceId).subscribe((res) => {
				if (res.success == 'OK') {
					this.listOrgDistrict = res.result.CADistrictGetAll
					//get district
					this.onChangeOrgDistrict()
				}
			})
		} else {
		}
	}

	onChangeOrgDistrict(clearable = false) {
		if (clearable) {
			this.listOrgVillage = []
			this.model.orgWardsId = ''
		}
		if (this.model.orgDistrictId != null && this.model.orgDistrictId != '') {
			this.diadanhService.getAllVillage(this.model.orgProvinceId, this.model.orgDistrictId).subscribe((res) => {
				if (res.success == 'OK') {
					this.listOrgVillage = res.result.CAVillageGetAll
				}
			})
		} else {
		}
	}
}
