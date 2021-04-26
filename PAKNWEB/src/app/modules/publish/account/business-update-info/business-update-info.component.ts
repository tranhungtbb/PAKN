import { Component, OnInit, ViewChild } from '@angular/core'
import { Router } from '@angular/router'
import { ToastrService } from 'ngx-toastr'
import { AccountService } from 'src/app/services/account.service'
import { FormBuilder, FormControl, FormGroup, Validator, Validators } from '@angular/forms'

import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
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
		private formBuider: FormBuilder,
		private toast: ToastrService,
		private router: Router,
		private accountService: AccountService,
		private storageService: UserInfoStorageService,
		private authenService: AuthenticationService,
		private sharedataService: DataService,
		private diadanhService: DiadanhService
	) {}

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

	ngOnInit() {
		this.getUserInfo()
		this.formUpdateAccountInfo = this.formBuider.group({
			representativeName: [this.model.representativeName],
			representativeBirthDay: [this.model.representativeBirthDay],
			email: [this.model.email],
			nation: [this.model.nation],
			provinceId: [this.model.provinceId],
			districtId: [this.model.districtId],
			wardsId: [this.model.wardsId],
			address: [this.model.address],
			representativeGender: [this.model.representativeGender],
			fullName: [this.model.fullName],
			businessRegistration: [this.model.businessRegistration],
			decisionOfEstablishing: [this.model.decisionOfEstablishing],
			dateOfIssue: [this.model.dateOfBirth],
			tax: [this.model.tax],
			orgProvinceId: [this.model.orgProvinceId],
			orgDistrictId: [this.model.orgDistrictId],
			orgWardsId: [this.model.orgWardsId],
			orgAddress: [this.model.orgAddress],
			orgPhone: [this.model.orgPhone],
			orgEmail: [this.model.orgEmail],
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

		this.model.dateOfBirth = document.querySelector('#_dateOfBirth').nodeValue
		this.model.dateOfIssue = document.querySelector('#_dateOfIssue').nodeValue

		console.log(this.model)

		if (this.formUpdateAccountInfo.invalid) {
			this.toast.error('Dữ liệu không hợp lệ')
			return
		}
	}

	onChangeNation(clearable = false) {
		if (clearable) {
			this.listProvince = []
			this.listDistrict = []
			this.listVillage = []

			this.listOrgDistrict = []
			this.listOrgVillage = []

			this.model.provinceId = ''
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
