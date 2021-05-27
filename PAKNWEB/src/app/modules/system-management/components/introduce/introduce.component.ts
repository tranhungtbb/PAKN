import { Component, OnInit, ViewChild } from '@angular/core'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'
import { Router } from '@angular/router'
import { MESSAGE_COMMON, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'

import { IntroduceService } from 'src/app/services/introduce.service'
import { IntroduceObjet, IntroduceFunction, IntroduceUnit } from 'src/app/models/IntroductObject'

@Component({
	selector: 'app-introduce',
	templateUrl: './introduce.component.html',
	styleUrls: ['./introduce.component.css'],
})
export class IntroduceComponent implements OnInit {
	constructor(private _service: IntroduceService, private _toastr: ToastrService, private _fb: FormBuilder, private _router: Router) {}

	model: any = new IntroduceObjet()
	PageSize: number = 10
	PageIndex: number = 1
	totalRecords: number = 0
	ltsIntroductUnit: Array<IntroduceUnit>
	lstIntroduceFunction: Array<IntroduceFunction>
	submitted: boolean
	// form
	form: FormGroup
	// chid
	@ViewChild('table', { static: false }) table: any

	ngOnInit() {
		// get model
		this.buildForm()

		this._service.GetInfo({}).subscribe((res) => {
			debugger
			if (res.success == RESPONSE_STATUS.success) {
				this.model = res.result.model
				this.lstIntroduceFunction = res.result.lstIntroduceFunction
			}
		}),
			(error) => {
				console.log(error)
				alert(error)
			}
	}

	// validate

	get f() {
		return this.form.controls
	}

	buildForm() {
		this.form = this._fb.group({
			title: [this.model.Title, Validators.required],
			summary: [this.model.Summary, Validators.required],
			descriptionUnit: [this.model.DescriptionUnit, Validators.required],
			descriptionFunction: [this.model.DescriptionFunction, Validators.required],
		})
	}

	rebuilForm() {
		this.form.reset({
			title: this.model.Title,
			summary: this.model.Summary,
			descriptionUnit: this.model.DescriptionUnit,
			descriptionFunction: this.model.DescriptionFunction,
		})
	}

	getListUnit() {
		let obj = {
			IntroduceId: this.model.id,
			PageSize: this.PageSize,
			PageIndex: this.PageIndex,
		}
		this._service.IntroduceUnitGetListOnPage(obj).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				if (res.result.SYIntroduceUnitGetOnPage.length > 0) {
					this.ltsIntroductUnit = res.result.SYIntroduceUnitGetOnPage
					this.PageIndex = res.result.PageIndex
					this.PageSize = res.result.PageSize
					this.totalRecords = res.result.TotalCount
				} else {
					this.ltsIntroductUnit = []
					this.PageIndex = 1
					this.PageSize = 10
					this.totalRecords = 0
					return
				}
			}
			this.ltsIntroductUnit = []
			this.PageIndex = 1
			this.PageSize = 10
			this.totalRecords = 0
			return
		}),
			(error) => {
				console.log(error)
				alert(error)
			}
	}
	redirectList() {}
	onSave() {}
}
