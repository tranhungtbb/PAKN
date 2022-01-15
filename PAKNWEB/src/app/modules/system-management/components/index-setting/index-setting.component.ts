import { Component, OnInit, ViewChild } from '@angular/core'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'
import { Router } from '@angular/router'
import { SafeUrl, DomSanitizer } from '@angular/platform-browser'

import { MESSAGE_COMMON, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { AppSettings } from 'src/app/constants/app-setting'

import { IndexSettingService } from 'src/app/services/index-setting.service'
import { IndexSettingObjet, IndexBanner, IndexWebsite } from 'src/app/models/indexSettingObject'
import { COMMONS } from 'src/app/commons/commons'
declare var $: any
@Component({
	selector: 'app-index-setting',
	templateUrl: './index-setting.component.html',
	styleUrls: ['./index-setting.component.css'],
})
export class IndexSettingComponent implements OnInit {
	constructor(private _service: IndexSettingService, private _toastr: ToastrService, private _fb: FormBuilder, private _router: Router, private sanitizer: DomSanitizer) {
		this.ltsIndexSettingWebsite = []
		this.lstIndexSettingBanner = []
		this.lstRemoveBanner = []
		this.lstInsertBanner = []
	}

	model: any = new IndexSettingObjet()

	modelWebsite: any = new IndexWebsite()
	ltsIndexSettingWebsite: Array<IndexWebsite>
	lstIndexSettingBanner: Array<IndexBanner>
	lstInsertBanner: any[]
	lstRemoveBanner: Array<IndexBanner>
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

		this._service.GetInfo({}).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				this.model = res.result.model
				this.bannerUrl = this.model.bannerUrl
				this.lstIndexSettingBanner = res.result.lstIndexSettingBanner == null ? [] : res.result.lstIndexSettingBanner
				this.ltsIndexSettingWebsite = res.result.lstSYIndexWebsite == null ? [] : res.result.lstSYIndexWebsite
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
			phone: [this.model.phone, [Validators.required, Validators.pattern('[- +()0-9]+')]],
			email: [this.model.email, [Validators.required, Validators.email]],
			address: [this.model.address, Validators.required],
			description: [this.model.description, Validators.required],
			license: [this.model.license, Validators.required],
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
		this.formWebsite.reset({
			nameWebsite: this.modelWebsite.nameWebsite,
			urlWebsite: this.modelWebsite.urlWebsite,
		})
	}

	redirect() {
		window.history.back()
	}

	onSave(isPreView: boolean = false) {
		this.submitted = true
		this.model.phone = this.model.phone.trim()
		this.model.email = this.model.email.trim()
		this.model.address = this.model.address.trim()
		this.model.description = this.model.description.trim()
		this.model.license = this.model.license.trim()

		if (this.form.invalid) {
			return
		}

		let obj = {
			model: this.model,
			fileBanner: this.BannerImg,
			ltsIndexWebsite: this.ltsIndexSettingWebsite,
			lstInsertBanner: this.lstInsertBanner,
			lstRemoveBanner: this.lstRemoveBanner,
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

	onSaveWebsite() {
		this.submittedWebsite = true
		this.modelWebsite.nameWebsite = this.modelWebsite.nameWebsite.trim()
		this.modelWebsite.urlWebsite = this.modelWebsite.urlWebsite.trim()
		this.modelWebsite.indexSystemId = this.model.id

		if (this.formWebsite.invalid) {
			return
		}
		if (this.ltsIndexSettingWebsite.length > 0) {
			var check = 0
			this.ltsIndexSettingWebsite.map((item) => {
				if (item.nameWebsite == this.modelWebsite.nameWebsite) {
					check += 1
					return
				}
			})
			if (check > 0) {
				this._toastr.error('Tên website đã bị trùng')
				return
			} else {
				this.ltsIndexSettingWebsite.push(this.modelWebsite)
				this.submittedWebsite = false
				this.modelWebsite = new IndexWebsite()
				this.rebuilFormWebsite()
				this._toastr.success(MESSAGE_COMMON.ADD_SUCCESS)
				return
			}
		} else {
			this.ltsIndexSettingWebsite.push(this.modelWebsite)
			this.submittedWebsite = false
			this.modelWebsite = new IndexWebsite()
			this.rebuilFormWebsite()
			this._toastr.success(MESSAGE_COMMON.ADD_SUCCESS)
		}
	}

	onDeleteWebsite(nameWebsite: any) {
		if (nameWebsite == undefined) return
		if (this.ltsIndexSettingWebsite.length > 0) {
			this.ltsIndexSettingWebsite = this.ltsIndexSettingWebsite.filter((x) => x.nameWebsite != nameWebsite)
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

	// lts banner

	preInsertBanner() {
		$('#insertBanner').click()
	}
	onInsertBanner(event: any) {
		var file = event.target.files[0]
		if (!['image/jpeg', 'image/png'].includes(file.type)) {
			this._toastr.error('Chỉ chọn tệp tin ảnh')
			event.target.value = null
			return
		}

		for (let item of this.lstIndexSettingBanner) {
			if (item.name === file.name) {
				this._toastr.error('Không phép đẩy cùng 1 file lên hệ thống')
				return
			}
		}

		let banner = new IndexBanner()

		banner.fileAttach = this.sanitizer.bypassSecurityTrustUrl(window.URL.createObjectURL(event.target.files[0]))

		this.lstIndexSettingBanner.push(banner)

		this.lstInsertBanner.push(file)
	}

	onDeleteBanner(args) {
		const index = this.lstIndexSettingBanner.indexOf(args)
		const file = this.lstIndexSettingBanner[index]
		this.lstRemoveBanner.push(file)
		this.lstIndexSettingBanner.splice(index, 1)
	}
}
