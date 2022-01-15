import { Component, Input, OnInit } from '@angular/core'
import { Router } from '@angular/router'
import { DomSanitizer } from '@angular/platform-browser'
import { ChartType, ChartOptions } from 'chart.js'
import { ToastrService } from 'ngx-toastr'
import { Color, MultiDataSet, Label, SingleDataSet } from 'ng2-charts'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { PuRecommendationService } from 'src/app/services/pu-recommendation.service'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { NewsService } from 'src/app/services/news.service'

declare var $: any

@Component({
	selector: 'app-statistics-right',
	templateUrl: './statistics-right.component.html',
	styleUrls: ['./statistics-right.component.css'],
})
export class StatisticsRightComponent implements OnInit {
	constructor(private _router: Router, private _newsService: NewsService, private _toastr: ToastrService, private storageService: UserInfoStorageService, private _service: PuRecommendationService) { }

	@Input() isShowSatisfationChart: boolean
	@Input() isShowNotification: boolean
	isLogin: boolean = this.storageService.getIsHaveToken()
	typeObject: number = this.storageService.getTypeObject()
	recommendationStatistics: any
	totalRecommentdation: number = 0
	ltsIndexSettingWebsite: any = []
	// chart

	chartOptions: ChartOptions = {
		responsive: true,
		legend: {
			display: false,
			position: 'bottom',
		},
		tooltips: {
			enabled: true,
			callbacks: {
				label: function (tooltipItem, data) {
					let sum = 0;
					let dataArr: any[] = data.datasets[0].data;
					dataArr.map((data: number) => {
						sum += data;
					});
					let percentage = (dataArr[tooltipItem.index] * 100 / sum).toFixed(2) + "%: " + dataArr[tooltipItem.index];
					return percentage;
				},
			},
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


	satisfactionChartLabels: Label[] = ['Hài lòng', 'Không hài lòng', 'Chấp nhận'];
	satisfactionChartData: SingleDataSet = [500, 150, 250];
	satisfactionChartPlugins = [];
	satisfactionChartColors: Color[] = [
		{
			backgroundColor: ['#2E73D5', '#DA2222', '#FFB200'],
		},
	]
	onTime: number = 0
	listNotification: any = []

	ngOnInit() {

		this._newsService.getListHomePage({}).subscribe(
			(res) => {
				if (res.success == RESPONSE_STATUS.success) {
					this.listNotification = res.result
				}
			},
			(error) => {
				console.log(error)
				alert(error)
			}
		)

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

		this._service.recommendationStatisticsSatisfaction().subscribe(
			(res) => {
				if (res.success == RESPONSE_STATUS.success) {
					this.satisfactionChartData = res.result.Values
					this.onTime = Math.floor(res.result.Expire.onTime / res.result.Expire.total * 100)
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

	ngAfterViewInit() { }
	shortTitle(title: string) {
		const arr = title.split(' ')
		if (arr.length > 20) {
			return arr.slice(0, 19).join(' ') + '...'
		} else {
			return title
		}
	}
}
