import { Component, ElementRef, OnInit, ViewChild } from '@angular/core'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { BotMessage, BotRoom } from 'src/app/models/chatbotObject'
import { ChatBotService } from './chatbot.service'
import * as signalR from '@aspnet/signalr/'
import { AppSettings } from 'src/app/constants/app-setting'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { UserService } from 'src/app/services/user.service'
import { ToastrService } from 'ngx-toastr'

@Component({
	selector: 'app-dashboard',
	templateUrl: './chatbot.component.html',
	styleUrls: ['./chatbot.component.css'],
})
export class DashboardChatBotComponent implements OnInit {
	channel: any
	roomNameSelected: string
	messages: any = []
	newMessage = ''
	rooms: BotRoom[]
	roomsShow: any[] = []
	connection: signalR.HubConnection
	pageIndex: number = 1
	pageSize: number = 10
	totalMessage: number = 0
	roomActive: number = 0
	userId: number
	model: any = {}
	userAvatar: string
	audio: any

	@ViewChild('boxChat', { static: true }) private boxChat: ElementRef

	constructor(private botService: ChatBotService, private userService: UserService, private user: UserInfoStorageService, private toast: ToastrService) { }
	ngOnInit() {
		//console.log('ngOnInit 0')

		this.audio = new Audio()
		this.audio.src = '../../../assets/img/ring.mp3'
		this.audio.loop = true
		this.userId = this.user.getUserId()
		this.userService.getById({ id: this.userId }).subscribe((res) => {
			this.model = res.result.SYUserGetByID[0]
			//console.log('userService ', this.model);
			if (this.model.avatar == '' || this.model.avatar == null) {
				this.userAvatar = ''
			} else {
				this.userAvatar = this.model.avatar
			}
		})
		this.connection = new signalR.HubConnectionBuilder()
			.withUrl(`${AppSettings.SIGNALR_ADDRESS}?sysUserName=${this.userId}`, {
				skipNegotiation: true,
				transport: signalR.HttpTransportType.WebSockets,
			})
			.configureLogging(signalR.LogLevel.Information)
			.withAutomaticReconnect()
			.build()
		this.connection.start().then(() => {
			this.connection.on('ReceiveMessageToGroup', (data: any) => {
				console.log('ngOnInit SignalR ReceiveMessageToGroup 1', data, this.roomNameSelected, this.userId)
				if (data.type === 'Conversation' && this.roomNameSelected && this.roomNameSelected === data.to && `${this.userId}` !== data.from) {

					const answers = [];
					if (data.results) {
						console.log('answers 2', data.results)
						for (let ind = 0; ind < data.results.length; ind++) {
							const el = data.results[ind]
							if (el.subTags !== '') {
								const subTags = JSON.parse(el.subTags)
								console.log('answers 3', subTags)
								answers.push({ answer: el.answer, subTags: subTags })
							}
						}
					}

					console.log('answers 4', answers)
					this.messages = [...this.messages, { messageContent: data.content, fromAvatar: data.fromAvatar, fromFullName: data.fromFullName, answers }]

					console.log('ngOnInit SignalR ReceiveMessageToGroup 2', this.messages)
				}

				this.convertMessageToObjectList()
			})

			this.connection.on('ReceiveRoomToGroup', data => {
				debugger
				this.rooms.unshift(data)
			})

			this.connection.on('BroadcastMessage', (data: any) => {
				//console.log('ngOnInit SignalR BroadcastMessage ', data)

				this.fetchRooms()
			})
			this.connection.on('NotifyAdmin', (data: any) => {
				//console.log('ngOnInit SignalR BroadcastMessage ', data)

				this.playSoundWarning()
			})
		})
		this.fetchRooms()
	}

	playSoundWarning() {
		try {
			console.log('playSoundWarning ')
			this.audio = new Audio()
			this.audio.src = '../../../assets/img/ring.mp3'
			this.audio.load()
			this.audio.play()
		} catch (error) {
			console.log('playSoundWarning error', error)
		}
	}

	fetchRooms = async () => {
		try {
			//console.log('fetchRooms 1')
			this.botService.getRooms({}).subscribe((res) => {
				if (res != 'undefined' && res.success == RESPONSE_STATUS.success) {
					if (res.result) {
						console.log('fetchRooms ', res.result.Data)
						this.rooms = res.result.Data
						this.roomsShow = res.result.ListRoomIsShow
					}
				} else {
					//this._toastr.error(res.message)
				}
			})
		} catch (error) {
			//console.log('handleConnect ', error)
		}
	}

