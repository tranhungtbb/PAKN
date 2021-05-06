import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core'
import { FormGroup, FormBuilder, Validators, FormArray, FormControl } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'
import { MatDialog, MatDialogModule } from '@angular/material'
import { TreeNode } from 'primeng/api'
import { REGEX } from 'src/app/constants/CONSTANTS'

import { COMMONS } from 'src/app/commons/commons'
import { UnitObject } from 'src/app/models/unitObject'
import { IndividualObject } from 'src/app/models/individualObject'

declare var jquery: any
declare var $: any
@Component({
	selector: 'app-individual',
	templateUrl: './individual.component.html',
	styleUrls: ['./individual.component.css'],
})
export class IndividualComponent implements OnInit, AfterViewInit {
	listStatus: any = [
		{ value: 0, text: 'Hết hiệu lực' },
		{ value: 1, text: 'Hiệu lực' },
	]
	listGender: any = [
		{ value: true, text: 'Nữ' },
		{ value: false, text: 'Nam' },
	]

	regex_phone = REGEX.PHONE_VN

	treeUnit: any[]
	listUnitPaged: any[] = []
	unitObject: any = {}
	individualObject: any = {}
	listUserPaged: any[] = []
	unitFlatlist: any[] = []
	rolesList: any[] = []
	positionsList: any[] = []

	createIndividualForm: FormGroup

	modelUnit: UnitObject = new UnitObject()
	modelIndividual: IndividualObject = new IndividualObject()

	/*unit query*/
	query: any = {
		pageSize: 20,
		pageIndex: 1,
		parentId: '',
		unitLevel: '',
		name: '',
		phone: '',
		email: '',
		address: '',
		isActived: null,
	}

	//sort
	unitSortDir = 'DESC'
	unitSortField = 'ID'

	individualSortDir = 'DESC'
	individualField = 'ID'

	usSortDir = 'DESC'
	usSortField = 'ID'

	totalCount_Unit: number = 0
	unitPageCount: number = 0

	/*user query*/
	queryUser: any = {
		pageSize: 20,
		pageIndex: 1,
		userName: '',
		email: '',
		fullName: '',
		phone: '',
		isActived: null,
	}
	totalCount_User: number = 0
	userPageCount: number = 0

	constructor(private formBuilder: FormBuilder, private _toastr: ToastrService, private dialog: MatDialog) {}

	ngOnInit() {
		/*individual form*/
		this.createIndividualForm = this.formBuilder.group({
			name: [this.modelIndividual.name, Validators.required],
			email: [this.modelIndividual.email, [Validators.required, Validators.email]],
			phone: [this.modelIndividual.phone, [Validators.required, Validators.pattern('^(84|0[3|5|7|8|9])+([0-9]{8})$')]],
			identity: [this.modelIndividual.identity, Validators.required],
			placeIssue: [this.modelIndividual.placeIssue],
			dateIssue: [this.modelIndividual.dateIssue],
			nation: [this.modelIndividual.nation, Validators.required],
			province: [this.modelIndividual.province, Validators.required],
			district: [this.modelIndividual.district, Validators.required],
			village: [this.modelIndividual.village, Validators.required],
			address: [this.modelIndividual.address, Validators.required],
			gender: [this.modelIndividual.gender, Validators.required],
			dOB: [this.modelIndividual.dOB, Validators.required],
			status: [this.modelIndividual.status],
		})
	}
	ngAfterViewInit() {
		// this.childCreateOrUpdateUser.parentUnit = this
	}

	/*modal thêm / sửa cá nhân*/
	modalCreateOrUpdateTitle: string = 'Tạo mới cá nhân'
	modalCreateOrUpdate(id: any, level: any = 1, parentId: any = 0) {
		this.createIndividualForm = this.formBuilder.group({
			name: [this.modelIndividual.name, Validators.required],
			email: [this.modelIndividual.email, [Validators.required, Validators.email]],
			phone: [this.modelIndividual.phone, [Validators.required, Validators.pattern('^(84|0[3|5|7|8|9])+([0-9]{8})$')]],
			identity: [this.modelIndividual.identity, Validators.required],
			placeIssue: [this.modelIndividual.placeIssue],
			dateIssue: [this.modelIndividual.dateIssue],
			nation: [this.modelIndividual.nation, Validators.required],
			province: [this.modelIndividual.province, Validators.required],
			district: [this.modelIndividual.district, Validators.required],
			village: [this.modelIndividual.village, Validators.required],
			address: [this.modelIndividual.address, Validators.required],
			gender: [this.modelIndividual.gender, Validators.required],
			dOB: [this.modelIndividual.dOB, Validators.required],
			status: [this.modelIndividual.status, Validators.required],
		})

		this.individualFormSubmitted = false
		if (id == 0) {
			this.modalCreateOrUpdateTitle = 'Tạo mới cá nhân'
			this.modelIndividual = new IndividualObject()
			this.modelIndividual.name = ''
		} else {
			this.modalCreateOrUpdateTitle = 'Chỉnh sửa cá nhân'
			// this.unitService.getById({ id }).subscribe((res) => {
			// 	if (res.success != 'OK') return
			// 	this.modelUnit = res.result.CAUnitGetByID[0]
			// })
		}
		$('#modal-create-or-update').modal('show')
	}

	get fIndividual() {
		return this.createIndividualForm.controls
	}

	individualFormSubmitted = false
	onSaveUnit() {
		this.individualFormSubmitted = true

		if (this.createIndividualForm.invalid) {
			this._toastr.error('Dữ liệu không hợp lệ')
			return
		}
	}
}
