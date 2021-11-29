import { AfterContentInit, Component, OnInit } from '@angular/core'
import { FormGroup, FormBuilder, Validators, FormGroupName } from '@angular/forms'
import { SystemconfigService } from '../../../../services/systemconfig.service'
import { ToastrService } from 'ngx-toastr'
import { ActivatedRoute } from '@angular/router'
import { RESPONSE_STATUS, TYPECONFIG } from 'src/app/constants/CONSTANTS'
import { SystemtConfig, ConfigApplication } from 'src/app/models/systemtConfigObject'
import { COMMONS } from 'src/app/commons/commons'
import { Router } from '@angular/router'

declare var $: any

@Component({
	selector: 'app-application-setting',
	templateUrl: './application-setting.component.html',
	styleUrls: ['./application-setting.component.css'],
})
export class ApplicationSettingComponent implements OnInit, AfterContentInit {
	model: SystemtConfig = new SystemtConfig()
	submitted: boolean = false
	configApplication: ConfigApplication = new ConfigApplication()

	form: FormGroup
	constructor(private _service: SystemconfigService, private _toastr: ToastrService, private _fb: FormBuilder, private activatedRoute: ActivatedRoute, private _router: Router) {}

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
					this.model = { ...response.result.SYConfigGetByID[0] }
					this.configApplication = JSON.parse(this.model.content)
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
			urlIOS: [this.configApplication.urlIOS,[ Validators.required,Validators.pattern('(https?://)?([\\da-z.-]+)\\.([a-z.]{2,6})[/\\w .-]*/?')]],
			urlAndroid: [this.configApplication.urlAndroid, [Validators.required,Validators.pattern('(https?://)?([\\da-z.-]+)\\.([a-z.]{2,6})[/\\w .-]*/?')]]
		})
	}
	rebuilForm() {
		this.form.reset({
			title: this.model.title,
			description: this.model.description,
			urlIOS: this.configApplication.urlIOS,
			urlAndroid: this.configApplication.urlAndroid
		})
	}
	onSave() {
		this.model.type = TYPECONFIG.APPLICATION
		this.submitted = true
		if (this.form.invalid) {
			return
		}
		this.model.content = JSON.stringify(this.configApplication)
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
