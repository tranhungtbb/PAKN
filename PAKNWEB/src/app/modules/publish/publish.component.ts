import { Component, OnInit, OnChanges } from '@angular/core'
import { Router } from '@angular/router'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { RESPONSE_STATUS, TYPE_NOTIFICATION, RECOMMENDATION_STATUS } from 'src/app/constants/CONSTANTS'
import { AuthenticationService } from 'src/app/services/authentication.service'
import { DataService } from 'src/app/services/sharedata.service'
import { NotificationService } from 'src/app/services/notification.service'
import { stat } from 'fs'

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
		private notificationService: NotificationService
	) {}

	activeUrl: string = ''
	isHasToken: boolean = this.storageService.getIsHaveToken()
	typeUserLoginPublish: number = this.storageService.getTypeObject()

	notifications: any[]
	ViewedCount: number = 0

	ngOnInit() {
		let splitRouter = this._router.url.split('/')
		if (splitRouter.length > 2) {
			this.activeUrl = splitRouter[2]
		}

		// this.loadScript('assets/dist/vendor/bootstrap-select/dist/js/bootstrap-select.min.js')
		this.loadScript('assets/dist/vendor/bootstrap/is/bootstrap.min.js')
		this.loadScript('assets/dist/vendor/chart.js/Chart.bundle.min.js')
		// this.loadScript('assets/dist/js/custom.min.js')
		// this.loadScript('assets/dist/js/deznav-init.js')
		// this.loadScript('assets/dist/vendor/waypoints/jquery.waypoints.min.js')
		// this.loadScript('assets/dist/vendor/jquery.counterup/jquery.counterup.min.js')
		// this.loadScript('assets/dist/js/plugins-init/piety-init.js')
		this.loadScript('assets/dist/vendor/apexchart/apexchart.js')
		// this.loadScript('assets/dist/vendor/peity/jquery.peity.min.js')
		this.loadScript('assets/dist/js/dashboard/dashboard-1.js')
		this.loadScript('assets/dist/js/owl.carousel.min.js')
		this.loadScript('assets/dist/js/sd-js.js')

		this.notificationService.getListNotificationOnPageByReceiveId({ PageSize: 5, PageIndex: 1 }).subscribe((res) => {
			if ((res.success = RESPONSE_STATUS.success)) {
				this.notifications = res.result.syNotifications
				this.notifications.forEach((item) => {
					if (item.isViewed == true) {
						this.ViewedCount += 1
					}
				})
			}
			return
		})
	}
	ngOnChanges() {
		let splitRouter = this._router.url.split('/')
		if (splitRouter.length > 2) {
			this.activeUrl = splitRouter[2]
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
				//location.href = "/dang-nhap";
			}
		})
	}
	onClickNotification(id: number, type: number, typeSend: number) {
		if (type == TYPE_NOTIFICATION.NEWS) {
			this._router.navigate(['/tin-tuc-su-kien/' + id])
		} else {
			if (typeSend == RECOMMENDATION_STATUS.FINISED) {
				this._router.navigate(['/cong-bo/phan-anh-kien-nghi/' + id])
			} else {
				return
			}
		}
	}
	checkDeny(status: any) {
		if (status == RECOMMENDATION_STATUS.PROCESS_DENY || status == RECOMMENDATION_STATUS.RECEIVE_DENY || status == RECOMMENDATION_STATUS.APPROVE_DENY) {
			return true
		}
		return false
	}
}
