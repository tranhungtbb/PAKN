import { Component, OnInit, ViewChild } from '@angular/core'
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js'
import { Label } from 'ng2-charts'
import { MESSAGE_COMMON, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { PuRecommendationService } from 'src/app/services/pu-recommendation.service'
import { ToastrService } from 'ngx-toastr'
import { TreeNode } from 'primeng/api'
import { TreeTableModule } from 'primeng/treetable'

@Component({
	selector: 'app-statistics',
	templateUrl: './statistics.component.html',
	styleUrls: ['./statistics.component.css'],
})
export class StatisticsComponent implements OnInit {
	// property
	statistics: TreeNode[]
	constructor(private _service: PuRecommendationService, private _toastr: ToastrService) { }

	// chart

	barChartOptions: ChartOptions = {
		responsive: true,
		title: {
			display: true,
			text: 'Biểu đồ tổng hợp số liệu',
			fontSize: 16,
			position: 'bottom',
		},
		legend: {
			position: 'bottom',
		},
		scales: {
			yAxes: [
				{
					ticks: {
						beginAtZero: true,
						min: 0,
					},
				},
			],
		},
	}
	barChartLabels: Label[] = [
		'Sở y tế',
		'Sở Giao thông - Vận tải',
		'Sở Tài nguyên - Môi trường',
		'Sở công thương',
		'Sở Giáo dục - Đào tạo',
		'Sở Kế hoạch và Đầu tư',
		'Sở Khoa học và Công nghệ',
		'Sở Lao động - Thương binh và Xã hội',
		'Sở Ngoại vụ',
		'Sở Nội vụ',
		'Sở Tài chính',
		'Sở Thông tin - Truyền thông',
		'Sở Văn hóa và Thể thao',
	]
	barChartType: ChartType = 'bar'
	barChartLegend = true
	barChartPlugins = []
	barChartColors: any[] = [{ backgroundColor: '#222222' }, { backgroundColor: '#58A55C' }, { backgroundColor: '#2E73D5' }, { backgroundColor: '#DA2222' }]

	barChartData: ChartDataSets[] = [
		{ data: [140, 2, 3, 81, 94, 55, 43, 65, 12, 12, 81, 21, 55, 90], label: 'Tổng' },
		{ data: [90, 23, 2, 19, 9, 27, 23, 28, 48, 40, 19, 86, 12, 1], label: 'Đã xử lý' },
		{ data: [27, 5, 23, 21, 70, 55, 21, 21, 3, 2, 30, 4, 3, 33], label: 'Đang xử lý' },
		{ data: [64, 10, 27, 5, 37, 27, 1, 20, 10, 27, 19, 21, 45, 23], label: 'Quá hạn' },
	]

	ngOnInit() {
		this._service.recommendationStatisticsByUnitParentId({}).subscribe(
			(res) => {
				if (res.success == RESPONSE_STATUS.success) {
					this.statistics = []
					for (var item of res.result) {
						let itemDefault = item
						item.children = []
						item.label = item.unitName
						item.leaf = false
						item.data = { ...itemDefault, expanded: false, leaf: true, children: [] }
					}
					this.statistics = res.result
				} else {
					this._toastr.error(res.message)
				}
			},
			(err) => {
				console.log(err)
			}
		)

		/// data for chart
		this._service.recommendationStatisticsForChart({}).subscribe(
			(res) => {
				if (res.success == RESPONSE_STATUS.success) {
					this.barChartLabels = res.result.Titles
					this.barChartData = res.result.Values
				} else {
					this._toastr.error(res.message)
				}
			},
			(err) => {
				console.log(err)
			}
		)
	}

	onNodeExpand(event) {
		this._service.recommendationStatisticsByUnitParentId({ ParentId: event.node.id }).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				for (var item of res.result) {
					let itemDefault = item
					item.children = []
					item.label = item.unitName
					item.leaf = false
					item.data = { ...itemDefault, index: event.node.data.index + '.' + item.index, expanded: false, leaf: true, children: [] }
				}
				event.node.children = res.result
				this.statistics = [...this.statistics]
			}
		})
	}
}
