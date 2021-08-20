import { Component, OnInit, AfterViewInit } from '@angular/core'
import { DashBoardService } from '../../services/dashboard.service'
import { UserInfoStorageService } from '../../commons/user-info-storage.service'
import { DatepickerDateCustomClasses } from 'ngx-bootstrap/datepicker/models'
import { MESSAGE_COMMON, PROCESS_STATUS_RECOMMENDATION, RECOMMENDATION_STATUS, RESPONSE_STATUS, STEP_RECOMMENDATION } from 'src/app/constants/CONSTANTS'

import { RecommendationService } from 'src/app/services/recommendation.service'
import { DataService } from '../../services/sharedata.service'
import { Router } from '@angular/router'
import { ToastrService } from 'ngx-toastr'

declare var $: any

@Component({
	selector: 'app-dashboard',
	templateUrl: './dash-board.component.html',
	styleUrls: ['./dash-board.component.css'],
})
export class DashboardComponent implements OnInit, AfterViewInit {
	constructor(
		private _toast: ToastrService,
		private service: DashBoardService,
		private userStorage: UserInfoStorageService,
		private _shareData: DataService,
		private router: Router,
		private recommenService: RecommendationService
	) {}

	dataGraph: any = {
		stt_2: { count: 0 },
		stt_6: { count: 0 },
		stt_5: { count: 0 },
		stt_10: { count: 5 },
	}

	dataAll: any ={}

	totalCount = 0
	abc = 3

	ngOnInit() {
		this.getDataGraph()
		// $('.data-attr').peity('donut')
	}
	ngAfterViewInit(){
		// setTimeout(()=>{
		// 	$('.data-attr').peity('donut')
		// },200)
		
	}


	getDataGraph() {
		let req = {
			UnitProcessId: this.userStorage.getUnitId(),
			UserProcessId: this.userStorage.getUserId(),
		}
		this.recommenService.get7DayDataGraph(req).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				this.totalCount = res.result.data7day.reduce((acc, item, index) => {
					acc += item.total
					return acc
				}, 0)

				this.dataGraph = res.result.data7day.reduce((acc, item, index) => {
					item.per_10 = ((item.total / this.totalCount) * 10).toPrecision(2)
					item.per_100 = ((item.total / this.totalCount) * 100).toPrecision(2)
					acc['stt_' + item.status] = item;
					return acc
				}, {})

				console.log(res.result.data7day);


				let totalCountA = res.result.data.reduce((acc, item, index) => {
					acc += item.total
					return acc
				}, 0)

				this.dataAll = res.result.data.reduce((acc, item, index) => {
					item.per_10 = ((item.total / totalCountA) * 10).toPrecision(2)
					acc['stt_' + item.status] = item;
					return acc
				}, {})
				setTimeout(()=>{
					$('.data-attr').peity('donut')
				},1)
			}
		})

	}
}
