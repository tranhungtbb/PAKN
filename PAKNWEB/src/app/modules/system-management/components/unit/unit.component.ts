import { NullTemplateVisitor } from '@angular/compiler'
import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core'
import { FormGroup, FormBuilder, Validators, FormArray, FormControl } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'
import { MatDialog, MatDialogModule } from '@angular/material'
import { TreeNode } from 'primeng/api'

import { UnitService } from '../../../../services/unit.service'
import { UserService } from '../../../../services/user.service'
import { PositionService } from '../../../../services/position.service'
import { RoleService } from '../../../../services/role.service'

import { UserCreateOrUpdateComponent } from '../user/user-create-or-update/user-create-or-update.component'

import { COMMONS } from 'src/app/commons/commons'
import { UnitObject } from 'src/app/models/unitObject'
import { UserObject2 } from 'src/app/models/UserObject'

declare var jquery: any
declare var $: any
@Component({
	selector: 'app-unit',
	templateUrl: './unit.component.html',
	styleUrls: ['./unit.component.css'],
})
export class UnitComponent implements OnInit, AfterViewInit {
	listStatus: any = [
		{ value: true, text: 'Hiệu lực' },
		{ value: false, text: 'Không hiệu lực' },
	]
	listGender: any = [
		{ value: true, text: 'Nam' },
		{ value: false, text: 'Nữ' },
	]

	treeUnit: TreeNode[]
	listUnitPaged: any[] = []
	unitObject: any = {}
	listUserPaged: any[] = []
	unitFlatlist: any[] = []
	rolesList: any[] = []
	positionsList: any[] = []

	createUnitFrom: FormGroup
	createUserForm: FormGroup

