import { Component, OnInit, ViewChild, ElementRef } from '@angular/core'
import { FormGroup, FormBuilder, Validators } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'

import { defineLocale } from 'ngx-bootstrap/chronos'
import { viLocale } from 'ngx-bootstrap/locale'
import { BsDatepickerConfig, BsLocaleService } from 'ngx-bootstrap/datepicker'

import { Router, ActivatedRoute } from '@angular/router'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { StatisticService } from 'src/app/services/statistic.service'
import { UnitService } from 'src/app/services/unit.service'
import { DataService } from 'src/app/services/sharedata.service'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'

declare var $: any
defineLocale('vi', viLocale)
@Component({
	selector: 'app-recommendations-by-groupword',
	templateUrl: './recommendations-by-groupword.component.html',
	styleUrls: ['./recommendations-by-groupword.component.css'],
})
export class RecommendationsByGroupwordComponent implements OnInit {
	totalRecords: Number
	frozenCols: any = [{ field: 'text', header: 'Đơn vị', cssClass: 'text-left' }]
	scrollableCols: any = []
	lstData: any = []
	fromDate: Date
	toDate: Date
	year: any
	timeline: any

	lstTimeline: any = [
		{ value: 1, text: 'Quý I' },
		{ value: 2, text: 'Quý II' },
		{ value: 3, text: 'Quý III' },
		{ value: 4, text: 'Quý IV' },
		{ value: 5, text: '6 tháng đầu năm' },
		{ value: 6, text: '6 tháng cuối năm' },
	]
	listYear: any = [
		{ value: 2020, text: '2020' },
		{ value: 2021, text: '2021' },
		{ value: 2022, text: '2022' },
		{ value: 2023, text: '2023' },
	]
	listUnit: any[]
	listUnitSelected: any[]
	ltsUnitId: any
	ltsUnitIdAll: any
	minDate: Date
	maxDate: Date

