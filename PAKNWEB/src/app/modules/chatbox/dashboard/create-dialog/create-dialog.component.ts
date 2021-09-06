import { Component, Input, OnInit } from '@angular/core'
import { Helpers } from 'src/app/modules/chatbox/helper/helpers'
import { UserServiceChatBox } from 'src/app/modules/chatbox/user/user.service'
import { DialogService } from 'src/app/modules/chatbox/dashboard/dialogs/dialog.service'
import { DashboardService } from 'src/app/modules/chatbox/dashboard/dashboard.service'
import { MessageService } from 'src/app/modules/chatbox/dashboard/messages/message.service'
import { CONSTANTS } from 'src/app/modules/chatbox/QBconfig'
import {UserService } from 'src/app/services/user.service'
import {RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'

@Component({
	selector: 'app-create-dialog',
	templateUrl: './create-dialog.component.html',
	styleUrls: ['./create-dialog.component.css'],
})
export class CreateDialogComponent implements OnInit {
	@Input() dialog: any

	public loggedinUser: any
	public users: any = []
	public selectedUsers: number[] = []
	public helpers: Helpers
	public _usersCache: any
	public messageField = ''
	pageIndex : number = 1
	textSearch : any = ''
	lstIdQB : any = ''

	constructor(
		private dashboardService: DashboardService, 
		public dialogService: DialogService, 
		private messageService: MessageService,
		private _service : UserService, 
		private userService: UserServiceChatBox)
		{
		this.helpers = Helpers
		this._usersCache = this.userService._usersCache
		this.userService.usersCacheEvent.subscribe((usersCache: Object) => {
			this._usersCache = usersCache
		})
	}

	ngOnInit() {
		this.getUserList()
		this.loggedinUser = this.userService.user
		this.selectedUsers.push(this.loggedinUser.id)
	}

	toggleSelectItem(userId: number) {
		const index = this.selectedUsers.indexOf(userId)
		if (this.loggedinUser.id === userId) {
			return false
		}
		if (index >= 0) {
			this.selectedUsers.splice(index, 1)
		} else {
			this.selectedUsers.push(userId)
		}
	}

	goBack() {
		this.dashboardService.showComponent({
			createGroupClicked: false,
			updateDialog: false,
			onChatClick: !this.dashboardService.components.welcomeChat,
		})
	}

	getUserList() {
		let obj = {
			PageIndex : this.pageIndex,
			UserName : this.lstIdQB,
			TextSearch : this.textSearch
		}
		this.userService
			.getUserListForChat(obj)
			.then((res) => {
				this.users = res.result.users
				.map(item => {
					item.color = Math.floor(Math.random() * (10 - 1 + 1)) + 1
					return item
				})
				this.pageIndex = res.result.page
			})
			.catch((err) => {
				console.log('Get User List Error: ', err)
				this.pageIndex = 1
				this.textSearch = ''
				this.lstIdQB = ''
				this.users = []
			})
	}

	public onSubmit() {
		const self = this
		const params = {
			type: this.selectedUsers.length > 2 ? 2 : 3,
			occupants_ids: this.selectedUsers.join(','),
			name : ''
		}

		let name = ''

		if (params.type) {
			const userNames = this.users
			.filter((array) => {
				return self.selectedUsers.indexOf(array.idQB) !== -1 && array.idQB !== this.loggedinUser.id
			})
			.map((array) => {
				return array.fullName
			})
			name = userNames.join(', ')
		}

		if (this.messageField) {
			params.name = this.messageField
		}else{
			params.name = name
		}

		this.dialogService.createDialog(params).then((dialog) => {
			let messageBody = this.userService.user.full_name + ' đã tạo nhóm mới với: '
			messageBody += name
			const systemMessage = {
					extension: {
						notification_type: 1,
						dialog_id: dialog._id,
					},
				},
				notificationMessage = {
					type: 'groupchat',
					body: messageBody,
					extension: {
						save_to_history: 1,
						dialog_id: dialog._id,
						notification_type: 1,
						date_sent: Date.now(),
					},
				}

			new Promise(function (resolve) {
				if (dialog.xmpp_room_jid) {
					self.dialogService.joinToDialog(dialog).then(() => {
						if (dialog.type === CONSTANTS.DIALOG_TYPES.GROUPCHAT) {
							const message = self.messageService.sendMessage(dialog, notificationMessage),
								newMessage = self.messageService.fillNewMessageParams(self.userService.user.id, message)
							self.dialogService.dialogs[dialog._id] = dialog
							self.dialogService.setDialogParams(newMessage)
							self.messageService.messages.push(newMessage)
							self.messageService.addMessageToDatesIds(newMessage)
							self.messageService.messagesEvent.emit(self.messageService.datesIds)
						}
						resolve('')
					})
				}
				resolve('')
			}).then(() => {
				const userIds = dialog.occupants_ids.filter((userId) => {
					return userId !== self.userService.user.id
				})
				self.messageService.sendSystemMessage(userIds, systemMessage)
				if (self.dialogService.dialogs[dialog._id] === undefined) {
					const tmpObj = {}
					tmpObj[dialog._id] = dialog
					self.dialogService.dialogs = Object.assign(tmpObj, self.dialogService.dialogs)
					self.dialogService.dialogsEvent.emit(self.dialogService.dialogs)
				}

				this.dialogService.currentDialog = dialog
				this.dialogService.currentDialogEvent.emit(dialog)
				this.dashboardService.showComponent({
					createGroupClicked: false,
					updateDialog: false,
					welcomeChat: false,
					onChatClick: true,
				})
			}).catch(err=>{
				console.log(err)	
			})
		})
	}
}
