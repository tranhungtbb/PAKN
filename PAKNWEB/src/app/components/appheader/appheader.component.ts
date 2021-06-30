import { Component, OnInit, HostListener, Input, ViewChild } from '@angular/core'
import { UserInfoStorageService } from '../../commons/user-info-storage.service'
import { AuthenticationService } from '../../services/authentication.service'
import { Router } from '@angular/router'
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms'
import { ChangePasswordUserObject } from '../../models/changePasswordUserObject'
import { ToastrService } from 'ngx-toastr'
import { UserObject } from '../../models/UserObject'
import { UserService } from '../../services/user.service'
import { DataService } from '../../services/sharedata.service'
import { AccountService } from 'src/app/services/account.service'
import { RESPONSE_STATUS, RECOMMENDATION_STATUS, TYPE_NOTIFICATION } from 'src/app/constants/CONSTANTS'
import { NotificationService } from 'src/app/services/notification.service'
import { from } from 'rxjs'
import { UserViewInfoComponent } from '../../modules/system-management/components/user/user-view-info/user-view-info.component'
import { create } from 'domain'

declare var $: any
@HostListener('window:scroll', ['$event'])
@Component({
	selector: 'app-appheader',
	templateUrl: './appheader.component.html',
	styleUrls: ['./appheader.component.css'],
})
export class AppheaderComponent implements OnInit {
	totalThongBao: number = 0
	myDate: any
	myHours: any
	pageindex: number
	listPageIndex: any[] = []

	listData: any[] = []
	totalRecords: number = 0
	emailUser: string = ''

	pageSizeGrid: number = 10
	files: any
	updateForm: FormGroup
	userForm: FormGroup
	userName: string
	errorMessage: any
	year: Date = new Date()
	notifications: any[] = []
	mindateUyQuyen: Date = new Date()
	exchange: FormGroup
	lstUserbyDep: Array<any> = []
	lstTimline: Array<any> = []
	lstTimlineMore: Array<any> = []
	model: UserObject = new UserObject()
	submitted: boolean = false
	listSexs: any[] = [
		{ text: 'Nam', value: true },
		{ text: 'Nữ', value: false },
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
	numberNotifications: any = 7
	ViewedCount: number = 0
	@ViewChild('table', { static: false }) table: any

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
	) {}

	formChangePassword: FormGroup
	oldPassword: string
	newPassword: string
	rePassword: string
	samePass = false

	@Input('title') _title: string

