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

	lstTimeline: any = [
		{ value: 1, text: 'Qúy I' },
		{ value: 2, text: 'Quý II' },
		{ value: 3, text: 'Quý II' },
		{ value: 4, text: 'Quý III' },
		{ value: 5, text: '6 tháng đầu năm' },
		{ value: 6, text: '6 tháng cuối năm' },
	]

	constructor(
		private _toastr: ToastrService,
		private router: Router,
		private activatedRoute: ActivatedRoute,
		private BsLocaleService: BsLocaleService,
		private _service: StatisticService
	) {}

	ngOnInit() {
		this.BsLocaleService.use('vi')
		this.getList()
	}

	getList() {
		let obj = {
			PageIndex: this.pageIndex,
			PageSize: this.pageSize,
			LtsUnitId: '',
			Year: '',
			Timeline: '',
			FromDate: '',
			ToDate: '',
		}
		this._service.getStatisticRecommendationByUnit(obj).subscribe((res) => {
			debugger
			if (res.success != RESPONSE_STATUS.success) {
				this.listData = []
				this.totalRecords = 0
				this.pageIndex = 1
				this.pageSize = 20
				return
			}
			this.listData = res.result.StatisticRecommendationByUnitGetAllOnPage
			this.totalRecords = res.result.TotalCount
			this.pageSize = res.result.PageSize
			this.pageIndex = res.result.PageIndex
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
}
