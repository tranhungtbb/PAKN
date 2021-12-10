import { Component, OnInit, ViewChild } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { DataService } from 'src/app/services/sharedata.service'
import { Router } from '@angular/router'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { StatisticService } from 'src/app/services/statistic.service'

declare var $: any
@Component({
	selector: 'app-statistic-processing-results-by-reception-type',
	templateUrl: './processing-results-by-reception-type.component.html',
	styleUrls: ['./processing-results-by-reception-type.component.css'],
})
export class ProcessingResultsByReceptionTypeComponent implements OnInit {
	constructor(
		private _service: StatisticService,
		private _router: Router,
		private _localService: UserInfoStorageService,
		private _toastr: ToastrService,
		private _shareData: DataService
	) { }

	listYear: any = []

	// list active status
	listQuarter: any = [
		{ value: 1, text: 'Quý I' },
		{ value: 2, text: 'Quý II' },
		{ value: 3, text: 'Quý III' },
		{ value: 4, text: 'Quý IV' },
	]
	listType: any = [
		{ value: 1, text: 'Lĩnh vực' },
		{ value: 2, text: 'Đơn vị' }
	]
	year: number
	quarter: number
	fromDate: Date
	toDate: Date
	fromDateSearch: Date
	toDateSearch: Date
	totalRecords: number
	listData: any[] = []
	pageIndex: number = 1
	pageSize: number = 10
	type: number = 1

	maxDateValue = new Date()
	firstLoad = true
	ngOnInit() {
		this.initData()
	}

	ngAfterViewInit() {
		this._shareData.seteventnotificationDropdown()
		this.firstLoad = true
	}

	initData() {
		let currentTime = new Date()
		this.year = currentTime.getFullYear()
		var curMonth = currentTime.getMonth() + 1
		for (var i = this.year; i >= this.year - 5; i--) {
			this.listYear.push({ value: i, text: 'Năm ' + i })
		}
		if (1 <= curMonth && curMonth <= 3) {
			this.quarter = 1
		} else if (4 <= curMonth && curMonth <= 6) {
			this.quarter = 2
		} else if (7 <= curMonth && curMonth <= 9) {
			this.quarter = 3
		} else {
			this.quarter = 4
		}
		this.changeQuarter()
	}

	minusDays(date: Date, days: number): Date {
		date.setDate(date.getDate() - days)
		return date
	}
	changeYear() {
		if (this.year != null) {
			this.fromDate = new Date(this.year, 0, 1)
			let tmp_date = new Date(this.year + 1, 0, 1)
			this.toDate = this.minusDays(tmp_date, 1)
			this.getList()
		}
	}
	changeQuarter() {
		if (this.year != null) {
			if (this.quarter == 1) {
				this.fromDateSearch = new Date(this.year, 0, 1)
				let tmp_date = new Date(this.year, 3, 1)
				this.toDateSearch = this.minusDays(tmp_date, 1)
			} else if (this.quarter == 2) {
				this.fromDateSearch = new Date(this.year, 3, 1)
				let tmp_date = new Date(this.year, 6, 1)
				this.toDateSearch = this.minusDays(tmp_date, 1)
			} else if (this.quarter == 3) {
				this.fromDateSearch = new Date(this.year, 6, 1)
				let tmp_date = new Date(this.year, 9, 1)
				this.toDateSearch = this.minusDays(tmp_date, 1)
			} else if (this.quarter == 4) {
				this.fromDateSearch = new Date(this.year, 9, 1)
				let tmp_date = new Date(this.year + 1, 0, 1)
				this.toDateSearch = this.minusDays(tmp_date, 1)
			} else {
				this.fromDateSearch = new Date(this.year, 0, 1)
				let tmp_date = new Date(this.year + 1, 0, 1)
				this.toDateSearch = this.minusDays(tmp_date, 1)
			}
		}
		this.getList()
	}

