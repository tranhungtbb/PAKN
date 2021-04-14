import { Component, OnInit } from '@angular/core'
import { Router } from '@angular/router'
import { ToastrService } from 'ngx-toastr'
import { AccountService } from 'src/app/services/account.service'

import { FormBuilder, FormControl, FormGroup, Validator, Validators } from '@angular/forms'
import { DataService } from 'src/app/services/sharedata.service'
import { DiadanhService } from 'src/app/services/diadanh.service'
import { UserInfoObject } from 'src/app/models/UserObject'
import { resultMemoize } from '@ngrx/store'
import { COMMONS } from 'src/app/commons/commons'

@Component({
	selector: 'app-account-update-info',
	templateUrl: './account-update-info.component.html',
	styleUrls: ['./account-update-info.component.css'],
})
export class AccountUpdateInfoComponent implements OnInit {
	constructor(
		private formBuider: FormBuilder,
		private toast: ToastrService,
		private accountService: AccountService,
		private router: Router,
		private diadanhService: DiadanhService
	) {}

	formData: FormGroup
	model: UserInfoObject = new UserInfoObject()
	modeCopy: UserInfoObject

	listNation: any[] = [{ id: '1', name: 'Việt Nam' }]
	listProvince: any[] = []
	listDistrict: any[] = []
	listVillage: any[] = []

	listGender: any[] = [
		{ value: true, text: 'Nam' },
		{ value: false, text: 'Nữ' },
	]

	//event
	onChangeNation(clearable = false) {
		if (clearable) {
			this.listProvince = []
			this.listDistrict = []
			this.listVillage = []

			this.model.provinceId = ''
		}
		if (this.model.nation == 1 || this.model.nation == '1') {
			this.diadanhService.getAllProvince().subscribe((res) => {
				if (res.success == 'OK') {
					this.listProvince = res.result.CAProvinceGetAll
					//get province
					this.onChangeProvince()
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

	get f() {
		return this.formData.controls
	}
	ngOnInit() {
		this.getUserInfo()
		this.formData = this.formBuider.group({
			//userName: [this.model.userName, [Validators.required]],
			fullName: [this.model.fullName, [Validators.required]],
			dateOfBirth: [this.model.dateOfBirth, [Validators.required]],
			email: [this.model.email, [Validators.required, Validators.email]],
			//phone: [this.model.phone, [Validators.required,Validators.pattern(/^(84|0[3|5|7|8|9])+([0-9]{8})$/g)]],
			nation: [this.model.nation, [Validators.required]],
			provinceId: [this.model.provinceId, [Validators.required]],
			districtId: [this.model.districtId, [Validators.required]],
			wardsId: [this.model.wardsId, [Validators.required]],
			address: [this.model.address, [Validators.required]],
			idCard: [this.model.idCard, [Validators.required, Validators.pattern(/^([0-9]){9,12}$/g)]],
			issuedPlace: [this.model.issuedPlace, [Validators.required]],
			issuedDate: [this.model.issuedDate, [Validators.required]],
			gender: [this.model.gender, [Validators.required]],
		})
	}

	getUserInfo() {
		this.accountService.getUserInfo().subscribe((res) => {
			if (res.success != 'OK') {
				this.toast.error(res.message)
				return
			}
			this.model = res.result
			this.model = res.result
			this.onChangeNation()
		})
	}

	onReSet() {
		this.model = { ...this.modeCopy }
	}
	submitted = false
	onSave() {
		this.submitted = true

		let fDob: any = document.querySelector('#_dateOfBirth')
		let fDateIssue: any = document.querySelector('#_issuedDate')

		this.model.dateOfBirth = fDob.value
		this.model.issuedDate = fDateIssue.value

		if (this.formData.invalid) {
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
}
