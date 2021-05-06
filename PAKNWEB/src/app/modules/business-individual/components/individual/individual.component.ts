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
export class IndividualComponent implements OnInit {
	listStatus: any = [
		// { value: '', text: 'Toàn bộ' },
		{ value: true, text: 'Hiệu lực' },
		{ value: false, text: 'Hết hiệu lực' },
	]
	listGender: any = [
		{ value: true, text: 'Nam' },
		{ value: false, text: 'Nữ' },
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

	createUnitFrom: FormGroup
	createIndividual: FormGroup

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

	ngOnInit() {}

	/*modal thêm / sửa đơn vị*/
	modalCreateOrUpdateTitle: string = 'Tạo mới cá nhân'
	modalCreateOrUpdate(id: any, level: any = 1, parentId: any = 0) {
		this.createUnitFrom = this.formBuilder.group({
			name: [this.modelUnit.name, Validators.required],
			unitLevel: [this.modelUnit.unitLevel, [Validators.required]],
			isActived: [this.modelUnit.isActived],
			parentId: [this.modelUnit.parentId, Validators.required],
			description: [this.modelUnit.description],
			email: [this.modelUnit.email, [Validators.required, Validators.email]], //Validators.pattern('^[a-z][a-z0-9_.]{5,32}@[a-z0-9]{2,}(.[a-z0-9]{2,4}){1,2}$')
			phone: [this.modelUnit.phone, [Validators.required, Validators.pattern('^(84|0[3|5|7|8|9])+([0-9]{8})$')]],
			address: [this.modelUnit.address],
		})

		// this.unitFormSubmitted = false
		if (id == 0) {
			this.modalCreateOrUpdateTitle = 'Tạo mới cá nhân'
			this.modelIndividual = new IndividualObject()
			this.modelIndividual.name = ''
			// if (this.modelUnit.unitLevel > 1) this.modelUnit.parentId = null
			// else {
			// 	this.modelUnit.parentId = 0
			// }
		} else {
			this.modalCreateOrUpdateTitle = 'Chỉnh sửa cá nhân'
			// this.unitService.getById({ id }).subscribe((res) => {
			// 	if (res.success != 'OK') return
			// 	this.modelUnit = res.result.CAUnitGetByID[0]
			// })
		}
		$('#modal-create-or-update').modal('show')
		// this.modelUnit.unitLevel = level
		// if (parentId > 0) this.modelUnit.parentId = parentId
	}
}
