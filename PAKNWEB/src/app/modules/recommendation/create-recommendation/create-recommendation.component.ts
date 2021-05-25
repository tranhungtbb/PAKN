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
		private notificationService: NotificationService
	) {}
	ngOnInit() {
		this.model = new RecommendationObject()
		this.getDropdown()
		this.activatedRoute.params.subscribe((params) => {
			this.model.id = +params['id']
			if (this.model.id != 0) {
				this.getData()
				this.title = 'Sửa'
			} else {
				this.model.typeObject = 1
				this.title = 'Thêm mới'
			}
			this.builForm()
		})
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

	getData() {
		let request = {
			Id: this.model.id,
		}
		this.recommendationService.recommendationGetById(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
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
				}
				this.lstField = response.result.lstField
				this.lstHashtag = response.result.lstHashTag
				this.lstBusiness = response.result.lstBusiness
				this.lstIndividual = response.result.lstIndividual
				this.lstObject = response.result.lstIndividual
				this.model.code = response.result.code
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
		if (typeObject == 1) {
			this.titleObject = 'Cá nhân'
			this.lstObject = this.lstIndividual
		} else {
			this.titleObject = 'Doanh nghiệp'
			this.lstObject = this.lstBusiness
		}
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

	onSave() {
		this.model.content = this.model.content.trim()
		this.model.title = this.model.title.trim()
		this.builForm()
		this.submitted = true
		if (this.form.invalid) {
			return
		}
		this.changeObject()
		this.model.status = RECOMMENDATION_STATUS.RECEIVE_APPROVED
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

					this.notificationService.insertNotificationTypeRecommendation({ recommendationId: response.result }).subscribe((res) => {})

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
	private unflatten(arr): any[] {
		var tree = [],
			mappedArr = {},
			arrElem,
			mappedElem

		// First map the nodes of the array to an object -> create a hash table.
		for (var i = 0, len = arr.length; i < len; i++) {
			arrElem = arr[i]
			mappedArr[arrElem.id] = arrElem
			mappedArr[arrElem.id]['children'] = []
		}

		for (var id in mappedArr) {
			if (mappedArr.hasOwnProperty(id)) {
				mappedElem = mappedArr[id]
				// If the element is not at the root level, add it to its parent array of children.
				if (mappedElem.parentId) {
					if (!mappedArr[mappedElem['parentId']]) continue
					mappedArr[mappedElem['parentId']]['children'].push(mappedElem)
				}
				// If the element is at the root level, add it to first level elements array.
				else {
					tree.push(mappedElem)
				}
			}
		}
		return tree
	}
}
