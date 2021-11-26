import { Component, OnInit } from '@angular/core'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { BotRoom } from 'src/app/models/chatbotObject'
import { ChatBotService } from './chatbot.service'
//import { StreamChat, ChannelData, Message, User } from 'stream-chat'
//import axios from 'axios'
import * as signalR from '@aspnet/signalr'
import { AppSettings } from 'src/app/constants/app-setting'

@Component({
	selector: 'app-dashboard',
	templateUrl: './chatbot.component.html',
	styleUrls: ['./chatbot.component.css'],
})
export class DashboardChatBotComponent implements OnInit {
	title = 'angular-chat'
	// channel: ChannelData
	// username = ''
	// messages: Message[] = []
	// newMessage = ''
	rooms: BotRoom[]
	// chatClient: any
	// currentUser: User
	connection: signalR.HubConnection
	constructor(private botService: ChatBotService) {}
	ngOnInit() {
		console.log('SignalR ngOnInit 0')
		this.connection = new signalR.HubConnectionBuilder().configureLogging(signalR.LogLevel.Information).withUrl(AppSettings.SIGNALR_ADDRESS).build()

		this.connection
			.start()
			.then(function () {
				console.log('SignalR Connected!')
			})
			.catch(function (err) {
				return console.error(err.toString())
			})

		this.connection.on('ReceiveMessageToGroup', (data: any) => {
			console.log('SignalR ReceiveMessageToGroup ', data)
		})

		console.log('SignalR 1')
		this.botService.getRooms({}).subscribe((res) => {
			if (res != 'undefined' && res.success == RESPONSE_STATUS.success) {
				if (res.result) {
					console.log('getRooms ', res)
					this.rooms = res.result.Data
					console.log('getRooms ', this.rooms)
					// this.totalRecord = res.result.TotalCount
				}
			} else {
				//this._toastr.error(res.message)
			}
		})
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
	}

	ngOnDestroy() {
		if (this.connection) {
			console.log('SignalR ngOnDestroy 0')
			this.connection.off('ReceiveMessageToGroup')
		}
	}
}
