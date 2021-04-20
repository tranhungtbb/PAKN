import { Component, OnInit, ViewChild } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { RecommendationForwardObject, RecommendationObject, RecommendationProcessObject, RecommendationSearchObject } from 'src/app/models/recommendationObject'
import { RecommendationRequestService } from 'src/app/services/recommendation-req.service'
import { DataService } from 'src/app/services/sharedata.service'
import { saveAs as importedSaveAs } from 'file-saver'
import { MESSAGE_COMMON, PROCESS_STATUS_RECOMMENDATION, RECOMMENDATION_STATUS, RESPONSE_STATUS, STEP_RECOMMENDATION } from 'src/app/constants/CONSTANTS'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { COMMONS } from 'src/app/commons/commons'

declare var $: any

@Component({
	selector: 'app-list-request',
	templateUrl: './list-request.component.html',
	styleUrls: ['./list-request.component.css'],
})
export class ListRequestComponent implements OnInit {
	constructor(
		private _service: RecommendationRequestService,
		private storeageService: UserInfoStorageService,
		private _fb: FormBuilder,
		private _toastr: ToastrService,
		private _shareData: DataService
	) {}
	userLoginId: number = this.storeageService.getUserId()
	listData = new Array<RecommendationObject>()
	listStatus: any = [
		{ value: 1, text: 'Chưa giải quyết' },
		{ value: 2, text: 'Đang giải quyết' },
		{ value: 3, text: 'Đã giải quyết' },
	]
	formForward: FormGroup
	lstUnitNotMain: any = []
	lstUnit: any = []
	lstFields: any = []
	lstFieldFilter: any = []
	dataSearch: RecommendationSearchObject = new RecommendationSearchObject()
	submitted: boolean = false
	isActived: boolean
	pageIndex: number = 1
	pageSize: number = 20
	@ViewChild('table', { static: false }) table: any
	totalRecords: number = 0
	idDelete: number = 0
	lstHistories: any = []
	modelForward: RecommendationForwardObject = new RecommendationForwardObject()
	ngOnInit() {
		this.buildForm()
		this.getDataForCreate()
		this.getList()
	}

	ngAfterViewInit() {
		this._shareData.seteventnotificationDropdown()
	}

