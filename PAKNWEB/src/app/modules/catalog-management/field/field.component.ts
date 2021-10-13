import { Component, OnInit, ViewChild, AfterViewInit, Input } from '@angular/core'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'
import { FieldObject } from 'src/app/models/fieldObject'
import { CatalogService } from 'src/app/services/catalog.service'
import { DataService } from 'src/app/services/sharedata.service'
import { saveAs as importedSaveAs } from 'file-saver'
import { MESSAGE_COMMON, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'

// import { RemindComponent } from 'src/app/modules/recommendation/remind/remind.component'

declare var $: any

@Component({
	selector: 'app-field',
	templateUrl: './field.component.html',
	styleUrls: ['./field.component.css'],
})
export class FieldComponent implements OnInit, AfterViewInit {
	constructor(private _service: CatalogService, private _toastr: ToastrService, private _fb: FormBuilder, private _shareData: DataService) {}

	// child

	// @ViewChild(RemindComponent, { static: false }) remindComponent: RemindComponent

	listData = new Array<FieldObject>()
	listStatus: any = [
		{ value: true, text: 'Hiệu lực' },
		{ value: false, text: 'Hết hiệu lực' },
	]
	form: FormGroup
	model: any = new FieldObject()
	submitted: boolean = false
	title: string = ''

	@ViewChild('table', { static: false }) table: any
	totalRecords: number = 0
	idDelete: number = 0
	ngOnInit() {
		this.buildForm()
		this.getList()
	}

	ngAfterViewInit() {
		this._shareData.seteventnotificationDropdown()
		// $('#modal').on('keypress', function(e) {
		// 	if (e.which == 13) e.preventDefault()
		// })
	}

	get f() {
		return this.form.controls
	}

	buildForm() {
		this.form = this._fb.group({
			name: [this.model.name, Validators.required],
			description: [this.model.description],
			isActived: [this.model.isActived, Validators.required],
		})
	}

	rebuilForm() {
		this.form.reset({
			name: this.model.name,
			isActived: this.model.isActived,
			description: this.model.description,
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
		this.rebuilForm()
		this.submitted = false
		this.title = 'Thêm mới lĩnh vực'
		$('#modal').modal('show')
	}

	onSave() {
		this.submitted = true
		this.model.name = this.model.name.trim()
		if (this.model.name == '') return
		if (this.form.invalid) {
			return
		}
		if (this.model.id == 0 || this.model.id == null) {
			this._service.fieldInsert(this.model).subscribe((response) => {
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
			this._service.fieldUpdate(this.model).subscribe((response) => {
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
		let request = {
			Id: data.id,
			Type: 1,
		}
		this._service.fieldGetById(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				this.rebuilForm()
				this.title = 'Chỉnh sửa lĩnh vực'
				this.model = response.result.CAFieldGetByID[0]
				$('#modal').modal('show')
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
}
