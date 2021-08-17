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
import { UserService } from 'src/app/services/user.service'

declare var $: any

@Component({
	selector: 'app-list-administrative-forward',
	templateUrl: './list-administrative-forward.component.html',
	styleUrls: ['./list-administrative-forward.component.css'],
})
export class ListAdministrativeForwardComponent implements OnInit {
	constructor(
		private afService: AdministrativeFormalitiesService,
		private recommendationService: RecommendationService,
		private storeageService: UserInfoStorageService,
		private _toastr: ToastrService,
		private _shareData: DataService,
		private userUnit : UserService
	) {}
	listData : any
	listStatus: any = [
		{ value: 0, text: 'Đã gửi' },
		{ value: 1, text: 'Đã nhận' }
	]
	lstUnit: any = []
	lstField: any = []
	pageIndex: number = 1
	pageSize: number = 20
	@ViewChild('table', { static: false }) table: any
	totalRecords: number = 0

	dataSearch = {
		code: '',
		name: '',
		object: '',
		organization: '',
		field: null,
		unitForward: null,
		status: null,
	}

	ngOnInit() {
		this.getDataForCreate()
		this.getList()
		this.userUnit.getDataForCreate({}).subscribe(res=>{
			if(res.success == RESPONSE_STATUS.success){
				this.lstUnit = res.result.lstUnit
			}
			
		})
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

	getList() {
		this.dataSearch.code = this.dataSearch.code.trim()
		this.dataSearch.name = this.dataSearch.name.trim()
		this.dataSearch.object = this.dataSearch.object.trim()
		this.dataSearch.organization = this.dataSearch.organization.trim()
		let request = {
			Code: this.dataSearch.code,
			Name: this.dataSearch.name,
			Organization: this.dataSearch.organization,
			UnitForward: this.dataSearch.unitForward != null ? this.dataSearch.unitForward : '',
			FieldId: this.dataSearch.field != null ? this.dataSearch.field : '',
			Status: this.dataSearch.status != null ? this.dataSearch.status : '',
			PageIndex: this.pageIndex,
			PageSize: this.pageSize,
		}
		this.listData = []

		this.afService.getListForward(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result != null) {
					this.listData = response.result.DAMAdministrationForwardGetListOnPage
					console.log(this.listData)
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
