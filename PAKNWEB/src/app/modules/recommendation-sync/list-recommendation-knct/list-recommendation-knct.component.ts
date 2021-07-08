import { Component, OnInit, ViewChild } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { RecommendationForwardObject, RecommendationObject, RecommendationProcessObject, RecommendationSearchObject } from 'src/app/models/recommendationObject'
import { RecommendationRequestService } from 'src/app/services/recommendation-req.service'
import { DataService } from 'src/app/services/sharedata.service'
import { RESPONSE_STATUS, STEP_RECOMMENDATION } from 'src/app/constants/CONSTANTS'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { FormBuilder } from '@angular/forms'

declare var $: any

@Component({
	selector: 'app-list-recommendation-knct',
	templateUrl: './list-recommendation-knct.component.html',
	styleUrls: ['./list-recommendation-knct.component.css'],
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
	lstFields: any = []
	dataSearch: RecommendationSearchObject = new RecommendationSearchObject()
	pageIndex: number = 1
	pageSize: number = 20
	@ViewChild('table', { static: false }) table: any
	totalRecords: number = 0
	dateNow: Date = new Date()
	ngOnInit() {
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
					this.lstFields = response.result.CAFieldKNCTGetDropdown
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
		this.dataSearch.content = this.dataSearch.content.trim()
		this.dataSearch.unit = this.dataSearch.unit.trim()
		this.dataSearch.place = this.dataSearch.place.trim()
		let request = {
			Code: this.dataSearch.code,
			Content: this.dataSearch.content,
			Unit: this.dataSearch.unit,
			Place: this.dataSearch.place,
			Field: this.dataSearch.field != null ? this.dataSearch.field : '',
			Status: this.dataSearch.status != null ? this.dataSearch.status : '',
			PageIndex: this.pageIndex,
			PageSize: this.pageSize,
		}
		this._service.recommendationGetListProcess(request).subscribe((response) => {
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

	requestSync = () =>{
		this._service.asyncRecommentdationKNCT().subscribe(res => {
			if(res.success == RESPONSE_STATUS.success){
				this._toastr.success("Đồng bộ thành công")
			}else{
				this._toastr.error("Đồng bộ thất bại")
			}
		}), (err => {
			console.log(err)
		})
	}


}
