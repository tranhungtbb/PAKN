import { Component, OnInit, ViewChild } from '@angular/core'
import { SMSManagementService } from 'src/app/services/sms-management'
import { COMMONS } from 'src/app/commons/commons'
import { Router } from '@angular/router'
import { ToastrService } from 'ngx-toastr'

import { STATUS_HIS_SMS } from 'src/app/constants/CONSTANTS'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { smsManagementGetAllOnPageObject, smsManagementObject, smsManagementMapObject } from 'src/app/models/smsManagementObject'

declare var $: any
@Component({
	selector: 'app-sms-sent',
	templateUrl: './sms-sent.component.html',
	styleUrls: ['./sms-sent.component.css'],
})
export class SMSSentComponent implements OnInit {
	constructor(private smsService: SMSManagementService, private toast: ToastrService, private routes: Router) {
		this.AdministrativeUnits = []
	}
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
	unitId: number
	type: string
	status: Number
	pageIndex: Number = 1
	pageSize: Number = 20
	totalRecords: Number
	listData: Array<smsManagementGetAllOnPageObject>

	hisStatus: Number
	hisContent: string
	hisCreateDate: string
	hisUserCreate: string
	hisPageIndex: number = 1
	hisPageSize: number = 10
	hisTotalRecords: Number
	SMSId: Number
	listHis: any[]
	AdministrativeUnits: any[] = []

	smsId: any

	model: smsManagementObject = new smsManagementObject()
	listItemUserSelected: Array<smsManagementMapObject>

	ngOnInit() {
		this.getListPaged()
		this.getAdministrativeUnits()
	}

	getSMSModelById(id: any) {
		this.smsService.GetById({ id: id }).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				if (res.result) {
					this.model = { ...res.result.model }
					this.listItemUserSelected = [...res.result.individualBusinessInfo]
					$('#modalDetail').modal('show')
				}
			}
		})
	}

	getAdministrativeUnits() {
		this.smsService.GetListAdmintrative({ id: 37 }).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				this.AdministrativeUnits = res.result.CAAdministrativeUnitsGetDropDown
				// this.getSMSModelById()
			} else {
				this.AdministrativeUnits = []
			}
		})
	}

	getListPaged() {
		this.title = this.title.trim()
		// this.unitName = this.unitName.trim()
		this.smsService
			.GetListOnPage({
				PageIndex: this.pageIndex,
				PageSize: this.pageSize,
				Title: this.title == null ? '' : this.title,
				UnitId: this.unitId == null ? '' : this.unitId,
				Type: this.type == null ? '' : this.type,
				Status: 2,
			})
			.subscribe((res) => {
				if (res.success != RESPONSE_STATUS.success) {
					this.totalRecords = 0
					return
				}
				this.listData = res.result.SMSQuanLyTinNhanGetAllOnPage
				this.listData.forEach((item) => {
					let arr = []
					item.type != null ? (arr = item.type.split(',')) : []
					item.type = ''
					arr.forEach((i) => {
						this.listCategory.forEach((element) => {
							if (i == element.value) {
								item.type += element.text + ', '
							}
						})
					})
					item.type = item.type.substr(0, item.type.length - 2)
					item.title = item.title.concat('. ')
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
		this.smsId = id
	}

	dataStateChange() {
		this.pageIndex = 1
		this.table.first = 0
		this.getListPaged()
	}

	onDelete() {
		$('#modalConfirm').modal('hide')
		let obj = this.listData.find((x) => x.id == this.smsId)
		this.smsService.Delete({ id: this.smsId }, obj.title).subscribe((res) => {
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
	confirm(id: Number) {
		this.smsId = id
		$('#modalConfirm').modal('show')
	}

	redirectCreate() {
		this.routes.navigate(['quan-tri/email-sms/sms/them-moi'])
	}

	clearModelHis() {
		this.hisPageSize = 10
		this.hisPageIndex = 1
		this.hisContent = ''
		this.hisStatus = null
		this.hisUserCreate = ''
		this.hisCreateDate = ''
	}

	getHistory(id: Number) {
		if (id == undefined) return
		if (id != this.SMSId) {
			this.hisPageSize = 10
			this.hisPageIndex = 1
		}
		this.SMSId = id
		this.hisContent == null ? (this.hisContent = '') : (this.hisContent = this.hisContent.trim())
		this.hisUserCreate == null ? (this.hisContent = '') : (this.hisUserCreate = this.hisUserCreate.trim())
		var obj = {
			PageSize: this.hisPageSize,
			PageIndex: this.hisPageIndex,
			SMSId: id,
			CreateDate: this.hisCreateDate == null ? '' : this.hisCreateDate,
			Content: this.hisContent == null ? '' : this.hisContent,
			UserName: this.hisUserCreate == null ? '' : this.hisUserCreate,
			Status: this.hisStatus == null ? '' : this.hisStatus,
		}
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
					this.hisPageSize = 10
					this.hisPageIndex = 1
				}
				$('#modalHisSMS').modal('show')
			} else {
				this.listHis = []
				this.hisTotalRecords = 0
				this.hisPageSize = 10
				this.hisPageIndex = 1
				this.toast.error(res.message)
			}
		})
	}

	// sendDateChange(data) {
	// 	this.hisCreateDate = data
	// 	debugger
	// 	this.getHistory(this.SMSId)
	// }

	sendDateChange(newDate) {
		newDate != null ? (this.hisCreateDate = JSON.stringify(new Date(newDate)).slice(1, 11)) : (this.hisCreateDate = '')
		// this.pageIndex = 1
		// this.table.first = 0
		this.getHistory(this.SMSId)
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
