import { Component, OnInit, ViewChild } from '@angular/core'
import { Router } from '@angular/router'
import { DomSanitizer } from '@angular/platform-browser'

import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { NewsService } from 'src/app/services/news.service'
declare var $: any

@Component({
	selector: 'app-list-news',
	templateUrl: './list-news.component.html',
	styleUrls: ['./list-news.component.css'],
})
export class ListNewsComponent implements OnInit {
	constructor(private newsService: NewsService, private _router: Router, private sanitizer: DomSanitizer) {}

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

	ngOnInit() {
		this.getListPaged()
	}

	ngAfterViewInit() {}

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
