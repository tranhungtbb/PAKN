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

declare var $: any
defineLocale('vi', viLocale)

@Component({
	selector: 'app-recommentdation-by-field',
	templateUrl: './recommendations-by-field.component.html',
	styleUrls: ['./recommendations-by-field.component.css'],
})
export class RecommendationsByFieldComponent implements OnInit {
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
		private unitService: UnitService
	) {
		this.year = new Date().getFullYear()
		this.listUnitSelected = []
	}

	ngOnInit() {
		this.BsLocaleService.use('vi')
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

	getList() {
		if (this.listUnitSelected != null && this.listUnitSelected.length > 0) {
			this.ltsUnitId = this.listUnitSelected.reduce((x, y) => {
				return (x += y.unitId + ',')
			}, '')
			this.totalRecords = this.listUnitSelected.length
		} else {
			this.ltsUnitId = this.ltsUnitIdAll
			this.totalRecords = this.listUnit.length
		}
		let obj = {
			PageIndex: this.pageIndex == null ? 1 : this.pageIndex,
			PageSize: this.pageSize == null ? 20 : this.pageSize,
			LtsUnitId: this.ltsUnitId,
			Year: this.year,
			Timeline: this.timeline == null ? '' : this.timeline,
			FromDate: this.fromDate == null ? '' : (this.fromDate = JSON.stringify(new Date(this.fromDate)).slice(1, 11)),
			ToDate: this.toDate == null ? '' : (this.toDate = JSON.stringify(new Date(this.toDate)).slice(1, 11)),
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
		if (data != null) {
			this.fromDate = JSON.stringify(new Date(data)).slice(1, 11)
		} else {
			this.fromDate = null
		}
		this.getList()
	}
	toDateChange(data) {
		if (data != null) {
			this.toDate = JSON.stringify(new Date(data)).slice(1, 11)
			// this.getList()
		} else {
			this.toDate = null
		}
		this.getList()
	}
}
