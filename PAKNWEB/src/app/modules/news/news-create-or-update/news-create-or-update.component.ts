import { Component, ElementRef, OnInit, ViewChild } from '@angular/core'
import { DomSanitizer } from '@angular/platform-browser'
import * as ClassicEditor from '@ckeditor/ckeditor5-build-classic'
import { FormGroup, FormBuilder, Validators } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'

import { Router, ActivatedRoute } from '@angular/router'
import { FILETYPE, RESPONSE_STATUS } from '../../../constants/CONSTANTS'
import { NewsService } from '../../../services/news.service'
import { CatalogService } from '../../../services/catalog.service'

import { NewsRelateModalComponent } from '../news-relate-modal/news-relate-modal.component'
import { COMMONS } from '../../../commons/commons'
import { AppSettings } from '../../../constants/app-setting'
import { NewsModel, HISNewsModel } from '../../../models/NewsObject'
import { Api } from 'src/app/constants/api'
import { UploadAdapter } from 'src/app/services/uploadAdapter'
import { HttpClient } from '@angular/common/http'
import { UploadFileService } from 'src/app/services/uploadfiles.service'
import { saveAs as importedSaveAs } from 'file-saver'

declare var $: any

@Component({
	selector: 'app-news-create-or-update',
	templateUrl: './news-create-or-update.component.html',
	styleUrls: ['./news-create-or-update.component.css'],
})
export class NewsCreateOrUpdateComponent implements OnInit {
	@ViewChild(NewsRelateModalComponent, { static: false }) child_NewsRelate: NewsRelateModalComponent
	@ViewChild('file', { static: false }) file: ElementRef
	constructor(
		private toast: ToastrService,
		private formBuilder: FormBuilder,
		private newsService: NewsService,
		private catalogService: CatalogService,
		private router: Router,
		private activatedRoute: ActivatedRoute,
		private sanitizer: DomSanitizer,
		private http: HttpClient,
		private fileService: UploadFileService
	) { }
	allowImageExtend = ['image/jpeg', 'image/png']
	public Editor = ClassicEditor
	public ckConfig = {
		simpleUpload: {
			uploadUrl: AppSettings.API_ADDRESS + Api.UploadImageNews,
		},
	}

	model: NewsModel = new NewsModel()
	files: any = []
	fileDelete: any = []
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
	avatarUrl: any = '/assets/dist/images/index/Frame 2184.png'

	pageIndex: any = 1

