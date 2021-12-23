import { Component, OnInit } from '@angular/core'
import { DomSanitizer } from '@angular/platform-browser'
import { ToastrService } from 'ngx-toastr'

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
	constructor(private newsService: NewsService, private catalogService: CatalogService, private sanitizer: DomSanitizer, private _toa: ToastrService) { }

	public newsCreateOrUpdateComponent: NewsCreateOrUpdateComponent
	listNewsCategories: any[] = []
	listDataPaged: any[]
	newsSelected: any[] = []
	query: any = {
		pageSize: 15,
		pageIndex: 1,
		title: '',
		newsType: '',
	}
	newsType: any = null
	modalTitle: string = ''
	totalCount: number = 0
	pageCount: number = 0
	parentNews: number
	newsRelate: any = []
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
	onPageChange(event: any): void {
		this.query.pageSize = event.rows
		this.query.pageIndex = event.first / event.rows + 1
		this.getListPaged()
	}
	onChangeChecked(id: number, checked: boolean) {
		let newsItem = this.listDataPaged.find((c) => c.id == id)
		if (checked) {
			this.newsSelected.push(id)
		} else {
			let index = this.newsSelected.indexOf(id)
			this.newsSelected.splice(index, 1)
		}
	}
	onSave() {
		if (this.newsSelected.length == 0) {
			this._toa.error('Vui lòng chọn bài viết liên quan')
			return
		}
		this.newsCreateOrUpdateComponent.onModalNewsRelate_Closed()
		this._toa.success('Thêm bài viết liên quan thành công')
		$('#modal-news-relate').modal('hide')
	}
	getListPaged() {
		this.query.title = this.query.title == null ? '' : this.query.title.trim()
		this.query.newsType = this.newsType == null ? '' : this.newsType
		this.newsService
			.getAllNewsRelatesForCreate({
				NewId: this.parentNews,
				LstNewsId: this.newsRelate.join(','),
				Title: this.query.title,
				NewsType: this.query.newsType,
				PageSize: this.query.pageSize,
				PageIndex: this.query.pageIndex,
			})
			.subscribe((res) => {
				if (res.success != 'OK') {
					this.listDataPaged = []
					this.totalCount = 0
					this.padi()
					return
				}
				this.listDataPaged = res.result.NENewsGetAllOnPage.filter((c) => c.id != this.parentNews)
				this.totalCount = res.result.TotalCount
				this.padi()
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
		this.query.pageIndex = 1
		this.getListPaged()
	}

	hasOne(id: number): boolean {
		if (this.newsSelected == null || this.newsSelected.length == 0) {
			return false
		}
		return this.newsSelected.some((c) => c == id)
	}

	//mở modal, được gọi từ comp cha
	openModal(newsRelate: any[], parentNews: number) {
		debugger
		this.parentNews = parentNews
		this.newsRelate = newsRelate.concat(this.newsSelected)
		this.newsRelate = this.newsRelate.filter((item, index) => this.newsRelate.indexOf(item) === index)
		this.getListPaged()
		// this.newsSelected = []
		$('#modal-news-relate').modal('show')
	}

	pagination = []
	padi() {
		this.pagination = []
		for (let i = 0; i < Math.ceil(this.totalCount / this.query.pageSize); i++) {
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
