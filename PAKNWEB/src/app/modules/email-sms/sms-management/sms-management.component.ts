import { Component, OnInit, ViewChild } from '@angular/core'
import { SMSManagementService } from 'src/app/services/sms-management'
import { COMMONS } from 'src/app/commons/commons'
import { Router } from '@angular/router'
import { ToastrService } from 'ngx-toastr'

import { STATUS_HIS_SMS } from 'src/app/constants/CONSTANTS'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { smsManagementGetAllOnPageObject } from 'src/app/models/smsManagementObject'

declare var $: any
@Component({
	selector: 'app-sms',
	templateUrl: './sms-management.component.html',
	styleUrls: ['./sms-management.component.css'],
})
export class SMSManagementComponent implements OnInit {
	constructor(private smsService: SMSManagementService, private toast: ToastrService, private routes: Router) {}
	@ViewChild('table', { static: false }) table: any
	@ViewChild('table2', { static: false }) table2: any

	listStatus: any = [
		{ value: 1, text: 'Đang soạn thảo' },
		{ value: 2, text: 'Đã gửi' },
	]

	listCategory: any = [
		{ value: '1', text: 'Cá nhân' },
		{ value: '2', text: 'Doanh nghiệp' },
	]

	listHisStatus: any = [
		{ value: '0', text: 'Khởi tạo' },
		{ value: '1', text: 'Cập nhập' },
		{ value: '2', text: 'Đã gửi' },
	]
	title: string = ''
	unitName: string = ''
	type: string
	status: Number
	pageIndex: Number = 1
	pageSize: Number = 20
	totalRecords: Number
	listData: Array<smsManagementGetAllOnPageObject>

	hisStatus: Number
	hisContent: string
	hisUserCreate: string
	hisPageIndex: number = 1
	hisPageSize: number = 20
	hisTotalRecords: Number
	SMSId: Number
	listHis: any[]

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
				Type: this.type == null ? '' : this.type,
				Status: this.status == null ? '' : this.status,
			})
			.subscribe((res) => {
				if (res.success != RESPONSE_STATUS.success) {
					this.totalRecords = 0
					return
				}
				this.listData = res.result.SMSQuanLyTinNhanGetAllOnPage
				this.listData.forEach((item) => {
					let arr = item.type.split(',')
					item.type = ''
					arr.forEach((i) => {
						this.listCategory.forEach((element) => {
							if (i == element.value) {
								item.type += element.text + ', '
							}
						})
					})
					item.type = item.type.substr(0, item.type.length - 2)
				})
				this.totalRecords = res.result.TotalCount
			})
	}

	onPageChange(event: any) {
		this.pageSize = event.rows
		this.pageIndex = event.first / event.rows + 1
		this.getListPaged()
	}

	onSend(id: Number) {
		$('#modalConfirmChangeStatus').modal('show')
		this.InvitationId = id
	}

	onUpdateStatusTypeSend() {
		$('#modalConfirmChangeStatus').modal('hide')
		this.smsService.UpdateStatusSend({ idMSMS: this.InvitationId }).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				// ghi his

				this.smsService
					.InsertHisSMS({
						ObjectId: this.InvitationId,
						Status: STATUS_HIS_SMS.SEND,
					})
					.subscribe()

				this.toast.success('Gửi thành công')
				this.getListPaged()
			} else {
				this.toast.error('Lỗi khi gửi')
				return
			}
		})
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
		this.routes.navigate(['quan-tri/email-sms/sms/them-moi'])
	}

	redirectUpdate(id: number, status: number) {
		if (status == 1) {
			this.routes.navigate(['quan-tri/email-sms/sms/cap-nhap/' + id])
		}
		return
	}

	getHistory(id: Number) {
		this.SMSId = id
		var obj = {
			PageSize: this.hisPageSize,
			PageIndex: this.hisPageIndex,
			SMSId: id,
			Content: this.hisContent == null ? '' : this.hisContent,
			UserName: this.hisUserCreate == null ? '' : this.hisUserCreate,
			Status: this.hisStatus == null ? '' : this.hisStatus,
		}
		debugger
		this.smsService.GetListHisOnPage(obj).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				if (res.result.HISSMSGetBySMSIdOnPage.length > 0) {
					this.listHis = res.result.HISSMSGetBySMSIdOnPage
					this.hisTotalRecords = res.result.TotalCount
					this.hisPageSize = res.result.PageSize
					this.hisPageIndex = res.result.PageIndex
				} else {
					this.listHis = []
					this.hisTotalRecords = 0
					this.hisPageSize = 20
					this.hisPageIndex = 1
				}
				$('#modalHisSMS').modal('show')
			} else {
				this.listHis = []
				this.hisTotalRecords = 0
				this.hisPageSize = 20
				this.hisPageIndex = 1
				this.toast.error(res.message)
			}
		})
	}

	onPageChange2(event: any) {
		this.hisPageSize = event.rows
		this.hisPageIndex = event.first / event.rows + 1
		this.getHistory(this.SMSId)
	}
	dataStateChange2() {
		this.hisPageIndex = 1
		this.table.first = 0
		this.getHistory(this.SMSId)
	}
}
