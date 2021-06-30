import { Component, ElementRef, OnInit, ViewChild } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { COMMONS } from 'src/app/commons/commons'
import { CONSTANTS, FILETYPE, MESSAGE_COMMON, RECOMMENDATION_STATUS, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { RecommendationObject, RecommendationProcessObject } from 'src/app/models/recommendationObject'
import { UploadFileService } from 'src/app/services/uploadfiles.service'
import { RecommendationService } from 'src/app/services/recommendation.service'
import { ActivatedRoute, Router } from '@angular/router'
import { HashtagObject } from 'src/app/models/hashtagObject'
import { CatalogService } from 'src/app/services/catalog.service'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { CaptchaService } from 'src/app/services/captcha-service'
import { UserService } from 'src/app/services/user.service'
import { PuRecommendationService } from 'src/app/services/pu-recommendation.service'
import { AuthenticationService } from 'src/app/services/authentication.service'
import { DataService } from 'src/app/services/sharedata.service'
declare var $: any
@Component({
	selector: 'app-my-recommendation',
	templateUrl: './my-recommendation.component.html',
	styleUrls: ['./my-recommendation.component.css'],
})
export class MyRecommendationComponent implements OnInit {
	userName: string = ''
	phone: string = ''
	title: string = ''
	isFilter: boolean = false
	recommandationId: number = 0
	pageIndex: number = 1
	pageSize: number = 20
	pagination = []
	status: any
	@ViewChild('table', { static: false }) table: any
	totalRecords: number = 0
	listData = new Array<RecommendationObject>()

	recommendationStatistics: any = new RecommendationStatistics()
	totalRecommentdation: number = 0

	constructor(
		private toastr: ToastrService,
		private fileService: UploadFileService,
		private recommendationService: RecommendationService,
		private storageService: UserInfoStorageService,
		private router: Router,
		private captchaService: CaptchaService,
		private userService: UserService,
		private puRecommendationService: PuRecommendationService,
		private activatedRoute: ActivatedRoute,
		private authenService: AuthenticationService,
		private sharedataService: DataService,
	) {}

	ngOnInit() {
		this.userName = this.storageService.getFullName()
		this.getSoDienThoai()

		this.puRecommendationService.recommendationStatisticsGetByUserId({}).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success && res.result != null) {
				this.recommendationStatistics = res.result.PURecommendationStatisticsGetByUserId[0]
				for (const iterator in this.recommendationStatistics) {
					this.totalRecommentdation += this.recommendationStatistics[iterator]
				}
			}
			return
		})

		this.activatedRoute.params.subscribe((params) => {
			let id = +params['id']
			this.status = Number.isNaN(id) == true ? 0 : id
		})
		this.filterMyRecommendation(this.status)
	}
	Percent(value: any) {
		var result = Math.ceil((value / this.totalRecommentdation) * 100)
		return result
	}
	getSoDienThoai() {
		this.userService.getById({ id: this.storageService.getUserId() }).subscribe((res) => {
			if (res.success != 'OK') return
			var modelUser = res.result.SYUserGetByID[0]
			if (modelUser && modelUser.phone) {
				this.phone = modelUser.phone
			}
		})
	}

	mySignOut(): void {
		this.authenService.logOut({}).subscribe((success) => {
			if (success.success == RESPONSE_STATUS.success) {
				this.sharedataService.setIsLogin(false)
				this.storageService.setReturnUrl('')
				this.storageService.clear()
				this.router.navigate(['/dang-nhap'])
			}
		})
	}

	LtsStatus = ''
	getList(Status: any = null) {
		this.title = this.title.trim()
		if (Status) this.LtsStatus = Status
		let request = {
			userId: this.storageService.getUserId(),
			LtsStatus: this.LtsStatus == null ? '' : this.LtsStatus,
			Title: this.title == null ? '' : this.title,
			IsFilter: this.isFilter,
			pageIndex: this.pageIndex,
			PageSize: this.pageSize,
		}
		this.puRecommendationService.getMyRecommentdation(request).subscribe((res) => {
			if (res != 'undefined' && res.success == RESPONSE_STATUS.success) {
				if (res.result.MyRecommendation.length > 0) {
					this.listData = res.result.MyRecommendation
					this.totalRecords = res.result.TotalCount
					this.padi()
				} else {
					this.listData = this.pagination = []
					this.pageIndex = 1
					this.totalRecords = 0
				}
			} else {
				this.listData = this.pagination = []
				this.pageIndex = 1
				this.totalRecords = 0
			}
		}),
			(error) => {
				console.log(error)
				alert(error)
			}
	}

	filterMyRecommendation(status: any) {
		this.pageIndex = 1
		this.pageSize = 20
		switch (status) {
			case 1:
				// chờ xl
				this.getList(',2,5')
				this.LtsStatus = ',2,5'
				break
			case 2:
				// đã tiếp nhận
				this.getList(',4,5,7,8')
				this.LtsStatus = ',4,5,7,8'
				break
			case 3:
				// đã trả lời
				this.getList(',10')
				this.LtsStatus = ',10'
				break
			case 4:
				// bị từ chối
				this.getList(',3,6,9')
				this.LtsStatus = ',3,6,9'
				break
			default:
				this.getList()
				break
		}
	}

	changeKeySearch(event) {
		this.title = event.target.value
	}

	sendRecommandation() {
		this.router.navigateByUrl('/cong-bo/them-moi-kien-nghi')
	}

	//delete recommandateion
	preDelete(id: number) {
		this.recommandationId = id
		$('#modalConfirmDelete').modal('show')
	}

	onDelete(id: number) {
		let request = {
			Id: id,
		}
		this.recommendationService.recommendationDelete(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				this.toastr.success(MESSAGE_COMMON.DELETE_SUCCESS)
				$('#modalConfirmDelete').modal('hide')
				this.getList()
			} else {
				this.toastr.error(response.message)
			}
		}),
			(error) => {
				console.error(error)
			}
	}

	preSend(id: any) {
		this.recommandationId = id
		$('#modalConfirmSend').modal('show')
	}
	onSend(status) {
		var request = {
			id: this.recommandationId,
			status: status,
		}
		this.recommendationService.recommendationUpdateStatus(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				$('#modalConfirmSend').modal('hide')
				this.toastr.success(COMMONS.SEND_SUCCESS)
				this.getList()
				let item = this.listData.find((c) => c.id == this.recommandationId)
				if (item) item.status = status
			} else {
				this.toastr.error(response.message)
			}
		}),
			(err) => {
				console.error(err)
			}
	}

	padi() {
		this.pagination = []
		for (let i = 0; i < Math.ceil(this.totalRecords / this.pageSize); i++) {
			this.pagination.push({ index: i + 1 })
		}
	}

	changePagination(index: any) {
		if (this.pageIndex > index) {
			if (index > 0) {
				this.pageIndex = index
				this.getList()
			}
			return
		} else if (this.pageIndex < index) {
			if (this.pagination.length >= index) {
				this.pageIndex = index
				this.getList()
			}
			return
		}
		return
	}
}

class RecommendationStatistics {
	approve: Number
	finised: Number
	receiveApproved: Number
	receiveWait: Number
}
