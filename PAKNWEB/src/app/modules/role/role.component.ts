import { Component, OnInit, Input, ViewChild } from '@angular/core'
import { ToastrService } from 'ngx-toastr'

import { RESPONSE_STATUS, STATUS_HISNEWS } from 'src/app/constants/CONSTANTS'
import { RoleService } from 'src/app/services/role.service'
import { COMMONS } from 'src/app/commons/commons'
import { NewsModel, HISNewsModel } from 'src/app/models/NewsObject'
declare var $: any
@Component({
	selector: 'app-role',
	templateUrl: './role.component.html',
	styleUrls: ['./role.component.css'],
})
//acbd
export class RoleComponent implements OnInit {
	constructor(private roleService: RoleService, private toast: ToastrService) {}
	@ViewChild('table', { static: false }) table: any

	listStatus: any = [
		{ value: '', text: 'Trạng thái' },
		{ value: true, text: 'Hiệu lực' },
		{ value: false, text: 'Hết hiệu lực' },
	]
	pageIndex: Number = 1
	pageSize: Number = 20
	name: string
	description: string
	isActive: boolean
	totalCount: Number
	listData: any[]

	ngOnInit() {
		this.getListPaged()
	}

	getListPaged() {
		this.roleService
			.getAllPagedList({
				pageIndex: this.pageIndex,
				pageSize: this.pageSize,
				name: this.name,
				description: this.description,
				isActived: this.isActive == null ? '' : this.isActive,
			})
			.subscribe((res) => {
				if (res.success != 'OK') return
				this.listData = res.result.SYRoleGetAllOnPage

				if (this.totalCount == null || this.totalCount == 0) this.totalCount = res.result.TotalCount
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
}
