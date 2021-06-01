import { Component, OnInit, ViewChild } from '@angular/core'
import { Router } from '@angular/router'
import { DomSanitizer } from '@angular/platform-browser'

import { PuRecommendation } from 'src/app/models/recommendationObject'
import { PuRecommendationService } from 'src/app/services/pu-recommendation.service'
import { NewsService } from 'src/app/services/news.service'
import { AdministrativeFormalitiesService } from 'src/app/services/administrative-formalities.service'
import { ViewRightComponent } from 'src/app/modules/publish/view-right/view-right.component'
import { RECOMMENDATION_STATUS, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { IndexSettingService } from 'src/app/services/index-setting.service'
import { IndexSettingObjet } from 'src/app/models/indexSettingObject'

declare var $: any

@Component({
	selector: 'app-index',
	templateUrl: './index.component.html',
	styleUrls: ['./index.component.css'],
})
export class IndexComponent implements OnInit {
	constructor(
		private _service: PuRecommendationService,
		private _router: Router,
		private _newsService: NewsService,
		private _serviceAdministrative: AdministrativeFormalitiesService,
		private sanitizer: DomSanitizer,
		private indexSettingService: IndexSettingService
	) {}
	@ViewChild(ViewRightComponent, { static: true }) viewRightComponent: ViewRightComponent

	RecommendationsOrderByCountClick: Array<PuRecommendation>
	ReflectionsRecommendations: Array<PuRecommendation>
	news: any[]
	firstNews: any
	Administrations: any[]

	indexSettingObj = new IndexSettingObjet()
	ngOnInit() {
		this.getData()
		this.indexSettingService.GetInfo({}).subscribe(
			(res) => {
				if (res.success == RESPONSE_STATUS.success) {
					this.indexSettingObj = res.result.model
				}
			},
			(error) => {
				console.log(error)
				alert(error)
			}
		)
	}
	async getData() {
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
		this._newsService.getListHomePage({}).subscribe((res) => {
			if (res.success != RESPONSE_STATUS.success) {
				return
			}
			if (res.result.length > 0) {
				this.firstNews = res.result[0]
				res.result.shift()
				this.news = res.result
			}
			return
		})
		// list thủ tục hành chính
		this._serviceAdministrative.getListHomePage({}).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				if (res.result.DAMAdministrationGetListTop) {
					this.Administrations = res.result.DAMAdministrationGetListTop
				}
			}
			return
		})
	}

	ngAfterViewInit() {
		setTimeout(function () {
			$('#owl-news').owlCarousel({
				loop: true,
				margin: 30,
				nav: false,
				autoplay: true,
				autoplayTimeout: 10000,
				autoplayHoverPause: true,
				responsive: {
					0: {
						items: 1,
					},
					600: {
						items: 2,
					},
					1000: {
						items: 2,
					},
				},
			})
		}, 200)
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

	redirectDetailNews(id: any) {
		this._router.navigate(['/cong-bo/tin-tuc-su-kien/' + id])
	}
	redirectNews() {
		this._router.navigate(['/cong-bo/tin-tuc-su-kien'])
	}

	redirectDetailAdministration(id: any) {
		this._router.navigate(['/cong-bo/thu-tuc-hanh-chinh/' + id])
	}

	redirectAdministration() {
		this._router.navigate(['/cong-bo/thu-tuc-hanh-chinh'])
	}
}
