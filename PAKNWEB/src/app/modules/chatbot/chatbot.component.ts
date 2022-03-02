import { Component, ElementRef, OnInit, ViewChild } from '@angular/core'
import { FILETYPE, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { BotMessage, BotRoom } from 'src/app/models/chatbotObject'
import { ChatBotService } from './chatbot.service'
import * as signalR from '@aspnet/signalr/'
import { AppSettings } from 'src/app/constants/app-setting'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { UserService } from 'src/app/services/user.service'
import { ToastrService } from 'ngx-toastr'
import { ActivatedRoute } from '@angular/router'
import { saveAs as importedSaveAs } from 'file-saver'
import { UploadFileService } from 'src/app/services/uploadfiles.service'

@Component({
	selector: 'app-dashboard',
	templateUrl: './chatbot.component.html',
	styleUrls: ['./chatbot.component.css'],
})
export class DashboardChatBotComponent implements OnInit {
	channel: any
	roomNameSelected: string
	messages: any = []
	newMessage = ''
	rooms: BotRoom[]
	connection: signalR.HubConnection
	pageIndex: number = 1
	pageSize: number = 10
	totalMessage: number = 0
	roomActive: number = 0
	botEnable: boolean = true
	userId: number
	model: any = {}
	userAvatar: string
	audio: any
	roomsShow: any[] = []

	currentDate: any = new Date()

	@ViewChild('boxChat', { static: true }) private boxChat: ElementRef

	constructor(private botService: ChatBotService, private fileService: UploadFileService, private userService: UserService, private user: UserInfoStorageService, private toast: ToastrService, private activatedRoute: ActivatedRoute) { }
	async ngOnInit() {
		this.audio = new Audio()
		this.audio.src = '../../../assets/img/ring.mp3'
		this.audio.loop = true

		await this.botService.getRoomForNotification({}).toPromise().then(res => {
			if (res.success == RESPONSE_STATUS.success) {

				this.roomsShow = res.result.ListRoomIsShow
			} else {
				this.toast.error(res.result.message)
			}
		}).catch(err => {
			this.toast.error(err)
		})
		this.userId = this.user.getUserId()
		this.userService.getById({ id: this.userId }).subscribe((res) => {
			this.model = res.result.SYUserGetByID[0]
			//console.log('userService ', this.model);
			if (this.model.avatar == '' || this.model.avatar == null) {
				this.userAvatar = ''
			} else {
				this.userAvatar = this.model.avatar
			}
		})
		this.connection = new signalR.HubConnectionBuilder()
			.withUrl(`${AppSettings.SIGNALR_ADDRESS}?sysUserName=${this.userId}`, {
				skipNegotiation: true,
				transport: signalR.HttpTransportType.WebSockets,
			})
			.configureLogging(signalR.LogLevel.Information)
			.withAutomaticReconnect()
			.build()
		this.connection.start().then(() => {
			this.connection.on('ReceiveMessageToGroup', (data: any) => {
				console.log('ngOnInit SignalR ReceiveMessageToGroup 1', data, this.roomNameSelected, this.userId)
				debugger
				if ((data.type === 'Conversation' || data.type === 'File') && this.roomNameSelected && this.roomNameSelected === data.to && `${this.userId}` !== data.from) {

					const answers = [];
					if (data.results) {
						console.log('answers 2', data.results)
						for (let ind = 0; ind < data.results.length; ind++) {
							const el = data.results[ind]
							if (el.subTags !== '') {
								const subTags = JSON.parse(el.subTags)
								console.log('answers 3', subTags)
								answers.push({ answer: el.answer, subTags: subTags })
							}
						}
					}

					console.log('answers 4', answers)
					const newMessage = {
						dateSent: data.timestamp,
						messageContent: data.type != 'File' ? data.content : JSON.parse(data.content),
						type: data.type,
						answers: answers,
						link: "",
						fromUserName: data.from,
						fromAvatarPath: data.fromAvatarPath ? data.fromAvatarPath : '',
						fromFullName: data.fromFullName,
						toUserName: data.to,
						fromUserId: data.fromId,
						dateSend: data.dateSend
					}
					this.messages = [...this.messages, newMessage]

					console.log('ngOnInit SignalR ReceiveMessageToGroup 2', this.messages)
				}

				this.convertMessageToObjectList()
			})

			this.connection.on('ReceiveRoomToGroup', data => {
				this.rooms.unshift(data)
			})

			// this.connection.on('BroadcastMessage', (data: any) => {
			// 	//console.log('ngOnInit SignalR BroadcastMessage ', data)
			// 	this.fetchRooms()
			// })
			this.connection.on('OnNewMessage', (room: any) => {
				let roomCheck: any = this.rooms.find(x => x.id === room.id)
				if (roomCheck) {
					this.rooms.splice(this.rooms.indexOf(roomCheck), 1)
					this.rooms.unshift({ ...room, 'type': roomCheck.type })
				} else {
					this.rooms.unshift(room)
				}
			})


			this.connection.on('NotifyAdmin', (data: any) => {
				data = { ...data, 'type': 2 }
				console.log('ngOnInit SignalR NotifyAdmin ', data)
				if (!this.roomsShow.find(x => x.name === data.name)) {
					this.roomsShow.unshift(data)
				}
				let room: any = this.rooms.find(x => x.name === data.name)
				if (room) {
					this.rooms.splice(this.rooms.indexOf(room), 1)
					this.rooms.unshift(data)

				} else {
					this.botService.getRoomById({ Id: data.id }).subscribe(res => {
						if (res.success == RESPONSE_STATUS.success) {
							if (res.result) {
								this.rooms.unshift({ ...res.result, 'type': 2 })

							}

						} else {
							this.toast.error(res.message)
						}
					})
				}
				this.playSoundWarning()
			})
		})
		this.fetchRooms()


		this.activatedRoute.params.subscribe((params) => {
			if (params['roomId']) {
				this.botService.getRoomById({ Id: params['roomId'] }).subscribe(res => {
					if (res.success == RESPONSE_STATUS.success) {
						let room = res.result
						if (room) {
							this.resetGetMessage(params['roomId'], room.name, room.type)
						}

					} else {
						this.toast.error(res.message)
					}
				})
			}
		})
	}

	updateStatus(data: any, selectedRoom: boolean = false) {
		let index = this.roomsShow.indexOf(data);
		this.roomsShow.splice(index, 1)
		this.botService.updateStatusRoom({ roomId: data.id }).subscribe()
		if (selectedRoom) {
			this.resetGetMessage(data.id, data.name, data.type)
		}
	}

	// swapArr(index: number, data: any) {
	// 	[this.rooms[0], this.rooms[index]] = [data, this.rooms[0]]
	// }

	playSoundWarning() {
		try {
			console.log('playSoundWarning ')
			this.audio = new Audio()
			this.audio.src = '../../assets/img/ring.mp3'
			this.audio.load()
			this.audio.play()
		} catch (error) {
			console.log('playSoundWarning error', error)
		}
	}

	fetchRooms = () => {
		this.roomTitle = this.roomTitle == null ? '' : this.roomTitle.trim()
		let obj = {
			Title: this.roomTitle,
			CreatedDate: this.createDate == null ? '' : this.createDate.toDateString()
		}
		console.log(obj)
		this.botService.getRooms(obj).subscribe((res) => {
			if (res != 'undefined' && res.success == RESPONSE_STATUS.success) {
				if (res.result) {
					console.log('fetchRooms ', res.result.Data)
					this.rooms = res.result.Data
				}
			} else {
			}
		})
	}

	resetGetMessage(roomId: number, roomName: string, type: number) {
		this.roomActive = roomId
		this.botEnable = type == 1 ? true : false
		this.pageIndex = 1
		this.roomNameSelected = roomName
		this.getMessage(roomId, roomName)
	}
	async getMessage(roomId: number, roomName: string) {
		try {
			const request = {
				RoomId: roomId,
				PageIndex: this.pageIndex,
				PageSize: this.pageSize,
			}
			console.log('getMessage ', roomName)
			this.connection.invoke('JoinToRoom', roomName)
			this.botService.getMessages(request).subscribe((result) => {
				console.log('getMessage 1', result)
				let res = { ...result }

				if (res != 'undefined' && res.success == RESPONSE_STATUS.success) {
					if (res.result && res.result.length > 0) {

						console.log('getMessage result : ' + res.result)
						if (this.pageIndex == 1) {
							this.messages = res.result.reverse()
						} else {
							this.messages = [res.result, ...this.messages].reverse()
						}
						if (this.messages != null && this.messages.length > 0) {
							this.totalMessage = this.messages[0].rowNumber
						}

						this.convertMessageToObjectList()
						this.scrollToBottom()
					}
					else {
						this.messages = []
					}
				} else {
					this.messages = []
				}
			})
		} catch (error) {
			console.log('handleConnect ', error)
		}
	}

	scrollToBottom(): void {
		try {
			//console.log('height scroll:' + this.boxChat.nativeElement.scrollHeight)
			this.boxChat.nativeElement.scrollTop = this.boxChat.nativeElement.scrollHeight
		} catch (err) { }
	}

	sendMessage() {
		if (this.newMessage !== '') {
			//	this.playSoundWarning();
			console.log('sendMessage ', this.roomNameSelected, this.rooms)
			this.connection.invoke('AdminSendToRoom', this.roomNameSelected, this.newMessage)

			if (this.rooms.filter((room) => room.name === this.roomNameSelected).length > 0) {
				this.rooms.find((room) => room.name === this.roomNameSelected).type = 2
			}
			this.botEnable = false

			this.messages = [...this.messages, { messageContent: this.newMessage, fromUserId: this.userId, fromAvatar: this.userAvatar }]
			this.newMessage = ''

			//	this.audio.stop();
		}
	}

	onKeyDown() {
		this.newMessage = this.newMessage == null ? '' : this.newMessage.trim()
		if (this.newMessage === '' && this.files.length == 0) {
			this.toast.error('Vui lòng nhập nội dung')
			return
		}
		this.sendMessage()
		this.sendFile()
	}
	ngOnDestroy() {
		if (this.connection) {
			console.log('SignalR ngOnDestroy 0')
			this.connection.off('ReceiveMessageToGroup')
			this.connection.off('BroadcastMessage')
			this.connection.off('ReceiveRoomToGroup')
		}
	}

	changeRoomStatus() {
		this.botEnable = !this.botEnable
		console.log('changeRoomStatus ', this.roomNameSelected, this.botEnable)
		this.connection.invoke('EnableBot', this.roomNameSelected, this.botEnable)
	}

	onScrollBoxChat(event: any) {
		// if (event.target.scrollTop == 0) {
		// 	if (this.pageIndex * this.pageSize < this.totalMessage) {
		// 		this.pageIndex++
		// 		this.getMessage(this.roomActive, this.roomNameSelected)
		// 	}
		// }
	}

	roomTitle: string
	createDate: Date = null


	dateValueChange(events) {
		if (events) {
			this.createDate = events
		} else {
			this.createDate = null
		}
		this.fetchRooms()
	}

	convertMessageToObjectList() {
		try {
			if (this.messages) {
				for (let index = 0; index < this.messages.length; index++) {
					let element = this.messages[index]
					let result, type

					if (element.xresults) {
						result = element.xresults
						type = 'json'
					} else {
						const rs = this.stringToObject(element.messageContent)
						result = rs.result
						type = rs.type
					}

					element.fromAvatar = element.fromAvatar ? element.fromAvatar : ''
					element.fromFullName = element.fromFullName ? element.fromFullName : ''
					const answers = []
					element.type = type
					if (type === 'string') {
						element.messageContent = result
						if (Array.isArray(result)) {
							element.type = 'File'
						}
					} else if (type === 'json') {
						console.log('answers ', result)
						if (Array.isArray(result)) {
							element.messageContent = result
							element.type = 'File'
						} else {
							if (result.Results && result.Results.length > 0) {
								console.log('answers 1', result.Results)
								try {
									for (let ind = 0; ind < result.Results.length; ind++) {
										let el = result.Results[ind]
										if (el.SubTags !== '') {
											const subTags = JSON.parse(el.SubTags)
											console.log('answers 2', subTags)
											answers.push({ answer: el.Answer, subTags: subTags })
										}
									}
								} catch (error) { }
							}
							element.messageContent = ''
						}


					}
					element.answers = answers
				}
			}
			console.log('ReceiveMessageToGroup 2', this.messages)
		} catch (error) { }
	}

	stringToObject(string) {
		if (string) {
			try {
				const result = JSON.parse(string)
				return { result: result, type: 'json' }
			} catch (error) {
				return { result: string, type: 'string' }
			}
		}
	}


	files: any[] = []
	@ViewChild('file', { static: false }) public file: ElementRef

	onUpload(event) {
		if (event.target.files.length == 0) {
			return
		}
		const check = this.fileService.checkFileWasExitsted(event, this.files)
		if (check === 1) {
			for (let item of event.target.files) {
				FILETYPE.forEach((fileType) => {
					if (item.type == fileType.text) {
						let max = this.files.reduce((a, b) => {
							return a.id > b.id ? a.id : b.id
						}, 0)
						item.id = max + 1
						item.fileType = fileType.value
						this.files.push(item)
					}
				})
				if (!item.fileType) {
					this.toast.error('Định dạng không được hỗ trợ')
				}
			}
		} else if (check === 2) {
			this.toast.error('Không được phép đẩy trùng tên file lên hệ thống')
		} else {
			this.toast.error('File tải lên vượt quá dung lượng cho phép 10MB')
		}
		this.file.nativeElement.value = ''
	}

	onRemoveFile(index: number) {
		this.files.splice(index, 1)
	}


	sendFile() {
		if (this.files.length == 0) { return }
		let obj = {
			files: this.files,
			roomName: this.roomNameSelected
		}
		this.botService.clientSendFile(obj).subscribe(res => {
			if (res.success == RESPONSE_STATUS.success) {

				setTimeout(() => {
					var objDiv = document.getElementById('bodyMessage')
					objDiv.scrollTop = objDiv.scrollHeight
				}, 300)


				this.file.nativeElement.value = ''
				this.files = []
			} else {
				this.toast.error(res.message)
			}
		})

	}
	DownloadFile(file: any) {
		var request = {
			Path: file.FilePath,
			Name: file.Name,
		}
		this.fileService.downloadFile(request).subscribe(
			(response) => {
				var blob = new Blob([response], { type: response.type })
				importedSaveAs(blob, file.name)
			},
			(error) => {
				this.toast.error('Không tìm thấy file trên hệ thống')
			}
		)
	}
}
