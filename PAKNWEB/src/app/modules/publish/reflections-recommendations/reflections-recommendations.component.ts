import { Component, OnInit, ViewChild } from '@angular/core'
import { ActivatedRoute, Router } from '@angular/router'
import { ToastrService } from 'ngx-toastr'

import { PuRecommendationService } from 'src/app/services/pu-recommendation.service'
import { RESPONSE_STATUS, RECOMMENDATION_STATUS } from 'src/app/constants/CONSTANTS'
import { PuRecommendation } from 'src/app/models/recommendationObject'
import { RecommendationService } from 'src/app/services/recommendation.service'

@Component({
	selector: 'app-reflections-recommendations',
	templateUrl: './reflections-recommendations.component.html',
	styleUrls: ['./reflections-recommendations.component.css'],
})
export class ReflectionsRecommendationsComponent implements OnInit {
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

	ReflectionsRecommendations = new Array<PuRecommendation>()

	constructor(
		private activatedRoute: ActivatedRoute,
		private service: PuRecommendationService,
		private routers: Router,
		private recommendationService: RecommendationService,
		private _toas: ToastrService
	) { }

	async ngOnInit() {
		await this.activatedRoute.params.subscribe((params) => {
			let s = +params['field']
			if (s) {
				this.field = s
			}
			let key = params['keysearch']
			if (key) {
				this.KeySearch = key
			}
		})
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
		this.service.getAllPagedList(obj).subscribe((res) => {
			if (res != 'undefined' && res.success == RESPONSE_STATUS.success) {
				if (res.result.PURecommendation.length > 0) {
					this.ReflectionsRecommendations = res.result.PURecommendation.map((item) => {
						item.shortName = this.getShortName(item.name)

						// if (this.checkIncludeString(item.title)) {
						// 	item.title = item.title.replaceAll(this.KeySearch, '<span class ="txthighlight">' + this.KeySearch + '</span>')
						// }

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
	}

	checkIncludeString(title: string) {
		if (this.KeySearch && title.toUpperCase().indexOf(this.KeySearch.toUpperCase()) != -1) {
			return true
		}
		return false
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
