import { Component, OnInit, ViewChild } from '@angular/core'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'
import { Router } from '@angular/router'
import { SafeUrl, DomSanitizer } from '@angular/platform-browser'

import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'

import { IndexSettingService } from 'src/app/services/index-setting.service'
import { IndexWebsite } from 'src/app/models/indexSettingObject'
import { COMMONS } from 'src/app/commons/commons'
import { saveAs as importedSaveAs } from 'file-saver'
import { UploadFileService } from 'src/app/services/uploadfiles.service'
declare var $: any


@Component({
	selector: 'app-index-setting',
	templateUrl: './index-setting.component.html',
	styleUrls: ['./index-setting.component.css'],
})
export class IndexSettingComponent implements OnInit {
	constructor(private fileService: UploadFileService, private _service: IndexSettingService, private _toastr: ToastrService, private _fb: FormBuilder, private _router: Router, private sanitizer: DomSanitizer) {
		this.ltsIndexSettingWebsite = []
		this.lstInsertBanner = []
	}

	model: any = {}

	modelWebsite: any = new IndexWebsite()
	ltsIndexSettingWebsite: Array<IndexWebsite>
	lstInsertBanner: any[]
	submitted: boolean = false
	submittedWebsite: boolean = false

	// file

	BannerImg: any
	bannerUrl: any

	// form
	form: FormGroup
	formWebsite: FormGroup

	ngOnInit() {
		// get model
		this.buildForm()
		this.buildFormWebsite()
		this.getListWebsite()

		this._service.GetInfo({}).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				this.model = res.result.model
				this.bannerUrl = this.model.bannerUrl

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
			metaTitle: [this.model.metaTitle, [Validators.required]],
			metaDescription: [this.model.metaDescription, [Validators.required]],
			systemTitle: [this.model.systemTitle, Validators.required],
			bannerLink: [this.model.bannerLink, Validators.required],
			organization: [this.model.organization, Validators.required],
			unit: [this.model.unit, Validators.required],
			phone: [this.model.phone, [Validators.required, Validators.pattern('[- +()0-9]+')]],
			email: [this.model.email, [Validators.required, Validators.email]],
			address: [this.model.address, Validators.required],

		})
	}

	get fWebsite() {
		return this.formWebsite.controls
	}

	buildFormWebsite() {
		this.formWebsite = this._fb.group({
			nameWebsite: [this.modelWebsite.nameWebsite, Validators.required],
			urlWebsite: [this.modelWebsite.urlWebsite, [Validators.required]],
		})
	}

	rebuilFormWebsite() {
		this.submittedWebsite = false
		this.formWebsite.reset({
			nameWebsite: '',
			urlWebsite: ''
		})
	}

	redirect() {
		window.history.back()
	}

	getListWebsite() {
		this._service.SYIndexWebsiteGetAll().subscribe(res => {
			if (res.success == RESPONSE_STATUS.success) {
				this.ltsIndexSettingWebsite = res.result == null ? [] : res.result
			}
		}, err => {
			console.log(err)
		})
	}

	onSave(isPreView: boolean = false) {
		this.submitted = true
		this.model.phone = this.model.phone.trim()
		this.model.email = this.model.email.trim()
		this.model.address = this.model.address.trim()

		this.model.systemTitle = this.model.systemTitle == null ? '' : this.model.systemTitle.trim()
		this.model.bannerLink = this.model.bannerLink == null ? '' : this.model.bannerLink.trim()
		this.model.organization = this.model.organization == null ? '' : this.model.organization.trim()
		this.model.unit = this.model.unit == null ? '' : this.model.unit.trim()

		if (this.form.invalid) {
			return
		}

		let obj = {
			model: this.model,
			fileBanner: this.BannerImg
		}
		this._service.Update(obj).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				if (isPreView) {
					window.open('/cong-bo/xem-truoc/trang-chu')
					return
				}
				this._toastr.success(COMMONS.UPDATE_SUCCESS)

			} else {
				this._toastr.error(res.message)
			}
		}),
			(error) => {
				console.log(error)
			}
	}


	@ViewChild('fileWebsite', { static: false }) fileWebsite: any
	file: any

	onChooseImageWebsite(event: any) {
		var file = event.target.files[0]
		if (!['image/jpeg', 'image/png'].includes(file.type)) {
			this._toastr.error('Chỉ chọn tệp tin ảnh')
			event.target.value = null
			return
		}
		// banner.fileAttach = this.sanitizer.bypassSecurityTrustUrl(window.URL.createObjectURL(event.target.files[0]))
		this.file = file
		this.modelWebsite.fileName = file.name
	}

	onSaveWebsite() {
		this.submittedWebsite = true
		this.modelWebsite.nameWebsite = this.modelWebsite.nameWebsite.trim()
		this.modelWebsite.urlWebsite = this.modelWebsite.urlWebsite.trim()
		this.modelWebsite.indexSystemId = this.model.id

		if (this.formWebsite.invalid) {
			return
		}

		if (!this.modelWebsite.id) {
			if (this.file == null) {
				this._toastr.error("Vui lòng chọn ảnh cho liên kết website")
				return
			}
			this._service.IndexWebsiteInsert({ model: this.modelWebsite, file: this.file }).subscribe((res) => {
				// console.log(res)
				if (res.success == RESPONSE_STATUS.success) {
					this.fileWebsite.nativeElement.value = ''
					this.modelWebsite = {}
					this.rebuilFormWebsite()
					this.file = null
					this._toastr.success(COMMONS.ADD_SUCCESS)
					this.getListWebsite()
				} else {
					this._toastr.error(COMMONS.ADD_FAILED)
				}
			})
		} else {
			this._service.IndexWebsiteUpdate({ model: this.modelWebsite, file: this.file }).subscribe((res) => {
				if (res.success == RESPONSE_STATUS.success) {
					this.fileWebsite.nativeElement.value = ''
					this.modelWebsite = {}
					this.file = null
					this.rebuilFormWebsite()
					this._toastr.success(COMMONS.UPDATE_SUCCESS)
					this.getListWebsite()
				} else {
					this._toastr.error(COMMONS.UPDATE_FAILED)
				}
			}, err => {
				console.log(err)
			})
		}
	}

	IndexWebsiteGetById(data: any) {
		this.modelWebsite = { ...data }
	}

	onDeleteWebsite(id: number) {
		this._service.IndexWebsiteDelete({ Id: id }).subscribe(res => {
			if (res.success == RESPONSE_STATUS.success) {
				this.getListWebsite()
			} else {
				this._toastr.error(res.message)
			}
		})
	}

	DownloadFile(file: any) {
		var request = {
			Path: file.filePathBase,
			Name: file.fileName,
		}
		this.fileService.downloadFile(request).subscribe(
			(response) => {
				var blob = new Blob([response], { type: response.type })
				importedSaveAs(blob, file.name)
			},
			(error) => {
				this._toastr.error('Không tìm thấy file trên hệ thống')
			}
		)
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

	// lts banner


}
