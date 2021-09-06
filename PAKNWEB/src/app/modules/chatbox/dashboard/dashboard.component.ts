import { Component, OnInit } from '@angular/core'
import { Router } from '@angular/router'
import { DashboardService } from './dashboard.service'
import { DialogService } from '../dashboard/dialogs/dialog.service'
import { UserServiceChatBox } from '../user/user.service'
import { QBHelper } from 'src/app/modules/chatbox/helper/qbHelper'
import { MessageService } from './messages/message.service'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { Helpers } from 'src/app/modules/chatbox/helper/helpers'


interface Message {
	name: string,	
}
@Component({
	selector: 'app-dashboard',
	templateUrl: './dashboard.component.html',
	styleUrls: ['./dashboard.component.css'],
})


export class DashboardChatBoxComponent implements OnInit {
	loggedinUser: any
	tinNhan: Message[];

	SelectedMessage: Message;
	public helpers: Helpers
	public chats: any = []

	chatsClicked = false // For displaying OneToOne and Group Chats
	publicChatClicked = false // For displaying Public Chats
	createGroupClicked = false // For creating OneToOne and Group Chats
	onChatClick = false // For displaying messages ( Dialog Component )
	welcomeChat = true // Display default Welcome Chat screen
	updateDialog = false // For displaying update dialog

	dialog: any
	selectedChat: string
	private successUnSubscribe$

	constructor(
		private dashboardService: DashboardService,
		public dialogService: DialogService,
		private userService: UserServiceChatBox,
		private qbHelper: QBHelper,
		private messageService: MessageService,
		private router: Router,
		
	) {
		this.tinNhan = [
			{name: 'Tin Nhắn Gần Đây'},
			{name: 'Tin nhắn đến'},
			{name: 'tin nhắn all'},
		];
		this.dialogService.dialogsEvent.subscribe((chatData: any[]) => {
			this.chats = Object.values(chatData)
			console.log(this.chats)
		})
		this.dialogService.currentDialogEvent.subscribe((dialog) => {
			this.selectedChat = dialog._id
			this.dialog = dialog
		})
		this.dashboardService.componentsEvent.subscribe((components: Object) => {
			Object.entries(components).forEach(([key, value]) => {
				this[key] = value
			})
		})
	}

	ngOnInit() {
		this.welcomeChat = true
		this.loggedinUser = this.userService.user
		console.log('Logged In === ', this.loggedinUser)
		this.getChatList('chat')		
	}

	// Logout
	logout(userId) {
		console.log('Logout: ', userId)
		this.qbHelper.qbLogout()
		window.location.href = '/quan-tri/ban-lam-viec'
	}

	// Chats List
	getChatList(type) {
		const filter = {
			limit: 100,
			sort_desc: 'updated_at',
		}

		this.dashboardService.showComponent({
			chatsClicked: type === 'chat',
			publicChatClicked: type !== 'chat',
		})

		if (type === 'chat') {
			filter['type[in]'] = [3, 2].join(',')
		} else {
			filter['type'] = 1
		}

		this.dialogService
			.getDialogs(filter)
			.then((res) => {
				if (res) {
					res['items'].forEach((chat, index, self) => {
						if (chat.xmpp_room_jid) {
							this.dialogService.joinToDialog(chat)
						}
						self[index].last_message_date_sent = +chat.last_message_date_sent * 1000
					})
					this.dialogService.setDialogs(res['items'])
				}
			})
			.catch((err) => {
				console.log('Get chats error: ' + err)
			})
	}

	// Create New Group
	createNewGroup() {
		this.dashboardService.showComponent({
			createGroupClicked: true,
			updateDialog: false,
			welcomeChat: false,
			onChatClick: false,
		})
	}

	// Open Chat
	openChat(chat) {
		this.selectedChat = chat._id
		this.dialogService.currentDialog = chat
		this.dialogService.currentDialogEvent.emit(chat)
		this.dashboardService.showComponent({
	 'createGroupClicked': false,
      'updateDialog': false,
      'welcomeChat': false,
      'onChatClick': true
		})
	}

	getTime(date : any){
		let result = ''
		let sendDate = new Date(date)
		let currentDate = new Date()
		if(sendDate.getFullYear() != currentDate.getFullYear()){
			result = String(sendDate.getMinutes()) +':'+ sendDate.getHours() + ' ' + sendDate.getDate() + '/' + sendDate.getMonth() + '/' + sendDate.getFullYear()
		}else{
			if(sendDate.getMonth() != currentDate.getMonth()){
				result = Number(currentDate.getMonth() - sendDate.getMonth()) + ' tháng trước'
			}
			else{
				if(sendDate.getDate() != currentDate.getDate()){
					result = Number(currentDate.getDate() - sendDate.getDate()) + ' ngày trước'
				}
				else{
					if(sendDate.getHours() != currentDate.getHours()){
						result = Number(currentDate.getHours() - sendDate.getHours()) + ' giờ trước'
					}
					else{
						if(sendDate.getMinutes() != currentDate.getMinutes()){
							result = Number(currentDate.getMinutes() - sendDate.getMinutes()) + ' phút trước'
						}else{
							result = '1 giây trước'
						}
						
					}
				}
			}
		}
		return result
	}
	ramdom(){
		return Math.floor(Math.random() * (10 - 1 + 1)) + 1
	}
	getShortName(string) {
		var names = string.split(' '),
			initials = names[0].substring(0, 1).toUpperCase()
		if (names.length > 1) {
			initials += names[names.length - 1].substring(0, 1).toUpperCase()
		}
		return initials
	}
}
