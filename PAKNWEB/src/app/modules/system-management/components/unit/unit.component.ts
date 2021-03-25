import { NullTemplateVisitor } from '@angular/compiler'
import { Component, OnInit } from '@angular/core'
import { FormGroup, FormBuilder, Validators, FormArray, FormControl } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'
import { MatDialog, throwMatDialogContentAlreadyAttachedError } from '@angular/material'

import { UnitService } from '../../../../services/unit.service'
import { UserService } from '../../../../services/user.service'

import { ConfirmDialogComponent } from '../../../../directives/confirm-dialog/confirm-dialog.component'

import { COMMONS } from 'src/app/commons/commons'
import { UnitObject } from 'src/app/models/unitObject'

declare var jquery: any
declare var $: any
@Component({
	selector: 'app-unit',
	templateUrl: './unit.component.html',
	styleUrls: ['./unit.component.css'],
})
export class UnitComponent implements OnInit {
	listUnitPaged: any[] = []
	unitObject: any = {}
	listUser: any[]
	listUnitTreeview: any[]

	createUnitFrom: FormGroup
	createUserForm: FormGroup

	modelUnit: UnitObject = new UnitObject()

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
	activeParent: number
	activeLevel: number = 1

	constructor(private unitService: UnitService, private formBuilder: FormBuilder, private _toastr: ToastrService, private dialog: MatDialog) {}

	ngOnInit() {
		this.initialTreeViewJs()
		this.getAllUnitShortInfo()
		console.log(this.unitObject)
		/*unit form*/
		this.createUnitFrom = this.formBuilder.group({
			name: ['', Validators.required],
			unitLevel: ['', [Validators.required]],
			isActived: [''],
			isDeleted: [''],
			parentId: [''],
			description: [''],
			email: ['', [Validators.required, Validators.pattern('^[a-z][a-z0-9_.]{5,32}@[a-z0-9]{2,}(.[a-z0-9]{2,4}){1,2}$')]], //Validators.pattern('^[a-z][a-z0-9_.]{5,32}@[a-z0-9]{2,}(.[a-z0-9]{2,4}){1,2}$')
			phone: ['', [Validators.required, Validators.pattern('^(0?)(3[2-9]|5[6|8|9]|7[0|6-9]|8[0-6|8|9]|9[0-4|6-9])[0-9]{7}$')]],
			address: ['', [Validators.required]],
		})
	}

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

	timeout: any
	loadUnitChildren(parentId: number, level: number): any[] {
		if (!$('#tree1').hasClass('tree')) {
			$('#tree1').treed()
		}

		if (!this.listUnitTreeview) return []
		return this.listUnitTreeview.filter((c) => c.parentId == parentId && c.unitLevel == level)
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
		})
	}

	getAllUnitShortInfo() {
		this.unitService
			.getAllPagedList({
				pageIndex: 1,
				pageSize: 1000,
			})
			.subscribe(
				(res) => {
					if (res.success != 'OK') return
					this.listUnitTreeview = res.result.CAUnitGetAllOnPage.map((e) => {
						return { id: e.id, name: e.name, parentId: e.parentId == null ? 0 : e.parentId, unitLevel: e.unitLevel }
					})
				},
				(err) => {}
			)
	}

	/*modal thêm / sửa đơn vị*/
	modalCreateOrUpdateTitle: string = 'Thêm cơ quan, đơn vị'
	modalCreateOrUpdate(id: any, level: any = 1, parentId: any = 0) {
		if (id == 0) {
			this.modalCreateOrUpdateTitle = 'Thêm cơ quan, đơn vị'
			this.modelUnit = new UnitObject()
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
		this.modelUnit.parentId = $('#f_parentId').val()

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
		if (!this.listUnitTreeview) return []
		//if (!this.unitObject.parentId) return this.listUnitTreeview.filter((c) => c.unitLevel == this.modelUnit.unitLevel - 1 && c.parentId == this.unitObject.parentId)
		return this.listUnitTreeview.filter((c) => c.unitLevel == this.modelUnit.unitLevel - 1)
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

	private loadTreeArray(arr): void {
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
				if (mappedElem.parentid) {
					mappedArr[mappedElem['parentid']]['children'].push(mappedElem)
				}
				// If the element is at the root level, add it to first level elements array.
				else {
					tree.push(mappedElem)
				}
			}
		}
		this.listUnitTreeview = tree
	}

	private initialTreeViewJs(): void {
		$('[data-dismiss="modal"]').click((e) => {
			$(e.currentTarget).parents('.modal').modal('hide')
		})

		$.fn.extend({
			treed: function (o) {
				var openedClass = 'bi-dash-circle-fill'
				var closedClass = 'bi-plus-circle-fill'

				if (typeof o != 'undefined') {
					if (typeof o.openedClass != 'undefined') {
						openedClass = o.openedClass
					}
					if (typeof o.closedClass != 'undefined') {
						closedClass = o.closedClass
					}
				}

				//initialize each of the top levels
				var tree = $(this)
				tree.addClass('tree')
				tree
					.find('li')
					.has('ul')
					.each(function () {
						var branch = $(this) //li with children ul
						branch.prepend("<i class='bi " + closedClass + "'></i>")

						branch.addClass('branch')
						branch.on('click', function (e) {
							if (this == e.target) {
								var icon = $(this).children('i:first')
								icon.toggleClass(openedClass + ' ' + closedClass)
								$(this).children().children().toggle()
							}
						})
						branch.children().children().toggle()
					})
				//fire event from the dynamically added icon
				tree.find('.branch .indicator').each(function () {
					$(this).on('click', function () {
						$(this).closest('li').click()
					})
				})
				//fire event to open branch if the li contains an anchor instead of text
				tree.find('.branch>a').each(function () {
					$(this).on('click', function (e) {
						$(this).closest('li').click()
						e.preventDefault()
					})
				})
				//fire event to open branch if the li contains a button instead of text
				tree.find('.branch>button').each(function () {
					$(this).on('click', function (e) {
						$(this).closest('li').click()
						e.preventDefault()
					})
				})
			},
		})

		//Initialization of treeviews
	}
}
