import { Component, OnInit, HostListener, Input, ViewChild } from '@angular/core'
import { UserInfoStorageService } from '../../commons/user-info-storage.service'
import { AuthenticationService } from '../../services/authentication.service'
import { Router } from '@angular/router'
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'
import { UserSystemObject } from '../../models/UserObject'
import { UserService } from '../../services/user.service'
import { DataService } from '../../services/sharedata.service'
import { AccountService } from 'src/app/services/account.service'
import { RESPONSE_STATUS, RECOMMENDATION_STATUS, TYPE_NOTIFICATION, CONSTANTS } from 'src/app/constants/CONSTANTS'
import { NotificationService } from 'src/app/services/notification.service'
import { COMMONS } from 'src/app/commons/commons'
import { UserServiceChatBox } from 'src/app/modules/chatbox/user/user.service'

declare var $: any
@HostListener('window:scroll', ['$event'])
@Component({
	selector: 'app-appheader',
	templateUrl: './appheader.component.html',
	styleUrls: ['./appheader.component.css'],
})
export class AppheaderComponent implements OnInit {
	fileAccept = CONSTANTS.FILEACCEPTAVATAR
	totalThongBao: number = 0
	myDate: any
	myHours: any
	pageindex: number
	listPageIndex: any[] = []

	listData: any[] = []
	totalRecords: number = 0
	emailUser: string = ''
	isShowPassword: any = false
	isShowPasswordNew: any = false
	isShowRePasswordNew: any = false

	pageSizeGrid: number = 10
	files: any = ([] = [])
	updateForm: FormGroup
	userForm: FormGroup
	createUserForm: FormGroup
	userName: string
	errorMessage: any
	year: Date = new Date()
	notifications: any[] = []
	mindateUyQuyen: Date = new Date()
	exchange: FormGroup
	lstUserbyDep: Array<any> = []
	lstTimline: Array<any> = []
	lstTimlineMore: Array<any> = []
	model: UserSystemObject = new UserSystemObject()
	submitted: boolean = false
	listSexs: any[] = [
		{ text: 'Nam', value: true },
		{ text: 'Nữ', value: false },
	]
	listStatus: any = [
		{ value: 1, text: 'Thành công' },
		{ value: 0, text: 'Thất bại' },
	]
	listStatusUser: any = [
		{ value: true, text: 'Hiệu lực' },
		{ value: false, text: 'Hết hiệu lực' },
	]
	lstDropDownLayout = [
		{ value: 1, Text: 'Thành công' },
		{ value: 0, Text: 'Thất bại' },
	]
	fromDate: string = ''
	toDate: string = ''
	form: FormGroup
	dataSearch: SearchHistoryUser = new SearchHistoryUser()
	public timeOut: number = 1
	public exchangedata: any = {}
	listThongBao: any = []
	formData = new FormData()
	showLoader: boolean = true
	tenDonVi: string = ''
	remindWork: any = {}
	minDate: Date = new Date()

	pageIndex: number = 1
	pageSize: number = 10
	lstChucVu: any = []
	lstPhongBan: any = []

	Notifications: any[]
	numberNotifications: any = 10
	ViewedCount: number = 0
	@ViewChild('table', { static: false }) table: any
	isMain: any = this.storageService.getIsMain()
	userId: any = this.storageService.getUserId()

	constructor(
		private formBuilder: FormBuilder,
		private router: Router,
		private userService: UserService,
		private storageService: UserInfoStorageService,
		private authenService: AuthenticationService,
		private _router: Router,
		private _fb: FormBuilder,
		private _toastr: ToastrService,
		private sharedataService: DataService,
		private notificationService: NotificationService,
		private accountService: AccountService,
		private _userServiceChat: UserServiceChatBox
	) { }

	formChangePassword: FormGroup
	oldPassword: string
	newPassword: string
	rePassword: string
	samePass = false
	viewedCountLate: number = 0

	@Input('title') _title: string

