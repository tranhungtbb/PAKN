import { Component, OnInit, ViewChild } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { RecommendationObject, RecommendationSearchObject, RecommendationForwardObject } from 'src/app/models/recommendationObject'
import { RecommendationService } from 'src/app/services/recommendation.service'
import { DataService } from 'src/app/services/sharedata.service'
import { saveAs as importedSaveAs } from 'file-saver'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { Router } from '@angular/router'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { PROCESS_STATUS_RECOMMENDATION, RECOMMENDATION_STATUS, RESPONSE_STATUS, STEP_RECOMMENDATION } from 'src/app/constants/CONSTANTS'
import { COMMONS } from 'src/app/commons/commons'
import { UnitService } from 'src/app/services/unit.service'
declare var $: any

@Component({
	selector: 'app-list-processing',
	templateUrl: './list-processing.component.html',
	styleUrls: ['./list-processing.component.css'],
})
export class ListProcessingComponent implements OnInit {
	constructor(
		private _service: RecommendationService,
		private storeageService: UserInfoStorageService,
		private _toastr: ToastrService,
		private _shareData: DataService,
		private _router: Router,
		private _fb: FormBuilder,
		private _unitService: UnitService
	) { }
	userLoginId: number = this.storeageService.getUserId()
	unitId: number = this.storeageService.getUnitId()
	isMain: boolean = this.storeageService.getIsMain()
	isUnitMain: boolean = this.storeageService.getIsUnitMain()
	listData = new Array<RecommendationObject>()
	formForward: FormGroup
	formCombine: FormGroup
	dateNow: Date = new Date()
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
	ngOnInit() {
		this.dataSearch.status = RECOMMENDATION_STATUS.PROCESSING
		this.getDataForCreate()
		this.getList()
		this.buildForm()
		this.buildCombineForm()
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
					this.lstUnitChild = response.result.lstUnitChild
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


	modelForward: any = new RecommendationForwardObject()
	lstUnitChild: any = []

	buildForm() {
		this.formForward = this._fb.group({
			unitReceiveId: [this.modelForward.unitReceiveId, Validators.required],
			expiredDate: [this.modelForward.expiredDate],
			content: [this.modelForward.content],
		})
	}
	get f() {
		return this.formForward.controls
	}

	rebuilForm() {
		this.formForward.reset({
			unitReceiveId: this.modelForward.unitReceiveId,
			expiredDate: this.modelForward.expiredDate,
			content: this.modelForward.content,
		})
	}

	buildCombineForm() {
		this.formCombine = this._fb.group({
			unitReceiveId: [this.modelForward.unitReceiveId, Validators.required],
			expiredDate: [this.modelForward.expiredDate],
			content: [this.modelForward.content],
		})
	}
	get fCombine() {
		return this.formCombine.controls
	}

	rebuilCombineForm() {
		this.formCombine.reset({
			unitReceiveId: this.modelForward.unitReceiveId,
			expiredDate: this.modelForward.expiredDate,
			content: this.modelForward.content,
		})
	}

	preForward(id: number) {
		this.modelForward = new RecommendationForwardObject()
		this.modelForward.recommendationId = id
		this.submitted = false
		this.rebuilForm()
		$('#modal-tc-pakn').modal('show')
	}

	onForward() {
		this.modelForward.content = this.modelForward.content.trim()
		this.submitted = true
		if (this.formForward.invalid) {
			return
		}
		this.modelForward.step = STEP_RECOMMENDATION.PROCESS
		this.modelForward.status = PROCESS_STATUS_RECOMMENDATION.FORWARD
		var request = {
			_mRRecommendationForwardInsertIN: this.modelForward,
			RecommendationStatus: RECOMMENDATION_STATUS.PROCESS_WAIT,
			IsForwardUnitChild: true,
			IsList: true,
		}
		let obj = this.listData.find((x) => x.id == this.modelForward.recommendationId)
		// chuyển tiếp đơn vị con
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

	listUnitCombine: any = []
	preProcessCombine(item: any) {
		this.modelForward = new RecommendationForwardObject()
		this.modelForward.recommendationId = item.id
		this.modelForward.id = item.processId
		this.submitted = false
		this.rebuilCombineForm()

		this._unitService.getDropdownForCombine({ RecommendationId: item.id }).subscribe(res => {
			this.listUnitCombine = res.result
			$('#modal-combine').modal('show')
		}, err => {
			console.log(err)
		})


	}

	onProcessCombine() {
		this.modelForward.content = this.modelForward.content.trim()
		this.submitted = true
		if (this.formCombine.invalid) {
			return
		}
		this.modelForward.step = STEP_RECOMMENDATION.PROCESS
		this.modelForward.status = RECOMMENDATION_STATUS.PROCESSING

		let obj = this.listData.find((x) => x.id == this.modelForward.recommendationId)
		let requestCombine = {
			RecommendationCombination: { ...this.modelForward, 'unitReceiveId': null },
			RecommendationStatus: RECOMMENDATION_STATUS.PROCESSING,
			ListUnit: this.modelForward.unitReceiveId,
			ProcessId: this.modelForward.id
		}

		this._service.recommendationCombineInsert(requestCombine, obj.title).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				$('#modal-combine').modal('hide')
				this._toastr.success(COMMONS.FORWARD_SUCCESS)
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
		passingObj.TitleReport = 'DANH SÁCH ĐANG GIẢI QUYẾT'
		this._shareData.setobjectsearch(passingObj)
		this._shareData.sendReportUrl = 'Recommendation_ListGeneral?' + JSON.stringify(passingObj)
		this._router.navigate(['quan-tri/xuat-file'])
	}
}
