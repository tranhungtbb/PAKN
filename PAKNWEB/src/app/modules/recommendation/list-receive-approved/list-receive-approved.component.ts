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
import { NotificationService } from 'src/app/services/notification.service'
import { Router } from '@angular/router'

declare var $: any

@Component({
	selector: 'app-list-receive-approved',
	templateUrl: './list-receive-approved.component.html',
	styleUrls: ['./list-receive-approved.component.css'],
})
export class ListReceiveApprovedComponent implements OnInit {
	constructor(
		private _service: RecommendationService,
		private storeageService: UserInfoStorageService,
		private _toastr: ToastrService,
		private _fb: FormBuilder,
		private _shareData: DataService,
		private notificationService: NotificationService,
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
	@ViewChild('table', { static: false }) table: any
	totalRecords: number = 0
	idDelete: number = 0
	lstHistories: any = []

	lstUnitNotMain: any = []
	modelForward: RecommendationForwardObject = new RecommendationForwardObject()
	formForward: FormGroup
	dateNow: Date = new Date()
	ngOnInit() {
		this.buildForm()
		this.dataSearch.status = RECOMMENDATION_STATUS.RECEIVE_APPROVED
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
	isForwardMultiUnit: boolean
	preForward(item: any, isForwardMultiUnit: boolean = false) {
		this.submitted = false
		this.isForwardMultiUnit = isForwardMultiUnit
		this.modelForward = new RecommendationForwardObject()
		this.modelForward.recommendationId = item.id
		this.modelForward.id = item.processId
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
		let obj = this.listData.find((x) => x.id == this.modelForward.recommendationId)
		if (!this.isForwardMultiUnit) {
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
		} else {
			let requestCombine = {
				RecommendationCombination: { ...this.modelForward, 'status': RECOMMENDATION_STATUS.PROCESS_WAIT },
				RecommendationStatus: RECOMMENDATION_STATUS.PROCESSING,
				ListUnit: this.modelForward.unitReceiveId,
				ProcessId: this.modelForward.id
			}
			requestCombine.RecommendationCombination.unitReceiveId = null


			this._service.recommendationCombineInsert(requestCombine, obj.title).subscribe((response) => {
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
	}

	preProcessAccept(item: any) {
		this.modelProcess = new RecommendationProcessObject()
		this.modelProcess.recommendationId = item.id
		this.modelProcess.id = item.processId
		$('#modalAccept').modal('show')
	}

	modelProcess: RecommendationProcessObject = new RecommendationProcessObject()

	onProcessAccept() {
		let obj = this.listData.find((x) => x.id == this.modelProcess.recommendationId)
		this.modelProcess.reactionaryWord = false
		this.modelProcess.status = PROCESS_STATUS_RECOMMENDATION.APPROVED
		this.modelProcess.step = STEP_RECOMMENDATION.PROCESS

		var request = {
			_mRRecommendationForwardProcessIN: this.modelProcess,
			RecommendationStatus: RECOMMENDATION_STATUS.PROCESSING,
			ReactionaryWord: this.modelProcess.reactionaryWord,
			IsList: true
		}
		this._service.recommendationProcess(request, obj.title).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				$('#modalAccept').modal('hide')
				this.notificationService.insertNotificationTypeRecommendation({ recommendationId: this.modelProcess.recommendationId }).subscribe((res) => { })
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
		passingObj.TitleReport = 'DANH SÁCH TIẾP NHẬN XỬ LÝ'
		this._shareData.setobjectsearch(passingObj)
		this._shareData.sendReportUrl = 'Recommendation_ListGeneral?' + JSON.stringify(passingObj)
		this._router.navigate(['quan-tri/xuat-file'])
	}
}
