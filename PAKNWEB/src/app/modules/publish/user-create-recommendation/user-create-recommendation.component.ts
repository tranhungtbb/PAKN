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
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { AppSettings } from 'src/app/constants/app-setting'
import { Api } from 'src/app/constants/api'
import { CaptchaService } from 'src/app/services/captcha-service'
import { NotificationService } from 'src/app/services/notification.service'
import { UnitService } from '../../../services/unit.service'
import { ViewRightComponent } from 'src/app/modules/publish/view-right/view-right.component'

@Component({
	selector: 'app-user-create-recommendation',
	templateUrl: './user-create-recommendation.component.html',
	styleUrls: ['./user-create-recommendation.component.css'],
})
export class CreateRecommendationComponent implements OnInit {
	form: FormGroup
	model: RecommendationObject = new RecommendationObject()
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
	@ViewChild(ViewRightComponent, { static: true }) viewRightComponent: ViewRightComponent
	captchaImage: any = ''
	captchaCode: string = null
	resultsRecommendation: any = []
	lstDictionariesWord: any = []

	unitSelected: any = { name: null, id: null }
	lstUnitTree: any[] = []

	constructor(
		private unitService: UnitService,
		private toastr: ToastrService,
		private fileService: UploadFileService,
		private recommendationService: RecommendationService,
		private storageService: UserInfoStorageService,
		private _serviceCatalog: CatalogService,
		private router: Router,
		private captchaService: CaptchaService,
		private activatedRoute: ActivatedRoute,
		private notificationService: NotificationService
	) {}

	ngOnInit() {
		this.model = new RecommendationObject()
		this.reloadImage()
		this.getDropdown()
		this.activatedRoute.params.subscribe((params) => {
			if (params['id']) {
				this.model.id = +params['id']
			}

			if (this.model.id != 0) {
				this.getData()
			} else {
				this.model.typeObject = 1
			}
			this.builForm()
		})
	}

