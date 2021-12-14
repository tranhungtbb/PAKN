import { Component, OnInit } from '@angular/core'
import { Router } from '@angular/router'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { stat } from 'fs'
import { ChatbotService } from 'src/app/services/chatbot.service'
import { IndexSettingService } from 'src/app/services/index-setting.service'
import { IndexSettingObjet, IndexBanner, IndexWebsite } from 'src/app/models/indexSettingObject'
import { RESPONSE_STATUS, TYPE_NOTIFICATION, RECOMMENDATION_STATUS } from 'src/app/constants/CONSTANTS'
import { AuthenticationService } from 'src/app/services/authentication.service'
import { DataService } from 'src/app/services/sharedata.service'
import { NotificationService } from 'src/app/services/notification.service'

declare var $: any
declare var jquery: any

@Component({
	selector: 'app-chatbot',
	templateUrl: './chatbot.component.html',
	styleUrls: ['./chatbot.component.css'],
})
export class ChatbotComponent implements OnInit {
	constructor(
		private _router: Router,
		private storageService: UserInfoStorageService,
		private authenService: AuthenticationService,
		private sharedataService: DataService,
		private notificationService: NotificationService,
		private chatBotService: ChatbotService,
		private indexSettingService: IndexSettingService
	) {}

	currentFullnName: string = this.storageService.getFullName()

	botname: string = 'Bot'
	message: string = ''
	messages: any = [
		{
			who: this.botname,
			isReply: true,
			message: 'Vui lòng nhập câu hỏi...',
		},
	]

	ngOnInit() {}

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
			this.chatBotService.getNewUserId().subscribe((data) => {
				let newUs = null
				newUs = data
				if (newUs.UserID) {
					localStorage.setItem('kluid', newUs.UserID)
					this.sendToServer()
				}
			}),
				(error) => {
					console.log(error)
				}
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
				console.log('hi')
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
