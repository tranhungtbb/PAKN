import { Component, ElementRef, OnInit, ViewChild } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { COMMONS } from 'src/app/commons/commons'
import { CONSTANTS, FILETYPE, MESSAGE_COMMON, PROCESS_STATUS_RECOMMENDATION, RECOMMENDATION_STATUS, RESPONSE_STATUS, STEP_RECOMMENDATION } from 'src/app/constants/CONSTANTS'
import {
	RecommendationConclusionObject,
	RecommendationForwardObject,
	RecommendationProcessObject,
	RecommendationViewObject,
	RecommendationSuggestObject,
	RecommnendationInfomationExchange,
} from 'src/app/models/recommendationObject'
import { UploadFileService } from 'src/app/services/uploadfiles.service'
import { RecommendationService } from 'src/app/services/recommendation.service'
import { ActivatedRoute, Router } from '@angular/router'
import { HashtagObject } from 'src/app/models/hashtagObject'
import { CatalogService } from 'src/app/services/catalog.service'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { RemindComponent } from 'src/app/modules/recommendation/remind/remind.component'
import { AppSettings } from 'src/app/constants/app-setting'
import { RecommendationCommentService } from 'src/app/services/recommendation-comment.service'
import { saveAs as importedSaveAs } from 'file-saver'
import { BusinessIndividualService } from 'src/app/services/business-individual.service'

declare var $: any

@Component({
	selector: 'app-view-combine-recommendation',
	templateUrl: './view-combine-recommendation.component.html',
	styleUrls: ['./view-combine-recommendation.component.css'],
})
export class ViewCombineRecommendationComponent implements OnInit {
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
	pageIndex: number = 1
	pageSize: number = 20
	APIADDRESS: string
	listData = new Array<RecommendationSuggestObject>()
	suggest: boolean = false
	totalRecords: number = 0
	dateNow: Date = new Date()
	lstGroupWord: any = []
	lstGroupWordSelected: any = []
	titleAccept: any = ''
	lstDictionariesWord: any = []
	@ViewChild('table', { static: false }) table: any
	@ViewChild('file', { static: false }) public file: ElementRef
	@ViewChild(RemindComponent, { static: true }) remindComponent: RemindComponent
	unitName = this.storeageService.getUnitName()
	markers: any = {}
	zoom: any = 15

	enableEdit = false

	constructor(
		private toastr: ToastrService,
		private fileService: UploadFileService,
		private storeageService: UserInfoStorageService,
		private recommendationService: RecommendationService,
		private _serviceCatalog: CatalogService,
		private router: Router,
		private _fb: FormBuilder,
		private activatedRoute: ActivatedRoute,
		private commentService: RecommendationCommentService,
		private biService: BusinessIndividualService
	) { }

	ngOnInit() {
		this.APIADDRESS = AppSettings.API_ADDRESS.replace('api/', '')
		this._serviceCatalog.wordGetListSuggest({}).subscribe(
			(response) => {
				if (response.success == RESPONSE_STATUS.success) {
					this.lstDictionariesWord = response.result.CAWordGetListSuggest
				} else {
					this.toastr.error(response.message)
				}
			},
			(error) => {
				console.log(error)
			}
		)
		this.buildFormForward()
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

		this.activatedRoute.queryParams.subscribe((params) => {
			let suggest = params['suggest']
			if (suggest) {
				this.suggest = suggest
			}
		})
	}

