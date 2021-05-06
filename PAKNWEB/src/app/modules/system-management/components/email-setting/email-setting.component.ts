import { Component, OnInit, Pipe, Directive } from '@angular/core'
import { FormGroup, FormBuilder, Validators } from '@angular/forms'
import { UserObject } from '../../../../models/UserObject'
import { SystemconfigService } from '../../../../services/systemconfig.service'
import { ToastrService } from 'ngx-toastr'

import { RESPONSE_STATUS, MESSAGE_COMMON } from 'src/app/constants/CONSTANTS'
import { NullTemplateVisitor } from '@angular/compiler'
declare var $: any
@Component({
	selector: 'app-email-setting',
	templateUrl: './email-setting.component.html',
	styleUrls: ['./email-setting.component.css'],
})
export class EmailSettingComponent implements OnInit {
	totalThongBao: number = 0
	myDate: any
	myHours: any
	pageindex: number
	listPageIndex: any[] = []
	listUser: any[] = []
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
	model: SystemEmail = new SystemEmail()
	submitted: boolean = false
	fromDate: string = ''
	toDate: string = ''
	form: FormGroup
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
	idDelete: number
	listStatus: any = [
		{ value: 1, text: 'Thành công' },
		{ value: 0, text: 'Thất bại' },
	]

	Notifications: any[]
	numberNotifications: any = 5
	ViewedCount: number = 0
	constructor(private _service: SystemconfigService, private _toastr: ToastrService, private _fb: FormBuilder) {}

	ngOnInit() {
		this.buildForm()
		this._service.getSystemEmail().subscribe(response => {
			console.log(response)
			if (response.success == RESPONSE_STATUS.success) {
				this.model = response.result.SYEmailGetFirst[0]
			} else {
				this._toastr.error(response.message)
			}
		}),
			error => {
				console.error(error)
				alert(error)
			}
	}
	get f() {
		return this.form.controls
	}
	resd = /^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/
	buildForm() {
		this.form = this._fb.group({
			password: [this.model.password, Validators.required],
			server: [this.model.server, Validators.required],
			port: [this.model.port, Validators.required],
			email: ['', [Validators.required, Validators.pattern(this.resd)]], //Validators.pattern('^[a-z][a-z0-9_.]{5,32}@[a-z0-9]{2,}(.[a-z0-9]{2,4}){1,2}$')
		})
	}
	rebuilForm() {
		this.form.reset({
			email: this.model.email,
			password: this.model.password,
			server: this.model.server,
			port: this.model.port,
		})
	}
	onPageChange(event: any) {
		this.pageSize = event.rows
		this.pageIndex = event.first / event.rows + 1
		this.getList()
	}
	dataStateChange() {
		this.pageIndex = 1
		this.getList()
	}
	preDelete(id: number) {
		this.idDelete = id
		$('#modalConfirmDelete').modal('show')
	}

	getList() {}
	onSave() {
		this.submitted = true
		if (this.form.invalid) {
			return
		}
		let req = {
			Email: this.model.email,
			Password: this.model.password,
			Server: this.model.server,
			Port: this.model.port,
		}
		this._service.updateSystemEmail(req).subscribe(response => {
			console.log(response)
			if (response.success == RESPONSE_STATUS.success) {
				this._toastr.success('Cập nhật cấu hình Email thành công')
			} else {
				this._toastr.error(response.message)
			}
		}),
			error => {
				console.error(error)
				alert(error)
			}
	}
}

export class SystemEmail {
	constructor() {
		this.email = ''
		this.password = ''
		this.server = ''
		this.port = ''
	}
	email: string = ''
	password: string = ''
	server: string = ''
	port: string = ''
}
