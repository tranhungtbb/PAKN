import { Component, OnInit, ViewChild } from '@angular/core'
import { DomSanitizer } from '@angular/platform-browser'
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
		private activatedRoute: ActivatedRoute,
		private sanitizer: DomSanitizer
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
	avatarUrl: any = 'assets/dist/images/no.jpg'

	ngOnInit() {
		this.newsForm = this.formBuilder.group({
			title: ['', [Validators.required]],
			summary: ['', [Validators.required]],
			contents: [''],
			newsType: [''],
			postType: [''],
			pushNotify: [''],
		})


		// lấy thông tin, nếu là sửa
		this.activatedRoute.params.subscribe((params) => {
			if (params['id']) {
				this.newsService.getById({ id: params['id'] }).subscribe((res) => {
					if (res.success != 'OK') {
						return
					}
					this.model = res.result.NENewsGetByID[0]
					this.getNewsRelatesInfo()

					this.child_NewsRelate.parentNews = this.model.id

					// lấy avatar bài viết đang chỉnh sửa
					if (this.model.imagePath != null && this.model.imagePath.trim() != '') {
						this.newsService.getAvatars([this.model.id]).subscribe((res:any[])=>{
							if(res){
								let objectURL = 'data:image/jpeg;base64,' + res[0].byteImage
								this.avatarUrl = this.sanitizer.bypassSecurityTrustUrl(objectURL)
							}
						});
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

					//get avatar 
					this.getNewsRelatesAvatars();
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

		this.model.isPublished = published
		if(published){
			this.model.status = 1
		}

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

	//sau khi đóng modal con sẽ gọi đến
	onModalNewsRelate_Closed() {

		// lấy danh sách bài viết liên quan từ popup con
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
					this.newsRelatesSelected = res.result.NENewsGetAllOnPage
					this.getNewsRelatesAvatars();
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

	getNewsRelatesAvatars(){
		let ids = this.newsRelatesSelected.map(c=>c.id);

		this.newsService.getAvatars(ids).subscribe(res=>{
			if(res){
				res.forEach(e=>{
					let item = this.newsRelatesSelected.find(c=>c.id == e.id)
					let objectURL = 'data:image/jpeg;base64,' + e.byteImage
					item.imageBin = this.sanitizer.bypassSecurityTrustUrl(objectURL)
				})
			}
		});
	}

	public onChangeEditor({ editor }: any) {
		let data = editor.getData()
		this.model.contents = data
	}
	public onReady(editor) {
		editor.ui.getEditableElement().parentElement.insertBefore(editor.ui.view.toolbar.element, editor.ui.getEditableElement())
	}
}
