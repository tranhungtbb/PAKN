import { Component, OnInit, ViewChild } from '@angular/core'
import { ActivatedRoute, Router } from '@angular/router'
import { ToastrService } from 'ngx-toastr'

import { PuRecommendationService } from 'src/app/services/pu-recommendation.service'
import { RESPONSE_STATUS, RECOMMENDATION_STATUS } from 'src/app/constants/CONSTANTS'
import { PuRecommendation } from 'src/app/models/recommendationObject'
import { RecommendationService } from 'src/app/services/recommendation.service'

@Component({
	selector: 'app-unit-dissatisfaction-rate',
	templateUrl: './unit-dissatisfaction-rate.component.html',
	styleUrls: ['./unit-dissatisfaction-rate.component.css'],
})
export class UnitDissatisfactionRateComponent implements OnInit {
	// property
	public keySearch: string = ''
	public unitId: number = null
	public pageSize = 20
	public pageIndex = 1
	public total = 0

	lstUnit: [] = []

	listData : [] = []

	constructor(
		private activatedRoute: ActivatedRoute,
		private service: PuRecommendationService,
		private routers: Router,
		private recommendationService: RecommendationService,
		private _toas: ToastrService
	) {}

	async ngOnInit() {
		
		this.getList()

		this.recommendationService.recommendationGetDataForSearch({}).subscribe(
			(res) => {
				if (res.success == RESPONSE_STATUS.success) {
					this.lstUnit = res.result.lstUnit
				} else {
					this.lstUnit = []
					this._toas.error(res.message)
				}
			},
			(err) => {
				console.log(err)
			}
		)
	}

	dataStateChange = () => {
		this.getList()
	}

	getList() {
		this.keySearch = this.keySearch.trim()
		var obj = {
			KeySearch: this.keySearch,
			UnitId: this.unitId == null ? '' : this.unitId,
			PageSize: this.pageSize,
			PageIndex: this.pageIndex,
		}
		this.service.getUnitDissatisfactionRatePagedList(obj).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				if (res.result.listUnit.length > 0) {
					this.listData = res.result.listUnit
					this.pageIndex = res.result.PageIndex
					this.total = res.result.TotalCount
				} else {
					this.listData = []
					this.pageIndex = 1
					this.total = 0
				}
			} else {
				this.listData = []
				this.pageIndex = 1
				this.total = 0
			}
		})
	}
	
	redirectCreateRecommendation() {
		this.routers.navigate(['/cong-bo/them-moi-kien-nghi'])
	}
}
