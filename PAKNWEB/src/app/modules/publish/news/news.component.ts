import { Component, OnInit, ViewChild } from '@angular/core'
import { Router } from '@angular/router'

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
	query: any = {
		pageSize: 10,
		pageIndex: 1,
		title: '',
		status: null,
		newsType: null,
	}
	pagination = []
	Status: number = 1 // trạng thái đã công bố
	totalRecords: number = 0

	constructor(private newsService: NewsService, private _router: Router) {}

	ngOnInit() {
		this.getListPaged()
	}

	getListPaged() {
		this.newsService
			.getAllPagedList({
				pageIndex: this.query.pageIndex,
				pageSize: this.query.pageSize,
				title: '',
				newsType: '',
				status: this.Status,
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
