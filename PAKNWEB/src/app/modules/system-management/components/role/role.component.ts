import { RoleService } from 'src/app/services/role.service'
import { COMMONS } from 'src/app/commons/commons'
import { Router } from '@angular/router'
import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'
import { RoleObject } from '../../../../models/roleObject'
import { from } from 'rxjs'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'

declare var $: any
@Component({
	selector: 'app-role',
	templateUrl: './role.component.html',
	styleUrls: ['./role.component.css'],
})
//acbd
export class RoleComponent implements OnInit {
	constructor(private roleService: RoleService, private toast: ToastrService, private routes: Router) {}
	@ViewChild('table', { static: false }) table: any

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

	ngOnInit() {
		this.getListPaged()
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
}
