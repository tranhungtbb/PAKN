import { Component, OnInit, ViewChild } from '@angular/core'
import { Router } from '@angular/router'
import { DomSanitizer } from '@angular/platform-browser'
import { OwlOptions } from 'ngx-owl-carousel-o'
import { ChartType, ChartOptions } from 'chart.js'
import { Color, MultiDataSet, Label } from 'ng2-charts'

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

	ReflectionsRecommendations: Array<PuRecommendation>
	news: any[]
	firstNews: any
	Administrations: any[]
	isGrid: boolean = true
	// owl carocel
	customOptions: OwlOptions = {
		loop: true,
		dots: false,
		margin: 30,
		lazyLoad: true,
		autoplay: true,
		autoplayTimeout: 5000,
		autoplaySpeed: 1000,
		dotsSpeed: 700,
		dotsEach: false,
		navText: ['<i class="bi bi-arrow-left"></i>', '<i class="bi bi-arrow-right"></i>'],
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
		nav: true,
	}

	// chart
	doughnutChartLabels: Label[] = ['Hồ sơ đã giải quyết', 'Hồ sơ đã tiếp nhận']
	doughnutChartData: MultiDataSet = [[950, 350]]
	doughnutChartType: ChartType = 'doughnut'
	doughnutChartColors: Color[] = [
		{
			backgroundColor: ['#58A55C', '#73BCFF'],
		},
	]
	doughnutChartOptions: ChartOptions = {
		responsive: true,
		legend: {
			position: 'bottom',
		},
		cutoutPercentage: 75,
	}

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

	ngAfterViewInit() {}

	getShortName(string) {
		var names = string.split(' '),
			initials = names[0].substring(0, 1).toUpperCase()
		if (names.length > 1) {
			initials += names[names.length - 1].substring(0, 1).toUpperCase()
		}
		return initials
	}

	redirectDetailAdministration(id: any) {
		this._router.navigate(['/cong-bo/thu-tuc-hanh-chinh/' + id])
	}

	redirectDetailNews(id: any) {
		this._router.navigate(['/cong-bo/tin-tuc-su-kien/' + id])
	}
}
