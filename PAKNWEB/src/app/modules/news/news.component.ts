import { Component, OnInit, ViewChild } from '@angular/core'
import { DomSanitizer } from '@angular/platform-browser'
import { ToastrService } from 'ngx-toastr'
import { ActivatedRoute } from '@angular/router'

import { NewsService } from 'src/app/services/news.service'
import { CatalogService } from 'src/app/services/catalog.service'
import { RESPONSE_STATUS, STATUS_HISNEWS } from 'src/app/constants/CONSTANTS'

import { COMMONS } from 'src/app/commons/commons'
import { NewsModel, HISNewsModel } from 'src/app/models/NewsObject'
declare var $: any
@Component({
	selector: 'app-news',
	templateUrl: './news.component.html',
	styleUrls: ['./news.component.css'],
})
//acbd
export class NewsComponent implements OnInit {
	constructor(
		private newsService: NewsService,
		private catalogService: CatalogService,
		private activatedRoute: ActivatedRoute,
		private toast: ToastrService,
		private sanitizer: DomSanitizer
	) {}
	hisNewsModel: HISNewsModel = new HISNewsModel()
	query: any = {
		pageSize: 20,
		pageIndex: 1,
		title: '',
		status: null,
		newsType: null,
	}

	listNewCategories: any[] = []

	listDataPaged: any[] = []
	listStatus: any[] = [
		{ value: 2, text: 'Đang soạn thảo' },
		{ value: 1, text: 'Đã công bố' },
		{ value: 0, text: 'Hủy công bố' },
	]
	totalCount: number = 0
	pageCount: number = 0
	listHisNews: any[]
	@ViewChild('table', { static: false }) table: any

	ngOnInit() {
		this.activatedRoute.params.subscribe((params) => {
			if (params['pageIndex']) {
				this.query.pageIndex = Number(params['pageIndex'])
			}
			this.getListPaged()
		})
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
	checkPageIndex: any = false
	getListPaged() {
		this.checkPageIndex = true
		this.query.title = this.query.title == null ? '' : this.query.title.trim()
		this.newsService
			.getAllPagedList({
				pageIndex: this.query.pageIndex,
				pageSize: this.query.pageSize,
				title: this.query.title,
				newsType: this.query.newsType == null ? '' : this.query.newsType,
				status: this.query.status == null ? '' : this.query.status,
			})
			.subscribe((res) => {
				if (res.success != 'OK') {
					this.listDataPaged = []
					this.totalCount = 0
					this.query.pageSize = 20
					this.query.pageIndex = 1
				} else {
					if (res.result.NENewsGetAllOnPage.length > 0) {
						this.listDataPaged = res.result.NENewsGetAllOnPage
						if (res.result.TotalCount) this.totalCount = res.result.TotalCount
					} else {
						this.listDataPaged = []
						this.totalCount = 0
						this.query.pageSize = 20
						this.query.pageIndex = 1
					}
				}
			})
	}

	getHisNewsListByNewsId(id: any) {
		this.newsService.getListHisNewsByNewsId({ NewsId: id }).subscribe((res) => {
			if ((res.success = RESPONSE_STATUS.success)) {
				this.listHisNews = res.result
			}
			return
		})
	}

	getHistory(id: any) {
		this.getHisNewsListByNewsId(id)
		$('#modal-history').modal('show')
	}

	modalConfirm_message = 'Anh/Chị có chắc chắn muốn xóa bài viết này?'
	modalConfirm_type = 'delete'
	modalConfirm_item_id = 0
	onOpenModalConfirm(id: number, type: string) {
		if (type == 'delete') {
			this.modalConfirm_message = 'Anh/Chị có chắc chắn muốn xóa bài viết này?'
		} else {
			let item = this.listDataPaged.find((c) => c.id == id)
			if (item.isPublished == false) {
				this.modalConfirm_message = 'Anh/Chị có muốn công bố bài viết này?'
			} else {
				this.modalConfirm_message = 'Anh/Chị có muốn hủy công bố bài viết này?'
			}
		}
		$('#modal-confirm').modal('show')

		this.modalConfirm_type = type
		this.modalConfirm_item_id = id
	}
	acceptConfirm() {
		let item = this.listDataPaged.find((c) => c.id == this.modalConfirm_item_id)
		if (this.modalConfirm_type == 'delete') {
			this.newsService.delete({ id: item.id }, item.title).subscribe((res) => {
				if (res.success != 'OK') {
					this.toast.error(COMMONS.DELETE_FAILED)
					return
				}
				this.table.reset()
				this.query.pageSize = 20
				this.query.pageIndex = 1
				this.toast.success(COMMONS.DELETE_SUCCESS)
				this.getListPaged()
			})
		} else if (this.modalConfirm_type == 'publish') {
			item.isPublished = !item.isPublished
			// if (item.isPublished) item.status = 1
			// else item.status = 0
			item.status = item.status == 1 ? 0 : 1
			this.newsService.changeStatus({ NewsId: item.id, Status: item.status }, item.title).subscribe((res) => {
				if (res.success != 'OK') {
					this.toast.error('Xảy ra lỗi trong quá trình xử lý')
					return
				}
				this.toast.success(item.isPublished ? 'Đã công bố' : 'Đã hủy công bố')
			})
		}
		$('#modal-confirm').modal('hide')
	}

	filterChange() {
		this.getListPaged()
	}

	onPageChange(event: any): void {
		this.query.pageSize = event.rows
		this.query.pageIndex = event.first / event.rows + 1
		this.getListPaged()
	}
}
