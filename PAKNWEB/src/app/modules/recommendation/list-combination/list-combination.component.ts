import { Component, OnInit, ViewChild } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { RecommendationObject, RecommendationProcessObject, RecommendationSearchObject } from 'src/app/models/recommendationObject'
import { RecommendationService } from 'src/app/services/recommendation.service'
import { DataService } from 'src/app/services/sharedata.service'
import { saveAs as importedSaveAs } from 'file-saver'
import { MESSAGE_COMMON, PROCESS_STATUS_RECOMMENDATION, RECOMMENDATION_STATUS, RESPONSE_STATUS, STEP_RECOMMENDATION } from 'src/app/constants/CONSTANTS'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { Router } from '@angular/router'
import { COMMONS } from 'src/app/commons/commons'

declare var $: any
@Component({
	selector: 'app-list-combination',
	templateUrl: './list-combination.component.html',
	styleUrls: ['./list-combination.component.css'],
})
export class ListCombinationComponent implements OnInit {
	constructor(
		private _service: RecommendationService,
		private storeageService: UserInfoStorageService,
		private _toastr: ToastrService,
		private _shareData: DataService,
		private _router: Router
	) { }
	userLoginId: number = this.storeageService.getUserId()
	isMain: boolean = this.storeageService.getIsMain()
	listData = new Array<RecommendationObject>()
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
	lstUnit: any = []
	lstField: any = []
	dataSearch: RecommendationSearchObject = new RecommendationSearchObject()
	submitted: boolean = false
	isActived: boolean
	pageIndex: number = 1
	pageSize: number = 20
	lstHistories: any = []
	@ViewChild('table', { static: false }) table: any
	totalRecords: number = 0
	idDelete: number = 0
	ngOnInit() {
		this.dataSearch.status = RECOMMENDATION_STATUS.APPROVE_DENY
		this.getDataForCreate()
		this.getList()
	}

	ngAfterViewInit() {
		this._shareData.seteventnotificationDropdown()
	}

	getDataForCreate() {
		this._service.recommendationGetDataForCreate({}).subscribe((response) => {
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
	}

	getList() {
		this.dataSearch.code = this.dataSearch.code.trim()
		this.dataSearch.name = this.dataSearch.name.trim()
		this.dataSearch.content = this.dataSearch.content.trim()
		let request = {
			Code: this.dataSearch.code,
			SendName: this.dataSearch.name,
			Content: this.dataSearch.content,
			UnitId: this.dataSearch.unitId != null ? this.dataSearch.unitId : '',
			Field: this.dataSearch.field != null ? this.dataSearch.field : '',
			Status: this.dataSearch.status != null ? this.dataSearch.status : '',
			UnitProcessId: this.storeageService.getUnitId(),
			UserProcessId: this.storeageService.getUserId(),
			PageIndex: this.pageIndex,
			PageSize: this.pageSize,
		}

		this._service.recommendationCombinationGet(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result != null) {
					this.listData = []
					this.listData = response.result.MRRecommendationCombine
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
	modelProcess: RecommendationProcessObject = new RecommendationProcessObject()
	recommendationStatusProcess: number
	titleAccept: string
	preProcess(model: any, status: number) {
		debugger
		this.modelProcess.status = status
		this.modelProcess.id = model.processId
		this.modelProcess.recommendationId = model.id
		this.modelProcess.reasonDeny = ''
		if (status == PROCESS_STATUS_RECOMMENDATION.DENY) {
			if (model.status == RECOMMENDATION_STATUS.PROCESS_WAIT) {
				this.recommendationStatusProcess = RECOMMENDATION_STATUS.PROCESS_DENY
			} else if (model.status == RECOMMENDATION_STATUS.APPROVE_WAIT) {
				this.recommendationStatusProcess = RECOMMENDATION_STATUS.APPROVE_DENY
			}
			$('#modalReject').modal('show')
			setTimeout(() => {
				$('#targetReject').focus()
			}, 400)
		} else if (status == PROCESS_STATUS_RECOMMENDATION.APPROVED) {
			if (model.status == RECOMMENDATION_STATUS.PROCESS_WAIT) {
				this.titleAccept = 'Anh/Chị có chắc chắn muốn tiếp nhận tham mưu PAKN này?'
				this.recommendationStatusProcess = RECOMMENDATION_STATUS.PROCESSING
			} else if (model.status == RECOMMENDATION_STATUS.APPROVE_WAIT) {
				this.titleAccept = 'Anh/Chị có chắc chắn muốn phê duyệt Phản ánh, Kiến nghị này?'
				this.recommendationStatusProcess = RECOMMENDATION_STATUS.FINISED
			}
			$('#modalAccept').modal('show')
		}
	}

	onProcessAccept() {
		let obj = this.listData.find((x) => x.id == this.modelProcess.recommendationId)
		var request = {
			_mRRecommendationForwardProcessIN: this.modelProcess,
			RecommendationStatus: this.recommendationStatusProcess
		}
		this._service.recommendationProcess(request, obj.title).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				$('#modalAccept').modal('hide')
				this._toastr.success(COMMONS.ACCEPT_SUCCESS)
				this.getList()
			} else {
				this._toastr.error(response.message)
			}
		}),
			(err) => {
				console.error(err)
			}
	}

	onProcessDeny() {
		if (this.modelProcess.reasonDeny == '' || this.modelProcess.reasonDeny.trim() == '') {
			this._toastr.error('Vui lòng nhập lý do')
			return
		} else {
			let obj = this.listData.find((x) => x.id == this.modelProcess.recommendationId)
			var request = {
				_mRRecommendationForwardProcessIN: this.modelProcess,
				RecommendationStatus: this.recommendationStatusProcess
			}
			this._service.recommendationProcess(request, obj.title).subscribe((response) => {
				if (response.success == RESPONSE_STATUS.success) {
					$('#modalReject').modal('hide')
					this._toastr.success(COMMONS.DENY_SUCCESS)
					this.getList()
				} else {
					this._toastr.error(response.message)
				}
			}),
				(err) => {
					console.error(err)
				}
		}
	}




	onExport() {
		let passingObj: any = {}
		passingObj = this.dataSearch
		if (this.listData.length > 0) {
			passingObj.UnitProcessId = this.storeageService.getUnitId()
			passingObj.UserProcessId = this.storeageService.getUserId()
			passingObj.UserProcessName = this.storeageService.getFullName()
		}
		passingObj.TitleReport = 'DANH SÁCH TỪ CHỐI PHÊ DUYỆT'
		this._shareData.setobjectsearch(passingObj)
		this._shareData.sendReportUrl = 'Recommendation_ListGeneral?' + JSON.stringify(passingObj)
		this._router.navigate(['quan-tri/xuat-file'])
	}
}
