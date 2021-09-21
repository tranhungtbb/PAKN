import { Component, OnInit } from '@angular/core'
import { Router } from '@angular/router'
import { DomSanitizer } from '@angular/platform-browser'

import { PuRecommendation } from 'src/app/models/recommendationObject'
import { PuRecommendationService } from 'src/app/services/pu-recommendation.service'
import { RECOMMENDATION_STATUS, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { IndexSettingService } from 'src/app/services/index-setting.service'
import { IndexBanner, IndexWebsite } from 'src/app/models/indexSettingObject'

declare var $: any

@Component({
	selector: 'app-view-right',
	templateUrl: './view-right.component.html',
	styleUrls: ['./view-right.component.css'],
})
export class ViewRightComponent implements OnInit {
	constructor(private _service: PuRecommendationService, private _router: Router, private storageService: UserInfoStorageService, private indexSettingService: IndexSettingService) {
		this.ltsIndexSettingWebsite = []
		this.lstIndexSettingBanner = []
	}

	RecommendationsOrderByCountClick: Array<PuRecommendation>
	recommendationStatistics: any = new RecommendationStatistics()
	totalRecommentdation: number = 0
	isLogin: boolean = this.storageService.getIsHaveToken()
	lstIndexSettingBanner: Array<IndexBanner>
	bannerFinal: any = new IndexBanner()
	ltsIndexSettingWebsite: Array<IndexWebsite>
	typeUserLoginPublish: number = this.storageService.getTypeObject()

	ngOnInit() {
		this.getData()

		this.indexSettingService.GetInfo({}).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				this.ltsIndexSettingWebsite = res.result.lstSYIndexWebsite == null ? [] : res.result.lstSYIndexWebsite
				this.bannerFinal = res.result.lstIndexSettingBanner.pop()
				this.lstIndexSettingBanner = res.result.lstIndexSettingBanner == null ? [] : res.result.lstIndexSettingBanner
			}
		}),
			(error) => {
				console.log(error)
				alert(error)
			}
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
		if (this.isLogin) {
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
	}

	redirectDetailRecommendaton(id: any) {
		this._router.navigate(['/cong-bo/phan-anh-kien-nghi/' + id])
	}

	redirectMyRecommendaton(status: any) {
		this._router.navigate(['/cong-bo/phan-anh-kien-nghi-cua-toi/' + status])
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
