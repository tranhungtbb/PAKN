import { Component, Input, OnInit } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { RecommendationService } from 'src/app/services/recommendation.service'
import { DataService } from 'src/app/services/sharedata.service'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'

declare var $: any
@Component({
	selector: 'app-history-recommendation',
	templateUrl: './history-recommendation.component.html',
	styleUrls: ['./history-recommendation.component.css'],
})
export class HistoryRecommendationComponent implements OnInit {
	constructor(
		private _service: RecommendationService,
		private _toastr: ToastrService,
		private _shareData: DataService
	) { }
	lstHistories: any = []

	@Input() recommendationId: number
	ngOnInit() {

	}


	ngAfterViewInit() {
		this._shareData.seteventnotificationDropdown()
	}
	checkShow: boolean = false

	getHistories() {
		this.checkShow = true
		let request = {
			Id: this.recommendationId
		}
		this._service.recommendationGetHistories(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				this.lstHistories = [...response.result.HISRecommendationGetByObjectId]
				$('#history-pakn').modal('show')
			} else {
				this._toastr.error(response.message)
			}
		}),
			(error) => {
				console.log(error)
			}
	}

}
