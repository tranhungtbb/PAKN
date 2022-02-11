import { Component, OnInit, ViewChild, ElementRef } from '@angular/core'
import { FormGroup, FormBuilder, Validators } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'
import { defineLocale } from 'ngx-bootstrap/chronos'
import { viLocale } from 'ngx-bootstrap/locale'
import { BsLocaleService } from 'ngx-bootstrap/datepicker'
import { TreeviewItem, TreeviewConfig } from 'ngx-treeview'
import { TreeviewI18n } from 'ngx-treeview'

import { Router, ActivatedRoute } from '@angular/router'
import { CONSTANTS, FILETYPE, USER_TYPE, RESPONSE_STATUS, MY_FORMATS } from 'src/app/constants/CONSTANTS'
import { COMMONS } from 'src/app/commons/commons'
import { InvitationService } from 'src/app/services/invitation.service'
import { InvitationObject, InvitationUserMapObject, InvitationMapObject } from 'src/app/models/invitationObject'
import { UserService } from 'src/app/services/user.service'
import { UploadFileService } from 'src/app/services/uploadfiles.service'
import { DefaultTreeviewI18n } from 'src/app/shared/default-treeview-i18n'
import * as moment from 'moment'
import { DateAdapter, MAT_DATE_FORMATS, ThemePalette } from '@angular/material/core'
import { NGX_MAT_DATE_FORMATS } from '@angular-material-components/datetime-picker'


declare var $: any
defineLocale('vi', viLocale)

@Component({
	selector: 'app-invitation-create-or-update',
	templateUrl: './invitation-create-or-update.component.html',
	styleUrls: ['./invitation-create-or-update.component.css'],
	providers: [
		{ provide: TreeviewI18n, useClass: DefaultTreeviewI18n },
		{ provide: NGX_MAT_DATE_FORMATS, useValue: MY_FORMATS }
	],
})
export class InvitationCreateOrUpdateComponent implements OnInit {
	model: InvitationObject = new InvitationObject()
	sendEmail: boolean = true
	sendSMS: boolean = true
	form: FormGroup
	submitted = false
	action: any
	files: any[]
	lstFileDelete: any[]
	title: string = 'Tạo thư mời'
	listStatus: any = [
		{ value: 1, text: 'Đang soạn thảo' },
		{ value: 2, text: 'Đã gửi' },
	]
	listUserIsSystem: Array<UserIsSystem>
	listItemUserSelected: Array<InvitationUserMapObject> = []
	userMap: any[]
	key: any = ''
	fileAccept = CONSTANTS.FILEACCEPT
	listUserSelected: Array<UserIsSystem>
	statusCurent: any = 1
	listIdUserSelected: any[] = []

	lstIndividual: Array<InvitationMapObject>
	lstBussiness: Array<InvitationMapObject>
	bussinessId: any[] = []
	individualId: any[] = []
	lstIndividualBussiness: any[] = []
	@ViewChild('file', { static: false }) public file: ElementRef

	// datepicker
	public date: moment.Moment
	public disabled = false
	public showSpinners = true
	public showSeconds = false
	public touchUi = false
	public enableMeridian = false
	public stepMinute = 5
	public stepHour = 1
	public color: ThemePalette = 'primary'

	// treeview

	items: TreeviewItem[]
	values: number[]
	config = TreeviewConfig.create({
		hasAllCheckBox: true,
		hasFilter: true,
		hasCollapseExpand: true,
		decoupleChildFromParent: false,
		maxHeight: 400,
	})
	onFilterChange(value: string): void { }
	onSelectedChange(values: []) {
		this.listUserSelected = []
		if (values.length > 0) {
			for (const iterator of values) {
				let check = this.listUserIsSystem.find((x) => x.id == iterator)
				if (check != null || check != undefined) {
					this.listUserSelected.push(check)
				}
			}
			return
		}
	}

	constructor(
		private _toastr: ToastrService,
		private formBuilder: FormBuilder,
		private router: Router,
		private invitationService: InvitationService,
		private userService: UserService,
		private activatedRoute: ActivatedRoute,
		private fileService: UploadFileService,
		private BsLocaleService: BsLocaleService,
	) {
		this.listItemUserSelected = []
		this.files = []
		this.userMap = []
		this.lstFileDelete = []
		this.action = 'Lưu'
		this.listUserSelected = []
	}

