import { Component, ElementRef, OnInit, ViewChild } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { COMMONS } from 'src/app/commons/commons'
import { CONSTANTS, FILETYPE, RECOMMENDATION_STATUS, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { UploadFileService } from 'src/app/services/uploadfiles.service'
import { RecommendationService } from 'src/app/services/recommendation.service'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { ActivatedRoute, Router } from '@angular/router'
import { CatalogService } from 'src/app/services/catalog.service'
import { AdministrativeFormalitiesObject } from 'src/app/models/AdministrativeFormalitiesObject'
declare var $: any

@Component({
	selector: 'app-cu-administrative-formalities',
	templateUrl: './cu-administrative-formalities.component.html',
	styleUrls: ['./cu-administrative-formalities.component.css'],
})
export class CU_AdministrativeFormalitiesComponent implements OnInit {
	form: FormGroup;
	model: AdministrativeFormalitiesObject = new AdministrativeFormalitiesObject();
	titleObject: string = 'Cá nhân';

	lstUnit: any[] = [];
	lstField: any[] = [];
	lstBusiness: any[] = [];
	lstIndividual: any[] = [];
	lstObject: any[] = [];
	lstHashtag: any[] = [];
	lstHashtagSelected: any[] = [];
	hashtagId: number = null;
	fileAccept = CONSTANTS.FILEACCEPT;
	fileAcceptForm = CONSTANTS.FILEACCEPT_FORM_ADMINISTRATION;
	files: any[] = [];
	lstXoaFile: any[] = [];
	lstXoaFileForm: any[] = [];
	submitted: boolean = false;
	dateNow: Date = new Date();
	lstTypeSend: any[] = [];
	@ViewChild('file', { static: false }) public file: ElementRef;

	lstCompositionProfile: any[] = [];
	lstCharges: any[] = [];
	lstImplementationProcess: any[] = [];
	constructor(
		private toastr: ToastrService,
		private fileService: UploadFileService,
		private recommendationService: RecommendationService,
		private _serviceCatalog: CatalogService,
		private router: Router,
		private activatedRoute: ActivatedRoute
	) { }

	ngOnInit() {
		this.model = new AdministrativeFormalitiesObject()
		this.getDropdown()
		this.activatedRoute.params.subscribe((params) => {
			this.model.id = +params['id']
			if (this.model.id != 0) {
				this.getData()
			}
			this.builForm()
		})
	}

	getData() {
		let request = {
			Id: this.model.id,
		}
		this.recommendationService.recommendationGetById(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				this.model = response.result.model
				this.files = response.result.lstFiles

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


	builForm() {
		this.form = new FormGroup({
			// code: new FormControl(this.model.code, [Validators.required]),
			name: new FormControl(this.model.name, [Validators.required]),
			rankReceive: new FormControl(this.model.rankReceive, [Validators.required]),
			unitReceive: new FormControl(this.model.unitReceive, [Validators.required]),
			field: new FormControl(this.model.field, [Validators.required]),
			typeSend: new FormControl(this.model.typeSend, [Validators.required]),
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
		this.model.status = status;
		this.submitted = true
		if (this.form.invalid) {
			return
		}
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

	onAddCompositionProfile() {
		var cp = {
			nameExhibit: '',
			originalForm: '',
			copyForm: '',
			form: '',
			isBind: false,
			files: []
		}
		this.lstCompositionProfile.push(cp);
	}

	onAddCharges() {
		var cp = {
			charges: '',
			description: '',
		}
		this.lstCharges.push(cp);
	}

	onAddImplementationProcess() {
		var cp = {
			name: '',
			unit: '',
			time: '',
			result: '',
		}
		this.lstImplementationProcess.push(cp);
	}
}
