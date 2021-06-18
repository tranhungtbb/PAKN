import { Component, OnInit } from '@angular/core'
import { FormGroup, FormBuilder, Validators, FormGroupName } from '@angular/forms'
import { SystemconfigService } from '../../../../services/systemconfig.service'
import { ToastrService } from 'ngx-toastr'
import {ActivatedRoute} from '@angular/router'
import { RESPONSE_STATUS, TYPECONFIG } from 'src/app/constants/CONSTANTS'
import {SystemtConfig, ConfigEmail} from 'src/app/models/systemtConfigObject'
import { COMMONS } from 'src/app/commons/commons'
@Component({
	selector: 'app-email-setting',
	templateUrl: './email-setting.component.html',
	styleUrls: ['./email-setting.component.css'],
})
export class EmailSettingComponent implements OnInit {
	model: SystemtConfig = new SystemtConfig()
	submitted: boolean = false
	configEmail : ConfigEmail = new ConfigEmail()
	
	form: FormGroup
	constructor(private _service: SystemconfigService, private _toastr: ToastrService, private _fb: FormBuilder, private activatedRoute : ActivatedRoute) {}

	ngOnInit() {
		this.activatedRoute.params.subscribe((params) => {
			let id = +params['id']
			if(!isNaN(id)){
				this.model.id = Number(id)
				this.onCancel()
			}
		})
	}
	onCancel() {
		this.buildForm()
		this._service.syConfigGetById({Id : this.model.id}).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result.SYConfigGetByID.length > 0) {
					this.model = {...response.result.SYConfigGetByID[0]}
					this.configEmail = JSON.parse(this.model.content)
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
			title : [this.model.title , Validators.required],
			description : [this.model.description , Validators.required],
			password: [this.configEmail.password, Validators.required],
			server: [this.configEmail.server, Validators.required],
			port: [this.configEmail.port, Validators.required],
			email: [this.configEmail.email, [Validators.required, Validators.pattern(this.resd)]], //Validators.pattern('^[a-z][a-z0-9_.]{5,32}@[a-z0-9]{2,}(.[a-z0-9]{2,4}){1,2}$')
		})
	}
	rebuilForm() {
		this.form.reset({
			title : this.model.title,
			description : this.model.description,
			email: this.configEmail.email,
			password: this.configEmail.password,
			server: this.configEmail.server,
			port: this.configEmail.port,
		})
	}
	onSave() {
		this.model.type = TYPECONFIG.CONFIG_EMAIL
		this.submitted = true
		if (this.form.invalid) {
			return
		}
		debugger
		this.model.content = JSON.stringify(this.configEmail)
		this._service.syConfigUpdate(this.model).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				this._toastr.success(COMMONS.UPDATE_SUCCESS)
			} else {
				this._toastr.error(response.message)
			}
		}),
			(error) => {
				console.error(error)
				alert(error)
			}
	}
	redirectHis(){
		window.history.back()
	}
}

