import { Component, OnInit } from '@angular/core'

import { NewsService } from 'src/app/services/news.service'
import { CatalogService } from 'src/app/services/catalog.service'

import { NewsCreateOrUpdateComponent } from '../news-create-or-update/news-create-or-update.component'
declare var $: any
@Component({
	selector: 'app-news-relate-modal',
	templateUrl: './news-relate-modal.component.html',
	styleUrls: ['./news-relate-modal.component.css'],
})
export class NewsRelateModalComponent implements OnInit {
	constructor(private newsService: NewsService, private newsCreateOrUpdateComponent: NewsCreateOrUpdateComponent, private catalogService: CatalogService) {}

	listNewsCategories: any[] = []
	listDataPaged: any[]
	newsSelected: any[] = []
	query: any = {
		pageSize: 20,
		pageIndex: 1,
		title: '',
		newsType: '',
	}
	totalCount: number = 0
	pageCount: number = 0
	parentNews: number
	ngOnInit() {
		this.getListPaged()
		this.catalogService
			.newsTypeGetList({
				pageSize: 10000,
				pageIndex: 1,
			})
			.subscribe((res) => {
				if (res.success != 'OK') {
					return
				}
				this.listNewsCategories = res.result.CANewsTypeGetAllOnPage
			})
	}
	onChangeChecked(id: number, checked: boolean) {
		if (checked) {
			this.newsSelected.push(id)
		} else {
			let index = this.newsSelected.indexOf(id)
			this.newsSelected.splice(index, 1)
		}
	}
	onSave() {
		this.newsCreateOrUpdateComponent.onModalNewsRelate_Closed()
		$('#modal-news-relate').modal('hide')
	}
	getListPaged() {
		this.query.newsType == null ? '' : this.query.newsType
		this.newsService.getAllPagedList(this.query).subscribe((res) => {
			if (res.success != 'OK') return
			this.listDataPaged = res.result.NENewsGetAllOnPage.filter((c) => c.id != this.parentNews)
			if (this.totalCount <= 0) this.totalCount = res.result.TotalCount
			this.totalCount = Math.ceil(this.totalCount / this.query.pageSize)
		})
	}
	changePage(page: any) {
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
	filterChange() {
		this.getListPaged()
	}
	hasOne(id: number): boolean {
		if (this.newsSelected == null || this.newsSelected.length == 0) {
			return false
		}
		return this.newsSelected.some((c) => c == id)
	}
	openModal(newsRelate: any[], parentNews: number) {
		if (newsRelate) this.newsSelected = newsRelate.map((c) => parseInt(c))
		this.parentNews = parentNews
		$('#modal-news-relate').modal('show')
	}
}
