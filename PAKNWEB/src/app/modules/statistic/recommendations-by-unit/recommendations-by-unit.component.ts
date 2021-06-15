import { Component, OnInit, ViewChild, ElementRef } from '@angular/core'
import { FormGroup, FormBuilder, Validators } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'

import { defineLocale } from 'ngx-bootstrap/chronos'
import { viLocale } from 'ngx-bootstrap/locale'
import { BsDatepickerConfig, BsLocaleService } from 'ngx-bootstrap/datepicker'

import { Router, ActivatedRoute } from '@angular/router'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { CONSTANTS, STATUS_HISNEWS, FILETYPE } from 'src/app/constants/CONSTANTS'
import { COMMONS } from 'src/app/commons/commons'
import { StatisticService } from 'src/app/services/statistic.service'
import { UnitService } from 'src/app/services/unit.service'
import { DataService } from 'src/app/services/sharedata.service'

declare var $: any
defineLocale('vi', viLocale)

@Component({
	selector: 'app-recommentdation-by-unit',
	templateUrl: './recommendations-by-unit.component.html',
	styleUrls: ['./recommendations-by-unit.component.css'],
})
export class RecommendationsByUnitComponent implements OnInit {
	@ViewChild('table', { static: false }) table: any

	pageIndex: Number = 1
	pageSize: Number = 20
	totalRecords: Number
	listData: any[]
	fromDate: any
	toDate: any
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

	constructor(
		private _toastr: ToastrService,
		private router: Router,
		private activatedRoute: ActivatedRoute,
		private BsLocaleService: BsLocaleService,
		private _service: StatisticService,
		private unitService: UnitService,
		private _shareData: DataService
	) {
		this.year = new Date().getFullYear()
		this.listUnitSelected = []
	}
	ngAfterViewInit() {}
	ngOnInit() {
		this.BsLocaleService.use('vi')
		this.fromDate = new Date(this.year, 0, 1)
		let tmp_date = new Date(this.year + 1, 0, 1)
		this.toDate = this.minusDays(tmp_date, 1)
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

	changeYear() {
		if (this.year != null) {
			this.fromDate = new Date(this.year, 0, 1)
			let tmp_date = new Date(this.year + 1, 0, 1)
			this.toDate = this.minusDays(tmp_date, 1)
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
			this.dataStateChange()
		}
	}

	getList() {
		if (this.listUnitSelected != null && this.listUnitSelected.length > 0) {
			this.ltsUnitId = this.listUnitSelected.reduce((x, y) => {
				return (x += y.unitId + ',')
			}, '')
		} else {
			if(!this.ltsUnitIdAll){
				return
			}
			this.ltsUnitId = this.ltsUnitIdAll
		}
		let obj = {
			PageIndex: this.pageIndex == null ? 1 : this.pageIndex,
			PageSize: this.pageSize == null ? 20 : this.pageSize,
			LtsUnitId: this.ltsUnitId,
			Year: this.year,
			Timeline: this.timeline == null ? '' : this.timeline,
			FromDate: this.fromDate == null ? '' : this.getFormattedDate(this.fromDate),
			ToDate: this.toDate == null ? '' : this.getFormattedDate(this.toDate),
		}
		this._service.getStatisticRecommendationByUnit(obj).subscribe((res) => {
			if (res.success != RESPONSE_STATUS.success) {
				this.listData = []
				this.totalRecords = 0
				this.pageIndex = 1
				this.pageSize = 20
				return
			} else {
				if (res.result.StatisticRecommendationByUnitGetAllOnPage.length > 0) {
					this.listData = res.result.StatisticRecommendationByUnitGetAllOnPage
					this.totalRecords = res.result.TotalCount
				} else {
					this.listData = []
					this.pageIndex = 1
					this.pageSize = 20
				}
			}
		})
	}

	onPageChange(event: any) {
		this.pageSize = event.rows
		this.pageIndex = event.first / event.rows + 1
		this.getList()
	}

	dataStateChange() {
		this.pageIndex = 1
		this.table.first = 0
		this.getList()
	}

	fromDateChange(data) {
		if(data){
			this.getList()
		}
	}
	toDateChange(data) {
		if(data){
			this.getList()
		}
	}
	viewDetail(unitId : any , status : any = null) {
		if(status){
			return this.router.navigate([
				'/quan-tri/bao-cao/phan-anh-kien-nghi-theo-don-vi-chi-tiet',
				unitId,
				this.getFormattedDate(this.fromDate),
				this.getFormattedDate(this.toDate),
				status
			])
		}
		return this.router.navigate([
			'/quan-tri/bao-cao/phan-anh-kien-nghi-theo-don-vi-chi-tiet',
			unitId,
			this.getFormattedDate(this.fromDate),
			this.getFormattedDate(this.toDate),
		])
	}

	onExport() {
		let passingObj: any = {}
		// passingObj.UnitId = this.query.parentId
		// passingObj.UnitName = this.unitObject.name

		passingObj.PageIndex = this.pageIndex == null ? 1 : this.pageIndex
		passingObj.PageSize = this.pageSize == null ? 20 : this.pageSize
		passingObj.LtsUnitId = this.ltsUnitId
		passingObj.Year = this.year
		passingObj.Timeline = this.timeline == null ? '' : this.timeline
		passingObj.FromDate = this.fromDate == null ? '' : (this.fromDate = JSON.stringify(new Date(this.fromDate)).slice(1, 11))
		passingObj.ToDate = this.toDate == null ? '' : (this.toDate = JSON.stringify(new Date(this.toDate)).slice(1, 11))
		this._shareData.setobjectsearch(passingObj)
		this._shareData.sendReportUrl = 'recommendation_by_unit?' + JSON.stringify(passingObj)
		this.router.navigate(['quan-tri/xuat-file'])
	}
}
