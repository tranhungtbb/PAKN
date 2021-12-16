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
	) { }
	userLoginId: number = this.storeageService.getUserId()
	unitLoginId: number = this.storeageService.getUnitId()
	isMain: boolean = this.storeageService.getIsMain()
	listData: any = new Array<RecommendationObject>()
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
		{ value: 11, text: 'Đã chuyển' },
	]
	lstGroupWord: any = []
	lstGroupWordSelected: any = []
	formForward: FormGroup // form chuyển từ trung tâm xuống đơn vị khác
	lstUnitNotMain: any = []
	listUnitChild: any = []
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
	dateNow: Date = new Date()
	titleAccept: any = ''
	titleAcceptTag: any = ''
	modelForward: RecommendationForwardObject = new RecommendationForwardObject()
	ngOnInit() {
		this.buildForm()
		this.getDataForCreate()
		// this.getList()
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
					this.listUnitChild = response.result.lstUnitChild
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


	preForward(id: number, isForwardUnitChild: boolean) {
		this.modelForward = new RecommendationForwardObject()
		this.modelForward.recommendationId = id
		this.submitted = false
		this.rebuilForm()
		let obj = this.listData.find((x) => x.id == id)
		debugger
		if (isForwardUnitChild == true && obj.status == RECOMMENDATION_STATUS.PROCESS_DENY) {
			this.lstUnitNotMain = this.listUnitChild
			$('#modal-tc-pakn').modal('show')
		} else {
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
	}

	onForward() {
		this.modelForward.content = this.modelForward.content.trim()
		this.submitted = true
		if (this.formForward.invalid) {
			return
		}
		let obj = this.listData.find((x) => x.id == this.modelForward.recommendationId)

		this.modelForward.step = STEP_RECOMMENDATION.PROCESS
		if (obj.isForwardUnitChild) {
			this.modelForward.status = obj.status == RECOMMENDATION_STATUS.PROCESS_DENY ? PROCESS_STATUS_RECOMMENDATION.WAIT : PROCESS_STATUS_RECOMMENDATION.FORWARD
		} else {
			this.modelForward.status = PROCESS_STATUS_RECOMMENDATION.WAIT
		}
		var request = {
			_mRRecommendationForwardInsertIN: this.modelForward,
			RecommendationStatus: RECOMMENDATION_STATUS.PROCESS_WAIT,
			IsList: true,
		}

		this._service.recommendationForward(request, obj.title).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				$('#modal-tc-pakn').modal('hide')
				this.getList()
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
	isForwardMain: boolean = false // từ chối đơn vị và chuyển tiếp về trung tâm
	// contentForward: string = ''
	unitForward: any[] = []
	preProcess(model: any, status: number, isForwardMain: boolean = false) {
		this.modelProcess.status = status
		this.modelProcess.id = model.processId
		this.modelProcess.recommendationId = model.id
		this.isForwardProcess = model.isForwardProcess
		this.modelProcess.reactionaryWord = false
		this.modelProcess.reasonDeny = ''
		this.isForwardMain = isForwardMain
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
							setTimeout(() => {
								$('#targetReject').focus()
							}, 400)
						}
					} else {
						this._toastr.error(response.message)
					}
				}),
					(error) => {
						console.log(error)
					}
			} else if (model.status == RECOMMENDATION_STATUS.PROCESS_WAIT) {
				if (this.isForwardProcess) {
					this.recommendationStatusProcess = RECOMMENDATION_STATUS.RECEIVE_DENY
					this.modelProcess.step = STEP_RECOMMENDATION.RECEIVE
					this._service.recommendationGetDataForProcess({}).subscribe((response) => {
						if (response.success == RESPONSE_STATUS.success) {
							if (response.result != null) {
								this.lstGroupWord = response.result.lstGroupWord
								this.lstGroupWordSelected = []
								$('#modalReject').modal('show')
								setTimeout(() => {
									$('#targetReject').focus()
								}, 400)
							}
						} else {
							this._toastr.error(response.message)
						}
					}),
						(error) => {
							console.log(error)
						}
				} else {
					if (!model.isForwardUnitChild && model.isForwardForUnit) {
						this.recommendationStatusProcess = RECOMMENDATION_STATUS.RECEIVE_DENY
						this.modelProcess.step = STEP_RECOMMENDATION.RECEIVE
					} else {
						this.recommendationStatusProcess = RECOMMENDATION_STATUS.PROCESS_DENY
						this.modelProcess.step = STEP_RECOMMENDATION.PROCESS
					}
					$('#modalReject').modal('show')
					setTimeout(() => {
						$('#targetReject').focus()
					}, 400)
				}
			} else if (model.status == RECOMMENDATION_STATUS.APPROVE_WAIT) {
				this.recommendationStatusProcess = RECOMMENDATION_STATUS.APPROVE_DENY
				this.modelProcess.step = STEP_RECOMMENDATION.APPROVE
				$('#modalReject').modal('show')
				setTimeout(() => {
					$('#targetReject').focus()
				}, 400)
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
			debugger
			this.recommendationStatusProcess = RECOMMENDATION_STATUS.PROCESSING
			this.modelProcess.step = STEP_RECOMMENDATION.FORWARD_MAIN
			// this.contentForward = ''

			this._service.recommendationGetDataForForward({}).subscribe((response) => {
				if (response.success == RESPONSE_STATUS.success) {
					if (response.result != null) {
						this.unitForward = response.result.lstUnitForward
						this.modelForward = new RecommendationForwardObject()
						this.rebuilForm()
						this.submitted = false
						$('#modalForward').modal('show')
					}
				} else {
					this._toastr.error(response.message)
				}
			}),
				(error) => {
					console.log(error)
				}
			setTimeout(() => {
				$('#targetForward').focus()
			}, 400)
		}
	}
	onProcessAccept() {
		let obj = this.listData.find((x) => x.id == this.modelProcess.recommendationId)
		var request = {
			_mRRecommendationForwardProcessIN: this.modelProcess,
			RecommendationStatus: this.recommendationStatusProcess,
			ReactionaryWord: this.modelProcess.reactionaryWord,
			IsList: true,
			IsForwardUnitChild: obj.isForwardUnitChild,
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

	onProcessForward() {
		this.submitted = true
		this.modelForward.content = this.modelForward.content == null ? '' : this.modelForward.content.trim()
		if (this.formForward.invalid) {
			return
		}
		this.modelProcess.reasonDeny = this.modelForward.content
		this.modelProcess.unitReceiveId = this.modelForward.unitReceiveId
		var request = {
			_mRRecommendationForwardProcessIN: this.modelProcess,
			RecommendationStatus: RECOMMENDATION_STATUS.RECEIVE_WAIT,
			ReactionaryWord: this.modelProcess.reactionaryWord,
			IsList: true,
			IsForwardProcess: this.isForwardProcess,
		}
		let obj = this.listData.find((x) => x.id == this.modelProcess.recommendationId)
		this._service.recommendationProcess(request, obj.title).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				$('#modalForward').modal('hide')
				this.notificationService.insertNotificationTypeRecommendation({ recommendationId: this.modelProcess.recommendationId }).subscribe((res) => { })
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
			let obj = this.listData.find((x) => x.id == this.modelProcess.recommendationId)
			var request = {
				_mRRecommendationForwardProcessIN: this.modelProcess,
				RecommendationStatus: this.recommendationStatusProcess,
				ReactionaryWord: this.modelProcess.reactionaryWord,
				IsFakeImage: this.modelProcess.isFakeImage,
				ListGroupWordSelected: this.lstGroupWordSelected.join(','),
				IsForwardUnitChild: obj.isForwardUnitChild ? true : false,
				IsList: true,
				IsForwardMain: this.isForwardMain,
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
		passingObj.TitleReport = 'DANH SÁCH TỔNG HỢP'
		this._shareData.setobjectsearch(passingObj)
		this._shareData.sendReportUrl = 'Recommendation_ListGeneral?' + JSON.stringify(passingObj)
		this._router.navigate(['quan-tri/xuat-file'])
	}
}
