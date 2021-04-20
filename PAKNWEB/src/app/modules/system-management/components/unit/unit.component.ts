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

declare var jquery: any
declare var $: any
@Component({
	selector: 'app-unit',
	templateUrl: './unit.component.html',
	styleUrls: ['./unit.component.css'],
})
export class UnitComponent implements OnInit, AfterViewInit {
	listStatus: any = [
		// { value: '', text: 'Toàn bộ' },
		{ value: true, text: 'Hiệu lực' },
		{ value: false, text: 'Hết hiệu lực' },
	]
	listGender: any = [
		{ value: true, text: 'Nam' },
		{ value: false, text: 'Nữ' },
	]

	treeUnit: any[]
	listUnitPaged: any[] = []
	unitObject: any = {}
	listUserPaged: any[] = []
	unitFlatlist: any[] = []
	rolesList: any[] = []
	positionsList: any[] = []

	createUnitFrom: FormGroup

	modelUnit: UnitObject = new UnitObject()

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
		isActived: '',
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
			name: [this.modelUnit.name, Validators.required],
			unitLevel: [this.modelUnit.unitLevel, [Validators.required]],
			isActived: [this.modelUnit.isActived],
			parentId: [this.modelUnit.parentId, Validators.required],
			description: [this.modelUnit.description],
			email: [this.modelUnit.email, [Validators.required, Validators.email]], //Validators.pattern('^[a-z][a-z0-9_.]{5,32}@[a-z0-9]{2,}(.[a-z0-9]{2,4}){1,2}$')
			phone: [this.modelUnit.phone, [Validators.required, Validators.pattern('^(84|0[3|5|7|8|9])+([0-9]{8})$')]],
			address: [this.modelUnit.address],
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

