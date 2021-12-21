import { Component, OnInit, ViewChild } from '@angular/core'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'
import { ChatbotObject } from 'src/app/models/chatbotObject'
import { ChatbotService } from 'src/app/services/chatbot.service'
import { DataService } from 'src/app/services/sharedata.service'
import { saveAs as importedSaveAs } from 'file-saver'
import { MESSAGE_COMMON, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { MatGridTileHeaderCssMatStyler } from '@angular/material'
import { HashtagObject } from 'src/app/models/hashtagObject'
import { CatalogService } from 'src/app/services/catalog.service'

declare var $: any

@Component({
	selector: 'app-chat-bot',
	templateUrl: './chat-bot.component.html',
	styleUrls: ['./chat-bot.component.css'],
})
export class ChatBotComponent implements OnInit {
	constructor(private _service: ChatbotService, private _toastr: ToastrService, private _fb: FormBuilder, private _shareData: DataService,
		private _serviceCatalog: CatalogService,) { }

	listData = new Array<ChatbotObject>()
	lstHashtag: any = []
	lstHashtagSelected: any[] = []
	hashtagId: number = null
	listStatus: any = [
		{ value: true, text: 'Hiệu lực' },
		{ value: false, text: 'Hết hiệu lực' },
	]
	listType: any = [
		{ value: 1, text: 'QnA' },
		{ value: 2, text: 'Kịch bản' },
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
	// answer: any = ''
	textAnswer: any = ''
	lstAnswer: any = [];
	lstQuestion: any = [];
	questionId: number = 0;
	pageIndex: number = 1;
	pageSize: number = 20;
	dataSearch: any = {
		title: "",
		question: "",
		isActived: null
	}

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
			title: [this.model.title],
			typeChat: [this.model.typeChat, Validators.required],
			question: [this.model.question, Validators.required],
			// answer: [this.model.answer, Validators.required],
			categoryId: [this.model.categoryId],
			isActived: [this.model.isActived, Validators.required]
		})
	}

	rebuilForm() {
		this.form.reset({
			title: this.model.title,
			typeChat: this.model.typeChat,
			question: this.model.question,
			isActived: this.model.isActived,
			// answer: this.model.answer,
			categoryId: this.model.categoryId
		})
	}

	onPageChange(event: any) {
		this.pageSize = event.rows
		this.pageIndex = event.first / event.rows + 1
		this.getList()
	}

	dataStateChange() {
		this.pageIndex = 1
		this.getList()
	}

	getList() {
		let req = {
			PageIndex: this.pageIndex,
			PageSize: this.pageSize,
			Title: this.dataSearch.title,
			Question: this.dataSearch.question,
			IsActive: this.dataSearch.isActived != null ? this.dataSearch.isActived : '',
			UserId: this.dataSearch.userId != null ? this.dataSearch.userId : '',
		}
		this._service.chatbotGetList(req).subscribe((response) => {
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
		this.getAllDataActive()
		this.model = new ChatbotObject()
		this.rebuilForm()
		this.submitted = false
		this.titlePopup = 'Thêm mới câu hỏi'
		this.lstAnswer = []
		this.lstHashtagSelected = []
		this.hashtagId = null
		this.isAddButton = false
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
		this.model.lstAnswer = this.lstAnswer;
		this.model.lstHashtags = this.lstHashtagSelected;
		if (this.model.id == 0 || this.model.id == null) {
			this._service.chatbotInsertQuestion(this.model).subscribe((response) => {
				if (response.success == RESPONSE_STATUS.success) {
					if (response.result == -1) {
						this._toastr.error(MESSAGE_COMMON.EXISTED_NAME)
						$('#target').focus()
						return
					} else {
						$('#modal').modal('hide')
						this.lstAnswer = []
						this.lstHashtagSelected = []
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
						this.lstAnswer = []
						this.lstHashtagSelected = []
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
	isAddButton: boolean = false
	selectionString: string = ""
	mouseUpevent(event) {
		console.log(event)
		this.selectionString = window.getSelection().toString().trim()
		if (this.selectionString != null && this.selectionString != "" && this.selectionString.trim() != "") {
			this.isAddButton = true
			setTimeout(() => {
				var button = document.getElementById("addHashtagButton")
				button.style.left = (event.offsetX + 10) + "px"
				button.style.top = (event.offsetY - 10) + "px"
			}, 0);
		} else {
			this.isAddButton = false
		}
	}
	getAllDataActive() {
		let request = {}
		this._service.chatbotGetAllActive(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				this.lstQuestion = response.result.ChatbotGetAll
				this.lstHashtag = response.result.ListHashtag
			} else {
				this._toastr.error(response.message)
			}
		}),
			(error) => {
				console.error(error)
				alert(error)
			}
	}

	getListAnswer(id) {
		this.lstAnswer = []
		let request = {
			Id: id
		}
		this._service.chatbotLibGetById(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				this.lstAnswer = response.result.ChatbotLibGetByID
			} else {
				this._toastr.error(response.message)
			}
		}),
			(error) => {
				console.error(error)
				alert(error)
			}
	}

	modelHashTagAdd: HashtagObject = new HashtagObject()
	onCreateHashtag(e) {
		if (e.target.value != null && e.target.value != '' && e.target.value.trim() != '' && e.keyCode == 13) {
			var isExist = false
			for (var i = 0; i < this.lstHashtag.length; i++) {
				if (this.lstHashtag[i].text.toUpperCase() == e.target.value.trim().toUpperCase()) {
					isExist = true
					break
				}
			}
			if (isExist == false) {
				this.modelHashTagAdd = new HashtagObject()
				this.modelHashTagAdd.name = e.target.value
				this._serviceCatalog.hashtagChatbotInsert(this.modelHashTagAdd).subscribe((response) => {
					if (response.success == RESPONSE_STATUS.success) {
						this.hashtagId = response.result
						this.getAllDataActive()
					}
				}),
					(error) => {
						console.error(error)
					}
			}
		}
	}

	changeTypeChat() {
		if (this.model.typeChat == 1) {
			this.textAnswer = ""
			this.lstAnswer = []
		} else {
			this.lstHashtagSelected = []
			this.hashtagId = null
		}
	}

	addHashtagFromHighlight() {
		this.isAddButton = false
		if (this.selectionString.length > 50) {
			return this._toastr.error("Từ khóa được chọn không vượt quá 50 ký tự")
		}
		if (this.selectionString != "") {
			var isExist = false
			for (var i = 0; i < this.lstHashtag.length; i++) {
				if (this.lstHashtag[i].text.toUpperCase() == this.selectionString.toUpperCase()) {
					isExist = true
					var isExistSelected = false
					for (var j = 0; j < this.lstHashtagSelected.length; j++) {
						if (this.lstHashtagSelected[j].hashtagName.toUpperCase() == this.selectionString.toUpperCase()) {
							isExistSelected = true
							break
						}
					}
					if (!isExistSelected) {
						this.lstHashtagSelected.push({
							chatBotId: this.model.id,
							hashtagId: this.lstHashtag[i].value,
							hashtagName: this.lstHashtag[i].text,
						})
					}
					break
				}
			}
			if (isExist == false) {
				this.modelHashTagAdd = new HashtagObject()
				this.modelHashTagAdd.name = this.selectionString
				this._serviceCatalog.hashtagChatbotInsert(this.modelHashTagAdd).subscribe((response) => {
					if (response.success == RESPONSE_STATUS.success) {
						this.getAllDataActive()
						this.lstHashtagSelected.push({
							chatBotId: this.model.id,
							hashtagId: response.result,
							hashtagName: this.selectionString,
						})
					}
				}),
					(error) => {
						console.error(error)
					}
			}
		}
	}

	onAddHashtag() {
		var isExist = false
		for (var i = 0; i < this.lstHashtagSelected.length; i++) {
			if (this.lstHashtagSelected[i].hashtagId == this.hashtagId) {
				isExist = true
				break
			}
		}
		if (!isExist) {
			let hashtag = this.lstHashtag.find((x) => x.value == this.hashtagId)
			this.lstHashtagSelected.push({
				chatBotId: this.model.id,
				hashtagId: this.hashtagId,
				hashtagName: hashtag.text,
			})
		}
	}
	onRemoveHashtag(item: any) {
		let index = this.lstHashtagSelected.indexOf(item)
		this.lstHashtagSelected.splice(index, 1)
	}

	preUpdate(data) {
		this.isAddButton = false
		let request = {
			Id: data.id,
			Type: 1,
		}
		this._service.chatbotGetById(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				this.rebuilForm()
				this.titlePopup = 'Chỉnh sửa câu hỏi'
				this.model = response.result.ChatbotGetByID[0]
				this.lstHashtagSelected = response.result.ListChatbotHashtag
				this.getListAnswer(data.id)
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
		this.getAllDataActive()
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
		if (this.textAnswer == '') {
			this._toastr.error('Vui lòng nhập câu trả lời!')
			return
		}
		if (this.questionId) {
			let bussiness = this.lstQuestion.find((x) => x.id == this.questionId)
			if (bussiness) {
				let obj = { answer: this.textAnswer, idSuggetLibrary: this.questionId, questionAnswers: bussiness.question }
				this.lstAnswer.push(obj)
			}
		} else {
			let obj = { answer: this.textAnswer, idSuggetLibrary: this.questionId, questionAnswers: null }
			this.lstAnswer.push(obj)
		}
		this.textAnswer = '';
		this.questionId = null
	}

	onRemoveAnswer = (item: any) => {
		this.lstAnswer = this.lstAnswer.filter((x) => x.answer != item.answer && x.idSuggetLibrary != item.idSuggetLibrary)
		return
	}
}
