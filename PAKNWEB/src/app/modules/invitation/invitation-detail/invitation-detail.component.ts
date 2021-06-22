import { Component, OnInit} from '@angular/core'

import { defineLocale } from 'ngx-bootstrap/chronos'
import { viLocale } from 'ngx-bootstrap/locale'
import { BsLocaleService } from 'ngx-bootstrap/datepicker'

import { ActivatedRoute } from '@angular/router'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { InvitationService } from 'src/app/services/invitation.service'
import { InvitationObject } from 'src/app/models/invitationObject'

declare var $: any
defineLocale('vi', viLocale)

@Component({
	selector: 'app-invitation-detail',
	templateUrl: './invitation-detail.component.html',
	styleUrls: ['./invitation-detail.component.css'],
})
export class InvitationDetailComponent implements OnInit {
	model: InvitationObject = new InvitationObject()
	senderName : any = ''
	files: any[]
	title: string
	listStatus: any = [
		{ value: 1, text: 'Đang soạn thảo' },
		{ value: 2, text: 'Đã gửi' },
	]
	constructor(
		private invitationService: InvitationService,
		private activatedRoute: ActivatedRoute,
		private BsLocaleService: BsLocaleService
	) {
		this.files = []
	}

	ngOnInit() {
		this.BsLocaleService.use('vi')
		this.getInvitatonModelById()
	}


	getInvitatonModelById() {
		this.activatedRoute.params.subscribe((params) => {
			let id = +params['id']
			this.model.id = isNaN(id) == true ? 0 : id
			if (this.model.id != 0) {
				this.invitationService.invitationDetail({ id: this.model.id }).subscribe((res) => {
					if (res.success == RESPONSE_STATUS.success) {
						if (res.result) {
							this.model = { ...res.result.model }
							this.files = res.result.invFileAttach
							this.senderName = res.result.senderName
						}
					}
				})
			}
		})
	}

	getShortName(string) {
		var names = string.split(' '),
			initials = names[0].substring(0, 1).toUpperCase()
		if (names.length > 1) {
			initials += names[names.length - 1].substring(0, 1).toUpperCase()
		}
		return initials
	}
	getTime(date : any){
		let result = ''
		let sendDate = new Date(date)
		let currentDate = new Date()
		if(sendDate.getFullYear() != currentDate.getFullYear()){
			result = Number(currentDate.getFullYear() - sendDate.getFullYear()) + ' năm trước'
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
							result = Number(currentDate.getMilliseconds() - sendDate.getMilliseconds()) + ' giây trước'
						}
						
					}
				}
			}
		}
		return result
	}

	

	redirectHis() {
		window.history.back()
	}
}
