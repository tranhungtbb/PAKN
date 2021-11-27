import { Component, ElementRef, OnInit, ViewChild } from '@angular/core'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { BotRoom } from 'src/app/models/chatbotObject'
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
	@ViewChild('boxChat', { static: true }) private boxChat: ElementRef

	constructor(private botService: ChatBotService, private user: UserInfoStorageService) {}
	ngOnInit() {
		const userId = this.user.getUserId()
		this.connection = new signalR.HubConnectionBuilder()
			.withUrl(`${AppSettings.SIGNALR_ADDRESS}?userName=${userId}`, {
				skipNegotiation: true,
				transport: signalR.HttpTransportType.WebSockets,
			})
			.configureLogging(signalR.LogLevel.Information)
			.build()
		//connection.keepAliveIntervalInMilliseconds = 60
		this.connection.serverTimeoutInMilliseconds = 60
		async function start() {
			try {
				await this.connection.start()
				this.connection.off('ReceiveMessageToGroup')
				this.connection.on('ReceiveMessageToGroup', (data: any) => {
					console.log('SignalR ReceiveMessageToGroup ', data)
				})
				console.log('SignalR Connected.')
			} catch (err) {
				console.log(err)
				//setTimeout(start, 5000)
			}
		}

		this.connection.onclose(async () => {
			await start()
		})

		// Start the connection.
		start()
		this.handleConnect()
	}

	handleConnect = async () => {
		console.log('SignalR ngOnInit 0')
		try {
			console.log('SignalR 1')
			this.botService.getRooms({}).subscribe((res) => {
				if (res != 'undefined' && res.success == RESPONSE_STATUS.success) {
					if (res.result) {
						console.log('getRooms ', res)
						this.rooms = res.result.Data
						console.log('getRooms ', this.rooms)
						// this.totalRecord = res.result.TotalCount
						if (this.rooms != null && this.rooms.length > 0) {
							this.resetGetMessage(this.rooms[0].id, this.rooms[0].name)
						}
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
			this.connection.invoke('JoinToRoom', roomName)
			this.botService.getMessages(request).subscribe((res) => {
				if (res != 'undefined' && res.success == RESPONSE_STATUS.success) {
					if (res.result) {
						if (this.pageIndex == 1) {
							this.messages = res.result
						} else {
							res.result.forEach((element) => {
								this.messages.splice(0, 0, element)
							})
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
			console.log('height scroll:' + this.boxChat.nativeElement.scrollHeight)
			this.boxChat.nativeElement.scrollTop = this.boxChat.nativeElement.scrollHeight
		} catch (err) {}
	}

	async joinChat() {
		// const { username } = this
		// try {
		// 	const response = await axios.post('http://localhost:5500/join', {
		// 		username,
		// 	})
		// 	const { token } = response.data
		// 	const apiKey = response.data.api_key
		// 	this.chatClient = new StreamChat(apiKey)
		// 	this.currentUser = await this.chatClient.setUser(
		// 		{
		// 			id: username,
		// 			name: username,
		// 		},
		// 		token
		// 	)
		// 	const channel = this.chatClient.channel('team', 'talkshop')
		// 	await channel.watch()
		// 	this.channel = channel
		// 	this.messages = channel.state.messages
		// 	this.channel.on('message.new', (event) => {
		// 		this.messages = [...this.messages, event.message]
		// 	})
		// 	const filter = {
		// 		type: 'team',
		// 		members: { $in: [`${this.currentUser.me.id}`] },
		// 	}
		// 	const sort = { last_message_at: -1 }
		// 	this.channelList = await this.chatClient.queryChannels(filter, sort, {
		// 		watch: true,
		// 		state: true,
		// 	})
		// } catch (err) {
		// 	console.log(err)
		// 	return
		// }
	}

	async sendMessage() {
		// if (this.newMessage.trim() === '') {
		// 	return
		// }
		// try {
		// 	await this.channel.sendMessage({
		// 		text: this.newMessage,
		// 	})
		// 	this.newMessage = ''
		// } catch (err) {
		// 	console.log(err)
		// }
		this.connection.invoke('SendToRoom', this.newMessage)
	}

	ngOnDestroy() {
		// if (this.connection) {
		// 	console.log('SignalR ngOnDestroy 0')
		// 	this.connection.off('ReceiveMessageToGroup')
		// }
	}
	enabledBot() {}
	disabedBot() {}
	onScrollBoxChat(event: any) {
		if (event.target.scrollTop == 0) {
			if (this.pageIndex * this.pageSize < this.totalMessage) {
				this.pageIndex++
				this.getMessage(this.roomActive, this.roomNameSelected)
			}
		}
	}
}
