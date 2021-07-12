import { Component, OnInit, ViewChild } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { Router } from '@angular/router'
import { RecommendationRequestService } from 'src/app/services/recommendation-req.service'
import { DataService } from 'src/app/services/sharedata.service'
import {RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { COMMONS } from 'src/app/commons/commons'

declare var $: any

@Component({
	selector: 'app-list-recommendation-knct-public',
	templateUrl: './recommendation-knct.component.html',
	styleUrls: ['./recommendation-knct.component.css'],
})
export class ListRecommendationKnct implements OnInit {
	constructor(
		private _service: RecommendationRequestService,
		private storeageService: UserInfoStorageService,
		private _toastr: ToastrService,
		private _shareData: DataService,
		private _router : Router 
	) {}
	userLoginId: number = this.storeageService.getUserId()
	listData = []
	listStatus: any = [
		{ value: 1, text: 'Chưa giải quyết' },
		{ value: 2, text: 'Đang giải quyết' },
		{ value: 3, text: 'Đã giải quyết' },
	]
	lstFields: any = []
	keySearch = ''
	pageIndex: number = 1
	pageSize: number = 20
	@ViewChild('table', { static: false }) table: any
	totalRecords: number = 0
	dateNow: Date = new Date()
	ngOnInit() {
		this.getList()
	}

	ngAfterViewInit() {
		this._shareData.seteventnotificationDropdown()
	}



	getList() {
		this.keySearch = this.keySearch.trim()
		let request = {
			Content: this.keySearch,
			Unit: '',
			Place: '',
			Field: '',
			Status: 3, // đã giải quyết
			PageIndex: this.pageIndex,
			PageSize: this.pageSize,
		}
		this._service.recommendationGetListProcess(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result != null) {
					this.listData = []
					this.listData = response.result.MRRecommendationKNCTGetAllWithProcess
					console.log(this.listData)
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
	redirectCreateRecommendation() {
		this._router.navigate(['/cong-bo/them-moi-kien-nghi'])
	}


}
