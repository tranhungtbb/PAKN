import { Component, ElementRef, OnInit, ViewChild, HostListener } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { MouseEvent } from '@agm/core'

import { COMMONS } from 'src/app/commons/commons'
import { CONSTANTS, FILETYPE, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
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
import { LocationService } from 'src/app/services/location.service'

declare var $: any

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
	isLogin: any

	// map
	markers: any = null
	private geoCoder
	zoom: number = 15

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
		private notificationService: NotificationService,
		private eRef: ElementRef,
		private locationService: LocationService
	) {}

	ngOnInit() {
		this.model = new RecommendationObject()
		this.model.typeObject = this.storageService.getTypeObject()
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
		this.isLogin = this.storageService.getAccessToken()
		$('[data-toggle="tooltip"]').tooltip()

		// this.locationService
		// 	.getPosition()
		// 	.then((res) => {
		// 		this.markers = { ...res }
		// 	})
		// 	.catch((err) => {
		// 		console.log(err)
		// 	})
		this.setCurrentLocation()
	}

	private setCurrentLocation() {
		if ('geolocation' in navigator) {
			navigator.geolocation.getCurrentPosition((position) => {
				this.markers = { lat: position.coords.latitude, lng: position.coords.longitude }
			})
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
									recommens.title = recommens.title.replaceAll(element, '<span class ="txthighlight">' + element + '</span>')
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
				this.getListUnitDefault()
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
					this.lstField = response.result.lstField
					this.lstHashtag = response.result.lstHashTag
					this.lstBusiness = response.result.lstBusiness
					this.lstIndividual = response.result.lstIndividual
					this.lstObject = response.result.lstIndividual
					this.model.code = response.result.code
					// this.lstUnit = response.result.lstUnit
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
	}

	getListUnit() {
		this.lstUnit = []
		this.model.unitId = null
		let obj = {
			FieldId: this.model.field == null ? '' : this.model.field,
		}
		this.unitService.getChildrenDropdownByField(obj).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				this.lstUnit = res.result
				if (this.lstUnit.length === 1) {
					this.model.unitId = this.lstUnit[0].value
				}
			} else {
				this.lstUnit = []
			}
		})
	}

	getListUnitDefault = () => {
		let obj = {
			FieldId: this.model.field == null ? '' : this.model.field,
		}
		this.unitService.getChildrenDropdownByField(obj).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				this.lstUnit = res.result
			} else {
				this.lstUnit = []
			}
		})
	}

	builForm() {
		this.form = new FormGroup({
			title: new FormControl(this.model.title, [Validators.required]),
			content: new FormControl(this.model.content, [Validators.required]),
			field: new FormControl(this.model.field, [Validators.required]),
			unitId: new FormControl(this.model.unitId, [Validators.required]),
			hashtag: new FormControl(this.hashtagId),
			captcha: new FormControl(this.captchaCode, [Validators.required]),
			address: new FormControl(this.model.address),
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

	async onSave(status) {
		this.submitted = true
		this.model.content = this.model.content.trim()
		this.model.title = this.model.title.trim()
		if (this.model.content == null || this.model.content == '') {
			return
		}
		if (this.model.title == null || this.model.title == '') {
			return
		}
		this.model.address = this.model.address == null ? '' : this.model.address.trim()
		// if (!this.model.address) {
		// 	await this.getAddress(this.markers.lat, this.markers.lng).then((res) => {
		// 		this.model.address = String(res)
		// 	})
		// }

		if (this.form.invalid) {
			this.reloadImage()
			return
		}
		// nếu chưa đăng nhập cho lưu tạm
		if (!this.isLogin || (this.isLogin && this.isLogin.trim() == '')) {
			this.storageService.setRecommentdationObjectRemember(JSON.stringify(this.model))
			this.toastr.error('Vui lòng đăng nhập để gửi phản ánh kiến nghị')
			return
		}
		this.model.status = status
		this.model.sendId = this.storageService.getUserId()
		this.model.sendDate = new Date()
		this.model.typeObject = this.storageService.getTypeObject() //  == 2 ? 1 : 2
		this.model.name = this.storageService.getFullName()

		if (this.model.typeObject == 1) {
			this.toastr.error('Vui lòng đăng nhập tài khoản người dân, doanh nghiệp để gửi Phản ánh, Kiến nghị')
			return
		}

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
							return this.router.navigate(['/cong-bo/phan-anh-kien-nghi-cua-toi'])
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
							this.toastr.success(COMMONS.ADD_SUCCESS)
							return this.router.navigate(['/cong-bo/phan-anh-kien-nghi-cua-toi'])
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
		this.captchaImage = AppSettings.API_ADDRESS + Api.getImageCaptcha + '?IpAddress=' + this.storageService.getIpAddress() + '&&Ramdom' + Math.random() * 100000000000000000000
	}

	reloadForm() {
		this.submitted = false
		this.model = { ...new RecommendationObject(), code: this.model.code }
		this.model.typeObject = this.storageService.getTypeObject()
		this.captchaCode = null
		$('#_unitId .ng-input input').val('')
		this.showEditContent()
		this.resultsRecommendation = []
		this.form.reset({
			title: this.model.title,
			content: this.model.content,
			field: this.model.field,
			unitId: this.model.unitId,
			hashtag: this.hashtagId,
			captcha: this.captchaCode,
		})
		$('#contentRecommendation').html()
	}
	hightLightText() {
		if (this.model.content != null && this.model.content != '' && this.model.content.trim() != '') {
			let content = this.model.content //.replace(/\\n/g, String.fromCharCode(13, 10))
			for (let index = 0; index < this.lstDictionariesWord.length; index++) {
				var nameWord = new RegExp(this.lstDictionariesWord[index].name, 'ig')
				content = content.replace(
					nameWord,
					'<span class="txthighlight wrapper" data-toggle="tooltip" data-placement="top" title="' +
						this.lstDictionariesWord[index].description +
						'">' +
						this.lstDictionariesWord[index].name +
						'</span>'
				)
			}
			$('#contentRecommendation').addClass('show')
			document.getElementById('contentRecommendation').style.height = document.getElementById('inputContent').style.height
			$('#contentRecommendation').html(content)
		}
		$('[data-toggle="tooltip"]').tooltip()
	}
	showEditContent() {
		$('#contentRecommendation').removeClass('show')
		$('#inputContent').focus()
	}

	showMaps() {
		console.log('markers :' + this.markers)
		$('#modalMaps').modal('show')
	}

	async onSaveMaps() {
		if (this.markers == null || this.markers.lat == null) {
			return this.toastr.error('Vui lòng chọn vị trí')
		} else {
			this.model.latitude = this.markers.lat
			this.model.longitude = this.markers.lng
			await this.getAddress(this.model.latitude, this.model.longitude).then((res) => {
				this.model.address = String(res)
			})
			$('#modalMaps').modal('hide')
			$('#modal').modal('show')
		}
	}
	async getAddress(latitude, longitude) {
		this.geoCoder = new google.maps.Geocoder()
		return new Promise((resolve, reject) => {
			this.geoCoder.geocode({ location: { lat: latitude, lng: longitude } }, (results, status) => {
				if (status === 'OK') {
					if (results[0]) {
						resolve(results[0].formatted_address)
					} else {
						window.alert('No results found')
						reject('No results found')
					}
				} else {
					window.alert('Geocoder failed due to: ' + status)
					reject('Geocoder failed due to: ' + status)
				}
			})
		})
	}
	mapClicked($event: MouseEvent) {
		this.markers = {}
		this.markers.lat = $event.coords.lat
		this.markers.lng = $event.coords.lng
		//console.log('clicked', $event)
	}
	markerDragEnd(m: any, $event: MouseEvent) {
		console.log('dragEnd', m, $event)
	}
}

class ItemResultsRecommendation {
	title: string
}
