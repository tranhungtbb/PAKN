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
	selector: 'app-recommendations-by-groupword',
	templateUrl: './recommendations-by-groupword.component.html',
	styleUrls: ['./recommendations-by-groupword.component.css'],
})
export class RecommendationsByGroupwordComponent implements OnInit {
	totalRecords: Number
	frozenCols: any = [{ field: 'text', header: 'Đơn vị', cssClass: 'text-left' }]
	scrollableCols: any = []
	lstData: any = []
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
		let obj = {
			LtsUnitId: this.listUnitSelected.join(','),
			Year: this.year,
			Timeline: this.timeline == null ? '' : this.timeline,
			FromDate: this.fromDate == null ? '' : (this.fromDate = JSON.stringify(new Date(this.fromDate)).slice(1, 11)),
			ToDate: this.toDate == null ? '' : (this.toDate = JSON.stringify(new Date(this.toDate)).slice(1, 11)),
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
	viewDetail(unitId, groupWordId) {
		return this.router.navigate(['/quan-tri/bao-cao/phan-anh-kien-nghi-theo-nhom-tu-ngu-chi-tiet', unitId, groupWordId])
	}
}
