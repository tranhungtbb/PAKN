import { Component, OnInit } from '@angular/core'
import { Router } from '@angular/router'
import { DomSanitizer } from '@angular/platform-browser'

import { PuRecommendation } from 'src/app/models/recommendationObject'
import { PuRecommendationService } from 'src/app/services/pu-recommendation.service'
import { RECOMMENDATION_STATUS, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'

declare var $: any

@Component({
	selector: 'app-view-right',
	templateUrl: './view-right.component.html',
	styleUrls: ['./view-right.component.css'],
})
export class ViewRightComponent implements OnInit {
	constructor(private _service: PuRecommendationService, private _router: Router, private sanitizer: DomSanitizer, private storageService: UserInfoStorageService) {}

	RecommendationsOrderByCountClick: Array<PuRecommendation>
	recommendationStatistics: any = new RecommendationStatistics()
	totalRecommentdation: number = 0
	isLogin: boolean = this.storageService.getIsHaveToken()
	ngOnInit() {
		this.getData()
	}
	async getData() {
		// list recommendation order by count click
		this._service.getListOrderByCountClick({ status: RECOMMENDATION_STATUS.FINISED }).subscribe((res) => {
			if (res != undefined) {
				if (res.result) {
					this.RecommendationsOrderByCountClick = res.result
				}
			}
		})
		debugger
		this._service.recommendationStatisticsGetByUserId({}).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success && res.result != null) {
				this.recommendationStatistics = res.result.PURecommendationStatisticsGetByUserId[0]
				for (const iterator in this.recommendationStatistics) {
					this.totalRecommentdation += this.recommendationStatistics[iterator]
				}
			}
			return
		})
	}

	redirectDetailRecommendaton(id: any) {
		this._router.navigate(['/cong-bo/phan-anh-kien-nghi/' + id])
	}

	Percent(value: any) {
		var result = Math.ceil((value / this.totalRecommentdation) * 100)
		return result
	}
}

class RecommendationStatistics {
	approve: Number
	finised: Number
	receiveApproved: Number
	receiveWait: Number
}
