import { Component, OnInit, ViewChild } from '@angular/core'
import { InvitationService } from 'src/app/services/invitation.service'
import { COMMONS } from 'src/app/commons/commons'
import { Router } from '@angular/router'
import { ToastrService } from 'ngx-toastr'
import { defineLocale } from 'ngx-bootstrap/chronos'
import { viLocale } from 'ngx-bootstrap/locale'
import { BsLocaleService } from 'ngx-bootstrap/datepicker'
import { DataService } from 'src/app/services/sharedata.service'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { InvitationObject, InvitationUserMapObject } from 'src/app/models/invitationObject'
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
		private BsLocaleService: BsLocaleService,
		private _shareData: DataService
	) {
		this.listItemUserSelected = []
	}
	@ViewChild('table', { static: false }) table: any
	@ViewChild('tableHis', { static: false }) tableHis: any
	// @ViewChild('tableLstUser', { static: false }) tableLstUser: any

	listStatus: any = [
		{ value: 1, text: 'Đang soạn thảo' },
		{ value: 2, text: 'Đã gửi' },
		{ value: 3, text: 'Chưa xem' },
		{ value: 4, text: 'Đã xem' },
	]

	listHisStatus: any = [
		{ value: '0', text: 'Khởi tạo' },
		{ value: '1', text: 'Cập nhập' },
		{ value: '2', text: 'Đã gửi' },
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
	listItemUserSelected: any[]
	listFile: any[]
	listUserIsSystem: any[]
	InvitationId: any

	// his

	hisStatus: Number
	hisContent: string
	hisCreateDate: string
	hisUserCreate: string
	hisPageIndex: number = 1
	hisPageSize: number = 10
	hisTotalRecords: Number
	hisInvitationId: Number
	listHis: any[]

	// lts user
	lstUser: any = []
	pagination: any = []
	totalRecordsUser: number
	watchedDate: Date
	queryLstUser: queryLstUser = new queryLstUser()

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
		let obj = this.listData.find((x) => x.id == this.InvitationId)
		this.invitationService.delete({ id: this.InvitationId }, obj.title).subscribe((res) => {
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

	redirectDetail(id: number) {
		this.routes.navigate(['quan-tri/thu-moi/chi-tiet/' + id])
		return
	}

	preUserWatched(id: number) {
		if (this.queryLstUser.InvitationId != 0 && this.queryLstUser.InvitationId != id) {
			this.queryLstUser = new queryLstUser()
		}
		this.queryLstUser.InvitationId = id
		this.queryLstUser.UserName = this.queryLstUser.UserName.trim()
		this.watchedDate == null ? (this.queryLstUser.WatchedDate = '') : (this.queryLstUser.WatchedDate = this.watchedDate.toDateString())
		$('#modalLstUser').modal('show')
		this.invitationService.userReadedInvitationGetList(this.queryLstUser).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				if (res.result.SYUserReadedInvitationGetAllOnPage.length > 0) {
					this.lstUser = res.result.SYUserReadedInvitationGetAllOnPage
					this.totalRecordsUser = res.result.TotalCount
				} else {
					this.lstUser = []
					this.totalRecordsUser = 0
					this.queryLstUser.PageIndex = 1
					this.queryLstUser.PageSize = 20
				}
				this.padi()
			} else {
				this.lstUser = []
				this.totalRecordsUser = 0
				this.queryLstUser.PageIndex = 1
				this.queryLstUser.PageSize = 20
				this.padi()
			}
		}),
			(error) => {
				console.log(error)
				alert(error)
			}
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

	padi() {
		this.pagination = []
		for (let i = 0; i < Math.ceil(this.totalRecordsUser / this.queryLstUser.PageSize); i++) {
			this.pagination.push({ index: i + 1 })
		}
	}

	changePagination(index: any) {
		if (this.queryLstUser.PageIndex > index) {
			if (index > 0) {
				this.queryLstUser.PageIndex = index
				this.preUserWatched(this.queryLstUser.InvitationId)
			}
			return
		} else if (this.queryLstUser.PageIndex < index) {
			if (this.pagination.length >= index) {
				this.queryLstUser.PageIndex = index
				this.preUserWatched(this.queryLstUser.InvitationId)
			}
			return
		}
		return
	}

	// his

	getHistory(id: Number) {
		if (id == undefined) return
		if (id != this.hisInvitationId) {
			this.tableHis.reset()
			this.hisPageSize = 10
			this.hisPageIndex = 1
		}
		this.hisInvitationId = id
		this.hisContent == null ? (this.hisContent = '') : (this.hisContent = this.hisContent.trim())
		this.hisUserCreate == null ? (this.hisContent = '') : (this.hisUserCreate = this.hisUserCreate.trim())
		var obj = {
			PageSize: this.hisPageSize,
			PageIndex: this.hisPageIndex,
			ObjectId: id,
			CreateDate: this.hisCreateDate == null ? '' : this.hisCreateDate,
			Content: this.hisContent == null ? '' : this.hisContent,
			UserName: this.hisUserCreate == null ? '' : this.hisUserCreate,
			Status: this.hisStatus == null ? '' : this.hisStatus,
		}
		this.invitationService.GetListHisOnPage(obj).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				if (res.result.HISInvitationGetByInvitaionIdOnPage.length > 0) {
					this.listHis = res.result.HISInvitationGetByInvitaionIdOnPage
					this.hisTotalRecords = res.result.TotalCount
					this.hisPageSize = res.result.PageSize
					this.hisPageIndex = res.result.PageIndex
				} else {
					this.listHis = []
					this.hisTotalRecords = 0
					this.hisPageSize = 10
					this.hisPageIndex = 1
				}
				$('#modalHisInvitation').modal('show')
			} else {
				this.listHis = []
				this.hisTotalRecords = 0
				this.hisPageSize = 10
				this.hisPageIndex = 1
				this.toast.error(res.message)
			}
		})
	}

	onPageChange2(event: any) {
		this.hisPageSize = event.rows
		this.hisPageIndex = event.first / event.rows + 1
		this.getHistory(this.hisInvitationId)
	}
	dataStateChange2() {
		this.hisPageIndex = 1
		this.table.first = 0
		this.getHistory(this.hisInvitationId)
	}

	onExport() {
		let passingObj: any = {}
		$('#modalLstUser').modal('hide')
		let invitation = this.listData.find((x) => x.id == this.queryLstUser.InvitationId)
		passingObj.TitleReport = 'THỐNG KÊ NGƯỜI XEM THƯ MỜI : ' + invitation.title.toUpperCase()
		passingObj.InvitationId = this.queryLstUser.InvitationId
		this._shareData.setobjectsearch(passingObj)
		this._shareData.sendReportUrl = 'UserReadedInvitationByInvitationId?' + JSON.stringify(passingObj)
		this.routes.navigate(['quan-tri/xuat-file'])
	}
}

class queryLstUser {
	InvitationId: number
	UserName: string
	WatchedDate: string
	PageSize: number
	PageIndex: number
	constructor() {
		this.InvitationId = 0
		this.UserName = ''
		this.WatchedDate = ''
		this.PageSize = 20
		this.PageIndex = 1
	}
}