	constructor(
		private _toastr: ToastrService,
		private router: Router,
		private activatedRoute: ActivatedRoute,
		private BsLocaleService: BsLocaleService,
		private _service: StatisticService,
		private unitService: UnitService,
		private _shareData: DataService,
		private storeageService: UserInfoStorageService
	) {
		this.year = new Date().getFullYear()
		this.listUnitSelected = []
	}
	minusDays(date: Date, days: number): Date {
		date.setDate(date.getDate() - days)
		return date
	}
	getFormattedDate(date) {
		var year = date.getFullYear()
		var month = (1 + date.getMonth()).toString()
		month = month.length > 1 ? month : '0' + month
		var day = date.getDate().toString()
		day = day.length > 1 ? day : '0' + day
		return month + '-' + day + '-' + year
	}
	ngOnInit() {
		this.BsLocaleService.use('vi')

		const currentMonth = new Date().getMonth()
		switch (currentMonth) {
			case 1:
			case 2:
			case 3:
				this.timeline = 1
				break
			case 4:
			case 5:
			case 6:
				this.timeline = 2
				break
			case 7:
			case 8:
			case 9:
				this.timeline = 3
				break
			case 10:
			case 11:
			case 12:
				this.timeline = 4
				break
		}
		this.changeTimeLine()

		this.unitService.getChildrenDropdown().subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				this.listUnit = res.result
				this.ltsUnitId = this.ltsUnitIdAll = this.listUnit.reduce((x, y) => {
					return (x += y.unitId + ',')
				}, '')
				this.getList()
			} else {
				this.listUnit = []
			}
		})
	}
	changeYear() {
		if (this.year != null) {
			this.fromDate = new Date(this.year, 0, 1)
			let tmp_date = new Date(this.year + 1, 0, 1)
			this.toDate = this.minusDays(tmp_date, 1)
			this.minDate = this.fromDate
			this.maxDate = this.toDate
			this.dataStateChange()
		}
	}
	changeTimeLine() {
		if (this.year != null) {
			if (this.timeline == 1) {
				this.fromDate = new Date(this.year, 0, 1)
				let tmp_date = new Date(this.year, 3, 1)
				this.toDate = this.minusDays(tmp_date, 1)
			} else if (this.timeline == 2) {
				this.fromDate = new Date(this.year, 3, 1)
				let tmp_date = new Date(this.year, 6, 1)
				this.toDate = this.minusDays(tmp_date, 1)
			} else if (this.timeline == 3) {
				this.fromDate = new Date(this.year, 6, 1)
				let tmp_date = new Date(this.year, 9, 1)
				this.toDate = this.minusDays(tmp_date, 1)
			} else if (this.timeline == 4) {
				this.fromDate = new Date(this.year, 9, 1)
				let tmp_date = new Date(this.year + 1, 0, 1)
				this.toDate = this.minusDays(tmp_date, 1)
			} else if (this.timeline == 5) {
				this.fromDate = new Date(this.year, 0, 1)
				let tmp_date = new Date(this.year, 6, 1)
				this.toDate = this.minusDays(tmp_date, 1)
			} else if (this.timeline == 6) {
				this.fromDate = new Date(this.year, 6, 1)
				let tmp_date = new Date(this.year + 1, 0, 1)
				this.toDate = this.minusDays(tmp_date, 1)
			}
		}
		this.minDate = this.fromDate
		this.maxDate = this.toDate
	}

	getList() {
		if (this.listUnitSelected != null && this.listUnitSelected.length > 0) {
			this.ltsUnitId = this.listUnitSelected.reduce((x, y) => {
				return (x += y.unitId + ',')
			}, '')
		} else {
			if (!this.ltsUnitIdAll) {
				return
			}
			this.ltsUnitId = this.ltsUnitIdAll
		}
		let obj = {
			LtsUnitId: this.ltsUnitId,
			FromDate: this.fromDate == null ? '' : this.getFormattedDate(this.fromDate),
			ToDate: this.toDate == null ? '' : this.getFormattedDate(this.toDate),
		}
		this._service.getStatisticRecommendationByGroupWord(obj).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				var data = res.result
				this.scrollableCols = []
				this.lstData = []
				for (var i = 0; i < data.ListData.length; i++) {
					this.scrollableCols.push({
						field: data.ListData[i].groupWordId,
						header: data.ListData[i].groupWordName,
						codeHeader: data.ListData[i].groupWordName,
						giaTri: data.ListData[i].total,
						cssClass: 'text-right',
					})
				}
				for (var a = 0; a < data.ListUnits.length; a++) {
					var dataDT = data.ListUnits[a]
					for (var b = 0; b < this.scrollableCols.length; b++) {
						for (var c = 0; c < data.ListData.length; c++) {
							if (dataDT.value == data.ListData[c].unitId && this.scrollableCols[b].field == data.ListData[c].groupWordId) {
								dataDT[this.scrollableCols[b].field] = data.ListData[c].total
							}
						}
					}
					this.lstData.push(dataDT)
				}
			}
		})
	}

	dataStateChange() {
		this.changeTimeLine()
		this.getList()
	}

	fromDateChange(data) {
		if (data) {
			this.fromDate = data
		}
		this.getList()
	}
	toDateChange(data) {
		if (data) {
			this.toDate = data
		}
		this.getList()
	}
	viewDetail(unitId, groupWordId) {
		return this.router.navigate([
			'/quan-tri/bao-cao/phan-anh-kien-nghi-theo-nhom-tu-ngu-chi-tiet',
			unitId,
			groupWordId,
			this.getFormattedDate(this.fromDate),
			this.getFormattedDate(this.toDate),
		])
	}

	onExport() {
		let passingObj: any = {}
		passingObj.TitleReport = 'Thống kê Phản ánh kiến nghị theo nhóm từ'
		passingObj.LstUnitId = this.ltsUnitId
		passingObj.FromDate = this.fromDate == null ? '' : JSON.stringify(new Date(this.fromDate)).slice(1, 11)
		passingObj.ToDate = this.toDate == null ? '' : JSON.stringify(new Date(this.toDate)).slice(1, 11)
		passingObj.UserProcessId = this.storeageService.getUserId()
		passingObj.UserProcessName = this.storeageService.getFullName()
		this._shareData.setobjectsearch(passingObj)
		this._shareData.sendReportUrl = 'phan-anh-kien-nghi-theo-nhom-tu-ngu?' + JSON.stringify(passingObj)
		this.router.navigate(['quan-tri/xuat-file'])
	}
}
