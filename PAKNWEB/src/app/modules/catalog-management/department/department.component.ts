import { Component, OnInit, ViewChild } from '@angular/core'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'
import { DepartmentObject } from 'src/app/models/departmentObject'
import { CatalogService } from 'src/app/services/catalog.service'
import { DataService } from 'src/app/services/sharedata.service'
import { saveAs as importedSaveAs } from 'file-saver'
import { MESSAGE_COMMON, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'

declare var $: any

@Component({
	selector: 'app-department',
	templateUrl: './department.component.html',
	styleUrls: ['./department.component.css'],
})
export class DepartmentComponent implements OnInit {
	constructor(private _service: CatalogService, private _toastr: ToastrService, private _fb: FormBuilder, private _shareData: DataService) {}
	listDepartmentGroup: []
	listData = new Array<DepartmentObject>()
	listStatus: any = [
		{ value: true, text: 'Hiệu lực' },
		{ value: false, text: 'Hết hiệu lực' },
	]
	form: FormGroup
	model: any = new DepartmentObject()
	submitted: boolean = false
	isActived: boolean
	title: string = ''
	name: string = ''
	groupName: number = null
	phone: string = ''
	email: string = ''
	address: string = ''
	fax: string = ''
	description: string = ''
	pageIndex: number = 1
	pageSize: number = 20
	@ViewChild('table', { static: false }) table: any
	totalRecords: number = 0
	idDelete: number = 0
	dataUpdate: any
	ngOnInit() {
		this.buildForm()
		this.getList()
		let request = {
			Name: '',
			isActived: true,
			PageIndex: this.pageIndex,
			PageSize: 1000,
		}
		this._service.departmentGroupGetList(request).subscribe(response => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result != null) {
					this.listDepartmentGroup = []
					this.listDepartmentGroup = response.result.CADepartmentGroupGetAllOnPage
					console.log(this.listDepartmentGroup)
				}
			}
		})
	}

	ngAfterViewInit() {
		this._shareData.seteventnotificationDropdown()
		$('#modal').on('keypress', function(e) {
			if (e.which == 13) e.preventDefault()
		})
	}

	get f() {
		return this.form.controls
	}
	resd = /^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/

	buildForm() {
		this.form = this._fb.group({
			name: [this.model.name, Validators.required],
			description: [this.model.description],
			isActived: [this.model.isActived, Validators.required],
			departmentGroupId: [this.model.departmentGroup, Validators.required],
			email: ['', [Validators.required, Validators.pattern(this.resd)]], //Validators.pattern('^[a-z][a-z0-9_.]{5,32}@[a-z0-9]{2,}(.[a-z0-9]{2,4}){1,2}$')
			phone: ['', [Validators.required, Validators.pattern('^(84|0[3|5|7|8|9])+([0-9]{8})$')]],
			address: [''],
			fax: [''],
		})
	}

	rebuilForm() {
		this.form.reset({
			name: this.model.name,
			isActived: this.model.isActived,
			description: this.model.description,
			departmentGroupId: this.model.departmentGroupId,
			phone: this.model.phone,
			email: this.model.email,
			address: this.model.address,
			fax: this.model.fax,
		})
	}

	getList() {
		this.name = this.name.trim()

		let request = {
			Name: this.name.trim(),
			Phone: this.phone.trim(),
			Email: this.email.trim(),
			Address: this.address.trim(),
			Fax: this.fax.trim(),
			Description: this.description.trim(),
			DepartmentGroupId: this.groupName != null ? this.groupName : '',
			isActived: this.isActived != null ? this.isActived : '',
			PageIndex: this.pageIndex,
			PageSize: this.pageSize,
		}
		console.log(request)
		this._service.departmentGetList(request).subscribe(response => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result != null) {
					this.listData = []
					this.listData = response.result.CADepartmentGetAllOnPage
					console.log(this.listData)
					this.totalRecords = response.result.CADepartmentGetAllOnPage[0]?response.result.CADepartmentGetAllOnPage[0].rowNumber: 0
				}
			} else {
				this._toastr.error(response.message)
			}
		}),
			error => {
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
		this.model = new DepartmentObject()
		this.rebuilForm()
		this.submitted = false
		this.title = 'Thêm mới sở ngành'
		$('#modal').modal('show')
	}

	onSave() {
		this.submitted = true
		if (this.form.invalid) {
			return
		}
		if (this.model.id == 0 || this.model.id == null) {
			this._service.departmentInsert(this.model).subscribe(response => {
				console.log(response)
				if (response.success == RESPONSE_STATUS.success) {
					if (response.result == -1) {
						this._toastr.error(MESSAGE_COMMON.EXISTED_NAME)
						$('#target').focus()
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
				error => {
					console.error(error)
					alert(error)
				}
		} else {
			this._service.departmentUpdate(this.model).subscribe(response => {
				if (response.success == RESPONSE_STATUS.success) {
					if (response.result == -1) {
						this._toastr.error(MESSAGE_COMMON.EXISTED_NAME)
						$('#target').focus()
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
				error => {
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
		this._service.departmentGetById(request).subscribe(response => {
			if (response.success == RESPONSE_STATUS.success) {
				this.rebuilForm()
				this.title = 'Chỉnh sửa sở ngành'
				this.model = response.result.CADepartmentGetByID[0]
				$('#modal').modal('show')
			} else {
				this._toastr.error(response.message)
			}
		}),
			error => {
				console.error(error)
				alert(error)
			}
	}
	preDelete(id: number) {
		this.idDelete = id
		$('#modalConfirmDelete').modal('show')
	}
	preUpdateStatus(data) {
		this.dataUpdate = data
		$('#modalConfirmUpdateStatus').modal('show')
	}
	onDelete(id: number) {
		let request = {
			Id: id,
		}
		this._service.departmentDelete(request).subscribe(response => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result > 0) {
					this._toastr.success(MESSAGE_COMMON.DELETE_SUCCESS)
				} else {
					this._toastr.error('Bộ phận này đang được sử dụng')
				}
				$('#modalConfirmDelete').modal('hide')
				this.getList()
			} else {
				this._toastr.error(response.message)
			}
		}),
			error => {
				console.error(error)
			}
	}

	onUpdateStatus(data) {
		var isActived = data.isActived
		let request = {
			Type: 1,
			Id: data.id,
		}
		data.isActived = !data.isActived
		this._service.departmentUpdateStatus(data).subscribe(res => {
			console.log(res)
			$('#modalConfirmUpdateStatus').modal('hide')
			if (res.success == 'OK') {
				console.log(data)
				if (data.isActived) {
					this._toastr.success(MESSAGE_COMMON.UNLOCK_SUCCESS)
				} else {
					this._toastr.success(MESSAGE_COMMON.LOCK_SUCCESS)
				}
				this.getList()
			} else {
				this._toastr.error(res.message)
			}
		}),
			error => {
				console.error(error)
			}
	}

	preView(data) {
		this.model = data
		$('#modalDetail').modal('show')
	}
	exportExcel() {
		let request = {
			Name: this.name,
			IsActived: this.isActived,
		}

		this._service.fieldExportExcel(request).subscribe(response => {
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
