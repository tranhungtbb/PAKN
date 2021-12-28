import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { RecommendationObject, RecommendationProcessObject, RecommendationSearchObject, RecommendationForwardObject } from 'src/app/models/recommendationObject'
import { RecommendationService } from 'src/app/services/recommendation.service'
import { DataService } from 'src/app/services/sharedata.service'
import { saveAs as importedSaveAs } from 'file-saver'
import { MESSAGE_COMMON, PROCESS_STATUS_RECOMMENDATION, RECOMMENDATION_STATUS, RESPONSE_STATUS, STEP_RECOMMENDATION } from 'src/app/constants/CONSTANTS'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
// import { stat } from 'fs'
import { COMMONS } from 'src/app/commons/commons'
import { NotificationService } from 'src/app/services/notification.service'
import { Router } from '@angular/router'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'

declare var $: any

@Component({
	selector: 'app-list-process-wait',
	templateUrl: './list-process-wait.component.html',
	styleUrls: ['./list-process-wait.component.css'],
})
export class ListProcessWaitComponent implements OnInit, AfterViewInit {
	constructor(
		private _service: RecommendationService,
		private storeageService: UserInfoStorageService,
		private _toastr: ToastrService,
		private _shareData: DataService,
		private notificationService: NotificationService,
		private _router: Router,
		private _fb: FormBuilder
	) { }
	userLoginId: number = this.storeageService.getUserId()
	unitLoginId: number = this.storeageService.getUnitId()
	isMain: boolean = this.storeageService.getIsMain()
	listData = new Array<RecommendationObject>()
	formForward: FormGroup
	modelForward: RecommendationForwardObject = new RecommendationForwardObject()
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
	@ViewChild('table', { static: false }) table: any
	totalRecords: number = 0
	idDelete: number = 0
	lstGroupWord: any = []
	lstGroupWordSelected: any = []
	titleAccept: any = ''
	ngOnInit() {
		this.buildForm()
		this.buildFormAccept()
		this.dataSearch.status = RECOMMENDATION_STATUS.PROCESS_WAIT
		this.getDataForCreate()
		this.getList()
	}

	ngAfterViewInit() {
		this._shareData.seteventnotificationDropdown()
		$("#modalAcceptWithFiled").on('hide.bs.modal', function () {
			this.fieldSelected = null
		});
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
	isForwardProcess: boolean = false
	isForwardMain: boolean = false // từ chối đơn vị và chuyển tiếp về trung tâm
	unitForward: any[] = []
	recommendationStatusProcess: number = 0

	preProcess(model: any, idProcess, status, isForwardProcess, isForwardMain: boolean = false) {
		this.modelProcess.status = status
		this.modelProcess.id = idProcess
		this.modelProcess.step = STEP_RECOMMENDATION.PROCESS
		this.modelProcess.recommendationId = model.id
		this.modelProcess.reactionaryWord = false
		this.modelProcess.reasonDeny = ''
		this.isForwardProcess = isForwardProcess
		this.isForwardMain = isForwardMain

		if (status == PROCESS_STATUS_RECOMMENDATION.DENY) {
			if (this.isForwardProcess) {
				this.recommendationStatusProcess = RECOMMENDATION_STATUS.RECEIVE_DENY
				this.modelProcess.step = STEP_RECOMMENDATION.RECEIVE
				this._service.recommendationGetDataForProcess({}).subscribe((response) => {
					if (response.success == RESPONSE_STATUS.success) {
						if (response.result != null) {
							this.modelProcess.reactionaryWord = false
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
						alert(error)
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
		} else if (status == PROCESS_STATUS_RECOMMENDATION.APPROVED) {
			this.recommendationStatusProcess = RECOMMENDATION_STATUS.PROCESSING
			this.titleAccept = 'Anh/Chị có chắc chắn muốn giải quyết Phản ánh, Kiến nghị này?'
			if (isForwardProcess) {
				this.rebuilFormAccept()
				$('#modalAcceptWithFiled').modal('show')
			} else {
				$('#modalAccept').modal('show')
			}

		} else if (status == PROCESS_STATUS_RECOMMENDATION.FORWARD) {
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

	formAccept: FormGroup
	fieldSelected: number
	get fAccept() {
		return this.formAccept.controls
	}

	buildFormAccept() {
		this.formAccept = this._fb.group({
			field: [this.fieldSelected, Validators.required]
		})
	}

	rebuilFormAccept() {
		this.submitted = false
		this.fieldSelected = null
		this.formAccept.reset({
			field: this.fieldSelected
		})
	}


	onProcessAccept() {
		var request = {
			_mRRecommendationForwardProcessIN: this.modelProcess,
			RecommendationStatus: this.recommendationStatusProcess,
			ReactionaryWord: this.modelProcess.reactionaryWord,
			Field: this.fieldSelected,
			IsList: true,
		}
		let obj = this.listData.find((x) => x.id == this.modelProcess.recommendationId)
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

	onProcessAcceptWithField() {
		this.submitted = true
		if (!this.formAccept.valid) {
			return
		}
		let obj = this.listData.find((x) => x.id == this.modelProcess.recommendationId)
		var request = {
			_mRRecommendationForwardProcessIN: this.modelProcess,
			RecommendationStatus: this.recommendationStatusProcess,
			ReactionaryWord: this.modelProcess.reactionaryWord,
			IsList: true,
			Field: this.fieldSelected,
		}
		this._service.recommendationProcess(request, obj.title).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				$('#modalAcceptWithFiled').modal('hide')
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
				RecommendationStatus: this.recommendationStatusProcess,
				ReactionaryWord: this.modelProcess.reactionaryWord,
				IsFakeImage: this.modelProcess.isFakeImage,
				ListGroupWordSelected: this.lstGroupWordSelected.join(','),
				IsForwardUnitChild: true,
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

	onExport() {
		let passingObj: any = {}
		passingObj = this.dataSearch
		if (this.listData.length > 0) {
			passingObj.UnitProcessId = this.storeageService.getUnitId()
			passingObj.UserProcessId = this.storeageService.getUserId()
			passingObj.UserProcessName = this.storeageService.getFullName()
		}
		passingObj.TitleReport = 'DANH SÁCH CHỜ GIẢI QUYẾT'
		this._shareData.setobjectsearch(passingObj)
		this._shareData.sendReportUrl = 'Recommendation_ListGeneral?' + JSON.stringify(passingObj)
		this._router.navigate(['quan-tri/xuat-file'])
	}
}