	ngOnInit() {
		this.BsLocaleService.use('vi')
		this.buildForm()
		this.userService.getIsSystem({}).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				this.listUserIsSystem = res.result.SYUserGetIsSystem
				this.getInvitatonModelById()
			} else {
				this.listUserIsSystem = []
			}
		})
		this.items = []
		this.invitationService.GetDataForCreate({}).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				this.lstIndividual = res.result.listIndividual.map(item => {
					item.title = item.title + ' - ' + item.phone
					return item
				})
				this.lstBussiness = res.result.listBusiness.map(item => {
					item.title = item.title + ' - ' + item.phone
					return item
				})
			}
		})
	}

	buildForm() {
		this.form = this.formBuilder.group({
			title: [this.model.title, [Validators.required]],
			startDate: [this.model.startDate, [Validators.required]],
			endDate: [this.model.endDate, [Validators.required]],
			content: [this.model.content, [Validators.required]],
			place: [this.model.place, [Validators.required]],
			note: [this.model.note],
			listUserSelected: [this.listUserSelected],
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
							this.statusCurent = this.model.status

							for (const iterator of res.result.invitationUserMap) {
								if (iterator.type == USER_TYPE.SYSTEM) {
									// hệ thống
									let item = this.listUserIsSystem.find((x) => x.id == iterator.userId && iterator.type == 1)
									if (item) {
										var obj = new InvitationUserMapObject()
										obj.userId = iterator.userId
										obj.sendEmail = iterator.sendEmail
										obj.sendSMS = iterator.sendSMS
										obj.fullName = item.fullName
										obj.unitName = item.unitName
										obj.positionName = item.positionName
										if (item.avatar == null || item.avatar == '') {
											obj.avatar = ''
										} else {
											obj.avatar = item.avatar
										}
										this.listItemUserSelected.push(obj)
										this.listIdUserSelected.push(iterator.userId)
									}
								} else {
									let itemBI = iterator.type == USER_TYPE.INDIVIDUAL ? this.lstIndividual.find((x) => x.id == iterator.userId) : this.lstBussiness.find((x) => x.id == iterator.userId)

									if (itemBI) {
										debugger
										iterator.type == USER_TYPE.INDIVIDUAL ?
											this.individualId.push(itemBI) : this.bussinessId.push(itemBI)

										this.lstIndividualBussiness.push({ ...itemBI, sendEmail: iterator.sendEmail, sendSMS: iterator.sendSMS })
									}
								}
							}
							if (this.statusCurent == 2) {
								this.title = 'Chi tiết thư mời'
							} else {
								this.title = this.model.id == 0 ? 'Tạo thư mời ' : 'Cập nhật thư mời'
							}
						}
					}
				})
			}
			this.getUsersIsSystem()
		})
	}

	getUsersIsSystem() {
		// get drop list
		this.userService.getIsSystemOrderByUnit({}).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				for (const iterator of res.result) {
					iterator.checked = this.listIdUserSelected.includes(iterator.value) == true ? true : false
					if (iterator.children == null || iterator.children.length == 0) {
						delete iterator.children
					} else {
						for (const iterator2 of iterator.children) {
							iterator2.checked = this.listIdUserSelected.includes(iterator2.value) == true ? true : false
							if (iterator2.children == null || iterator2.children.length == 0) {
								delete iterator2.children
							} else {
								for (const iterator3 of iterator2.children) {
									iterator3.checked = this.listIdUserSelected.includes(iterator3.value) == true ? true : false
									if (iterator3.children == null || iterator3.children.length == 0) {
										delete iterator3.children
									} else {
										for (const iterator4 of iterator3.children) {
											iterator4.checked = this.listIdUserSelected.includes(iterator4.value) == true ? true : false
										}
									}
								}
							}
						}
					}
				}
				for (const iterator of res.result) {
					this.items.push(new TreeviewItem({ ...iterator }))
				}
			} else {
				this.items = []
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
			listUserSelected: this.listUserSelected,
			sendEmail: this.sendEmail,
			sendSMS: this.sendSMS,
		})
	}
	checkDate: boolean
	onSave(isSend: boolean) {
		this.checkDate = true
		this.model.status = isSend == false ? 1 : 2
		this.submitted = true

		this.model.title = this.model.title.trim()
		this.model.content = this.model.content.trim()
		this.model.place = this.model.place.trim()
		if (this.model.startDate && this.model.endDate) {
			if (this.model.startDate > this.model.endDate) {
				this.checkDate = false
				return
			}
		}
		if (this.form.invalid) {
			return
		}
		if (this.listItemUserSelected.length == 0 && this.lstIndividualBussiness.length == 0) {
			this._toastr.error('Vui lòng chọn người tham dự')
			return
		}
		this.userMap = []
		for (const i of this.listItemUserSelected) {
			let item = {
				UserId: i.userId,
				InvitationId: 0,
				Watched: false,
				SendEmail: i.sendEmail,
				SendSMS: i.sendSMS,
				Type: USER_TYPE.SYSTEM,
			}
			this.userMap.push(item)
		}

		for (const i of this.lstIndividualBussiness) {
			let item = {
				UserId: i.id,
				InvitationId: 0,
				Watched: false,
				SendEmail: i.sendEmail,
				SendSMS: i.sendSMS,
				Type: i.type,
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
					let res = isNaN(response.result) == true ? 0 : response.result
					if (res == -1) {
						this._toastr.error('Tiêu đề thư mời đã tồn tại')
						return
					} else {
						this._toastr.error(response.message)
						return
					}
				}
			}),
				(error) => {
					console.error(error)
					alert(error)
				}
		} else {
			this.invitationService.invitationUpdate(obj).subscribe((response) => {
				if (response.success == RESPONSE_STATUS.success) {
					if (response.result == -1) {
						this._toastr.error('Tiêu đề thư mời đã tồn tại')
					} else {
						if (isSend == true) {
							this._toastr.success('Gửi thư mời thành công')
						} else {
							this._toastr.success(COMMONS.UPDATE_SUCCESS)
						}

						this.redirectList()
						return
					}
				} else {
					let res = isNaN(response.result) == true ? 0 : response.result
					if (res == -1) {
						this._toastr.error('Tiêu đề thư mời đã tồn tại')
						return
					} else {
						this._toastr.error(response.message)
						return
					}
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

	onCreateBussiness = () => {
		if (!this.individualId && !this.bussinessId) {
			this._toastr.error('Vui lòng chọn cá nhân doanh nghiệp')
			return
		}
		this.lstIndividualBussiness = []
		if (this.individualId) {
			this.individualId.forEach(item => {
				let obj = { ...item, sendEmail: this.sendEmail, sendSMS: this.sendSMS }
				this.lstIndividualBussiness.push(obj)
			})
		}
		if (this.bussinessId) {
			this.bussinessId.forEach(item => {
				let obj = { ...item, sendEmail: this.sendEmail, sendSMS: this.sendSMS }
				this.lstIndividualBussiness.push(obj)
			})
		}
		// this.bussinessId = []
		// this.individualId = []
	}

	onCreateUser() {
		if (this.sendEmail == false && this.sendSMS == false) {
			this._toastr.error('Vui lòng chọn Gửi Mail,SMS')
			return
		}
		let i = 0
		if (this.listUserSelected != undefined && this.listUserSelected.length > 0) {
			this.listItemUserSelected = []
			for (const iterator of this.listUserSelected) {
				var obj = new InvitationUserMapObject()
				obj.userId = iterator.id
				obj.sendEmail = this.sendEmail
				obj.sendSMS = this.sendSMS
				obj.unitName = iterator.unitName
				obj.fullName = iterator.fullName
				obj.positionName = iterator.positionName
				if (iterator.avatar == null || iterator.avatar == '') {
					obj.avatar = ''
				} else {
					obj.avatar = iterator.avatar
				}

				this.listItemUserSelected.push(obj)
				i++
			}
			if (i == 0) {
				this._toastr.error('Bạn đã chọn những người tham dự này')
				return
			}
			this._toastr.success('Thêm mới thành công ' + i + ' người tham dự')
		} else {
			this._toastr.error('Vui lòng chọn người tham dự')
			return
		}
	}
	onRemoveUser(item: any) {
		this.listItemUserSelected = this.listItemUserSelected.filter((x) => x.userId != item.userId)
		return
	}

	onRemoveIndividualBussiness = (item: any) => {
		this.lstIndividualBussiness = this.lstIndividualBussiness.filter((x) => x.id != item.id && x.type != item.type)
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
		if (this.statusCurent == 2) {
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
		if (this.statusCurent == 2) {
			return
		}
		const index = this.files.indexOf(args)
		const file = this.files[index]
		this.lstFileDelete.push(file)
		this.files.splice(index, 1)
	}
	getShortName(string) {
		if (!string) {
			return
		}
		var names = string.split(' '),
			initials = names[0].substring(0, 1).toUpperCase()
		if (names.length > 1) {
			initials += names[names.length - 1].substring(0, 1).toUpperCase()
		}
		return initials
	}
}

interface UserIsSystem {
	avatar: string
	email: string
	fullName: string
	id: number
	phone: string
	positionName: string
	unitName: string
	userName: string
}
