import { Component, OnInit } from '@angular/core'
import { ToastrService } from 'ngx-toastr'

import { NewsService } from 'src/app/services/news.service'
import { CatalogService } from 'src/app/services/catalog.service'

import { COMMONS } from 'src/app/commons/commons'
import { NewsModel } from 'src/app/models/NewsObject'
declare var $: any
@Component({
	selector: 'app-news',
	templateUrl: './news.component.html',
	styleUrls: ['./news.component.css'],
})
export class NewsComponent implements OnInit {
	constructor(private newsService: NewsService, private catalogService: CatalogService, private toast: ToastrService) {}

	query: any = {
		pageSize: 20,
		pageIndex: 1,
		title: '',
		status: '',
		newType: '',
	}

	listNewCategories: any[] = []

	listDataPaged: any[] = []
	listStatus: any[] = [
		{ value: 0, text: 'Đã thu hồi' },
		{ value: 1, text: 'Đã công bố' },
		{ value: 2, text: 'Đang soạn thảo' },
	]
	totalCount: number = 0
	pageCount: number = 0

	ngOnInit() {
		this.getListPaged()
		//get all news type
		this.catalogService
			.newsTypeGetList({
				pageSize: 10000,
				pageIndex: 1,
			})
			.subscribe((res) => {
				if (res.success != 'OK') {
					return
				}
				this.listNewCategories = res.result.CANewsTypeGetAllOnPage
			})
	}

	getListPaged() {
		this.newsService.getAllPagedList(this.query).subscribe((res) => {
			if (res.success != 'OK') return
			this.listDataPaged = res.result.NENewsGetAllOnPage
			if (this.totalCount <= 0) this.totalCount = res.result.TotalCount
			this.totalCount = Math.ceil(this.totalCount / this.query.pageSize)
		})
	}

	modalConfirm_message = 'Anh/chị có chắc chắn thực hiện hành động này?'
	modalConfirm_type = 'delete'
	modalConfirm_item_id = 0
	onOpenModalConfirm(id: number, type: string) {
		$('#modal-confirm').modal('show')
		this.modalConfirm_type = type
		this.modalConfirm_item_id = id
	}
	acceptConfirm() {
		let item = this.listDataPaged.find((c) => c.id == this.modalConfirm_item_id)
		if (this.modalConfirm_type == 'delete') {
			this.newsService.delete(item).subscribe((res) => {
				if (res.success != 'OK') {
					this.toast.error(COMMONS.DELETE_FAILED)
					return
				}
				this.toast.success(COMMONS.DELETE_SUCCESS)
				this.getListPaged()
			})
		} else if (this.modalConfirm_type == 'publish') {
			item.isPublished = !item.isPublished
			this.newsService.update(item).subscribe((res) => {
				if (res.success != 'OK') {
					this.toast.error(COMMONS.DELETE_FAILED)
					return
				}
				this.toast.success(COMMONS.DELETE_SUCCESS)
			})
		}
	}

	filterChange() {
		this.getListPaged()
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
