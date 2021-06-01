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
import { CONSTANTS, STATUS_HISNEWS } from 'src/app/constants/CONSTANTS'
import { COMMONS } from '../../../commons/commons'
import { AppSettings } from '../../../constants/app-setting'
import { NewsModel, HISNewsModel } from '../../../models/NewsObject'
import { from } from 'rxjs'
import { switchMap } from 'rxjs/operators'
import { Api } from 'src/app/constants/api'
import { UploadAdapter } from 'src/app/services/uploadAdapter'
import { HttpClient } from '@angular/common/http'

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
		private http: HttpClient,
		private notificationService: NotificationService
	) {}
	allowImageExtend = ['image/jpeg', 'image/png']
	public Editor = ClassicEditor
	public ckConfig = {
		simpleUpload: {
			uploadUrl: AppSettings.API_ADDRESS + Api.UploadImageNews,
		},
	}

	model: NewsModel = new NewsModel()
	newsForm: FormGroup
	listNewsTypes: any[]
	postTypes: any[] = [
		{ text: 'Bài viết thường', value: '0' },
		{ text: 'nổi bật', value: '1' },
	]
	hisPublic: boolean = false

	hisNewsModel: HISNewsModel = new HISNewsModel()

	postTypeSelected: any[] = []

	newsRelatesSelected: any[] = []
	categoriesSelected: any[]
	avatarUrl: any = 'assets/dist/images/no.jpg'

	ngOnInit() {
		this.newsForm = this.formBuilder.group({
			title: [this.model.title, [Validators.required, Validators.maxLength(500)]],
			summary: [this.model.summary],
			contents: [this.model.contents],
			newsType: [this.model.newsType, [Validators.required]],
			postType: [this.model.postType, [Validators.required]],
			imagePath: [this.model.imagePath, [Validators.required]],
			pushNotify: [''],
		})
		this.model.imagePath = 'assets/dist/images/no.jpg'

		this.activatedRoute.params.subscribe((params) => {
			if (params['id']) {
				this.newsService.getById({ id: params['id'] }).subscribe((res) => {
					if (res.success != 'OK') {
						return
					}
					this.model = res.result.NENewsGetByID[0]
					this.hisPublic = this.model.isPublished
					this.postTypeSelected = this.model.postType.trim().split(',')
					this.rebuidForm()
					//lay danh sach bai viet lien quan
					this.getNewsRelatesInfo()

					//get current avatar
					// if (this.model.imagePath != null && this.model.imagePath.trim() != '') {
					// 	this.newsService.getAvatars([this.model.id]).subscribe((res) => {
					// 		if (res) {
					// 			let objectURL = 'data:image/jpeg;base64,' + res[0].byteImage
					// 			this.avatarUrl = this.sanitizer.bypassSecurityTrustUrl(objectURL)
					// 		}
					// 	})
					// }

					// if (this.model.imagePath != null && this.model.imagePath.trim() != '') {
					// 	this.newsService.getAvatars([this.model.id]).subscribe((res) => {
					// 		if (res) {
					// 			if (!res[0].byteImage) return
					// 			let objectURL = 'data:image/jpeg;base64,' + res[0].byteImage
					// 			this.avatarUrl = this.sanitizer.bypassSecurityTrustUrl(objectURL)
					// 		}
					// 	})
					// }
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

	rebuidForm() {
		this.newsForm.reset({
			title: this.model.title,
			summary: this.model.summary,
			contents: this.model.contents,
			newsType: this.model.newsType,
			postType: this.model.postType,
			imagePath: this.model.imagePath,
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
					// .map((e) => {
					// 	e.imagePath = `${AppSettings.API_DOWNLOADFILES}/${e.imagePath}`
					// 	return e
					// })
				})
		}
	}

	get f() {
		return this.newsForm.controls
	}

	filePost: any = null
	submitted = false
	onSave(event, published = false, viewDemo = false) {
		this.submitted = true

		if (this.postTypeSelected.length > 0) this.model.postType = this.postTypeSelected.toString()
		//this.newsForm.controls.postType.setValue(this.model.postType)

		if (this.newsForm.invalid) {
			return
		}
		//return
		this.model.isPublished = published
		if (published) this.model.status = 1
		else this.model.status = 2
		if (this.model.id && this.model.id > 0) {
			this.newsService.update(this.model, this.filePost).subscribe((res) => {
				if (res.success != 'OK') {
					this.toast.error(COMMONS.UPDATE_FAILED)
					return
				}
				// insert vào Notification
				this.insertNotification(false)
				if (viewDemo) {
					//this.router.navigate()
					window.open('/cong-bo/tin-tuc-su-kien/xem-truoc/' + this.model.id)
					return
				}
				// cap nhap
				this.hisNewsModel.status = STATUS_HISNEWS.UPDATE
				this.hisNewsModel.objectId = this.model.id
				this.hisNewsModel.type = 1 // tin tức
				this.newsService.hisNewsCreate(this.hisNewsModel).subscribe((res) => {
					if ((res.success = RESPONSE_STATUS.success)) {
						if (published == true) {
							this.hisNewsModel.status = STATUS_HISNEWS.PUBLIC
							this.newsService.hisNewsCreate(this.hisNewsModel).subscribe()
						}
						if (published == false && this.hisPublic == true) {
							this.hisNewsModel.status = STATUS_HISNEWS.CANCEL
							this.newsService.hisNewsCreate(this.hisNewsModel).subscribe()
						}
					}
					return
				})

				// this.newsService.hisNewsCreate(this.hisNewsModel).pipe(
				// 	switchMap(result=> result.success),
				// 	switchMap(success =>this.newsService.hisNewsCreate({...this.hisNewsModel,"status" : STATUS_HISNEWS.PUBLIC}))
				// )

				this.toast.success(COMMONS.UPDATE_SUCCESS)
				this.router.navigate(['/quan-tri/tin-tuc'])
			})
		} else {
			this.newsService.create(this.model, this.filePost).subscribe((res) => {
				if (res.success != 'OK') {
					let errorMsg = COMMONS.ADD_FAILED
					this.toast.error(errorMsg)
					return
				}
				this.model.id = res.result
				this.insertNotification(true)

				if (viewDemo) {
					window.open('/cong-bo/tin-tuc-su-kien/xem-truoc/' + this.model.id)
					return
				}

				// khởi tạo
				this.hisNewsModel.status = STATUS_HISNEWS.CREATE
				this.hisNewsModel.objectId = res.result
				this.hisNewsModel.type = 1 // tin tức
				this.newsService.hisNewsCreate(this.hisNewsModel).subscribe((res) => {
					if ((res.success = RESPONSE_STATUS.success)) {
						if (published == true) {
							this.hisNewsModel.status = STATUS_HISNEWS.PUBLIC
							this.newsService.hisNewsCreate(this.hisNewsModel).subscribe()
						}
						return
					}
					return
				})
				// soạn thảo

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
		this.model.postType = this.postTypeSelected.toString()
		this.newsForm.controls.postType.setValue(this.model.postType)
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
		var file = event.target.files[0]
		if (!file) {
			return
		}
		if (file.size > 3000000) {
			this.toast.error('Chỉ chọn tệp có dụng lượng nhỏ hơn 3MB')
			return
		}
		if (!this.allowImageExtend.includes(file.type)) {
			this.toast.error('Chỉ chọn tệp tin hình ảnh')
			return
		}

		//preview image
		this.filePost = file
		this.avatarUrl = URL.createObjectURL(file)
		if (!this.model.imagePath) this.model.imagePath = 'hasImage'
		let formData = new FormData()
		formData.append('file', file, file.name)

		this.newsService.uploadFile(formData).subscribe((res) => {
			if (res.success != 'OK') {
				this.toast.error('Xảy ra lỗi trong quá trình xử lý')
				return
			}
			this.model.imagePath = res.result.path
			this.newsForm.controls.imagePath.setValue(this.model.imagePath)
			// this.avatarUrl = `${AppSettings.API_DOWNLOADFILES}/${this.model.imagePath}`
		})
	}

	public onReady(editor) {
		editor.ui.getEditableElement().parentElement.insertBefore(editor.ui.view.toolbar.element, editor.ui.getEditableElement())
		editor.plugins.get('FileRepository').createUploadAdapter = (loader) => {
			return new UploadAdapter(loader, this.http)
		}
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
