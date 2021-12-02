import { Component, OnInit, OnChanges } from '@angular/core'
import { Router } from '@angular/router'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { RESPONSE_STATUS, TYPE_NOTIFICATION, RECOMMENDATION_STATUS } from 'src/app/constants/CONSTANTS'
import { AuthenticationService } from 'src/app/services/authentication.service'
import { DataService } from 'src/app/services/sharedata.service'
import { NotificationService } from 'src/app/services/notification.service'
import { IndexSettingService } from 'src/app/services/index-setting.service'
import { IndexSettingObjet } from 'src/app/models/indexSettingObject'
import * as signalR from '@aspnet/signalr/'
import { AppSettings } from 'src/app/constants/app-setting'
import { v4 as uuidv4 } from 'uuid'
import { ChatBotService } from '../chatbot/chatbot.service'
import { BotMessage } from 'src/app/models/chatbotObject'
import { link } from 'fs'

declare var $: any

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
		private indexSettingService: IndexSettingService,
		private botService: ChatBotService
	) {}

	activeUrl: string = ''
	isHasToken: boolean = this.storageService.getIsHaveToken()
	typeUserLoginPublish: number = this.storageService.getTypeObject()
	currentFullnName: string = this.storageService.getFullName()
	numberNotifications: any = 10
	notifications: any[]
	ViewedCount: number = 0
	index: number = 0
	routerHome = 'trang-chu'
	isLogin: boolean = this.storageService.getIsHaveToken()
	indexSettingObj: any = new IndexSettingObjet()
	connection: signalR.HubConnection
	subMenu: any[] = []
	textMessage = null
	messages: any[] = []
	loading: boolean
	myGuid: string
	ngOnInit() {
		let splitRouter = this._router.url.split('/')
		if (splitRouter.length > 2) {
			this.activeUrl = splitRouter[2]
		}
		// this.loadScript('assets/dist/js/owl.carousel.min.js')
		// this.loadScript('assets/dist/js/sd-js.js')
		if (this.isLogin) {
			this.getListNotification(this.numberNotifications)
		}
		this.indexSettingService.GetInfo({}).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				this.indexSettingObj = res.result.model
			}
		}),
			(error) => {
				console.log(error)
				alert(error)
			}

		console.log('receiveMessage 3', this.messages)
		this.subMenu = [
			{ path: ['phan-anh-kien-nghi/da-tra-loi'], text: 'Phản ánh- kiến nghị đã trả lời' },
			// { path: ['phan-anh-kien-nghi/sync/cong-ttdt-tinh-khanh-hoa'], text: 'Cổng thông tin điện tử tỉnh Khánh Hoà' },
			{ path: ['phan-anh-kien-nghi/sync/cong-dv-hcc-tinh-khoanh-hoa'], text: 'Cổng thông tin dịch vụ hành chính công trực tuyến tỉnh Khánh Hoà' },
			{ path: ['phan-anh-kien-nghi/sync/he-thong-cu-tri-khanh-hoa'], text: 'Hệ thống quản lý kiến nghị cử tri tỉnh Khánh Hoà' },
			// { path: ['phan-anh-kien-nghi/sync/he-thong-pakn-quoc-gia'], text: 'Hệ thống tiếp nhận, trả lời PAKN của Chính Phủ' },
		]
	}

	async onConnectChatBot() {
		this.myGuid = uuidv4()
		this.connection = new signalR.HubConnectionBuilder()
			.withUrl(`${AppSettings.SIGNALR_ADDRESS}?userName=${this.myGuid}`, {
				skipNegotiation: true,
				transport: signalR.HttpTransportType.WebSockets,
			})
			.withAutomaticReconnect()
			.build()
		this.connection.serverTimeoutInMilliseconds = 180000
		this.connection.keepAliveIntervalInMilliseconds = 180000

		const resConnect = await this.connection.start()
		const resCreate = await this.botService
			.createRoom({
				userName: this.myGuid,
			})
			.toPromise()
		//console.log('resCreate ', resCreate)
		if (resCreate.success === 'OK') {
			this.connection.invoke('JoinToRoom', resCreate.result.RoomName)
			this.connection.on('ReceiveMessageToGroup', (data: any) => {
				if (this.myGuid !== data.from) {
					this.loading = false
					console.log('receiveMessage 0', this.messages, data)

					let link = ''
					let subTags
					let typeFrom
					if (data.subTags && data.subTags.length > 0) {
						const par = JSON.parse(data.subTags)
						typeFrom = par.type
						if (par.type === 'carousel') {
							subTags = par.data
						}
					}

					const newMessage = {
						dateSent: data.timestamp,
						title: data.content,
						type: typeFrom,
						subTags: subTags,
						link: link,
						fromUserName: data.from,
						toUserName: data.to,
					}

					if (this.messages) {
						this.messages = [...this.messages, newMessage]
					} else {
						this.messages = [newMessage]
					}
					setTimeout(() => {
						var objDiv = document.getElementById('bodyMessage')
						objDiv.scrollTop = objDiv.scrollHeight
					}, 300)
				}
			})
			this.sendMessage({ title: 'Xin chào' }, false)
		}
	}

	sendMessage(message: any, append: boolean = true) {
		console.log('message ', message)
		this.loading = true
		if (message.hiddenAnswer && message.hiddenAnswer !== '') {
			this.connection.invoke('AnonymousChatWithBot', message.title, message.hiddenAnswer)
			if (append) {
				this.messages = [
					...this.messages,
					{
						dateSent: '',
						title: message.title,
						fromId: 0,
					},
				]
				setTimeout(() => {
					var objDiv = document.getElementById('bodyMessage')
					objDiv.scrollTop = objDiv.scrollHeight
				}, 300)
			}
		} else {
			this.connection.invoke('AnonymousChatWithBot', message.title, '')
			if (append) {
				this.messages = [
					...this.messages,
					{
						dateSent: '',
						title: message.title,
						fromId: 0,
					},
				]
				setTimeout(() => {
					var objDiv = document.getElementById('bodyMessage')
					objDiv.scrollTop = objDiv.scrollHeight
				}, 300)
			}
		}
	}

	onDisconnectChatBot() {
		if (this.connection) {
			this.connection.off('ReceiveMessageToGroup')
			this.connection.stop()
		}
	}

	onActivate(event) {
		window.scroll(0, 0)
	}

	getListNotification(PageSize: any) {
		this.ViewedCount = 0
		this.notificationService.getListNotificationOnPageByReceiveId({ PageSize: PageSize, PageIndex: 1 }).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				if (res.result.syNotifications.length > 0) {
					this.ViewedCount = res.result.syNotifications[0].viewedCount
					this.viewedCountLate = res.result.syNotifications[0].viewedCount
					this.notifications = res.result.syNotifications
				} else {
					this.notifications = []
				}
			}
			return
		})
	}
	viewedCountLate: number = 0
	updateNotifications() {
		this.viewedCountLate = 0
		this.notificationService.updateIsViewedNotification({}).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
			}
			return
		})
	}

	ngOnChanges() {
		this.keySearch = null
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
				this.storageService.clear()
				this._router.navigate(['/dang-nhap'])
			}
		})
	}
	onClickNotification(id: number, type: number, typeSend: number) {
		this.updateIsReadNotification(id)
		if (type == TYPE_NOTIFICATION.NEWS) {
			this._router.navigate(['cong-bo/tin-tuc-su-kien/' + id])
		} else if (type == TYPE_NOTIFICATION.RECOMMENDATION) {
			if (typeSend == RECOMMENDATION_STATUS.FINISED) {
				this._router.navigate(['/cong-bo/phan-anh-kien-nghi/' + id])
			} else {
				this._router.navigate(['/cong-bo/chi-tiet-kien-nghi/' + id])
			}
		} else if (type == TYPE_NOTIFICATION.INVITATION) {
			this._router.navigate(['/quan-tri/thu-moi/chi-tiet/' + id])
		} else if (type == TYPE_NOTIFICATION.ADMINISTRATIVE) {
			this._router.navigate(['/quan-tri/thu-tuc-hanh-chinh/chi-tiet/' + id])
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
	keySearch: any
	searchRecommendation() {
		this.keySearch = this.keySearch == null ? '' : this.keySearch.trim()
		if (this.keySearch) {
			this._router.navigate(['/cong-bo/danh-sach-phan-anh-kien-nghi/0/', this.keySearch])
		}
	}
	checkDeny(status: any) {
		if (status == RECOMMENDATION_STATUS.PROCESS_DENY || status == RECOMMENDATION_STATUS.RECEIVE_DENY || status == RECOMMENDATION_STATUS.APPROVE_DENY) {
			return true
		}
		return false
	}

	showHideMessage() {
		var message = document.getElementById('message')
		if (message) {
			if (message.classList.contains('show')) {
				message.classList.remove('show')
				message.style.display = 'none'
				this.onDisconnectChatBot()
			} else {
				message.classList.add('show')
				message.style.display = 'block'
				this.onConnectChatBot()
			}
		}
	}

	onKeyDown(event) {
		//console.log(event)
		if (event.shiftKey && event.key === 'Enter') {
			var text = document.getElementById('type_msg')
			//  text.value += '\n';
		} else if (event.key === 'Enter') {
			event.preventDefault()
			console.log(this.textMessage)
			this.sendMessageToApi()
		}
	}

	onSend() {
		this.sendMessageToApi()
	}

	sendMessageToApi() {
		if (this.textMessage) {
			this.textMessage = this.textMessage.trim()
		}
		const message = { title: this.textMessage }
		console.log('sendMessageToApi ', message)
		this.sendMessage(message, true)
		this.textMessage = null
	}

	goToLink(url: string) {
		window.open(url, '_blank')
	}
}
