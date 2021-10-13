import { Component, ElementRef, OnInit, ViewChild } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { COMMONS } from 'src/app/commons/commons'
import { CONSTANTS, FILETYPE, RECOMMENDATION_STATUS, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { UploadFileService } from 'src/app/services/uploadfiles.service'
import { RecommendationService } from 'src/app/services/recommendation.service'
import { FormControl, FormGroup, Validators, FormBuilder } from '@angular/forms'
import { ActivatedRoute, Router } from '@angular/router'
import { CatalogService } from 'src/app/services/catalog.service'
import { AdministrativeFormalitiesObject, AdministrativeFormalitiesForward } from 'src/app/models/AdministrativeFormalitiesObject'
import { AdministrativeFormalitiesService } from 'src/app/services/administrative-formalities.service'
import { UnitService } from 'src/app/services/unit.service'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
declare var $: any

@Component({
	selector: 'app-detail-administrative-formalities',
	templateUrl: './detail-administrative-formalities.component.html',
	styleUrls: ['./detail-administrative-formalities.component.css'],
})
export class DetailAdministrativeFormalitiesComponent implements OnInit {
	form: FormGroup
	formForward: FormGroup
	model: AdministrativeFormalitiesObject = new AdministrativeFormalitiesObject()
	modelForward: AdministrativeFormalitiesForward = new AdministrativeFormalitiesForward()
	titleObject: string = 'Cá nhân'
	lstUnitDAM: any[] = []
	lstUnit: any[] = []
	lstUnitForward: any = new Array<itemSelected>()
	lstUnitForwardBase: any = new Array<itemSelected>()
	lstField: any[] = []
	lstBusiness: any[] = []
	lstIndividual: any[] = []
	lstObject: any[] = []
	fileAccept = CONSTANTS.FILEACCEPT
	fileAcceptForm = CONSTANTS.FILEACCEPT_FORM_ADMINISTRATION
	files: any[] = []
	lstXoaFile: any[] = []
	lstXoaFileForm: any[] = []
	submitted: boolean = false
	lstTypeSend: any[] = [
		{ value: null, text: '-- Chọn mức độ trực tuyến --' },
		{ value: true, text: 'Nộp trực tuyến' },
		{ value: false, text: 'Nộp qua mạng' },
	]
	lstBind: any[] = [
		{ value: null, text: '-- Chọn bắt buộc --' },
		{ value: true, text: 'Có' },
		{ value: false, text: 'Không' },
	]
	@ViewChild('file', { static: false }) public file: ElementRef

	lstCompositionProfile: any[] = []
	lstCharges: any[] = []
	lstImplementationProcess: any[] = []

	typeDelete: number
	itemDelete: any
	lstDelete: any[] = [] //list id , type tp hồ sơ, trình tự thực hiện, lệ phí
	constructor(
		private toastr: ToastrService,
		private fileService: UploadFileService,
		private afService: AdministrativeFormalitiesService,
		private recommendationService: RecommendationService,
		private router: Router,
		private activatedRoute: ActivatedRoute,
		private _fb: FormBuilder,
		private _service: UnitService,
		private stogateService: UserInfoStorageService
	) {}

	ngOnInit() {
		this.model = new AdministrativeFormalitiesObject()
		this.getDropdown()
		this.activatedRoute.params.subscribe((params) => {
			this.model.administrationId = params['id']
			if (this.model.administrationId != 0) {
				this.getData()
			}
			this.builForm()
			this._service.getChildrenDropdown().subscribe((response) => {
				if (response.success == RESPONSE_STATUS.success) {
					if (response.result != null) {
						let arrUnit = response.result.filter((x) => x.unitId != this.stogateService.getUnitId())
						this.lstUnitForward = arrUnit.map((item) => {
							return { text: item.unitName, value: item.unitId }
						})
						this.lstUnitForwardBase = [...this.lstUnitForward]
					}
				} else {
					this.toastr.error(response.message)
				}
			}),
				(error) => {
					console.log(error)
				}
		})
	}

	getData() {
		let request = {
			Id: this.model.administrationId,
		}
		this.afService.getByAdmintrativeId(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				this.model = response.result.data
				this.files = response.result.files
				this.lstCharges = response.result.lstCharges
				this.lstCompositionProfile = response.result.lstCompositionProfile
				this.lstImplementationProcess = response.result.lstImplementationProcess
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
				this.lstUnit = response.result.lstUnit
				this.lstField = response.result.lstField
				var defaultValueUnit = {
					text: '-- Chọn đơn vị tiếp nhận --',
					value: null,
				}
				var defaultValueField = {
					text: '-- Chọn lĩnh vực --',
					value: null,
				}
				this.lstUnit.unshift(defaultValueUnit)
				this.lstField.unshift(defaultValueField)
			} else {
				this.toastr.error(response.message)
			}
		}),
			(error) => {
				console.log(error)
			}
	}

	builForm() {
		this.model = new AdministrativeFormalitiesObject()
		this.form = new FormGroup({
			// code: new FormControl(this.model.code, [Validators.required]),
			name: new FormControl(this.model.name, [Validators.required]),
			rankReceive: new FormControl(this.model.rankReceive, [Validators.required]),
			unitReceive: new FormControl(this.model.unitReceive, [Validators.required]),
			field: new FormControl(this.model.field, [Validators.required]),
			typeSend: new FormControl(null, [Validators.required]),
			fileNum: new FormControl(this.model.fileNum, [Validators.required]),
			amountTime: new FormControl(this.model.amountTime, [Validators.required]),
			proceed: new FormControl(this.model.proceed, [Validators.required]),
			object: new FormControl(this.model.object, [Validators.required]),
			organization: new FormControl(this.model.organization, [Validators.required]),
			organizationDecision: new FormControl(this.model.organizationDecision),
			organizationAuthor: new FormControl(this.model.organizationAuthor),
			organizationCombine: new FormControl(this.model.organizationCombine),
			address: new FormControl(this.model.address),
			result: new FormControl(this.model.result),
			legalGrounds: new FormControl(this.model.legalGrounds),
			request: new FormControl(this.model.request),
			impactAssessment: new FormControl(this.model.impactAssessment),
			note: new FormControl(this.model.note),
		})
	}

	onChangeLstItemUnit() {
		console.log(this.lstUnitForward)
		this.lstUnitForward = this.lstUnitForwardBase.filter((item, index) => {
			let check = this.modelForward.lstUnitId.find((x) => x == item.value)
			if (check) {
				return
			}
			return item
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

	onUploadForm(event, it) {
		if (event.target.files.length == 0) {
			return
		}
		const check = this.fileService.checkFileWasExitsted(event, it.files)
		if (check === 1) {
			for (let item of event.target.files) {
				FILETYPE.forEach((fileType) => {
					if (item.type == fileType.text) {
						item.fileType = fileType.value
						it.files.push(item)
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
		it.file.nativeElement.value = ''
	}

	onRemoveFileForm(args, item) {
		const index = item.files.indexOf(args)
		const file = item.files[index]
		this.lstXoaFileForm.push(file)
		item.files.splice(index, 1)
	}

	onSave(status) {
		this.submitted = true
		this.model.status = status
		const request = {
			Id: this.model.administrationId,
			Status: this.model.status,
		}
		if (this.model.id != 0) {
			this.afService.updateShow(request).subscribe((response) => {
				if (response.success == RESPONSE_STATUS.success) {
					this.toastr.success(COMMONS.UPDATE_SUCCESS)
					// return this.router.navigate(['/quan-tri/thu-tuc-hanh-chinh'])
				} else {
					this.toastr.error(response.message)
				}
			}),
				(err) => {
					console.error(err)
				}
		}
	}

	// forward
	preForward = () => {
		this.modelForward = new AdministrativeFormalitiesForward()
		this.submitted = false
		$('#modal-forward').modal('show')
	}
	onForward = () => {
		this.modelForward.content = this.modelForward.content.trim()
		if (this.modelForward.lstUnitId.length < 0) {
			this.toastr.error('Vui lòng chọn cơ quan tiếp nhận')
			return
		}
		this.submitted = true
		let lstUnit = this.modelForward.lstUnitId.reduce((x, y) => {
			return (x += y + ',')
		}, '')
		let obj = {
			LstUnitId: lstUnit.substring(0, lstUnit.length - 1),
			AdministrationId: this.model.administrationId,
			Content: this.modelForward.content,
		}
		this.afService.forward(obj, this.model.name).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				this.toastr.success('Gửi cơ quan, đơn vị thành công')
				$('#modal-forward').modal('hide')
			} else {
				this.toastr.error('Lỗi khi gửi cơ quan, đơn vị')
			}
		}),
			(err) => {
				console.log(err)
			}
	}

	onAddCompositionProfile() {
		var cp = {
			index: this.lstCompositionProfile.length + 1,
			nameExhibit: '',
			originalForm: '',
			copyForm: '',
			form: '',
			isBind: null,
			files: [],
		}
		this.lstCompositionProfile.push(cp)
	}

	onAddCharges() {
		var cp = {
			index: this.lstCharges.length + 1,
			charges: '',
			description: '',
		}
		this.lstCharges.push(cp)
	}

	onAddImplementationProcess() {
		var cp = {
			index: this.lstImplementationProcess.length + 1,
			name: '',
			unit: '',
			time: '',
			result: '',
		}
		this.lstImplementationProcess.push(cp)
	}
	preDelete(item, type) {
		this.itemDelete = item
		this.typeDelete = type
		$('#modalDelete').modal('show')
	}

	onDelete() {
		if (this.typeDelete == 1) {
			this.onRemoveCompositionProfile()
		} else if (this.typeDelete == 2) {
			this.onRemoveImplementationProcess()
		} else if (this.typeDelete == 3) {
			this.onRemoveCharges()
		}
		this.itemDelete = null
		this.typeDelete = null
		$('#modalDelete').modal('hide')
	}
	onRemoveCompositionProfile() {
		var index = this.lstCompositionProfile.indexOf(this.itemDelete)
		var idDelete = {
			id: this.itemDelete.id,
			type: this.typeDelete,
		}
		this.lstCompositionProfile.splice(index, 1)
		this.lstDelete.push(idDelete)
	}

	onRemoveCharges() {
		var index = this.lstCharges.indexOf(this.itemDelete)
		var idDelete = {
			id: this.itemDelete.id,
			type: this.typeDelete,
		}
		this.lstCharges.splice(index, 1)
		this.lstDelete.push(idDelete)
	}

	onRemoveImplementationProcess() {
		var index = this.lstImplementationProcess.indexOf(this.itemDelete)
		var idDelete = {
			id: this.itemDelete.id,
			type: this.typeDelete,
		}
		this.lstImplementationProcess.splice(index, 1)
		this.lstDelete.push(idDelete)
	}

	public getFileBin(path: string) {
		this.fileService.downloadFile({ path }).subscribe((res) => {
			console.log(res)
		})
	}
}
class itemSelected {
	text: string
	value: number
}
