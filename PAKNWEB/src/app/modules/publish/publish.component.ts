import { Component, OnInit, OnChanges } from '@angular/core'
import { Router } from '@angular/router'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { RESPONSE_STATUS, TYPE_NOTIFICATION, RECOMMENDATION_STATUS } from 'src/app/constants/CONSTANTS'
import { AuthenticationService } from 'src/app/services/authentication.service'
import { DataService } from 'src/app/services/sharedata.service'
import { NotificationService } from 'src/app/services/notification.service'
import { stat } from 'fs'
import { ChatbotService } from 'src/app/services/chatbot.service'

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
		private chatBotService: ChatbotService
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
	ngOnInit() {
		let splitRouter = this._router.url.split('/')
		if (splitRouter.length > 2) {
			this.activeUrl = splitRouter[2]
		}

		//this.loadScript('assets/dist/vendor/bootstrap/is/bootstrap.min.js')
		//this.loadScript('assets/dist/vendor/chart.js/Chart.bundle.min.js')
		this.loadScript('assets/dist/vendor/apexchart/apexchart.js')
		// this.loadScript('assets/dist/vendor/peity/jquery.peity.min.js')
		this.loadScript('assets/dist/js/dashboard/dashboard-1.js')
		this.loadScript('assets/dist/js/owl.carousel.min.js')
		this.loadScript('assets/dist/js/sd-js.js')

		this.getListNotification(this.numberNotifications)
	}

	botname: string = 'Bot'
	message: string = ''
	messages: any = [
		{
			who: this.botname,
			isReply: true,
			message: 'Vui lòng nhập câu hỏi...',
		},
	]

	getListNotification(PageSize: any) {
		this.notificationService.getListNotificationOnPageByReceiveId({ PageSize: PageSize, PageIndex: 1 }).subscribe((res) => {
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

	updateNotifications() {
		if (this.index == 0) {
			this.notificationService.updateIsViewedNotification({}).subscribe((res) => {
				if (res.success == RESPONSE_STATUS.success) {
					this.index = this.index + 1
					this.getListNotification(this.numberNotifications)
				}
				return
			})
		}
	}

	ngOnChanges() {
		debugger
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
				this._router.navigate(['/cong-bo/chi-tiet-kien-nghi/' + id])
			}
		}
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

	/*================ CHAT BOT ===============*/
	toggleChatForm() {
		let formChat = document.getElementById('form-chat')
		formChat.classList.toggle('togger-show-chatbot')

		let btnChat = document.getElementById('btn-chat')
		btnChat.classList.toggle('togger-show-chatbot')
	}
	// Send question to server chatbot
	send() {
		if (this.message.trim() === '') {
			return
		}

		this.messages.push({
			who: 'Tôi',
			isReply: false,
			message: this.message,
		})

		// get userID
		let kluid = localStorage.getItem('kluid')

		// not exist userid
		if (!kluid) {
			this.chatBotService.getNewUserId().subscribe(
				(data) => {
					let newUs = null
					newUs = data
					if (newUs.UserID) {
						localStorage.setItem('kluid', newUs.UserID)
						this.sendToServer()
					}
				},
				(error) => {
					console.log(error)
				}
			)
		} else {
			this.sendToServer()
		}
	}

	async sendToServer() {
		let kluid = localStorage.getItem('kluid')

		const data = {
			Sentence: this.message,
		}

		this.chatBotService.sendToServer(kluid, data).subscribe(
			(response) => {
				let res = null
				res = response
				this.messages.push({
					who: this.botname,
					isReply: true,
					message: res.ResponseText.toString(),
				})

				let userId = localStorage.getItem('userId')
				userId = userId === '' || userId === null ? null : userId
				let fullName = this.currentFullnName === '' || this.currentFullnName === null ? '' : this.currentFullnName

				const dataChatbot = {
					kluid: kluid,
					userId: userId,
					fullName: fullName,
					question: this.message,
					answer: res.ResponseText.toString(),
				}

				this.insertDataChatBot(dataChatbot)
				this.message = ''

				document.getElementById('messages-content').style.overflow = 'scroll'
				setTimeout(function () {
					document.getElementById('messages-content').scrollTo(0, 1000)
				}, 500)
			},
			(error) => {
				console.log(error)
			}
		)
	}
	insertDataChatBot(obj: any) {
		this.chatBotService.chatbotInsertData(obj).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result == -1) {
					return
				} else {
				}
			} else {
			}
		})
	}
	/*================ CHAT BOT ===============*/
}
