import { Component, ElementRef, OnInit, ViewChild } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { COMMONS } from 'src/app/commons/commons'
import { CONSTANTS, FILETYPE, RECOMMENDATION_STATUS, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { RecommendationObject } from 'src/app/models/recommendationObject'
import { UploadFileService } from 'src/app/services/uploadfiles.service'
import { RecommendationService } from 'src/app/services/recommendation.service'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { ActivatedRoute, Router } from '@angular/router'

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
	lstUnit: any[] = []
	lstField: any[] = []
	lstBusiness: any[] = []
	lstIndividual: any[] = []
	lstObject: any[] = []
	lstHashTag: any[] = []
	txtHashtag: string = ''
	fileAccept = CONSTANTS.FILEACCEPT
	files: any[] = []
	lstXoaFile: any[] = []
	submitted: boolean = false

	@ViewChild('file', { static: false }) public file: ElementRef
	constructor(
		private toastr: ToastrService,
		private fileService: UploadFileService,
		private recommendationService: RecommendationService,
		private router: Router,
		private activatedRoute: ActivatedRoute
	) {}

	ngOnInit() {
		this.getDropdown()
		this.activatedRoute.params.subscribe((params) => {
			this.model = new RecommendationObject()
			this.model.id = +params['id']
			if (this.model.id != 0) {
				this.getData()
			} else {
				this.model.typeObject = true
			}
			this.builForm()
		})
	}

	onAddHashtag() {
		this.lstHashTag.forEach((element) => {
			if (element.text == this.txtHashtag.trim()) {
				this.toastr.error('Từ khóa đã tồn tại')
				return
			}
		})
		this.lstHashTag.push({ text: this.txtHashtag.trim() })
		this.txtHashtag = ''
	}
	onRemoveHashtag(item: any) {
		this.lstHashTag.forEach((element) => {
			if (element.text == item.text) {
				this.lstHashTag.splice(element)
				return
			}
		})
	}

	getData() {
		let request = {
			Id: this.model.id,
		}
		this.recommendationService.recommendationGetById(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				this.model = response.result.data
				this.files = response.result.files

				if (this.model.sendDate) {
					this.model.sendDate = new Date(this.model.sendDate)
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
	getDropdown() {
		let request = {}
		this.recommendationService.recommendationGetDataForCreate(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				this.lstUnit = response.result.lstUnit
				this.lstField = response.result.lstField
				this.lstBusiness = response.result.lstBusiness
				this.lstIndividual = response.result.lstIndividual
				this.lstObject = response.result.lstIndividual
			} else {
				this.toastr.error(response.message)
			}
		}),
			(error) => {
				console.log(error)
				alert(error)
			}
	}

	changeTypeObject(typeObject: boolean) {
		this.model.sendId = null
		if (typeObject) {
			this.titleObject = 'Cá nhân'
			this.lstObject = this.lstIndividual
		} else {
			this.titleObject = 'Doanh nghiệp'
			this.lstObject = this.lstBusiness
		}
	}

	builForm() {
		this.form = new FormGroup({
			code: new FormControl(this.model.code, [Validators.required]),
			title: new FormControl(this.model.title, [Validators.required]),
			content: new FormControl(this.model.content, [Validators.required]),
			field: new FormControl(this.model.field, [Validators.required]),
			unitId: new FormControl(this.model.unitId, [Validators.required]),
			sendId: new FormControl(this.model.sendId, [Validators.required]),
			sendDate: new FormControl(this.model.sendDate, [Validators.required]),
			hashtag: new FormControl(this.txtHashtag),
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

		this.submitted = true
		if (this.form.invalid) {
			return
		}
		this.model.status = RECOMMENDATION_STATUS.RECEIVE_APPROVED
		const request = {
			Data: this.model,
			Files: this.files,
			LstXoaFile: this.lstXoaFile,
		}
		if (this.model.id == 0) {
			this.recommendationService.recommendationInsert(request).subscribe((response) => {
				if (response.success == RESPONSE_STATUS.success) {
					this.toastr.success(COMMONS.ADD_SUCCESS)
					//	return this.router.navigate(['/business/business-management/letter/view/', this.model.idDonThu])
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
					//return this.router.navigate(['/business/business-management/letter/view/', this.model.idDonThu])
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
