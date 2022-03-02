import { Component, OnInit, ViewChild, ElementRef, AfterViewInit } from '@angular/core'

import { Router, ActivatedRoute } from '@angular/router'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { NewsService } from 'src/app/services/news.service'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { IndexSettingService } from 'src/app/services/index-setting.service'
import { UploadFileService } from 'src/app/services/uploadfiles.service'
import { saveAs as importedSaveAs } from 'file-saver'
import { ToastrService } from 'ngx-toastr'

@Component({
	selector: 'app-news',
	templateUrl: './view-news.component.html',
	styleUrls: ['./view-news.component.css'],
})
export class ViewNewsComponent implements OnInit, AfterViewInit {
	constructor(
		private newsService: NewsService,
		private indexSettingService: IndexSettingService,
		private router: Router,
		private activatedRoute: ActivatedRoute,
		private userStorage: UserInfoStorageService,
		private fileService: UploadFileService,
		private toastr: ToastrService
	) {
		this.newsHightlight = []
	}
	@ViewChild('contents', { static: true }) contents: ElementRef

	model: any = {}
	files: any = []
	newsRelates: any[] = []
	viewDemo = false
	newsHightlight: any
	title: string = ''
	ltsIndexSettingWebsite: any = []

	ngOnInit() {
		let url: string = this.router.url
		let userType = this.userStorage.getTypeObject()
		if (url.includes('/xem-truoc/') && userType == 1) {
			this.viewDemo = true
		}

		this.activatedRoute.params.subscribe((params) => {
			if (params['id']) {
				window.scroll(0, 0)
				$('html, body')
					.animate({ scrollTop: 0 })
					.promise()
				this.getData(params['id'])
				this.newsService.getListHomePage({}).subscribe(
					(res) => {
						if (res.success == RESPONSE_STATUS.success) {
							this.newsRelates = res.result.filter(x => x.id != params['id'])
						}
					},
					(error) => {
						console.log(error)
						alert(error)
					}
				)
			}
		})

	}
	ngAfterViewInit() {

	}
	changeKeySearch(event) {
		this.title = event.target.value
	}
	redirectNews() {
		if (this.title == null || this.title == '') return
		this.router.navigateByUrl('/cong-bo/tin-tuc-su-kien?title=' + this.title)
	}
	getData(id) {
		if (this.viewDemo) {
			this.newsService.getViewDetail({ id }).subscribe((res) => {
				if (res.success == RESPONSE_STATUS.success) {
					this.model = res.result.NENewsViewDetail
					this.files = res.result.Files
				}
			})
		} else {
			this.newsService.getViewDetailPublic({ id }).subscribe((res) => {
				if (res.success == RESPONSE_STATUS.success) {
					this.model = res.result.NENewsViewDetail
					this.files = res.result.Files
				} else {
					this.model = null
				}
			})
		}
	}

	// getNewsRelates(id) {
	// 	this.newsService.getAllRelates({ id }).subscribe((res) => {
	// 		if (res.success == RESPONSE_STATUS.success) {
	// 			this.newsRelates = res.result.NENewsGetAllRelates
	// 		} else {
	// 			this.newsRelates = []
	// 		}
	// 	})
	// }
	redirectDetail(id: any) {
		this.router.navigate(['/cong-bo/tin-tuc-su-kien/' + id])
	}
	DownloadFile(file: any) {
		var request = {
			Path: file.fileAttach,
			Name: file.name,
		}
		this.fileService.downloadFile(request).subscribe(
			(response) => {
				var blob = new Blob([response], { type: response.type })
				importedSaveAs(blob, file.name)
			},
			(error) => {
				this.toastr.error('Không tìm thấy file trên hệ thống')
			}
		)
	}
}
