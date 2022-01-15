import { Component, OnInit, ViewChild } from '@angular/core'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
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
	keySearch: string = null

	constructor(
		private newsService: NewsService
	) {
	}

	ngOnInit() {
		this.getListPaged()
	}

	// get type news

	dataStateChange() {
		this.getListPaged()
	}


	getListPaged() {
		this.query.newsType == null ? '' : this.query.newsType
		this.query.title = this.keySearch == null ? '' : this.keySearch.trim(),
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
