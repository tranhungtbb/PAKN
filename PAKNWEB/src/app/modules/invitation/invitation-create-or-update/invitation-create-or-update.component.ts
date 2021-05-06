import { Component, OnInit, ViewChild, ElementRef } from '@angular/core'
import { FormGroup, FormBuilder, Validators } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'

import { Router, ActivatedRoute } from '@angular/router'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { CONSTANTS, STATUS_HISNEWS, FILETYPE } from 'src/app/constants/CONSTANTS'
import { COMMONS } from 'src/app/commons/commons'
import { RoleObject } from 'src/app/models/roleObject'
import { InvitationService } from 'src/app/services/invitation.service'
import { InvitationObject, InvitationUserMapObject } from 'src/app/models/invitationObject'
import { from } from 'rxjs'
import { UserService } from 'src/app/services/user.service'
import { UploadFileService } from 'src/app/services/uploadfiles.service'
import { iterator } from 'rxjs/internal-compatibility'

declare var $: any

@Component({
	selector: 'app-invitation-create-or-update',
	templateUrl: './invitation-create-or-update.component.html',
	styleUrls: ['./invitation-create-or-update.component.css'],
})
export class InvitationCreateOrUpdateComponent implements OnInit {
	model: InvitationObject = new InvitationObject()
	sendEmail: boolean = false
	sendSMS: boolean = false
	form: FormGroup
	submitted = false
	action: any
	files: any[]
	lstFileDelete: any[]
	title: string
	listStatus: any = [
		{ value: 1, text: 'Đang soạn thảo' },
		{ value: 2, text: 'Đã gửi' },
	]
	listUserIsSystem: any[]
	listItemUserSelected: Array<InvitationUserMapObject>
	userMap: any[]
	key: any = ''
	fileAccept = CONSTANTS.FILEACCEPT
	userId: any
	@ViewChild('file', { static: false }) public file: ElementRef
	constructor(
		private _toastr: ToastrService,
		private formBuilder: FormBuilder,
		private router: Router,
		private invitationService: InvitationService,
		private userService: UserService,
		private activatedRoute: ActivatedRoute,
		private fileService: UploadFileService
	) {
		this.listItemUserSelected = []
		this.files = []
		this.userMap = []
		this.lstFileDelete = []
	}

	ngOnInit() {
		this.getUsersIsSystem()
		this.buildForm()
		// this.getInvitatonModelById()
	}

	buildForm() {
		this.form = this.formBuilder.group({
			title: [this.model.title, Validators.required],
			startDate: [this.model.startDate, Validators.required],
			endDate: [this.model.endDate, Validators.required],
			content: [this.model.content, Validators.required],
			place: [this.model.place, Validators.required],
			note: [this.model.note],
			userId: [this.userId],
			sendEmail: [this.sendEmail],
			sendSMS: [this.sendSMS],
		})
	}

	getInvitatonModelById() {
		this.activatedRoute.params.subscribe((params) => {
			let id = +params['id']
			this.model.id = isNaN(id) == true ? 0 : id
			if (this.model.id != 0) {
				this.invitationService.invitationGetById({ id: this.model.id }).subscribe((res) => {
					if (res.success == RESPONSE_STATUS.success) {
						if (res.result) {
							this.model = { ...res.result.model }
							this.model.startDate = new Date(this.model.startDate)
							this.model.endDate = new Date(this.model.endDate)
							this.files = res.result.invFileAttach
							for (const iterator of res.result.invitationUserMap) {
								let item = this.listUserIsSystem.find((x) => x.id == iterator.userId)
								var obj = new InvitationUserMapObject()
								obj.userId = iterator.userId
								obj.sendEmail = iterator.sendEmail
								obj.sendSMS = iterator.sendSMS
								obj.fullName = item.fullName
								obj.unitName = item.unitName
								obj.positionName = item.positionName
								obj.avatar = item.avatar
								this.listItemUserSelected.push(obj)
							}
						}
					}
				})
			}
		})
		this.action = this.model.id == 0 ? 'Thêm mới' : 'Cập nhập'
		this.title = this.model.id == 0 ? 'Thêm mới thư mời ' : 'Cập nhập thư mời'
	}

