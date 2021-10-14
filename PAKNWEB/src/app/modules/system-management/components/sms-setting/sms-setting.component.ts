import { Component, OnInit, Pipe, Directive } from '@angular/core'
import { FormGroup, FormBuilder, Validators } from '@angular/forms'
import { SystemconfigService } from '../../../../services/systemconfig.service'
import { ToastrService } from 'ngx-toastr'
import { ActivatedRoute, Router } from '@angular/router'
import { RESPONSE_STATUS, TYPECONFIG } from 'src/app/constants/CONSTANTS'
import { SystemtConfig, ConfigSMS } from 'src/app/models/systemtConfigObject'
import { COMMONS } from 'src/app/commons/commons'

@Component({
	selector: 'app-sms-setting',
	templateUrl: './sms-setting.component.html',
	styleUrls: ['./sms-setting.component.css'],
})
export class SmsSettingComponent implements OnInit {
	model: SystemtConfig = new SystemtConfig()
	configSMS: ConfigSMS = new ConfigSMS()
	submitted: boolean = false
	form: FormGroup

	listStatus: any = [
		{ value: true, text: 'Tin nhắn nội dung có dấu' },
		{ value: false, text: 'Tin nhắn nội dung không dấu' },
	]

	constructor(private _service: SystemconfigService, private _router: Router, private _toastr: ToastrService, private _fb: FormBuilder, private activatedRoute: ActivatedRoute) {}

	ngOnInit() {
		this.activatedRoute.params.subscribe((params) => {
			let id = +params['id']
			if (!isNaN(id)) {
				this.model.id = Number(id)
				this.onCancel()
			}
		})
	}
	onCancel() {
		this.buildForm()
		this._service.syConfigGetById({ Id: this.model.id }).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result.SYConfigGetByID.length > 0) {
					this.model = response.result.SYConfigGetByID[0]
					this.configSMS = JSON.parse(this.model.content)
				}
			} else {
				this._toastr.error(response.message)
			}
		}),
			(error) => {
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
			title: [this.model.title, Validators.required],
			description: [this.model.description, Validators.required],
			linkwebservice: [this.configSMS.linkwebservice, Validators.required],
			password: [this.configSMS.password, Validators.required],
			user: [this.configSMS.user, Validators.required],
			code: [this.configSMS.code, Validators.required],
			serviceID: [this.configSMS.serviceID, Validators.required],
			commandCode: [this.configSMS.commandCode, Validators.required],
			contentType: [this.configSMS.contentType, Validators.required],
		})
	}
	rebuilForm() {
		this.form.reset({
			title: this.model.title,
			description: this.model.description,
			linkwebservice: this.configSMS.linkwebservice,
			password: this.configSMS.password,
			user: this.configSMS.user,
			code: this.configSMS.code,
			serviceID: this.configSMS.serviceID,
			commandCode: this.configSMS.commandCode,
			contentType: this.configSMS.contentType,
		})
	}
	onSave() {
		this.model.type = TYPECONFIG.CONFIG_SMS
		this.submitted = true
		if (this.form.invalid) {
			return
		}
		this.model.content = JSON.stringify(this.configSMS)

		this._service.syConfigUpdate(this.model).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				this._toastr.success(COMMONS.UPDATE_SUCCESS)
				this._router.navigate(['quan-tri/he-thong/cau-hinh-he-thong'])
			} else {
				this._toastr.error(response.message)
			}
		}),
			(error) => {
				console.error(error)
				alert(error)
			}
	}
	redirectHis() {
		window.history.back()
	}
}