		//bind parent
		this.childCreateOrUpdateUser.parentUnit = this
	}
	ngAfterViewInit() {}

	collapsed_checked: any = {
		item01: false,
		item02: false,
	}
	onCollapsed(item: string) {
		this.collapsed_checked[item] = !this.collapsed_checked[item]
	}

	getUnitPagedList(): void {
		this.unitService
			.getAllPagedList({
				parentId: this.query.parentId,
				pageSize: this.query.pageSize,
				pageIndex: this.query.pageIndex,
				name: this.query.name,
				email: this.query.email,
				phone: this.query.phone,
				address: this.query.address,
				isActived: this.query.isActived == null ? '' : this.query.isActived,
			})
			.subscribe(
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
	onPageChange(event): void {
		this.query.pageSize = event.rows
		this.query.pageIndex = event.first / event.rows + 1
		this.getUnitPagedList()
	}

	treeViewActive(id, level) {
		this.query.parentId = id
		this.query.pageIndex = 1
		this.totalCount_Unit = 0

		this.queryUser.pageIndex = 1
		this.totalCount_User = 0

		this.getUnitInfo(id)
		this.getUnitPagedList()
	}

	getUnitInfo(id) {
		this.unitService.getById({ id }).subscribe((res) => {
			if (res.success != 'OK') return
			this.unitObject = res.result.CAUnitGetByID[0]
			this.getUserPagedList()
		})
	}

	getAllUnitShortInfo(activeTreeNode: any = null) {
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

					return item
				})
				this.unitFlatlist = listUnit
				this.treeUnit = this.unflatten(listUnit)

				//active first
				if (activeTreeNode == null) this.treeViewActive(this.treeUnit[0].id, this.treeUnit[0].unitLevel)
				else {
					this.treeViewActive(activeTreeNode.id, activeTreeNode.unitLevel)
					//this.expandNode(activeTreeNode)
				}
			},
			(err) => {}
		)
	}

	/*start user area*/
	getUserPagedList() {
		this.userService
			.getAllPagedList({
				unitid: this.unitObject.id,
				pageSize: this.queryUser.pageSize,
				pageIndex: this.queryUser.pageIndex,
				userName: this.queryUser.userName,
				email: this.queryUser.email,
				fullName: this.queryUser.fullName,
				phone: this.queryUser.phone,
				isActived: this.queryUser.isActived == null ? '' : this.queryUser.isActived,
			})
			.subscribe((res) => {
				if (res.success != 'OK') return
				this.listUserPaged = res.result.SYUserGetAllOnPage
				if (this.totalCount_User <= 0) this.totalCount_User = res.result.TotalCount
				this.userPageCount = Math.ceil(this.totalCount_User / this.query.pageSize)
			})
	}
	onUserFilterChange() {
		this.getUserPagedList()
	}
	onUserPageChange(event: any) {
		this.queryUser.pageSize = event.rows
		this.queryUser.pageIndex = event.first / event.rows + 1
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
	modalCreateOrUpdateTitle: string = 'Thêm mới cơ quan, đơn vị'
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

		this.unitFormSubmitted = false
		if (id == 0) {
			this.modalCreateOrUpdateTitle = 'Thêm mới cơ quan, đơn vị'
			this.modelUnit = new UnitObject()
			this.modelUnit.name = ''
			if (this.modelUnit.unitLevel > 1) this.modelUnit.parentId = null
			else {
				this.modelUnit.parentId = 0
			}
		} else {
			this.modalCreateOrUpdateTitle = 'Chỉnh sửa cơ quan, đơn vị'
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

		if (this.createUnitFrom.invalid) {
			this._toastr.error('Dữ liệu không hợp lệ')
			return
		}

		if (this.modelUnit.id != null && this.modelUnit.id > 0) {
			this.unitService.update(this.modelUnit).subscribe((res) => {
				if (res.success != 'OK') {
					let errorMsg = COMMONS.UPDATE_FAILED
					if (res.message.includes(`UNIQUE KEY constraint 'UC_SY_Unit_Email'`)) {
						errorMsg = 'Email đã được sử dụng'
					}
					return
				}
				this._toastr.success(COMMONS.UPDATE_SUCCESS)
				// this.getAllUnitShortInfo()
				//this.getUnitPagedList()
				this.treeViewActive(this.unitObject.id, this.unitObject.unitLevel)

				// cập nhật tên ptree khi đã sửa thành công
				let current_edit = this.searchTree(this.treeUnit, this.modelUnit.id)
				current_edit.name = this.modelUnit.name

				this.modelUnit = new UnitObject()
				$('#modal-create-or-update').modal('hide')
			})
		} else {
			this.unitService.create(this.modelUnit).subscribe((res) => {
				if (res.success != 'OK') {
					let errorMsg = COMMONS.ADD_FAILED
					if (res.message.includes(`UNIQUE KEY constraint 'UC_SY_Unit_Email'`)) {
						errorMsg = 'Email đã được sử dụng'
					}
					return
				}
				this._toastr.success(COMMONS.ADD_SUCCESS)
				this.getAllUnitShortInfo(this.unitObject)
				this.modelUnit = new UnitObject()
				$('#modal-create-or-update').modal('hide')
			})
		}
	}

	onChangeUnitStatus(id: number) {
		let item = this.listUnitPaged.find((c) => c.id == id)
		if (item == null) item = this.unitObject

		item.isActived = !item.isActived
		this.unitService.update(item).subscribe((res) => {
			if (res.success != 'OK') {
				this._toastr.error(COMMONS.UPDATE_FAILED)
				item.isActived = !item.isActived
				return
			}
			this._toastr.success(COMMONS.UPDATE_SUCCESS)
			//this.getAllUnitShortInfo(this.unitObject)
			this.getUnitPagedList()
			this.modelUnit = new UnitObject()
			$('#modal-create-or-update').modal('hide')
		})
	}
	onChangeUserStatus(id: number) {
		let item = this.listUserPaged.find((c) => c.id == id)

		item.isActived = !item.isActived
		item.typeId = 1
		item.countLock = 0
		item.lockEndOut = ''
		this.userService.update(item).subscribe((res) => {
			if (res.success != 'OK') {
				this._toastr.error(COMMONS.UPDATE_FAILED)
				item.isActived = !item.isActived
				return
			}
			this._toastr.success(COMMONS.UPDATE_SUCCESS)
			this.getAllUnitShortInfo(this.unitObject)
			//this.getUnitPagedList()
			this.modelUnit = new UnitObject()
			$('#modal-create-or-update').modal('hide')
		})
	}

	//////expand node
	// expandNode(node: any) {
	// 	let _node = this.searchTree(this.treeUnit, node.id)
	// 	if (_node) this.expandRecursive(_node, true)
	// }
	// private expandRecursive(node: any, isExpand: boolean) {
	// 	node.expanded = isExpand
	// 	if (node.parentId) {
	// 		let pNode = this.searchTree(this.treeUnit, node.parentId)
	// 		this.expandRecursive(pNode, isExpand)
	// 	}
	// }
	private searchTree(data, value, key = 'id', sub = 'children', tempObj: any = {}) {
		if (value && data) {
			data.find((node) => {
				if (node[key] == value) {
					tempObj.found = node
					return node
				}
				return this.searchTree(node[sub], value, key, sub, tempObj)
			})
			if (tempObj.found) {
				return tempObj.found
			}
		}
		return false
	}

	//////end expand node

	changeLevel(level: number) {
		this.modelUnit.unitLevel = level
		if (this.modelUnit.unitLevel > 1) this.modelUnit.parentId = null
		else {
			this.modelUnit.parentId = 0
		}
	}
	get getUnitParent(): any[] {
		if (!this.unitFlatlist) return []
		//if (!this.unitObject.parentId) return this.listUnitTreeview.filter((c) => c.unitLevel == this.modelUnit.unitLevel - 1 && c.parentId == this.unitObject.parentId)
		return this.unitFlatlist.filter((c) => c.unitLevel == this.modelUnit.unitLevel - 1)
	}

	/*start - chức năng xác nhận hành động xóa*/
	modalConfirm_type = 'unit'
	modelConfirm_itemId: number = 0
	onOpenConfirmModal(id: any, type = 'unit') {
		$('#modal-confirm').modal('show')
		this.modalConfirm_type = type
		this.modelConfirm_itemId = id
	}
	acceptConfirm() {
		if (this.modalConfirm_type == 'unit') {
			this.onDeleteUnit(this.modelConfirm_itemId)
		} else if (this.modalConfirm_type == 'user') {
			this.onDelUser(this.modelConfirm_itemId)
		} else if (this.modalConfirm_type == 'unit_status') {
			this.onChangeUnitStatus(this.modelConfirm_itemId)
		} else if (this.modalConfirm_type == 'user_status') {
			this.onChangeUserStatus(this.modelConfirm_itemId)
		}

		$('#modal-confirm').modal('hide')
	}
	onDeleteUnit(id) {
		let item = this.listUnitPaged.find((c) => c.id == this.modelConfirm_itemId)
		if (!item) item = this.unitObject
		this.unitService.delete(item).subscribe((res) => {
			if (res.success != 'OK') {
				if (res.message.includes(`REFERENCE constraint "FK_SY_User_UnitId"`)) {
					this._toastr.error(COMMONS.DELETE_FAILED + ', đơn vị đang được sử dụng trong quy trình')
					return
				}
				this._toastr.error(res.message)
				return
			}
			this._toastr.success(COMMONS.DELETE_SUCCESS)

			if (this.unitObject.id == id) {
				this.getAllUnitShortInfo()
				this.getUnitPagedList()
			} else {
				this.getAllUnitShortInfo(this.unitObject)
			}
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
