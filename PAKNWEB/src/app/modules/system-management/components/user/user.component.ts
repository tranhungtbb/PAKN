import { Component, OnInit, ViewChild, AfterViewInit, Input } from '@angular/core'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'
import { Router } from '@angular/router'

import { DataService } from 'src/app/services/sharedata.service'
import { UserObject, UserObject2 } from 'src/app/models/UserObject'
import { UserService } from 'src/app/services/user.service'
import { UnitService } from 'src/app/services/unit.service'
import { PositionService } from 'src/app/services/position.service'
import { RoleService } from 'src/app/services/role.service'
import { MESSAGE_COMMON, RESPONSE_STATUS, EXCEL_TYPE, EXCEL_EXTENSION } from 'src/app/constants/CONSTANTS'
import { COMMONS } from 'src/app/commons/commons'
import { UserCreateOrUpdateComponent } from 'src/app/modules/system-management/components/user/user-create-or-update/user-create-or-update.component'
import { UserViewInfoComponent } from 'src/app/modules/system-management/components/user/user-view-info/user-view-info.component'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { UserServiceChatBox } from 'src/app/modules/chatbox/user/user.service'

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
		private positionService: PositionService,
		private _shareData: DataService,
		private _router: Router,
		private storeageService: UserInfoStorageService,
		private _userServiceChat: UserServiceChatBox
	) {}

	listData = new Array<UserObject2>()
	listStatus: any = [
		{ value: true, text: 'Hiệu lực' },
		{ value: false, text: 'Hết hiệu lực' },
	]
	listHisStatus: any = [
		{ value: 1, text: 'Thành công' },
		{ value: 0, text: 'Thất bại' },
	]

	positionsList: any[] = []
	rolesList: any[] = []
	unitsList: any[] = []

	// change Password
	formChangePassword: FormGroup
	newPassword: string
	rePassword: string
	samePass = false

	// object User
	modelUser: any = new UserObject2()
	submitted: boolean = false
	isShowPassword: any = false
	isShowRePassword: any = false

	// view child
	@ViewChild('table', { static: false }) table: any
	@ViewChild('table2', { static: false }) table2: any
	@ViewChild(UserCreateOrUpdateComponent, { static: false }) childCreateOrUpdateUser: UserCreateOrUpdateComponent
	@ViewChild(UserViewInfoComponent, { static: false }) childDetailUser: UserViewInfoComponent

	// history
	dataSearch2: HistoryUser = new HistoryUser()
	listHisData: any[] = []
	hisTotalRecords: number = 0
	emailUser: any
	hisPageIndex: any = 1
	hisPageSize: any = 10
	hisUserId: any

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

		this.formChangePassword = this._fb.group({
			newPassword: [this.newPassword, Validators.required],
			rePassword: [this.rePassword, Validators.required],
		})
	}

	ngAfterViewInit() {
		$('#modal').on('keypress', function (e) {
			if (e.which == 13) e.preventDefault()
		})
		$('#modalChangePassword').on('keypress', function (e) {
			if (e.which == 13) e.preventDefault()
		})

		this.childCreateOrUpdateUser.parentUser = this
		this.childDetailUser.parentUser = this
	}
	getDropDown() {
		this._service.getDataForCreate({}).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				this.positionsList = res.result.lstPossition
				this.unitsList = res.result.lstUnit
			}
		})
	}

	getList() {
		this._service.getAllOnPagedList({}).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result.SYUserGetAllOnPage.length > 0) {
					this.listData = response.result.SYUserGetAllOnPage
					this.totalRecords = response.result.TotalCount
				} else {
					this.listData = []
					this.totalRecords = 0
				}
			} else {
				this.listData = []
				this.totalRecords = 0
				this._toastr.error(response.message)
			}
		}),
			(error) => {
				console.log(error)
				alert(error)
			}
	}

	get f() {
		return this.formChangePassword.controls
	}

	rebuilForm() {
		this.formChangePassword.reset({
			newPassword: this.newPassword,
			rePassword: this.rePassword,
		})
	}

	clearChangePasswordModel() {
		this.newPassword = ''
		this.rePassword = ''
	}
	preChangePassword(id: any) {
		this.submitted = false
		this.clearChangePasswordModel()
		this.samePass = false
		this.userId = id
		this.rebuilForm()
		$('#modalChangePassword').modal('show')
	}

	onChangePassword() {
		this.submitted = true
		this.newPassword = this.newPassword.trim()
		this.rePassword = this.rePassword.trim()

		this.rebuilForm()
		if (this.formChangePassword.invalid) {
			return
		}
		if (this.newPassword != this.rePassword) {
			this.samePass = true
			return
		} else {
			this.samePass = false
		}
		let obj = {
			UserId: this.userId,
			NewPassword: this.newPassword,
			RePassword: this.rePassword,
		}
		this._service.changePasswordInManage(obj).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				$('#modalChangePassword').modal('hide')
				this._toastr.success('Đổi mật khẩu thành công.')
			} else {
				this._toastr.error(res.message)
			}
		}),
			(error) => {
				console.error(error)
				alert(error)
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
				let user = this.listData.find((x) => x.id == this.userId)
				if (user) {
					const userDel = {
						login: user.userName,
						password: 'quickblox',
					}
					this._userServiceChat.deleteUser(userDel)
				}
			} else {
				if (isNaN(response.result)) {
					this._toastr.error(response.message)
					return
				}
				this._toastr.error('Không thể xóa người dùng đã nằm trong 1 qui trình')
				return
			}
		}),
			(error) => {
				this._toastr.error(error)
				console.error(error)
			}
	}

	preDelete(id: any) {
		this.userId = id
		$('#modalConfirmDelete').modal('show')
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
		this._service.changeStatus({ Id: item.id, IsActived: item.isActived }).subscribe((res) => {
			if (res.success != 'OK') {
				this._toastr.error(COMMONS.UPDATE_FAILED)
				//item.isActived = !item.isActived
				return
			}
			this._toastr.success(COMMONS.UPDATE_SUCCESS)
			this.getList()
		})
	}
	onChange(data) {
		console.log(data)
	}

	cleaseHisModel() {
		this.hisPageIndex = 1
		this.hisPageSize = 10
		this.hisUserId = null
		this.dataSearch2.fromDate = null
		this.dataSearch2.toDate = null
		this.dataSearch2.content = null
		this.dataSearch2.status = null
		this.hisTotalRecords = 0
	}

	close() {
		this.cleaseHisModel()
		this.clearChangePasswordModel()
	}

	onHisPageChange(event: any) {
		this.hisPageSize = event.rows
		this.hisPageIndex = event.first / event.rows + 1
		this.getHistory(this.hisUserId, this.emailUser)
	}

	dataHisStateChange() {
		this.hisPageIndex = 1
		this.table2.first = 0
		this.getHistory(this.hisUserId, this.emailUser)
	}
	onChangeFromDate(event) {
		if (event) {
			this.dataSearch2.fromDate = event
		} else {
			this.dataSearch2.fromDate = null
		}
		this.getHistory(this.hisUserId, this.emailUser)
	}

	onChangeToDate(event) {
		if (event) {
			this.dataSearch2.toDate = event
		} else {
			this.dataSearch2.toDate = null
		}
		this.getHistory(this.hisUserId, this.emailUser)
	}

	getHistory(id: any, email: any, status = null) {
		if (id == undefined) return
		if (id != this.hisUserId || status == true) {
			this.cleaseHisModel()
			this.table2.reset()
		}

		this.hisUserId = id
		this.listHisData = []
		this.emailUser = email
		this.dataSearch2.content = this.dataSearch2.content == null ? '' : this.dataSearch2.content.trim()
		let req = {
			FromDate: this.dataSearch2.fromDate == null ? '' : this.dataSearch2.fromDate.toDateString(),
			ToDate: this.dataSearch2.toDate == null ? '' : this.dataSearch2.toDate.toDateString(),
			Content: this.dataSearch2.content,
			Status: this.dataSearch2.status == null ? '' : this.dataSearch2.status,
			PageIndex: this.hisPageIndex,
			PageSize: this.hisPageSize,
			UserId: id,
		}
		this._service.getSystemLogin(req).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result.SYSystemLogGetAllOnPage.length > 0) {
					this.listHisData = response.result.SYSystemLogGetAllOnPage
					this.hisTotalRecords = response.result.SYSystemLogGetAllOnPage.length != 0 ? response.result.SYSystemLogGetAllOnPage[0].rowNumber : 0
					// this.hisPageIndex = response.result.
					$('#modalHis').modal('show')
				} else {
					this.hisPageIndex = 1
					this.hisPageSize = 10
					this.hisTotalRecords = 0
					this.listHisData = []
					$('#modalHis').modal('show')
				}
			} else {
				this.cleaseHisModel()
				this.listHisData = []
			}
		})
	}
	showPassword() {
		this.isShowPassword = !this.isShowPassword
	}
	showRePassword() {
		this.isShowRePassword = !this.isShowRePassword
	}
	onExport() {
		let passingObj: any = {}
		if (this.listHisData.length > 0) {
			$('#modalHis').modal('hide')
			passingObj.UserId = this.hisUserId
			passingObj.UserProcessId = this.storeageService.getUserId()
			passingObj.UserProcessName = this.storeageService.getFullName()

			passingObj.FromDate = this.dataSearch2.fromDate
			passingObj.ToDate = this.dataSearch2.toDate
			passingObj.Content = this.dataSearch2.content
			passingObj.Status = this.dataSearch2.status
			this._shareData.setobjectsearch(passingObj)
			this._shareData.sendReportUrl = 'HistoryUser?' + JSON.stringify(passingObj)
			this._router.navigate(['quan-tri/xuat-file'])
		} else {
			this._toastr.error('Không có thông tin lịch sử')
		}
	}
}

export class HistoryUser {
	constructor() {
		this.fromDate = null
		this.toDate = null
	}
	fromDate: Date
	toDate: Date
	content: string
	status: number
}
