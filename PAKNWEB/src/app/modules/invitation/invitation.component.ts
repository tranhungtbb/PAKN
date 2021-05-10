import { Component, OnInit, ViewChild } from '@angular/core'
import { InvitationService } from 'src/app/services/invitation.service'
import { COMMONS } from 'src/app/commons/commons'
import { Router } from '@angular/router'
import { ToastrService } from 'ngx-toastr'
import { defineLocale } from 'ngx-bootstrap/chronos'
import { viLocale } from 'ngx-bootstrap/locale'
import { BsDatepickerConfig, BsLocaleService } from 'ngx-bootstrap/datepicker'

import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { InvitationObject } from 'src/app/models/invitationObject'
import { UserService } from 'src/app/services/user.service'

declare var $: any
defineLocale('vi', viLocale)

@Component({
	selector: 'app-invitation',
	templateUrl: './invitation.component.html',
	styleUrls: ['./invitation.component.css'],
})
export class InvitationComponent implements OnInit {
	constructor(
		private invitationService: InvitationService,
		private toast: ToastrService,
		private routes: Router,
		private userService: UserService,
		private BsLocaleService: BsLocaleService
	) {}
	@ViewChild('table', { static: false }) table: any

	listStatus: any = [
		{ value: 1, text: 'Đang soạn thảo' },
		{ value: 2, text: 'Đã gửi' },
	]
	model: any = new InvitationObject()
	title: string = ''
	place: string = ''
	startDate: any
	endDate: any
	status: Number
	pageIndex: Number = 1
	pageSize: Number = 20
	totalRecords: Number
	listData: any[]

	InvitationId: any

	ngOnInit() {
		this.getListPaged()
		this.BsLocaleService.use('vi')
	}

	getListPaged() {
		this.title = this.title.trim()
		this.place = this.place.trim()
		this.invitationService
			.invitationGetList({
				PageIndex: this.pageIndex,
				PageSize: this.pageSize,
				StartDate: this.startDate == null ? '' : this.startDate,
				EndDate: this.endDate == null ? '' : this.endDate,
				Title: this.title == null ? '' : this.title,
				Place: this.place == null ? '' : this.place,
				Status: this.status == null ? '' : this.status,
			})
			.subscribe((res) => {
				if (res.success != RESPONSE_STATUS.success) {
					this.totalRecords = 0
					return
				}
				this.listData = res.result.INVInvitationGetAllOnPage
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
		this.invitationService.delete({ id: this.InvitationId }).subscribe((res) => {
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

	redirectUpdate(id: number) {
		this.routes.navigate(['quan-tri/thu-moi/cap-nhap/' + id])
		return
	}
	sendDateChange(data) {
		if (data != null) {
			let count = 0
			for (const iterator of data) {
				var date = JSON.stringify(new Date(iterator)).slice(1, 11)
				if (count == 0) {
					this.startDate = date
				} else {
					this.endDate = date
				}
				count++
			}
		} else {
			this.startDate = this.endDate = ''
		}
		this.getListPaged()
	}

	getHistory(id: any) {
		return
	}
}
