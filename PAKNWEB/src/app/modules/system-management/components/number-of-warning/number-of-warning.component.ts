import { Component, OnInit } from '@angular/core'
import { FormGroup, FormBuilder, Validators } from '@angular/forms'
import { SystemconfigService } from '../../../../services/systemconfig.service'
import { ToastrService } from 'ngx-toastr'
import { ActivatedRoute } from '@angular/router'
import { RESPONSE_STATUS, TYPECONFIG } from 'src/app/constants/CONSTANTS'
import { SystemtConfig, GeneralSetting } from 'src/app/models/systemtConfigObject'
import { COMMONS } from 'src/app/commons/commons'
import { Router } from '@angular/router'
@Component({
	selector: 'app-number-of-warning',
	templateUrl: './number-of-warning.component.html',
	styleUrls: ['./number-of-warning.component.css'],
})
export class NummerOfWarningSettingComponent implements OnInit {
	model: SystemtConfig = new SystemtConfig()
	generalSetting: GeneralSetting = new GeneralSetting()
	submitted: boolean = false

	form: FormGroup
	constructor(private _service: SystemconfigService, private _router: Router, private _toastr: ToastrService, private _fb: FormBuilder, private activatedRoute: ActivatedRoute) { }

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
					this.generalSetting = JSON.parse(this.model.content)
					if (typeof this.generalSetting != 'object') {
						this.generalSetting = new GeneralSetting()
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
	buildForm() {
		this.form = this._fb.group({
			title: [this.model.title, Validators.required],
			description: [this.model.description, Validators.required],
			numberOfWarning: [this.generalSetting.numberOfWarning, Validators.required],
			fileQuantity: [this.generalSetting.fileQuantity, Validators.required],
			fileSize: [this.generalSetting.fileSize, Validators.required],
		})
	}
	rebuilForm() {
		this.form.reset({
			title: this.model.title,
			description: this.model.description,
			numberOfWarning: this.generalSetting.numberOfWarning,
			fileQuantity: this.generalSetting.fileQuantity,
			fileSize: this.generalSetting.fileSize,
		})
	}
	onSave() {
		this.model.type = TYPECONFIG.CONFIG_NUMBER_WARNING
		this.submitted = true
		if (this.form.invalid) {
			return
		}

		this.model.content = JSON.stringify(this.generalSetting)
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
