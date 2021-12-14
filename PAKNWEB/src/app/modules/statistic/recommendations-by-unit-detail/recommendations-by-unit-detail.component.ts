import { Component, OnInit, ViewChild } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { RecommendationObject, RecommendationSearchStatisticObject } from 'src/app/models/recommendationObject'
import { RecommendationService } from 'src/app/services/recommendation.service'
import { DataService } from 'src/app/services/sharedata.service'
import { saveAs as importedSaveAs } from 'file-saver'
import { MESSAGE_COMMON, RECOMMENDATION_STATUS, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { ActivatedRoute, Router } from '@angular/router'
import { StatisticService } from 'src/app/services/statistic.service'
import { CatalogService } from 'src/app/services/catalog.service'

declare var $: any
@Component({
	selector: 'app-recommendations-by-unit-detail',
	templateUrl: './recommendations-by-unit-detail.component.html',
	styleUrls: ['./recommendations-by-unit-detail.component.css'],
})
export class RecommendationsByUnitDetailComponent implements OnInit {
	constructor(
		private _serviceRecommendation: RecommendationService,
		private _service: StatisticService,
		private storeageService: UserInfoStorageService,
		private _toastr: ToastrService,
		private _shareData: DataService,
		private activatedRoute: ActivatedRoute,
		private _serviceCatalog: CatalogService,
		private _router: Router
	) {
		this.listData = []
	}
	isMain: boolean = this.storeageService.getIsMain()
	listData : any = []
	lstField: any = []
	dataSearch: DataSearch = new DataSearch()
	unitId : number
	unitName : string
	fromDate: string
	toDate: string
	pageIndex: number = 1
	pageSize: number = 20
	lstHistories: any = []
	listStatus: any = [
		{ value: 1, text: 'Đang soạn thảo' },
		{ value: 2, text: 'Chờ xử lý' },
		{ value: 3, text: 'Từ chối xử lý' },
		{ value: 4, text: 'Đã tiếp nhận' },
		{ value: 5, text: 'Chờ giải quyết' },
		{ value: 6, text: 'Từ chối giải quyết' },
		{ value: 7, text: 'Đang giải quyết' },
		{ value: 8, text: 'Chờ phê duyệt' },
		{ value: 9, text: 'Từ chối phê duyệt' },
		{ value: 10, text: 'Đã giải quyết' },
	]
	@ViewChild('table', { static: false }) table: any
	totalRecords: number = 0
	ngOnInit() {
		this.activatedRoute.params.subscribe((params) => {
			this.unitId = +params['unitId']
			this.fromDate = params['fromDate']
			this.toDate = params['toDate']
			let status = params['status']
			if(!isNaN(status)){
				this.dataSearch.status = Number(status)
			}
			this.getList()
		})
		this._serviceRecommendation.recommendationGetDataForCreate({}).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result != null) {
					this.lstField = response.result.lstField
					let unit = response.result.lstUnit.find(x=>x.value == this.unitId)
					this.unitName = unit.text.replaceAll('-', '')
				}
			} else {
				this._toastr.error(response.message)
			}
		}),
			(error) => {
				console.log(error)
			}
	}

	ngAfterViewInit() {
		this._shareData.seteventnotificationDropdown()
	}


	getList() {
		this.dataSearch.code = this.dataSearch.code.trim()
		this.dataSearch.sendName = this.dataSearch.sendName.trim()
		this.dataSearch.title = this.dataSearch.title.trim()
		let request = {
			Code: this.dataSearch.code,
			CreateName: this.dataSearch.sendName,
			Title: this.dataSearch.title,
			Status : this.dataSearch.status != null ? this.dataSearch.status : '',
			Field : this.dataSearch.fieldId != null ? this.dataSearch.fieldId : '',
			UnitId: this.unitId != null ? this.unitId : '',
			FromDate: this.fromDate == null ? '' : this.fromDate,
			ToDate: this.toDate == null ? '' : this.toDate,
			PageIndex: this.pageIndex,
			PageSize: this.pageSize,
		}

		this._service.getStatisticRecommendationByUnitDetail(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result != null) {
					this.listData = []
					this.listData = response.result.RecommendationsByUnitDetailGetAllOnPage
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


	getHistories(id: number) {
		let request = {
			Id: id,
		}
		this._serviceRecommendation.recommendationGetHistories(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				this.lstHistories = response.result.HISRecommendationGetByObjectId
				$('#modal-history-pakn').modal('show')
			} else {
				this._toastr.error(response.message)
			}
		}),
			(error) => {
				console.log(error)
			}
	}

	onExport() {
		let passingObj: any = {}
		passingObj.TitleReport = "THỐNG KÊ PHẢN ẢNH KIẾN NGHỊ ĐƠN VỊ " + this.unitName.toUpperCase()
		passingObj.Code = this.dataSearch.code
		passingObj.CreateName = this.dataSearch.sendName
		passingObj.TitleMR = this.dataSearch.title
		passingObj.Status = this.dataSearch.status
		passingObj.UnitId = this.unitId
		passingObj.Field = this.dataSearch.fieldId
		passingObj.UnitProcessId = this.storeageService.getUnitId()
		passingObj.UserProcessId = this.storeageService.getUserId()
		passingObj.UserProcessName = this.storeageService.getFullName()
		passingObj.FromDate = this.fromDate == null ? '' : this.fromDate
		passingObj.ToDate = this.toDate == null ? '' : this.toDate
		this._shareData.setobjectsearch(passingObj)
		this._shareData.sendReportUrl = 'Statistic_Recommendation_ByUnitDetail?' + JSON.stringify(passingObj)
		this._router.navigate(['quan-tri/xuat-file'])
	}
	redirectHis(){
		window.history.back()
	}
}

class DataSearch {
	constructor() {
		this.code = ''
		this.sendName = ''
		this.title = ''
		this.fieldId = null
		this.status = null
	}
	code: string
	sendName: string
	title: string
	fieldId: number
	status : number
}