	getList() {
		if (!this.firstLoad)
			return
		let request = {
			Type: this.type,
			FromDate: this.fromDateSearch == null ? '' : this.fromDateSearch.toDateString(),
			ToDate: this.toDateSearch == null ? '' : this.toDateSearch.toDateString(),
			PageSize: this.pageSize,
			PageIndex: this.pageIndex
		}
		this._service.getProcessingResultByReceptionType(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				this.listData = response.result;
				if (response.result && response.result.length > 0) {
					this.totalRecords = response.result[0].rowNumber;
				}
			} else {
			}
		}),
			(error) => {
				console.log(error)
				alert(error)
			}
	}

	dataStateChange() {
		this.getList()
	}

	fromDateValueChange(value: any): void {
		if (value) {
			this.fromDateSearch = value
		}
		this.getList()
	}

	toDateValueChange(value: Date): void {
		if (value) {
			this.toDateSearch = value
		}
		this.getList()
	}

	parseDate = function (value: any) {
		if (value) {
			var currentTime = new Date(value)
			var month = (currentTime.getMonth() + 1).toString()
			var day = currentTime.getDate().toString()
			var year = currentTime.getFullYear()
			if (parseInt(day) < 10) {
				day = '0' + day
			}
			if (parseInt(month) < 10) {
				month = '0' + month
			}
			var date = year + '-' + month + '-' + day
			return date
		}
		return null
	}

	onPageChange(event: any) {
		this.pageSize = event.rows
		this.pageIndex = event.first / event.rows + 1
		this.getList()
	}

	getFormattedDate(date) {
		var year = date.getFullYear()
		var month = (1 + date.getMonth()).toString()
		month = month.length > 1 ? month : '0' + month
		var day = date.getDate().toString()
		day = day.length > 1 ? day : '0' + day
		return month + '-' + day + '-' + year
	}


	viewDetail(id: any, receptionType: number = null) {
		var url
		if (this.type == 1) {
			if (receptionType) {
				url =
					'/quan-tri/bao-cao/chi-tiet-ket-qua-xu-ly-theo-pttn-va-linh-vuc/' +
					this.type + '/' +
					id, receptionType + '/' +
					this.getFormattedDate(this.fromDate) + '/' +
					this.getFormattedDate(this.toDate)
			} else {
				url =
					'/quan-tri/bao-cao/chi-tiet-ket-qua-xu-ly-theo-pttn-va-linh-vuc/' +
					this.type + '/' +
					id + '/' +
					this.getFormattedDate(this.fromDate) + '/' +
					this.getFormattedDate(this.toDate)
			}
		} else {
			if (receptionType) {
				url =
					'/quan-tri/bao-cao/chi-tiet-ket-qua-xu-ly-theo-pttn-va-don-vi/' +
					this.type + '/' +
					id + '/' + receptionType + '/' +
					this.getFormattedDate(this.fromDate) + '/' +
					this.getFormattedDate(this.toDate)
			} else {
				url =
					'/quan-tri/bao-cao/chi-tiet-ket-qua-xu-ly-theo-pttn-va-don-vi/' +
					this.type + '/' +
					id + '/' +
					this.getFormattedDate(this.fromDate) + '/' +
					this.getFormattedDate(this.toDate)
			}
		}
		setTimeout(() => {
			window.open(url, '_blank')
		}, 100)
	}
	viewResultDetail(id: any, status: number, isOnTime: number = null) {
		var url
		if (this.type == 1) {
			if (isOnTime) {
				url =
					'/quan-tri/bao-cao/chi-tiet-ket-qua-xu-ly-linh-vuc/' +
					this.type + '/' +
					id + '/' + status + '/' + isOnTime + '/' +
					this.getFormattedDate(this.fromDate) + '/' +
					this.getFormattedDate(this.toDate)
			} else {
				url =
					'/quan-tri/bao-cao/chi-tiet-ket-qua-xu-ly-linh-vuc/' +
					this.type + '/' +
					id + '/' + status + '/' +
					this.getFormattedDate(this.fromDate) + '/' +
					this.getFormattedDate(this.toDate)
			}
		}
		else {
			if (isOnTime) {
				url =
					'/quan-tri/bao-cao/chi-tiet-ket-qua-xu-ly-don-vi/' +
					this.type + '/' +
					id + '/' + status + '/' + isOnTime + '/' +
					this.getFormattedDate(this.fromDate) + '/' +
					this.getFormattedDate(this.toDate)
			} else {
				url =
					'/quan-tri/bao-cao/chi-tiet-ket-qua-xu-ly-don-vi/' +
					this.type + '/' +
					id + '/' + status + '/' +
					this.getFormattedDate(this.fromDate) + '/' +
					this.getFormattedDate(this.toDate)
			}
		}
		setTimeout(() => {
			window.open(url, '_blank')
		}, 100)
	}

	onExport() {
		let passingObj: any = {}
		if (this.fromDate) {
			this.fromDate.setHours(0, 0, 0, 0);
		}
		if (this.toDate) {
			this.toDate.setHours(0, 0, 0, 0);
		}
		passingObj.FromDate = this.fromDate;
		passingObj.ToDate = this.toDate;
		this._shareData.setobjectsearch(passingObj)
		this._shareData.sendReportUrl = this.type == 1 ?
			'processing_results_by_feild_and_reception?' + JSON.stringify(passingObj)
			: 'processing_results_by_unit_and_reception?' + JSON.stringify(passingObj)
		this._router.navigate(['quan-tri/xuat-file'])
	}
}
