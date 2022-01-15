import { Component, OnInit, ViewChild } from '@angular/core'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'
import { Router } from '@angular/router'
import { MESSAGE_COMMON, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { AppSettings } from 'src/app/constants/app-setting'

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
	ltsIcon: any[] = []
	submitted: boolean = false
	submittedUnit: boolean = false
	title: string = 'Thêm mới đơn vị'
	idDeleteUnit: number
	idIcon: any

	// file

	BannerImg: any
	bannerUrl: any

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
				this.bannerUrl = this.model.bannerUrl
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
			title: this.modelUnit.title,
			description: this.modelUnit.description,
			infomation: this.modelUnit.infomation,
			index: this.modelUnit.index,
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
	onSave(isPreview: boolean = false) {
		let obj = {
			model: this.model,
			fileBanner: this.BannerImg,
			lstIntroduceFunction: this.lstIntroduceFunction,
			ltsIcon: this.ltsIcon,
		}
		this._service.Update(obj).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				if (isPreview) {
					window.open('/cong-bo/xem-truoc/gioi-thieu')
				}
				this._toastr.success(COMMONS.UPDATE_SUCCESS)
				// window.history.back()
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
		this.submittedUnit = false
		this.rebuilFormUnit()
		$('#modal-create-update-introduce-unit').modal('show')
	}

	preUpdate(model: any) {

		this.title = 'Chỉnh sửa đơn vị'
		this.modelUnit = { ...model }
		this.rebuilFormUnit()
		this.submittedUnit = false
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
	ChooseIcon(id: any) {
		this.idIcon = id
		$("[id='" + id + "']").click()
	}
	ChangeIcon(event: any) {
		if (!event.target.files) return
		var file = event.target.files[0]
		if (!['image/jpeg', 'image/png', 'image/svg+xml'].includes(file.type)) {
			this._toastr.error('Chỉ chọn tệp tin ảnh')
			event.target.value = null
			return
		}
		let check = false
		for (var j = 0; j < this.ltsIcon.length; j++) {
			if (this.ltsIcon[j].name === event.target.files[0].name) {
				check = true
				break
			}
		}
		for (var j = 0; j < this.lstIntroduceFunction.length; j++) {
			if (this.lstIntroduceFunction[j].icon != null && this.lstIntroduceFunction[j].icon.includes(event.target.files[0].name)) {
				check = true
				break
			}
		}
		if (check == true) {
			event.target.value = null
			this._toastr.error('Không được chọn trùng icon')
			return
		}
		file['Id'] = this.idIcon
		this.ltsIcon.push(file)
		let item = this.lstIntroduceFunction.find((x) => x.id == this.idIcon)
		let index = this.lstIntroduceFunction.indexOf(item)
		item.iconNew = event.target.files[0].name
		this.lstIntroduceFunction[index] = { ...item }

		let output: any = $('#img_icon_' + this.idIcon)
		output.attr('src', URL.createObjectURL(file))
		output.onload = function () {
			URL.revokeObjectURL(output.src) // free memory
		}
	}
}
