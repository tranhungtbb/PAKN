import { Component, Input, OnInit } from '@angular/core'
import { Helpers } from 'src/app/modules/chatbox/helper/helpers'
import { UserServiceChatBox } from 'src/app/modules/chatbox/user/user.service'
import { DialogService } from 'src/app/modules/chatbox/dashboard/dialogs/dialog.service'
import { DashboardService } from 'src/app/modules/chatbox/dashboard/dashboard.service'
import { MessageService } from 'src/app/modules/chatbox/dashboard/messages/message.service'
import { CONSTANTS } from 'src/app/modules/chatbox/QBconfig'
import {UserService } from 'src/app/services/user.service'
import { ToastrService } from 'ngx-toastr'

@Component({
	selector: 'app-delete-dialog',
	templateUrl: './delete-dialog.component.html',
	styleUrls: ['./delete-dialog.component.css'],
})
export class DeleteDialogComponent implements OnInit {
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
		private _toastr : ToastrService,
		private userService: UserServiceChatBox)
		{
		this.helpers = Helpers
		this._usersCache = this.userService._usersCache
		this.userService.usersCacheEvent.subscribe((usersCache: Object) => {
			this._usersCache = usersCache
		})
	}

	ngOnInit() {
		this.loggedinUser = this.userService.user
		this.lstIdQB = this.dialog.occupants_ids.join(',')
		// this.selectedUsers.push(this.loggedinUser.id)
		this.getUserList()
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
			deleteDialog : false,
			onChatClick: true,
		})
	}

	getUserList() {
		this.textSearch = this.textSearch == null ? '' : this.textSearch.trim()
		let obj = {
			lstId : this.dialog.occupants_ids.filter(x=>x != this.loggedinUser.id).join(','),
			textSearch : this.textSearch
		}
		this.userService
			.getUserListForDelete(obj)
			.then((res) => {
				this.users = res.result.users
			})
			.catch((err) => {
				console.log('Get User List Error: ', err)
				this.lstIdQB = ''
				this.users = []
			})
	}

	private updateDialog(toUpdateParams, updatedMsg, systemMessage, onChatClick = true) {
		if (this.dialog.type !== CONSTANTS.DIALOG_TYPES.GROUPCHAT) {
			return false
		}

		const self = this,
			dialogId = this.dialog._id

		this.dialogService
			.updateDialog(dialogId, toUpdateParams)
			.then(function (dialog) {
				self.dialogService.joinToDialog(dialog).then(() => {
					const message = self.messageService.sendMessage(dialog, updatedMsg),
						newMessage = self.messageService.fillNewMessageParams(self.userService.user.id, message)
					self.dialogService.dialogs[dialog._id] = dialog
					self.dialogService.setDialogParams(newMessage)
					self.messageService.messages.push(newMessage)
					self.messageService.addMessageToDatesIds(newMessage)
					self.messageService.messagesEvent.emit(self.messageService.datesIds)
				})

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
				self.dialogService.currentDialog = dialog
				self.dialogService.currentDialogEvent.emit(dialog)
				if (onChatClick) {
					self.dashboardService.showComponent({
						createGroupClicked: false,
						updateDialog: false,
						welcomeChat: false,
						onChatClick: true,
						deleteDialog: false,
					})
				}
			})
			.catch(function (error) {
				console.error(error)
			})
	}

	public onSubmit() {
		if (this.dialog.type !== CONSTANTS.DIALOG_TYPES.GROUPCHAT) {
			return false
		}
		const self = this
			const dialogId = this.dialog._id,
			newUsers = this.selectedUsers.filter(x=>x != this.loggedinUser.id),
			usernames = newUsers.map(function (userId) {
				let s = self.users.find(x=>x.idQB == userId)
				if(s){
					return s.fullName
				}
				return
			}),
			toUpdateParams = {},
			updatedMsg = {
				type: 'groupchat',
				body: '',
				extension: {
					save_to_history: 1,
					dialog_id: dialogId,
					notification_type: 2,
					dialog_updated_at: Date.now() / 1000,
					new_occupants_ids: newUsers.join(','),
				},
				markable: 1,
			}
		if ('updates.userList' === 'updates.userList' && newUsers.length) {
			toUpdateParams['pull_all'] = {
				occupants_ids: newUsers,
			}
			updatedMsg.body = self.userService.user.full_name + ' đã xóa ' + usernames.join(', ') + ' khỏi cuộc trò chuyện.'
			updatedMsg.extension['new_occupants_ids'] = newUsers.join(',')
		}

		const systemMessage = {
			extension: {
				notification_type: 2,
				dialog_id: dialogId,
				new_occupants_ids: newUsers.toString(),
			},
		}
		self.updateDialog(toUpdateParams, updatedMsg, systemMessage, true)
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
