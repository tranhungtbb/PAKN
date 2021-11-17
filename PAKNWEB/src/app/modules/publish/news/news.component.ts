import { Component, OnInit, ViewChild } from '@angular/core'
import { Router, ActivatedRoute } from '@angular/router'
import { ToastrService } from 'ngx-toastr'

import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { ViewRightComponent } from 'src/app/modules/publish/view-right/view-right.component'
import { CatalogService } from 'src/app/services/catalog.service'
import { IndexSettingService } from 'src/app/services/index-setting.service'
import { NewsService } from 'src/app/services/news.service'

@Component({
	selector: 'app-news',
	templateUrl: './news.component.html',
	styleUrls: ['./news.component.css'],
})
export class NewsComponent implements OnInit {
	@ViewChild(ViewRightComponent, { static: true }) viewRightComponent: ViewRightComponent
	listData: any[] = []
	listNewsType: any[] = []
	newHighLight: any = {}
	listNewHighLight: any
	query: any = {
		pageSize: 5,
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
		this.listNewHighLight = []
	}

	async ngOnInit() {
		this.activatedRoute.queryParams.subscribe((params) => {
			let suggest = params['title']
			if (suggest) {
				this.query.title = suggest
			}
		})
		await this._catalogService
			.newsTypeGetDrop({})
			.toPromise()
			.then((res) => {
				if (res.success == RESPONSE_STATUS.success) {
					this.listNewsType = res.result
					if (this.listNewsType.length > 0) {
						// this.query.newsType = this.listNewsType[0].value
					}
				} else {
					this._toastr.error(res.message)
				}
			})
			.catch((err) => {
				console.log(err)
			})

		this.getListPaged()

		this.newsService.getListHomePage({}).subscribe((res) => {
			if (res.success != RESPONSE_STATUS.success) {
				return
			}
			if (res.result.length > 0) {
				this.newHighLight = res.result.shift()
				this.listNewHighLight = res.result
			}
			return
		})
		this.indexSettingService.GetInfo({}).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				this.ltsIndexSettingWebsite = res.result.lstSYIndexWebsite == null ? [] : res.result.lstSYIndexWebsite
			}
		}),
			(error) => {
				console.log(error)
				alert(error)
			}
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
