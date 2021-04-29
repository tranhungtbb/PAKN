import { Component, OnInit, Pipe, Directive } from '@angular/core'
import { FormGroup } from '@angular/forms'
import { UserObject } from '../../../../models/UserObject'
import { UserService } from '../../../../services/user.service'

import { RESPONSE_STATUS, RECOMMENDATION_STATUS } from 'src/app/constants/CONSTANTS'
import { NullTemplateVisitor } from '@angular/compiler'

@Component({
	selector: 'app-system-log',
	templateUrl: './system-log.component.html',
	styleUrls: ['./system-log.component.css'],
})
export class SystemLogComponent implements OnInit {
	totalThongBao: number = 0
	myDate: any
	myHours: any
	pageindex: number
	listPageIndex: any[] = []

	listData: any[] = []
	totalRecords: number = 0
	emailUser: string = ''

	pageSizeGrid: number = 10
	files: any
	updateForm: FormGroup
	userForm: FormGroup
	userName: string
	errorMessage: any
	year: Date = new Date()
	notifications: any[] = []
	mindateUyQuyen: Date = new Date()
	exchange: FormGroup
	lstUserbyDep: Array<any> = []
	lstTimline: Array<any> = []
	lstTimlineMore: Array<any> = []
	model: UserObject = new UserObject()
	submitted: boolean = false
	listSexs: any[] = [
		{ text: 'Nam', value: true },
		{ text: 'Nữ', value: false },
	]
	lstDropDownLayout = [
		{ value: 1, Text: 'Thành công' },
		{ value: 0, Text: 'Thất bại' },
	]
	fromDate: string = ''
	toDate: string = ''
	form: FormGroup
	dataSearch: SearchHistoryUser = new SearchHistoryUser()
	public timeOut: number = 1
	public exchangedata: any = {}
	listThongBao: any = []
	formData = new FormData()
	showLoader: boolean = true
	tenDonVi: string = ''
	remindWork: any = {}
	minDate: Date = new Date()

	pageIndex: number = 1
	pageSize: number = 20
	lstChucVu: any = []
	lstPhongBan: any = []

	Notifications: any[]
	numberNotifications: any = 5
	ViewedCount: number = 0
	constructor(private userService: UserService) {}

	ngOnInit() {
		this.getList()
	}
	onPageChange(event: any) {
		this.pageSize = event.rows
		this.pageIndex = event.first / event.rows + 1
		this.getList()
	}
	getList() {
		let req = {
			FromDate: this.dataSearch.fromDate != null ? this.dataSearch.fromDate.toLocaleDateString() : '',
			ToDate: this.dataSearch.toDate != null ? this.dataSearch.toDate.toLocaleDateString() : '',
			PageIndex: this.pageIndex,
			PageSize: this.pageSize,
			//UserId: localStorage.getItem('userId'),
			Description: this.dataSearch.description,
			Status: this.dataSearch.status != null ? this.dataSearch.status : '',
			UserId: this.dataSearch.userId != null ? this.dataSearch.userId : '',
		}
		console.log(req)
		this.userService.getSystemLoginAdmin(req).subscribe(response => {
			console.log(response)
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result != null) {
					this.listData = []
					this.listData = response.result.SYSystemLogGetAllOnPageAdmin
					this.totalRecords = response.result.SYSystemLogGetAllOnPageAdmin.length != 0 ? response.result.SYSystemLogGetAllOnPageAdmin[0].rowNumber : 0
				}
			} else {
			}
		})
	}
}

export class SearchHistoryUser {
	constructor() {
		this.fromDate = null
		this.toDate = null
		this.status = null
		this.description = ''
		this.userId = null
	}
	fromDate: Date
	toDate: Date
	description: string = ''
	status: number = null
	userId: number = null
}
