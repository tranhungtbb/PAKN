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
import { DiadanhService } from 'src/app/services/diadanh.service'
import { AccountSideLeftComponent } from '../account-side-left/account-side-left.component'
import { BussinessUpdateModel} from 'src/app/models/businessIndividualObject'

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
		private diadanhService: DiadanhService,
		private userLocal: UserInfoStorageService
	) {
		defineLocale('vi', viLocale)
		this.localeService.use('vi')
	}

	formUpdateAccountInfo: FormGroup

	model: any = new BussinessUpdateModel()
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
			address: [this.model.address],
			representativeGender: [this.model.representativeGender],
			businessRegistration: [this.model.businessRegistration],
			decisionOfEstablishing: [this.model.decisionOfEstablishing],
			dateOfIssue: [this.model.dateOfIssue],
			tax: [this.model.tax, [Validators.required]],
			orgProvinceId: [this.model.orgProvinceId, [Validators.required]],
			orgDistrictId: [this.model.orgDistrictId , [Validators.required]],
			orgWardsId: [this.model.orgWardsId, [Validators.required]],
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
			this.model._representativeBirthDay  = this.model.representativeBirthDay != null ? new Date(this.model.representativeBirthDay) : null
			this.model._dateOfIssue = this.model.dateOfIssue != null ? new Date(this.model.dateOfIssue) : null
			if (this.model.nation == 'Việt Nam') {
				this.nation_enable_type = false
			} else {
				this.nation_enable_type = true
			}
			this.onChangeNation()
			this.getAllProvice()
			// this.child_SideLeft.model = this.model
		})
	}
	resetNationField(event: any) {
		if (event.target.value == 'Nhập...') event.target.value = ''
	}
	submitted = false
	onSave() {
		this.submitted = true
		// let fDob: any = document.querySelector('#_dateOfBirth')
		// let fDateIssue: any = document.querySelector('#_dateOfIssue')
		if (this.model.nation == 'Nhập...') {
			this.model.nation = ''
		}
		// this.model.dateOfBirth = fDob.value
		// this.model.dateOfIssue = fDateIssue.value
		this.model.fullName = this.model.business
		this.model.idCard == null ? (this.model.idCard = '') : (this.model.idCard = this.model.idCard)
		this.model.businessRegistration == null ? (this.model.businessRegistration = '') : (this.model.businessRegistration = this.model.businessRegistration)
		this.model.decisionOfEstablishing == null ? (this.model.decisionOfEstablishing = '') : (this.model.decisionOfEstablishing = this.model.decisionOfEstablishing)
		this.model.nativePlace == null ? (this.model.nativePlace = '') : (this.model.nativePlace = this.model.nativePlace)
		if (!this.model.orgEmail) this.model.orgEmail = ''
		if (!this.model.email) this.model.email = ''
		
		
		if (this.formUpdateAccountInfo.invalid) {
			return
		}
		if(!this.nation_enable_type && (!this.model.wardsId || !this.model.provinceId || !this.model.districtId ))
		{
			return
		}

		this.model.wardsId = this.model.wardsId == null ? '' : this.model.wardsId;
		this.model.provinceId = this.model.provinceId == null ? '' : this.model.provinceId;
		this.model.districtId = this.model.districtId == null ? '' : this.model.districtId;
		this.model.representativeBirthDay = this.model._representativeBirthDay == null ? '': JSON.stringify(new Date(this.model._representativeBirthDay)).slice(1, 11);
		this.model.dateOfBirth = this.model._dateOfIssue == null ? '': JSON.stringify(new Date(this.model._dateOfIssue)).slice(1, 11);
		


		this.accountService.updateInfoUserCurrent(this.model).subscribe((res) => {
			if (res.success != 'OK') {
				this.toast.error(res.message)
				return
			}
			this.userLocal.setFullName(this.model.fullName)
			this.toast.success(COMMONS.UPDATE_SUCCESS)
			// this.router.navigate(['/cong-bo/tai-khoan/thong-tin'])
			window.location.href = '/cong-bo/tai-khoan/thong-tin';
		})
	}

	onReset() {
		this.getUserInfo()
	}

	getAllProvice() {
		this.diadanhService.getAllProvince().subscribe((res) => {
			if (res.success == 'OK') {
				this.listProvince = res.result.CAProvinceGetAll
				this.onChangeOrgProvince()
			}
		})
	}

	backToDfVal() {
		this.nation_enable_type = false
		this.model.nation = ''
		this.formUpdateAccountInfo.controls['provinceId'].setValue(null)
		this.formUpdateAccountInfo.controls['districtId'].setValue(null)
		this.formUpdateAccountInfo.controls['wardsId'].setValue(null)
	}
	onChangeNation(clearable = false) {
		if (clearable) {
			this.listProvince = []
			this.listDistrict = []
			this.listVillage = []

			// this.listOrgDistrict = []
			// this.listOrgVillage = []

			this.model.provinceId = null
			//this.model.orgProvinceId = null
			this.model.wardsId = null
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
				this.model.ProvinceId = 0
				this.model.DistrictId = 0
				this.model.WardsId = 0
				//
				// this.formInfo.controls.province.disable()
				// this.formInfo.controls.district.disable()
				// this.formInfo.controls.village.disable()
			}
		}
	}

	getAllDiaDanhOfProvince(id: any) {
		this.diadanhService.getAllByProvinceId(id).subscribe((res) => {
			console.log(res)
		})
	}

	onChangeProvince(clearable = false) {
		if (clearable) {
			this.listDistrict = []
			this.listVillage = []

			this.model.districtId = null
			this.model.wardsId = null
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
			this.model.wardsId = null
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

			this.model.orgDistrictId = null
			this.model.orgWardsId = null
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
			this.model.orgWardsId = null
		}
		if (this.model.orgDistrictId != null && this.model.orgDistrictId != '') {
			this.diadanhService.getAllVillage(this.model.orgProvinceId, this.model.orgDistrictId).subscribe((res) => {
				if (res.success == 'OK') {
					this.listOrgVillage = res.result.CAVillageGetAll
					console.log(this.listOrgVillage)
				}
			})
		} else {
		}
	}
}
