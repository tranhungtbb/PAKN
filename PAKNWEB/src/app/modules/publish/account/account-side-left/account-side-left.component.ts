import { Component, OnInit } from '@angular/core'

import { Router } from '@angular/router'
import { RECOMMENDATION_STATUS } from 'src/app/constants/CONSTANTS'

import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { AuthenticationService } from 'src/app/services/authentication.service'
import { DataService } from 'src/app/services/sharedata.service'
import { PuRecommendationService } from 'src/app/services/pu-recommendation.service'
import { UserService } from 'src/app/services/user.service'


declare var $: any
@Component({
	selector: 'app-account-side-left',
	templateUrl: './account-side-left.component.html',
	styleUrls: ['./account-side-left.component.css'],
})
export class AccountSideLeftComponent implements OnInit {
	constructor(
		private router: Router,
		private storageService: UserInfoStorageService,
		private authenService: AuthenticationService,
		private sharedataService: DataService,
		private puRecommendationService: PuRecommendationService,
		private userService: UserService
	) { }
	modelInfo: any = {}
	urlPath: string
	userId: number = 0
	graphData: any = {}
	totalPANK: number = 0
	totalRecommentdation: number = 0
	recommendationStatistics: any = new RecommendationStatistics()
	typeUserLoginPublish: number = this.storageService.getTypeObject()
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
		this.userService.getById({ Id: this.userId }).subscribe(res => {
			if (res.success == RESPONSE_STATUS.success) {
				if (res.result.SYUserGetByID.length > 0) {
					this.modelInfo = res.result.SYUserGetByID[0]
					// console.log(this.model)
				}
			}
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

	preSignOut() {
		$('#acceptSignOut').modal('show')
	}
	closeModal() {
		$('#acceptSignOut').modal('hide')
	}

	signOut(): void {
		this.authenService.logOut({}).subscribe((success) => {
			if (success.success == RESPONSE_STATUS.success) {
				this.sharedataService.setIsLogin(false)
				this.storageService.setReturnUrl('')
				this.storageService.clear()
				$('#acceptSignOut').modal('hide')
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
