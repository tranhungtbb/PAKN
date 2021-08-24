import { Component, OnInit, AfterViewInit } from '@angular/core'
import { FormGroup, FormBuilder, Validators } from '@angular/forms'
import { DiadanhService } from 'src/app/services/diadanh.service'
import { RegisterService } from 'src/app/services/register.service'
import { OrganizationObject } from 'src/app/models/businessIndividualObject'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { CreateUpdBusinessComponent } from '../create-upd-business.component'

declare var $: any
@Component({
	selector: 'app-org-repre-form',
	templateUrl: './org-repre-form.component.html',
	styleUrls: ['./org-repre-form.component.css'],
})
export class OrgRepreFormComponent implements OnInit, AfterViewInit {
	constructor(private formBuilder: FormBuilder, private diadanhService: DiadanhService, private registerService: RegisterService) {}

	dateNow: Date = new Date()
	formInfo: FormGroup
	fInfoSubmitted = false

	public parent: CreateUpdBusinessComponent
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
	backToSelectBox() {
		this.nation_enable_type = false
		this.model.Nation = 'Việt Nam'
		this.onChangeNation()
		this.model.ProvinceId = null
		this.model.DistrictId = null
		this.model.WardsId = null
		this.model.OrgProvinceId = null
		this.model.OrgDistrictId = null
		this.model.OrgWardsId = null
	}
	resetNationField() {
		if (this.model.Nation == 'Nhập...') this.model.Nation = ''
	}
	onChangeNation() {
		this.listProvince = []
		this.listDistrict = []
		this.listVillage = []

		//update value
		if (this.model.Nation == 'Việt Nam') {
			this.diadanhService.getAllProvince().subscribe((res) => {
				if (res.success == 'OK') {
					this.listProvince = res.result.CAProvinceGetAll

					this.model.ProvinceId = 37
					this.model.OrgProvinceId = 37
					$('#_OrgDistrictId').click()
					this.onChangeProvince()
					this.parent.child_OrgAddressForm.onChangeProvince()
				}
			})
		} else {
			if (this.model.Nation == '#') {
				this.nation_enable_type = true
				this.model.Nation = ''

				this.model.ProvinceId = 0
				this.model.DistrictId = 0
				this.model.WardsId = 0
			}
		}
		//update state for child
		this.parent.child_OrgAddressForm.nation_enable_type = this.nation_enable_type
		this.parent.nation_enable_type = this.nation_enable_type
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
			DOB: [this.model.RepresentativeBirthDay, []],
			Nation: [this.model.Nation, [Validators.required]],
			Province: [this.model.ProvinceId, [Validators.required]], //int
			District: [this.model.DistrictId, [Validators.required]], // int
			Village: [this.model.WardsId, [Validators.required]], // int
			Address: [this.model.Address, []],
			phone: [this.model.phone, [Validators.required, Validators.pattern(/^(84|0[3|5|7|8|9])+([0-9]{8})$/)]],
		})
	}

	onResetNationValue(event: any) {
		if (event.target.value == 'Nhập...') {
			event.target.value = ''
		}
	}

	ngAfterViewInit() {
		this.model = this.parent.model
		if (!this.model.id) {
			this.onChangeNation()
		}
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
