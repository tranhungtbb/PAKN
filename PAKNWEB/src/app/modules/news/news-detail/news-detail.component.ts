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

@Component({
	selector: 'app-news-detail',
	templateUrl: './news-detail.component.html',
	styleUrls: ['./news-detail.component.css'],
})
export class NewsDetailComponent implements OnInit {
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
				this.getInfo()
			})
	}

	getInfo() {
		this.activatedRoute.params.subscribe((params) => {
			if (params['id']) {
				this.newsService.getById({ id: params['id'] }).subscribe((res) => {
					if (res.success != 'OK') {
						return
					}
					this.model = res.result.NENewsGetByID[0]
					this.hisPublic = this.model.isPublished
					this.postTypeSelected = this.model.postType.trim().split(',')
					let type = this.listNewsTypes.find((x) => x.id == this.model.newsType)
					this.model.newsType = type.name
					//lay danh sach bai viet lien quan
					this.getNewsRelatesInfo()
				})
			}
		})
	}

	ngAfterViewInit() {}

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

	public onReady(editor) {
		editor.ui.getEditableElement().parentElement.insertBefore(editor.ui.view.toolbar.element, editor.ui.getEditableElement())
		editor.plugins.get('FileRepository').createUploadAdapter = (loader) => {
			return new UploadAdapter(loader, this.http)
		}
	}
}
