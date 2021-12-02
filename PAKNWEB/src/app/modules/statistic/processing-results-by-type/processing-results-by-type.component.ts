import { Component, OnInit, ViewChild } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { DataService } from 'src/app/services/sharedata.service'
import { Router } from '@angular/router'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { StatisticService } from 'src/app/services/statistic.service'

declare var $: any
@Component({
	selector: 'app-statistic-processing-results-by-type',
	templateUrl: './processing-results-by-type.component.html',
	styleUrls: ['./processing-results-by-type.component.css'],
})
export class ProcessingResultsByTypeComponent implements OnInit {
	constructor(
		private _service: StatisticService,
		private _router: Router,
		private _localService: UserInfoStorageService,
		private _toastr: ToastrService,
		private _shareData: DataService
	) {}

	listYear: any = []
	listType: any = [
		{ value: 1, text: 'Lĩnh vực' },
		{ value: 2, text: 'Đơn vị' }
	]
	// list active status
	listQuarter: any = [
		{ value: 1, text: 'Quý I' },
		{ value: 2, text: 'Quý II' },
		{ value: 3, text: 'Quý III' },
		{ value: 4, text: 'Quý IV' },
	]
	year: number
	quarter: number
	fromDate: Date
	toDate: Date
	type : number = 1
	totalRecords : number
	listData : any [] = []
	pageIndex: number = 1
	pageSize: number = 10

	maxDateValue = new Date()

	ngOnInit() {
		this.initData()
	}

	ngAfterViewInit() {
		this._shareData.seteventnotificationDropdown()
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
				this.fromDate = new Date(this.year, 0, 1)
				let tmp_date = new Date(this.year, 3, 1)
				this.toDate = this.minusDays(tmp_date, 1)
			} else if (this.quarter == 2) {
				this.fromDate = new Date(this.year, 3, 1)
				let tmp_date = new Date(this.year, 6, 1)
				this.toDate = this.minusDays(tmp_date, 1)
			} else if (this.quarter == 3) {
				this.fromDate = new Date(this.year, 6, 1)
				let tmp_date = new Date(this.year, 9, 1)
				this.toDate = this.minusDays(tmp_date, 1)
			} else if (this.quarter == 4) {
				this.fromDate = new Date(this.year, 9, 1)
				let tmp_date = new Date(this.year + 1, 0, 1)
				this.toDate = this.minusDays(tmp_date, 1)
			} else {
				this.fromDate = new Date(this.year, 0, 1)
				let tmp_date = new Date(this.year + 1, 0, 1)
				this.toDate = this.minusDays(tmp_date, 1)
			}
		}
		this.getList()
	}

	getList() {
		let request = {
			FromDate: this.fromDate == null ? '' : this.fromDate.toDateString(),
			ToDate: this.toDate == null ? '' : this.toDate.toDateString(),
			Type : this.type,
			PageSize: this.pageSize,
			PageIndex: this.pageIndex
		}
		this._service.getProcessingResultByType(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				this.listData = response.result;
				if(response.result && response.result.length > 0){
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
			this.fromDate = value
		} else {
			this.fromDate = null
		}
		this.getList()
	}

	toDateValueChange(value: Date): void {
		if (value) {
			this.toDate = value
		} else {
			this.toDate = null
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

	onExport() {
		let passingObj: any = {}
		if(this.fromDate){
			this.fromDate.setHours(0,0,0,0);
		}
		if(this.toDate){
			this.toDate.setHours(0,0,0,0);
		}
		passingObj.FromDate = this.fromDate;
		passingObj.ToDate = this.toDate;
		this._shareData.setobjectsearch(passingObj)
		this._shareData.sendReportUrl = this.type == 1 ? 
			'processing_results_by_feild?' + JSON.stringify(passingObj)
			: 'processing_results_by_unit?' + JSON.stringify(passingObj)
		this._router.navigate(['quan-tri/xuat-file'])
	}
}