	ngOnInit() {
		// this.buildForm()
		this.form = this._fb.group({
			toDate: [this.dataSearch.toDate],
			fromDate: [this.dataSearch.toDate],
			content: [this.dataSearch.content],
			status: [this.dataSearch.status],
		})
		this.userName = this.storageService.getFullName()
		this.buildForm()

		this.sharedataService.getnotificationDropdown.subscribe((data) => {
			if (data) {
				var result: any = data
				this.listThongBao = []
				this.listThongBao = result.notifications
				this.totalThongBao = result.totalRecords
			}
		})
		this.getNotifications(this.numberNotifications)
		this.formChangePassword = this._fb.group({
			oldPassword: [this.oldPassword, Validators.required],
			newPassword: [this.newPassword, Validators.required],
			rePassword: [this.rePassword, Validators.required],
		})
	}
	ngAfterViewInit() {
		$('#modalChangePasswordByMe').on('keypress', function (e) {
			if (e.which == 13) e.preventDefault()
		})
		$('#modalDetailLog').on('keypress', function (e) {
			if (e.which == 13) e.preventDefault()
		})
	}

	showPassword() {
		this.isShowPassword = !this.isShowPassword
	}
	showPasswordNew() {
		this.isShowPasswordNew = !this.isShowPasswordNew
	}
	showRePasswordNew() {
		this.isShowRePasswordNew = !this.isShowRePasswordNew
	}

