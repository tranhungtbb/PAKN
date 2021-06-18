import { Component, OnInit } from '@angular/core'
import { CallHistoryService } from 'src/app/services/call-history.service'

@Component({
	selector: 'app-call-history-list',
	templateUrl: './call-history-list.component.html',
	styleUrls: ['./call-history-list.component.css'],
})
export class CallHistoryListComponent implements OnInit {
	constructor(private _CallHistoryService: CallHistoryService) {}

	callTypes = [
		{ value: null, text: 'Tất cả cuộc gọi' },
		{ value: 0, text: 'Cuộc gọi đến' },
		{ value: 1, text: 'Cuộc gọi đi' },
		{ value: 2, text: 'Cuộc gọi nhỡ' },
	]

	listData: any[] = []

	query = {
		type: null,
		phone: '',
		pageIndex: 1,
		pageSize: 20,
	}

	totalCount = 0

	ngOnInit() {}

	private getDataPageList() {
		let query = { ...this.query }
		this.query.type = query.type == null ? '' : query.type

		this._CallHistoryService.getPagedList(this.query).subscribe((res) => {
			if (res) {
				this.listData = res.result.ListData
				if (this.listData[0].rowNumber) {
					this.totalCount = this.listData[0].rowNumber
				}
			}
		})
	}
	filterChange(event: any, key: string) {
		this.query[key] = event.value
		this.getDataPageList()
	}
	onPageChange(event: any): void {
		this.query.pageSize = event.rows
		this.query.pageIndex = event.first / event.rows + 1
		this.getDataPageList()
	}
}
