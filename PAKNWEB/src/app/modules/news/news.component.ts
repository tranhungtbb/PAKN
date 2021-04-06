import { Component, OnInit, Input } from '@angular/core'
import { DomSanitizer } from '@angular/platform-browser'
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
//acbd
export class NewsComponent implements OnInit {
	constructor(private newsService: NewsService, private catalogService: CatalogService, private toast: ToastrService, private sanitizer: DomSanitizer) {}

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
		this.newsService
			.getAllPagedList({
				pageIndex: this.query.pageIndex,
				pageSize: this.query.pageSize,
				title: this.query.title,
				newsType: this.query.newsType == null ? '' : this.query.newsType,
				status: this.query.status == null ? '' : this.query.status,
			})
			.subscribe((res) => {
				if (res.success != 'OK') return
				this.listDataPaged = res.result.NENewsGetAllOnPage
				this.totalCount = res.result.TotalCount
				// load image
				this.getNewsAvatars()
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
			if (item.isPublished) item.status = 1
			else item.status = 0
			this.newsService.update(item).subscribe((res) => {
				if (res.success != 'OK') {
					this.toast.error('Xảy ra lỗi trong quá trình xử lý')
					return
				}

				this.toast.success(item.isPublished ? 'Đã công bố' : 'Đã thu hồi')
			})
		}
	}

	filterChange() {
		this.getListPaged()
	}

	onPageChange(event: any): void {
		this.query.pageSize = event.rows
		this.query.pageIndex = event.first / event.rows + 1
		this.getListPaged()
	}

	// getCateName(id): string {
	// 	return this.listNewCategories.find((c) => c.id == id).name
	// }

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
