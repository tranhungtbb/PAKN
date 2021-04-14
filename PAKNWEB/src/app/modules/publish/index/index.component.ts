import { Component, OnInit } from '@angular/core'
import { Router } from '@angular/router'
import { DomSanitizer } from '@angular/platform-browser'

import { PuRecommendation } from 'src/app/models/recommendationObject'
import { PuRecommendationService } from 'src/app/services/pu-recommendation.service'
import { NewsService } from 'src/app/services/news.service'
import { RECOMMENDATION_STATUS, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'

declare var $: any

@Component({
	selector: 'app-index',
	templateUrl: './index.component.html',
	styleUrls: ['./index.component.css'],
})
export class IndexComponent implements OnInit {
	constructor(private _service: PuRecommendationService, private _router: Router, private _newsService: NewsService, private sanitizer: DomSanitizer) {}

	RecommendationsOrderByCountClick: Array<PuRecommendation>
	ReflectionsRecommendations: Array<PuRecommendation>
	news: any[]
	firstNews: any
	ngOnInit() {
		this.getData()
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
			if (res.result) {
				this.news = res.result
				this.getNewsAvatars()
				this.firstNews = this.news[0]
			}
			return
		})
		// list thủ tục hành chính
	}

	ngAfterViewInit() {
		setTimeout(function () {
			$('.owl-carousel').owlCarousel({
				loop: false,
				margin: 30,
				nav: false,
				autoplay: true,
				autoplayTimeout: 5000,
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
	getNewsAvatars() {
		let ids = this.news.map((c) => c.id)
		this._newsService.getAvatars(ids).subscribe((res) => {
			if (res) {
				res.forEach((e) => {
					let item = this.news.find((c) => c.id == e.id)
					let objectURL = 'data:image/jpeg;base64,' + e.byteImage
					item.imageBin = this.sanitizer.bypassSecurityTrustUrl(objectURL)
				})
			}
		})
	}

	redirectDetailNews(id: any) {
		this._router.navigate(['/cong-bo/tin-tuc-su-kien/' + id])
	}
	redirectNews() {
		this._router.navigate(['/cong-bo/tin-tuc-su-kien'])
	}
}
