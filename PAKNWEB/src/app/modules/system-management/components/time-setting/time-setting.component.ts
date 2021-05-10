import { Component, OnInit, ViewChild } from '@angular/core'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'
import { FieldObject } from 'src/app/models/fieldObject'
import { SystemconfigService } from '../../../../services/systemconfig.service'
import { DataService } from 'src/app/services/sharedata.service'
import { saveAs as importedSaveAs } from 'file-saver'
import { MESSAGE_COMMON, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { CalendarModule } from 'primeng/calendar'
declare var $: any

@Component({
	selector: 'app-time-setting',
	templateUrl: './time-setting.component.html',
	styleUrls: ['./time-setting.component.css'],
})
export class TimeSettingComponent implements OnInit {
	constructor(private _service: SystemconfigService, private _toastr: ToastrService, private _fb: FormBuilder, private _shareData: DataService) {}

	listData = new Array<TimeObject>()
	listStatus: any = [
		{ value: true, text: 'Hiệu lực' },
		{ value: false, text: 'Hết hiệu lực' },
	]
	form: FormGroup
	model: any = new TimeObject()
	submitted: boolean = false
	isActived: boolean
	title: string = ''
	name: string = ''
	time: Date
	date11: Date = new Date('01/11/2021')
	description: string = ''
	pageIndex: number = 1
	pageSize: number = 20
	listDate: []
	lstDate = []
	@ViewChild('table', { static: false }) table: any
	totalRecords: number = 0
	idDelete: number = 0
	ngOnInit() {
		this.buildForm()
		this.getList()
		this.getListDate()
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

	buildForm() {
		this.form = this._fb.group({
			name: [this.model.name, Validators.required],
			description: [this.model.description],
			isActived: [this.model.isActived, Validators.required],
			time: [this.model.time, Validators.required],
		})
	}
	getListDate() {
		this._service.getSystemTimeDateActive().subscribe(response => {
			if (response.success == RESPONSE_STATUS.success) {
				this.listDate = response.result.SYTimeGetDateActive
				this.lstDate = this.listDate.map(({ time }) => new Date(time).getTime())
				console.log(this.listDate)
				console.log(this.lstDate)
			} else {
				this._toastr.error(response.message)
			}
		}),
			error => {
				console.error(error)
				alert(error)
			}
	}
	getMarkedDays(date) {
		var i = new Date(date.month + 1 + '/' + date.day + '/' + date.year).getTime()
		return this.lstDate.find(item => {
			return item === i
		})
	}
	rebuilForm() {
		this.form.reset({
			name: this.model.name,
			isActived: this.model.isActived,
			description: this.model.description,
			time: this.model.time,
		})
	}

	getList() {
		this.name = this.name.trim()
		this.description = this.description.trim()
		let request = {
			Name: this.name,
			Time: this.time != null ? this.time.toLocaleDateString() : '',
			Description: this.description.trim(),
			isActived: this.isActived != null ? this.isActived : '',
			PageIndex: this.pageIndex,
			PageSize: this.pageSize,
		}
		this._service.getSystemTime(request).subscribe(response => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result != null) {
					this.listData = []
					this.listData = response.result.SYTimeGetAllOnPage
					console.log(response)
					this.totalRecords = response.result.SYTimeGetAllOnPage.length != 0 ? response.result.SYTimeGetAllOnPage[0].rowNumber : 0
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
	dataUpdate: any
	preUpdateStatus(data) {
		this.dataUpdate = data
		$('#modalConfirmUpdateStatus').modal('show')
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
		this.title = 'Thêm mới chức vụ'
		$('#modal').modal('show')
	}

	onSave() {
		this.submitted = true
		if (this.form.invalid) {
			return
		}
		if (this.model.id == 0 || this.model.id == null) {
			this.model.time = this.model.time.toLocaleDateString()
			this._service.insertSystemTime(this.model).subscribe(response => {
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
			this._service.updateSystemTime(this.model).subscribe(response => {
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
		}
		this._service.getSystemTimeById(request).subscribe(response => {
			if (response.success == RESPONSE_STATUS.success) {
				this.rebuilForm()
				this.title = 'Chỉnh sửa chức vụ'
				this.model = response.result.SYTimeGetByID[0]
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

	onDelete(id: number) {
		let request = {
			Id: id,
		}
		this._service.deleteSystemTime(request).subscribe(response => {
			console.log(response)
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result > 0) {
					this._toastr.success(MESSAGE_COMMON.DELETE_SUCCESS)
				} else {
					this._toastr.error('Chức vụ này đang được sử dụng')
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
		data.isActived = !data.isActived
		this._service.updateSystemTime(data).subscribe(res => {
			$('#modalConfirmUpdateStatus').modal('hide')
			if (res.success == 'OK') {
				if (data.isActived == true) {
					this._toastr.success(MESSAGE_COMMON.UNLOCK_SUCCESS)
				} else {
					this._toastr.success(MESSAGE_COMMON.LOCK_SUCCESS)
				}
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
	// exportExcel() {
	//   let request = {
	//     Name: this.name,
	//     IsActived: this.isActived,
	//   }

	//   this._service.fieldExportExcel(request).subscribe((response) => {
	//     var today = new Date()
	//     var dd = String(today.getDate()).padStart(2, '0')
	//     var mm = String(today.getMonth() + 1).padStart(2, '0')
	//     var yyyy = today.getFullYear()
	//     var hh = String(today.getHours()).padStart(2, '0')
	//     var minute = String(today.getMinutes()).padStart(2, '0')
	//     var fileName = 'DM_ChucVuHanhChinh_' + yyyy + mm + dd + hh + minute
	//     var blob = new Blob([response], { type: response.type })
	//     importedSaveAs(blob, fileName)
	//   })
	// }
}
export class TimeObject {
	constructor() {
		this.id = 0
		this.orderNumber = null
		this.name = ''
		this.code = ''
		this.description = ''
		this.isDeleted = false
		this.isActived = true
		this.time = null
	}
	id: number
	orderNumber: number
	name: string
	code: string
	description: string
	isActived: boolean
	isDeleted: boolean
	time: Date
}
