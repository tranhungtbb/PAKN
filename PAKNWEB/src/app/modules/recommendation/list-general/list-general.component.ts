import { Component, OnInit, ViewChild } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { RecommendationForwardObject, RecommendationObject, RecommendationProcessObject, RecommendationSearchObject } from 'src/app/models/recommendationObject'
import { RecommendationService } from 'src/app/services/recommendation.service'
import { DataService } from 'src/app/services/sharedata.service'
import { saveAs as importedSaveAs } from 'file-saver'
import { MESSAGE_COMMON, PROCESS_STATUS_RECOMMENDATION, RECOMMENDATION_STATUS, RESPONSE_STATUS, STEP_RECOMMENDATION } from 'src/app/constants/CONSTANTS'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { COMMONS } from 'src/app/commons/commons'
import { Router } from '@angular/router'
import { NotificationService } from 'src/app/services/notification.service'
declare var $: any

@Component({
	selector: 'app-list-general',
	templateUrl: './list-general.component.html',
	styleUrls: ['./list-general.component.css'],
})
export class ListGeneralComponent implements OnInit {
	constructor(
		private _service: RecommendationService,
		private storeageService: UserInfoStorageService,
		private _fb: FormBuilder,
		private _toastr: ToastrService,
		private _router: Router,
		private _shareData: DataService,
		private notificationService: NotificationService
	) {}
	userLoginId: number = this.storeageService.getUserId()
	unitLoginId: number = this.storeageService.getUnitId()
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
	lstGroupWord: any = []
	lstGroupWordSelected: any = []
	formForward: FormGroup
	lstUnitNotMain: any = []
	lstUnit: any = []
	lstField: any = []
	dataSearch: RecommendationSearchObject = new RecommendationSearchObject()
	submitted: boolean = false
	isActived: boolean
	pageIndex: number = 1
	pageSize: number = 20
	@ViewChild('table', { static: false }) table: any
	totalRecords: number = 0
	idDelete: number = 0
	lstHistories: any = []
	dateNow: Date = new Date()
	titleAccept : any = ''
	titleAcceptTag : any = ''
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

	preDelete(id: number) {
		this.idDelete = id
		$('#modalConfirmDelete').modal('show')
	}

