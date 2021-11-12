import { Component, OnInit } from '@angular/core'
import { Router } from '@angular/router'
import { DomSanitizer } from '@angular/platform-browser'
import { ChartType, ChartOptions } from 'chart.js'
import { Color, MultiDataSet, Label } from 'ng2-charts'

declare var $: any

@Component({
	selector: 'app-statistics-right',
	templateUrl: './statistics-right.component.html',
	styleUrls: ['./statistics-right.component.css'],
})
export class StatisticsRightComponent implements OnInit {
	constructor(private _router: Router, private sanitizer: DomSanitizer) {}
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
	ngOnInit() {}

	ngAfterViewInit() {}
}
