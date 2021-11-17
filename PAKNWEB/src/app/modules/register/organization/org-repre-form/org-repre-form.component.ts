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
		this.model.nation = null
		this.formInfo.controls['Province'].setValue(null)
		this.formInfo.controls['District'].setValue(null)
		this.formInfo.controls['Village'].setValue(null)
	}

	ngOnInit() {
		//form thông tin nguoi dai dien
		this.formInfo = this.formBuilder.group({
			//----thông tin người đại diện
			representativeName: [this.model.representativeName, [Validators.required, Validators.maxLength(100)]], // tên người đại diện
			email: [this.model.email, [Validators.email]],
			phone: [this.model.phone],
			address: [this.model.address, []],
		})
	}

	resetObject() {
		this.model = new OrganizationObject()
		this.fInfoSubmitted = false
		this.rebuidForm()
	}

	rebuidForm() {
		this.formInfo.reset({
			representativeName: this.model.representativeName,
			email: this.model.email,
			address: this.model.address,
			phone: this.model.phone,
		})
	}

	checkExists = {
		Email: false,
		IDCard: false,
		Phone: false,
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
