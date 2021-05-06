import { Component, OnInit, Pipe, Directive } from '@angular/core'
import { FormGroup, FormBuilder, Validators } from '@angular/forms'
import { UserObject } from '../../../../models/UserObject'
import { SystemconfigService } from '../../../../services/systemconfig.service'
import { ToastrService } from 'ngx-toastr'

import { RESPONSE_STATUS, MESSAGE_COMMON } from 'src/app/constants/CONSTANTS'
import { NullTemplateVisitor } from '@angular/compiler'
declare var $: any
@Component({
	selector: 'app-sms-setting',
	templateUrl: './sms-setting.component.html',
	styleUrls: ['./sms-setting.component.css'],
})
export class SmsSettingComponent implements OnInit {
	model: any = new SystemSMS()
	submitted: boolean = false
	form: FormGroup
	linkwebservice: string = ''
	password: string = ''
	user: string = ''
	code: string = ''
	serviceID: string = ''
	commandCode: string = ''
	contenType: boolean
	listStatus: any = [
		{ value: true, text: 'Tin nhắn nội dung có dấu' },
		{ value: false, text: 'Tin nhắn nội dung không dấu' },
	]

	Notifications: any[]
	numberNotifications: any = 5
	ViewedCount: number = 0
	constructor(private _service: SystemconfigService, private _toastr: ToastrService, private _fb: FormBuilder) {}

	ngOnInit() {
		this.buildForm()
		this._service.getSystemSMS().subscribe(response => {
			console.log(response)
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result.SYSMSGetFirst.length != 0) {
					this.model = response.result.SYSMSGetFirst[0]
				}
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
			linkwebservice: [this.model.linkwebservice, Validators.required],
			password: [this.model.password, Validators.required],
			user: [this.model.user, Validators.required],
			code: [this.model.code, Validators.required],
			serviceID: [this.model.serviceID, Validators.required],
			commandCode: [this.model.commandCode, Validators.required],
			contenType: [this.model.contenType, Validators.required],
		})
	}

	getList() {}
	onSave() {
		this.submitted = true
		if (this.form.invalid) {
			return
		}
		let req = {
			Linkwebservice: this.model.linkwebservice,
			Password: this.model.password,
			User: this.model.user,
			Code: this.model.code,
			ServiceID: this.model.serviceID,
			CommandCode: this.model.commandCode,
			ContenType: this.model.contenType,
		}
		this._service.updateSystemSMS(req).subscribe(response => {
			console.log(response)
			if (response.success == RESPONSE_STATUS.success) {
				this._toastr.success('Cập nhật cấu hình SMS thành công')
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

export class SystemSMS {
	constructor() {
		this.linkwebservice = ''
		this.password = ''
		this.user = ''
		this.code = ''
		this.serviceID = ''
		this.commandCode = ''
		this.contenType = null
	}
	linkwebservice: string = ''
	password: string = ''
	user: string = ''
	code: string = ''
	serviceID: string = ''
	commandCode: string = ''
	contenType: boolean
}