	modelUnit: UnitObject = new UnitObject()
	modelUser: UserObject2 = new UserObject2()

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
		isActive: '',
	}
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
		isActived: '',
	}
	totalCount_User: number = 0
	userPageCount: number = 0

	constructor(
		private unitService: UnitService,
		private userService: UserService,
		private positionService: PositionService,
		private formBuilder: FormBuilder,
		private _toastr: ToastrService,
		private dialog: MatDialog,
		private roleService: RoleService
	) {}

	ngOnInit() {
		this.getAllUnitShortInfo()
		/*unit form*/
		this.createUnitFrom = this.formBuilder.group({
			name: ['', Validators.required],
			unitLevel: ['', [Validators.required]],
			isActived: [''],
			isDeleted: [''],
			parentId: [''],
			description: [''],
			email: ['', [Validators.required, Validators.pattern('^[a-z][a-z0-9_.]{2,32}@[a-z0-9]{2,}(.[a-z0-9]{2,4}){1,2}$')]], //Validators.pattern('^[a-z][a-z0-9_.]{5,32}@[a-z0-9]{2,}(.[a-z0-9]{2,4}){1,2}$')
			phone: ['', [Validators.required, Validators.pattern('^(84|0[3|5|7|8|9])+([0-9]{8})$')]],
			address: ['', [Validators.required]],
		})
		/*user form*/
		this.createUserForm = this.formBuilder.group({
			userName: ['', [Validators.required]],
			email: ['', [Validators.required, Validators.pattern('^[a-z][a-z0-9_.]{2,32}@[a-z0-9]{2,}(.[a-z0-9]{2,4}){1,2}$')]],
			fullName: ['', [Validators.required]],
			phone: ['', [Validators.required, Validators.pattern('^(84|0[3|5|7|8|9])+([0-9]{8})$')]],
			positionId: ['', [Validators.required]],
			unitId: ['', [Validators.required]],
			gender: ['', [Validators.required]],
			roleId: ['', [Validators.required]],
			isActived: [''],
			address: [''],
		})

		this.positionService
			.positionGetList({
				pageIndex: 1,
				pageSize: 1000,
			})
			.subscribe((res) => {
				if (res.success != 'OK') return
				this.positionsList = res.result.CAPositionGetAllOnPage
			})
		this.roleService.getAll({}).subscribe((res) => {
			if (res.success != 'OK') return
			this.rolesList = res.result.SYRoleGetAll
		})
	}
	ngAfterViewInit() {}

	getUnitPagedList(): void {
		this.unitService.getAllPagedList(this.query).subscribe(
			(res) => {
				if (res.success != 'OK') return
				this.listUnitPaged = res.result.CAUnitGetAllOnPage
				if (this.totalCount_Unit <= 0) this.totalCount_Unit = res.result.TotalCount
				this.unitPageCount = Math.ceil(this.totalCount_Unit / this.query.pageSize)
			},
			(err) => {}
		)
	}
	unitFilterChange(): void {
		this.getUnitPagedList()
	}
	changePage(page: number): void {
		this.query.pageIndex += page
		if (this.query.pageIndex < 1) {
			this.query.pageIndex = 1
			return
		}
		if (this.query.pageIndex > this.unitPageCount) {
			this.query.pageIndex = this.unitPageCount
			return
		}
		this.getUnitPagedList()
	}

	treeViewActive(id, level) {
		this.getUnitInfo(id)
		this.query.parentId = id
		this.getUnitPagedList()
	}

	getUnitInfo(id) {
		this.unitService.getById({ id }).subscribe((res) => {
			if (res.success != 'OK') return
			this.unitObject = res.result.CAUnitGetByID[0]
			this.getUserPagedList()
		})
	}

	getAllUnitShortInfo() {
		this.unitService.getAll({}).subscribe(
			(res) => {
				if (res.success != 'OK') return
				let listUnit = res.result.CAUnitGetAll.map((e) => {
					let item = {
						id: e.id,
						name: e.name,
						parentId: e.parentId == null ? 0 : e.parentId,
						unitLevel: e.unitLevel,
						children: [],
					}
					if (e.unitLevel < 3) {
						item['expandedIcon'] = 'pi bi-dash-circle-fill'
						item['collapsedIcon'] = 'pi bi-plus-circle-fill'
					}

					return item
				})
				this.unitFlatlist = listUnit
				this.treeUnit = this.unflatten(listUnit)
				this.treeViewActive(listUnit[0].id, listUnit[0].unitLevel)
			},
			(err) => {}
		)
	}

	/*start user area*/
	getUserPagedList() {
		this.queryUser.unitId = this.unitObject.id
		this.userService.getAllPagedList(this.queryUser).subscribe((res) => {
			if (res.success != 'OK') return
			this.listUserPaged = res.result.SYUserGetAllOnPage
			if (this.totalCount_User <= 0) this.totalCount_User = res.result.TotalCount
			this.userPageCount = Math.ceil(this.totalCount_User / this.query.pageSize)
		})
	}
	onUserFilterChange() {
		this.getUserPagedList()
	}
	onUserPageChange(page) {
		this.queryUser.pageIndex += page
		if (this.queryUser.pageIndex < 1) {
			this.queryUser.pageIndex = 1
			return
		}
		if (this.queryUser.pageIndex > this.userPageCount) {
			this.queryUser.pageIndex = this.userPageCount
			return
		}
		this.getUserPagedList()
	}

	@ViewChild(UserCreateOrUpdateComponent, { static: false }) childCreateOrUpdateUser: UserCreateOrUpdateComponent
	modalUserCreateOrUpdate(key: any = 0) {
		this.childCreateOrUpdateUser.openModal(this.unitObject.id, key)
	}

	onDelUser(id: number) {
		let userObj = this.listUserPaged.find((c) => c.id == id)

		this.userService.delete(userObj).subscribe((res) => {
			if (res.success != 'OK') {
				this._toastr.error(COMMONS.DELETE_FAILED)
				return
			}
			this._toastr.success(COMMONS.DELETE_SUCCESS)
			this.getUserPagedList()
		})
	}
	/*end user area*/

	/*modal thêm / sửa đơn vị*/
	modalCreateOrUpdateTitle: string = 'Thêm cơ quan, đơn vị'
	modalCreateOrUpdate(id: any, level: any = 1, parentId: any = 0) {
		if (id == 0) {
			this.modalCreateOrUpdateTitle = 'Thêm cơ quan, đơn vị'
			this.modelUnit = new UnitObject()
			this.unitFormSubmitted = false
		} else {
			this.modalCreateOrUpdateTitle = 'Thêm cơ quan, đơn vị'
			this.unitService.getById({ id }).subscribe((res) => {
				if (res.success != 'OK') return
				this.modelUnit = res.result.CAUnitGetByID[0]
			})
		}
		$('#modal-create-or-update').modal('show')
		this.modelUnit.unitLevel = level
		if (parentId > 0) this.modelUnit.parentId = parentId
	}

	get fUnit() {
		return this.createUnitFrom.controls
	}
	unitFormSubmitted = false
	onSaveUnit() {
		this.unitFormSubmitted = true
		console.log(this.modelUnit)
		if (this.createUnitFrom.invalid) {
			return
		}
		if (this.modelUnit.id != null && this.modelUnit.id > 0) {
			this.unitService.update(this.modelUnit).subscribe((res) => {
				if (res.success != 'OK') {
					this._toastr.error(COMMONS.UPDATE_FAILED)
					return
				}
				this._toastr.success(COMMONS.UPDATE_SUCCESS)
				this.getAllUnitShortInfo()
				this.getUnitPagedList()
				this.modelUnit = new UnitObject()
				$('#modal-create-or-update').modal('hide')
			})
		} else {
			this.unitService.create(this.modelUnit).subscribe((res) => {
				if (res.success != 'OK') {
					this._toastr.error(COMMONS.ADD_FAILED)
					return
				}
				this._toastr.success(COMMONS.ADD_SUCCESS)
				this.getAllUnitShortInfo()
				this.getUnitPagedList()
				this.modelUnit = new UnitObject()
				$('#modal-create-or-update').modal('hide')
			})
		}
	}

	changeLevel(level: number) {
		this.modelUnit.unitLevel = level
		//this.modelUnit.parentId = 0
	}
	get getUnitParent(): any[] {
		if (!this.unitFlatlist) return []
		//if (!this.unitObject.parentId) return this.listUnitTreeview.filter((c) => c.unitLevel == this.modelUnit.unitLevel - 1 && c.parentId == this.unitObject.parentId)
		return this.unitFlatlist.filter((c) => c.unitLevel == this.modelUnit.unitLevel - 1)
	}

	/*start - chức năng xác nhận hành động xóa*/
	delType = 'unit'
	delId: number = 0
	onOpenConfirmModal(id: any, type = 'unit') {
		$('#modal-confirm').modal('show')
		this.delType = type
		this.delId = id
	}
	acceptConfirm() {
		if (this.delType == 'unit') {
			this.onDeleteUnit(this.delId)
		} else if (this.delType == 'user') {
			this.onDelUser(this.delId)
		}

		$('#modal-confirm').modal('hide')
	}
	onDeleteUnit(id) {
		let item = this.listUnitPaged.find((c) => c.id == this.delId)
		this.unitService.delete(item).subscribe((res) => {
			if (res.success != 'OK') {
				this._toastr.error(COMMONS.DELETE_FAILED)
				return
			}
			this._toastr.success(COMMONS.DELETE_SUCCESS)
			this.getAllUnitShortInfo()
			this.getUnitPagedList()
		})
	}
	/*end - chức năng xác nhận hành động xóa*/

	modalCreateOrUpdateUser(id: number) {
		let unitId = this.unitObject.id
	}

	private unflatten(arr): any[] {
		var tree = [],
			mappedArr = {},
			arrElem,
			mappedElem

		// First map the nodes of the array to an object -> create a hash table.
		for (var i = 0, len = arr.length; i < len; i++) {
			arrElem = arr[i]
			mappedArr[arrElem.id] = arrElem
			mappedArr[arrElem.id]['children'] = []
		}

		for (var id in mappedArr) {
			if (mappedArr.hasOwnProperty(id)) {
				mappedElem = mappedArr[id]
				// If the element is not at the root level, add it to its parent array of children.
				if (mappedElem.parentId) {
					if (!mappedArr[mappedElem['parentId']]) continue
					mappedArr[mappedElem['parentId']]['children'].push(mappedElem)
				}
				// If the element is at the root level, add it to first level elements array.
				else {
					tree.push(mappedElem)
				}
			}
		}
		return tree
	}
}
