import { Component, OnInit, ViewChild } from '@angular/core'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'
import { Router } from '@angular/router'
import { MESSAGE_COMMON, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'

import { IntroduceService } from 'src/app/services/introduce.service'
import { IntroduceObjet, IntroduceFunction, IntroduceUnit } from 'src/app/models/IntroductObject'
import { error } from 'jquery'
import { COMMONS } from 'src/app/commons/commons'
declare var $: any
@Component({
	selector: 'app-introduce',
	templateUrl: './introduce.component.html',
	styleUrls: ['./introduce.component.css'],
})
export class IntroduceComponent implements OnInit {
	constructor(private _service: IntroduceService, private _toastr: ToastrService, private _fb: FormBuilder, private _router: Router) {
		this.lstIntroduceFunction = []
		for (var i = 0; i < 6; i++) {
			this.lstIntroduceFunction.push(new IntroduceFunction())
		}
	}

	model: any = new IntroduceObjet()
	PageSize: number = 10
	PageIndex: number = 1
	totalRecords: number = 0
	modelUnit: any = new IntroduceUnit()
	ltsIntroductUnit: Array<IntroduceUnit>
	lstIntroduceFunction: Array<IntroduceFunction>
	submitted: boolean = false
	submittedUnit: boolean = false
	title: string = 'Thêm mới đơn vị'
	idDeleteUnit: number

	// file

	BannerImg: any

	// form
	form: FormGroup
	formUnit: FormGroup
	// chid
	@ViewChild('table', { static: false }) table: any

	ngOnInit() {
		// get model
		this.buildForm()
		this.buildFormUnit()

		this._service.GetInfo({}).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				this.model = res.result.model
				this.lstIntroduceFunction = res.result.lstIntroduceFunction
				this.getListUnit()
			}
		}),
			(error) => {
				console.log(error)
				alert(error)
			}
	}

	// validate introduce

	get f() {
		return this.form.controls
	}

	buildForm() {
		this.form = this._fb.group({
			title: [this.model.title, Validators.required],
			summary: [this.model.summary, Validators.required],
			descriptionUnit: [this.model.descriptionUnit, Validators.required],
			descriptionFunction: [this.model.descriptionFunction, Validators.required],
			////
			titleFunc0: [this.lstIntroduceFunction[0].title, Validators.required],
			contentFunc0: [this.lstIntroduceFunction[0].content, Validators.required],
			titleFunc1: [this.lstIntroduceFunction[1].title, Validators.required],
			contentFunc1: [this.lstIntroduceFunction[1].content, Validators.required],
			titleFunc2: [this.lstIntroduceFunction[2].title, Validators.required],
			contentFunc2: [this.lstIntroduceFunction[2].content, Validators.required],
			titleFunc3: [this.lstIntroduceFunction[3].title, Validators.required],
			contentFunc3: [this.lstIntroduceFunction[3].content, Validators.required],
			titleFunc4: [this.lstIntroduceFunction[4].title, Validators.required],
			contentFunc4: [this.lstIntroduceFunction[4].content, Validators.required],
			titleFunc5: [this.lstIntroduceFunction[5].title, Validators.required],
			contentFunc5: [this.lstIntroduceFunction[5].content, Validators.required],
		})
	}

	// rebuilForm() {
	// 	this.form.reset({
	// 		title: this.model.title,
	// 		summary: this.model.summary,
	// 		descriptionUnit: this.model.descriptionUnit,
	// 		descriptionFunction: this.model.descriptionFunction,
	// 		// form hơi dài, aem thông cảm ko nghĩ ra giải pháp, có 6 x 2 cái thôi mà

	// 		titleFunc0: this.lstIntroduceFunction[0].title,
	// 		contentFunc0: this.lstIntroduceFunction[0].content,
	// 		titleFunc1: this.lstIntroduceFunction[1].title,
	// 		contentFunc1: this.lstIntroduceFunction[1].content,
	// 		titleFunc2: this.lstIntroduceFunction[2].title,
	// 		contentFunc2: this.lstIntroduceFunction[2].content,
	// 		titleFunc3: this.lstIntroduceFunction[3].title,
	// 		contentFunc3: this.lstIntroduceFunction[3].content,
	// 		titleFunc4: this.lstIntroduceFunction[4].title,
	// 		contentFunc4: this.lstIntroduceFunction[4].content,
	// 		titleFunc5: this.lstIntroduceFunction[5].title,
	// 		contentFunc5: this.lstIntroduceFunction[5].content,
	// 	})
	// }

	// validate introduce Unit

	get fUnit() {
		return this.formUnit.controls
	}

	buildFormUnit() {
		this.formUnit = this._fb.group({
			title: [this.modelUnit.title, Validators.required],
			description: [this.modelUnit.description, Validators.required],
			infomation: [this.modelUnit.infomation, Validators.required],
			index: [this.modelUnit.index],
		})
	}

	rebuilFormUnit() {
		this.formUnit.reset({
			title: this.modelUnit.Title,
			description: this.modelUnit.Description,
			infomation: this.modelUnit.Infomation,
			index: this.modelUnit.Index,
		})
	}

	onPageChange(event: any) {
		this.PageSize = event.rows
		this.PageIndex = event.first / event.rows + 1
		this.getListUnit()
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
			} else {
				this.ltsIntroductUnit = []
				this.PageIndex = 1
				this.PageSize = 10
				this.totalRecords = 0
				return
			}
		}),
			(error) => {
				console.log(error)
				alert(error)
			}
	}
	redirect() {
		window.history.back()
	}
	onSave() {
		let obj = {
			model: this.model,
			fileBanner: this.BannerImg,
			lstIntroduceFunction: this.lstIntroduceFunction,
		}
		this._service.Update(obj).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				this._toastr.success(COMMONS.UPDATE_SUCCESS)
				window.history.back()
			} else {
				this._toastr.error(res.message)
			}
		}),
			(error) => {
				console.log(error)
			}
	}

	onSaveUnit() {
		this.submittedUnit = true
		this.modelUnit.title = this.modelUnit.title.trim()
		this.modelUnit.description = this.modelUnit.description.trim()
		this.modelUnit.infomation = this.modelUnit.infomation.trim()

		if (this.formUnit.invalid) {
			return
		}
		if (this.modelUnit.id == 0 || this.modelUnit.id == null) {
			this.modelUnit.introduceId = this.model.id
			this._service.IntroduceUnitInsert(this.modelUnit).subscribe((response) => {
				if (response.success == RESPONSE_STATUS.success) {
					$('#modal-create-update-introduce-unit').modal('hide')
					this._toastr.success(MESSAGE_COMMON.ADD_SUCCESS)
					this.getListUnit()
				} else {
					let res = isNaN(response.result) == true ? 0 : response.result
					if (res == -1) {
						this._toastr.error(MESSAGE_COMMON.EXISTED_NAME)
						return
					} else {
						this._toastr.error(response.message)
						return
					}
				}
			}),
				(error) => {
					console.error(error)
					alert(error)
				}
		} else {
			this._service.IntroduceUnitUpdate(this.modelUnit).subscribe((response) => {
				if (response.success == RESPONSE_STATUS.success) {
					$('#modal-create-update-introduce-unit').modal('hide')
					this._toastr.success(MESSAGE_COMMON.UPDATE_SUCCESS)
					this.getListUnit()
				} else {
					let res = isNaN(response.result) == true ? 0 : response.result
					if (res == -1) {
						this._toastr.error(MESSAGE_COMMON.EXISTED_NAME)
						return
					} else {
						this._toastr.error(response.message)
						return
					}
				}
			}),
				(error) => {
					console.error(error)
					alert(error)
				}
		}
	}

	preCreate() {
		this.title = 'Thêm mới đơn vị'
		this.modelUnit = new IntroduceUnit()
		$('#modal-create-update-introduce-unit').modal('show')
	}

	preUpdate(model: any) {
		this.title = 'Chỉnh sửa đơn vị'
		this.modelUnit = { ...model }
		$('#modal-create-update-introduce-unit').modal('show')
	}

	preDelete(id: number) {
		this.idDeleteUnit = id
		$('#modalConfirmDelete').modal('show')
	}

	onDelete() {
		let request = {
			Id: this.idDeleteUnit,
		}
		this._service.IntroduceUnitDelete(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result > 0) {
					this._toastr.success(MESSAGE_COMMON.DELETE_SUCCESS)
				} else {
					this._toastr.error(MESSAGE_COMMON.DELETE_FAILED)
				}
				$('#modalConfirmDelete').modal('hide')
				this.getListUnit()
			} else {
				this._toastr.error(response.message)
			}
		}),
			(error) => {
				console.error(error)
			}
	}

	ChooseBanner() {
		$('#bannerImg').click()
	}
	ChangeBanner(event: any) {
		var file = event.target.files[0]
		if (!['image/jpeg', 'image/png'].includes(file.type)) {
			this._toastr.error('Chỉ chọn tệp tin ảnh')
			event.target.value = null
			return
		}

		let output: any = $('#bannerIntroduce')
		output.attr('src', URL.createObjectURL(file))
		output.onload = function () {
			URL.revokeObjectURL(output.src) // free memory
		}

		this.BannerImg = event.target.files[0]
	}
}
