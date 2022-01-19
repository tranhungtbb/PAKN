import { Component, OnInit, AfterViewInit, OnDestroy } from '@angular/core'
// declare var jquery: any;
declare var $: any
import { BsDatepickerConfig, BsLocaleService } from 'ngx-bootstrap/datepicker'
import { defineLocale } from 'ngx-bootstrap/chronos'
import { viLocale } from 'ngx-bootstrap/locale'
import { UserInfoStorageService } from '../commons/user-info-storage.service'
import { SocketService } from '../services/socket.service'
import { Router, NavigationEnd } from '@angular/router'
import { filter } from 'rxjs/operators'
import { StoreLink } from '../constants/store-link'
import { environment } from '../../environments/environment'
import { MetaService } from '../services/tag-meta.service'
import { IndexSettingService } from '../services/index-setting.service'
import { RESPONSE_STATUS } from '../constants/CONSTANTS'
import * as signalR from '@aspnet/signalr/'
import { AppSettings } from '../constants/app-setting'
import { BotService } from '../services/bot.service'
import { ChatBotService } from './chatbot/chatbot.service'
import { ToastrService } from 'ngx-toastr'

defineLocale('vi', viLocale)

declare var $: any
@Component({
	selector: 'app-business',
	templateUrl: './business.component.html',
	styleUrls: ['./business.component.css'],
})
export class BusinessComponent implements OnInit, AfterViewInit, OnDestroy {
	userId: number
	url: string = ''
	currentRouter: string = ''
	isMain: any = this.userInfoService.getIsMain()
	connection: signalR.HubConnection
	audio: any
	roomsShow: any = []

	checkPermissionChatbot: boolean = false

	constructor(
		private localeService: BsLocaleService,
		public userInfoService: UserInfoStorageService,
		public socketService: SocketService,
		private _router: Router,
		private metaService: MetaService,
		private indexSetting: IndexSettingService,
		private botService: ChatBotService,
		private toas: ToastrService,
	) {
		this._router.events
			.pipe(filter((event) => event instanceof NavigationEnd))
			.pairwise()
			.subscribe((e: any[]) => {
				if (e != undefined && e != null && e.length > 1) {
					var linkolder = environment.olderbacklink
					var backlink = false
					var currentlink = false

					for (let sp of StoreLink.ListBackLink) {
						if (e[0].url.includes(sp)) {
							backlink = true
						}
						if (e[1].url.includes(sp)) {
							currentlink = true
						}
					}
					if ((backlink && currentlink) || (environment.olderbacklink !== e[1].url && currentlink)) {
					}
					environment.olderbacklink = e[0].url
				}
				if (this.checkPermissionChatbot) {
					this.checkUrlChatBot()
					this.getRooms()
				}
			})
	}


	async ngOnInit() {
		let arrPermission = this.userInfoService.getPermissions().split(',')
		if (arrPermission.includes('A_XI_6')) {
			this.checkPermissionChatbot = true;
		}
		this.checkUrlChatBot()
		this.localeService.use('vi')
		this.userInfoService.setReturnUrl('')
		this.indexSetting.GetInfo({}).subscribe(res => {
			if (res.success == RESPONSE_STATUS.success) {
				let settingModel = res.result.model
				this.metaService.updateTitle(settingModel.metaTitle)
				this.metaService.updateDescription(settingModel.metaDescription)
			}
		})

		/// 

		if (this.checkPermissionChatbot) {
			this.audio = new Audio()
			this.audio.src = '../../../assets/img/ring.mp3'
			this.audio.loop = true


			await this.getRooms()

			this.connection = new signalR.HubConnectionBuilder()
				.withUrl(`${AppSettings.SIGNALR_ADDRESS}?sysUserName=${this.userId}`, {
					skipNegotiation: true,
					transport: signalR.HttpTransportType.WebSockets,
				})
				.configureLogging(signalR.LogLevel.Information)
				.withAutomaticReconnect()
				.build()
			this.connection.start().then(() => {
				this.connection.on('NotifyAdmin', (data: any) => {
					console.log('ngOnInit SignalR NotifyAdmin ', data)
					let room = this.roomsShow.find(x => x.name === data.name)
					if (!room) {
						this.roomsShow.unshift(data)
					}
					this.playSoundWarning()
				})
			})
		}
	}

	getRooms = async () => {
		await this.botService.getRoomForNotification({}).toPromise().then(res => {
			if (res.success == RESPONSE_STATUS.success) {

				this.roomsShow = res.result.ListRoomIsShow
			} else {
				this.toas.error(res.result.message)
			}
		}).catch(err => {
			this.toas.error(err)
		})
	}

	playSoundWarning() {
		try {
			console.log('playSoundWarning ')
			this.audio = new Audio()
			this.audio.src = '../../assets/img/ring.mp3'
			this.audio.load()
			this.audio.play()
		} catch (error) {
			console.log('playSoundWarning error', error)
		}
	}

	updateStatus(data: any, selectedRoom: boolean = false) {
		let index = this.roomsShow.indexOf(data);
		this.roomsShow.splice(index, 1)
		this.botService.updateStatusRoom({ roomId: data.id }).subscribe()
		if (selectedRoom) {
			this._router.navigate(['/quan-tri/chat-bot', data.id])
		}
	}
	isUrlChatBot: boolean = false
	checkUrlChatBot() {
		let currentlink = this._router.url
		if (currentlink.includes('quan-tri/chat-bot')) {
			this.isUrlChatBot = true
		}
		else {
			this.isUrlChatBot = false
		}

	}



	ngAfterViewInit() {
		this.currentRouter = this._router.url
		setTimeout(() => {
			var widthMenu = $(".header-right").width();
			var css = '.group-button-table { right: ' + (widthMenu + 25) + 'px; }',
				head = document.head || document.getElementsByTagName('head')[0],
				style = document.createElement('style');

			head.appendChild(style);
			style.appendChild(document.createTextNode(css));
		}, 500)
	}

	ngOnDestroy() {
		if (this.connection) {
			console.log('SignalR ngOnDestroy 0')
			this.connection.off('NotifyAdmin')
		}
	}


	public loadScript(url: string) {
		$('script[src="' + url + '"]').remove()
		$('<script>').attr('src', url).appendTo('body')
	}
}
