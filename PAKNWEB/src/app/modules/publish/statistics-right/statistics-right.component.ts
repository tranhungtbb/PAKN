import { Component, OnInit } from '@angular/core'
import { Router } from '@angular/router'
import { DomSanitizer } from '@angular/platform-browser'
import { ChartType, ChartOptions } from 'chart.js'
import { ToastrService } from 'ngx-toastr'
import { Color, MultiDataSet, Label } from 'ng2-charts'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { PuRecommendationService } from 'src/app/services/pu-recommendation.service'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'

declare var $: any

@Component({
	selector: 'app-statistics-right',
	templateUrl: './statistics-right.component.html',
	styleUrls: ['./statistics-right.component.css'],
})
export class StatisticsRightComponent implements OnInit {
	constructor(private _router: Router, private _toastr: ToastrService, private storageService: UserInfoStorageService, private _service: PuRecommendationService) {}

	isLogin: boolean = this.storageService.getIsHaveToken()
	typeObject: number = this.storageService.getTypeObject()
	recommendationStatistics: any
	totalRecommentdation: number = 0
	ltsIndexSettingWebsite: any = []
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
	ngOnInit() {
		this._service.recommendationStatisticsProvince({}).subscribe(
			(res) => {
				if (res.success == RESPONSE_STATUS.success) {
					this.doughnutChartLabels = res.result.Titles
					this.doughnutChartData = res.result.Values
				} else {
					this._toastr.error(res.message)
				}
			},
			(err) => {
				console.log(err)
			}
		)

		if (this.isLogin) {
			this._service.recommendationStatisticsGetByUserId({}).subscribe((res) => {
				if (res.success == RESPONSE_STATUS.success && res.result != null) {
					this.recommendationStatistics = res.result.PURecommendationStatisticsGetByUserId[0]
					for (const iterator in this.recommendationStatistics) {
						this.totalRecommentdation += this.recommendationStatistics[iterator]
					}
				}
				return
			})
		}
	}

	ngAfterViewInit() {}
}
