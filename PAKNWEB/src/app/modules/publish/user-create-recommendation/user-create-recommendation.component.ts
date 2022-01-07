import { Component, ElementRef, OnInit, ViewChild, AfterViewInit } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { MouseEvent } from '@agm/core'

import { COMMONS } from 'src/app/commons/commons'
import { CONSTANTS, FILETYPE, RECEPTION_TYPE, RESPONSE_STATUS, TYPE_RECOMMENDATION } from 'src/app/constants/CONSTANTS'
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
import { LocationService } from 'src/app/services/location.service'

declare var $: any

@Component({
	selector: 'app-user-create-recommendation',
	templateUrl: './user-create-recommendation.component.html',
	styleUrls: ['./user-create-recommendation.component.css'],
})
export class CreateRecommendationComponent implements OnInit, AfterViewInit {
	form: FormGroup
	model: RecommendationObject = new RecommendationObject()
	lstUnit: any[] = []
	lstField: any[] = []
	lstBusiness: any[] = []
	lstIndividual: any[] = []
	lstObject: any[] = []
	lstHashtag: any[] = []
	lstGroupUnit: any[] = []
	lstHashtagSelected: any[] = []
	hashtagId: number = null
	fileAccept = CONSTANTS.FILEACCEPT
	files: any[] = []
	lstXoaFile: any[] = []
	submitted: boolean = false
	modelHashTagAdd: HashtagObject = new HashtagObject()
	dateNow: Date = new Date()
	@ViewChild('file', { static: false }) public file: ElementRef
	captchaImage: any = ''
	captchaCode: string = null
	resultsRecommendation: any = []
	lstDictionariesWord: any = []
	flagRecommend: boolean;
	unitSelected: any = { name: null, id: null }
	lstUnitChild: any[] = []
	isLogin: any
	flagInputContent: boolean = false;
	// map
	markers: any = {}
	private geoCoder
	zoom: number = 15
	milliseconds = new Date().getTime();
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

	) { }

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

	}

	ngAfterViewInit() {
		this.setCurrentLocation()
		$('[data-toggle="tooltip"]').tooltip()
	}

	setCurrentLocation() {
		if ('geolocation' in navigator) {
			navigator.geolocation.getCurrentPosition((position) => {
				this.markers = { lat: position.coords.latitude, lng: position.coords.longitude }
				this.model.lat = this.markers.lat
				this.model.lng = this.markers.lng
				setTimeout(() => {
					this.getAddress(this.model.lat, this.model.lng).then((res) => {
						this.model.address = String(res)
					})
				}, 200)

			})
		}
	}
	inputTitleFocus() {
		this.flagInputContent = false;
	}
	onFocusContent() {
		this.flagInputContent = true;
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
					if (this.flagInputContent == true) {
						this.flagRecommend = false;
					}
					else {
						this.flagRecommend = true;
					}
					this.flagInputContent = false;
				} else {
					this.toastr.error(response.message)
				}
			}),
				(error) => {
					console.log(error)
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
				if (!this.model.lat || !this.model.lng) {
					this.setCurrentLocation()
				} else {
					this.markers.lat = Number(this.model.lat)
					this.markers.lng = Number(this.model.lng)
				}
				this.getListUnitDefault()
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
					this.lstGroupUnit = response.result.lstGroupUnit
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

	getListUnitChild() {
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
			field: new FormControl(this.model.field),
			unitId: new FormControl(this.model.unitId),
			hashtag: new FormControl(this.hashtagId),
			captcha: new FormControl(this.captchaCode, [Validators.required]),
			address: new FormControl(this.model.address, [Validators.required]),
			groupUnitId: new FormControl(this.model.address, [Validators.required]),
		})
	}
	get f() {
		return this.form.controls
	}
	isShowUnitChild: boolean = false
	isChooseUnit: boolean = false
	onChangeGroup(event) {
		this.isChooseUnit = event.isMain == null ? false : true
		this.model.unitReceive = null
		this.model.unitChildId = null
		if (!event.isMain) {
			this.getUnitByGroup(event.id)
		}
		if (event.isAdministrative) {
			this.isShowUnitChild = true
		} else {
			this.isShowUnitChild = false
		}
		this.lstUnit = []
	}
	getUnitByGroup(groupId: number) {
		this.unitService.getByGroup({ GroupId: groupId }).subscribe(res => {
			if (res.success == RESPONSE_STATUS.success) {
				this.lstUnit = res.result
			} else {
				this.toastr.error(res.message)
			}
		})
	}

	getUnitByParent(event) {
		this.model.unitChildId = null
		if (event) {
			this.unitService.getByParent({ ParentId: event.id }).subscribe(res => {
				if (res.success == RESPONSE_STATUS.success) {
					this.lstUnitChild = res.result
				} else {
					this.toastr.error(res.message)
				}
			})
		} else {
			this.lstUnitChild = []
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
						if (fileType.value === 4) {
							item.filePathUrl = URL.createObjectURL(item)
						}
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
	isSuitableContent: boolean = true
	onSave(status) {
		this.isSuitableContent = true
		this.submitted = true
		if (this.isChooseUnit) {
			this.model.unitId = this.model.groupUnitId
		} else {
			if (this.isShowUnitChild && this.model.unitChildId) {
				this.model.unitId = this.model.unitChildId
			} else {
				if (!this.model.unitReceive) {
					return
				}
				this.model.unitId = this.model.unitReceive
			}
		}

		this.model.content = this.model.content.trim()
		this.model.title = this.model.title.trim()
		if (this.model.content == null || this.model.content == '') {
			return
		}
		if (this.model.title == null || this.model.title == '') {
			return
		}
		this.model.address = this.model.address == null ? '' : this.model.address.trim()
		if (!this.checkContent()) {
			this.isSuitableContent = false
			return
		}
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
		this.model.lat = this.markers.lat
		this.model.lng = this.markers.lng
		this.model.type = TYPE_RECOMMENDATION.Socioeconomic
		this.model.receptionType = RECEPTION_TYPE.Web
		this.model.isFakeImage = false

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
			MillisecondsCurrent: this.milliseconds
		}

		this.captchaService.send(constdata).subscribe((result) => {
			if (result.success === RESPONSE_STATUS.success) {
				if (this.model.id == 0) {
					this.recommendationService.recommendationInsert(request).subscribe((response) => {
						if (response.success == RESPONSE_STATUS.success) {
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
							this.toastr.success(COMMONS.UPDATE_SUCCESS)
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
		this.milliseconds = new Date().getTime();
		this.captchaImage = AppSettings.API_ADDRESS + Api.getImageCaptcha + '?IpAddress=' + this.storageService.getIpAddress() + '&&Ramdom' + Math.random() * 100000000000000000000 + '&&MillisecondsCurrent=' + this.milliseconds
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
				console.log(nameWord)
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
		this.hideRecommendBox();
	}


	checkContent() {
		for (let index = 0; index < this.lstDictionariesWord.length; index++) {
			if (this.model.content.toUpperCase().indexOf(this.lstDictionariesWord[index].name.toUpperCase()) != -1) {
				return false
			}
		}
		return true
	}

	showEditContent() {
		$('#contentRecommendation').removeClass('show')
		$('#inputContent').focus()
	}

	showMaps() {
		console.log('markers :' + this.markers)
		$('#modalMaps').modal('show')
	}

	closeMap() {
		$('#modalMaps').modal('hide')
	}
	getCurrentMark() {
		$('#modalMaps').modal('show');
		console.log('markers :' + this.markers)


	}
	async onSaveMaps() {
		if (this.markers == null || this.markers.lat == null) {
			return this.toastr.error('Vui lòng chọn vị trí')
		} else {
			this.model.lat = this.markers.lat
			this.model.lng = this.markers.lng
			this.getAddress(this.model.lat, this.model.lng).then((res) => {
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
	}
	dragEnd(event): void {
		this.markers = {}
		this.markers.lat = event.coords.lat
		this.markers.lng = event.coords.lng

	}
	closeModalMap() {
		$('#modalMaps').modal('hide')
	}
	hideRecommendBox() {
		this.flagRecommend = false;
	}
}

class ItemResultsRecommendation {
	title: string
}
