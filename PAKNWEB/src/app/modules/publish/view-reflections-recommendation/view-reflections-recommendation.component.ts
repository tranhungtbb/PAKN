import { Component, OnInit } from '@angular/core'
import { ActivatedRoute } from '@angular/router'

import { PuRecommendationService } from 'src/app/services/pu-recommendation.service'
import { RESPONSE_STATUS, RECOMMENDATION_STATUS } from 'src/app/constants/CONSTANTS'
import { PuRecommendation } from 'src/app/models/recommendationObject'

@Component({
	selector: 'app-view-reflections-recommendations',
	templateUrl: './view-reflections-recommendation.component.html',
	styleUrls: ['./view-reflections-recommendation.component.css'],
})
export class ViewReflectionsRecommendationComponent implements OnInit {
	public id
	public model: any
	public lstFiles: any
	public lstConclusion: any
	public lstConclusionFiles: any
	constructor(private service: PuRecommendationService, private activatedRoute: ActivatedRoute) {}
	ngOnInit() {
		this.getRecommendationById()
	}

	getRecommendationById() {
		this.activatedRoute.params.subscribe((params) => {
			this.id = +params['id']
			if (this.id != 0) {
				// call api getRecommendation by id
				// tạm thời fix status = 3, nhưng thực tế status success = 10
				this.service.getById({ id: this.id, status: RECOMMENDATION_STATUS.RECEIVE_DENY }).subscribe((res) => {
					if (res.success == RESPONSE_STATUS.success) {
						if (res.result.model) {
							this.model = { ...res.result.model, shortName: this.getShortName(res.result.model.name) }
						}
						this.lstFiles = res.result.lstFiles
						this.lstConclusion = res.result.lstConclusion
						this.lstConclusionFiles = res.result.lstConclusionFiles
					}
				})
			}
		})
	}

	getShortName(string) {
		var names = string.split(' '),
			initials = names[0].substring(0, 1).toUpperCase()
		if (names.length > 1) {
			initials += names[names.length - 1].substring(0, 1).toUpperCase()
		}
		return initials
	}
}
