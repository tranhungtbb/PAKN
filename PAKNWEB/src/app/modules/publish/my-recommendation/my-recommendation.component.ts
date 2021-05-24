import { Component, ElementRef, OnInit, ViewChild } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { COMMONS } from 'src/app/commons/commons'
import { CONSTANTS, FILETYPE, MESSAGE_COMMON, RECOMMENDATION_STATUS, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { RecommendationObject, RecommendationProcessObject } from 'src/app/models/recommendationObject'
import { UploadFileService } from 'src/app/services/uploadfiles.service'
import { RecommendationService } from 'src/app/services/recommendation.service'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { ActivatedRoute, Router } from '@angular/router'
import { HashtagObject } from 'src/app/models/hashtagObject'
import { CatalogService } from 'src/app/services/catalog.service'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { AppSettings } from 'src/app/constants/app-setting'
import { Api } from 'src/app/constants/api'
import { CaptchaService } from 'src/app/services/captcha-service'
import { UserService } from 'src/app/services/user.service'
import { PuRecommendationService } from 'src/app/services/pu-recommendation.service'

declare var $: any
@Component({
	selector: 'app-my-recommendation',
	templateUrl: './my-recommendation.component.html',
	styleUrls: ['./my-recommendation.component.css'],
})
export class MyRecommendationComponent implements OnInit {
	userName: string = ''
	phone: string = ''
	recommandationId: number = 0
	pageIndex: number = 1
	pageSize: number = 20
	pagination = []
	@ViewChild('table', { static: false }) table: any
	totalRecords: number = 0
	listDataNotChange = new Array<RecommendationObject>()
	listData = new Array<RecommendationObject>()

	recommendationStatistics: any = new RecommendationStatistics()
	totalRecommentdation: number = 0

	constructor(
		private toastr: ToastrService,
		private fileService: UploadFileService,
		private recommendationService: RecommendationService,
		private storageService: UserInfoStorageService,
		private _serviceCatalog: CatalogService,
		private router: Router,
		private captchaService: CaptchaService,
		private userService: UserService,
		private puRecommendationService: PuRecommendationService
	) {}

	ngOnInit() {
		this.userName = this.storageService.getFullName()
		this.getSoDienThoai()
		this.getList()

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

	getList() {
		let request = {
			Code: '',
			SendName: '',
			Content: '',
			UnitId: '',
			Field: '',
			Status: '',
			UnitProcessId: '',
			UserProcessId: this.storageService.getUserId(),
			pageIndex: this.pageIndex,
			PageSize: this.pageSize,
		}

		this.recommendationService.recommendationGetListProcess(request).subscribe((res) => {
			if (res != 'undefined' && res.success == RESPONSE_STATUS.success) {
				if (res.result.MRRecommendationGetAllWithProcess.length > 0) {
					this.listDataNotChange = res.result.MRRecommendationGetAllWithProcess
					this.listData = res.result.MRRecommendationGetAllWithProcess
					//this.pageIndex = res.result.pageIndex
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
		switch (status) {
			case 1:
				// chờ xl
				this.listData = this.listDataNotChange.filter((item) => {
					if (item.status == 2) return item
				})
				break
			case 2:
				// đã tiếp nhận

				this.listData = this.listDataNotChange.filter((item) => {
					if (item.status == 4 || item.status == 5 || item.status == 7 || item.status == 8) return item
				})
				break
			case 3:
				// đã trả lời
				this.listData = this.listDataNotChange.filter((item) => {
					if (item.status == 10) return item
				})

				break
			case 4:
				// bị từ chối
				this.listData = this.listDataNotChange.filter((item) => {
					if (item.status == 3 || item.status == 6 || item.status == 9) return item
				})
				break
			default:
				this.listData = this.listDataNotChange
				break
		}
	}

	dataStateChange() {
		this.pageIndex = 1
		// this.table.first = 0
		this.getList()
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
