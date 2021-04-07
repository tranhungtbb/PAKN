import { Component, OnInit } from '@angular/core'
import { DomSanitizer } from '@angular/platform-browser'

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
	constructor(
		private newsService: NewsService,
		private newsCreateOrUpdateComponent: NewsCreateOrUpdateComponent,
		private catalogService: CatalogService,
		private sanitizer: DomSanitizer
	) {}

	listNewsCategories: any[] = []
	listDataPaged: any[]
	newsSelected: any[] = []
	query: any = {
		pageSize: 20,
		pageIndex: 1,
		title: '',
		newsType: null,
	}
	modalTitle: string = ''
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
		let newsItem = this.listDataPaged.find((c) => c.id == id)
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
		this.newsService
			.getAllPagedList({
				pageSize: this.query.pageSize,
				pageIndex: this.query.pageIndex,
				title: this.query.title,
				newsType: this.query.newsType == null ? '' : this.query.newsType,
			})
			.subscribe((res) => {
				if (res.success != 'OK') return
				this.listDataPaged = res.result.NENewsGetAllOnPage.filter((c) => c.id != this.parentNews)
				if (this.totalCount <= 0) this.totalCount = res.result.TotalCount
				this.totalCount = Math.ceil(this.totalCount / this.query.pageSize)

				//get avatars
				this.getNewsAvatars()

				// lấy ds tin tức từ id
				// if (this.newsIds != null && this.newsIds.length > 0)
				// 	this.newsSelected=this.listDataPaged.filter(c=>this.newsIds.some(d=>d==c.id))
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

	//mở modal, được gọi từ comp cha
	openModal(newsRelate: any[], parentNews: number) {
		this.parentNews = parentNews
		if (newsRelate) {
			this.newsSelected = newsRelate.map((c) => parseInt(c))
			this.listDataPaged = this.listDataPaged.filter((c) => c.id != this.parentNews)
		}

		$('#modal-news-relate').modal('show')
	}

	getNewsAvatars() {
		let ids = this.listDataPaged.map((c) => c.id)

		this.newsService.getAvatars(ids).subscribe((res) => {
			if (res) {
				res.forEach((e) => {
					let item = this.listDataPaged.find((c) => c.id == e.id)
					let objectURL = 'data:image/jpeg;base64,' + e.byteImage
					item.imageBin = this.sanitizer.bypassSecurityTrustUrl(objectURL)
				})
			}
		})
	}
}
