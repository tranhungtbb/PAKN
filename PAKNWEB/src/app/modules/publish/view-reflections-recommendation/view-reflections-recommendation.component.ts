import { Component, OnInit } from '@angular/core'
import { ActivatedRoute, Router } from '@angular/router'
import { ToastrService } from 'ngx-toastr'
import { PuRecommendationService } from 'src/app/services/pu-recommendation.service'
import { RESPONSE_STATUS, RECOMMENDATION_STATUS } from 'src/app/constants/CONSTANTS'

declare var require: any
const FileSaver = require('file-saver')

@Component({
	selector: 'app-view-reflections-recommendations',
	templateUrl: './view-reflections-recommendation.component.html',
	styleUrls: ['./view-reflections-recommendation.component.css'],
})
export class ViewReflectionsRecommendationComponent implements OnInit {
	public id
	public model: any
	public lstFiles: any
	public lstConclusion: any
	public lstConclusionFiles: any
	public satisfactions: Array<satisfaction>
	satisfactionCurrent: boolean
	checkSatisfaction: boolean
	constructor(private service: PuRecommendationService, private activatedRoute: ActivatedRoute, public router: Router, private _toastr: ToastrService) {
		this.checkSatisfaction = false
	}
	ngOnInit() {
		this.getRecommendationById()
		this.setSatisfaction()
	}

	getRecommendationById() {
		this.activatedRoute.params.subscribe((params) => {
			this.id = +params['id']
			if (this.id != 0) {
				//update count click
				this.service.countClick({ RecommendationId: this.id }).subscribe()

				// call api getRecommendation by id
				// tạm thời fix status = 3, nhưng thực tế status success = 10
				this.service.getById({ id: this.id, status: RECOMMENDATION_STATUS.FINISED }).subscribe((res) => {
					if (res.success == RESPONSE_STATUS.success) {
						if (res.result.model != null) {
							this.model = { ...res.result.model, shortName: this.getShortName(res.result.model.name) }
							this.lstFiles = res.result.lstFiles
							this.lstConclusion = res.result.lstConclusion
							this.lstConclusionFiles = res.result.lstConclusionFiles
						}
					}
				})
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
	setSatisfaction() {
		var data = localStorage.getItem('satisfaction')
		if (data == null || data == undefined) {
			this.satisfactions = []
			localStorage.setItem('satisfaction', JSON.stringify(this.satisfactions))
			return
		} else {
			this.satisfactions = JSON.parse(data)
			this.satisfactions.forEach((item) => {
				if (item.recommendationID == this.id) {
					this.satisfactionCurrent = item.satisfaction
					this.checkSatisfaction = true
				}
			})
		}
	}

	changeSatisfaction(status: any) {
		if (this.satisfactionCurrent == null || this.satisfactionCurrent == undefined) {
			// chưa like hoặc dislike lần nào
			if (this.checkSatisfaction == false) {
				this.satisfactionCurrent = status
				// call api
				this.service.changeSatisfaction({ RecommendationId: this.id, Satisfaction: status }).subscribe((res) => {
					if (res.success == RESPONSE_STATUS.success) {
						this._toastr.success('Đánh giá thành công!')
						let check = false
						this.satisfactions.forEach((item) => {
							if (item.recommendationID == this.id) {
								check = true
							}
						})
						if (check == false) {
							let obj = {
								recommendationID: this.id,
								satisfaction: status,
							}
							this.satisfactions.push(obj)
							localStorage.setItem('satisfaction', JSON.stringify(this.satisfactions))
						}
						if (status) {
							this.model.quantityLike = this.model.quantityLike + 1
						} else {
							this.model.quantityDislike = this.model.quantityDislike + 1
						}
					} else {
						this._toastr.error('Đánh giá thất bại!')
					}
				})
			} else {
				this.checkSatisfaction = true
				this._toastr.success('Bạn đã đánh giá PAKN này!')
				return
			}
		} else {
			this._toastr.success('Bạn đã đánh giá PAKN này!')
			return
		}
	}
}

interface satisfaction {
	recommendationID: number
	satisfaction: boolean
}
