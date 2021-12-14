import { Component, OnInit, ViewChild } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { RecommendationForwardObject, RecommendationObject, RecommendationProcessObject, RecommendationSearchObject } from 'src/app/models/recommendationObject'
import { DataService } from 'src/app/services/sharedata.service'
import { saveAs as importedSaveAs } from 'file-saver'
import { MESSAGE_COMMON, PROCESS_STATUS_RECOMMENDATION, RECOMMENDATION_STATUS, RESPONSE_STATUS, STEP_RECOMMENDATION } from 'src/app/constants/CONSTANTS'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { COMMONS } from 'src/app/commons/commons'
import { AdministrativeFormalitiesService } from 'src/app/services/administrative-formalities.service'
import { RecommendationService } from 'src/app/services/recommendation.service'

declare var $: any

@Component({
	selector: 'app-list-administrative-formalities-publish',
	templateUrl: './list-administrative-formalities-publish.component.html',
	styleUrls: ['./list-administrative-formalities-publish.component.css'],
})
export class ListAdministrativeFormalitiesPublishComponent implements OnInit {
	constructor(
		private afService: AdministrativeFormalitiesService,
		private recommendationService: RecommendationService,
		private storeageService: UserInfoStorageService,
		private _fb: FormBuilder,
		private _toastr: ToastrService,
		private _shareData: DataService
	) {}
	userLoginId: number = this.storeageService.getUserId()
	listData: any = []
	listStatus: any = [
		{ value: 1, text: 'Đang soạn thảo' },
		{ value: 3, text: 'Đã công bố' },
		{ value: 2, text: 'Đã thu hồi' },
	]
	formForward: FormGroup
	lstUnitNotMain: any = []
	lstUnit: any = []
	lstField: any = []
	submitted: boolean = false
	isActived: boolean
	pageIndex: number = 1
	pageSize: number = 20
	@ViewChild('table', { static: false }) table: any
	totalRecords: number = 0
	idDelete: number = 0

	dataSearch = {
		code: '',
		name: '',
		title: '',
		object: '',
		organization: '',
		field: null,
		unitId: null,
		status: null,
	}

	ngOnInit() {
		this.getDataForCreate()
		this.getList()
	}

	ngAfterViewInit() {
		this._shareData.seteventnotificationDropdown()
	}

	getDataForCreate() {
		this.afService.getCAFieldDAM({}).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result != null) {
					this.lstField = response.result.CAFieldDAMGetDropdown
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

	getList() {
		this.dataSearch.code = this.dataSearch.code.trim()
		this.dataSearch.name = this.dataSearch.name.trim()
		this.dataSearch.object = this.dataSearch.object.trim()
		this.dataSearch.organization = this.dataSearch.organization.trim()
		let request = {
			Code: this.dataSearch.code,
			Name: this.dataSearch.name,
			Object: this.dataSearch.object,
			Organization: this.dataSearch.organization,
			UnitId: this.dataSearch.unitId != null ? this.dataSearch.unitId : '',
			Field: this.dataSearch.field != null ? this.dataSearch.field : '',
			Status: 3,
			PageIndex: this.pageIndex,
			PageSize: this.pageSize,
		}

		this.afService.getList(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result != null) {
					this.listData = []
					this.listData = response.result.DAMAdministrationGetList
					this.totalRecords = response.result.DAMAdministrationGetList[0] ? response.result.DAMAdministrationGetList[0].rowNumber : 0
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
		this.afService.delete(request).subscribe((response) => {
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
	idChangeStatus: number
	statusChange: number

	preChangeStatus(id: number, status: number) {
		this.idChangeStatus = id
		this.statusChange = status
		$('#modalConfirmChangeStatus').modal('show')
	}

	onChangeStatus() {
		const request = {
			Id: this.idChangeStatus,
			Status: this.statusChange,
		}
		let obj = this.listData.find((x) => x.administrationId == this.idChangeStatus)
		$('#modalConfirmChangeStatus').modal('hide')
		this.afService.updateShow(request, obj.name).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				this._toastr.success(COMMONS.UPDATE_SUCCESS)
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
