import { Component, ElementRef, OnInit, ViewChild } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { COMMONS } from 'src/app/commons/commons'
import { CONSTANTS, FILETYPE, PROCESS_STATUS_RECOMMENDATION, RECOMMENDATION_STATUS, RESPONSE_STATUS, STEP_RECOMMENDATION } from 'src/app/constants/CONSTANTS'
import { RecommendationConclusionObject, RecommendationForwardObject, RecommendationProcessObject, RecommendationViewObject } from 'src/app/models/recommendationObject'
import { UploadFileService } from 'src/app/services/uploadfiles.service'
import { RecommendationRequestService } from 'src/app/services/recommendation-req.service'
import { ActivatedRoute, Router } from '@angular/router'
import { HashtagObject } from 'src/app/models/hashtagObject'
import { CatalogService } from 'src/app/services/catalog.service'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'

declare var $: any

@Component({
	selector: 'app-detail-recommendation',
	templateUrl: './detail-recommendation.component.html',
	styleUrls: ['./detail-recommendation.component.css'],
})
export class DetailRecommendationComponent implements OnInit {
	model: RecommendationViewObject = new RecommendationViewObject()
	modelData: RequestData = new RequestData()
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
	@ViewChild('file', { static: false }) public file: ElementRef
	constructor(
		private toastr: ToastrService,
		private fileService: UploadFileService,
		private storeageService: UserInfoStorageService,
		private recommendationService: RecommendationRequestService,
		private _serviceCatalog: CatalogService,
		private router: Router,
		private _fb: FormBuilder,
		private activatedRoute: ActivatedRoute
	) { }

	ngOnInit() {
		this.getDropdown()
		this.model = new RecommendationViewObject()
		this.activatedRoute.params.subscribe((params) => {
			this.model.id = +params['id']
			if (this.model.id != 0) {
				this.getData()
			} else {
				this.model.typeObject = 1
			}
		})
	}

	getData() {
		let request = {
			Id: this.model.id,
		}
		this.recommendationService.recommendationGetByIdView(request).subscribe((response) => {
			console.log(response)
			if (response.success == RESPONSE_STATUS.success) {
				this.modelData = response.result.MRRecommendationKNCTGetById[0]
				let req = {
					Id: this.modelData.recommendationKNCTId,
				}
				this.recommendationService.recommendationGetFileByIdView(req).subscribe((res) => {
					if (res.success == RESPONSE_STATUS.success) {
						this.filesModel = res.result.MRRecommendationKNCTFilesGetByRecommendationId
						console.log(this.filesModel)
					}
				})
				this.model.shortName = this.getShortName('Kiến nghị cử tri')
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
	getDropdown() {

	}

	getShortName(string) {
		var names = string.split(' '),
			initials = names[0].substring(0, 1).toUpperCase()

		if (names.length > 1) {
			initials += names[names.length - 1].substring(0, 1).toUpperCase()
		}
		return initials
	}
}
export class RequestData {
	classify: string
	code: string
	content: string
	createdDate: string
	department: string
	endDate: string
	fieldId: number
	fieldName: string
	id: number
	place: string
	progress: string
	recommendationKNCTId: number
	rowNumber: number
	sendDate: string
	status: number
	term: string
}

