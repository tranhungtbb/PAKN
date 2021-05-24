import { Component, OnInit } from '@angular/core'
import { RecommendationService } from 'src/app/services/recommendation.service'
import { ToastrService } from 'ngx-toastr'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { MESSAGE_COMMON, PROCESS_STATUS_RECOMMENDATION, RECOMMENDATION_STATUS, RESPONSE_STATUS, STEP_RECOMMENDATION } from 'src/app/constants/CONSTANTS'
declare var $: any

@Component({
	selector: 'app-recommnendation-get-list',
	templateUrl: './recommnendation-get-list.component.html',
	styleUrls: ['./recommnendation-get-list.component.css'],
})
export class RecommnendationGetListComponent implements OnInit {
	constructor(private _service: RecommendationService, private _toast: ToastrService, private storeageService: UserInfoStorageService) {}

	listData: any[]
	dataSearch: any = {}
	totalRecords: number = 0

	ngOnInit() {
		this.getList()
	}

	getList() {
		let request = {
			Status: 5,
			PageIndex: 1,
			PageSize: 4,
			UnitProcessId: this.storeageService.getUnitId(),
			UserProcessId: this.storeageService.getUserId(),
		}

		this._service.recommendationGetListProcess(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result != null) {
					this.listData = []
					this.listData = response.result.MRRecommendationGetAllWithProcess
					this.totalRecords = response.result.TotalCount
				}
			} else {
				this._toast.error(response.message)
			}
		}),
			(error) => {
				console.log(error)
				alert(error)
			}
	}
}
