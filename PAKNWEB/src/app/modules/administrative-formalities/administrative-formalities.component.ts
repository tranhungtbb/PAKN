import { Component, OnInit, Input } from '@angular/core'
import { DomSanitizer } from '@angular/platform-browser'
import { ToastrService } from 'ngx-toastr'

import { AdministrativeFormalitiesService } from 'src/app/services/administrative-formalities.service'
import { CatalogService } from 'src/app/services/catalog.service'

import { COMMONS } from 'src/app/commons/commons'
import { AdministrativeFormalitiesObject } from 'src/app/models/AdministrativeFormalitiesObject'
declare var $: any
@Component({
	selector: 'app-administrative-formalities',
	templateUrl: './administrative-formalities.component.html',
	styleUrls: ['./administrative-formalities.component.css'],
})
//acbd
export class AdministrativeFormalitiesComponent implements OnInit {
	constructor(private administrativeFormalitiesService: AdministrativeFormalitiesService, private catalogService: CatalogService, private toast: ToastrService, private sanitizer: DomSanitizer) { }

	query: any = {
		pageSize: 20,
		pageIndex: 1,
		title: '',
		status: '',
		newsType: '',
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
				this.listNewCategories = res.result.CAAdministrativeFormalitiesTypeGetAllOnPage
			})
	}

	getListPaged() {

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
			this.administrativeFormalitiesService.delete({ id: item.id }).subscribe((res) => {
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
			this.administrativeFormalitiesService.update(item).subscribe((res) => {
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
}
