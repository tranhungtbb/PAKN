import { Component, OnInit } from '@angular/core'
import { FormGroup, FormBuilder, Validators } from '@angular/forms'
import { SystemconfigService } from '../../../../services/systemconfig.service'
import { ToastrService } from 'ngx-toastr'
import { ActivatedRoute } from '@angular/router'
import { RESPONSE_STATUS, TYPECONFIG } from 'src/app/constants/CONSTANTS'
import { SystemtConfig, ConfigSwitchboard } from 'src/app/models/systemtConfigObject'
import { COMMONS } from 'src/app/commons/commons'
import { Router } from '@angular/router'
@Component({
	selector: 'app-switchboard-setting',
	templateUrl: './switchboard-setting.component.html',
	styleUrls: ['./switchboard-setting.component.css'],
})
export class SwitchboardSettingComponent implements OnInit {
	model: SystemtConfig = new SystemtConfig()
	submitted: boolean = false
	configSwitchboard: ConfigSwitchboard = new ConfigSwitchboard()

	form: FormGroup
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
					this.model = { ...response.result.SYConfigGetByID[0] }
					this.configSwitchboard = JSON.parse(this.model.content)
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
			link: [this.configSwitchboard.link, Validators.required],
			config: [this.configSwitchboard.config, Validators.required],
			// port: [this.configEmail.port, Validators.required],
			// email: [this.configEmail.email, [Validators.required, Validators.pattern(this.resd)]], //Validators.pattern('^[a-z][a-z0-9_.]{5,32}@[a-z0-9]{2,}(.[a-z0-9]{2,4}){1,2}$')
		})
	}
	rebuilForm() {
		this.form.reset({
			title: this.model.title,
			description: this.model.description,
			link: this.configSwitchboard.link,
			config: this.configSwitchboard.config,
		})
	}
	onSave() {
		this.model.type = TYPECONFIG.CONFIG_SWITCHBOARD
		this.submitted = true
		if (this.form.invalid) {
			return
		}
		this.model.content = JSON.stringify(this.configSwitchboard)
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
