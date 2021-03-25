import { Component, OnInit, ViewChild } from '@angular/core'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'
import { FieldObject } from 'src/app/models/fieldObject'
import { CatalogService } from 'src/app/services/catalog.service'
import { DataService } from 'src/app/services/sharedata.service'
import { saveAs as importedSaveAs } from 'file-saver'
import { MESSAGE_COMMON, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'

declare var $: any

@Component({
	selector: 'app-field',
	templateUrl: './field.component.html',
	styleUrls: ['./field.component.css'],
})
export class FieldComponent implements OnInit {
	constructor(private _service: CatalogService, private _toastr: ToastrService, private _fb: FormBuilder, private _shareData: DataService) {}

	listData = new Array<FieldObject>()
	listStatus: any = [
		{ value: true, text: 'Sử dụng' },
		{ value: false, text: 'Không sử dụng' },
	]
	form: FormGroup
	model: any = new FieldObject()
	submitted: boolean = false
	isActived: boolean = true
	title: string = ''
	code: string = ''
	name: string = ''
	pageIndex: number = 1
	pageSize: number = 20
	@ViewChild('table', { static: false }) table: any
	totalRecords: number = 0
	idDelete: number = 0
	ngOnInit() {
		this.buildForm()
		this.getList()
	}

	ngAfterViewInit() {
		this._shareData.seteventnotificationDropdown()
	}

	get f() {
		return this.form.controls
	}

	buildForm() {
		this.form = this._fb.group({
			code: [this.model.code, Validators.required],
			name: [this.model.name, Validators.required],
			description: [this.model.description],
			isActived: [this.model.isActived, Validators.required],
			orderNumber: [this.model.orderNumber],
		})
	}

	rebuilForm() {
		this.form.reset({
			code: this.model.code,
			name: this.model.name,
			isActived: this.model.isActived,
			description: this.model.description,
			orderNumber: this.model.orderNumber,
		})
	}

	getList() {
		this.code = this.code.trim()
		this.name = this.name.trim()
		let baserequest = {
			Code: this.code,
			Name: this.name,
			isActived: this.isActived,
			PageIndex: this.pageIndex,
			PageSize: this.pageSize,
		}
		let request =
			'?Code=' + this.code + '&Name=' + this.name + '&isActived=' + (this.isActived != null ? this.isActived : '') + '&PageIndex=' + this.pageIndex + '&PageSize=' + this.pageSize

		this._service.fieldGetList(baserequest).subscribe((response) => {
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

	onPageChange(event: any) {
		this.pageSize = event.rows
		this.pageIndex = event.first / event.rows + 1
		this.getList()
	}

	dataStateChange() {
		this.pageIndex = 1
		this.table.first = 0
		this.getList()
	}

	changeState(event: any) {
		if (event) {
			if (event.target.value == 'null') {
				this.isActived = null
			} else {
				this.isActived = event.target.value
			}
			this.pageIndex = 1
			this.pageSize = 20
			this.getList()
		}
	}

	changeType(event: any) {
		if (event) {
			this.pageIndex = 1
			this.pageSize = 20
			this.getList()
		}
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
		if (this.form.invalid) {
			return
		}
		if (this.model.orderNumber <= 0 && this.model.orderNumber != null && this.model.orderNumber.toString() != '') {
			this.model.orderNumber = 1
		}
		if (this.model.id == 0 || this.model.id == null) {
			this._service.fieldInsert(this.model).subscribe((response) => {
				if (response.success == RESPONSE_STATUS.success) {
					$('#modal').modal('hide')
					this._toastr.success(MESSAGE_COMMON.ADD_SUCCESS)
					this.getList()
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
					$('#modal').modal('hide')
					this._toastr.success(MESSAGE_COMMON.UPDATE_SUCCESS)
					this.getList()
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
		this._service.fieldDelete(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				this._toastr.success(MESSAGE_COMMON.DELETE_SUCCESS)
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

	onUpdateStatus(data) {
		var isActived = data.isActived
		let request = {
			Type: 1,
			Id: data.id,
		}
		this._service.fieldUpdateStatus(request).subscribe((res) => {
			if (res.success == 1) {
				if (data.isActive == true) {
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
	exportExcel() {
		let request = {
			Code: this.code,
			Name: this.name,
			//orderNumber: this.orderNumber,
			IsActived: this.isActived,
		}

		this._service.fieldExportExcel(request).subscribe((response) => {
			var today = new Date()
			var dd = String(today.getDate()).padStart(2, '0')
			var mm = String(today.getMonth() + 1).padStart(2, '0')
			var yyyy = today.getFullYear()
			var hh = String(today.getHours()).padStart(2, '0')
			var minute = String(today.getMinutes()).padStart(2, '0')
			var fileName = 'DM_ChucVuHanhChinh_' + yyyy + mm + dd + hh + minute
			var blob = new Blob([response], { type: response.type })
			importedSaveAs(blob, fileName)
		})
	}
}
