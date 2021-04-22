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

	nation_enable_type = false
	//event
	//event
	onChangeNation() {
		this.listProvince = []
		this.listDistrict = []
		this.listVillage = []

		this.model.ProvinceId = ''
		if (this.model.Nation == 'Việt Nam') {
			this.diadanhService.getAllProvince().subscribe((res) => {
				if (res.success == 'OK') {
					this.listProvince = res.result.CAProvinceGetAll

					this.model.ProvinceId = 37
					this.model.OrgProvinceId = 37
					$('#_OrgDistrictId').click()
				}
			})
		} else {
			if (this.model.Nation == '#') {
				this.nation_enable_type = true
				this.model.Nation = ''
			}
		}
	}
	onChangeProvince() {
		this.listDistrict = []
		this.listVillage = []

		this.model.DistrictId = ''
		this.model.WardsId = ''
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

		this.model.WardsId = ''
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
			Province: [this.model.ProvinceId, []], //int
			District: [this.model.DistrictId, []], // int
			Village: [this.model.WardsId, []], // int
			Address: [this.model.Address, []],
		})

		this.onChangeNation()
	}

	email_exists: boolean = false
	idCard_exists: boolean = false
	onCheckExist(field: string, value: string) {
		this.registerService
			.businessCheckExists({
				field,
				value,
			})
			.subscribe((res) => {
				if (res.success == RESPONSE_STATUS.success) {
					if (field == 'Email') this.email_exists = res.result.BIBusinessCheckExists[0].exists
					else if (field == 'IDCard') this.idCard_exists = res.result.BIBusinessCheckExists[0].exists
				}
			})
	}
}
