import { Component, OnInit } from '@angular/core'
import { FormGroup, FormBuilder, Validators, ValidatorFn, AbstractControl } from '@angular/forms'
import { DiadanhService } from 'src/app/services/diadanh.service'
import { RegisterService } from 'src/app/services/register.service'

import { OrganizationComponent } from '../organization.component'
import { OrganizationObject } from 'src/app/models/RegisterObject'
import { MESSAGE_COMMON, PROCESS_STATUS_RECOMMENDATION, RECOMMENDATION_STATUS, RESPONSE_STATUS, STEP_RECOMMENDATION } from 'src/app/constants/CONSTANTS'

@Component({
	selector: 'app-org-form-address',
	templateUrl: './org-form-address.component.html',
	styleUrls: ['./org-form-address.component.css'],
})
export class OrgFormAddressComponent implements OnInit {
	constructor(private formBuilder: FormBuilder, private diadanhService: DiadanhService, private registerService: RegisterService) {}
	formOrgAddress: FormGroup
	fOrgAddressSubmitted = false
	public model: OrganizationObject = new OrganizationObject()

	get fOrgAdr() {
		return this.formOrgAddress.controls
	}

	///
	listProvince: any[] = []
	listDistrict: any[] = []
	listVillage: any[] = []
	listGender: any[] = [
		{ value: true, text: 'Nam' },
		{ value: false, text: 'Nữ' },
	]

	nation_enable_type = false
	//event
	onChangeNation() {
		this.listProvince = []
		this.listDistrict = []
		this.listVillage = []

		this.model.OrgProvinceId = ''
		if (this.model.Nation == 'Việt Nam') {
			this.diadanhService.getAllProvince().subscribe((res) => {
				if (res.success == 'OK') {
					this.listProvince = res.result.CAProvinceGetAll
				}
			})
		} else {
		}
	}
	onClickProvince() {
		if (this.listProvince == null || this.listProvince.length == 0) {
			this.onChangeNation()
		}
	}
	onChangeProvince() {
		this.listDistrict = []
		this.listVillage = []

		this.model.OrgDistrictId = ''
		this.model.OrgWardsId = ''
		if (this.model.OrgProvinceId != null && this.model.OrgProvinceId != '') {
			this.diadanhService.getAllDistrict(this.model.OrgProvinceId).subscribe((res) => {
				if (res.success == 'OK') {
					this.listDistrict = res.result.CADistrictGetAll
				}
			})
		} else {
		}
	}

	onChangeDistrict() {
		this.listVillage = []

		this.model.OrgWardsId = ''
		if (this.model.OrgDistrictId != null && this.model.OrgDistrictId != '') {
			this.diadanhService.getAllVillage(this.model.OrgProvinceId, this.model.OrgDistrictId).subscribe((res) => {
				if (res.success == 'OK') {
					this.listVillage = res.result.CAVillageGetAll
				}
			})
		} else {
		}
	}

	ngOnInit() {
		this.formOrgAddress = this.formBuilder.group({
			OrgProvince: [this.model.OrgProvinceId, []], //int
			OrgDistrict: [this.model.OrgDistrictId, []], //int
			OrgVillage: [this.model.OrgWardsId, []], //int
			OrgAddress: [this.model.OrgAddress, [Validators.required]],
			OrgPhone: [this.model.OrgPhone, [Validators.required]], //, Validators.pattern(/^(84|0[3|5|7|8|9])+([0-9]{8})$/g)
			OrgEmail: [this.model.OrgEmail, [Validators.email]],
		})
	}

	orgPhone_exists = false
	orgEmail_exists = false
	onCheckExist(field: string, value: string) {
		this.registerService
			.businessCheckExists({
				field,
				value,
			})
			.subscribe((res) => {
				if (res.success == RESPONSE_STATUS.success) {
					if (field == 'OrgPhone') this.orgPhone_exists = res.result.BIBusinessCheckExists[0].exists
					else if (field == 'OrgEmail') this.orgEmail_exists = res.result.BIBusinessCheckExists[0].exists
				}
			})
	}
}
