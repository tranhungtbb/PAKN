import { Component, OnInit, OnChanges } from '@angular/core'
import { Router } from '@angular/router'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { RESPONSE_STATUS, TYPE_NOTIFICATION, RECOMMENDATION_STATUS } from 'src/app/constants/CONSTANTS'
import { AuthenticationService } from 'src/app/services/authentication.service'
import { DataService } from 'src/app/services/sharedata.service'
import { NotificationService } from 'src/app/services/notification.service'
import { stat } from 'fs'
import { ChatbotService } from 'src/app/services/chatbot.service'
import { IndexSettingService } from 'src/app/services/index-setting.service'
import { IndexSettingObjet, IndexBanner, IndexWebsite } from 'src/app/models/indexSettingObject'

@Component({
	selector: 'app-publish',
	templateUrl: './publish.component.html',
	styleUrls: ['./publish.component.css'],
})
export class PublishComponent implements OnInit, OnChanges {
	constructor(
		private _router: Router,
		private storageService: UserInfoStorageService,
		private authenService: AuthenticationService,
		private sharedataService: DataService,
		private notificationService: NotificationService,
		private chatBotService: ChatbotService,
		private indexSettingService: IndexSettingService
	) {}

	activeUrl: string = ''
	isHasToken: boolean = this.storageService.getIsHaveToken()
	typeUserLoginPublish: number = this.storageService.getTypeObject()
	currentFullnName: string = this.storageService.getFullName()
	numberNotifications: any = 5
	notifications: any[]
	ViewedCount: number = 0
	index: number = 0
	routerHome = 'trang-chu'
	isLogin: boolean = this.storageService.getIsHaveToken()
	indexSettingObj: any = new IndexSettingObjet()

	ngOnInit() {
		let splitRouter = this._router.url.split('/')
		if (splitRouter.length > 2) {
			this.activeUrl = splitRouter[2]
		}

		//this.loadScript('assets/dist/vendor/bootstrap/is/bootstrap.min.js')
		//this.loadScript('assets/dist/vendor/chart.js/Chart.bundle.min.js')
		//this.loadScript('assets/dist/vendor/apexchart/apexchart.js')
		// this.loadScript('assets/dist/vendor/peity/jquery.peity.min.js')
		//this.loadScript('assets/dist/js/dashboard/dashboard-1.js')
		this.loadScript('assets/dist/js/owl.carousel.min.js')
		this.loadScript('assets/dist/js/sd-js.js')
		if (this.isLogin) {
			this.getListNotification(this.numberNotifications)
		}
		this.indexSettingService.GetInfo({}).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				this.indexSettingObj = res.result.model
				// this.lstIndexSettingBanner = res.result.lstIndexSettingBanner == null ? [] : res.result.lstIndexSettingBanner
				// this.ltsIndexSettingWebsite = res.result.lstSYIndexWebsite == null ? [] : res.result.lstSYIndexWebsite
			}
		}),
			(error) => {
				console.log(error)
				alert(error)
			}
	}

	getListNotification(PageSize: any) {
		this.ViewedCount = 0
		this.notificationService.getListNotificationOnPageByReceiveId({ PageSize: PageSize, PageIndex: 1 }).subscribe((res) => {
			if ((res.success = RESPONSE_STATUS.success)) {
				if (res.result.syNotifications.length > 0) {
					this.ViewedCount = res.result.syNotifications[0].viewedCount
					this.notifications = res.result.syNotifications
				} else {
					this.notifications = []
				}
			}
			return
		})
	}

	updateNotifications() {
		this.notificationService.updateIsViewedNotification({}).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				this.getListNotification(this.numberNotifications)
			}
			return
		})
	}

	ngOnChanges() {
		let splitRouter = this._router.url.split('/')
		if (splitRouter.length > 2) {
			if (splitRouter[2] != this.routerHome) {
				this.activeUrl = splitRouter[2]
			} else {
				this.activeUrl = 'n'
			}
		} else {
			this.activeUrl = ''
		}
	}
	routingMenu(pageRouting: string) {
		this.activeUrl = pageRouting
		this._router.navigate(['../cong-bo/' + pageRouting])
	}

	public loadScript(url: string) {
		$('script[src="' + url + '"]').remove()
		$('<script>').attr('src', url).appendTo('body')
	}

	signOut(): void {
		this.authenService.logOut({}).subscribe((success) => {
			if (success.success == RESPONSE_STATUS.success) {
				this.sharedataService.setIsLogin(false)
				this.storageService.setReturnUrl('')
				this.storageService.clearStoreage()
				this._router.navigate(['/dang-nhap'])
			}
		})
	}
	onClickNotification(id: number, type: number, typeSend: number) {
		if (type == TYPE_NOTIFICATION.NEWS) {
			this.updateIsReadNotification(id)
			this._router.navigate(['/tin-tuc-su-kien/' + id])
		} else if(type == TYPE_NOTIFICATION.RECOMMENDATION){
			if (typeSend == RECOMMENDATION_STATUS.FINISED) {
				this.updateIsReadNotification(id)
				this._router.navigate(['/cong-bo/phan-anh-kien-nghi/' + id])
			} else {
				this.updateIsReadNotification(id)
				this._router.navigate(['/cong-bo/chi-tiet-kien-nghi/' + id])
			}
		}else if (type == TYPE_NOTIFICATION.INVITATION) { // Thư mời
			this.updateIsReadNotification(id)
			this._router.navigate(['/quan-tri/thu-moi/chi-tiet/' + id])
		}
	}

	updateIsReadNotification(dataId: any) {
		this.notificationService.updateIsReadedNotification({ ObjectId: dataId }).subscribe()
		this.getListNotification(this.numberNotifications)
	}

	onScroll(event: any) {
		if (event.target.offsetHeight + event.target.scrollTop >= event.target.scrollHeight - 50) {
			this.numberNotifications = this.numberNotifications + 5
			this.getListNotification(this.numberNotifications)
		}
	}
	checkDeny(status: any) {
		if (status == RECOMMENDATION_STATUS.PROCESS_DENY || status == RECOMMENDATION_STATUS.RECEIVE_DENY || status == RECOMMENDATION_STATUS.APPROVE_DENY) {
			return true
		}
		return false
	}
}
