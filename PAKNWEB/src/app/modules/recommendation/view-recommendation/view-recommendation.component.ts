import { Component, ElementRef, OnInit, ViewChild } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { COMMONS } from 'src/app/commons/commons'
import { CONSTANTS, FILETYPE, RECOMMENDATION_STATUS, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { RecommendationConclusionObject, RecommendationViewObject } from 'src/app/models/recommendationObject'
import { UploadFileService } from 'src/app/services/uploadfiles.service'
import { RecommendationService } from 'src/app/services/recommendation.service'
import { ActivatedRoute, Router } from '@angular/router'
import { HashtagObject } from 'src/app/models/hashtagObject'
import { CatalogService } from 'src/app/services/catalog.service'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'

declare var $: any

@Component({
	selector: 'app-view-recommendation',
	templateUrl: './view-recommendation.component.html',
	styleUrls: ['./view-recommendation.component.css'],
})
export class ViewRecommendationComponent implements OnInit {
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
	@ViewChild('file', { static: false }) public file: ElementRef
	constructor(
		private toastr: ToastrService,
		private fileService: UploadFileService,
		private storeageService: UserInfoStorageService,
		private recommendationService: RecommendationService,
		private _serviceCatalog: CatalogService,
		private router: Router,
		private activatedRoute: ActivatedRoute
	) {}

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
			if (response.success == RESPONSE_STATUS.success) {
				this.model = response.result.model
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
	getDropdown() {
		let request = {
			UnitId: this.storeageService.getUnitId(),
		}
		this.recommendationService.recommendationGetDataForProcess(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				this.lstHashtag = response.result.lstHashtag
				this.lstUsers = response.result.lstUsers
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
			for (var i = 0; i < this.lstHashtag.length; i++) {
				if (this.lstHashtag[i].value == this.hashtagId) {
					this.lstHashtagSelected.push(this.lstHashtag[i])
					break
				}
			}
		}
	}
	onRemoveHashtag(item: any) {
		for (let index = 0; index < this.lstHashtagSelected.length; index++) {
			if (this.lstHashtagSelected[index].id == item.id) {
				this.lstHashtagSelected.splice(index, 1)
				break
			}
		}
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

	onRemoveFile(args) {
		const index = this.files.indexOf(args)
		const file = this.files[index]
		this.files.splice(index, 1)
	}

	onProcessConclusion() {
		if (this.modelConclusion.content == '' || this.modelConclusion.content.trim() == '') {
			this.toastr.error('Vui lòng nhập nội dung')
			return
		} else if (this.modelConclusion.receiverId == null) {
			this.toastr.error('Vui lòng nhập người phê duyệt')
			return
		} else {
			this.modelConclusion.recommendationId = this.model.id
			var request = {
				DataConclusion: this.modelConclusion,
				Files: this.files,
				RecommendationStatus: RECOMMENDATION_STATUS.APPROVE_WAIT,
			}
			this.recommendationService.recommendationProcessConclusion(request).subscribe((response) => {
				if (response.success == RESPONSE_STATUS.success) {
					$('#modalReject').modal('hide')
					this.toastr.success(COMMONS.PROCESS_SUCCESS)
					return this.router.navigate(['/quan-tri/kien-nghi/dang-giai-quyet'])
				} else {
					this.toastr.error(response.message)
				}
			}),
				(err) => {
					console.error(err)
				}
		}
	}
}
