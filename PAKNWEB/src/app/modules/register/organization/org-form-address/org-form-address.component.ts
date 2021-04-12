import { Component, OnInit } from '@angular/core'
import { FormGroup, FormBuilder, Validators, ValidatorFn, AbstractControl } from '@angular/forms'
import { DiadanhService } from 'src/app/services/diadanh.service'

import { OrganizationComponent } from '../organization.component'

@Component({
	selector: 'app-org-form-address',
	templateUrl: './org-form-address.component.html',
	styleUrls: ['./org-form-address.component.css'],
})
export class OrgFormAddressComponent implements OnInit {
	constructor(private parentCompo: OrganizationComponent, private formBuilder: FormBuilder, private diadanhService: DiadanhService) {}

	formOrgAddress: FormGroup
	fOrgAddressSubmitted = false
	model = this.parentCompo.model

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

	//event
	onChangeNation() {
		this.listProvince = []
		this.listDistrict = []
		this.listVillage = []

		this.model.OrgProvince = ''
		if (this.model.Nation == 1) {
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

		this.model.OrgDistrict = ''
		this.model.OrgVillage = ''
		if (this.model.OrgProvince != null && this.model.OrgProvince != '') {
			this.diadanhService.getAllDistrict(this.model.OrgProvince).subscribe((res) => {
				if (res.success == 'OK') {
					this.listDistrict = res.result.CADistrictGetAll
				}
			})
		} else {
		}
	}

	onChangeDistrict() {
		this.listVillage = []

		this.model.OrgVillage = ''
		if (this.model.OrgDistrict != null && this.model.OrgDistrict != '') {
			this.diadanhService.getAllVillage(this.model.OrgProvince, this.model.OrgDistrict).subscribe((res) => {
				if (res.success == 'OK') {
					this.listVillage = res.result.CAVillageGetAll
				}
			})
		} else {
		}
	}

	ngOnInit() {
		this.formOrgAddress = this.formBuilder.group({
			OrgProvince: [this.model.OrgDistrict, [Validators.required]], //int
			OrgDistrict: [this.model.OrgDistrict, [Validators.required]], //int
			OrgVillage: [this.model.OrgVillage, [Validators.required]], //int
			OrgAddress: [this.model.OrgAddress, [Validators.required]],
			OrgPhone: [this.model.OrgPhone, [Validators.required, Validators.pattern(/^(84|0[3|5|7|8|9])+([0-9]{8})$/g)]],
			OrgEmail: [this.model.OrgEmail, [Validators.required, Validators.email]],
		})
	}
}
