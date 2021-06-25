import { AfterViewInit, Component, ElementRef, EventEmitter, Input, Output, ViewChild } from '@angular/core'
import { MessageService } from './message.service'
import { CONSTANTS } from 'src/app/modules/chatbox/QBconfig'
import { DialogService } from '../dialogs/dialog.service'

@Component({
	selector: 'app-message',
	templateUrl: './message.component.html',
	styleUrls: ['../dialogs/dialogs.component.css'],
})
export class MessageComponent implements AfterViewInit {
	@Input() message: any = []
	@ViewChild('element', { static: false }) el: ElementRef
	public CONSTANTS = CONSTANTS

	User: any
	statusAcc: boolean = true

	idUser: number
	constructor(private messageService: MessageService, private dialogService: DialogService) {
		this.User = JSON.parse(localStorage.loggedinUser)
		this.idUser = this.User.id
		//this.classNameMess()
	}

	ngAfterViewInit() {
		if (this.message.visibilityEvent) {
			this.el.nativeElement['dataset'].messageId = this.message._id
			this.el.nativeElement['dataset'].userId = this.message.sender_id
			this.el.nativeElement['dataset'].dialogId = this.message.chat_dialog_id
			this.messageService.intersectionObserver.observe(this.el.nativeElement)
		}
	}
	checkTypeFile(name: string) {
		let File = name
		let count = File.indexOf('.zip')
		if (count > 0) {
			return true
		} else return false
	}

	visibility(e) {
		this.dialogService.dialogs[e.detail.dialogId].unread_messages_count--
		this.dialogService.dialogsEvent.emit(this.dialogService.dialogs)
		this.messageService.intersectionObserver.unobserve(this.el.nativeElement)
		this.messageService.messages = this.messageService.messages.map((message) => {
			if (message._id === e.detail.messageId) {
				message.visibilityEvent = false
			}
			return message
		})
	}

	loadImagesEvent(e) {
		let img: any, container: Element, imgPos: number, scrollHeight: number
		img = e.target
		container = document.querySelector('.j-messages')
		// @ts-ignore
		imgPos = container.offsetHeight + container.scrollTop - img.offsetTop
		scrollHeight = container.scrollTop + img.offsetHeight

		img.classList.add('loaded')

		if (imgPos >= 0) {
			container.scrollTop = scrollHeight + 5
		}
	}

	styleClass: string
	classNameMess() {
		if (this.idUser === this.message.sender_id && !this.message.notification_type) {
			return 'bg-primary-light message__content border border-light p-2 rounded'
		} else if (this.idUser !== this.message.sender_id && !this.message.notification_type) {
			return 'bg-primary-light message__content m_bg border p-2 rounded'
		} else {
			return 'message__content'
		}
	}

	classNameMess_Content() {
		if (this.idUser !== this.message.sender_id && !this.message.notification_type) {
			return 'm_content'
		} else {
			return 'message__text'
		}
	}
	styleAT_User() {
		if (this.idUser !== this.message.sender_id && !this.message.notification_type) {
			return 'm_attachment'
		} else {
			return 'message_attachment'
		}
	}
	checkstatusAcc() {
		if (this.idUser !== this.message.sender_id) {
			this.statusAcc = false
		}
		this.statusAcc = true
	}
}
