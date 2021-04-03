import { Component, OnInit } from '@angular/core'

import { PuRecommendationService } from 'src/app/services/pu-recommendation.service'
import { RESPONSE_STATUS, RECOMMENDATION_STATUS } from 'src/app/constants/CONSTANTS'
import { PuRecommendation } from 'src/app/models/recommendationObject'

@Component({
	selector: 'app-view-reflections-recommendations',
	templateUrl: './view-reflections-recommendation.component.html',
	styleUrls: ['./view-reflections-recommendation.component.css'],
})
export class ViewReflectionsRecommendationComponent implements OnInit {
	constructor() {}
	ngOnInit() {}
}