	ngOnInit() {
		// this.buildForm()
		this.form = this._fb.group({
			toDate: [this.dataSearch.toDate],
			fromDate: [this.dataSearch.toDate],
		})
		this.userName = this.storageService.getFullName()

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

	getNotifications(PageSize: Number) {
		this.ViewedCount = 0
		this.notificationService.getListNotificationOnPageByReceiveId({ PageSize: PageSize, PageIndex: 1 }).subscribe((res) => {
			if ((res.success = RESPONSE_STATUS.success)) {
				if (res.result.syNotifications.length > 0) {
					this.Notifications = res.result.syNotifications
					this.ViewedCount = res.result.syNotifications[0].viewedCount
				} else {
					this.Notifications = []
				}
			}
			return
		})
	}

	updateNotifications() {
		this.notificationService.updateIsViewedNotification({}).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				this.getNotifications(this.numberNotifications)
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
			oldPassword : this.oldPassword,
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
			OldPassword : this.oldPassword,
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
		}),
			(error) => {
				console.error(error)
				alert(error)
			}
		// this.userService.changePasswordInManage(obj).subscribe((res) => {
		// 	if (res.success == RESPONSE_STATUS.success) {
		// 		$('#modalChangePasswordByMe').modal('hide')
		// 		this._toastr.success('Đổi mật khẩu thành công.')
		// 	} else {
		// 		this._toastr.error(res.message)
		// 	}
		// }),
		// 	(error) => {
		// 		console.error(error)
		// 		alert(error)
		// 	}
	}

	redirectListNotification() {
		this.router.navigate(['/quan-tri/thong-bao'])
	}

	onClickNotification(dataId: any, type: any, typeSend: any) {
		if (type == TYPE_NOTIFICATION.NEWS) { // tin tức
			this.updateIsReadNotification(dataId)
			this.router.navigate(['/quan-tri/tin-tuc/chinh-sua/' + dataId])
		} else if (type == TYPE_NOTIFICATION.RECOMMENDATION) { // PAKN
			this.updateIsReadNotification(dataId)
			this.router.navigate(['/quan-tri/kien-nghi/chi-tiet/' + dataId])
		}
		else if (type == TYPE_NOTIFICATION.INVITATION) { // Thư mời
			this.updateIsReadNotification(dataId)
			this.router.navigate(['/quan-tri/thu-moi/chi-tiet/' + dataId])
		}
	}

	updateIsReadNotification(dataId: any) {
		this.notificationService.updateIsReadedNotification({ ObjectId: dataId }).subscribe()
		this.getNotifications(this.pageSize)
	}

	preUpdate() {
		this.GetListChucVuAndListPhongBan()
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

	onUpdate() {}

	signOut(): void {
		this.authenService.logOut({}).subscribe((success) => {
			if (success.success == RESPONSE_STATUS.success) {
				this.sharedataService.setIsLogin(false)
				this.storageService.setReturnUrl('')
				this.storageService.clear()
				this._router.navigate(['/dang-nhap'])
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

	// getNotification() {
	//   var request = {
	//     PageSize: this.pageSizeGrid
	//   }
	//   this.notificationService.GetNotification(request).subscribe(response => {
	//     if (response.status == 1) {
	//       this.listThongBao = [];
	//       this.listThongBao = response.listThongBao;
	//       this.totalThongBao = response.total;
	//     } else {
	//       this.toastr.error(response.message);
	//     }
	//   }), err => {
	//     console.error(err);
	//   }
	// }

	goLink(type, id) {
		if (type == 1 || type == 2 || type == 3) {
			this.router.navigate([`/business/business-management/view-meeting/${id}`])
		} else if (type == 4) {
			this.router.navigate([`/business/business-management/contact-view/${id}`])
		} else if (type == 5 || type == 6) {
			this.router.navigate([`/business/business-management/resolution/list/${id}`])
		} else if (type == 7 || type == 8) {
			this.router.navigate([`/business/business-management/letters-view/${id}`])
		} else if (type == 9 || type == 10 || type == 11) {
			this.router.navigate([`/business/business-management/monitoring/view/${id}`])
		} else if (type == 12) {
			this.router.navigate([`/business/business-management/feedback/meeting/view/${id}`])
		} else if (type == 15) {
			this.router.navigate([`/business/business-management/feedback/petition/view/${id}`])
		} else if (type == 16) {
			this.router.navigate([`/business/business-management/feedback/letters/view/${id}`])
		} else if (type == 13 || type == 14) {
			this.router.navigate([`/business/business-management/records-view/${id}`])
		} else if (type == 17) {
			this.sharedataService.setQuestionId(id)
			this.router.navigate(['/business/business-management/question/list'])
		} else if (type == 18 || type == 19 || type == 20) {
			this.router.navigate([`/business/business-management/session/view/${id}`])
		}
	}

	onScroll(event: any) {
		if (event.target.offsetHeight + event.target.scrollTop >= event.target.scrollHeight - 50) {
			this.numberNotifications = this.numberNotifications + 5
			this.getNotifications(this.numberNotifications)
		}
	}

	GetListChucVuAndListPhongBan() {}

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
	getList() {
		let req = {
			FromDate: this.dataSearch.fromDate == null ? '' : JSON.stringify(new Date(this.dataSearch.fromDate)).slice(1, 11),
			ToDate: this.dataSearch.toDate == null ? '' : JSON.stringify(new Date(this.dataSearch.toDate)).slice(1, 11),
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
	onExport() {
		$('#modalDetailLog').modal('hide')
		let passingObj: any = {}
		passingObj.UserId = this.storageService.getUserId()
		passingObj.UserProcessId = this.storageService.getUserId()
		passingObj.UserProcessName = this.storageService.getFullName()

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
}
