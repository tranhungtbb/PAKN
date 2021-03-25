import { NullTemplateVisitor } from '@angular/compiler'
import { Component, OnInit } from '@angular/core'

import { UnitService } from '../../../../services/unit.service'
import { UserService } from '../../../../services/user.service'

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

	listUnitTreeview: any[]

	listUser: any[]

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
	totalCount: number = 0
	activeParent: number
	activeLevel: number = 1

	constructor(private unitService: UnitService) {}

	ngOnInit() {
		this.initialTreeViewJs()
		this.getAllUnitShortInfo()
	}

	getUnitPagedList(): void {
		this.unitService.getAllPagedList(this.query).subscribe(
			(res) => {
				if (res.success != 'OK') return
				this.listUnitPaged = res.result.CAUnitGetAllOnPage
				this.totalCount = res.totalCount
			},
			(err) => {}
		)
	}

	timeout: any
	loadUnitChildren(parentId: number, level: number): any[] {
		// if (!$('#tree1').hasClass('tree')) {
		// 	$('#tree1').treed()
		// }
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

	modalCreateOrUpdateTitle: string = 'Thêm cơ quan, đơn vị'
	modalCreateOrUpdate(id: any, level: any) {
		if ((id = 0)) this.modalCreateOrUpdateTitle = 'Sửa cơ quan, đơn vị'
		$('#modal-create-or-update').modal('show')
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