	getNotifications(PageSize: Number) {
		this.ViewedCount = 0
		this.notificationService.getListNotificationOnPageByReceiveId({ PageSize: PageSize, PageIndex: 1 }).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				if (res.result.syNotifications.length > 0) {
					this.Notifications = res.result.syNotifications
					this.ViewedCount = res.result.syNotifications[0].viewedCount
					this.viewedCountLate = res.result.syNotifications[0].viewedCount
				} else {
					this.Notifications = []
				}
			}
			return
		})
	}

	updateNotifications() {
		this.viewedCountLate = 0
		this.notificationService.updateIsViewedNotification({}).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
			}
			return
		})
	}

	get f() {
		return this.updateForm.controls
	}
	checkDeny(status: any) {
		if (status == RECOMMENDATION_STATUS.PROCESS_DENY || status == RECOMMENDATION_STATUS.RECEIVE_DENY || status == RECOMMENDATION_STATUS.APPROVE_DENY) {
			return true
		}
		return false
	}

	get fChangePass() {
		return this.formChangePassword.controls
	}
	rebuilForm() {
		this.formChangePassword.reset({
			oldPassword: this.oldPassword,
			newPassword: this.newPassword,
			rePassword: this.rePassword,
		})
	}

	close() {
		this.clearChangePasswordModel()
	}

	clearChangePasswordModel() {
		this.newPassword = ''
		this.rePassword = ''
		this.oldPassword = ''
	}
	preChangePassword() {
		this.submitted = false
		this.clearChangePasswordModel()
		this.rebuilForm()
		$('#modalChangePasswordByMe').modal('show')
	}

	onChangePassword() {
		this.submitted = true
		this.oldPassword = this.oldPassword.trim()
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
			// UserId: this.storageService.getUserId(),
			OldPassword: this.oldPassword,
			NewPassword: this.newPassword,
			RePassword: this.rePassword,
		}
		this.accountService.changePassword(obj).subscribe((res) => {
			if (res.success != 'OK') {
				this._toastr.error(res.message)
				return
			}
			this._toastr.success('Đổi mật khẩu thành công')
			$('#modalChangePasswordByMe').modal('hide')
			// this.storageService.clear()
			// this.router.navigate(['/dang-nhap'])
		}),
			(error) => {
				console.error(error)
				alert(error)
			}
	}

	redirectListNotification() {
		this.router.navigate(['/quan-tri/thong-bao'])
	}

	onChangeAvatar() {
		$('#modalEditUserInfo .seclect-avatar').click()
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
		let output: any = $('#modalEditUserInfo .user-avatar-view')
		output.attr('src', URL.createObjectURL(file))
		this.userAvatar = ''
		output.onload = function () {
			URL.revokeObjectURL(output.src) // free memory
		}
		this.files = []
		this.files.push(file)
		$('#modalEditUserInfo .seclect-avatar').val('')
	}

	onClickNotification(dataId: any, type: any, typeSend: any) {
		if (type == TYPE_NOTIFICATION.NEWS) {
			// tin tức
			this.updateIsReadNotification(dataId)
			this.router.navigate(['/quan-tri/tin-tuc/chinh-sua/' + dataId])
		} else if (type == TYPE_NOTIFICATION.RECOMMENDATION) {
			// PAKN
			this.updateIsReadNotification(dataId)
			if (typeSend == 13) {
				this.router.navigate(['/quan-tri/kien-nghi/chi-tiet/' + dataId])
			} else {
				this.router.navigate(['/quan-tri/kien-nghi/chi-tiet/' + dataId])
			}
		} else if (type == TYPE_NOTIFICATION.INVITATION) {
			// Thư mời
			this.updateIsReadNotification(dataId)
			this.router.navigate(['/quan-tri/thu-moi/chi-tiet/' + dataId])
		} else if (type == TYPE_NOTIFICATION.ADMINISTRATIVE) {
			// tthc
			this.updateIsReadNotification(dataId)
			this.router.navigate(['/quan-tri/thu-tuc-hanh-chinh/chi-tiet/' + dataId])
		}
	}

	updateIsReadNotification(dataId: any) {
		this.notificationService.updateIsReadedNotification({ ObjectId: dataId }).subscribe()
		this.getNotifications(this.pageSize)
	}

	loadImage(path: string) {
		let request = {
			filePath: path,
		}
	}

	selectImage(event: any) {
		if (event) {
			const file = event.target.files[0]
			this.files = file
			const reader = new FileReader()
			reader.onload = () => {
				var avatar = document.getElementById('anhDaiDien')
				avatar.setAttribute('src', reader.result.toString())
			}
			reader.readAsDataURL(file)
		}
	}

	onUpdate() { }

	// profile

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
		this.userService
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

	userAvatar: any
	preUpdate() {
		this.submitted = false
		$('#modalViewUserInfo').modal('hide')
		if (this.model.avatar == '' || this.model.avatar == null) {
			this.userAvatar = null
		} else {
			this.userAvatar = this.model.avatar
			let output: any = $('#modalEditUserInfo .user-avatar-view')
			output.attr('src', this.userAvatar)
		}
		$('#modalEditUserInfo').modal('show')
	}

	preView() {
		this.userService.getByIdUpdate({ id: this.userId }).subscribe((res) => {
			if (res.success != 'OK') return
			this.model = res.result.SYUserGetByID[0]

			if (this.model.avatar == '' || this.model.avatar == null) {
				this.userAvatar = null
			} else {
				this.userAvatar = this.model.avatar
				let output: any = $('#modalViewUserInfo .user-avatar-view')
				output.attr('src', this.userAvatar)
			}
			$('#modalViewUserInfo').modal('show')
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
		if (this.isMain != true) {
			this.userService.userUpdateProfile(this.model, this.files).subscribe((res) => {
				$('#modalEditUserInfo .seclect-avatar').val('')
				if (res.success != 'OK') {
					let errorMsg = res.message
					this._toastr.error(errorMsg)
					return
				} else {
					$('#modalEditUserInfo').modal('hide')
					this._toastr.success(COMMONS.UPDATE_SUCCESS)
					this._userServiceChat.createUserForApp(user, fileParams)
				}
			})
		} else {
			this.userService.userSystemUpdate(this.model, this.files).subscribe((res) => {
				$('#modalEditUserInfo .seclect-avatar').val('')
				if (res.success != 'OK') {
					let errorMsg = res.message
					this._toastr.error(errorMsg)
					return
				} else {
					$('#modalEditUserInfo').modal('hide')
					this._toastr.success(COMMONS.UPDATE_SUCCESS)
					this._userServiceChat.createUserForApp(user, fileParams)
				}
			})
		}
	}

	preViewUserInfo() {
		if (this.isMain) {
		}
		return
	}

	signOut(): void {
		this.authenService.logOut({}).subscribe((success) => {
			if (success.success == RESPONSE_STATUS.success) {
				this.sharedataService.setIsLogin(false)
				this.storageService.setReturnUrl('')
				this.storageService.clear()
				this._router.navigate(['/dang-nhap-quan-tri'])
				//location.href = "/dang-nhap";
			}
		})
	}

	get newpassword() {
		return this.userForm.get('newpassword')
	}
	get oldpassword() {
		return this.userForm.get('oldpassword')
	}
	get confirmpassword() {
		return this.userForm.get('confirmpassword')
	}

	expandMenu() {
		if ($('body').hasClass('sidebar-collapse') || $('body').hasClass('sidebar-open')) {
			$('body').removeClass('sidebar-collapse sidebar-open')
		} else {
			$('body').addClass('sidebar-collapse sidebar-open')
		}
	}

	get controlform() {
		return this.exchange.controls
	}
	onScroll(event: any) {
		if (event.target.offsetHeight + event.target.scrollTop >= event.target.scrollHeight - 50) {
			if (this.numberNotifications == 10) {
				this.updateNotifications()
			}
			this.numberNotifications = this.numberNotifications + 5
			this.getNotifications(this.numberNotifications)
		}
	}

	isInvalidNam(event) {
		let test = event

		if (test == 'Invalid Date') {
			event = new Date()
		}
	}
	onPageChange(event: any) {
		this.pageSize = event.rows
		this.pageIndex = event.first / event.rows + 1
		this.getList()
	}

	onChangeFromDate(event) {
		if (event) {
			this.dataSearch.fromDate = event
		} else {
			this.dataSearch.fromDate = null
		}
		this.getList()
	}

	onChangeToDate(event) {
		if (event) {
			this.dataSearch.toDate = event
		} else {
			this.dataSearch.toDate = null
		}
		this.getList()
	}

	getList() {
		this.dataSearch.content = this.dataSearch.content == null ? '' : this.dataSearch.content.trim()
		let req = {
			FromDate: this.dataSearch.fromDate == null ? '' : this.dataSearch.fromDate.toDateString(),
			ToDate: this.dataSearch.toDate == null ? '' : this.dataSearch.toDate.toDateString(),
			Content: this.dataSearch.content,
			Status: this.dataSearch.status == null ? '' : this.dataSearch.status,
			PageIndex: this.pageIndex,
			PageSize: this.pageSize,
			UserId: localStorage.getItem('userId'),
		}
		this.userService.getSystemLogin(req).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result != null) {
					this.listData = []
					this.listData = response.result.SYSystemLogGetAllOnPage
					this.totalRecords = response.result.SYSystemLogGetAllOnPage.length != 0 ? response.result.SYSystemLogGetAllOnPage[0].rowNumber : 0
				}
			} else {
			}
		})
	}
	getUserDetail() {
		let req = {
			Id: localStorage.getItem('userId'),
		}
		this.userService.getById(req).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result != null) {
					this.emailUser = response.result.SYUserGetByID[0].email
				}
			} else {
			}
		})
	}
	dataStateChange() {
		this.pageIndex = 1
		this.getList()
	}
	showModalDetail(): void {
		$('#modalDetailLog').modal('show')
		this.pageIndex = 1
		this.pageSize = 20
		this.totalRecords = 0
		this.dataSearch.fromDate = this.dataSearch.toDate = null
		this.listData = []
		this.table.reset()
		this.getUserDetail()
		this.getList()
	}

	messageError: any
	showMessageError = (messageError: any) => {
		if (messageError) {
			this.messageError = messageError
			$('#modal-error-his-system').modal('show')
		}
	}

	onExport() {
		$('#modalDetailLog').modal('hide')
		let passingObj: any = {}
		passingObj.UserId = this.storageService.getUserId()
		passingObj.UserProcessId = this.storageService.getUserId()
		passingObj.UserProcessName = this.storageService.getFullName()

		passingObj.FromDate = this.dataSearch.fromDate
		passingObj.ToDate = this.dataSearch.toDate
		passingObj.Content = this.dataSearch.content
		passingObj.Status = this.dataSearch.status

		this.sharedataService.setobjectsearch(passingObj)
		this.sharedataService.sendReportUrl = 'HistoryUser?' + JSON.stringify(passingObj)
		this._router.navigate(['quan-tri/xuat-file'])
	}
}
export class SearchHistoryUser {
	constructor() {
		this.fromDate = null
		this.toDate = null
	}
	fromDate: Date
	toDate: Date
	content: string
	status: number
}
