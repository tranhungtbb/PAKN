import { Component, OnInit, AfterViewInit } from '@angular/core'
import { DashBoardService } from '../../services/dashboard.service'
import { UserInfoStorageService } from '../../commons/user-info-storage.service'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'

import { RecommendationService } from 'src/app/services/recommendation.service'
import { DataService } from '../../services/sharedata.service'
import { Router } from '@angular/router'
import { ToastrService } from 'ngx-toastr'
import { RemindService } from 'src/app/services/remind.service'
import { STATUS_CODES } from 'http'
import { type } from 'os'

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
		private remindService : RemindService,
		private recommenService: RecommendationService
	) {}

	dataGraph: any = {
		stt_2: { count: 0 },
		stt_3: { count: 0 },
		stt_5: { count: 0 },
		stt_10: { count: 0 },
	}

	dataGraphTTHC : any = {
		stt_2: { count: 0 },
		stt_3: { count: 0 }
	}


	dataAll: any ={}

	totalCount = 0
	listRemind : any = []

	ngOnInit() {
		this.getDataGraph()
		this.remindService.remindGetList({}).subscribe(res=>{
			if(res.success === RESPONSE_STATUS.success){
				this.listRemind = res.result
			}
			else{
				this.listRemind = []
			}
		})
		
	}
	ngAfterViewInit(){
	}

	totalCountTTHC : any = 0

	getDataGraph() {
		let req = {
			UnitProcessId: this.userStorage.getUnitId(),
			UserProcessId: this.userStorage.getUserId(),
		}
		this.recommenService.get7DayDataGraph(req).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				for (const item of res.result.data7day) {
					if(item.type ==1){
						this.totalCount += item.total
					}
					if(item.type == 2){
						this.totalCountTTHC += item.total
					}
				}

				for (const item of res.result.data7day) {
					if(item.type == 1){
						item.per_100 = ((item.total / this.totalCount) * 100)
						this.dataGraph['stt_' + item.status] = item;
					}
					if(item.type == 2){
						item.per_100_TTHC = ((item.total / this.totalCountTTHC) * 100)
						this.dataGraphTTHC['stt_' + item.status] = item;
					}	
				}

				let totalCountA = res.result.data.reduce((acc, item, index) => {
					acc += item.total
					return acc
				}, 0)

				this.dataAll = res.result.data.reduce((acc, item, index) => {
					item.per_10 = ((item.total / totalCountA) * 10)
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
