import { Component, OnInit, ViewChild } from '@angular/core'
import { Router, ActivatedRoute } from '@angular/router'
import { ToastrService } from 'ngx-toastr'

import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { CatalogService } from 'src/app/services/catalog.service'
import { IndexSettingService } from 'src/app/services/index-setting.service'
import { NewsService } from 'src/app/services/news.service'

@Component({
	selector: 'app-government-notice',
	templateUrl: './government-notice.component.html',
	styleUrls: ['./government-notice.component.css'],
})
export class GovernmentNoticeComponent implements OnInit {
	listData: any[] = []
	query: any = {
		pageSize: 10,
		pageIndex: 1,
		title: '',
		status: 1,
		newsType: '',
	}
	pagination = []
	Status: number = 1 // trạng thái đã công bố
	totalRecords: number = 0

	//
	ltsIndexSettingWebsite: any = []

	constructor(
		private _toastr: ToastrService,
		private newsService: NewsService,
		private _router: Router,
		private activatedRoute: ActivatedRoute,
		private _catalogService: CatalogService,
		private indexSettingService: IndexSettingService
	) {
	}

	async ngOnInit() {
		this.getListPaged()
	}

	// get type news

	dataStateChange(newsType) {
		this.query.newsType = newsType
		this.getListPaged()
	}

	getListPaged() {
		this.query.newsType == null ? '' : this.query.newsType
		this.query.title == null ? '' : this.query.title.trim(),
			this.newsService
				.getAllPagedList({
					PageIndex: this.query.pageIndex,
					PageSize: this.query.pageSize,
					Title: this.query.title,
					NewsType: this.query.newsType,
					Status: this.Status,
				})
				.subscribe((res) => {
					if (res.success != RESPONSE_STATUS.success) {
						this.listData = []
						this.totalRecords = 0
						this.query.pageIndex = 1
						this.query.pageSize = 10
						return
					} else {
						if (res.result.NENewsGetAllOnPage.length == 0) {
							this.listData = []
							this.totalRecords = 0
							this.query.pageIndex = 1
							this.query.pageSize = 10
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
