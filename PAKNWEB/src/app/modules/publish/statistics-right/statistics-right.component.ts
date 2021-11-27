import { Component, Input, OnInit } from '@angular/core'
import { Router } from '@angular/router'
import { DomSanitizer } from '@angular/platform-browser'
import { ChartType, ChartOptions } from 'chart.js'
import { ToastrService } from 'ngx-toastr'
import { Color, MultiDataSet, Label, SingleDataSet } from 'ng2-charts'
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

	@Input() isShowSatisfationChart : boolean
	isLogin: boolean = this.storageService.getIsHaveToken()
	typeObject: number = this.storageService.getTypeObject()
	recommendationStatistics: any
	totalRecommentdation: number = 0
	ltsIndexSettingWebsite: any = []
	// chart

	chartOptions: ChartOptions = {
    responsive: true,
		legend: {
			position: 'bottom',
		},
  };
	chartType: ChartType = 'pie'

	// tk toàn tỉnh
	pieChartLabels: Label[] = ['Hồ sơ đã giải quyết', 'Hồ sơ đã tiếp nhận']
	pieChartData: MultiDataSet = [[950, 350]]
	pieChartColors: Color[] = [
		{
			backgroundColor: ['#58A55C', '#73BCFF'],
		},
	]

	// tk satisfaction

	
  satisfactionChartLabels: Label[] = ['Hài lòng','Không hài lòng', 'Chấp nhận'];
  satisfactionChartData: SingleDataSet = [500, 150, 250];
  satisfactionChartPlugins = [];
	satisfactionChartColors: Color[] = [
		{
			backgroundColor: ['#2E73D5', '#DA2222', '#FFB200'],
		},
	]

	ngOnInit() {
		this._service.recommendationStatisticsProvince({}).subscribe(
			(res) => {
				if (res.success == RESPONSE_STATUS.success) {
					this.pieChartLabels = res.result.Titles
					this.pieChartData = res.result.Values
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

	redirectMyRecommendaton(status: any) {
		this._router.navigate(['/cong-bo/phan-anh-kien-nghi-cua-toi/' + status])
	}

	ngAfterViewInit() {}
}
