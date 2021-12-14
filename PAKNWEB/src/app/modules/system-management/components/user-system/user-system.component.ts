import { Component, OnInit, ViewChild } from '@angular/core'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'
import { Router } from '@angular/router'

import { DataService } from 'src/app/services/sharedata.service'
import { UserSystemObject } from 'src/app/models/UserObject'
import { UserService } from 'src/app/services/user.service'
import { RoleService } from 'src/app/services/role.service'
import { MESSAGE_COMMON, RESPONSE_STATUS, CONSTANTS } from 'src/app/constants/CONSTANTS'
import { COMMONS } from 'src/app/commons/commons'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { UserServiceChatBox } from 'src/app/modules/chatbox/user/user.service'

declare var $: any
@Component({
	selector: 'app-user-system',
	templateUrl: './user-system.component.html',
	styleUrls: ['./user-system.component.css'],
})
export class UserSystemComponent implements OnInit {
	constructor(
		private _service: UserService,
		private roleService: RoleService,
		private _toastr: ToastrService,
		private _fb: FormBuilder,
		private _shareData: DataService,
		private _router: Router,
		private storeageService: UserInfoStorageService,
		private _userServiceChat: UserServiceChatBox
	) {}

	listData = new Array<UserSystemObject>()
	listStatus: any = [
		{ value: true, text: 'Hiệu lực' },
		{ value: false, text: 'Hết hiệu lực' },
	]
	listHisStatus: any = [
		{ value: 1, text: 'Thành công' },
		{ value: 0, text: 'Thất bại' },
	]
	listGender: any = [
		{ value: true, text: 'Nam' },
		{ value: false, text: 'Nữ' },
	]
	// change Password
	formChangePassword: FormGroup
	newPassword: string
	rePassword: string
	samePass = false

	userName: any = ''
	fullName: any = ''
	phone: any = ''
	// object User
	model = new UserSystemObject()
	submitted: boolean = false
	isShowPassword: any = false
	isShowRePassword: any = false

	// view child
	@ViewChild('table', { static: false }) table: any
	@ViewChild('table2', { static: false }) table2: any

	// history
	dataSearch2: HistoryUser = new HistoryUser()
	listHisData: any[] = []
	hisTotalRecords: number = 0
	emailUser: any
	hisPageIndex: any = 1
	hisPageSize: any = 10
	hisUserId: any

	totalRecords: number = 0
	userId: number = 0
	title: any

	// create or update
	createUserForm: FormGroup
	isAdmin: any
	fileAccept = CONSTANTS.FILEACCEPTAVATAR
	files: any = []

	ngOnInit() {
		this.getList()
		this.buildForm()
		this.formChangePassword = this._fb.group({
			newPassword: [this.newPassword, Validators.required],
			rePassword: [this.rePassword, Validators.required],
		})
		this.isAdmin = this.storeageService.getIsAdmin()
	}

	ngAfterViewInit() {
		$('#modal').on('keypress', function (e) {
			if (e.which == 13) e.preventDefault()
		})
		$('#modalChangePassword').on('keypress', function (e) {
			if (e.which == 13) e.preventDefault()
		})
	}

	buildForm() {
		this.createUserForm = this._fb.group({
			email: ['', [Validators.required, Validators.email]],
			fullName: ['', [Validators.required]],
			phone: ['', [Validators.required, Validators.pattern('^(84|0[3|5|7|8|9])+([0-9]{8})$')]],
			gender: ['', [Validators.required]],
			isActived: ['', [Validators.required]],
			address: [''],
		})
	}

	reBuildForm() {
		this.createUserForm.reset({
			email: this.model.email,
			fullName: this.model.fullName,
			phone: this.model.phone,
			gender: this.model.gender,
			isActived: this.model.isActived,
			address: this.model.address,
		})
	}
	get fUser() {
		return this.createUserForm.controls
	}
	checkExists = {
		Email: false,
		Phone: false,
	}
	onCheckExist(field: string, value: string) {
		this._service
			.checkExists({
				field,
				value,
				id: this.model.id ? this.model.id : 0,
			})
			.subscribe((res) => {
				if (res.success == RESPONSE_STATUS.success) {
					this.checkExists[field] = res.result.SYUserCheckExists[0].exists
				}
			})
	}

	preCreate() {
		this.model = new UserSystemObject()
		this.submitted = false
		this.checkExists = {
			Email: false,
			Phone: false,
		}
		this.userAvatar = null
		this.reBuildForm()
		$('#modal').modal('show')
	}

	preUpdate(Id: any) {
		this.submitted = false
		$('#modalView').modal('hide')
		this._service.getByIdUpdate({ id: Id }).subscribe((res) => {
			if (res.success != 'OK') return
			this.model = res.result.SYUserGetByID[0]
			if (this.model.avatar == '' || this.model.avatar == null) {
				this.userAvatar = null
			} else {
				this.userAvatar = this.model.avatar
				let output: any = $('#modal .user-avatar-view')
				output.attr('src', this.userAvatar)
			}
			$('#modal').modal('show')
		})
	}

