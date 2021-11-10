import { Component, OnInit, ViewChild } from '@angular/core'
import { Router } from '@angular/router'
import { DomSanitizer } from '@angular/platform-browser'

import { PuRecommendation } from 'src/app/models/recommendationObject'
import { PuRecommendationService } from 'src/app/services/pu-recommendation.service'
import { RECOMMENDATION_STATUS, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
declare var $: any

@Component({
	selector: 'app-list-recommendation',
	templateUrl: './list-recommendation.component.html',
	styleUrls: ['./list-recommendation.component.css'],
})
export class ListRecommendation2Component implements OnInit {
	constructor(private _service: PuRecommendationService, private _router: Router, private sanitizer: DomSanitizer) {}

	ReflectionsRecommendations: Array<PuRecommendation>

	ngOnInit() {}

	ngAfterViewInit() {}

	getShortName(string) {
		var names = string.split(' '),
			initials = names[0].substring(0, 1).toUpperCase()
		if (names.length > 1) {
			initials += names[names.length - 1].substring(0, 1).toUpperCase()
		}
		return initials
	}
}
