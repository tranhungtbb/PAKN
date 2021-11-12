import { Component, OnInit, ViewChild } from '@angular/core'
import { Router, ActivatedRoute } from '@angular/router'

import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { AppSettings } from 'src/app/constants/app-setting'
import { ViewRightComponent } from 'src/app/modules/publish/view-right/view-right.component'
import { NewsService } from 'src/app/services/news.service'

@Component({
	selector: 'app-news',
	templateUrl: './news.component.html',
	styleUrls: ['./news.component.css'],
})
export class NewsComponent implements OnInit {
	@ViewChild(ViewRightComponent, { static: true }) viewRightComponent: ViewRightComponent
	listData: any[] = []
	newsHightlight: any
	query: any = {
		pageSize: 5,
		pageIndex: 1,
		title: '',
		status: null,
		newsType: null,
	}
	pagination = []
	Status: number = 1 // trạng thái đã công bố
	totalRecords: number = 0

	constructor(private newsService: NewsService, private _router: Router, private activatedRoute: ActivatedRoute) {
		this.newsHightlight = []
	}

	ngOnInit() {
		this.activatedRoute.queryParams.subscribe((params) => {
			let suggest = params['title']
			if (suggest) {
				this.query.title = suggest
			}
		})
		this.getListPaged()

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

	getListPaged() {
		this.newsService
			.getAllPagedList({
				pageIndex: this.query.pageIndex,
				pageSize: this.query.pageSize,
				title: this.query.title,
				newsType: '',
				status: this.Status,
			})
			.subscribe((res) => {
				if (res.success != RESPONSE_STATUS.success) {
					this.listData = []
					this.totalRecords = 0
					this.query.pageIndex = 1
					this.query.pageSize = 5
					return
				} else {
					if (res.result.NENewsGetAllOnPage.length == 0) {
						this.listData = []
						this.totalRecords = 0
						this.query.pageIndex = 1
						this.query.pageSize = 5
					} else {
						this.listData = res.result.NENewsGetAllOnPage
						this.query.pageIndex = res.result.PageIndex
						this.query.pageSize = res.result.PageSize
						this.totalRecords = res.result.TotalCount
						this.padi()
					}
				}
			})
	}
	changeKeySearch(event) {
		this.query.title = event.target.value
	}
	redirectDetail(id: any) {
		this._router.navigate(['/cong-bo/tin-tuc-su-kien/' + id])
	}
	padi() {
		this.pagination = []
		for (let i = 0; i < Math.ceil(this.totalRecords / this.query.pageSize); i++) {
			this.pagination.push({ index: i + 1 })
		}
	}

	changePagination(index: any) {
		if (this.query.pageIndex > index) {
			if (index > 0) {
				this.query.pageIndex = index
				this.getListPaged()
			}
			return
		} else if (this.query.pageIndex < index) {
			if (this.pagination.length >= index) {
				this.query.pageIndex = index
				this.getListPaged()
			}
			return
		}
		return
	}
}
