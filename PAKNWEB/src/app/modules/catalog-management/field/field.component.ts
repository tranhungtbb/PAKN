import { Component, OnInit, ViewChild, AfterViewInit, Input, ElementRef } from '@angular/core'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'
import { FieldObject } from 'src/app/models/fieldObject'
import { CatalogService } from 'src/app/services/catalog.service'
import { RecommendationService } from 'src/app/services/recommendation.service'
import { DataService } from 'src/app/services/sharedata.service'
import { FILETYPE, MESSAGE_COMMON, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { UploadFileService } from 'src/app/services/uploadfiles.service'
import { saveAs as importedSaveAs } from 'file-saver'
// import { RemindComponent } from 'src/app/modules/recommendation/remind/remind.component'

declare var $: any

@Component({
	selector: 'app-field',
	templateUrl: './field.component.html',
	styleUrls: ['./field.component.css'],
})
export class FieldComponent implements OnInit, AfterViewInit {
	constructor(
		private _service: CatalogService,
		private _toastr: ToastrService,
		private _fb: FormBuilder,
		private _shareData: DataService,
		private _serviceR: RecommendationService,
		private fileService: UploadFileService
	) {}

	// child

	// @ViewChild(RemindComponent, { static: false }) remindComponent: RemindComponent

	@ViewChild('file', { static: false }) file: ElementRef
	listData = new Array<FieldObject>()
	listStatus: any = [
		{ value: true, text: 'Hiệu lực' },
		{ value: false, text: 'Hết hiệu lực' },
	]
	listUnit: any = []
	listUnitSelected: any = []
	form: FormGroup
	model: any = new FieldObject()
	submitted: boolean = false
	title: string = ''
	name: string = ''
	description: string = ''
	filePost: any = null
	allowImageExtend = ['image/jpeg', 'image/png']
	@ViewChild('table', { static: false }) table: any
	totalRecords: number = 0
	idDelete: number = 0
	ngOnInit() {
		this.buildForm()
		this.getList()
		this._serviceR.recommendationGetDataForCreate({}).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				this.listUnit = res.result.lstUnit
			} else {
				this.listUnit = []
			}
		})
	}

	ngAfterViewInit() {
		this._shareData.seteventnotificationDropdown()
		$('#modal').on('keypress', function (e) {
			if (e.which == 13) e.preventDefault()
		})
	}

	get f() {
		return this.form.controls
	}

	buildForm() {
		this.form = this._fb.group({
			name: [this.model.name, Validators.required],
			description: [this.model.description],
			isActived: [this.model.isActived, Validators.required],
			orderNumber: [this.model.orderNumber],
			isShowHome: [this.model.isShowHome],
			filePath: [this.model.filePath],
		})
	}

	rebuilForm() {
		this.form.reset({
			name: this.model.name,
			isActived: this.model.isActived,
			description: this.model.description,
			orderNumber: this.model.orderNumber,
			isShowHome: this.model.isShowHome,
			filePath: this.model.filePath,
		})
	}

	getList() {
		this._service.fieldGetList({}).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result != null) {
					this.listData = []
					this.listData = response.result.CAFieldGetAllOnPage
					this.totalRecords = response.result.TotalCount
				}
			} else {
				this._toastr.error(response.message)
			}
		}),
			(error) => {
				console.log(error)
				alert(error)
			}
	}

	confirmChangeStatus(data) {
		this.model = { ...data }
		this.model.isActived = !data.isActived
		$('#modalConfirmChangeStatus').modal('show')
	}

	preCreate() {
		this.model = new FieldObject()
		this.filePost = null
		this.rebuilForm()
		this.listUnitSelected = []
		this.submitted = false
		this.title = 'Thêm mới lĩnh vực'
		$('#modal').modal('show')
		setTimeout(() => {
			$('#target').focus()
		}, 400)
	}

	onSave() {
		this.submitted = true
		this.model.name = this.model.name.trim()
		if (this.model.name == '') return
		if (this.form.invalid) {
			return
		}

		this.model.listUnit = this.listUnitSelected.length == 0 ? null : this.listUnitSelected.join(',')
		let request = {
			model: this.model,
			files: this.filePost
		}
		
		if (this.model.id == 0 || this.model.id == null) {
			this._service.fieldInsert(request).subscribe((response) => {
				if (response.success == RESPONSE_STATUS.success) {
					if (response.result == -1) {
						this._toastr.error(MESSAGE_COMMON.EXISTED_NAME)
						return
					} else {
						$('#modal').modal('hide')
						this._toastr.success(MESSAGE_COMMON.ADD_SUCCESS)
						this.getList()
					}
				} else {
					this._toastr.error(response.message)
				}
			}),
				(error) => {
					console.error(error)
					alert(error)
				}
		} else {
			this._service.fieldUpdate(request).subscribe((response) => {
				if (response.success == RESPONSE_STATUS.success) {
					if (response.result == -1) {
						this._toastr.error(MESSAGE_COMMON.EXISTED_NAME)
						return
					} else {
						$('#modal').modal('hide')
						this._toastr.success(MESSAGE_COMMON.UPDATE_SUCCESS)
						this.getList()
					}
				} else {
					this._toastr.error(response.message)
				}
			}),
				(error) => {
					console.error(error)
					alert(error)
				}
		}
	}

	preUpdate(data) {
		this.filePost = null
		let request = {
			Id: data.id,
			Type: 1,
		}
		this._service.fieldGetById(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				this.rebuilForm()
				this.title = 'Chỉnh sửa lĩnh vực'
				this.model = response.result.CAFieldGetByID[0]
				this.listUnitSelected = this.model.listUnit == null ? [] : this.model.listUnit.split(',').map(Number)
				$('#modal').modal('show')
				setTimeout(() => {
					$('#target').focus()
				}, 400)
			} else {
				this._toastr.error(response.message)
			}
		}),
			(error) => {
				console.error(error)
				alert(error)
			}
	}
	preDelete(id: number) {
		this.idDelete = id
		$('#modalConfirmDelete').modal('show')
	}

	onDelete(id: number) {
		let request = {
			Id: id,
		}
		let obj = this.listData.find((x) => x.id == id)
		this._service.fieldDelete(request, obj.name).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result > 0) {
					this._toastr.success(MESSAGE_COMMON.DELETE_SUCCESS)
				} else {
					this._toastr.error('Lĩnh vực này đang được sử dụng!')
				}
				$('#modalConfirmDelete').modal('hide')
				this.getList()
			} else {
				this._toastr.error(response.message)
			}
		}),
			(error) => {
				console.error(error)
			}
	}

	onUpdateStatus() {
		this._service.fieldUpdateStatus(this.model).subscribe((res) => {
			if (res.success == 'OK') {
				$('#modalConfirmChangeStatus').modal('hide')
				this.getList()
				if (this.model.isActived == true) {
					this._toastr.success(MESSAGE_COMMON.UNLOCK_SUCCESS)
				} else {
					this._toastr.success(MESSAGE_COMMON.LOCK_SUCCESS)
				}
			} else {
				this._toastr.error(res.message)
			}
		}),
			(error) => {
				console.error(error)
			}
	}

	preView(data) {
		this.model = data
		$('#modalDetail').modal('show')
	}

	onChangeImage() {
		$('#avatar-image').click()
	}
	avatarLocalChange = false
	onImageChange(event: any) {
		var file = event.target.files[0]
		if (!file) {
			return
		}
		if (file.size > 3000000) {
			this._toastr.error('Chỉ chọn tệp có dụng lượng nhỏ hơn 3MB')
			return
		}
		if (!this.allowImageExtend.includes(file.type)) {
			this._toastr.error('Chỉ chọn tệp tin hình ảnh')
			return
		}

		//preview image
		this.filePost = file
		this.model.filePath = URL.createObjectURL(file)
		this.avatarLocalChange = true
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
				this._toastr.error('Không tìm thấy file trên hệ thống')
			}
		)
	}

	onRemoveImg(){
		this.model.filePath = null;
	}
}
