import { RoleService } from 'src/app/services/role.service'
import { COMMONS } from 'src/app/commons/commons'
import { Router } from '@angular/router'
import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { RoleObject } from '../../../../models/roleObject'
import { from } from 'rxjs'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { UserService } from 'src/app/services/user.service'
import { retryWhen } from 'rxjs/operators'

declare var $: any
@Component({
	selector: 'app-role',
	templateUrl: './role.component.html',
	styleUrls: ['./role.component.css'],
})
//acbd
export class RoleComponent implements OnInit {
	constructor(private roleService: RoleService, private toast: ToastrService, private routes: Router, private userService: UserService) {
		this.listItem = []
	}
	@ViewChild('table', { static: false }) table: any
	@ViewChild('table2', { static: false }) table2: any

	listStatus: any = [
		{ value: true, text: 'Hiệu lực' },
		{ value: false, text: 'Hết hiệu lực' },
	]
	model: any = new RoleObject()
	type: string
	mess: string
	pageIndex: Number = 1
	pageSize: Number = 20
	name: string = ''
	description: string = ''
	isActived: boolean
	userCount: Number
	totalRecords: Number
	listData: any[]

	// LIST USER
	listUser: any[]
	listUserIsSystem: any[]
	userPageIndex: Number = 1
	userPageSize: Number = 10
	userTotalRecords: Number
	roleId: Number

	// multi input

	listItem: any[]

	ngOnInit() {
		this.getListPaged()
		this.getUsersIsSystem()
	}

	getListPaged() {
		this.name = this.name.trim()
		this.description = this.description.trim()
		this.roleService
			.getAllPagedList({
				PageIndex: this.pageIndex,
				PageSize: this.pageSize,
				Name: this.name == null ? '' : this.name,
				UserCount: this.userCount == null ? '' : this.userCount,
				Description: this.description == null ? '' : this.description,
				IsActived: this.isActived == null ? '' : this.isActived,
			})
			.subscribe((res) => {
				if (res.success != 'OK') {
					this.totalRecords = 0
					return
				}
				this.listData = res.result.SYRoleGetAllOnPage
				this.totalRecords = res.result.TotalCount
			})
	}

	getUsersIsSystem() {
		this.userService.getIsSystem({}).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				this.listUserIsSystem = res.result.SYUserGetIsSystem
			} else {
				this.listUserIsSystem = []
			}
		})
	}

	onPageChange(event: any) {
		this.pageSize = event.rows
		this.pageIndex = event.first / event.rows + 1
		this.getListPaged()
	}

	dataStateChange() {
		this.pageIndex = 1
		this.table.first = 0
		this.getListPaged()
	}

	confirm(item: RoleObject, type: string) {
		this.model = { ...item, isActived: !item.isActived }
		this.type = type
		if (type == 'delete') {
			this.mess = 'Bạn có chắc chắn thực hiện hành động này?'
		} else {
			this.mess = 'Bạn muốn thay dổi trạng thái của vai trò này?'
		}
		$('#modalConfirm').modal('show')
	}

	onAction() {
		$('#modalConfirm').modal('hide')
		if (this.type == 'delete') {
			this.roleService.delete({ id: this.model.id }).subscribe((res) => {
				if (res.success == RESPONSE_STATUS.success) {
					if (res.result > 0) {
						this.toast.success(COMMONS.DELETE_SUCCESS)
						this.getListPaged()
					} else {
						this.toast.error('Vai trò này đang được sử dụng')
						this.getListPaged()
					}
				} else {
					this.toast.error(COMMONS.DELETE_FAILED)
					this.getListPaged()
				}
			})
		} else {
			this.roleService.update(this.model).subscribe((res) => {
				if (res.success == RESPONSE_STATUS.success) {
					this.toast.success(COMMONS.UPDATE_SUCCESS)
					this.getListPaged()
				} else {
					this.toast.error(COMMONS.UPDATE_FAILED)
					this.getListPaged()
				}
			})
		}
	}

	redirectCreate() {
		this.routes.navigate(['quan-tri/he-thong/vai-tro/them-moi'])
	}

	redirectUpdate(id: number) {
		this.routes.navigate(['quan-tri/he-thong/vai-tro/cap-nhap/' + id])
	}

	showListUser(id: any) {
		if (id == undefined) return
		if (id != this.roleId) {
			this.userPageIndex = 1
			this.userPageSize = 10
			this.listItem = []
		}
		this.roleId = id
		var obj = {
			PageIndex: this.userPageIndex,
			PageSize: this.userPageSize,
			RoleId: this.roleId,
		}
		this.userService.getByRoleIdOnPage(obj).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				this.listUser = res.result.SYUserGetByRoleIdAllOnPage
				this.userPageIndex = res.result.PageIndex
				this.userPageSize = res.result.PageSize
				this.userTotalRecords = res.result.TotalCount
				$('#modalUsersByRoleId').modal('show')
			} else {
				this.userPageIndex = 1
				this.userPageSize = 10
				this.userTotalRecords = 0
			}
			return
		})
	}

	onPageChange2(event: any) {
		this.userPageSize = event.rows
		this.userPageIndex = event.first / event.rows + 1
		this.showListUser(this.roleId)
	}

	onDeleteUserRole(userId: any) {
		this.userService.deleteUserRole({ UserId: userId, RoleId: this.roleId }).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				if (res.result > 0) {
					this.toast.success(COMMONS.DELETE_SUCCESS)
					this.showListUser(this.roleId)
					this.getListPaged()
				} else {
					this.toast.error(COMMONS.DELETE_FAILED)
				}
			} else {
				this.toast.error(COMMONS.DELETE_FAILED)
			}
		})
	}
	onCreateUserRole() {
		if (this.listItem.length == 0) {
			this.toast.error('Vui lòng chọn người dùng')
			return
		} else {
			let listModel = []
			this.listItem.forEach((item) => {
				listModel.push({
					UserId: item,
					RoleId: this.roleId,
				})
			})
			this.userService.insertMultiUserRole(listModel).subscribe((res) => {
				if (res.success == RESPONSE_STATUS.success) {
					if (res.result.CountSuccess == 0) {
						this.toast.error(COMMONS.ADD_FAILED)
						return
					}
					this.toast.success('Thêm mới thành công ' + res.result.CountSuccess + ' người dùng')
					this.showListUser(this.roleId)
					this.getListPaged()
					this.listItem = []
				} else {
					this.toast.error(COMMONS.ADD_FAILED)
				}
			})
		}
	}
}
