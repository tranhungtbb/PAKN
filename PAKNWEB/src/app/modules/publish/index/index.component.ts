import { Component, OnInit } from '@angular/core'
import { Router } from '@angular/router'

import { PuRecommendation } from 'src/app/models/recommendationObject'
import { PuRecommendationService } from 'src/app/services/pu-recommendation.service'
import { RECOMMENDATION_STATUS } from 'src/app/constants/CONSTANTS'

@Component({
	selector: 'app-index',
	templateUrl: './index.component.html',
	styleUrls: ['./index.component.css'],
})
export class IndexComponent implements OnInit {
	constructor(private _service: PuRecommendationService, private _router: Router) {}

	RecommendationsOrderByCountClick: Array<PuRecommendation>
	ReflectionsRecommendations: Array<PuRecommendation>

	ngOnInit() {
		this.getData()
	}
	getData() {
		// list recommendation order by count click
		this._service.getListOrderByCountClick({ status: RECOMMENDATION_STATUS.FINISED }).subscribe((res) => {
			if (res != undefined) {
				if (res.result) {
					this.RecommendationsOrderByCountClick = res.result
				}
			}
		})
		// list recommendation index
		let obj = {
			Status: RECOMMENDATION_STATUS.FINISED,
			PageSize: 4,
			PageIndex: 1,
		}
		this._service.getAllPagedList(obj).subscribe((res) => {
			if (res != undefined) {
				if (res.result) {
					this.ReflectionsRecommendations = res.result.PURecommendation.map((item) => {
						item.shortName = this.getShortName(item.name)
						return item
					})
				}
			}
		})

		//list news

		// list thủ tục hành chính
	}

	getShortName(string) {
		var names = string.split(' '),
			initials = names[0].substring(0, 1).toUpperCase()
		if (names.length > 1) {
			initials += names[names.length - 1].substring(0, 1).toUpperCase()
		}
		return initials
	}

	redirectDetailRecommendaton(id: any) {
		this._router.navigate(['/cong-bo/phan-anh-kien-nghi/' + id])
	}

	redirectReflectionsRecommendations() {
		this._router.navigate(['/cong-bo/phan-anh-kien-nghi'])
	}
}
