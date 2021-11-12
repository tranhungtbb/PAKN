import { Component, OnInit, ViewChild } from '@angular/core'
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js'
import { Label } from 'ng2-charts'
import { MESSAGE_COMMON, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'

@Component({
	selector: 'app-statistics',
	templateUrl: './statistics.component.html',
	styleUrls: ['./statistics.component.css'],
})
export class StatisticsComponent implements OnInit {
	// property
	statistics: any = [
		{
			unitName: 'Sở y tế',
			totalResult: 8,
			receive: 6,
			approved: 1,
			process: 1,
			expired: 0,
			satisfaction: 120,
			common: 50,
			unsatisfaction: 9,
		},
		{
			unitName: 'Sở Giao thông - Vận tải',
			totalResult: 8,
			receive: 6,
			approved: 1,
			process: 1,
			expired: 0,
			satisfaction: 120,
			common: 50,
			unsatisfaction: 9,
		},
		{
			unitName: 'Sở Tài nguyên - Môi trường',
			totalResult: 8,
			receive: 6,
			approved: 1,
			process: 1,
			expired: 0,
			satisfaction: 120,
			common: 50,
			unsatisfaction: 9,
		},
		{
			unitName: 'Sở công thương',
			totalResult: 8,
			receive: 6,
			approved: 1,
			process: 1,
			expired: 0,
			satisfaction: 120,
			common: 50,
			unsatisfaction: 9,
		},
		{
			unitName: 'Sở Giáo dục - Đào tạo',
			totalResult: 8,
			receive: 6,
			approved: 1,
			process: 1,
			expired: 0,
			satisfaction: 120,
			common: 50,
			unsatisfaction: 9,
		},
		{
			unitName: 'Sở Kế hoạch và Đầu tư',
			totalResult: 8,
			receive: 6,
			approved: 1,
			process: 1,
			expired: 0,
			satisfaction: 120,
			common: 50,
			unsatisfaction: 9,
		},
		{
			unitName: 'Sở Khoa học và Công nghệ',
			totalResult: 8,
			receive: 6,
			approved: 1,
			process: 1,
			expired: 0,
			satisfaction: 120,
			common: 50,
			unsatisfaction: 9,
		},
		{
			unitName: 'Sở Lao động - Thương binh và Xã hội',
			totalResult: 8,
			receive: 6,
			approved: 1,
			process: 1,
			expired: 0,
			satisfaction: 120,
			common: 50,
			unsatisfaction: 9,
		},
		{
			unitName: 'Sở Ngoại vụ',
			totalResult: 8,
			receive: 6,
			approved: 1,
			process: 1,
			expired: 0,
			satisfaction: 120,
			common: 50,
			unsatisfaction: 9,
		},
		{
			unitName: 'Sở Nội vụ',
			totalResult: 8,
			receive: 6,
			approved: 1,
			process: 1,
			expired: 0,
			satisfaction: 120,
			common: 50,
			unsatisfaction: 9,
		},
		{
			unitName: 'Sở Tài chính',
			totalResult: 8,
			receive: 6,
			approved: 1,
			process: 1,
			expired: 0,
			satisfaction: 120,
			common: 50,
			unsatisfaction: 9,
		},
		{
			unitName: 'Sở Thông tin - Truyền thông',
			totalResult: 8,
			receive: 6,
			approved: 1,
			process: 1,
			expired: 0,
			satisfaction: 120,
			common: 50,
			unsatisfaction: 9,
		},
		{
			unitName: 'Sở Văn hóa và Thể thao',
			totalResult: 8,
			receive: 6,
			approved: 1,
			process: 1,
			expired: 0,
			satisfaction: 120,
			common: 50,
			unsatisfaction: 9,
		},
	]
	constructor() {}

	// chart

	barChartOptions: ChartOptions = {
		responsive: true,
		title: {
			display: true,
			text: 'Biểu đồ tổng hợp số liệu',
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

	ngOnInit() {}
}
