import { Component, OnInit } from '@angular/core'
import { FormGroup, FormBuilder, Validators, ValidatorFn, AbstractControl } from '@angular/forms'
import { DatepickerOptions } from 'ng2-datepicker';

import { DiadanhService } from 'src/app/services/diadanh.service'
import { OrganizationComponent } from '../organization.component'

@Component({
	selector: 'app-org-repre-form',
	templateUrl: './org-repre-form.component.html',
	styleUrls: ['./org-repre-form.component.css'],
})
export class OrgRepreFormComponent implements OnInit {
	constructor(private parentCompo: OrganizationComponent, private formBuilder: FormBuilder, private diadanhService: DiadanhService) {}

	date:Date = new Date()
	datePickerConfig:DatepickerOptions={
		addClass: 'form-control border-brown',
		placeholder:'Nhập...',
		barTitleFormat: 'MM YYYY',
		firstCalendarDay: 1,
		barTitleIfEmpty: (`${this.date.getMonth()+1}`.includes('0')?`${this.date.getMonth()+1}`:`0${this.date.getMonth()+1}`)+` ${this.date.getFullYear()}`,
		displayFormat: 'DD/MM/YYYY',
	}

	formInfo: FormGroup
	fInfoSubmitted = false

	model = this.parentCompo.model

	get fInfo() {
		return this.formInfo.controls
	}

	///
	listNation: any[] = [{ id: 1, name: 'Việt Nam' }]
	listProvince: any[] = []
	listDistrict: any[] = []
	listVillage: any[] = []
	listGender: any[] = [
		{ value: true, text: 'Nam' },
		{ value: false, text: 'Nữ' },
	]

	//event
	//event
	onChangeNation() {
		this.listProvince = []
		this.listDistrict = []
		this.listVillage = []

		this.model.Province = ''
		if (this.model.Nation == 1) {
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

		this.model.District = ''
		this.model.Village = ''
		if (this.model.Province != null && this.model.Province != '') {
			this.diadanhService.getAllDistrict(this.model.Province).subscribe((res) => {
				if (res.success == 'OK') {
					this.listDistrict = res.result.CADistrictGetAll
				}
			})
		} else {
		}
	}

	onChangeDistrict() {
		this.listVillage = []

		this.model.Village = ''
		if (this.model.District != null && this.model.District != '') {
			this.diadanhService.getAllVillage(this.model.Province, this.model.District).subscribe((res) => {
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
			RepresentativeName: [this.model.RepresentativeName, [Validators.required]], // tên người đại diện
			Email: [this.model.Email, [Validators.required, Validators.email]],
			Gender: [this.model.Gender, [Validators.required]],
			DOB: [this.model.DOB, [Validators.required]],
			Nation: [this.model.Nation, [Validators.required]],
			Province: [this.model.Province, [Validators.required]], //int
			District: [this.model.District, [Validators.required]], // int
			Village: [this.model.Village, [Validators.required]], // int
			Address: [this.model.Address, [Validators.required]],
		})
	}
}
