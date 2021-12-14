import { Component, OnInit, Pipe, Directive, AfterContentInit } from '@angular/core'
import { FormGroup, FormBuilder, Validators } from '@angular/forms'
import { SystemconfigService } from '../../../../services/systemconfig.service'
import { ToastrService } from 'ngx-toastr'
import { ActivatedRoute, Router } from '@angular/router'
import { RESPONSE_STATUS, TYPECONFIG } from 'src/app/constants/CONSTANTS'
import { SystemtConfig, ConfigSync } from 'src/app/models/systemtConfigObject'
import { COMMONS } from 'src/app/commons/commons'

declare var $: any
@Component({
	selector: 'app-sync-setting',
	templateUrl: './sync-setting.component.html',
	styleUrls: ['./sync-setting.component.css'],
})
export class SyncSettingComponent implements OnInit, AfterContentInit {
	model: SystemtConfig = new SystemtConfig()
	configSync: ConfigSync = new ConfigSync()
	submitted: boolean = false
	form: FormGroup

	lstMethod: any = [
		{ value: 1, text: 'GET' },
		{ value: 2, text: 'POST' },
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

	ngAfterContentInit() {
		setTimeout(() => {
			$('#target').focus()
		}, 200)
	}

	onCancel() {
		this.buildForm()
		this._service.syConfigGetById({ Id: this.model.id }).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result.SYConfigGetByID.length > 0) {
					this.model = response.result.SYConfigGetByID[0]
					if (this.model.content != null && this.model.content != '') {
						this.configSync = JSON.parse(this.model.content)
					} else {
						this.configSync = new ConfigSync()
					}
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
			cskhLinkApi: [this.configSync.cskhLinkApi, Validators.required],
			cskhMethod: [this.configSync.cskhMethod, Validators.required],
			tdLinkApi: [this.configSync.tdLinkApi, Validators.required],
			tdMethod: [this.configSync.tdMethod, Validators.required],
			cttLinkApi: [this.configSync.cttLinkApi, Validators.required],
			cttMethod: [this.configSync.cttMethod, Validators.required],
			httnLinkApi: [this.configSync.httnLinkApi, Validators.required],
			httnMethod: [this.configSync.httnMethod, Validators.required],
			smsLinkApi: [this.configSync.smsLinkApi, Validators.required],
			smsMethod: [this.configSync.smsMethod, Validators.required],
		})
	}
	rebuilForm() {
		this.form.reset({
			title: this.model.title,
			description: this.model.description,
			cskhLinkApi: this.configSync.cskhLinkApi,
			cskhMethod: this.configSync.cskhMethod,
			tdLinkApi: this.configSync.tdLinkApi,
			tdMethod: this.configSync.tdMethod,
			cttLinkApi: this.configSync.cttLinkApi,
			cttMethod: this.configSync.cttMethod,
			smsLinkApi: this.configSync.smsLinkApi,
			smsMethod: this.configSync.smsMethod,
		})
	}
	onSave() {
		this.model.type = TYPECONFIG.SYNC_CONFIG
		this.submitted = true
		if (this.form.invalid) {
			return
		}
		this.model.content = JSON.stringify(this.configSync)

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
