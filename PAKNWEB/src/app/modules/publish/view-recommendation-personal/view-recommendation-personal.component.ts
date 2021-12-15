import { Component, ElementRef, OnInit, ViewChild } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { CONSTANTS, MESSAGE_COMMON, RECOMMENDATION_STATUS, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { RecommendationConclusionObject, RecommendationViewObject, RecommnendationInfomationExchange } from 'src/app/models/recommendationObject'
import { UploadFileService } from 'src/app/services/uploadfiles.service'
import { RecommendationService } from 'src/app/services/recommendation.service'
import { ActivatedRoute, Router } from '@angular/router'
import { HashtagObject } from 'src/app/models/hashtagObject'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { saveAs as importedSaveAs } from 'file-saver'
import { RecommendationCommentService } from 'src/app/services/recommendation-comment.service'
declare var $: any

@Component({
	selector: 'app-view-recommendation-personal',
	templateUrl: './view-recommendation-personal.component.html',
	styleUrls: ['./view-recommendation-personal.component.css'],
})
export class ViewRecommendationPersonalComponent implements OnInit {
	model: RecommendationViewObject = new RecommendationViewObject()
	modelConclusion: RecommendationConclusionObject = new RecommendationConclusionObject()
	lstHashtag: any[] = []
	lstUsers: any[] = []
	lstHashtagSelected: any[] = []
	filesModel: any[] = []
	files: any[] = []
	modelHashTagAdd: HashtagObject = new HashtagObject()
	hashtagId: number = null
	fileAccept = CONSTANTS.FILEACCEPT
	userLoginId: number = this.storeageService.getUserId()
	unitLoginId: number = this.storeageService.getUnitId()
	isLogin: boolean = this.storeageService.getIsHaveToken()
	denyContent: any
	@ViewChild('file', { static: false }) public file: ElementRef
	constructor(
		private toastr: ToastrService,
		private fileService: UploadFileService,
		private storeageService: UserInfoStorageService,
		private recommendationService: RecommendationService,
		private router: Router,
		private commentService: RecommendationCommentService,
		private activatedRoute: ActivatedRoute
	) { }

	ngOnInit() {

		this.model = new RecommendationViewObject()
		this.activatedRoute.params.subscribe((params) => {
			this.model.id = +params['id']
			if (this.model.id != 0) {
				this.getData()
				this.getAllInfomationExchange(1)
			} else {
				this.model.typeObject = 1
			}
		})
	}

	getData() {
		let request = {
			Id: this.model.id,
		}
		this.recommendationService.recommendationGetByIdViewPublic(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				this.model = response.result.model
				if (this.model.status > RECOMMENDATION_STATUS.PROCESSING) {
					this.modelConclusion = response.result.modelConclusion
					this.files = response.result.filesConclusion
				} else {
					this.modelConclusion = new RecommendationConclusionObject()
				}
				if ([3, 6, 9].includes(this.model.status)) {
					this.denyContent = response.result.denyContent[0]
				}
				this.lstHashtagSelected = response.result.lstHashtag
				this.filesModel = response.result.lstFiles
				this.model.shortName = this.getShortName(this.model.name)
				if (this.model.sendDate) {
					this.model.sendDate = new Date(this.model.sendDate)
				}
			} else {
				this.toastr.error(response.message)
			}
		}),
			(error) => {
				console.log(error)
			}
	}


	getShortName(string) {
		var names = string.split(' '),
			initials = names[0].substring(0, 1).toUpperCase()

		if (names.length > 1) {
			initials += names[names.length - 1].substring(0, 1).toUpperCase()
		}
		return initials
	}




	infoExchangeModel: RecommnendationInfomationExchange = new RecommnendationInfomationExchange()
	infomationExchangeQuery: any = {
		pageIndex: 1,
		pageSize: 20,
		recommendationId: 0,
		isPublish: true
	}
	listInfomationExchange: any[] = []
	totalInfomationExchange = 0

	onInsertInfomationExchange() {
		this.infoExchangeModel.fullName = this.storeageService.getFullName()
		this.infoExchangeModel.createdDate = new Date()
		this.infoExchangeModel.recommendationId = this.model.id
		this.infoExchangeModel.isPublish = true
		this.infoExchangeModel.contents = this.infoExchangeModel.contents == null ? '' : this.infoExchangeModel.contents.trim()
		if (!this.infoExchangeModel.contents) {
			this.toastr.error('Nội dung trao đổi không được để trống')
			return
		}

		this.commentService.insertInfomationExchange(this.infoExchangeModel).subscribe((res) => {
			if (res.success != RESPONSE_STATUS.success) {
				this.toastr.error(res.message)
				return
			}
			this.toastr.success(MESSAGE_COMMON.ADD_SUCCESS)
			this.listInfomationExchange.push(this.infoExchangeModel)
			this.totalInfomationExchange += 1
			this.infoExchangeModel = new RecommnendationInfomationExchange()

		})
	}

	getAllInfomationExchange(pageIndex: any) {
		this.infomationExchangeQuery.pageIndex = pageIndex
		this.infomationExchangeQuery.recommendationId = this.model.id
		this.commentService.getAllInfomationChangeOnPage(this.infomationExchangeQuery).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				this.listInfomationExchange = res.result.MRInfomationExchangeAllOnPage
				this.totalInfomationExchange = res.result.TotalCount
			}
		})
	}

	DownloadFile(file: any) {
		var request = {
			Path: file.filePath,
			Name: file.name,
		}
		this.fileService.downloadFile(request).subscribe(
			(response) => {
				var blob = new Blob([response], { type: response.type })
				importedSaveAs(blob, file.name)
			},
			(error) => {
				this.toastr.error('Không tìm thấy file trên hệ thống')
			}
		)
	}
}
