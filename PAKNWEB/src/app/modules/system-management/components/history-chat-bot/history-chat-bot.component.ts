import { Component, OnInit, ViewChild } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { HistoryChatbotObject } from 'src/app/models/historyChatbotObject'
import { ChatbotService } from 'src/app/services/chatbot.service'
import { DataService } from 'src/app/services/sharedata.service'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'

declare var $: any

@Component({
	selector: 'app-history-chat-bot',
	templateUrl: './history-chat-bot.component.html',
	styleUrls: ['./history-chat-bot.component.css'],
})
export class HistoryChatBotComponent implements OnInit {
	constructor(private _service: ChatbotService, private _toastr: ToastrService, private _shareData: DataService) {}
	listData = new Array<HistoryChatbotObject>()
	listStatus: any = [
		{ value: '', text: 'Chọn trạng thái' },
		{ value: true, text: 'Hiệu lực' },
		{ value: false, text: 'Hết hiệu lực' },
	]

	model: any = new HistoryChatbotObject()
	submitted: boolean = false
	title: string = ''
	kluid: string = ''
	userId: number = 0
	fullName: string = ''
	question: string = ''
	answer: string = ''
	pageIndex: number = 1
	pageSize: number = 20
	@ViewChild('table', { static: false }) table: any
	totalRecords: number = 0

	ngOnInit() {
		this.getList()
	}
	ngAfterViewInit() {
		this._shareData.seteventnotificationDropdown()
	}

	getList() {
		this.question = this.question.trim()
		this.answer = this.answer.trim()

		let request = {
			FullName: this.fullName,
			Question: this.question.trim(),
			Answer: this.answer.trim(),
			PageIndex: this.pageIndex,
			PageSize: this.pageSize,
		}
		this._service.chatbotGetListHistory(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result != null) {
					this.listData = []
					this.listData = response.result.HistoryChatbotGetAllOnPage
					this.totalRecords = response.result.HistoryChatbotGetAllOnPage.length != 0 ? response.result.HistoryChatbotGetAllOnPage[0].rowNumber : 0
				}
			} else {
				this._toastr.error(response.message)
			}
		}),
			(error) => {
				console.log(error)
				alert(error)
			}
	}

	onPageChange(event: any) {
		this.pageSize = event.rows
		this.pageIndex = event.first / event.rows + 1
		this.getList()
	}

	dataStateChange() {
		this.pageIndex = 1
		this.table.first = 0
		this.getList()
	}
}
