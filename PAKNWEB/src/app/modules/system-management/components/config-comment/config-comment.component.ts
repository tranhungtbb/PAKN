import { AfterContentInit, Component, OnInit } from '@angular/core'
import { FormGroup, FormBuilder, Validators, FormGroupName } from '@angular/forms'
import { SystemconfigService } from '../../../../services/systemconfig.service'
import { ToastrService } from 'ngx-toastr'
import { ActivatedRoute } from '@angular/router'
import { RESPONSE_STATUS, TYPECONFIG } from 'src/app/constants/CONSTANTS'
import { SystemtConfig, ConfigComment } from 'src/app/models/systemtConfigObject'
import { COMMONS } from 'src/app/commons/commons'
import { Router } from '@angular/router'

declare var $: any

@Component({
	selector: 'app-config-comment',
	templateUrl: './config-comment.component.html',
	styleUrls: ['./config-comment.component.css'],
})
export class ConfigCommentComponent implements OnInit, AfterContentInit {
	model: SystemtConfig = new SystemtConfig()
	submitted: boolean = false
	configComment: ConfigComment = new ConfigComment()

	form: FormGroup
	constructor(private _service: SystemconfigService, private _toastr: ToastrService, private _fb: FormBuilder, private activatedRoute: ActivatedRoute, private _router: Router) { }

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
			status: [this.model.content, [Validators.required]],
		})
	}
	rebuilForm() {
		this.form.reset({
			title: this.model.title,
			description: this.model.description,
			status: this.model.content
		})
	}
	onSave() {
		this.model.type = TYPECONFIG.CONFIG_COMMENT
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
