import { Component, OnInit, ViewChild } from '@angular/core'
import { SMSManagementService } from 'src/app/services/sms-management'
import { COMMONS } from 'src/app/commons/commons'
import { Router } from '@angular/router'
import { ToastrService } from 'ngx-toastr'

import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { smsManagementGetAllOnPageObject } from 'src/app/models/smsManagementObject'
import { UserService } from 'src/app/services/user.service'

declare var $: any
@Component({
	selector: 'app-sms',
	templateUrl: './sms-management.component.html',
	styleUrls: ['./sms-management.component.css'],
})
export class SMSManagementComponent implements OnInit {
	constructor(private smsService: SMSManagementService, private toast: ToastrService, private routes: Router, private userService: UserService) {}
	@ViewChild('table', { static: false }) table: any

	listStatus: any = [
		{ value: 1, text: 'Đang soạn thảo' },
		{ value: 2, text: 'Đã gửi' },
	]

	listCategory: any = [
		{ value: 1, text: 'Người dân' },
		{ value: 2, text: 'Doanh nghiệp' },
	]
	title: string = ''
	unitName: string = ''
	type: string = '1,2'
	status: Number
	pageIndex: Number = 1
	pageSize: Number = 20
	totalRecords: Number
	listData: any[]

	InvitationId: any

	ngOnInit() {
		this.getListPaged()
	}

	getListPaged() {
		this.title = this.title.trim()
		this.unitName = this.unitName.trim()
		this.smsService
			.GetListOnPage({
				PageIndex: this.pageIndex,
				PageSize: this.pageSize,
				Title: this.title == null ? '' : this.title,
				UnitName: this.unitName == null ? '' : this.unitName,
				Type: this.type == null ? '1,2' : this.type,
				Status: this.status == null ? '' : this.status,
			})
			.subscribe((res) => {
				if (res.success != RESPONSE_STATUS.success) {
					this.totalRecords = 0
					return
				}
				debugger
				this.listData = res.result.SMSQuanLyTinNhanGetAllOnPage
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

	confirm(id: Number) {
		this.InvitationId = id
		$('#modalConfirm').modal('show')
	}

	onDelete() {
		$('#modalConfirm').modal('hide')
		this.smsService.Delete({ id: this.InvitationId }).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				if (res.result > 0) {
					this.toast.success(COMMONS.DELETE_SUCCESS)
					this.getListPaged()
				} else {
					this.toast.error(COMMONS.DELETE_FAILED)
				}
			} else {
				this.toast.error(COMMONS.DELETE_FAILED)
				this.getListPaged()
			}
		})
	}

	redirectCreate() {
		this.routes.navigate(['quan-tri/thu-moi/them-moi'])
	}

	redirectUpdate(id: number, status: number) {
		if (status == 1) {
			this.routes.navigate(['quan-tri/thu-moi/cap-nhap/' + id])
		}
		return
	}
}
