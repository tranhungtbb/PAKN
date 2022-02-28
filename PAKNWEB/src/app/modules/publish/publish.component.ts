import { Component, OnInit, OnChanges, ViewChild, ElementRef } from '@angular/core'
import { Router } from '@angular/router'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { RESPONSE_STATUS, TYPE_NOTIFICATION, RECOMMENDATION_STATUS, TYPECONFIG, FILETYPE } from 'src/app/constants/CONSTANTS'
import { AuthenticationService } from 'src/app/services/authentication.service'
import { DataService } from 'src/app/services/sharedata.service'
import { NotificationService } from 'src/app/services/notification.service'
import { IndexSettingService } from 'src/app/services/index-setting.service'
import { IndexSettingObjet } from 'src/app/models/indexSettingObject'
import * as signalR from '@aspnet/signalr/'
import { AppSettings } from 'src/app/constants/app-setting'
import { v4 as uuidv4 } from 'uuid'
import { ChatBotService } from '../chatbot/chatbot.service'
import { SystemconfigService } from 'src/app/services/systemconfig.service'
import { MetaService } from 'src/app/services/tag-meta.service'
import { RecommendationService } from 'src/app/services/recommendation.service'
import { ToastrService } from 'ngx-toastr'
import { UploadFileService } from 'src/app/services/uploadfiles.service'
import { saveAs as importedSaveAs } from 'file-saver'

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
		private botService: ChatBotService,
		private systemConfig: SystemconfigService,
		private metaService: MetaService,
		private recomenservice: RecommendationService,
		private toastr: ToastrService,
		private fileService: UploadFileService
	) { }

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
	fullName: string
	config: any = {}
	room: any = {}
	ngOnInit() {
		let splitRouter = this._router.url.split('/')
		if (splitRouter.length > 2) {
			this.activeUrl = splitRouter[2]
		}
		this.loadScript('assets/dist/vendor/bootstrap/js/bootstrap.min.js')
		// this.loadScript('assets/dist/js/sd-js.js')
		if (this.isLogin) {
			this.getListNotification(this.numberNotifications)
		}
		this.indexSettingService.GetInfo({}).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				this.indexSettingObj = res.result.model
				this.metaService.updateTitle(this.indexSettingObj.metaTitle)
				this.metaService.updateDescription(this.indexSettingObj.metaDescription)
			}
		}, (error) => {
			console.log(error)
		})

		this.systemConfig.syConfigGetByType({ Type: TYPECONFIG.APPLICATION }).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				this.config = JSON.parse(res.result.SYConfigGetByType.content)
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
			// { path: ['phan-anh-kien-nghi/sync/cong-dv-hcc-tinh-khoanh-hoa'], text: 'Cổng thông tin dịch vụ hành chính công trực tuyến tỉnh Khánh Hoà' },
			{ path: ['phan-anh-kien-nghi/sync/he-thong-cu-tri-khanh-hoa'], text: 'Hệ thống quản lý kiến nghị cử tri tỉnh Khánh Hoà' },
			{ path: ['phan-anh-kien-nghi/sync/he-thong-pakn-quoc-gia'], text: 'Hệ thống tiếp nhận, trả lời PAKN của Chính Phủ' },
		]

		this.handleInitConnectionToChatBot();
	}
	roomId: any
	async handleInitConnectionToChatBot() {
		this.myGuid = this.storageService.getClientUserId();
		this.fullName = this.storageService.getFullName() == null ? 'Người dân' : this.storageService.getFullName()
		if (!this.myGuid) {
			this.myGuid = uuidv4();
			this.storageService.setClientUserId(this.myGuid);
		}
		console.log('onConnectChatBot ', this.myGuid);
		if (!this.connection) {
			this.connection = new signalR.HubConnectionBuilder()
				.withUrl(`${AppSettings.SIGNALR_ADDRESS}?userName=${this.myGuid}&fullName=${this.fullName}`, {
					skipNegotiation: true,
					transport: signalR.HttpTransportType.WebSockets,
				})
				.withAutomaticReconnect()
				.build()

			const resConnect = await this.connection.start()
			const resCreate = await this.botService
				.createRoom({
					userName: this.myGuid,
				})
				.toPromise()
			if (resCreate.success === 'OK') {
				this.connection.invoke('JoinToRoom', resCreate.result.RoomName)
				this.roomId = resCreate.result.RoomId
				this.room = {
					Id: Number(resCreate.result.RoomId),
					AnonymousId: Number(resCreate.result.AnonymousId),
					Name: resCreate.result.RoomName,
					Title: resCreate.result.RoomTitle,
					Type: Number(resCreate.result.Type),
					CreatedDate: new Date()
				}
				if (resCreate.result.IsCreateRoom === "True") {
					this.connection.invoke('ReceiveRoomToGroup', this.room).then(res => {
						console.log('ReceiveRoomToGroup : ' + res)
					})
						.catch(err => {
							console.log('ReceiveRoomToGroup : ' + err)
						})
				}

				this.connection.on('ReceiveMessageToGroup', (data: any) => {
					console.log('ReceiveMessageToGroup ', data, this.myGuid);
					if (this.myGuid !== data.from) {
						this.loading = false

						let link = ''
						let answers = []
						let typeFrom;
						console.log('ReceiveMessageToGroup 1', data.results);
						if (data.results && data.results.length > 0) {
							console.log('ReceiveMessageToGroup 2', data.results);
							try {
								for (let index = 0; index < data.results.length; index++) {
									const element = data.results[index];
									if (element.subTags !== '') {
										const subTags = JSON.parse(element.subTags)
										answers.push({ answer: this.urlify(element.answer), subTags: subTags });
									}

								}
								console.log('answers ', answers);
							} catch (error) {
								console.log('answers ', error);
							}
						}

						const newMessage = {
							dateSent: data.timestamp,
							title: data.type != 'File' ? data.content : JSON.parse(data.content),
							type: data.type,
							answers: answers,
							link: link,
							fromUserName: data.from,
							fromAvatarPath: data.fromAvatarPath ? data.fromAvatarPath : '',
							fromFullName: data.fromFullName,
							toUserName: data.to,
							fromId: data.fromId
						}
						console.log('ReceiveMessageToGroup 3', newMessage);
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

			}
		}
	}


	redirectCreateRecommendation() {
		this._router.navigate(['/cong-bo/them-moi-kien-nghi'])
	}

	async onConnectChatBot() {
		this.sendMessage({ title: '', idSuggetLibrary: '' }, false)
	}

	sendMessage(message: any, append: boolean = true) {
		console.log('message ', message)
		if (message.typeSuggest && message.typeSuggest == "2" || message.typeSuggest && message.typeSuggest == "4") {
			window.open(message.linkSuggest, '_blank')
			return
		}
		else if (message.typeSuggest && message.typeSuggest == "3") {
			this.connection.invoke('NotifyAdmin', { ...this.room, 'CreatedDate': new Date() })
			this.messages = [
				...this.messages,
				{
					dateSent: '',
					title: message.linkSuggest,
					fromId: 19,
					fromAvatarPath: '',
					answers: []
				},
			]
			setTimeout(() => {
				var objDiv = document.getElementById('bodyMessage')
				objDiv.scrollTop = objDiv.scrollHeight
			}, 300)
			return
		}
		this.loading = true
		if (message.hiddenAnswer && message.hiddenAnswer !== '') {
			this.connection.invoke('AnonymousChatWithBot', message.title, message.idSuggetLibrary ? message.idSuggetLibrary : '', message.hiddenAnswer)
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
			this.connection.invoke('AnonymousChatWithBot', message.title, message.idSuggetLibrary ? message.idSuggetLibrary : '', '')
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

	onActivate(event) {
		window.scroll(0, 0)
		$('html, body')
			.animate({ scrollTop: 0 })
			.promise()
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
		// this.notificationService.updateIsViewedNotification({}).subscribe((res) => {
		// 	if (res.success == RESPONSE_STATUS.success) {
		// 	}
		// 	return
		// })
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
		this.ViewedCount = this.ViewedCount - 1
		this.updateIsReadNotification(id)
		if (type == TYPE_NOTIFICATION.NEWS) {
			this._router.navigate(['cong-bo/thong-bao-chinh-quyen/' + id])
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
			this.recomenservice.keySearchEvent.emit(this.keySearch);
			this._router.navigate(['/cong-bo/danh-sach-phan-anh-kien-nghi/0/', this.keySearch])
		}
	}
	checkDeny(status: any) {
		if (status == RECOMMENDATION_STATUS.PROCESS_DENY || status == RECOMMENDATION_STATUS.RECEIVE_DENY || status == RECOMMENDATION_STATUS.APPROVE_DENY) {
			return true
		}
		return false
	}

	showChatBot: boolean = false

	showHideMessage() {
		this.showChatBot = !this.showChatBot
		var message = document.getElementById('message')
		if (message) {
			if (message.classList.contains('show')) {
				message.classList.remove('show')
				message.style.display = 'none'
			} else {
				message.classList.add('show')
				message.style.display = 'block'
				this.onConnectChatBot()
			}
		}
	}

	ngOnDestroy() {
		if (this.connection) {
			this.connection.off('ReceiveMessageToGroup')
			this.connection.stop()
		}
	}

	onKeyDown(event) {
		//console.log(event)
		if (event.shiftKey && event.key === 'Enter') {
			var text = document.getElementById('type_msg')
			//  text.value += '\n';
		} else if (event.key === 'Enter') {
			event.preventDefault()
			this.onSend()
		}
	}

	onSend() {
		this.textMessage = this.textMessage == null ? '' : this.textMessage.trim()
		if (this.textMessage === '' && this.files.length == 0) {
			this.toastr.error('Vui lòng nhập câu hỏi của anh/chị!')
		}
		this.sendMessageToApi()
		this.sendFile()
	}

	sendMessageToApi() {
		if (this.textMessage === '') return
		const message = { title: this.textMessage }
		console.log('sendMessageToApi ', message)
		this.sendMessage(message, true)
		this.textMessage = null
	}

	goToLink(url: string) {
		window.open(url, '_blank')
	}
	urlify(text) {
		var urlRegex = /(https?:\/\/[^\s]+)/g;
		return text.replace(urlRegex, function (url) {
			return '<a target="_blank" href="' + url + '">' + url + '</a>';
		})
	}


	/// upload file 

	files: any[] = []
	@ViewChild('file', { static: false }) public file: ElementRef

	onUpload(event) {
		if (event.target.files.length == 0) {
			return
		}
		const check = this.fileService.checkFileWasExitsted(event, this.files)
		if (check === 1) {
			for (let item of event.target.files) {
				FILETYPE.forEach((fileType) => {
					if (item.type == fileType.text) {
						let max = this.files.reduce((a, b) => {
							return a.id > b.id ? a.id : b.id
						}, 0)
						item.id = max + 1
						item.fileType = fileType.value
						this.files.push(item)
					}
				})
				if (!item.fileType) {
					this.toastr.error('Định dạng không được hỗ trợ')
				}
			}
		} else if (check === 2) {
			this.toastr.error('Không được phép đẩy trùng tên file lên hệ thống')
		} else {
			this.toastr.error('File tải lên vượt quá dung lượng cho phép 10MB')
		}
		this.file.nativeElement.value = ''
	}

	onRemoveFile(index: number) {
		this.files.splice(index, 1)
	}


	sendFile() {
		if (this.files.length == 0) { return }
		let obj = {
			files: this.files,
			roomName: this.myGuid
		}

		this.botService.clientSendFile(obj).subscribe(res => {
			if (res.success == RESPONSE_STATUS.success) {

				setTimeout(() => {
					var objDiv = document.getElementById('bodyMessage')
					objDiv.scrollTop = objDiv.scrollHeight
				}, 300)


				this.file.nativeElement.value = ''
				this.files = []
			} else {
				this.toastr.error(res.message)
			}
		})

	}


	DownloadFile(file: any) {
		var request = {
			Path: file.FilePath,
			Name: file.Name,
		}
		this.fileService.downloadFile(request).subscribe(
			(response) => {
				var blob = new Blob([response], { type: response.type })
				importedSaveAs(blob, file.name)
			},
			(error) => {
				this.toastr.error('Không tìm thấy file trên hệ thống')
			}
		)
	}



}
