import { Component, OnInit } from '@angular/core'
import { Router } from '@angular/router'

import { PuRecommendationService } from 'src/app/services/pu-recommendation.service'
import { RESPONSE_STATUS, RECOMMENDATION_STATUS } from 'src/app/constants/CONSTANTS'
import { PuRecommendation } from 'src/app/models/recommendationObject'

@Component({
	selector: 'app-reflections-recommendations',
	templateUrl: './reflections-recommendations.component.html',
	styleUrls: ['./reflections-recommendations.component.css'],
})
export class ReflectionsRecommendationsComponent implements OnInit {
	// property
	public KeySearch: string = ''
	public PageSize = 1
	public PageIndex = 1
	public Total = 0

	pagination = []

	// arr

	ReflectionsRecommendations = new Array<PuRecommendation>()

	constructor(private service: PuRecommendationService, private routers: Router) {}

	ngOnInit() {
		this.getList()
	}

	redirect(id: any) {
		this.routers.navigate(['/cong-bo/phan-anh-kien-nghi/' + id])
	}

	changeKeySearch(event) {
		this.KeySearch = event.target.value
	}

	getList() {
		var obj = {
			KeySearch: this.KeySearch,
			Code: RECOMMENDATION_STATUS.RECEIVE_DENY, //FINISED
			PageSize: this.PageSize,
			PageIndex: this.PageIndex,
		}
		this.service.getAllPagedList(obj).subscribe((res) => {
			if (res != 'undefined' && res.success == RESPONSE_STATUS.success) {
				if (res.result) {
					this.ReflectionsRecommendations = res.result.PURecommendation.map((item) => {
						item.shortName = this.getShortName(item.name)
						return item
					})
					this.PageIndex = res.result.PageIndex
					this.Total = res.result.TotalCount
					this.padi()
				} else {
					this.ReflectionsRecommendations = []
					this.PageIndex = 0
					this.Total = 0
				}
			} else {
				this.ReflectionsRecommendations = []
				this.PageIndex = 0
				this.Total = 0
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

	padi() {
		this.pagination = []
		for (let i = 0; i < Math.ceil(this.Total / this.PageSize); i++) {
			this.pagination.push({ index: i + 1 })
		}
	}

	changePagination(index: any) {
		if (this.PageIndex != index && index > 0) {
			this.PageIndex = index
			this.getList()
		}
		return
	}
}
