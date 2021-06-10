import { Component, OnInit } from '@angular/core'
import { FormGroup, FormBuilder, Validators, ValidatorFn, AbstractControl } from '@angular/forms'

import { DiadanhService } from 'src/app/services/diadanh.service'
import { RegisterService } from 'src/app/services/register.service'

import { OrganizationObject } from 'src/app/models/RegisterObject'
import { MESSAGE_COMMON, PROCESS_STATUS_RECOMMENDATION, RECOMMENDATION_STATUS, RESPONSE_STATUS, STEP_RECOMMENDATION } from 'src/app/constants/CONSTANTS'

declare var $: any
@Component({
	selector: 'app-org-repre-form',
	templateUrl: './org-repre-form.component.html',
	styleUrls: ['./org-repre-form.component.css'],
})
export class OrgRepreFormComponent implements OnInit {
	constructor(private formBuilder: FormBuilder, private diadanhService: DiadanhService, private registerService: RegisterService) {}

	dateNow: Date = new Date()
	formInfo: FormGroup
	fInfoSubmitted = false

	public model: OrganizationObject = new OrganizationObject()

	get fInfo() {
		return this.formInfo.controls
	}

	///
	listNation: any[] = [{ id: 'Việt Nam', name: 'Việt Nam' }]
	listProvince: any[] = []
	listDistrict: any[] = []
	listVillage: any[] = []
	listGender: any[] = [
		{ value: true, text: 'Nam' },
		{ value: false, text: 'Nữ' },
	]

	isOtherNation = false
	//event
	//event
	onResetNationValue(event: any) {
		if (event.target.value == 'Nhập...') {
			event.target.value = ''
		}
	}
	backToVal() {
		this.isOtherNation = false
		this.model.Nation = null
		this.formInfo.controls['Province'].setValue(null)
		this.formInfo.controls['District'].setValue(null)
		this.formInfo.controls['Village'].setValue(null)
	}
	onChangeNation() {
		this.listProvince = []
		this.listDistrict = []
		this.listVillage = []

		this.model.ProvinceId = null
		if (this.model.Nation == 'Việt Nam') {
			this.diadanhService.getAllProvince().subscribe((res) => {
				if (res.success == 'OK') {
					this.listProvince = res.result.CAProvinceGetAll

					this.model.ProvinceId = 37
					this.model.OrgProvinceId = 37
					this.onChangeProvince()
				}
			})
		} else {
			if (this.model.Nation == '#') {
				this.isOtherNation = true
				this.model.Nation = 'Nhập...'
				this.formInfo.controls['Province'].setValue(0)
				this.formInfo.controls['District'].setValue(0)
				this.formInfo.controls['Village'].setValue(0)
			}
		}
	}
	onChangeProvince() {
		this.listDistrict = []
		this.listVillage = []

		this.model.DistrictId = null
		this.model.WardsId = null
		if (this.model.ProvinceId != null && this.model.ProvinceId != '') {
			this.diadanhService.getAllDistrict(this.model.ProvinceId).subscribe((res) => {
				if (res.success == 'OK') {
					this.listDistrict = res.result.CADistrictGetAll
				}
			})
		} else {
		}
	}

	onChangeDistrict() {
		this.listVillage = []

		this.model.WardsId = null
		if (this.model.DistrictId != null && this.model.DistrictId != '') {
			this.diadanhService.getAllVillage(this.model.ProvinceId, this.model.DistrictId).subscribe((res) => {
				if (res.success == 'OK') {
					this.listVillage = res.result.CAVillageGetAll
				}
			})
		} else {
		}
	}

	ngOnInit() {
		//form thông tin nguoi dai dien
		this.formInfo = this.formBuilder.group({
			//----thông tin người đại diện
			RepresentativeName: [this.model.RepresentativeName, [Validators.required, Validators.maxLength(100)]], // tên người đại diện
			Email: [this.model.Email, [Validators.email]],
			Gender: [this.model.RepresentativeGender, [Validators.required]],
			DOB: [this.model._RepresentativeBirthDay, []],
			Nation: [this.model.Nation, [Validators.required]],
			Province: [this.model.ProvinceId, [Validators.required]], //int
			District: [this.model.DistrictId, [Validators.required]], // int
			Village: [this.model.WardsId, [Validators.required]], // int
			Address: [this.model.Address, []],
		})

		this.onChangeNation()
	}

	resetObject() {
		this.model = new OrganizationObject()
		this.fInfoSubmitted = false
		this.rebuidForm()
	}

	rebuidForm() {
		this.formInfo.reset({
			RepresentativeName: this.model.RepresentativeName,
			Email: this.model.Email,
			Gender: this.model.RepresentativeGender,
			DOB: this.model._RepresentativeBirthDay,
			Nation: this.model.Nation,
			Province: this.model.ProvinceId,
			District: this.model.DistrictId,
			Village: this.model.WardsId,
			Address: this.model.Address,
		})
	}

	checkExists = {
		Email: false,
		IDCard: false,
	}
	onCheckExist(field: string, value: string) {
		if (value == null || value == '') {
			this.checkExists[field] = false
			return
		}
		this.registerService
			.businessCheckExists({
				field,
				value,
				id: 0,
			})
			.subscribe((res) => {
				if (res.success == RESPONSE_STATUS.success) {
					this.checkExists[field] = res.result.BIBusinessCheckExists[0].exists
				}
			})
	}
}
