import { Component, OnInit, ViewChild } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { RecommendationObject, RecommendationProcessObject, RecommendationSearchObject } from 'src/app/models/recommendationObject'
import { RecommendationService } from 'src/app/services/recommendation.service'
import { DataService } from 'src/app/services/sharedata.service'
import { saveAs as importedSaveAs } from 'file-saver'
import { MESSAGE_COMMON, PROCESS_STATUS_RECOMMENDATION, RECOMMENDATION_STATUS, RESPONSE_STATUS, STEP_RECOMMENDATION } from 'src/app/constants/CONSTANTS'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { stat } from 'fs'
import { COMMONS } from 'src/app/commons/commons'

declare var $: any

@Component({
	selector: 'app-list-approve-wait',
	templateUrl: './list-approve-wait.component.html',
	styleUrls: ['./list-approve-wait.component.css'],
})
export class ListApproveWaitComponent implements OnInit {
	constructor(private _service: RecommendationService, private storeageService: UserInfoStorageService, private _toastr: ToastrService, private _shareData: DataService) {}
	userLoginId: number = this.storeageService.getUserId()
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
	isMain: boolean = this.storeageService.getIsMain()
	ngOnInit() {
		this.dataSearch.status = RECOMMENDATION_STATUS.APPROVE_WAIT
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
			Status: 8,
			UnitProcessId: this.storeageService.getUnitId(),
			UserProcessId: this.storeageService.getUserId(),
			PageIndex: this.pageIndex,
			PageSize: this.pageSize,
		}

		this._service.recommendationGetListProcess(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result != null) {
					this.listData = []
					this.listData = response.result.MRRecommendationGetAllWithProcess
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
	preProcess(recommendationId, idProcess, status) {
		this.modelProcess.status = status
		this.modelProcess.id = idProcess
		this.modelProcess.step = STEP_RECOMMENDATION.APPROVE
		this.modelProcess.recommendationId = recommendationId
		this.modelProcess.reactionaryWord = false
		this.modelProcess.reasonDeny = ''
		if (status == PROCESS_STATUS_RECOMMENDATION.DENY) {
			$('#modalReject').modal('show')
		} else {
			$('#modalAccept').modal('show')
		}
	}
	onProcessAccept() {
		var request = {
			_mRRecommendationForwardProcessIN: this.modelProcess,
			RecommendationStatus: RECOMMENDATION_STATUS.FINISED,
			ReactionaryWord: this.modelProcess.reactionaryWord,
		}
		this._service.recommendationProcess(request).subscribe((response) => {
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
			var request = {
				_mRRecommendationForwardProcessIN: this.modelProcess,
				RecommendationStatus: RECOMMENDATION_STATUS.APPROVE_DENY,
			}
			this._service.recommendationProcess(request).subscribe((response) => {
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

	exportExcel() {
		let request = {
			IsActived: this.isActived,
		}

		this._service.recommendationExportExcel(request).subscribe((response) => {
			var today = new Date()
			var dd = String(today.getDate()).padStart(2, '0')
			var mm = String(today.getMonth() + 1).padStart(2, '0')
			var yyyy = today.getFullYear()
			var hh = String(today.getHours()).padStart(2, '0')
			var minute = String(today.getMinutes()).padStart(2, '0')
			var fileName = 'DM_ChucVuHanhChinh_' + yyyy + mm + dd + hh + minute
			var blob = new Blob([response], { type: response.type })
			importedSaveAs(blob, fileName)
		})
	}
}
