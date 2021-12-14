import { Component, OnInit, ViewChild } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { DataService } from 'src/app/services/sharedata.service'
import { Router } from '@angular/router'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { StatisticService } from 'src/app/services/statistic.service'

declare var $: any
@Component({
	selector: 'app-statistic-processing-results',
	templateUrl: './processing-results.component.html',
	styleUrls: ['./processing-results.component.css'],
})
export class ProcessingResultsComponent implements OnInit {
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
		{ value: 1, text: 'Quý 1' },
		{ value: 2, text: 'Quý 2' },
		{ value: 3, text: 'Quý 3' },
		{ value: 4, text: 'Quý 4' },
	]
	year: number
	quarter: number
	fromDate: Date
	toDate: Date
	fromDateSearch: Date
	toDateSearch: Date
	totalRecords: number
	listData: any[] = []

	maxDateValue = new Date()
	firstLoad = false
	@ViewChild('myCanvas', { static: false }) canvas: any

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
			FromDate: this.fromDateSearch == null ? '' : this.fromDateSearch.toDateString(),
			ToDate: this.toDateSearch == null ? '' : this.toDateSearch.toDateString(),
		}
		this._service.getProcessingResult(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				this.listData = response.result
			} else {
				this._toastr.error(response.message)
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
		} else {
			this.fromDateSearch = null
		}
		this.getList()
	}

	toDateValueChange(value: Date): void {
		if (value) {
			this.toDateSearch = value
		} else {
			this.toDateSearch = null
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
}
