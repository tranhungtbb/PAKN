import { Component, OnInit, ViewChild } from '@angular/core'
import { ActivatedRoute, Router } from '@angular/router'
import { ToastrService } from 'ngx-toastr'

import { PuRecommendationService } from 'src/app/services/pu-recommendation.service'
import { RESPONSE_STATUS, RECOMMENDATION_STATUS } from 'src/app/constants/CONSTANTS'
import { PuRecommendation } from 'src/app/models/recommendationObject'
import { RecommendationService } from 'src/app/services/recommendation.service'

@Component({
	selector: 'app-late-processing-unit',
	templateUrl: './late-processing-unit.component.html',
	styleUrls: ['./late-processing-unit.component.css'],
})
export class LateProcessingUnitComponent implements OnInit {
	// property
	public keySearch: string = ''
	public pageSize = 20
	public pageIndex = 1
	public total = 0

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
	
	}

	dataStateChange = () => {
		this.getList()
	}

	onPageChange(event: any) {
		this.pageSize = event.rows
		this.pageIndex = event.first / event.rows + 1
		this.getList()
	}

	getList() {
		this.keySearch = this.keySearch.trim()
		var obj = {
			KeySearch: this.keySearch,
			PageSize: this.pageSize,
			PageIndex: this.pageIndex,
		}
		this.service.getLateProcessingUnitPagedList(obj).subscribe((res) => {
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
