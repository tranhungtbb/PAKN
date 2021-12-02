import { Component, OnInit, ViewChild } from '@angular/core'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'
import { ChatbotObject } from 'src/app/models/chatbotObject'
import { ChatbotService } from 'src/app/services/chatbot.service'
import { DataService } from 'src/app/services/sharedata.service'
import { saveAs as importedSaveAs } from 'file-saver'
import { MESSAGE_COMMON, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'

declare var $: any

@Component({
	selector: 'app-chat-bot',
	templateUrl: './chat-bot.component.html',
	styleUrls: ['./chat-bot.component.css'],
})
export class ChatBotComponent implements OnInit {
	constructor(private _service: ChatbotService, private _toastr: ToastrService, private _fb: FormBuilder, private _shareData: DataService) {}

	listData = new Array<ChatbotObject>()
	listStatus: any = [
		{ value: '', text: 'Chọn trạng thái' },
		{ value: true, text: 'Hiệu lực' },
		{ value: false, text: 'Hết hiệu lực' },
	]
	form: FormGroup
	model: any = new ChatbotObject()
	submitted: boolean = false
	categoryId: string = ''
	@ViewChild('table', { static: false }) table: any
	totalRecords: number = 0
	idDelete: number = 0
	categoryIdDelete: number = 0
	title: any = ''
	question: any = ''
	answer: any = ''
	testAnswer: any = ''
	lstAnswer: any = [];
	lstQuestion: any = [];
	questionId: number = 0;

	ngOnInit() {
		this.buildForm()
		this.getList()
	}

	ngAfterViewInit() {
		this._shareData.seteventnotificationDropdown()
		$('#modal').on('keypress', function (e) {
			if (e.which == 13) e.preventDefault()
		})
	}

	get f() {
		return this.form.controls
	}

	buildForm() {
		this.form = this._fb.group({
			title: [this.model.title, Validators.required],
			question: [this.model.question, Validators.required],
			answer: [this.model.answer, Validators.required],
			categoryId: [this.model.categoryId],
			isActived: [this.model.isActived, Validators.required],
		})
	}

	rebuilForm() {
		this.form.reset({
			title: this.model.title,
			question: this.model.question,
			isActived: this.model.isActived,
			answer: this.model.answer,
			categoryId: this.model.categoryId,
		})
	}

	getList() {
		this._service.chatbotGetList({}).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result != null) {
					this.listData = []
					this.listData = response.result.ChatbotGetAllOnPage

					this.totalRecords = response.result.ChatbotGetAllOnPage.length != 0 ? response.result.ChatbotGetAllOnPage[0].rowNumber : 0
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

	titlePopup: any
	preCreate() {
		this.model = new ChatbotObject()
		this.rebuilForm()
		this.submitted = false
		this.titlePopup = 'Thêm mới câu hỏi'
		$('#modal').modal('show')
		setTimeout(() => {
			$('#target').focus()
		}, 400)
	}

	onSave() {
		this.submitted = true
		if (this.form.invalid) {
			return
		}
		if (this.model.id == 0 || this.model.id == null) {
			this._service.chatbotInsertQuestion(this.model).subscribe((response) => {
				if (response.success == RESPONSE_STATUS.success) {
					if (response.result == -1) {
						this._toastr.error(MESSAGE_COMMON.EXISTED_NAME)
						$('#target').focus()
						return
					} else {
						$('#modal').modal('hide')
						this._toastr.success(MESSAGE_COMMON.ADD_SUCCESS)
						this.getList()
					}
				} else {
					this._toastr.error(response.message)
				}
			}),
				(error) => {
					console.error(error)
					alert(error)
				}
		} else {
			this._service.UpdateChatbot(this.model).subscribe((response) => {
				if (response.success == RESPONSE_STATUS.success) {
					if (response.result == -1) {
						this._toastr.error(MESSAGE_COMMON.EXISTED_NAME)
						$('#target').focus()
						return
					} else {
						$('#modal').modal('hide')
						this._toastr.success(MESSAGE_COMMON.UPDATE_SUCCESS)
						this.getList()
					}
				} else {
					this._toastr.error(response.message)
				}
			}),
				(error) => {
					console.error(error)
					alert(error)
				}
		}
	}

	preUpdate(data) {
		let request = {
			Id: data.id,
			Type: 1,
		}
		this._service.chatbotGetById(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				this.rebuilForm()
				this.titlePopup = 'Chỉnh sửa câu hỏi'
				this.model = response.result.ChatbotGetByID[0]
				$('#modal').modal('show')
				setTimeout(() => {
					$('#target').focus()
				}, 400)
			} else {
				this._toastr.error(response.message)
			}
		}),
			(error) => {
				console.error(error)
				alert(error)
			}
	}
	preDelete(id: number, categoryId: number) {
		this.idDelete = id
		this.categoryIdDelete = categoryId
		$('#modalConfirmDelete').modal('show')
	}

	onDelete(id: number, categoryIdDelete: number) {
		let request = {
			Id: id,
			CategoryId: categoryIdDelete,
		}
		let obj = this.listData.find((x) => x.id == id)
		this._service.chatbotDelete(request, obj.question).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				this._toastr.success(MESSAGE_COMMON.DELETE_SUCCESS)
				$('#modalConfirmDelete').modal('hide')
				this.getList()
			} else {
				this._toastr.error(response.message)
			}
		}),
			(error) => {
				console.error(error)
			}
	}

	onUpdateStatus(data) {
		var isActived = data.isActived
		let request = {
			Type: 1,
			Id: data.id,
		}
		data.isActived = !data.isActived
		this._service.chatbotUpdateStatus(data).subscribe((res) => {
			$('#modalConfirmUpdateStatus').modal('hide')
			if (res.success == 'OK') {
				if (data.isActived == true) {
					this._toastr.success(MESSAGE_COMMON.UNLOCK_SUCCESS)
				} else {
					this._toastr.success(MESSAGE_COMMON.LOCK_SUCCESS)
				}
			} else {
				this._toastr.error(res.message)
			}
		}),
			(error) => {
				console.error(error)
			}
	}

	preView(data) {
		this.model = data
		$('#modalDetail').modal('show')
	}

	onAddAnswer = () => {
		if (this.testAnswer == '') {
			this._toastr.error('Vui lòng nhập câu trả lời!')
			return
		}
		if (this.questionId) {
			let checkQuestion = this.lstQuestion.find((x) => x.id == this.questionId)
			if (!checkQuestion) {
				let bussiness = this.lstQuestion.find((x) => x.id == this.questionId)
				if (bussiness) {
					let obj = { answer: this.testAnswer, idSuggetLibrary: this.questionId, questionAnswers: bussiness.question }
					this.lstAnswer.push(obj)
				}
			}
		}
		this.testAnswer = null
		this.questionId = null
	}

	onRemoveAnswer = (item: any) => {
		this.lstAnswer = this.lstAnswer.filter((x) => x.answer != item.answer && x.idSuggetLibrary != item.idSuggetLibrary)
		return
	}
}