	getData() {
		let request = {
			Id: this.model.id,
		}
		this.getAllInfomationExchange(1)
		this.recommendationService.recommendationCombineGetByIdView(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				this.model = response.result.model
				if (this.model.lat && this.model.lng) {
					this.markers.lat = Number(this.model.lat)
					this.markers.lng = Number(this.model.lng)
				}
				if (this.model.status > RECOMMENDATION_STATUS.PROCESSING) {
					this.modelConclusion = response.result.modelConclusion
					this.files = response.result.filesConclusion
				} else {
					this.modelConclusion = new RecommendationConclusionObject()
				}
				this.lstHashtagSelected = response.result.lstHashtag
				this.filesModel = response.result.lstFiles
				this.model.shortName = this.getShortName(this.model.name)
				if (this.model.sendDate) {
					this.model.sendDate = new Date(this.model.sendDate)
				}
				this.remindComponent.getListRemind()
				this.sugestText()
				this.enableEdit = this.model.status == 1 && this.model.createdBy == this.storeageService.getUserId()
			} else {
				this.toastr.error(response.message)
			}
		}),
			(error) => {
				console.log(error)
			}
	}
	getDropdown() {
		let request = {
			UnitId: this.storeageService.getUnitId(),
		}
		this.recommendationService.recommendationGetDataForProcess(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				this.lstHashtag = response.result.lstHashtag
				this.lstUsers = response.result.lstUsersProcess
			} else {
				this.toastr.error(response.message)
			}
		}),
			(error) => {
				console.log(error)
			}
	}

	getListSuggest() {
		let lstId = []
		for (let index = 0; index < this.lstHashtagSelected.length; index++) {
			lstId.push(this.lstHashtagSelected[index].value)
		}
		let request = {
			ListIdHashtag: lstId.join(','),
			PageIndex: this.pageIndex,
			PageSize: this.pageSize,
		}

		this.recommendationService.recommendationGetSuggestReply(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result != null) {
					this.listData = []
					this.listData = response.result.MRRecommendationGetSuggestReply
					this.totalRecords = response.result.TotalCount
					$('#modalSuggestReply').modal('show')
				}
			} else {
				this.toastr.error(response.message)
			}
		}),
			(error) => {
				console.log(error)
				alert(error)
			}
	}

	onPageChange(event: any) {
		this.pageSize = event.rows
		this.pageIndex = event.first / event.rows + 1
		this.getListSuggest()
	}

	useConclustion(contentConclustion) {
		this.modelConclusion.content = contentConclustion
		$('#modalSuggestReply').modal('hide')
	}

	getShortName(string) {
		var names = string.split(' '),
			initials = names[0].substring(0, 1).toUpperCase()

		if (names.length > 1) {
			initials += names[names.length - 1].substring(0, 1).toUpperCase()
		}
		return initials
	}

	onCreateHashtag(e) {
		if (e.target.value != null && e.target.value != '' && e.target.value.trim() != '' && e.keyCode == 13) {
			var isExist = false
			for (var i = 0; i < this.lstHashtag.length; i++) {
				if (this.lstHashtag[i].text.toUpperCase() == e.target.value) {
					isExist = true
					break
				}
			}
			if (isExist == false) {
				this.modelHashTagAdd = new HashtagObject()
				this.modelHashTagAdd.name = e.target.value
				this._serviceCatalog.hashtagInsert(this.modelHashTagAdd).subscribe((response) => {
					if (response.success == RESPONSE_STATUS.success) {
						this.hashtagId = response.result
						this.getDropdown()
					}
				}),
					(error) => {
						console.error(error)
					}
			}
		}
	}

	onAddHashtag() {
		var isExist = false
		for (var i = 0; i < this.lstHashtagSelected.length; i++) {
			if (this.lstHashtagSelected[i].value == this.hashtagId) {
				isExist = true
				break
			}
		}
		if (!isExist) {
			let hashtag = this.lstHashtag.find((x) => x.value == this.hashtagId)
			let obj = {
				RecommendationId: this.model.id,
				HashtagId: this.hashtagId,
				HashtagName: hashtag.text,
			}
			this.recommendationService.addHashtagForRecommentdation(obj).subscribe()

			for (var i = 0; i < this.lstHashtag.length; i++) {
				if (this.lstHashtag[i].value == this.hashtagId) {
					this.lstHashtagSelected.push(this.lstHashtag[i])
					break
				}
			}
		}
	}
	onRemoveHashtag(item: any) {
		let obj = {
			RecommendationId: this.model.id,
			HashtagId: item.value,
		}
		this.recommendationService.deleteHashtagForRecommentdation(obj).subscribe()
		let index = this.lstHashtagSelected.indexOf(item)
		this.lstHashtagSelected.splice(index, 1)
	}

	onUpload(event) {
		if (event.target.files.length == 0) {
			return
		}
		const check = this.fileService.checkFileWasExitsted(event, this.files)
		if (check === 1) {
			for (let item of event.target.files) {
				FILETYPE.forEach((fileType) => {
					if (item.type == fileType.text) {
						item.fileType = fileType.value
						this.files.push(item)
					}
				})
				if (!item.fileType) {
					this.toastr.error('Định dạng không được hỗ trợ')
				}
			}
		} else if (check === 2) {
			this.toastr.error('Không được phép đẩy trùng tên file lên hệ thống')
		} else {
			this.toastr.error('File tải lên vượt quá dung lượng cho phép 10MB')
		}
		this.file.nativeElement.value = ''
	}

	filesDelete: any = []

	onRemoveFile(args) {
		const index = this.files.indexOf(args)
		const file = this.files[index]
		this.files.splice(index, 1)
		this.filesDelete.push(args)
	}

	onProcessConclusion() {
		if (this.modelConclusion.content == '' || this.modelConclusion.content.trim() == '') {
			this.toastr.error('Vui lòng nhập nội dung')
			return
		} else {
			this.modelConclusion.recommendationId = this.model.id
			var request = {
				DataConclusion: this.modelConclusion,
				Files: this.files,
				RecommendationStatus: RECOMMENDATION_STATUS.APPROVE_WAIT,
			}
			this.recommendationService.recommendationProcessConclusionCombine(request).subscribe((response) => {
				if (response.success == RESPONSE_STATUS.success) {
					this.toastr.success(COMMONS.PROCESS_SUCCESS)

					return this.router.navigate(['/quan-tri/kien-nghi/tham-muu-don-vi'])
				} else {
					this.toastr.error(response.message)
				}
			}),
				(err) => {
					console.error(err)
				}

		}
	}

	modelForward: RecommendationForwardObject = new RecommendationForwardObject()
	formForward: FormGroup
	lstUnitNotMain: any = []

	get f() {
		return this.formForward.controls
	}

	submitted: boolean = false
	buildFormForward() {
		this.formForward = this._fb.group({
			unitReceiveId: [this.modelForward.unitReceiveId, Validators.required],
			expiredDate: [this.modelForward.expiredDate],
			content: [this.modelForward.content],
		})
	}

	rebuilFormForward() {
		this.formForward.reset({
			unitReceiveId: this.modelForward.unitReceiveId,
			expiredDate: this.modelForward.expiredDate,
			content: this.modelForward.content,
		})
	}
	preForward() {
		this.modelForward = new RecommendationForwardObject()
		this.modelForward.recommendationId = this.model.id
		this.rebuilFormForward()
		this.recommendationService.recommendationGetDataForForward({}).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result != null) {
					this.lstUnitNotMain = response.result.lstUnitNotMain
					$('#modalForward').modal('show')
				}
			} else {
				this.toastr.error(response.message)
			}
		}),
			(error) => {
				console.log(error)
			}
	}

	onForward() {
		this.modelForward.content = this.modelForward.content.trim()
		this.submitted = true
		if (this.formForward.invalid) {
			return
		}
		this.modelForward.step = STEP_RECOMMENDATION.PROCESS
		this.modelForward.status = PROCESS_STATUS_RECOMMENDATION.WAIT
		var request = {
			_mRRecommendationForwardInsertIN: this.modelForward,
			RecommendationStatus: RECOMMENDATION_STATUS.PROCESS_WAIT,
			ListHashTag: this.lstHashtagSelected,
			IsList: false,
		}


		this.recommendationService.recommendationForward(request, this.model.title).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				$('#modalForward').modal('hide')
				this.getData()
				this.toastr.success(COMMONS.FORWARD_SUCCESS)
			} else {
				this.toastr.error(response.message)
			}
		}),
			(err) => {
				console.error(err)
			}
	}
	modelProcess: RecommendationProcessObject = new RecommendationProcessObject()
	recommendationStatusProcess: number = 0
	preProcess(status: number) {
		this.modelProcess.status = status
		this.modelProcess.id = this.model.idProcess
		this.modelProcess.step = this.model.stepProcess
		this.modelProcess.recommendationId = this.model.id
		this.modelProcess.reactionaryWord = false
		this.modelProcess.reasonDeny = ''
		if (status == PROCESS_STATUS_RECOMMENDATION.DENY) {
			if (this.model.status == RECOMMENDATION_STATUS.RECEIVE_WAIT) {
				this.recommendationStatusProcess = RECOMMENDATION_STATUS.RECEIVE_DENY
				this.recommendationService.recommendationGetDataForProcess({}).subscribe((response) => {
					if (response.success == RESPONSE_STATUS.success) {
						if (response.result != null) {
							this.lstGroupWord = response.result.lstGroupWord
							this.lstGroupWordSelected = []
							$('#modalReject').modal('show')
						}
					} else {
						this.toastr.error(response.message)
					}
				}),
					(error) => {
						console.log(error)
						alert(error)
					}
			} else if (this.model.status == RECOMMENDATION_STATUS.PROCESS_WAIT) {
				this.recommendationStatusProcess = RECOMMENDATION_STATUS.PROCESS_DENY
				$('#modalReject').modal('show')
			} else if (this.model.status == RECOMMENDATION_STATUS.APPROVE_WAIT) {
				this.recommendationStatusProcess = RECOMMENDATION_STATUS.APPROVE_DENY
				$('#modalReject').modal('show')
			}
		} else {
			if (this.model.status == RECOMMENDATION_STATUS.RECEIVE_WAIT) {
				this.titleAccept = 'Anh/Chị có chắc chắn muốn tiếp nhận Phản ánh, Kiến nghị này?'
				this.recommendationStatusProcess = RECOMMENDATION_STATUS.RECEIVE_APPROVED
			} else if (this.model.status == RECOMMENDATION_STATUS.PROCESS_WAIT) {
				this.titleAccept = 'Anh/Chị có chắc chắn muốn giải quyết Phản ánh, Kiến nghị này?'
				this.recommendationStatusProcess = RECOMMENDATION_STATUS.PROCESSING
			} else if (this.model.status == RECOMMENDATION_STATUS.APPROVE_WAIT) {
				this.titleAccept = 'Anh/Chị có chắc chắn muốn phê duyệt Phản ánh, Kiến nghị này?'
				this.recommendationStatusProcess = RECOMMENDATION_STATUS.FINISED
			}
			$('#modalAccept').modal('show')
		}
	}
	onProcessAccept() {
		var request = {
			_mRRecommendationForwardProcessIN: this.modelProcess,
			RecommendationStatus: this.recommendationStatusProcess,
			ReactionaryWord: this.modelProcess.reactionaryWord,
			ListHashTag: this.lstHashtagSelected,
			IsList: false,
		}
		this.recommendationService.recommendationProcess(request, this.model.title).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				$('#modalAccept').modal('hide')
				if (this.recommendationStatusProcess == RECOMMENDATION_STATUS.FINISED) {
					this.toastr.success(COMMONS.APPROVED_SUCCESS)
				} else {
					this.toastr.success(COMMONS.ACCEPT_SUCCESS)
				}
				this.getData()
			} else {
				this.toastr.error(response.message)
			}
		}),
			(err) => {
				console.error(err)
			}
	}
	onProcessDeny() {
		if (this.modelProcess.reasonDeny == '' || this.modelProcess.reasonDeny.trim() == '') {
			this.toastr.error('Vui lòng nhập lý do')
			return
		} else {
			var request = {
				_mRRecommendationForwardProcessIN: this.modelProcess,
				RecommendationStatus: this.recommendationStatusProcess,
				ReactionaryWord: this.modelProcess.reactionaryWord,
				ListGroupWordSelected: this.lstGroupWordSelected.join(','),
				ListHashTag: this.lstHashtagSelected,
				IsList: false,
			}
			this.recommendationService.recommendationProcess(request, this.model.title).subscribe((response) => {
				if (response.success == RESPONSE_STATUS.success) {
					$('#modalReject').modal('hide')
					this.toastr.success(COMMONS.DENY_SUCCESS)
					this.getData()
				} else {
					this.toastr.error(response.message)
				}
			}),
				(err) => {
					console.error(err)
				}
		}
	}
	modelDetail: any
	// preView info
	preViewInfo = () => {
		switch (this.model.typeObject) {
			// cá nhân
			case 2:
				this.biService.individualById({ Id: this.model.biSendId }).subscribe(
					(res) => {
						if (res.success == RESPONSE_STATUS.success) {
							if (res.result.InvididualGetByID.length > 0) {
								this.modelDetail = res.result.InvididualGetByID[0]
								$('#modalDetail').modal('show')
							} else {
								this.toastr.error('Đã xảy ra lỗi trong quá trình xử lý')
							}
						} else {
							this.toastr.error(res.message)
						}
					},
					(err) => {
						console.log(err)
					}
				)
				break
			// doanh nghiệp
			case 3:
				this.biService.businessGetByID({ Id: this.model.biSendId }).subscribe(
					(res) => {
						if (res.success == RESPONSE_STATUS.success) {
							if (res.result.BusinessGetById.length > 0) {
								this.modelDetail = res.result.BusinessGetById[0]
								$('#modalDetail').modal('show')
							} else {
								this.toastr.error('Đã xảy ra lỗi trong quá trình xử lý')
							}
						} else {
							this.toastr.error(res.message)
						}
					},
					(err) => {
						console.log(err)
					}
				)
				break
			default:
				return
		}
	}

	//infomationExchange area
	infoExchangeModel: RecommnendationInfomationExchange = new RecommnendationInfomationExchange()
	infomationExchangeQuery: any = {
		pageIndex: 1,
		pageSize: 20,
		recommendationId: 0,
		isPublish: false
	}
	listInfomationExchange: any[] = []
	totalInfomationExchange = 0

	onInsertInfomationExchange() {
		this.infoExchangeModel.fullName = this.storeageService.getFullName()
		this.infoExchangeModel.createdDate = new Date()
		this.infoExchangeModel.recommendationId = this.model.id
		this.infoExchangeModel.isPublish = false
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


	// sugest
	sugestText = () => {
		if ([2, 5, 8].includes(this.model.status)) {
			let content = this.model.content //.replace(/\\n/g, String.fromCharCode(13, 10))
			for (let index = 0; index < this.lstDictionariesWord.length; index++) {
				var nameWord = new RegExp(this.lstDictionariesWord[index].name, 'ig')
				content = content.replace(
					nameWord,
					'<span class="txthighlight" title="' + this.lstDictionariesWord[index].description + '">' + this.lstDictionariesWord[index].name + '</span>'
				)
			}
			this.model.content = content
		}
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