	onDelete(id: number) {
		let request = {
			Id: id,
		}
		this._service.recommendationDelete(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				this._toastr.success(MESSAGE_COMMON.DELETE_SUCCESS)
				$('#modalConfirmDelete').modal('hide')
				this.getList()
			} else {
				this._toastr.error(response.message)
			}
		}),
			(error) => {
				console.error(error)
			}
	}

	getHistories(id: number) {
		let request = {
			Id: id,
		}
		this._service.recommendationGetHistories(request).subscribe((response) => {
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
	preForward(id: number) {
		this.modelForward = new RecommendationForwardObject()
		this.modelForward.recommendationId = id
		this.submitted = false
		this.rebuilForm()
		this._service.recommendationGetDataForForward({}).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result != null) {
					this.lstUnitNotMain = response.result.lstUnitNotMain
					$('#modal-tc-pakn').modal('show')
				}
			} else {
				this._toastr.error(response.message)
			}
		}),
			(error) => {
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
			IsList: true,
		}
		this._service.recommendationForward(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				$('#modal-tc-pakn').modal('hide')
				this.getList()
				this.notificationService.insertNotificationTypeRecommendation({ recommendationId: this.modelProcess.recommendationId }).subscribe((res) => {})

				this._toastr.success(COMMONS.FORWARD_SUCCESS)
			} else {
				this._toastr.error(response.message)
			}
		}),
			(err) => {
				console.error(err)
			}
	}
	modelProcess: RecommendationProcessObject = new RecommendationProcessObject()
	recommendationStatusProcess: number = 0
	isForwardProcess: boolean = false
	contentForward: string = ''
	preProcess(model: any, status: number) {
		this.modelProcess.status = status
		this.modelProcess.id = model.processId
		this.modelProcess.recommendationId = model.id
		this.isForwardProcess = model.isForwardProcess
		this.modelProcess.reactionaryWord = false
		this.modelProcess.reasonDeny = ''
		if (status == PROCESS_STATUS_RECOMMENDATION.DENY) {
			if (model.status == RECOMMENDATION_STATUS.RECEIVE_WAIT) {
				this.recommendationStatusProcess = RECOMMENDATION_STATUS.RECEIVE_DENY
				this.modelProcess.step = STEP_RECOMMENDATION.RECEIVE
				this._service.recommendationGetDataForProcess({}).subscribe((response) => {
					if (response.success == RESPONSE_STATUS.success) {
						if (response.result != null) {
							this.lstGroupWord = response.result.lstGroupWord
							this.lstGroupWordSelected = []
							$('#modalReject').modal('show')
						}
					} else {
						this._toastr.error(response.message)
					}
				}),
					(error) => {
						console.log(error)
					}
			} else if (model.status == RECOMMENDATION_STATUS.PROCESS_WAIT) {
				this.recommendationStatusProcess = RECOMMENDATION_STATUS.PROCESS_DENY
				this.modelProcess.step = STEP_RECOMMENDATION.PROCESS
				if (this.isForwardProcess) {
					this._service.recommendationGetDataForProcess({}).subscribe((response) => {
						if (response.success == RESPONSE_STATUS.success) {
							if (response.result != null) {
								this.lstGroupWord = response.result.lstGroupWord
								this.lstGroupWordSelected = []
								$('#modalReject').modal('show')
							}
						} else {
							this._toastr.error(response.message)
						}
					}),
						(error) => {
							console.log(error)
						}
				} else {
					$('#modalReject').modal('show')
				}
			} else if (model.status == RECOMMENDATION_STATUS.APPROVE_WAIT) {
				this.recommendationStatusProcess = RECOMMENDATION_STATUS.APPROVE_DENY
				this.modelProcess.step = STEP_RECOMMENDATION.APPROVE
				$('#modalReject').modal('show')
			}
		} else if (status == PROCESS_STATUS_RECOMMENDATION.APPROVED) {
			if (model.status == RECOMMENDATION_STATUS.RECEIVE_WAIT) {
				this.titleAccept = 'Anh/Chị có chắc chắn muốn tiếp nhận Phản ánh, Kiến nghị này?'
				this.recommendationStatusProcess = RECOMMENDATION_STATUS.RECEIVE_APPROVED
				this.modelProcess.step = STEP_RECOMMENDATION.RECEIVE
			} else if (model.status == RECOMMENDATION_STATUS.PROCESS_WAIT) {
				this.titleAccept = 'Anh/Chị có chắc chắn muốn giải quyết Phản ánh, Kiến nghị này?'
				this.recommendationStatusProcess = RECOMMENDATION_STATUS.PROCESSING
				this.modelProcess.step = STEP_RECOMMENDATION.PROCESS
			} else if (model.status == RECOMMENDATION_STATUS.APPROVE_WAIT) {
				this.titleAccept = 'Anh/Chị có chắc chắn muốn phê duyệt Phản ánh, Kiến nghị này?'
				this.recommendationStatusProcess = RECOMMENDATION_STATUS.FINISED
				this.modelProcess.step = STEP_RECOMMENDATION.APPROVE
			}
			$('#modalAccept').modal('show')
		} else if (status == PROCESS_STATUS_RECOMMENDATION.FORWARD) {
			this.recommendationStatusProcess = RECOMMENDATION_STATUS.PROCESSING
			this.modelProcess.step = STEP_RECOMMENDATION.PROCESS
			this.contentForward = ''
			$('#modalForward').modal('show')
		}
	}
	onProcessAccept() {
		var request = {
			_mRRecommendationForwardProcessIN: this.modelProcess,
			RecommendationStatus: this.recommendationStatusProcess,
			ReactionaryWord: this.modelProcess.reactionaryWord,
			IsList: true,
		}
		this._service.recommendationProcess(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				$('#modalAccept').modal('hide')
				this.notificationService.insertNotificationTypeRecommendation({ recommendationId: this.modelProcess.recommendationId }).subscribe((res) => {})
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

	onProcessForward() {
		this.contentForward = this.contentForward == null ? '' : this.contentForward.trim();
		if(this.contentForward == ''){
			this._toastr.error('Vui lòng nhập lí do từ chối')
			return
		}
		this.modelProcess.reasonDeny = this.contentForward
		var request = {
			_mRRecommendationForwardProcessIN: this.modelProcess,
			RecommendationStatus: RECOMMENDATION_STATUS.RECEIVE_WAIT,
			ReactionaryWord: this.modelProcess.reactionaryWord,
			IsList: true,
			IsForwardProcess: this.isForwardProcess,
		}
		this._service.recommendationProcess(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				$('#modalForward').modal('hide')
				this.notificationService.insertNotificationTypeRecommendation({ recommendationId: this.modelProcess.recommendationId }).subscribe((res) => {})
				this._toastr.success(COMMONS.FORWARD_SUCCESS)
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
				RecommendationStatus: RECOMMENDATION_STATUS.PROCESS_DENY,
				ReactionaryWord: this.modelProcess.reactionaryWord,
				ListGroupWordSelected: this.lstGroupWordSelected.join(','),
				IsList: true,
			}
			this._service.recommendationProcess(request).subscribe((response) => {
				if (response.success == RESPONSE_STATUS.success) {
					this.notificationService.insertNotificationTypeRecommendation({ recommendationId: this.modelProcess.recommendationId }).subscribe((res) => {})
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
		passingObj.TitleReport = 'DANH SÁCH TỔNG HỢP'
		this._shareData.setobjectsearch(passingObj)
		this._shareData.sendReportUrl = 'Recommendation_ListGeneral?' + JSON.stringify(passingObj)
		this._router.navigate(['quan-tri/xuat-file'])
	}
}
