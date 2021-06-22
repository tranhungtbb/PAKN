import { Component, OnInit, Input } from '@angular/core'
import { DomSanitizer } from '@angular/platform-browser'
import { ToastrService } from 'ngx-toastr'

import { NewsService } from 'src/app/services/news.service'
import { CatalogService } from 'src/app/services/catalog.service'
import { RESPONSE_STATUS, STATUS_HISNEWS } from 'src/app/constants/CONSTANTS'
import { AppSettings } from 'src/app/constants/app-setting'

import { COMMONS } from 'src/app/commons/commons'
import { NewsModel, HISNewsModel } from 'src/app/models/NewsObject'
declare var $: any
@Component({
	selector: 'app-news-puslish',
	templateUrl: './news-puslish.component.html',
	styleUrls: ['./news-puslish.component.css'],
})
//acbd
export class NewsPuslishComponent implements OnInit {
	constructor(private newsService: NewsService, private catalogService: CatalogService, private toast: ToastrService, private sanitizer: DomSanitizer) {}
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
				status: 1,
			})
			.subscribe((res) => {
				if (res.success != 'OK') return
				this.listDataPaged = res.result.NENewsGetAllOnPage.map((item) => {
					// item.imagePath = `${AppSettings.API_DOWNLOADFILES}/${item.imagePath}`
					return item
				})
				if (res.result.TotalCount) this.totalCount = res.result.TotalCount
				// load image
				//this.getNewsAvatars()
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
			this.newsService.delete({ id: item.id }).subscribe((res) => {
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
			this.newsService.changeStatus(item).subscribe((res) => {
				if (res.success != 'OK') {
					this.toast.error('Xảy ra lỗi trong quá trình xử lý')
					return
				}
				this.hisNewsModel.objectId = this.modalConfirm_item_id
				this.hisNewsModel.type = 1 // tin tức
				this.hisNewsModel.status = item.isPublished ? STATUS_HISNEWS.PUBLIC : STATUS_HISNEWS.CANCEL
				this.newsService.hisNewsCreate(this.hisNewsModel).subscribe((res) => console.log(res))
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
}
