import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core'
import { FormGroup, FormBuilder, Validators } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'
import { TreeNode } from 'primeng/api'
import { Router } from '@angular/router'

import { DataService } from 'src/app/services/sharedata.service'
import { REGEX } from 'src/app/constants/CONSTANTS'
import { UnitService } from '../../../../services/unit.service'
import { UserService } from '../../../../services/user.service'
import { PositionService } from '../../../../services/position.service'
import { RoleService } from '../../../../services/role.service'

import { UserCreateOrUpdateComponent } from '../user/user-create-or-update/user-create-or-update.component'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
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

	regex_phone = REGEX.PHONE_VN

	treeUnit: any[]
	listUnitPaged: any[] = []
	ltsUnitIdNonActive: any[] = []
	unitObject: any = {}
	listUserPaged: any[] = []
	unitFlatlist: any[] = []
	lstField : any [] = []

	createUnitFrom: FormGroup
	titleConfirm : any = ''
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
		isActived: null,
	}
	titleSearch : any = ''

	//sort
	unitSortDir = 'DESC'
	unitSortField = 'ID'

	usSortDir = 'DESC'
	usSortField = 'ID'

	totalCount_Unit: number = 0
	unitPageCount: number = 0


	totalCount_User: number = 0
	userPageCount: number = 0

	constructor(
		private unitService: UnitService,
		private userService: UserService,
		private positionService: PositionService,
		private formBuilder: FormBuilder,
		private _toastr: ToastrService,
		private _shareData: DataService,
		private _router: Router,
		private roleService: RoleService
	) {}

	ngOnInit() {
		this.getAllUnitShortInfo()
		/*unit form*/
		this.createUnitFrom = this.formBuilder.group({
			name: [this.modelUnit.name, Validators.required],
			unitLevel: [this.modelUnit.unitLevel, [Validators.required]],
			isActived: [this.modelUnit.isActived, [Validators.required]],
			parentId: [this.modelUnit.parentId, Validators.required],
			description: [this.modelUnit.description],
			email: [this.modelUnit.email, [Validators.required, Validators.email]], //Validators.pattern('^[a-z][a-z0-9_.]{5,32}@[a-z0-9]{2,}(.[a-z0-9]{2,4}){1,2}$')
			phone: [this.modelUnit.phone, [Validators.required, Validators.pattern('^(84|0[3|5|7|8|9])+([0-9]{8})$')]],
			address: [this.modelUnit.address],
			index: [this.modelUnit.index],
			field: [this.modelUnit.field],
		})

		this.unitService.getDataForCreate().subscribe(res=>{
			if(res.success == RESPONSE_STATUS.success){
				this.lstField = res.result.lstField
			}
			
		})
	}
	ngAfterViewInit() {
		this.childCreateOrUpdateUser.parentUnit = this

		$('#tree-unit input.ui-tree-filter').attr('placeholder', 'Nhập...')
		$('#modal-create-or-update').on('hide.bs.modal', () => {
			this.modelUnit = new UnitObject()
			this.createUnitFrom.reset()
		})
		setTimeout(() => {
			this.onCollapsed('item02')
		}, 1000)
	}

	collapsed_checked: any = {
		item01: true,
		item02: true,
	}
	onCollapsed(item: string) {
		this.collapsed_checked[item] = !this.collapsed_checked[item]
	}

	onSortUnit(fieldName: string) {
		this.unitSortField = fieldName
		this.unitSortDir = this.unitSortDir == 'DESC' ? 'ASC' : 'DESC'
		//this.getUnitPagedList()
	}
	unitFilterChange(): void {
		//this.getUnitPagedList()
	}
	onPageChange(event): void {
		this.query.pageSize = event.rows
		this.query.pageIndex = event.first / event.rows + 1
		//this.getUnitPagedList()
	}

	treeViewActive(id, level) {
		this.query.parentId = id
		this.query.pageIndex = 1
		this.totalCount_Unit = 0
		this.totalCount_User = 0

		this.getUnitInfo(id)
	}

	onFilterTree() {
		this.titleSearch = this.titleSearch.trim()
		this.getAllUnitShortInfo(null, this.titleSearch)
	}

	getUnitInfo(id) {
		this.unitService.getById({ id }).subscribe((res) => {
			if (res.success != 'OK') return
			this.unitObject = res.result.CAUnitGetByID[0]
			this.getUserPagedList()
		})
	}

	getAllUnitShortInfo(activeTreeNode: any = null, title: any = '') {
		this.ltsUnitIdNonActive = []
		this.unitService.getAll({}).subscribe(
			(res) => {
				if (res.success != 'OK') return
				let listUnit = res.result.CAUnitGetAll.filter((e) => {
					if (title == '' && e.isActived == false) {
						this.ltsUnitIdNonActive.push(e.id)
					}
					if (e.name.toLowerCase().includes(title.toLowerCase())) {
						let item = {
							id: e.id,
							name: e.name,
							parentId: e.parentId == null ? 0 : e.parentId,
							unitLevel: e.unitLevel,
							children: [],
							label: e.name,
							index: e.index,
						}
						return item
					}
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



	getUserPagedList() {
		this.userService
			.getAllPagedList({
				unitid: this.unitObject.id
			})
			.subscribe((res) => {
				if (res.success != 'OK') {
					this.listUserPaged = []
					this.userPageCount = 0
					this.totalCount_User = 0
				} else {
					if (res.result.SYUserGetAllOnPage.length == 0) {
						this.listUserPaged = []
						this.userPageCount = 0
						this.totalCount_User = 0
					} else {
						this.listUserPaged = res.result.SYUserGetAllOnPage
						if (res.result.TotalCount > 0) this.totalCount_User = res.result.TotalCount
						this.userPageCount = Math.ceil(this.totalCount_User / this.query.pageSize)
					}
				}
			})
	}
	

	@ViewChild(UserCreateOrUpdateComponent, { static: false }) childCreateOrUpdateUser: UserCreateOrUpdateComponent
	modalUserCreateOrUpdate(key: any = 0) {
		this.childCreateOrUpdateUser.openModal(this.unitObject.id, key)
	}

	onDelUser(id: number) {
		this.userService.delete({ Id: id }).subscribe((res) => {
			if (res.success != 'OK') {
				if (isNaN(res.result)) {
					this._toastr.error(res.message)
					return
				} else {
					this._toastr.error('Dữ liệu đang được sử dụng, không được phép xoá!')
					return
				}
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
			isActived: [this.modelUnit.isActived, [Validators.required]],
			parentId: [this.modelUnit.parentId, Validators.required],
			description: [this.modelUnit.description],
			email: [this.modelUnit.email, [Validators.required, Validators.email]], //Validators.pattern('^[a-z][a-z0-9_.]{5,32}@[a-z0-9]{2,}(.[a-z0-9]{2,4}){1,2}$')
			phone: [this.modelUnit.phone, [Validators.required, Validators.pattern('^(84|0[3|5|7|8|9])+([0-9]{8})$')]],
			address: [this.modelUnit.address],
			index: [this.modelUnit.index],
			field : [this.modelUnit.field]
		})
		this.checkExists = {
			Phone: false,
			Email: false,
		}

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
	checkExists = {
		Phone: false,
		Email: false,
	}
	onCheckExist(field: string, value: string) {
		this.unitService
			.checkExists({
				field,
				value,
				id: this.modelUnit.id ? this.modelUnit.id : 0,
			})
			.subscribe((res) => {
				if (res.success == RESPONSE_STATUS.success) {
					this.checkExists[field] = res.result.SYUnitCheckExists[0].exists
				}
			})
	}

	unitFormSubmitted = false
	onSaveUnit() {
		this.unitFormSubmitted = true
		this.modelUnit.name = this.modelUnit.name.trim()
		this.modelUnit.email = this.modelUnit.email.trim()
		this.modelUnit.address == null ? (this.modelUnit.address = '') : (this.modelUnit.address = this.modelUnit.address.trim())
		this.modelUnit.description == null ? (this.modelUnit.description = '') : (this.modelUnit.description = this.modelUnit.description.trim())

		if (this.createUnitFrom.invalid) {
			return
		}
		if(this.modelUnit.index < 0){
			return
		}
		if (this.checkExists['Phone'] || this.checkExists['Email']) return

		if (this.modelUnit.id != null && this.modelUnit.id > 0) {
			this.unitService.update(this.modelUnit).subscribe((res) => {
				if (res.success != 'OK') {
					this._toastr.error(res.message)
					return
				} else {
					if (res.result > 0) {
						this.unitFormSubmitted = false
						$('#modal-create-or-update').modal('hide')
						this._toastr.success(COMMONS.UPDATE_SUCCESS)
						this.getAllUnitShortInfo(this.unitObject)
						this.treeViewActive(this.unitObject.id, this.unitObject.unitLevel)
						// cập nhật tên ptree khi đã sửa thành công
						let current_edit = this.searchTree(this.treeUnit, this.modelUnit.id)
						current_edit.name = this.modelUnit.name
					} else if(res.result == -1) {
						
						$("[id='unitId']").focus()
						this._toastr.error('Tên đơn vị đã tồn tại.')
					}
					else{
						this._toastr.error('Lĩnh vực này đã được sử dụng cho đơn vị khác')
					}
				}
			})
		} else {
			this.unitService.create(this.modelUnit).subscribe((res) => {
				if (res.success != 'OK') {
					let errorMsg = res.message
					this._toastr.error(errorMsg)
					return
				} else {
					if (res.result > 0) {
						this.unitFormSubmitted = false
						$('#modal-create-or-update').modal('hide')
						this._toastr.success(COMMONS.ADD_SUCCESS)
						this.modelUnit.id = res.result
						this.unitObject = {...this.modelUnit}
						this.treeViewActive(this.unitObject.id, this.unitObject.unitLevel)
						this.getAllUnitShortInfo(this.unitObject)
					} else if(res.result == -1){
						$("[id='unitId']").focus()
						this._toastr.error('Tên đơn vị đã tồn tại')
					}
					else{
						this._toastr.error('Lĩnh vực này đã được sử dụng cho đơn vị khác')
					}
				}
			})
		}
	}

	onChangeUnitStatus(id: number) {
		let item = this.listUnitPaged.find((c) => c.id == id)
		if (item == null) item = this.unitObject
		item.isActived = !item.isActived
		this.unitService.changeStatus(item).subscribe((res) => {
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
	onChangeUserStatus(id: number) {
		let item = this.listUserPaged.find((c) => c.id == id)

		item.isActived = !item.isActived
		item.typeId = 1
		item.countLock = 0
		item.lockEndOut = ''
		this.userService.changeStatus(item).subscribe((res) => {
			if (res.success != 'OK') {
				this._toastr.error(COMMONS.UPDATE_FAILED)
				item.isActived = !item.isActived
				return
			}
			this._toastr.success(COMMONS.UPDATE_SUCCESS)
			this.getAllUnitShortInfo(this.unitObject)
			////this.getUnitPagedList()
			this.modelUnit = new UnitObject()
			$('#modal-create-or-update').modal('hide')
		})
	}

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

	changeLevel(e, level: number) {
		if (this.modelUnit.id > 0) {
			this._toastr.error('Chỉnh sửa đơn vị không được phép thay đổi cấp đơn vị')
			e.preventDefault()
			return
		} else {
			this.modelUnit.unitLevel = level
			if (this.modelUnit.unitLevel > 1) this.modelUnit.parentId = null
			else {
				this.modelUnit.parentId = 0
			}
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
		this.modalConfirm_type = type
		this.modelConfirm_itemId = id
		if (this.modalConfirm_type == 'unit') {
			this.titleConfirm = 'Bạn có chắc chắn muốn xóa đơn vị này?'
		} else if (this.modalConfirm_type == 'user') {
			this.titleConfirm = 'Bạn có chắc chắn muốn xóa người dùng này?'
		} else if (this.modalConfirm_type == 'unit_status') {
			this.titleConfirm = 'Bạn có chắc chắn muốn thay đổi trạng thái của đơn vị này?'
		} else if (this.modalConfirm_type == 'user_status') {
			this.titleConfirm = 'Bạn có chắc chắn muốn thay đổi trạng thái của người dùng này?'
		}
		$('#modal-confirm').modal('show')
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
				//this.getUnitPagedList()
			} else {
				this.getAllUnitShortInfo(this.unitObject)
				//this.getUnitPagedList()
			}
		})
	}
	/*end - chức năng xác nhận hành động xóa*/

	// xuất excel

	onExport() {
		let passingObj: any = {}
		passingObj.UnitId = this.query.parentId
		passingObj.UnitName = this.unitObject.name

		this._shareData.setobjectsearch(passingObj)
		this._shareData.sendReportUrl = 'ListUserByUnitId?' + JSON.stringify(passingObj)
		this._router.navigate(['quan-tri/xuat-file'])
	}

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
					if (!mappedArr[mappedElem['parentId']]){
						if(this.unitObject && mappedElem['id'] == this.unitObject.parentId){
							mappedElem['expanded']=true;
							let s = tree.find(x=>x.id == mappedElem['parentId'])
							if(s){
								s['expanded']=true;
							}
						}
						tree.push(mappedElem)
					}else{
						if(this.unitObject && mappedElem['id'] == this.unitObject.parentId){
							mappedElem['expanded']=true;
							let s = tree.find(x=>x.id == mappedElem['parentId'])
							if(s){
								s['expanded']=true;
							}
						}
						mappedArr[mappedElem['parentId']]['children'].push(mappedElem)
					}
				}
				// If the element is at the root level, add it to first level elements array.
				else {
					if(this.unitObject && mappedElem['id'] == this.unitObject.parentId){
						mappedElem['expanded']=true;
						let s = tree.find(x=>x.id == mappedElem['parentId'])
							if(s){
								s['expanded']=true;
							}
					}
					tree.push(mappedElem)
				}
			}
		}
		tree = tree.sort((x, y) => {
			return x.index - y.index
		})
		console.log(tree)
		return tree
	}
}
