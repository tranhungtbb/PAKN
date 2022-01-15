import { Component, OnInit } from '@angular/core'
import { CallHistoryService } from 'src/app/services/call-history.service'
import { ToastrService } from 'ngx-toastr'

declare var $: any

@Component({
	selector: 'app-call-history-list',
	templateUrl: './call-history-list.component.html',
	styleUrls: ['./call-history-list.component.css'],
})
export class CallHistoryListComponent implements OnInit {
	constructor(private _CallHistoryService: CallHistoryService, private _toast: ToastrService) { }

	callTypes = [
		{ value: 0, text: 'Cuộc gọi đến' },
		{ value: 1, text: 'Cuộc gọi đi' },
		{ value: 2, text: 'Cuộc gọi nhỡ' },
	]

	listData: any[] = []

	query: any = {
		type: null,
		phone: '',
		pageIndex: 1,
		pageSize: 20,
	}
	type: number

	totalCount = 0

	ngOnInit() {
		this.getDataPageList()
	}

	getDataPageList() {
		this.query.phone = this.query.phone == null ? '' : this.query.phone.trim()
		this.query.type = this.type == null ? '' : this.type
		let query = { ...this.query }

		this._CallHistoryService.getPagedList(query).subscribe((res) => {
			if (res) {
				this.listData = res.result.ListData
				this.totalCount = this.listData[0] ? this.listData[0].rowNumber : 0
			}
		})
	}
	onPageChange(event: any): void {
		this.query.pageSize = event.rows
		this.query.pageIndex = event.first / event.rows + 1
		this.getDataPageList()
	}

	activeItem: any = {}
	confirm(item: any, dialog = true) {
		if (dialog) {
			this.activeItem = item
			$('#modalConfirm').modal('show')
			return
		}

		this._CallHistoryService.delete(this.activeItem.id).subscribe((res) => {
			if (res && res.success == 'OK') {
				this._toast.success('Xóa thành công')
				$('#modalConfirm').modal('hide')
				this.getDataPageList()
				return
			}
		})
	}
}
