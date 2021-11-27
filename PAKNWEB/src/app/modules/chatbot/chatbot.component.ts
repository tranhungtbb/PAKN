import { Component, OnInit } from '@angular/core'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { BotRoom, CustomHttpClient } from 'src/app/models/chatbotObject'
import { ChatBotService } from './chatbot.service'
//import { StreamChat, ChannelData, Message, User } from 'stream-chat'
//import axios from 'axios'
import * as signalR from '@aspnet/signalr/'
import { AppSettings } from 'src/app/constants/app-setting'
import * as uuid from 'uuid'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'

@Component({
	selector: 'app-dashboard',
	templateUrl: './chatbot.component.html',
	styleUrls: ['./chatbot.component.css'],
})
export class DashboardChatBotComponent implements OnInit {
	rooms: BotRoom[]
	constructor(private botService: ChatBotService, private user: UserInfoStorageService) {}
	ngOnInit() {
		const userId = this.user.getUserId()
		const connection = new signalR.HubConnectionBuilder()
			.withUrl(`${AppSettings.SIGNALR_ADDRESS}?sysUserName=${userId}`, {
				skipNegotiation: true,
				transport: signalR.HttpTransportType.WebSockets,
			})
			.configureLogging(signalR.LogLevel.Information)
			.build()

		async function start() {
			try {
				await connection.start()
				connection.off('ReceiveMessageToGroup')
				connection.on('ReceiveMessageToGroup', (data: any) => {
					console.log('SignalR ReceiveMessageToGroup ', data)
				})
				console.log('SignalR Connected.')
			} catch (err) {
				console.log(err)
				setTimeout(start, 5000)
			}
		}

		connection.onclose(async () => {
			await start()
		})

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
					}
				} else {
					//this._toastr.error(res.message)
				}
			})
		} catch (error) {
			console.log('handleConnect ', error)
		}
	}
}
