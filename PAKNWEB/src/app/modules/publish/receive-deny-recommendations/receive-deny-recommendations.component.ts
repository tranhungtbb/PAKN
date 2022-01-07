import { Component, OnInit, ViewChild } from '@angular/core'
import { ActivatedRoute, Router } from '@angular/router'
import { ToastrService } from 'ngx-toastr'

import { PuRecommendationService } from 'src/app/services/pu-recommendation.service'
import { RESPONSE_STATUS, RECOMMENDATION_STATUS } from 'src/app/constants/CONSTANTS'
import { PuRecommendation } from 'src/app/models/recommendationObject'
import { RecommendationService } from 'src/app/services/recommendation.service'

@Component({
	selector: 'app-receive-deny-recommendations',
	templateUrl: './receive-deny-recommendations.component.html',
	styleUrls: ['./receive-deny-recommendations.component.css'],
})
export class ReceiveDenyRecommendationsComponent implements OnInit {
	// property
	public KeySearch: string = ''
	public unitId: number = null
	public field: number = null
	public PageSize = 20
	public PageIndex = 1
	public Total = 0

	pagination = []
	// arr
	lstUnit: [] = []
	lstField: any[] = []

	listRecommendation = new Array<PuRecommendation>()

	constructor(
		private activatedRoute: ActivatedRoute,
		private service: PuRecommendationService,
		private routers: Router,
		private recommendationService: RecommendationService,
		private _toas: ToastrService
	) { }

	async ngOnInit() {

		this.recommendationService.recommendationGetDataForSearch({}).subscribe(
			(res) => {
				if (res.success == RESPONSE_STATUS.success) {
					this.lstUnit = res.result.lstUnit
					this.lstField = res.result.lstField
				} else {
					this.lstField = []
					this.lstUnit = []
					this._toas.error(res.message)
				}
				this.getList()
			},
			(err) => {
				console.log(err)
			}
		)
	}

	redirect(id: any) {
		this.routers.navigate(['/cong-bo/phan-anh-kien-nghi/' + id])
	}

	dataStateChange = () => {
		this.getList()
	}
	fieldName: string

	getList() {
		this.KeySearch = this.KeySearch.trim()
		var obj = {
			KeySearch: this.KeySearch,
			UnitId: this.unitId == null ? '' : this.unitId,
			FieldId: this.field == null ? '' : this.field,
			PageSize: this.PageSize,
			PageIndex: this.PageIndex,
		}
		if (this.field) {
			this.fieldName = this.lstField.find(x => x.value == this.field).text
		} else {
			this.fieldName = null
		}
		this.service.getRecommendationReceiveDeny(obj).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				if (res.result.RecommendationReceiveDeny.length > 0) {
					this.listRecommendation = res.result.RecommendationReceiveDeny.map((item) => {
						// item.shortName = this.getShortName(item.name)
						if (this.KeySearch) {
							item.title = this.highlight(item.title, this.KeySearch)
							item.content = this.highlight(item.content, this.KeySearch)
						}
						return item
					})

					this.PageIndex = res.result.PageIndex
					this.Total = res.result.TotalCount
					this.padi()
				} else {
					this.listRecommendation = this.pagination = []
					this.PageIndex = 1
					this.Total = 0
				}
			} else {
				this.listRecommendation = this.pagination = []
				this.PageIndex = 1
				this.Total = 0
			}
		})
	}

	highlight(inputText, text) {
		var index = inputText.toUpperCase().indexOf(text.toUpperCase());
		if (index >= 0) {
			inputText = inputText.substring(0, index) + "<span class='highlight-key-search'>" + inputText.substring(index, index + text.length) + "</span>" + inputText.substring(index + text.length);
		}
		return inputText
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
