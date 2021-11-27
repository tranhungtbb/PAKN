import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core'
import { Router } from '@angular/router'
import { ToastrService } from 'ngx-toastr'
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
import { SystemconfigService } from 'src/app/services/systemconfig.service'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'

declare var $: any

@Component({
	selector: 'app-index2',
	templateUrl: './index2.component.html',
	styleUrls: ['./index2.component.css'],
})
export class Index2Component implements OnInit, AfterViewInit {
	constructor(
		private _service: PuRecommendationService,
		private _router: Router,
		private _newsService: NewsService,
		private _serviceAdministrative: AdministrativeFormalitiesService,
		private indexSettingService: IndexSettingService,
		private _syService: SystemconfigService,
		private _toa: ToastrService,
		private storageService: UserInfoStorageService
	) {}
	@ViewChild(ViewRightComponent, { static: true }) viewRightComponent: ViewRightComponent
	isLogin: boolean = this.storageService.getIsHaveToken()
	ReflectionsRecommendations: Array<PuRecommendation>
	news: any[]
	firstNews: any
	Administrations: any[]
	isGrid: boolean = false
	isHomeMain: boolean = false
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
	
	indexSettingObj = new IndexSettingObjet()
	async ngOnInit() {
		await this.getData()
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
	getData() {
		this._service.getHomePage({}).subscribe(
			(res) => {
				if (res.success == RESPONSE_STATUS.success) {
					if (res.result) {
						this.isHomeMain = res.result.IsHomeDefault
						this.ReflectionsRecommendations = [...res.result.PURecommendation]
					}
				} else {
					this.ReflectionsRecommendations = []
					this._toa.error(res.message)
				}
			},
			(err) => {
				console.log(err)
			}
		)
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
		if (!string) {
			return ''
		}
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