	resetGetMessage(roomId: number, roomName: string) {
		this.roomActive = roomId
		this.pageIndex = 1
		this.roomNameSelected = roomName
		this.getMessage(roomId, roomName)
	}
	async getMessage(roomId: number, roomName: string) {
		try {
			const request = {
				RoomId: roomId,
				PageIndex: this.pageIndex,
				PageSize: this.pageSize,
			}
			console.log('getMessage ', roomName)
			this.connection.invoke('JoinToRoom', roomName)
			this.botService.getMessages(request).subscribe((result) => {
				console.log('getMessage 1', result)
				let res = { ...result }

				if (res != 'undefined' && res.success == RESPONSE_STATUS.success) {
					if (res.result && res.result.length > 0) {
						if (this.pageIndex == 1) {
							this.messages = res.result.reverse()
						} else {
							this.messages = [res.result, ...this.messages].reverse()
						}
						if (this.messages != null && this.messages.length > 0) {
							this.totalMessage = this.messages[0].rowNumber
						}

						this.convertMessageToObjectList()
						this.scrollToBottom()
					}
				} else {
				}
			})
		} catch (error) {
			console.log('handleConnect ', error)
		}
	}

	scrollToBottom(): void {
		try {
			//console.log('height scroll:' + this.boxChat.nativeElement.scrollHeight)
			this.boxChat.nativeElement.scrollTop = this.boxChat.nativeElement.scrollHeight
		} catch (err) { }
	}

	sendMessage() {
		if (this.newMessage !== '') {
			//	this.playSoundWarning();
			console.log('sendMessage ', this.roomNameSelected, this.rooms)
			this.connection.invoke('AdminSendToRoom', this.roomNameSelected, this.newMessage)

			if (this.rooms.filter((room) => room.name === this.roomNameSelected).length > 0) {
				this.rooms.find((room) => room.name === this.roomNameSelected).type = 2
			}

			this.messages = [...this.messages, { messageContent: this.newMessage, fromUserId: this.userId, fromAvatar: this.userAvatar }]
			this.newMessage = ''

			//	this.audio.stop();
		}
	}

	onKeyDown(event) {
		//console.log(event)
		if (event.shiftKey && event.key === 'Enter') {
			var text = document.getElementById('type_msg')
			//  text.value += '\n';
		} else if (event.key === 'Enter') {
			event.preventDefault()
			//console.log(this.newMessage)
			this.sendMessage()
		}
	}
	ngOnDestroy() {
		if (this.connection) {
			console.log('SignalR ngOnDestroy 0')
			this.connection.off('ReceiveMessageToGroup')
			this.connection.off('BroadcastMessage')
		}
	}

	changeRoomStatus(status: boolean) {
		console.log('changeRoomStatus ', this.roomNameSelected, status)
		this.connection.invoke('EnableBot', this.roomNameSelected, status)
	}

	enabledBot() {
		this.changeRoomStatus(true)
	}

	disabedBot() {
		this.changeRoomStatus(false)
	}

	onScrollBoxChat(event: any) {
		// if (event.target.scrollTop == 0) {
		// 	if (this.pageIndex * this.pageSize < this.totalMessage) {
		// 		this.pageIndex++
		// 		this.getMessage(this.roomActive, this.roomNameSelected)
		// 	}
		// }
	}

	convertMessageToObjectList() {
		try {
			if (this.messages) {
				for (let index = 0; index < this.messages.length; index++) {
					const element = this.messages[index]
					let result, type

					if (element.xresults) {
						result = element.xresults
						type = 'json'
					} else {
						const rs = this.stringToObject(element.messageContent)
						result = rs.result
						type = rs.type
					}

					element.fromAvatar = element.fromAvatar ? element.fromAvatar : ''
					element.fromFullName = element.fromFullName ? element.fromFullName : ''
					const answers = []

					if (type === 'string') {
						element.messageContent = result
					} else if (type === 'json') {
						console.log('answers ', result)
						if (result.Results && result.Results.length > 0) {
							console.log('answers 1', result.Results)
							try {
								for (let ind = 0; ind < result.Results.length; ind++) {
									const el = result.Results[ind]
									if (el.SubTags !== '') {
										const subTags = JSON.parse(el.SubTags)
										console.log('answers 2', subTags)
										answers.push({ answer: el.answer, subTags: subTags })
									}
								}
							} catch (error) { }
						}
						element.messageContent = ''
					}
					element.answers = answers
				}
			}
			console.log('ReceiveMessageToGroup 2', this.messages)
		} catch (error) { }
	}

	stringToObject(string) {
		if (string) {
			try {
				const result = JSON.parse(string)
				return { result: result, type: 'json' }
			} catch (error) {
				return { result: string, type: 'string' }
			}
		}
	}

	updateStatus(data: any, selectedRoom: boolean = false) {
		let index = this.roomsShow.indexOf(data);
		this.roomsShow.splice(index, 1)
		this.botService.updateStatusRoom({ roomId: data.id }).subscribe()
		if (selectedRoom) {
			this.resetGetMessage(data.id, data.name)
		}
	}
}