	preView(Id: any) {
		this._service.getByIdUpdate({ id: Id }).subscribe((res) => {
			if (res.success != 'OK') return
			this.model = res.result.SYUserGetByID[0]
			if (this.model.avatar == '' || this.model.avatar == null) {
				this.userAvatar = null
			} else {
				this.userAvatar = this.model.avatar
				let output: any = $('#modalView .user-avatar-view')
				output.attr('src', this.userAvatar)
			}
			$('#modalView').modal('show')
		})
	}

	onSave() {
		this.submitted = true
		this.model.userName = this.model.email
		if (this.createUserForm.invalid) {
			return
		}
		if (this.checkExists['Email'] || this.checkExists['Phone']) {
			return
		}
		this.model.countLock = 0
		this.model.lockEndOut = ''
		this.model.address = this.model.address == null ? '' : this.model.address.trim()
		const user = {
			login: this.model.email,
			password: 'quickblox',
			full_name: this.model.fullName,
			phone: this.model.phone,
			email: this.model.email,
			custom_data: JSON.stringify({ id: this.model.id }),
		}
		let fileParams = null
		if (this.files && this.files.length > 0) {
			fileParams = {
				name: this.files[0].name,
				file: this.files[0],
				type: this.files[0].type,
				size: this.files[0].size,
				public: false,
			}
		}

		if (!this.model.id || this.model.id == 0) {
			this._service.userSystemInsert(this.model, this.files).subscribe((res) => {
				if (res.success != 'OK') {
					let errorMsg = res.message
					this._toastr.error(errorMsg)
					return
				} else {
					$('#modal').modal('hide')
					this._toastr.success(COMMONS.ADD_SUCCESS)
					this.getList()
					user.custom_data = JSON.stringify({ id: res.result })
					this._userServiceChat.createUserForApp(user, fileParams)
				}
			})
		} else {
			this._service.userSystemUpdate(this.model, this.files).subscribe((res) => {
				if (res.success != 'OK') {
					let errorMsg = res.message
					this._toastr.error(errorMsg)
					return
				} else {
					$('#modal').modal('hide')
					this._toastr.success(COMMONS.UPDATE_SUCCESS)
					this.getList()
					this._userServiceChat.createUserForApp(user, fileParams)
				}
			})
		}
	}

	// avatar
	userAvatar: any
	onChangeAvatar() {
		$('#modal .seclect-avatar').click()
	}
	changeSelectAvatar(event: any) {
		var file = event.target.files[0]
		if (!['image/jpeg', 'image/png'].includes(file.type)) {
			this._toastr.error('Chỉ chọn tệp tin ảnh')
			event.target.value = null
			return
		}
		if (file.size > 3000000) {
			this._toastr.error('Ảnh dung lượng tối đa 3MB')
			event.target.value = null
			return
		}
		let output: any = $('#modal .user-avatar-view')
		output.attr('src', URL.createObjectURL(file))
		this.userAvatar = ''
		output.onload = function () {
			URL.revokeObjectURL(output.src) // free memory
		}
		this.files = []
		this.files.push(file)
		$('#modal .seclect-avatar').val('')
	}

	getList() {
		this._service.getUserSystemAllOnPagedList({}).subscribe((response) => {
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
		if (id != this.userId) {
			this.clearChangePasswordModel()
		}
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
		let user = this.listData.find((x) => x.id == this.userId)
		this._service.delete(request, user.fullName).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				this._toastr.success(MESSAGE_COMMON.DELETE_SUCCESS)
				this.getList()

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
		item.countLock = 0
		item.lockEndOut = ''
		this._service.changeStatus({ Id: item.id, IsActived: item.isActived }, item.fullName).subscribe((res) => {
			if (res.success != 'OK') {
				this._toastr.error(COMMONS.UPDATE_FAILED)
				//item.isActived = !item.isActived
				return
			}
			this._toastr.success(COMMONS.UPDATE_SUCCESS)
			this.getList()
		})
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
		this.getList()
	}

	onChangeToDate(event) {
		if (event) {
			this.dataSearch2.toDate = event
		} else {
			this.dataSearch2.toDate = null
		}
		this.getList()
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
		$('#modalHis').modal('hide')
		let passingObj: any = {}
		if (this.listHisData.length > 0) {
			passingObj.UserId = this.hisUserId
			passingObj.UserProcessId = this.storeageService.getUserId()
			passingObj.UserProcessName = this.storeageService.getFullName()

			passingObj.FromDate = this.dataSearch2.fromDate
			passingObj.ToDate = this.dataSearch2.toDate
			passingObj.Content = this.dataSearch2.content
			passingObj.Status = this.dataSearch2.status
		}

		this._shareData.setobjectsearch(passingObj)
		this._shareData.sendReportUrl = 'HistoryUser?' + JSON.stringify(passingObj)
		this._router.navigate(['quan-tri/xuat-file'])
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
