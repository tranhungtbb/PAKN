import { Component, OnInit } from '@angular/core'
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
export class DashboardComponent implements OnInit {
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
		stt_10: { count: 0 },
	}

	totalCount = 0

	ngOnInit() {
		this.getDataGraph()
	}

	getDataGraph() {
		let req = {
			UnitProcessId: this.userStorage.getUnitId(),
			UserProcessId: this.userStorage.getUserId(),
		}
		this.recommenService.getDataGraph(req).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				this.totalCount = res.result.MRRecommendationGetDataGraph.reduce((acc, item, index) => {
					acc += item.count
					return acc
				}, 0)
				let data = res.result.MRRecommendationGetDataGraph.reduce((acc, item, index) => {
					item.percent = ((item.count / this.totalCount) * 10).toPrecision(2)
					acc['stt_' + item.status] = item
					return acc
				}, {})
				this.dataGraph = data
			}
		})
	}
}
