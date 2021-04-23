import { Component, OnInit, ViewChild } from '@angular/core'
import { Router } from '@angular/router'

import { PuRecommendationService } from 'src/app/services/pu-recommendation.service'
import { RESPONSE_STATUS, RECOMMENDATION_STATUS } from 'src/app/constants/CONSTANTS'
import { PuRecommendation } from 'src/app/models/recommendationObject'
import { ViewRightComponent } from 'src/app/modules/publish/view-right/view-right.component'

@Component({
	selector: 'app-reflections-recommendations',
	templateUrl: './reflections-recommendations.component.html',
	styleUrls: ['./reflections-recommendations.component.css'],
})
export class ReflectionsRecommendationsComponent implements OnInit {
	// property
	@ViewChild(ViewRightComponent, { static: true }) viewRightComponent: ViewRightComponent
	public KeySearch: string = ''
	public PageSize = 20
	public PageIndex = 1
	public Total = 0

	pagination = []

	// arr

	ReflectionsRecommendations = new Array<PuRecommendation>()
	RecommendationsOrderByCountClick: any[]

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
			Status: RECOMMENDATION_STATUS.FINISED, //FINISED
			PageSize: this.PageSize,
			PageIndex: this.PageIndex,
		}
		this.service.getAllPagedList(obj).subscribe((res) => {
			if (res != 'undefined' && res.success == RESPONSE_STATUS.success) {
				if (res.result.PURecommendation.length > 0) {
					this.ReflectionsRecommendations = res.result.PURecommendation.map((item) => {
						item.shortName = this.getShortName(item.name)
						return item
					})
					this.PageIndex = res.result.PageIndex
					this.Total = res.result.TotalCount
					this.padi()
				} else {
					this.ReflectionsRecommendations = this.pagination = []
					this.PageIndex = 1
					this.Total = 0
				}
			} else {
				this.ReflectionsRecommendations = this.pagination = []
				this.PageIndex = 1
				this.Total = 0
			}
		})

		this.service.getListOrderByCountClick({ status: RECOMMENDATION_STATUS.FINISED }).subscribe((res) => {
			if (res != undefined) {
				if (res.result) {
					this.RecommendationsOrderByCountClick = res.result
				}
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
		if (this.PageIndex > index) {
			if (index > 0) {
				this.PageIndex = index
				this.getList()
			}
			return
		} else if (this.PageIndex < index) {
			if (this.pagination.length >= index) {
				this.PageIndex = index
				this.getList()
			}
			return
		}
		return
	}
	redirectCreateRecommendation() {
		this.routers.navigate(['/cong-bo/them-moi-kien-nghi'])
	}
}
