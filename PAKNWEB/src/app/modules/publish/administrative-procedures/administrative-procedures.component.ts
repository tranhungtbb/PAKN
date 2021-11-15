import { Component, OnInit, ViewChild } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { RecommendationObject } from 'src/app/models/recommendationObject'
import { DataService } from 'src/app/services/sharedata.service'
import { MESSAGE_COMMON, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { FormBuilder, FormGroup } from '@angular/forms'
import { AdministrativeFormalitiesService } from 'src/app/services/administrative-formalities.service'
import { RecommendationService } from 'src/app/services/recommendation.service'

declare var $: any

@Component({
	selector: 'app-administrative-procedures',
	templateUrl: './administrative-procedures.component.html',
	styleUrls: ['./administrative-procedures.component.css'],
})
export class AdministrativeProceduresComponent implements OnInit {
	constructor(
		private afService: AdministrativeFormalitiesService,
		private storeageService: UserInfoStorageService,
		private _toastr: ToastrService,
		private _shareData: DataService
	) {}
	userLoginId: number = this.storeageService.getUserId()
	listData = new Array<RecommendationObject>()
	listStatus: any = [
		{ value: 1, text: 'Đang soạn thảo' },
		{ value: 3, text: 'Đã công bố' },
		{ value: 2, text: 'Đã thu hồi' },
	]
	lstGrant: any = [
		{ value: 1, text: 'Cơ quan chuyên môn' },
		{ value: 2, text: 'Cấp tỉnh' },
		{ value: 3, text: 'Cấp huyện' },
		{ value: 4, text: 'Cấp xã' },
	]
	lstLevel: any = [
		{ value: 1, text: '1' },
		{ value: 2, text: '2' },
		{ value: 3, text: '3' },
		{ value: 4, text: '4' },
	]
	isBuuChinh: boolean
	isTTTT: boolean
	formForward: FormGroup
	lstUnitNotMain: any = []
	lstUnit: any = []
	lstField: any = []
	submitted: boolean = false
	isActived: boolean
	pageIndex: number = 1
	pageSize: number = 10
	@ViewChild('table', { static: false }) table: any
	totalRecords: number = 0
	idDelete: number = 0
	typeSelected: number = 1
	txtSearchDonVi: string = ''
	dataSearch = {
		name: '',
		rankReceiveId: null,
		level: null,
		unitId: null,
		field: null,
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
					this.lstUnit = response.result.CAUnitDAMGetDropdown
				}
			} else {
				this._toastr.error(response.message)
			}
		}),
			(error) => {
				console.log(error)
			}
	}

	getUnitDropdown() {
		this.afService.getCAUnitDAM({ Keyword: this.txtSearchDonVi }).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result != null) {
					this.lstUnit = response.result.CAUnitDAMGetDropdown
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
		this.dataSearch.name = this.dataSearch.name.trim()
		let request = {
			Name: this.dataSearch.name,
			RankReceiveId: this.dataSearch.rankReceiveId != null ? this.dataSearch.rankReceiveId : '',
			Level: this.dataSearch.level != null ? this.dataSearch.level : '',
			UnitId: this.dataSearch.unitId != null ? this.dataSearch.unitId : '',
			Field: this.dataSearch.field != null ? this.dataSearch.field : '',
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

	changeTypeSelected(type) {
		this.typeSelected = type
	}

	changeField(field: number) {
		this.dataSearch.unitId = null
		this.dataSearch.field = field
		this.dataStateChange()
	}

	changeUnit(unit: number) {
		this.dataSearch.field = null
		this.dataSearch.unitId = unit
		this.dataStateChange()
	}

	onPageChange(event: any) {
		this.pageSize = event.rows
		this.pageIndex = event.first / event.rows + 1
		this.getList()
	}

	dataStateChange() {
		this.pageIndex = 1
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
}