	//unit select event
	onSelectUnit(item: any) {
		this.unitSelected = item
		$('#_unitId .ng-input input').val(item.name)
		this.model.unitId = item.id
	}
	changeSelectUnit(event: any) {
		if (!event) {
			$('#_unitId .ng-input input').val('')
			this.unitSelected = {}
			this.model.unitId = null
		}
	}
	searchRecommendation() {
		this.resultsRecommendation = []
		if (this.model.title != '' && this.model.title.trim() != '') {
			this.recommendationService.recommendationGetSuggestCreate({ Title: this.model.title }).subscribe((response) => {
				if (response.success == RESPONSE_STATUS.success) {
					this.resultsRecommendation = response.result.MRRecommendationGetSuggestCreate
					for (let item of this.model.title.trim().toLowerCase().split(' ')) {
						this.resultsRecommendation.forEach((recommens) => {
							recommens.title.split(' ').forEach((element) => {
								if (item == element.toLowerCase()) {
									recommens.title = recommens.title.replaceAll(element, '<span class ="text-primary">' + element + '</span>')
								}
							})
						})
					}
				} else {
					this.toastr.error(response.message)
				}
			}),
				(error) => {
					console.log(error)
				}
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
				if (this.model.sendDate) {
					this.model.sendDate = new Date(this.model.sendDate)
					let unitItem = this.lstUnit.find((c) => c.id == this.model.unitId)
					$('#_unitId .ng-input input').val(unitItem.name)
					$('#_unitId .ng-select-container').addClass('ng-has-value')
				}
				this.hightLightText()
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
		this.recommendationService.recommendationGetDataForCreate(request).subscribe(
			(response) => {
				if (response.success == RESPONSE_STATUS.success) {
					//this.lstUnit = response.result.lstUnit
					this.lstField = response.result.lstField
					this.lstHashtag = response.result.lstHashTag
					this.lstBusiness = response.result.lstBusiness
					this.lstIndividual = response.result.lstIndividual
					this.lstObject = response.result.lstIndividual
					this.model.code = response.result.code

				} else {
					this.toastr.error(response.message)
				}
			},
			(error) => {
				console.log(error)
			}
		)
		this._serviceCatalog.wordGetListSuggest(request).subscribe(
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

		this.unitService.getAll({}).subscribe(
			(res) => {
				if (res.success != 'OK') return
				let listUnit = res.result.CAUnitGetAll.map((e) => {
					let item = {
						id: e.id,
						name: e.name,
						parentId: e.parentId == null ? 0 : e.parentId,
						unitLevel: e.unitLevel,
						children: [],
					}
					return item
				})

				this.lstUnit = listUnit
				// get data from localstogate
				if(this.model.id == 0){
					// nếu thêm mới và data local stogate vẫn có thì lấy ra
					let dataRecommetdation = JSON.parse(this.storageService.getRecommentdationObjectRemember())
					if(dataRecommetdation){
						this.model = {...dataRecommetdation}
						this.searchRecommendation()
						this.hightLightText()
						if(this.model.unitId){
							this.unitSelected = this.lstUnit.find(x=>x.id == this.model.unitId)
						}
					}
				}
				this.lstUnitTree = this.unflatten(listUnit)
			},
			(err) => {
				console.log(err)
			}
		)
	}

	builForm() {
		this.form = new FormGroup({
			title: new FormControl(this.model.title, [Validators.required]),
			content: new FormControl(this.model.content, [Validators.required]),
			field: new FormControl(this.model.field, [Validators.required]),
			unitId: new FormControl(this.model.unitId),
			hashtag: new FormControl(this.hashtagId),
			captcha: new FormControl(this.captchaCode, [Validators.required]),
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
			this.toastr.error('File tải lên vượt quá dung lượng cho phép 20MB')
		}
		this.file.nativeElement.value = ''
	}

	onRemoveFile(args) {
		const index = this.files.indexOf(args)
		const file = this.files[index]
		this.lstXoaFile.push(file)
		this.files.splice(index, 1)
	}

	onSave(status) {
		let isLogin = this.storageService.getSaveLogin()
		this.model.content = this.model.content.trim()
		this.model.title = this.model.title.trim()
		if (this.model.content == null || this.model.content == '') {
			return
		}

		if (this.model.title == null || this.model.title == '') {
			return
		}
		this.submitted = true
		if (this.form.invalid) {
			this.reloadImage()
			return
		}
		// nếu chưa đăng nhập cho lưu tạm
		if(!isLogin){
			this.storageService.setRecommentdationObjectRemember(JSON.stringify(this.model))
			this.toastr.error('Vui lòng đăng nhập để gửi phản ánh kiến nghị')
			return
		}
		this.model.status = status
		this.model.sendId = this.storageService.getUserId()
		this.model.sendDate = new Date()
		this.model.typeObject = this.storageService.getTypeObject() == 2 ? 1 : 2
		this.model.name = this.storageService.getFullName()
		const request = {
			Data: this.model,
			Hashtags: this.lstHashtagSelected,
			Files: this.files,
			LstXoaFile: this.lstXoaFile,
		}
		var constdata = {
			CaptchaCode: this.captchaCode,
		}

		this.captchaService.send(constdata).subscribe((result) => {
			if (result.success === RESPONSE_STATUS.success) {
				if (this.model.id == 0) {
					this.recommendationService.recommendationInsert(request).subscribe((response) => {
						if (response.success == RESPONSE_STATUS.success) {
							this.notificationService.insertNotificationTypeRecommendation({ recommendationId: response.result }).subscribe((res) => {})
							this.toastr.success(COMMONS.ADD_SUCCESS)
							localStorage.removeItem('recommentdationObjRemember')
							return this.router.navigate(['/cong-bo/phan-anh-kien-nghi'])
						} else {
							this.toastr.error(response.message)
						}
					}),
						(err) => {
							console.error(err)
							this.reloadImage()
						}
				} else {
					this.recommendationService.recommendationUpdate(request).subscribe((response) => {
						if (response.success == RESPONSE_STATUS.success) {
							this.toastr.success(COMMONS.UPDATE_SUCCESS)
							return this.router.navigate(['/cong-bo/phan-anh-kien-nghi'])
						} else {
							this.toastr.error(response.message)
						}
					}),
						(err) => {
							console.error(err)
						}
				}
			} else {
				this.toastr.error('Mã xác thực không chính xác!')
				this.captchaCode = ''
				this.reloadImage()
			}
		})
	}

	reloadImage() {
		this.captchaImage = AppSettings.API_ADDRESS + Api.getImageCaptcha + '?' + Math.random() * 100000000000000000000
	}

	reloadForm() {
		this.submitted = false
		this.model = new RecommendationObject()
		this.captchaCode = null
		this.form.reset()
	}
	hightLightText() {
		if (this.model.content != null && this.model.content != '' && this.model.content.trim() != '') {
			let content = ''
			content = this.model.content.replace(/\\n/g, String.fromCharCode(13, 10))
			for (let index = 0; index < this.lstDictionariesWord.length; index++) {
				var nameWord = new RegExp(this.lstDictionariesWord[index].name, 'i')
				content = content.replace(
					nameWord,
					'<span class="txthighlight" title="' + this.lstDictionariesWord[index].description + '">' + this.lstDictionariesWord[index].name + '</span>'
				)
			}
			$('#contentRecommendation').addClass('show')
			document.getElementById('contentRecommendation').style.height = document.getElementById('inputContent').style.height
			$('#contentRecommendation').html(content)
		}
	}
	showEditContent() {
		$('#contentRecommendation').removeClass('show')
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

class ItemResultsRecommendation {
	title: string
}
