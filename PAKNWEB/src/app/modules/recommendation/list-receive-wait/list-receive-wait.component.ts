import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { RecommendationObject, RecommendationProcessObject, RecommendationSearchObject } from 'src/app/models/recommendationObject'
import { RecommendationService } from 'src/app/services/recommendation.service'
import { DataService } from 'src/app/services/sharedata.service'
import { PROCESS_STATUS_RECOMMENDATION, RECOMMENDATION_STATUS, RESPONSE_STATUS, STEP_RECOMMENDATION } from 'src/app/constants/CONSTANTS'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { COMMONS } from 'src/app/commons/commons'
import { NotificationService } from 'src/app/services/notification.service'
import { Router } from '@angular/router'
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms'

declare var $: any

@Component({
	selector: 'app-list-receive-wait',
	templateUrl: './list-receive-wait.component.html',
	styleUrls: ['./list-receive-wait.component.css'],
})
export class ListReceiveWaitComponent implements OnInit, AfterViewInit {
	constructor(
		private _service: RecommendationService,
		private storeageService: UserInfoStorageService,
		private _toastr: ToastrService,
		private _shareData: DataService,
		private notificationService: NotificationService,
		private _router: Router,
		private _fb: FormBuilder,
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
	lstGroupWord: any = []
	lstGroupWordSelected: any = []
	dataSearch: RecommendationSearchObject = new RecommendationSearchObject()
	submitted: boolean = false
	isActived: boolean
	pageIndex: number = 1
	pageSize: number = 20
	@ViewChild('table', { static: false }) table: any
	totalRecords: number = 0
	idDelete: number = 0

	ngOnInit() {
		this.dataSearch.status = RECOMMENDATION_STATUS.RECEIVE_WAIT
		this.getDataForCreate()
		this.getList()
		this.buildForm()
	}

	ngAfterViewInit() {
		this._shareData.seteventnotificationDropdown()
		$("#modalAcceptWithFiled").on('hide.bs.modal', function () {
			this.fieldSelected = null
		});
	}

	formAccept: FormGroup
	get fAccept() {
		return this.formAccept.controls
	}

	buildForm() {
		this.formAccept = this._fb.group({
			field: [this.fieldSelected, Validators.required]
		})
	}

	rebuilForm() {
		this.formAccept.reset({
			field: this.fieldSelected
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
	preProcess(recommendationId, idProcess, status) {
		this.modelProcess.status = status
		this.modelProcess.id = idProcess
		this.modelProcess.step = STEP_RECOMMENDATION.RECEIVE
		this.modelProcess.recommendationId = recommendationId
		this.modelProcess.reactionaryWord = false
		this.modelProcess.reasonDeny = ''
		if (status == PROCESS_STATUS_RECOMMENDATION.DENY) {
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
					alert(error)
				}
		} else {
			this.submitted = false
			this.fieldSelected = null
			this.rebuilForm()
			$('#modalAccept').modal('show')
		}
	}
	fieldSelected: number
	onProcessAccept() {
		this.submitted = true
		if (!this.formAccept.valid) {
			return
		}
		var request = {
			_mRRecommendationForwardProcessIN: this.modelProcess,
			RecommendationStatus: RECOMMENDATION_STATUS.RECEIVE_APPROVED,
			ReactionaryWord: this.modelProcess.reactionaryWord,
			Field: this.fieldSelected,
			IsList: true,
		}
		let obj = this.listData.find((x) => x.id == this.modelProcess.recommendationId)
		this._service.recommendationProcess(request, obj.title).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				$('#modalAccept').modal('hide')
				this._toastr.success(COMMONS.ACCEPT_SUCCESS)
				// this.getList()
				this._router.navigate(['/quan-tri/kien-nghi/chi-tiet/', obj.id])
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
				RecommendationStatus: RECOMMENDATION_STATUS.RECEIVE_DENY,
				ReactionaryWord: this.modelProcess.reactionaryWord,
				IsFakeImage: this.modelProcess.isFakeImage,
				ListGroupWordSelected: this.lstGroupWordSelected.join(','),
				IsList: true,
			}
			let obj = this.listData.find((x) => x.id == this.modelProcess.recommendationId)
			this._service.recommendationProcess(request, obj.title).subscribe((response) => {
				if (response.success == RESPONSE_STATUS.success) {
					$('#modalReject').modal('hide')
					this._toastr.success(COMMONS.DENY_SUCCESS)
					// this.getList()
					this._router.navigate(['/quan-tri/kien-nghi/chi-tiet/', obj.id])
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
		passingObj.TitleReport = 'DANH SÁCH CHỜ XỬ LÝ'
		this._shareData.setobjectsearch(passingObj)
		this._shareData.sendReportUrl = 'Recommendation_ListGeneral?' + JSON.stringify(passingObj)
		this._router.navigate(['quan-tri/xuat-file'])
	}
}
