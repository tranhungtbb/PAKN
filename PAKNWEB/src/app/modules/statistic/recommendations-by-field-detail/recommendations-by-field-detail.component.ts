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
	selector: 'app-recommendations-by-field-detail',
	templateUrl: './recommendations-by-field-detail.component.html',
	styleUrls: ['./recommendations-by-field-detail.component.css'],
})
export class RecommendationsByFieldDetailComponent implements OnInit {
	constructor(
		private _serviceRecommendation: RecommendationService,
		private _service: StatisticService,
		private storeageService: UserInfoStorageService,
		private _toastr: ToastrService,
		private _shareData: DataService,
		private activatedRoute: ActivatedRoute,
		private _serviceCatalog: CatalogService,
		private _router: Router
	) {}
	isMain: boolean = this.storeageService.getIsMain()
	listData : any = []
	lstUnit : any [] = []
	unitId : any 
	dataSearch: DataSearch = new DataSearch()
	fieldName : string
	fromDate: string
	toDate: string
	pageIndex: number = 1
	pageSize: number = 20
	lstHistories: any = []
	listStatus: any = [
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
			this.dataSearch.fieldId = +params['fieldId']
			this.dataSearch.lstUnitId = params['lstUnitId']
			this.fromDate = params['fromDate']
			this.toDate = params['toDate']
			let status = params['status']
			if(!isNaN(status)){
				this.dataSearch.status = Number(status)
			}
			this.getList()
			this._serviceRecommendation.recommendationGetDataForCreate({}).subscribe((response) => {
				if (response.success == RESPONSE_STATUS.success) {
					if (response.result != null) {
						let lstUnitId = this.dataSearch.lstUnitId.split(',').map(Number)
						this.lstUnit  = response.result.lstUnit.filter(x=>{
							if(lstUnitId.includes(x.value)){
								return x
							}
						})
						let field = response.result.lstField.find(x=>x.value == this.dataSearch.fieldId)
						this.fieldName = field.text.replaceAll('-', '')
					}
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
		let request = {
			Code: this.dataSearch.code,
			CreateName: this.dataSearch.sendName,
			Title: this.dataSearch.title,
			Status : this.dataSearch.status != null ? this.dataSearch.status : '',
			FiledId : this.dataSearch.fieldId != null ? this.dataSearch.fieldId : '',
			lstUnitId: this.unitId == null ? this.dataSearch.lstUnitId : this.unitId + ',',
			FromDate: this.fromDate == null ? '' : this.fromDate,
			ToDate: this.toDate == null ? '' : this.toDate,
			PageIndex: this.pageIndex,
			PageSize: this.pageSize,
		}

		this._service.getStatisticRecommendationByFieldDetail(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result != null) {
					this.listData = []
					this.listData = response.result.RecommendationsByFieldDetailGetAllOnPage
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
		passingObj.TitleReport = "THỐNG KÊ PHẢN ẢNH KIẾN NGHỊ LĨNH VỰC " + this.fieldName.toUpperCase()
		passingObj.Code = this.dataSearch.code == '' ? null : this.dataSearch.code 
		passingObj.SendName = this.dataSearch.sendName =='' ? null : this.dataSearch.sendName
		passingObj.Title = this.dataSearch.title == '' ? null : this.dataSearch.title
		passingObj.UnitProcessId = this.storeageService.getUnitId()
		passingObj.UserProcessId = this.storeageService.getUserId()
		passingObj.Status = this.dataSearch.status 
		passingObj.LstUnitId = this.dataSearch.lstUnitId == null ? '' : this.dataSearch.lstUnitId
		passingObj.Field = this.dataSearch.fieldId
		passingObj.FromDate = this.fromDate == null ? '' : this.fromDate
		passingObj.ToDate = this.toDate == null ? '' : this.toDate
		this._shareData.setobjectsearch(passingObj)
		this._shareData.sendReportUrl = 'Statistic_Recommendation_ByFieldDetail?' + JSON.stringify(passingObj)
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
		this.lstUnitId = null
		this.fieldId = null
		this.status = null
	}
	code: string
	sendName: string
	title: string
	fieldId: number
	lstUnitId : string
	status : number
}
