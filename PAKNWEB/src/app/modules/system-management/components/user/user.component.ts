import { Component, OnInit, ViewChild, AfterViewInit, Input } from '@angular/core'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'
import { UserObject, UserObject2 } from 'src/app/models/UserObject'
import { UserService } from 'src/app/services/user.service'
import { UnitService } from 'src/app/services/unit.service'
import { PositionService } from 'src/app/services/position.service'
import { RoleService } from 'src/app/services/role.service'
import { MESSAGE_COMMON, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { COMMONS } from 'src/app/commons/commons'
import { UserCreateOrUpdateComponent } from 'src/app/modules/system-management/components/user/user-create-or-update/user-create-or-update.component'
import { UserViewInfoComponent } from 'src/app/modules/system-management/components/user/user-view-info/user-view-info.component'

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
	modelUser: any = new UserObject2()
	submitted: boolean = false
	isActived: boolean
	userName: string = ''
	fullName: string = ''
	phone: string = ''
	unitId: any
	positionId: any
	pageIndex: number = 1
	pageSize: number = 20
	@ViewChild('table', { static: false }) table: any
	@ViewChild(UserCreateOrUpdateComponent, { static: false }) childCreateOrUpdateUser: UserCreateOrUpdateComponent
	@ViewChild(UserViewInfoComponent, { static: false }) childDetailUser: UserViewInfoComponent

	modalUserCreateOrUpdate(key: any = 0) {
		this.childCreateOrUpdateUser.openModal(0, key)
	}
	modalUserInfo(id: any) {
		this.childDetailUser.openModal(id)
	}

	totalRecords: number = 0
	userId: number = 0
	title: any
	ngOnInit() {
		// this.buildForm()
		this.getList()
		this.getDropDown()
	}

	ngAfterViewInit() {
		$('#modal').on('keypress', function (e) {
			if (e.which == 13) e.preventDefault()
		})
		this.childCreateOrUpdateUser.parentUser = this
		this.childDetailUser.parentUser = this
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
			Phone: this.phone != null ? this.phone : '',
			UnitId: this.unitId != null ? this.unitId : '',
			PositionId: this.positionId != null ? this.positionId : '',
			TypeId: 1, // auto 1
			PageIndex: this.pageIndex,
			PageSize: this.pageSize,
		}
		this._service.getAllOnPagedList(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result.SYUserGetAllOnPage.length > 0) {
					this.listData = response.result.SYUserGetAllOnPage
					this.totalRecords = response.result.TotalCount
					this.pageSize = response.result.PageSize
					this.pageIndex = response.result.PageIndex
				} else {
					this.listData = []
					this.totalRecords = 0
					this.pageSize = 20
					this.pageIndex = 1
				}
			} else {
				this.listData = []
				this.totalRecords = 0
				this.pageSize = 20
				this.pageIndex = 1
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

	onDelete() {
		let request = {
			Id: this.userId,
		}
		$('#modalConfirmDelete').modal('hide')
		this._service.delete(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				this._toastr.success(MESSAGE_COMMON.DELETE_SUCCESS)

				this.getList()
			} else {
				this._toastr.error(response.message)
			}
		}),
			(error) => {
				console.error(error)
			}
	}

	changeStatus(id: number) {
		this.userId = id
		$('#modalConfirmChangeStatus').modal('show')
	}

	onChangeUserStatus() {
		let item = this.listData.find((c) => c.id == this.userId)
		$('#modalConfirmChangeStatus').modal('hide')
		item.isActived = !item.isActived
		item.typeId = 1
		item.countLock = 0
		item.lockEndOut = ''
		this._service.changeStatus(item).subscribe((res) => {
			if (res.success != 'OK') {
				this._toastr.error(COMMONS.UPDATE_FAILED)
				//item.isActived = !item.isActived
				return
			}
			this._toastr.success(COMMONS.UPDATE_SUCCESS)
			this.getList()
		})
	}
	getHistory(id: any) {}
}
