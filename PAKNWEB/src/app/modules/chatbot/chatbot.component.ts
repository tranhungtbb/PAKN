import { Component, ElementRef, OnInit, ViewChild } from '@angular/core'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { BotMessage, BotRoom } from 'src/app/models/chatbotObject'
import { ChatBotService } from './chatbot.service'
import * as signalR from '@aspnet/signalr/'
import { AppSettings } from 'src/app/constants/app-setting'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'

@Component({
	selector: 'app-dashboard',
	templateUrl: './chatbot.component.html',
	styleUrls: ['./chatbot.component.css'],
})
export class DashboardChatBotComponent implements OnInit {
	title = 'angular-chat'
	channel: any
	roomNameSelected: string
	messages: any = []
	newMessage = ''
	rooms: BotRoom[]
	// chatClient: any
	// currentUser: User
	connection: signalR.HubConnection
	pageIndex: number = 1
	pageSize: number = 10
	totalMessage: number = 0
	roomActive: number = 0
	newMessage : string
	@ViewChild('boxChat', { static: true }) private boxChat: ElementRef

	constructor(private botService: ChatBotService, private user: UserInfoStorageService) {}
	ngOnInit() {
		console.log('ngOnInit 0')
		const userId = this.user.getUserId()
		this.connection = new signalR.HubConnectionBuilder()
			.withUrl(`${AppSettings.SIGNALR_ADDRESS}?sysUserName=${userId}`, {
				skipNegotiation: true,
				transport: signalR.HttpTransportType.WebSockets,
			})
			.configureLogging(signalR.LogLevel.Information)
			.withAutomaticReconnect()
			.build()

		this.connection.start().then(() => {
			this.connection.on('ReceiveMessageToGroup', (data: any) => {
				console.log('ngOnInit SignalR ReceiveMessageToGroup ', data)
				if (data.type === 'Conversation' && this.roomNameSelected && this.roomNameSelected === data.from) {
					this.messages = [{ messageContent: data.content }, ...this.messages].reverse()
				} else if (data.type === 'All') {
					this.fetchRooms()
				}
			})
		})
		this.fetchRooms()
	}

	fetchRooms = async () => {
		console.log('fetchRooms 0')

		try {
			console.log('fetchRooms 1')
			this.botService.getRooms({}).subscribe((res) => {
				if (res != 'undefined' && res.success == RESPONSE_STATUS.success) {
					if (res.result) {
						console.log('fetchRooms ', res)
						this.rooms = res.result.Data
						console.log('fetchRooms ', this.rooms)
					}
				} else {
					//this._toastr.error(res.message)
				}
			})
		} catch (error) {
			console.log('handleConnect ', error)
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
			//if (this.connection.state === signalR.HubConnectionState.Connected) {
			this.connection.invoke('JoinToRoom', roomName)
			//}
			this.botService.getMessages(request).subscribe((res) => {
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
		} catch (err) {}
	}

	sendMessage() {
		if (this.newMessage !== '') {
			console.log('sendMessage ', this.newMessage)
			this.connection.invoke('SendToRoom', this.roomNameSelected, this.newMessage)
			this.messages = [...this.messages, { messageContent: this.newMessage }].reverse()
			this.newMessage = ''
		}
	}

	ngOnDestroy() {
		if (this.connection) {
			console.log('SignalR ngOnDestroy 0')
			this.connection.off('ReceiveMessageToGroup')
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
}
