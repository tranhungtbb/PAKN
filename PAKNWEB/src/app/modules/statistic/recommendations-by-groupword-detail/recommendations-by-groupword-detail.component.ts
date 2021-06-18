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
	selector: 'app-recommendations-by-groupword-detail',
	templateUrl: './recommendations-by-groupword-detail.component.html',
	styleUrls: ['./recommendations-by-groupword-detail.component.css'],
})
export class RecommendationsByGroupwordDetailComponent implements OnInit {
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
	userLoginId: number = this.storeageService.getUserId()
	isMain: boolean = this.storeageService.getIsMain()
	listData = new Array<RecommendationObject>()
	lstUnit: any = []
	lstField: any = []
	dataSearch: RecommendationSearchStatisticObject = new RecommendationSearchStatisticObject()
	submitted: boolean = false
	isActived: boolean
	fromDate: string
	toDate: string
	pageIndex: number = 1
	pageSize: number = 20
	lstHistories: any = []
	lstDictionariesWord: any = []
	@ViewChild('table', { static: false }) table: any
	totalRecords: number = 0
	idDelete: number = 0
	ngOnInit() {
		this.getDataForCreate()
		this.activatedRoute.params.subscribe((params) => {
			this.dataSearch.groupWordId = +params['groupWordId']
			this.dataSearch.unitId = +params['unitId']
			this.fromDate = params['fromDate']
			this.toDate = params['toDate']
			this.getList()
		})
	}

	ngAfterViewInit() {
		this._shareData.seteventnotificationDropdown()
	}

	getDataForCreate() {
		this._serviceRecommendation.recommendationGetDataForCreate({}).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result != null) {
					this.lstUnit = response.result.lstUnit
					this.lstField = response.result.lstField
				}
			} else {
				this._toastr.error(response.message)
			}
		}),
			(error) => {
				console.log(error)
				alert(error)
			}
		this._serviceCatalog.wordGetListSuggest({}).subscribe(
			(response) => {
				if (response.success == RESPONSE_STATUS.success) {
					this.lstDictionariesWord = response.result.CAWordGetListSuggest
				} else {
					this._toastr.error(response.message)
				}
			},
			(error) => {
				console.log(error)
			}
		)
	}

	getList() {
		this.dataSearch.code = this.dataSearch.code.trim()
		this.dataSearch.sendName = this.dataSearch.sendName.trim()
		this.dataSearch.title = this.dataSearch.title.trim()
		this.dataSearch.content = this.dataSearch.content.trim()
		let request = {
			Code: this.dataSearch.code,
			SendName: this.dataSearch.sendName,
			Title: this.dataSearch.title,
			Content: this.dataSearch.content,
			UnitId: this.dataSearch.unitId != null ? this.dataSearch.unitId : '',
			GroupWordId: this.dataSearch.groupWordId != null ? this.dataSearch.groupWordId : '',
			FromDate: this.fromDate == null ? '' : this.fromDate,
			ToDate: this.toDate == null ? '' : this.toDate,
			PageIndex: this.pageIndex,
			PageSize: this.pageSize,
		}

		this._service.getStatisticRecommendationByGroupWordDetail(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result != null) {
					this.listData = []
					this.listData = response.result.ListData
					this.totalRecords = response.result.TotalCount
					this.hightLightText()
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
	hightLightText() {
		for (let i = 0; i < this.listData.length; i++) {
			var content = this.listData[i].content.replace(/\\n/g, String.fromCharCode(13, 10))
			for (let index = 0; index < this.lstDictionariesWord.length; index++) {
				var nameWord = new RegExp(this.lstDictionariesWord[index].name, 'i')
				this.listData[i].content = this.listData[i].content.replace(
					nameWord,
					'<span class="txthighlight" title="' + this.lstDictionariesWord[index].description + '">' + this.lstDictionariesWord[index].name + '</span>'
				)
			}
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

	changeState(event: any) {
		if (event) {
			if (event.target.value == 'null') {
				this.isActived = null
			} else {
				this.isActived = event.target.value
			}
			this.pageIndex = 1
			this.pageSize = 20
			this.getList()
		}
	}

	changeType(event: any) {
		if (event) {
			this.pageIndex = 1
			this.pageSize = 20
			this.getList()
		}
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
		passingObj = this.dataSearch
		passingObj.GroupWordId = this.dataSearch.groupWordId != null ? this.dataSearch.groupWordId : ''
		passingObj.FromDate = this.fromDate == null ? '' : this.fromDate
		passingObj.ToDate = this.toDate == null ? '' : this.toDate
		this._shareData.setobjectsearch(passingObj)
		this._shareData.sendReportUrl = 'Statistic_Recommendation_ByGroupWord?' + JSON.stringify(passingObj)
		this._router.navigate(['quan-tri/xuat-file'])
	}
}