	getDataForCreate() {
		this._service.recommendationGetDataForCreate({}).subscribe(response => {
			console.log(response)
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result != null) {
					console.log(response.result.CAFieldKNCTGetDropdown)
					this.lstFields = response.result.CAFieldKNCTGetDropdown
					this.lstFieldFilter = this.lstFields.filter(lstField => {
						return lstField.text != null
					})
				}
			} else {
				this._toastr.error(response.message)
			}
		}),
			error => {
				console.log(error)
			}
	}

	get f() {
		return this.formForward.controls
	}

	buildForm() {
		this.formForward = this._fb.group({
			unitReceiveId: [this.modelForward.unitReceiveId, Validators.required],
			expiredDate: [this.modelForward.expiredDate],
			content: [this.modelForward.content],
		})
	}

	rebuilForm() {
		this.formForward.reset({
			unitReceiveId: this.modelForward.unitReceiveId,
			expiredDate: this.modelForward.expiredDate,
			content: this.modelForward.content,
		})
	}

	getList() {
		this.dataSearch.code = this.dataSearch.code.trim()
		this.dataSearch.name = this.dataSearch.name.trim()
		this.dataSearch.content = this.dataSearch.content.trim()
		this.dataSearch.unitId = this.dataSearch.unitId.trim()
		this.dataSearch.place = this.dataSearch.place.trim()
		let request = {
			Code: this.dataSearch.code,
			Content: this.dataSearch.content,
			Unit: this.dataSearch.unitId,
			Place: this.dataSearch.place,
			Field: this.dataSearch.field != null ? this.dataSearch.field : '',
			Status: this.dataSearch.status != null ? this.dataSearch.status : '',
			PageIndex: this.pageIndex,
			PageSize: this.pageSize,
		}
		console.log(request)
		this._service.recommendationGetListProcess(request).subscribe(response => {
			console.log(response)
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result != null) {
					this.listData = []
					this.listData = response.result.MRRecommendationKNCTGetAllWithProcess
					this.totalRecords = response.result.MRRecommendationKNCTGetAllWithProcess.length != 0 ? response.result.MRRecommendationKNCTGetAllWithProcess[0].rowNumber : 0
				}
			} else {
				this._toastr.error(response.message)
			}
		}),
			error => {
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

	preDelete(id: number) {
		this.idDelete = id
		$('#modalConfirmDelete').modal('show')
	}

	onDelete(id: number) {
		let request = {
			Id: id,
		}
		this._service.recommendationDelete(request).subscribe(response => {
			if (response.success == RESPONSE_STATUS.success) {
				this._toastr.success(MESSAGE_COMMON.DELETE_SUCCESS)
				$('#modalConfirmDelete').modal('hide')
				this.getList()
			} else {
				this._toastr.error(response.message)
			}
		}),
			error => {
				console.error(error)
			}
	}

	getHistories(id: number) {
		let request = {
			Id: id,
		}
		this._service.recommendationGetHistories(request).subscribe(response => {
			if (response.success == RESPONSE_STATUS.success) {
				this.lstHistories = response.result.HISRecommendationGetByObjectId
				$('#modal-history-pakn').modal('show')
			} else {
				this._toastr.error(response.message)
			}
		}),
			error => {
				console.log(error)
			}
	}
	preForward(id: number) {
		this.modelForward = new RecommendationForwardObject()
		this.modelForward.recommendationId = id
		this.rebuilForm()
		this._service.recommendationGetDataForForward({}).subscribe(response => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result != null) {
					this.lstUnitNotMain = response.result.lstUnitNotMain
					$('#modal-tc-pakn').modal('show')
				}
			} else {
				this._toastr.error(response.message)
			}
		}),
			error => {
				console.log(error)
			}
	}

	onForward() {
		this.modelForward.content = this.modelForward.content.trim()
		this.submitted = true
		if (this.formForward.invalid) {
			return
		}
		this.modelForward.step = STEP_RECOMMENDATION.PROCESS
		this.modelForward.status = PROCESS_STATUS_RECOMMENDATION.WAIT
		var request = {
			_mRRecommendationForwardInsertIN: this.modelForward,
			RecommendationStatus: RECOMMENDATION_STATUS.PROCESS_WAIT,
		}
		this._service.recommendationForward(request).subscribe(response => {
			if (response.success == RESPONSE_STATUS.success) {
				$('#modal-tc-pakn').modal('hide')
				this.getList()
				this._toastr.success(COMMONS.FORWARD_SUCCESS)
			} else {
				this._toastr.error(response.message)
			}
		}),
			err => {
				console.error(err)
			}
	}
	modelProcess: RecommendationProcessObject = new RecommendationProcessObject()
	recommendationStatusProcess: number = 0

	preProcess(model: any, status: number) {
		this.modelProcess.status = status
		this.modelProcess.id = model.idProcess
		this.modelProcess.step = model.stepProcess
		this.modelProcess.recommendationId = model.id
		this.modelProcess.reactionaryWord = false
		this.modelProcess.reasonDeny = ''
		if (status == PROCESS_STATUS_RECOMMENDATION.DENY) {
			if (model.status == RECOMMENDATION_STATUS.RECEIVE_WAIT) {
				this.recommendationStatusProcess = RECOMMENDATION_STATUS.RECEIVE_DENY
			} else if (model.status == RECOMMENDATION_STATUS.PROCESS_WAIT) {
				this.recommendationStatusProcess = RECOMMENDATION_STATUS.PROCESS_DENY
			} else if (model.status == RECOMMENDATION_STATUS.APPROVE_WAIT) {
				this.recommendationStatusProcess = RECOMMENDATION_STATUS.APPROVE_DENY
			}
			$('#modalReject').modal('show')
		} else {
			if (model.status == RECOMMENDATION_STATUS.RECEIVE_WAIT) {
				this.recommendationStatusProcess = RECOMMENDATION_STATUS.RECEIVE_APPROVED
			} else if (model.status == RECOMMENDATION_STATUS.PROCESS_WAIT) {
				this.recommendationStatusProcess = RECOMMENDATION_STATUS.PROCESSING
			} else if (model.status == RECOMMENDATION_STATUS.APPROVE_WAIT) {
				this.recommendationStatusProcess = RECOMMENDATION_STATUS.FINISED
			}
			$('#modalAccept').modal('show')
		}
	}
	onProcessAccept() {
		var request = {
			_mRRecommendationForwardProcessIN: this.modelProcess,
			RecommendationStatus: this.recommendationStatusProcess,
			ReactionaryWord: this.modelProcess.reactionaryWord,
		}
		this._service.recommendationProcess(request).subscribe(response => {
			if (response.success == RESPONSE_STATUS.success) {
				$('#modalAccept').modal('hide')
				this._toastr.success(COMMONS.ACCEPT_SUCCESS)
				this.getList()
			} else {
				this._toastr.error(response.message)
			}
		}),
			err => {
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
				RecommendationStatus: RECOMMENDATION_STATUS.PROCESS_DENY,
			}
			this._service.recommendationProcess(request).subscribe(response => {
				if (response.success == RESPONSE_STATUS.success) {
					$('#modalReject').modal('hide')
					this._toastr.success(COMMONS.DENY_SUCCESS)
					this.getList()
				} else {
					this._toastr.error(response.message)
				}
			}),
				err => {
					console.error(err)
				}
		}
	}

	exportExcel() {
		let request = {
			IsActived: this.isActived,
		}

		this._service.recommendationExportExcel(request).subscribe(response => {
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
