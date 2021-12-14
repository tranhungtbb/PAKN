import { Component, OnInit } from '@angular/core'
import { FormGroup, FormBuilder, Validators } from '@angular/forms'
import { SystemconfigService } from '../../../../services/systemconfig.service'
import { ToastrService } from 'ngx-toastr'
import { ActivatedRoute } from '@angular/router'
import { RESPONSE_STATUS, TYPECONFIG } from 'src/app/constants/CONSTANTS'
import { SystemtConfig } from 'src/app/models/systemtConfigObject'
import { COMMONS } from 'src/app/commons/commons'
import { Router } from '@angular/router'
@Component({
	selector: 'app-index-type-setting',
	templateUrl: './index-type-setting.component.html',
	styleUrls: ['./index-type-setting.component.css'],
})
export class IndexTypeSettingComponent implements OnInit {
	model: SystemtConfig = new SystemtConfig()
	submitted: boolean = false

	form: FormGroup
	constructor(private _service: SystemconfigService, private _router: Router, private _toastr: ToastrService, private _fb: FormBuilder, private activatedRoute: ActivatedRoute) {}

	listTypeIndex: any[] = [
		{ value: '1', text: 'Trang chủ mặc định' },
		{ value: '0', text: 'Trang chủ dạng lưới' },
	]
	ngOnInit() {
		this.buildForm()
		this.activatedRoute.params.subscribe((params) => {
			let id = +params['id']
			if (!isNaN(id)) {
				this.model.id = Number(id)

				this._service.syConfigGetById({ Id: this.model.id }).subscribe((response) => {
					if (response.success == RESPONSE_STATUS.success) {
						if (response.result.SYConfigGetByID.length > 0) {
							this.model = { ...response.result.SYConfigGetByID[0] }
							console.log(this.model)
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
		})
	}
	onCancel() {
		this.buildForm()
		this._service.syConfigGetById({ Id: this.model.id }).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result.SYConfigGetByID.length > 0) {
					this.model = { ...response.result.SYConfigGetByID[0] }
					console.log(this.model)
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
			content: [this.model.content, Validators.required],
		})
	}
	rebuilForm() {
		this.form.reset({
			title: this.model.title,
			description: this.model.description,
			content: this.model.content,
		})
	}
	onSave() {
		this.model.type = TYPECONFIG.TYPE_INDEX
		this.submitted = true
		if (this.form.invalid) {
			return
		}
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
