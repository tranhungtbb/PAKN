import { Component, ElementRef, OnInit, ViewChild } from '@angular/core'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { BotMessage, BotRoom } from 'src/app/models/chatbotObject'
import { ChatBotService } from './chatbot.service'
import * as signalR from '@aspnet/signalr/'
import { AppSettings } from 'src/app/constants/app-setting'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { UserService } from 'src/app/services/user.service'
// import playAlert from 'alert-sound-notify'
//playAlert = require('alert-sound-notify')


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
	connection: signalR.HubConnection
	pageIndex: number = 1
	pageSize: number = 10
	totalMessage: number = 0
	roomActive: number = 0
	userId: number
	model: any = {}
	userAvatar: string

	@ViewChild('boxChat', { static: true }) private boxChat: ElementRef

	constructor(private botService: ChatBotService, private userService: UserService, private user: UserInfoStorageService) { }
	ngOnInit() {

		// playAlert.content['foo'] = ['../../../assets/img/ring.mp3']
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


					// 				answers: [{…}]
					// dateSend: "2021-12-22T15:31:15.247+07:00"
					// fromAvatar: ""
					// fromFullName: ""
					// fromUserId: 0
					// id: 26232
					// messageContent: ""
					// roomId: 21031
					// rowNumber: 12

					// content: null
					// from: "Bot"
					// fromAvatarPath: null
					// fromFullName: "Bot"
					// fromId: "Bot"
					// hiddenAnswer: "alo"
					// results: [{…}]
					// subTags: null
					// timestamp: "1640161884"
					// to: "Room_24f03130-73c9-4256-aa48-09448126851e"
					// type: "Conversation"

					//this.convertMessageToObjectList()


					console.log('ngOnInit SignalR ReceiveMessageToGroup 2', this.messages)
				}

				this.convertMessageToObjectList()
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
			// playAlert('foo');
		} catch (error) {
			console.log('playSoundWarning error', error)
		}
	}

	fetchRooms = async () => {
		//console.log('fetchRooms 0')

		try {
			//console.log('fetchRooms 1')
			this.botService.getRooms({}).subscribe((res) => {
				if (res != 'undefined' && res.success == RESPONSE_STATUS.success) {
					if (res.result) {
						console.log('fetchRooms ', res)
						this.rooms = res.result.Data
						//console.log('fetchRooms ', this.rooms)
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
}