	getUsersIsSystem() {
		this.userService.getIsSystem2({}).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				this.listUserIsSystem = res.result.SYUserGetIsSystem2
				this.getInvitatonModelById()
			} else {
				this.listUserIsSystem = []
			}
		})
	}

	get f() {
		return this.form.controls
	}

	rebuilForm() {
		this.form.reset({
			title: this.model.title,
			startDate: this.model.startDate,
			endDate: this.model.endDate,
			content: this.model.content,
			place: this.model.place,
			note: this.model.note,
			userId: this.userId,
			sendEmail: this.sendEmail,
			sendSMS: this.sendSMS,
		})
	}

	onSave(isSend: boolean) {
		this.model.status = isSend == false ? 1 : 2
		this.submitted = true
		if (this.listItemUserSelected.length == 0) {
			this._toastr.error('Vui lòng chọn người tham dự')
			return
		}
		this.model.title = this.model.title.trim()
		this.model.content = this.model.content.trim()
		this.model.place = this.model.place.trim()
		this.rebuilForm()

		if (this.form.invalid) {
			return
		}
		for (const i of this.listItemUserSelected) {
			let item = {
				UserId: i.userId,
				InvitationId: 0,
				Watched: false,
				SendEmail: i.sendEmail,
				SendSMS: i.sendSMS,
			}
			this.userMap.push(item)
		}
		var obj = {
			model: this.model,
			Files: this.files,
			userMap: this.userMap,
			lstFileDelete: this.lstFileDelete,
		}

		if (this.model.id == 0 || this.model.id == null) {
			this.invitationService.invitationInsert(obj).subscribe((response) => {
				if (response.success == RESPONSE_STATUS.success) {
					this._toastr.success(COMMONS.ADD_SUCCESS)
					this.redirectList()
					return
				} else {
					this._toastr.error(response.message)
					return
				}
			}),
				(error) => {
					console.error(error)
					alert(error)
				}
		} else {
			this.invitationService.invitationUpdate(obj).subscribe((response) => {
				if (response.success == RESPONSE_STATUS.success) {
					this._toastr.success(COMMONS.UPDATE_SUCCESS)
					this.redirectList()
					return
				} else {
					this._toastr.error(response.message)
					return
				}
			}),
				(error) => {
					console.error(error)
					alert(error)
				}
		}
	}
	redirectList() {
		this.router.navigate(['quan-tri/thu-moi'])
	}

	onCreateUser() {
		if (this.userId != undefined) {
			if (this.listItemUserSelected.length == 0) {
				let item = this.listUserIsSystem.find((x) => x.id == this.userId)
				var obj = new InvitationUserMapObject()
				obj.userId = this.userId
				obj.sendEmail = this.sendEmail
				obj.sendSMS = this.sendSMS
				obj.fullName = item.fullName
				obj.unitName = item.unitName
				obj.positionName = item.positionName
				obj.avatar = item.avatar
				this.listItemUserSelected.push(obj)
			} else {
				let check = this.listItemUserSelected.find((x) => x.userId == this.userId)
				if (check != undefined) {
					this._toastr.error('Bạn đã chọn người này')
					return
				}
				let item = this.listUserIsSystem.find((x) => x.id == this.userId)
				var obj = new InvitationUserMapObject()
				obj.userId = this.userId
				obj.sendEmail = this.sendEmail
				obj.sendSMS = this.sendSMS
				obj.unitName = item.unitName
				obj.fullName = item.fullName
				obj.positionName = item.positionName
				obj.avatar = item.avatar
				this.listItemUserSelected.push(obj)
			}
		} else {
			this._toastr.error('Vui lòng chọn người tham dự')
			return
		}
	}
	onRemoveUser(item: any) {
		this.listItemUserSelected = this.listItemUserSelected.filter((x) => x.userId != item.userId)
		return
	}

	onUpdateUser(userId: any, isSendSMS: boolean) {
		let item = this.listItemUserSelected.find((x) => x.userId == userId)
		if (item != undefined) {
			if (isSendSMS == true) {
				item.sendSMS = !item.sendSMS
			} else {
				item.sendEmail = !item.sendEmail
			}
			let index = this.listItemUserSelected.indexOf(item)
			this.listItemUserSelected.splice(index, 1, item)
		}
		return
	}

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
					this._toastr.error('Định dạng không được hỗ trợ')
				}
			}
		} else if (check === 2) {
			this._toastr.error('Không được phép đẩy trùng tên file lên hệ thống')
		} else {
			this._toastr.error('File tải lên vượt quá dung lượng cho phép 10MB')
		}
		this.file.nativeElement.value = ''
	}

	onRemoveFile(args) {
		const index = this.files.indexOf(args)
		const file = this.files[index]
		this.lstFileDelete.push(file)
		this.files.splice(index, 1)
	}
}
