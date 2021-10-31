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
import { IndexBanner, IndexWebsite } from 'src/app/models/indexSettingObject'
import { Lightbox } from 'ngx-lightbox'

declare var $: any

@Component({
	selector: 'app-index2',
	templateUrl: './index2.component.html',
	styleUrls: ['./index2.component.css'],
})
export class Index2Component implements OnInit {
	constructor(
		private _service: PuRecommendationService,
		private _router: Router,
		private _newsService: NewsService,
		private _serviceAdministrative: AdministrativeFormalitiesService,
		private sanitizer: DomSanitizer,
		private indexSettingService: IndexSettingService,
		private _lightbox: Lightbox
	) {}
	_album: any[] = []

	RecommendationsOrderByCountClick: Array<PuRecommendation>
	ReflectionsRecommendations: Array<PuRecommendation>
	news: any[]
	firstNews: any
	Administrations: any[]
	//
	ltsIndexSettingWebsite: Array<IndexWebsite>

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

	activeUrl: string = ''
	routerHome = 'trang-chu2'
	indexSettingObj = new IndexSettingObjet()
	ngOnInit() {
		let splitRouter = this._router.url.split('/')
		if (splitRouter.length > 2) {
			this.activeUrl = splitRouter[2]
		}
		this.getData()
		this.indexSettingService.GetInfo({}).subscribe(
			(res) => {
				if (res.success == RESPONSE_STATUS.success) {
					this.indexSettingObj = res.result.model
					this.ltsIndexSettingWebsite = res.result.lstSYIndexWebsite == null ? [] : res.result.lstSYIndexWebsite
				}
			},
			(error) => {
				console.log(error)
				alert(error)
			}
		)

		for (let i = 1; i <= 4; i++) {
			const src = 'demo/img/image' + i + '.jpg'
			const caption = 'Image ' + i + ' caption here'
			const thumb = 'demo/img/image' + i + '-thumb.jpg'
			const album = {
				src: src,
				caption: caption,
				thumb: thumb,
			}

			this._album.push(album)
		}
	}
	routingMenu(pageRouting: string) {
		this.activeUrl = pageRouting
		this._router.navigate(['../cong-bo/' + pageRouting])
	}
	ngOnChanges() {
		let splitRouter = this._router.url.split('/')
		if (splitRouter.length > 2) {
			if (splitRouter[2] != this.routerHome) {
				this.activeUrl = splitRouter[2]
			} else {
				this.activeUrl = 'n'
			}
		} else {
			this.activeUrl = ''
		}
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

	ngAfterViewInit() {}

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

	//

	open(index: number): void {
		// open lightbox
		this._lightbox.open(this._album, index)
	}

	close(): void {
		// close lightbox programmatically
		this._lightbox.close()
	}
}
