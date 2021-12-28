import { Component, OnInit, ViewChild } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { BsDatepickerConfig, BsLocaleService } from 'ngx-bootstrap/datepicker'
import { RecommendationObject, RecommendationSearchStatisticObject } from 'src/app/models/recommendationObject'
import { RecommendationService } from 'src/app/services/recommendation.service'
import { DataService } from 'src/app/services/sharedata.service'
import { saveAs as importedSaveAs } from 'file-saver'
import { MESSAGE_COMMON, RECOMMENDATION_STATUS, RESPONSE_STATUS, STATUS_HIS_SMS } from 'src/app/constants/CONSTANTS'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { ActivatedRoute, Router } from '@angular/router'
import { StatisticService } from 'src/app/services/statistic.service'
import { CatalogService } from 'src/app/services/catalog.service'

declare var $: any
@Component({
	selector: 'app-processing-results-by-reception-type-detail',
	templateUrl: './processing-results-by-reception-type-detail.component.html',
	styleUrls: ['./processing-results-by-reception-type-detail.component.css'],
})
export class RecommendationsByReceptionTypeDetailComponent implements OnInit {
	constructor(
		private _serviceRecommendation: RecommendationService,
		private _service: StatisticService,
		private storeageService: UserInfoStorageService,
		private _toastr: ToastrService,
		private _shareData: DataService,
		private activatedRoute: ActivatedRoute,
		private _serviceCatalog: CatalogService,
		private _router: Router
	) { }
	isMain: boolean = this.storeageService.getIsMain()
	listData: any = []
	lstUnit: any[] = []
	lstField: any[] = []
	unitId: number
	fieldId: number
	receptionType: number
	dataSearch: DataSearch = new DataSearch()
	fieldName: string
	fromDate: string
	toDate: string
	pageIndex: number = 1
	pageSize: number = 20
	data: any = {}

	lstReceptionType: any = [
		{ value: 1, text: 'Qua Web' },
		{ value: 2, text: 'Qua App' },
		{ value: 3, text: 'Qua điện thoại' },
		{ value: 4, text: 'Qua email, văn bản' }
	]


	@ViewChild('table', { static: false }) table: any
	totalRecords: number = 0
	ngOnInit() {
		this.activatedRoute.params.subscribe((params) => {

			this.dataSearch.type = +params['type']
			if (this.dataSearch.type == 1) // linh vuc
			{
				this.fieldId = +params['fieldId']
			} else { // don vi
				this.unitId = +params['unitId']
			}

			let receptionType = +params['ReceptionType']
			if (receptionType) {
				this.receptionType = receptionType
			}
			this.fromDate = params['fromDate']
			this.toDate = params['toDate']
			this.getList()
			this._serviceRecommendation.recommendationGetDataForSearch({}).subscribe((response) => {
				if (response.success == RESPONSE_STATUS.success) {
					this.lstUnit = response.result.lstUnit
					this.lstField = response.result.lstField
				} else {
					this._toastr.error(response.message)
				}
			}),
				(error) => {
					console.log(error)
				}
		})
	}

	ngAfterViewInit() {
		this._shareData.seteventnotificationDropdown()
	}


	getList() {
		this.dataSearch.code = this.dataSearch.code.trim()
		this.dataSearch.sendName = this.dataSearch.sendName.trim()
		this.dataSearch.title = this.dataSearch.title.trim()
		this.dataSearch.unitId = (this.dataSearch.unitId == null || this.dataSearch.unitId == 0) ? '' : this.dataSearch.unitId
		let request = {
			Type: this.dataSearch.type,
			FieldId: this.fieldId == null ? '' : this.fieldId,
			UnitId: this.unitId == null ? '' : this.unitId,
			ReceptionType: this.receptionType == null ? '' : this.receptionType,
			Code: this.dataSearch.code,
			Name: this.dataSearch.sendName,
			Title: this.dataSearch.title,
			FromDate: this.fromDate == null ? '' : this.fromDate,
			ToDate: this.toDate == null ? '' : this.toDate,
			PageIndex: this.pageIndex,
			PageSize: this.pageSize,
		}

		this._service.getStatisticRecommendationByReceptionTypeDetail(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result != null) {
					this.listData = []
					this.data = response.result.Data
					this.listData = response.result.ListRecommentdation
					this.totalRecords = response.result.TotalCount
				}
			} else {
				this._toastr.error(response.message)
			}
		}),
			(error) => {
				console.log(error)
				alert(error)
			}
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



	onExport() {
		// let passingObj: any = {}
		// passingObj.TitleReport = "THỐNG KÊ PHẢN ẢNH KIẾN NGHỊ LĨNH VỰC " + this.fieldName.toUpperCase()
		// passingObj.Code = this.dataSearch.code == '' ? null : this.dataSearch.code 
		// passingObj.SendName = this.dataSearch.sendName =='' ? null : this.dataSearch.sendName
		// passingObj.Title = this.dataSearch.title == '' ? null : this.dataSearch.title
		// passingObj.UnitProcessId = this.storeageService.getUnitId()
		// passingObj.UserProcessId = this.storeageService.getUserId()
		// passingObj.UserProcessName = this.storeageService.getFullName()
		// passingObj.Status = this.dataSearch.status 
		// passingObj.LstUnitId = this.dataSearch.lstUnitId == null ? '' : this.dataSearch.lstUnitId
		// passingObj.Field = this.dataSearch.fieldId
		// passingObj.FromDate = this.fromDate == null ? '' : this.fromDate
		// passingObj.ToDate = this.toDate == null ? '' : this.toDate
		// this._shareData.setobjectsearch(passingObj)
		// this._shareData.sendReportUrl = 'Statistic_Recommendation_ByFieldDetail?' + JSON.stringify(passingObj)
		// this._router.navigate(['quan-tri/xuat-file'])
	}
	redirectHis() {
		window.history.back()
	}
}

class DataSearch {
	constructor() {
		this.code = ''
		this.sendName = ''
		this.title = ''
		this.type = null,
			this.recommendationType = null
	}
	code: string
	sendName: string
	title: string
	fieldId: number
	unitId: any
	type: number
	recommendationType: number
}
