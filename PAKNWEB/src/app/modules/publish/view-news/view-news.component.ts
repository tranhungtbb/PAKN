import { Component, OnInit } from '@angular/core'

import { Router, ActivatedRoute } from '@angular/router'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { NewsService } from 'src/app/services/news.service'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { AppSettings } from 'src/app/constants/app-setting'

@Component({
	selector: 'app-news',
	templateUrl: './view-news.component.html',
	styleUrls: ['./view-news.component.css'],
})
export class ViewNewsComponent implements OnInit {
	constructor(private newsService: NewsService, private router: Router, private activatedRoute: ActivatedRoute, private userStorage: UserInfoStorageService) {
		this.newsHightlight = []
	}

	model: any = {}
	newsRelates: any[] = []
	viewDemo = false
	newsHightlight: any
	title: string = ''

	ngOnInit() {
		let url: string = this.router.url
		let userType = this.userStorage.getTypeObject()
		if (url.includes('/xem-truoc/') && userType == 1) {
			this.viewDemo = true
		}

		this.activatedRoute.params.subscribe((params) => {
			if (params['id']) {
				this.getData(params['id'])
				this.getNewsRelates(params['id'])
			}
		})
		this.newsService.getListHomePage({}).subscribe((res) => {
			if (res.success != RESPONSE_STATUS.success) {
				return
			}
			if (res.result.length > 0) {
				this.newsHightlight = res.result
			}
			return
		})
	}
	changeKeySearch(event) {
		this.title = event.target.value
	}
	redirectNews() {
		if (this.title == null || this.title == '') return
		this.router.navigateByUrl('/cong-bo/tin-tuc-su-kien?title=' + this.title)
		// window.location.href = '/cong-bo/tin-tuc-su-kien?title=' + this.title
	}
	getData(id) {
		this.newsService.getViewDetail({ id }).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				this.model = res.result.NENewsViewDetail[0]
			}
		})
	}

	getNewsRelates(id) {
		this.newsService.getAllRelates({ id }).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				this.newsRelates = res.result.NENewsGetAllRelates
				// .map((e) => {
				// 	e.imagePath = `${AppSettings.API_DOWNLOADFILES}/${e.imagePath}`
				// 	return e
				// })
			} else {
				this.newsRelates = []
			}
		})
	}
}
