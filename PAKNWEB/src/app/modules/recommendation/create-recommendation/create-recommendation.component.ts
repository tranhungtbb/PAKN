import { Component, ElementRef, OnInit, ViewChild } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { COMMONS } from 'src/app/commons/commons'
import { CONSTANTS, FILETYPE, RECOMMENDATION_STATUS, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { RecommendationObject } from 'src/app/models/recommendationObject'
import { UploadFileService } from 'src/app/services/uploadfiles.service'
import { RecommendationService } from 'src/app/services/recommendation.service'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { ActivatedRoute, Router } from '@angular/router'
import { HashtagObject } from 'src/app/models/hashtagObject'
import { CatalogService } from 'src/app/services/catalog.service'
import { NotificationService } from 'src/app/services/notification.service'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'

@Component({
	selector: 'app-create-recommendation',
	templateUrl: './create-recommendation.component.html',
	styleUrls: ['./create-recommendation.component.css'],
})
export class CreateRecommendationComponent implements OnInit {
	form: FormGroup
	model: RecommendationObject = new RecommendationObject()
	titleObject: string = 'Cá nhân'
	lstHuongXuLy: any[] = [
		{ value: 1, text: 'Chuyển đơn' },
		{ value: 2, text: 'Thụ lý giải quyết' },
		{ value: 3, text: 'Trả đơn' },
		{ value: 4, text: 'Từ chối xử lý' },
	]
	isIndividual: boolean = true
	title: string = 'Thêm mới'
	spc: string = '-'
	treeUnit: any[]
	lstUnit: any[] = []
	lstField: any[] = []
	lstBusiness: any[] = []
	lstIndividual: any[] = []
	lstObject: any[] = []
	lstHashtag: any[] = []
	lstHashtagSelected: any[] = []
	hashtagId: number = null
	fileAccept = CONSTANTS.FILEACCEPT
	files: any[] = []
	lstXoaFile: any[] = []
	submitted: boolean = false
	modelHashTagAdd: HashtagObject = new HashtagObject()
	dateNow: Date = new Date()
	@ViewChild('file', { static: false }) public file: ElementRef
	constructor(
		private toastr: ToastrService,
		private fileService: UploadFileService,
		private recommendationService: RecommendationService,
		private _serviceCatalog: CatalogService,
		private router: Router,
		private activatedRoute: ActivatedRoute,
		private notificationService: NotificationService,
		private storeageService: UserInfoStorageService
	) {}
	ngOnInit() {
		this.model = new RecommendationObject()
		this.activatedRoute.params.subscribe((params) => {
			this.model.id = +params['id']

			if (this.model.id != 0) {
				this.getData()
				this.title = 'Sửa'
			} else {
				this.model.typeObject = 2
				this.title = 'Thêm mới'
			}
			debugger
			let typeObject = params['typeObject']
			if (typeObject) {
				this.isIndividual = Number(typeObject) == 1 ? true : false
				if (this.isIndividual) {
					this.changeTypeObject(2)
				} else {
					this.changeTypeObject(3)
				}
			} else {
				this.model.typeObject = 2
			}
			this.builForm()
		})

		this.getDropdown()
		this.recommendationService.recommendationGetDataForCreate({}).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result != null) {
					this.lstUnit = response.result.lstUnit
				}
			} else {
			}
		}),
			(error) => {
				console.log(error)
			}
	}

	onCreateHashtag(e) {
		if (e.target.value != undefined && e.target.value != null && e.target.value != '' && e.target.value.trim() != '' && e.keyCode == 13) {
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

	getData() {
		let request = {
			Id: this.model.id,
		}
		this.recommendationService.recommendationGetById(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (this.model.createdBy != this.storeageService.getUserId() && this.model.status == 1) return

				this.model = response.result.model
				this.lstHashtagSelected = response.result.lstHashtag
				this.files = response.result.lstFiles
				if (this.model.typeObject == 1) {
					this.titleObject = 'Cá nhân'
					this.lstObject = this.lstIndividual
				} else {
					this.titleObject = 'Doanh nghiệp'
					this.lstObject = this.lstBusiness
				}
				this.changeObject()
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
		let request = {}
		this.recommendationService.recommendationGetDataForCreate(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result != null) {
					this.lstUnit = response.result.lstUnit
					this.lstField = response.result.lstField
					this.lstHashtag = response.result.lstHashTag
					this.lstBusiness = response.result.lstBusiness
					this.lstIndividual = response.result.lstIndividual
					this.lstObject = response.result.lstIndividual
					this.model.code = response.result.code
					//
					if (this.model.id == 0) {
						// nếu thêm mới và data local stogate vẫn có thì lấy ra
						let dataRecommetdation = JSON.parse(this.storeageService.getRecommentdationObjectRemember())
						if (dataRecommetdation) {
							this.model = { ...dataRecommetdation.model, code: response.result.code, sendId: null }
							this.lstHashtagSelected = [...dataRecommetdation.lstHashtagSelected]
							if (this.model.sendDate) {
								this.model.sendDate = new Date(this.model.sendDate)
							}
						}
					}
				}
			} else {
				this.toastr.error(response.message)
			}
		}),
			(error) => {
				console.log(error)
			}
	}

	changeTypeObject(typeObject: number) {
		this.model.sendId = null
		if (typeObject == 2) {
			this.titleObject = 'Cá nhân'
			this.isIndividual = true
			this.model.typeObject = 2
			this.lstObject = this.lstIndividual
		} else {
			this.titleObject = 'Doanh nghiệp'
			this.isIndividual = false
			this.model.typeObject = 3
			this.lstObject = this.lstBusiness
		}
	}
	redirectToCreateIndividualBusiness() {
		if (this.model.typeObject == 2) {
			localStorage.setItem('isIndividual', 'true')
			this.router.navigate(['/quan-tri/ca-nhan-doanh-nghiep/ca-nhan'])
		} else if (this.model.typeObject == 3) {
			localStorage.setItem('isIndividual', 'false')
			this.router.navigate(['/quan-tri/ca-nhan-doanh-nghiep/them-moi/0'])
		}
		return
	}

	changeObject() {
		this.model.name = ''
		if (this.model.sendId != null) {
			for (let index = 0; index < this.lstObject.length; index++) {
				if (this.model.sendId == this.lstObject[index].value) {
					this.model.name = this.lstObject[index].text
					break
				}
			}
		}
	}

	builForm() {
		this.form = new FormGroup({
			// code: new FormControl(this.model.code, [Validators.required]),
			title: new FormControl(this.model.title, [Validators.required]),
			content: new FormControl(this.model.content, [Validators.required]),
			field: new FormControl(this.model.field, [Validators.required]),
			unitId: new FormControl(this.model.unitId, [Validators.required]),
			sendId: new FormControl(this.model.sendId, [Validators.required]),
			sendDate: new FormControl(this.model.sendDate, [Validators.required]),
			hashtag: new FormControl(this.hashtagId),
		})
	}
	get f() {
		return this.form.controls
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
		this.lstXoaFile.push(file)
		this.files.splice(index, 1)
	}

	onSave(onSend: any) {
		this.model.content = this.model.content.trim()
		this.model.title = this.model.title.trim()
		this.builForm()
		this.submitted = true
		if (this.form.invalid) {
			return
		}
		this.changeObject()

		onSend == false ? (this.model.status = RECOMMENDATION_STATUS.CREATED) : (this.model.status = RECOMMENDATION_STATUS.RECEIVE_APPROVED)
		const request = {
			Data: this.model,
			Hashtags: this.lstHashtagSelected,
			Files: this.files,
			LstXoaFile: this.lstXoaFile,
		}
		if (this.model.id == 0) {
			this.recommendationService.recommendationInsert(request).subscribe((response) => {
				if (response.success == RESPONSE_STATUS.success) {
					this.toastr.success(COMMONS.ADD_SUCCESS)
					localStorage.removeItem('recommentdationObjRemember')
					return this.router.navigate(['/quan-tri/kien-nghi/danh-sach-tong-hop'])
				} else {
					this.toastr.error(response.message)
				}
			}),
				(err) => {
					console.error(err)
				}
		} else {
			this.recommendationService.recommendationUpdate(request).subscribe((response) => {
				if (response.success == RESPONSE_STATUS.success) {
					this.toastr.success(COMMONS.UPDATE_SUCCESS)
					localStorage.removeItem('recommentdationObjRemember')
					return this.router.navigate(['/quan-tri/kien-nghi/danh-sach-tong-hop'])
				} else {
					this.toastr.error(response.message)
				}
			}),
				(err) => {
					console.error(err)
				}
		}
	}
	back() {
		localStorage.removeItem('isIndividual')
		this.router.navigate(['/quan-tri/kien-nghi/danh-sach-tong-hop'])
	}
}
