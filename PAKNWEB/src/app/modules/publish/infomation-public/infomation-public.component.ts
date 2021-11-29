import { Component, OnInit, ViewChild } from '@angular/core'
import { ActivatedRoute, Router } from '@angular/router'
import { ToastrService } from 'ngx-toastr'

import { PuRecommendationService } from 'src/app/services/pu-recommendation.service'
import { RESPONSE_STATUS, RECOMMENDATION_STATUS } from 'src/app/constants/CONSTANTS'
import { PuRecommendation } from 'src/app/models/recommendationObject'
import { RecommendationService } from 'src/app/services/recommendation.service'

@Component({
	selector: 'app-infomation-public',
	templateUrl: './infomation-public.component.html',
	styleUrls: ['./infomation-public.component.css'],
})
export class InfomationPublicComponent implements OnInit {
	constructor(
		private service: PuRecommendationService,
		private routers: Router,
		private _toas: ToastrService
	) {}

	currentDate = new Date()
	async ngOnInit() {
		
	}
}
