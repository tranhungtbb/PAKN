import { Component, OnInit, AfterViewInit } from '@angular/core'
import { FormGroup, FormBuilder, Validators } from '@angular/forms'
import { DiadanhService } from 'src/app/services/diadanh.service'
import { RegisterService } from 'src/app/services/register.service'
import { OrganizationObject } from 'src/app/models/businessIndividualObject'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { CreateUpdBusinessComponent } from '../create-upd-business.component'

@Component({
	selector: 'app-org-form-address',
	templateUrl: './org-form-address.component.html',
	styleUrls: ['./org-form-address.component.css'],
})
export class OrgFormAddressComponent implements OnInit, AfterViewInit {
	constructor(private formBuilder: FormBuilder, private diadanhService: DiadanhService, private registerService: RegisterService) {}
	formOrgAddress: FormGroup
	fOrgAddressSubmitted = false
	public parent: CreateUpdBusinessComponent
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

	onChangeProvince() {
		this.listDistrict = []
		this.listVillage = []

		this.model.OrgDistrictId = null
		this.model.OrgWardsId = null
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

		this.model.OrgWardsId = null
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
		this.onChangeNation()
		this.formOrgAddress = this.formBuilder.group({
			OrgProvince: [this.model.OrgProvinceId, []], //int
			OrgDistrict: [this.model.OrgDistrictId, [Validators.required]], //int
			OrgVillage: [this.model.OrgWardsId, [Validators.required]], //int
			OrgAddress: [this.model.OrgAddress, [Validators.required]],
			OrgPhone: [this.model.OrgPhone, [Validators.required]], //, Validators.pattern(/^(84|0[3|5|7|8|9])+([0-9]{8})$/g)
			OrgEmail: [this.model.OrgEmail, [Validators.email]],
		})
	}
	ngAfterViewInit() {
		this.model = this.parent.model
	}

	// orgPhone_exists = false
	// orgEmail_exists = false
	checkExists = {
		OrgPhone: false,
		OrgEmail: false,
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
