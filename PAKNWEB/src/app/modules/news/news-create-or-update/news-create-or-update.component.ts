import { Component, OnInit, ViewChild } from '@angular/core'
import { DomSanitizer } from '@angular/platform-browser'
import * as ClassicEditor from '@ckeditor/ckeditor5-build-classic'
//import * as ClassicEditor from '../../../../ckeditor'

//import Base64UploadAdapter from '@ckeditor/ckeditor5-upload/src/adapters/base64uploadadapter'

import { FormGroup, FormBuilder, Validators } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'

import { Router, ActivatedRoute } from '@angular/router'
import { RESPONSE_STATUS } from '../../../constants/CONSTANTS'
import { NewsService } from '../../../services/news.service'
import { CatalogService } from '../../../services/catalog.service'
import { NotificationService } from '../../../services/notification.service'

import { NewsRelateModalComponent } from '../news-relate-modal/news-relate-modal.component'

import { COMMONS } from '../../../commons/commons'
import { AppSettings } from '../../../constants/app-setting'
import { NewsModel } from '../../../models/NewsObject'
import { from } from 'rxjs'

declare var $: any

// ClassicEditor.create(document.querySelector('#editor'), {
// 	extraPlugins: [MyUploadAdapterPlugin],
// })
// 	.then((editor) => {
// 		console.log('Editor was initialized', editor)
// 	})
// 	.catch((error) => {
// 		console.error(error)
// 	})

// function MyUploadAdapterPlugin(editor) {
// 	console.log(editor)
// 	editor.plugins.get('FileRepository').createUploadAdapter = function (loader) {
// 		console.log(loader)
// 	}
// }

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
		private activatedRoute: ActivatedRoute,
		private sanitizer: DomSanitizer,
		private notificationService: NotificationService
	) {}
	allowImageExtend = ['image/jpeg', 'image/png']
	public Editor = ClassicEditor
	public ckConfig = {
		placeholder: 'Nhập...',
	}

	model: NewsModel = new NewsModel()
	newsForm: FormGroup
	listNewsTypes: any[]
	postTypes: any[] = [
		{ text: 'Bài viết thường', value: '0' },
		{ text: 'nổi bật', value: '1' },
	]

	postTypeSelected: any[] = []

	newsRelatesSelected: any[] = []
	categoriesSelected: any[]
	avatarUrl: any = 'assets/dist/images/no.jpg'

	ngOnInit() {
		this.newsForm = this.formBuilder.group({
			title: [this.model.title, [Validators.required, Validators.maxLength(500)]],
			summary: [this.model.summary],
			contents: [this.model.contents],
			newsType: [this.model.newsType],
			postType: [this.model.postType],
			pushNotify: [''],
		})

		this.activatedRoute.params.subscribe((params) => {
			if (params['id']) {
				this.newsService.getById({ id: params['id'] }).subscribe((res) => {
					if (res.success != 'OK') {
						return
					}
					this.model = res.result.NENewsGetByID[0]
					this.postTypeSelected = this.model.postType.trim().split(',')
					//lay danh sach bai viet lien quan
					this.getNewsRelatesInfo()

					//get current avatar
					if (this.model.imagePath != null && this.model.imagePath.trim() != '') {
						this.newsService.getAvatars([this.model.id]).subscribe((res) => {
							if (res) {
								let objectURL = 'data:image/jpeg;base64,' + res[0].byteImage
								this.avatarUrl = this.sanitizer.bypassSecurityTrustUrl(objectURL)
							}
						})
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
		if (this.model.newsRelateIds != null && this.model.newsRelateIds != '') {
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

					//get avatar
					this.getNewsRelateAvatar()
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
		if (this.postTypeSelected.length > 0) this.model.postType = this.postTypeSelected.toString()
		//return
		this.model.isPublished = published
		if (published) this.model.status = 1
		else this.model.status = 2
		if (this.model.id && this.model.id > 0) {
			this.newsService.update(this.model).subscribe((res) => {
				if (res.success != 'OK') {
					this.toast.error(COMMONS.UPDATE_FAILED)
					return
				}
				// insert vào Notification
				this.insertNotification(false)
				this.toast.success(COMMONS.UPDATE_SUCCESS)
				this.router.navigate(['/quan-tri/tin-tuc'])
			})
		} else {
			this.newsService.create(this.model).subscribe((res) => {
				if (res.success != 'OK') {
					let errorMsg = COMMONS.ADD_FAILED
					this.toast.error(errorMsg)
					return
				}
				this.model.id = res.result
				this.insertNotification(true)

				this.toast.success(COMMONS.ADD_SUCCESS)
				this.router.navigate(['/quan-tri/tin-tuc'])
			})
		}
	}
	onChangePostType(id: any, selected: boolean) {
		if (selected) {
			if (!this.postTypeSelected.includes(id)) this.postTypeSelected.push(id)
		} else {
			this.postTypeSelected.splice(this.postTypeSelected.indexOf(id), 1)
		}
	}

	onModalNewsRelate() {
		this.child_NewsRelate.newsCreateOrUpdateComponent = this
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

					//get avatar
					this.getNewsRelateAvatar()
				})
		} else {
			this.newsRelatesSelected = []
		}
	}

	removeRelate(id: number) {
		let index = this.newsRelatesSelected.indexOf(this.newsRelatesSelected.find((c) => c.id == id))
		if (index >= 0) {
			this.newsRelatesSelected.splice(index, 1)
			this.model.newsRelateIds = this.newsRelatesSelected.map((c) => c.id).toString()
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

			let avatarPath = this.model.imagePath.split('/')
			this.newsService.getAvatar(avatarPath[avatarPath.length - 1]).subscribe((res) => {
				if (res) {
					let objectURL = 'data:image/jpeg;base64,' + res
					this.avatarUrl = this.sanitizer.bypassSecurityTrustUrl(objectURL)
				}
			})
		})
	}

	getNewsRelateAvatar() {
		this.newsService.getAvatars(this.newsRelatesSelected.map((c) => c.id)).subscribe((res) => {
			if (res) {
				for (let img of res) {
					let item = this.newsRelatesSelected.find((c) => c.id == img.id)
					let objectURL = 'data:image/jpeg;base64,' + img.byteImage
					item.imageBin = this.sanitizer.bypassSecurityTrustUrl(objectURL)
				}
			}
		})
	}

	public onReady(editor) {
		editor.ui.getEditableElement().parentElement.insertBefore(editor.ui.view.toolbar.element, editor.ui.getEditableElement())
	}

	insertNotification(isCreate: boolean) {
		if (this.model.status == 1) {
			var obj = {
				id: this.model.id,
				title: this.model.title,
				isCreateNews: isCreate,
			}
			if (this.model.isNotification == true) {
				this.notificationService.insertNotificationTypeNews(obj).subscribe((res) => {
					if ((res.success = RESPONSE_STATUS.success)) {
						return
					}
				})
			}
			return
		}
		return
	}
}
