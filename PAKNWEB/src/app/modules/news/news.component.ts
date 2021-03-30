import { Component, OnInit } from '@angular/core'

import { NewsService } from 'src/app/services/news.service'

import { COMMONS } from 'src/app/commons/commons'
import { NewsModel } from 'src/app/models/NewsObject'

@Component({
	selector: 'app-news',
	templateUrl: './news.component.html',
	styleUrls: ['./news.component.css'],
})
export class NewsComponent implements OnInit {
	constructor(
		private newsService: NewsService
	) {}

	query: any = {
		pageSize: 20,
		pageIndex: 1,
		title: '',
		status: '',
		newType: '',
	}
	
	listNewCategories: any[] = []

	listDataPaged:any[] =[]
	listStatus: any[] = [
		{ value: 0, text: 'Đã thu hồi' },
		{ value: 1, text: 'Đã công bố' },
		{ value: 2, text: 'Đang soạn thảo' },
	]
	totalCount:number = 0;
	pageCount:number = 0


	ngOnInit() {
		this.getListPaged();

	}

	getListPaged() {
		this.newsService.getAllPagedList(this.query).subscribe(res=>{
			if (res.success != 'OK') return
			this.listDataPaged = res.result.NENewsGetAllOnPage
			if (this.totalCount <= 0) this.totalCount = res.result.TotalCount
			this.totalCount = Math.ceil(this.totalCount / this.query.pageSize)
		});
	}
	delNews(id:number){

	}
	filterChange(){
		this.getListPaged();
	}

	changePage(page: number): void {
		this.query.pageIndex += page
		if (this.query.pageIndex < 1) {
			this.query.pageIndex = 1
			return
		}
		if (this.query.pageIndex > this.pageCount) {
			this.query.pageIndex = this.pageCount
			return
		}
		this.getListPaged()
	}
}
