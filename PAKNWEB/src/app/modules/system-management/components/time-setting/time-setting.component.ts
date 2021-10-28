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
	time: Date
	date1: Date
	date11: Date = new Date('01/11/2021')
	listDate: []
	lstDate = []
	@ViewChild('table', { static: false }) table: any
	totalRecords: number = 0
	idDelete: number = 0
	title: any
	name: any
	description: any

	vi: any
	ngOnInit() {
		this.buildForm()
		this.getList()
		this.getListDate()
		this.vi = {
			firstDayOfWeek: 1,
			dayNames: ['Chủ Nhật', 'Thứ Hai', 'Thứ Ba', 'Thứ Tư', 'Thứ Năm', 'Thứ Sáu', 'Thứ Bảy'],
			dayNamesShort: ['Chủ Nhật', 'Thứ Hai', 'Thứ Ba', 'Thứ Tư', 'Thứ Năm', 'Thứ Sáu', 'Thứ Bảy'],
			dayNamesMin: ['CN', 'Hai', 'Ba', 'Tư', 'Năm', 'Sáu', 'Bảy'],
			monthNames: [
				'Tháng Một',
				'Tháng Hai',
				'Tháng Ba',
				'Tháng Tư',
				'Tháng Năm',
				'Tháng Sáu',
				'Tháng Bảy',
				'Tháng Tám',
				'Tháng Chín',
				'Tháng Mười',
				'Tháng Mười Một',
				'Tháng Mười Hai',
			],
			monthNamesShort: ['Thg 1', 'Thg 2', 'Thg 3', 'Thg 4', 'Thg 5', 'Thg 6', 'Thg 7', 'Thg 8', 'Thg 9', 'Thg 10', 'Thg 11', 'Thg 12'],
			today: 'Hôm nay',
			clear: 'Nhập lại',
		}
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
			time: [this.model.time, Validators.required],
		})
	}
	getListDate() {
		this._service.getSystemTimeDateActive().subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				this.listDate = response.result.SYTimeGetDateActive
				this.lstDate = this.listDate.map(({ time }) => new Date(time).setHours(0, 0, 0, 0))
			} else {
				this._toastr.error(response.message)
			}
		}),
			(error) => {
				console.error(error)
				alert(error)
			}
	}
	getMarkedDays(date) {
		var i = new Date(date.month + 1 + '/' + date.day + '/' + date.year).getTime()
		return this.lstDate.find((item) => {
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
		this._service.getSystemTime({}).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result != null) {
					this.listData = []
					this.listData = response.result.SYTimeGetAllOnPage

					this.totalRecords = response.result.SYTimeGetAllOnPage.length != 0 ? response.result.SYTimeGetAllOnPage[0].rowNumber : 0
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
	dataUpdate: any
	preUpdateStatus(data) {
		this.dataUpdate = data
		$('#modalConfirmUpdateStatus').modal('show')
	}
	preCreate() {
		this.model = new FieldObject()
		this.rebuilForm()
		this.submitted = false
		this.title = 'Thêm mới ngày nghỉ'
		$('#modal').modal('show')
		setTimeout(() => {
			$('#target').focus()
		}, 400)
	}

	onSave() {
		this.submitted = true
		if (this.form.invalid) {
			return
		}

		if (this.model.id == 0 || this.model.id == null) {
			let timeStr = this.model.time.toLocaleDateString()
			this.model.time = new Date(timeStr)
			this._service.insertSystemTime(this.model).subscribe((response) => {
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
				(error) => {
					console.error(error)
					alert(error)
				}
		} else {
			this._service.updateSystemTime(this.model).subscribe((response) => {
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
				(error) => {
					console.error(error)
					alert(error)
				}
		}
		this.getListDate()
	}

	preUpdate(data) {
		let request = {
			Id: data.id,
		}
		this._service.getSystemTimeById(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				this.rebuilForm()
				this.title = 'Chỉnh sửa ngày nghỉ'
				this.model = response.result.SYTimeGetByID[0]
				this.model.time = new Date(this.model.time)
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
		this._service.deleteSystemTime(request, obj.name).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result > 0) {
					this._toastr.success(MESSAGE_COMMON.DELETE_SUCCESS)
				} else {
					this._toastr.error('Ngày nghỉ này đang được sử dụng')
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

	onUpdateStatus(data) {
		data.isActived = !data.isActived
		this._service.updateSystemTime(data, true).subscribe((res) => {
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
			(error) => {
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