	ngOnInit() {
		this.newsForm = this.formBuilder.group({
			title: [this.model.title, [Validators.required, Validators.maxLength(500)]],
			summary: [this.model.summary],
			contents: [this.model.contents, [Validators.required]],
			newsType: [this.model.newsType],
			postType: [this.model.postType],
			imagePath: [this.model.imagePath],
			pushNotify: [this.model.isNotification],
		})

		this.postTypeSelected = this.model.postType.trim().split(',')

		this.activatedRoute.params.subscribe((params) => {
			if (params['id']) {
				this.newsService.getById({ id: params['id'] }).subscribe((res) => {
					if (res.success != 'OK') {
						return
					}
					this.rebuidForm()
					this.model = res.result.NENewsGetByID[0]
					this.files = res.result.NENewsFiles
					this.hisPublic = this.model.isPublished
					this.postTypeSelected = this.model.postType.trim().split(',')

					//lay danh sach bai viet lien quan
					this.getNewsRelatesInfo()
				})
			}
			if (params['pageIndex']) {
				this.pageIndex = Number(params['pageIndex'])
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

		this.onChangePostType(0, true)
	}

	rebuidForm() {
		this.newsForm.reset({
			title: this.model.title,
			summary: this.model.summary,
			contents: this.model.contents,
			newsType: this.model.newsType,
			postType: this.model.postType,
			imagePath: this.model.imagePath,
			pushNotify: this.model.isNotification
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
	onSave(isSave = false, published = false, viewDemo = false) {
		this.submitted = true
		this.model.title = this.model.title == null ? '' : this.model.title.trim()
		if (this.postTypeSelected.length > 0) this.model.postType = this.postTypeSelected.toString()
		//this.newsForm.controls.postType.setValue(this.model.postType)

		if (this.newsForm.invalid) {
			// this.toast.error('Vui lòng nhập dữ liệu những trường bắt buộc')
			return
		}
		//return
		this.model.isPublished = published
		if (published) this.model.status = 1
		else this.model.status = 0

		if (isSave == true) {
			this.model.status = 2
		}

		let request = {
			model: this.model,
			filePost: this.filePost,
			files: this.files,
			filesDelete: this.fileDelete,
		}

		if (this.model.id && this.model.id > 0) {
			if (published == true) {
				this.newsService.update(request).subscribe((res) => {
					if (res.success != 'OK') {
						this.toast.error(COMMONS.UPDATE_FAILED)
						return
					}
					if (viewDemo) {
						window.open('/cong-bo/tin-tuc-su-kien/xem-truoc/' + this.model.id)
						return
					}

					this.toast.success(COMMONS.UPDATE_SUCCESS)
					this.router.navigate(['/quan-tri/tin-tuc/danh-sach-tong-hop'])
				})
			} else {
				this.newsService.update(request).subscribe((res) => {
					if (res.success != 'OK') {
						let result = Number(res.result)
						if (result && result == -1) {
							this.toast.error('Tiêu đề thông báo bị trùng')
							return
						} else {
							this.toast.error(COMMONS.UPDATE_FAILED)
							return
						}
					}
					if (viewDemo) {
						window.open('/cong-bo/tin-tuc-su-kien/xem-truoc/' + this.model.id)
						return
					}
					this.toast.success(COMMONS.UPDATE_SUCCESS)
					this.router.navigate(['/quan-tri/tin-tuc/danh-sach-tong-hop'])
				})
			}
		} else {
			this.newsService.create(request).subscribe((res) => {
				if (res.success != 'OK') {
					let result = Number(res.result)
					if (result && result == -1) {
						this.toast.error('Tiêu đề thông báo bị trùng')
					} else {
						this.toast.error(COMMONS.ADD_FAILED)
					}
					return
				}
				this.model.id = res.result
				// this.insertNotification(true)

				if (viewDemo) {
					window.open('/cong-bo/tin-tuc-su-kien/xem-truoc/' + this.model.id)
					return
				}

				this.toast.success(COMMONS.ADD_SUCCESS)
				this.router.navigate(['/quan-tri/tin-tuc/danh-sach-tong-hop'])
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
		this.child_NewsRelate.openModal(this.model.newsRelateIds ? this.model.newsRelateIds.split(',').map(Number) : [], this.model.id)
	}
	onModalNewsRelate_Closed() {
		this.model.newsRelateIds = this.child_NewsRelate.newsSelected.concat(this.model.newsRelateIds != null ? this.model.newsRelateIds.split(',') : []).toString()

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
		this.child_NewsRelate.getListPaged()
	}

	removeRelate(id: number) {
		let arr = this.model.newsRelateIds.split(',')
		arr = arr.filter((x) => x != id)
		this.model.newsRelateIds = arr.join(',')
		this.newsRelatesSelected = this.newsRelatesSelected.filter((x) => x.id != id)
	}

	onChangeAvatar() {
		$('#avatar-image').click()
	}
	avatarLocalChange = false
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
		this.model.imagePath = URL.createObjectURL(file)
		this.avatarLocalChange = true
	}

	// upload media

	uploadMedia() {
		this.file.nativeElement.click()
	}
	onInsertMedia(event: any) {
		if (event.target.files.length == 0) {
			return
		}
		const check = this.fileService.checkFileWasExitsted(event, this.files)
		if (check === 1) {
			for (let item of event.target.files) {
				FILETYPE.forEach((fileType) => {
					if (item.type == fileType.text) {
						item.fileType = fileType.value
						this.files.push(item)
					}
				})
				if (!item.fileType) {
					this.toast.error('Định dạng không được hỗ trợ')
				}
			}
		} else if (check === 2) {
			this.toast.error('Không được phép đẩy trùng tên file lên hệ thống')
		} else {
			this.toast.error('File tải lên vượt quá dung lượng cho phép 10MB')
		}
		this.file.nativeElement.value = ''
		console.log(this.files)
	}

	onDeleteMedia(args) {
		const index = this.files.indexOf(args)
		const file = this.files[index]
		this.fileDelete.push(file)
		this.files.splice(index, 1)
	}
	DownloadFile(file: any) {
		var request = {
			Path: file.filePath,
			Name: file.name,
		}
		this.fileService.downloadFile(request).subscribe(
			(response) => {
				var blob = new Blob([response], { type: response.type })
				importedSaveAs(blob, file.name)
			},
			(error) => {
				this.toast.error('Không tìm thấy file trên hệ thống')
			}
		)
	}

	public onReady(editor) {
		editor.ui.getEditableElement().parentElement.insertBefore(editor.ui.view.toolbar.element, editor.ui.getEditableElement())
		editor.plugins.get('FileRepository').createUploadAdapter = (loader) => {
			return new UploadAdapter(loader, this.http)
		}
	}

	back() {
		window.history.back()
	}
}
