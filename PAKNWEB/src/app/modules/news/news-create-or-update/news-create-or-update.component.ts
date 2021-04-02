import { Component, OnInit, ViewChild } from '@angular/core'
import * as ClassicEditor from '@ckeditor/ckeditor5-build-classic'
import { FormGroup, FormBuilder, Validators } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'

import { Router, ActivatedRoute } from '@angular/router'

import { NewsService } from '../../../services/news.service'
import { CatalogService } from '../../../services/catalog.service'

import { NewsRelateModalComponent } from '../news-relate-modal/news-relate-modal.component'

import { COMMONS } from '../../../commons/commons'
import { AppSettings } from '../../../constants/app-setting'
import { NewsModel } from '../../../models/NewsObject'

@Component({
	selector: 'app-news-create-or-update',
	templateUrl: './news-create-or-update.component.html',
	styleUrls: ['./news-create-or-update.component.css'],
})
export class NewsCreateOrUpdateComponent implements OnInit {
	@ViewChild(NewsRelateModalComponent, { static: false }) child_NewsRelate: NewsRelateModalComponent
	constructor(
		private toast: ToastrService,
		private formBuilder: FormBuilder,
		private newsService: NewsService,
		private catalogService: CatalogService,
		private router: Router,
		private activatedRoute: ActivatedRoute
	) {}
	allowImageExtend = ['image/jpeg', 'image/png']
	public Editor = ClassicEditor
	model: NewsModel = new NewsModel()
	newsForm: FormGroup
	listNewsTypes: any[]
	postTypes: any[] = [
		{ text: 'thường', value: true, checked: true },
		{ text: 'nổi bật', value: false, checked: false },
	]
	newsRelatesSelected: any[] = []
	categoriesSelected: any[]
	avatarUrl: string = 'assets/dist/images/no.jpg'

	ngOnInit() {
		this.newsForm = this.formBuilder.group({
			title: ['', [Validators.required]],
			summary: ['', [Validators.required]],
			contents: [''],
			newsType: [''],
			postType: [''],
			pushNotify: [''],
		})

		this.activatedRoute.params.subscribe((params) => {
			if (params['id']) {
				this.newsService.getById({ id: params['id'] }).subscribe((res) => {
					if (res.success != 'OK') {
						return
					}
					this.model = res.result.NENewsGetByID[0]
					this.getNewsRelatesInfo()
					if (this.model.imagePath == null || this.model.imagePath.trim() == '') {
						this.avatarUrl = 'assets/dist/images/no.jpg'
					}
				})
			}
		})

		//get all news type
		this.catalogService
			.newsTypeGetList({
				pageSize: 10000,
				pageIndex: 1,
			})
			.subscribe((res) => {
				if (res.success != 'OK') {
					return
				}
				this.listNewsTypes = res.result.CANewsTypeGetAllOnPage
			})
	}

	getNewsRelatesInfo() {
		if (this.model.newsRelateIds != null && this.model.newsRelateIds.length > 0) {
			this.newsService
				.getAllPagedList({
					pageIndex: 1,
					pageSize: 100,
					newsIds: this.model.newsRelateIds,
				})
				.subscribe((res) => {
					if (res.success != 'OK') {
						return
					}
					this.newsRelatesSelected = res.result.NENewsGetAllOnPage
				})
		}
	}

	get f() {
		return this.newsForm.controls
	}

	submitted = false
	onSave(event, published = false) {
		this.submitted = true
		if (this.newsForm.invalid) {
			return
		}
		// if(this.model.imagePath == null || this.model.imagePath == ""){
		// 	this.toast.error()
		// 	return
		// }
		// if (this.model.newsRelateIds != null && this.model.newsRelateIds.length == 0) {
		// 	this.model.newsRelateIds = null
		// }
		this.model.isPublished = published

		if (this.model.id && this.model.id > 0) {
			this.newsService.update(this.model).subscribe((res) => {
				if (res.success != 'OK') {
					this.toast.error(COMMONS.UPDATE_FAILED)
					return
				}
				this.toast.success(COMMONS.UPDATE_SUCCESS)
				this.router.navigate(['/quan-tri/tin-tuc'])
			})
		} else {
			this.newsService.create(this.model).subscribe((res) => {
				if (res.success != 'OK') {
					this.toast.error(COMMONS.ADD_FAILED)
					return
				}
				this.toast.success(COMMONS.ADD_SUCCESS)
				this.router.navigate(['/quan-tri/tin-tuc'])
			})
		}
	}

	onModalNewsRelate() {
		this.child_NewsRelate.openModal(this.model.newsRelateIds ? this.model.newsRelateIds.split(',') : [], this.model.id)
	}
	onModalNewsRelate_Closed() {
		this.model.newsRelateIds = this.child_NewsRelate.newsSelected.toString()

		if (this.model.newsRelateIds != null && this.model.newsRelateIds != '') {
			this.newsService
				.getAllPagedList({
					pageIndex: 1,
					pageSize: 1000,
					NewsIds: this.model.newsRelateIds,
				})
				.subscribe((res) => {
					if (res.success != 'OK') return
					if (res.result.NENewsGetAllOnPage) this.newsRelatesSelected = res.result.NENewsGetAllOnPage
				})
		} else {
			this.newsRelatesSelected = []
		}
	}

	onChangeAvatar() {
		$('#avatar-image').click()
	}
	onAvatarChange(event: any) {
		console.log(event)
		var file = event.target.files[0]

		if (!this.allowImageExtend.includes(file.type)) {
			this.toast.error('Chỉ chọn tệp tin ảnh')
			return
		}

		let formData = new FormData()
		formData.append('file', file, file.name)

		this.newsService.uploadFile(formData).subscribe((res) => {
			if (res.success != 'OK') {
				this.toast.error('Xảy ra lỗi trong quá trình xử lý')
				return
			}
			this.model.imagePath = res.result.path
			this.avatarUrl = AppSettings.API_DOWNLOADFILES + '/' + this.model.imagePath
		})
	}

	public onChangeEditor({ editor }: any) {
		let data = editor.getData()
		this.model.contents = data
	}
	public onReady(editor) {
		editor.ui.getEditableElement().parentElement.insertBefore(editor.ui.view.toolbar.element, editor.ui.getEditableElement())
	}
}
