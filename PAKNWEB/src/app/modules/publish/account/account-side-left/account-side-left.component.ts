import { Component, OnInit } from '@angular/core'

import { Router } from '@angular/router'
import { ToastrService } from 'ngx-toastr'
import { AccountService } from 'src/app/services/account.service'
import { RECOMMENDATION_STATUS } from 'src/app/constants/CONSTANTS'

import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { AuthenticationService } from 'src/app/services/authentication.service'
import { DataService } from 'src/app/services/sharedata.service'
import { DiadanhService } from 'src/app/services/diadanh.service'
import { UserInfoObject } from 'src/app/models/UserObject'
import { RecommendationService } from 'src/app/services/recommendation.service'
import { PuRecommendationService } from 'src/app/services/pu-recommendation.service'

@Component({
	selector: 'app-account-side-left',
	templateUrl: './account-side-left.component.html',
	styleUrls: ['./account-side-left.component.css'],
})
export class AccountSideLeftComponent implements OnInit {
	constructor(
		private toast: ToastrService,
		private router: Router,
		private accountService: AccountService,
		private storageService: UserInfoStorageService,
		private authenService: AuthenticationService,
		private sharedataService: DataService,
		private diadanhService: DiadanhService,
		private recomnendationService: RecommendationService,
		private puRecommendationService: PuRecommendationService
	) {}
	model: any = { userName: '' }
	urlPath: string
	userId: number = 0
	graphData: any = {}
	totalPANK: number = 0
	totalRecommentdation: number = 0
	recommendationStatistics: any = new RecommendationStatistics()
	ngOnInit() {
		let urlPathArr = this.router.url.split('/')
		this.urlPath = urlPathArr[urlPathArr.length - 1]
		this.userId = this.storageService.getUserId()
		// this.getDataGraph()
		this.puRecommendationService.recommendationStatisticsGetByUserId({}).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success && res.result != null) {
				this.recommendationStatistics = res.result.PURecommendationStatisticsGetByUserId[0]
				for (const iterator in this.recommendationStatistics) {
					this.totalRecommentdation += this.recommendationStatistics[iterator]
				}
			}
			return
		})
	}

	mr_status: any = {
		'Chờ xử lý': [RECOMMENDATION_STATUS.RECEIVE_WAIT], //2
		'Đã tiếp nhận': [RECOMMENDATION_STATUS.RECEIVE_APPROVED, RECOMMENDATION_STATUS.PROCESS_WAIT, RECOMMENDATION_STATUS.PROCESSING, RECOMMENDATION_STATUS.APPROVE_WAIT], //4,5,7,8
		'Đã trả lời': [RECOMMENDATION_STATUS.FINISED], //10
		'Bị từ chối': [RECOMMENDATION_STATUS.PROCESS_DENY, RECOMMENDATION_STATUS.APPROVE_DENY, RECOMMENDATION_STATUS.RECEIVE_DENY], //3,6,9
	}
	Percent(value: any) {
		var result = Math.ceil((value / this.totalRecommentdation) * 100)
		return result
	}

	// private getDataGraph() {
	// 	this.recomnendationService
	// 		.getSendUserDataGraph({
	// 			sendId: this.userId,
	// 		})
	// 		.subscribe((res) => {
	// 			if (res.success == 'OK') {
	// 				this.totalPANK = res.result.MRRecommendationGetSendUserDataGraph.reduce((acc, e, i) => {
	// 					acc += e.count
	// 					return acc
	// 				}, 0)
	// 				this.graphData = res.result.MRRecommendationGetSendUserDataGraph.reduce((acc, e, i) => {
	// 					if (this.mr_status['Đã tiếp nhận'].includes(e.status)) {
	// 						if (acc['stt_4578']) {
	// 							acc['stt_4578'].count += e.count
	// 						} else {
	// 							acc['stt_4578'] = {
	// 								count: e.count,
	// 							}
	// 						}
	// 						acc['stt_4578'].per_100 = (acc['stt_4578'].count / this.totalPANK) * 100
	// 					}
	// 					if (this.mr_status['Bị từ chối'].includes(e.status)) {
	// 						if (acc['stt_369']) {
	// 							acc['stt_369'].count += e.count
	// 						} else {
	// 							acc['stt_369'] = {
	// 								count: e.count,
	// 							}
	// 						}
	// 						acc['stt_369'].per_100 = (acc['stt_369'].count / this.totalPANK) * 100
	// 					}
	// 					e['per_100'] = (e.count / this.totalPANK) * 100
	// 					acc['stt_' + e.status] = e
	// 					return acc
	// 				}, {})

	// 				console.log(this.graphData)
	// 			}
	// 		})
	// }

	signOut(): void {
		this.authenService.logOut({}).subscribe((success) => {
			if (success.success == RESPONSE_STATUS.success) {
				this.sharedataService.setIsLogin(false)
				this.storageService.setReturnUrl('')
				this.storageService.clearStoreage()
				this.router.navigate(['/dang-nhap'])
				//location.href = "/dang-nhap";
			}
		})
	}
	redirectMyRecommendaton(status: any) {
		this.router.navigate(['/cong-bo/phan-anh-kien-nghi-cua-toi/' + status])
	}
}

class RecommendationStatistics {
	approve: Number
	finised: Number
	receiveApproved: Number
	receiveWait: Number
}
