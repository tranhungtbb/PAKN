import { Component, OnInit, ViewChild } from '@angular/core'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'
import { HistoryChatbotObject } from 'src/app/models/historyChatbotObject'
import { ChatbotService } from 'src/app/services/chatbot.service'
import { DataService } from 'src/app/services/sharedata.service'
import { saveAs as importedSaveAs } from 'file-saver'
import { MESSAGE_COMMON, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'

declare var $: any

@Component({
	selector: 'app-history-chat-bot',
	templateUrl: './history-chat-bot.component.html',
	styleUrls: ['./history-chat-bot.component.css'],
})
export class HistoryChatBotComponent implements OnInit {
	constructor(private _service: ChatbotService, private _toastr: ToastrService, private _fb: FormBuilder, private _shareData: DataService) {}
	listData = new Array<HistoryChatbotObject>()
	listStatus: any = [
		{ value: '', text: 'Chọn trạng thái' },
		{ value: true, text: 'Hiệu lực' },
		{ value: false, text: 'Hết hiệu lực' },
	]
	form: FormGroup
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
		this.buildForm()
		this.getList()
	}
	ngAfterViewInit() {
		this._shareData.seteventnotificationDropdown()
	}

	get f() {
		return this.form.controls
	}

	buildForm() {
		this.form = this._fb.group({
			fullName: [this.model.fullName, Validators.required],
			question: [this.model.question, Validators.required],
			answer: [this.model.answer, Validators.required],
		})
	}

	rebuilForm() {
		this.form.reset({
			fullName: this.model.fullName,
			question: this.model.question,
			answer: this.model.answer,
		})
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
					console.log(response)
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

	dataUpdate: any
	preUpdateStatus(data) {
		this.dataUpdate = data
		$('#modalConfirmUpdateStatus').modal('show')
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

	changeState(event: any) {
		if (event) {
			if (event.target.value == 'null') {
			} else {
			}
			this.pageIndex = 1
			this.pageSize = 20
			this.getList()
		}
	}

	changeType(event: any) {
		if (event) {
			this.pageIndex = 1
			this.pageSize = 20
			this.getList()
		}
	}
}
