import { Component, OnInit, ViewChild, AfterViewInit, Input } from '@angular/core'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'
import { UserObject, UserObject2 } from 'src/app/models/UserObject'
import { UserService } from 'src/app/services/user.service'
import { UnitService } from 'src/app/services/unit.service'
import { PositionService } from 'src/app/services/position.service'
import { RoleService } from 'src/app/services/role.service'
import { MESSAGE_COMMON, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'

declare var $: any
@Component({
	selector: 'app-user',
	templateUrl: './user.component.html',
	styleUrls: ['./user.component.css'],
})
export class UserComponent implements OnInit {
	constructor(
		private _service: UserService,
		private roleService: RoleService,
		private _toastr: ToastrService,
		private _fb: FormBuilder,
		private unitService: UnitService,
		private positionService: PositionService
	) {}

	listData = new Array<UserObject2>()
	listStatus: any = [
		{ value: true, text: 'Hiệu lực' },
		{ value: false, text: 'Hết hiệu lực' },
	]

	positionsList: any[] = []
	rolesList: any[] = []
	unitsList: any[] = []

	form: FormGroup
	model: any = new UserObject2()
	submitted: boolean = false
	isActived: boolean
	userName: string = ''
	fullName: string = ''
	unitId: any
	pageIndex: number = 1
	pageSize: number = 20
	@ViewChild('table', { static: false }) table: any

	totalRecords: number = 0
	idDelete: number = 0
	title: any
	ngOnInit() {
		this.buildForm()
		this.getList()
	}

	ngAfterViewInit() {
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
		})
	}

	rebuilForm() {
		this.form.reset({
			name: this.model.name,
			isActived: this.model.isActived,
			description: this.model.description,
		})
	}

	getDropDown() {
		this.positionService
			.positionGetList({
				pageIndex: 1,
				pageSize: 1000,
			})
			.subscribe((res) => {
				if (res.success != 'OK') return
				this.positionsList = res.result.CAPositionGetAllOnPage
			})
		this.roleService.getAll({}).subscribe((res) => {
			if (res.success != 'OK') return
			this.rolesList = res.result.SYRoleGetAll
		})
		this.unitService.getAll({}).subscribe((res) => {
			if (res.success != 'OK') return
			this.unitsList = res.result.CAUnitGetAll
		})
	}

	getList() {
		this.userName = this.userName.trim()
		this.fullName = this.fullName.trim()
		let request = {
			UserName: this.userName != null ? this.userName : '',
			FullName: this.fullName != null ? this.fullName : '',
			IsActived: this.isActived != null ? this.isActived : '',
			Phone: '',
			UnitId: this.unitId != null ? this.unitId : '',
			TypeId: 1, // auto 1
			PageIndex: this.pageIndex,
			PageSize: this.pageSize,
		}
		this._service.getAllPagedList(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result != null) {
					this.listData = response.result.SYUserGetAllOnPage
					this.totalRecords = response.result.TotalCount
					this.pageSize = response.result.PageSize
					this.pageIndex = response.result.PageIndex
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

	confirmChangeStatus(data) {
		this.model = { ...data }
		this.model.isActived = !data.isActived
		$('#modalConfirmChangeStatus').modal('show')
	}

	preCreate() {
		this.model = new UserObject2()
		this.rebuilForm()
		this.submitted = false
		this.title = 'Thêm mới người dùng'
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
			this._service.insert(this.model).subscribe((response) => {
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
			this._service.update(this.model).subscribe((response) => {
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
		this._service.getById(request).subscribe((response) => {
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
		this._service.delete(request).subscribe((response) => {
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
		this._service.update(this.model).subscribe((res) => {
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
}
