import { Component, OnInit, ViewChild, ElementRef } from '@angular/core'
import { FormGroup, FormBuilder, Validators } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'

import { defineLocale } from 'ngx-bootstrap/chronos'
import { viLocale } from 'ngx-bootstrap/locale'
import { BsDatepickerConfig, BsLocaleService } from 'ngx-bootstrap/datepicker'

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
defineLocale('vi', viLocale)

@Component({
	selector: 'app-invitation-detail',
	templateUrl: './invitation-detail.component.html',
	styleUrls: ['./invitation-detail.component.css'],
})
export class InvitationDetailComponent implements OnInit {
	model: InvitationObject = new InvitationObject()

	files: any[]
	title: string
	listStatus: any = [
		{ value: 1, text: 'Đang soạn thảo' },
		{ value: 2, text: 'Đã gửi' },
	]
	listUserIsSystem: any[]
	listItemUserSelected: Array<InvitationUserMapObject>
	userMap: any[]
	startDate: any
	endDate: any
	statusCurent: any = 2
	@ViewChild('file', { static: false }) public file: ElementRef
	constructor(
		private router: Router,
		private invitationService: InvitationService,
		private userService: UserService,
		private activatedRoute: ActivatedRoute,
		private fileService: UploadFileService,
		private BsLocaleService: BsLocaleService
	) {
		this.listItemUserSelected = []
		this.files = []
		this.userMap = []
	}

	ngOnInit() {
		this.BsLocaleService.use('vi')
		this.getUsersIsSystem()
		this.getInvitatonModelById()
		// this.getInvitatonModelById()
	}

	// statusDisable() {
	// 	if (this.statusCurent == 2) {
	// 		return true
	// 	}
	// 	return false
	// }

	// ngAfterViewInit() {
	// 	$('#endDate').datepicker({
	// 		language: 'vi',
	// 	})
	// }

	getInvitatonModelById() {
		this.activatedRoute.params.subscribe((params) => {
			let id = +params['id']
			this.model.id = isNaN(id) == true ? 0 : id
			if (this.model.id != 0) {
				this.invitationService.invitationGetById({ id: this.model.id }).subscribe((res) => {
					if (res.success == RESPONSE_STATUS.success) {
						if (res.result) {
							this.listItemUserSelected = []
							this.model = { ...res.result.model }
							this.startDate = this.model.startDate.toString().substring(0, 10).replace('-', '/').replace('-', '/')
							this.endDate = this.model.endDate.toString().substring(0, 10).replace('-', '/').replace('-', '/')
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

	redirectList() {
		this.router.navigate(['quan-tri/thu-moi'])
	}
}
