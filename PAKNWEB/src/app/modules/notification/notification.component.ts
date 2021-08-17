import { Component, OnInit, ViewChild } from '@angular/core'
import { FormBuilder, Validators, FormGroup } from '@angular/forms'
import { Router } from '@angular/router'
import { ToastrService } from 'ngx-toastr'

import { NotificationService } from 'src/app/services/notification.service'
import { NotificationObject } from 'src/app/models/NotificationObjet'
import { RESPONSE_STATUS, MESSAGE_COMMON } from 'src/app/constants/CONSTANTS'

declare var $: any

@Component({
	selector: 'app-notification',
	templateUrl: './notification.component.html',
	styleUrls: ['./notification.component.css'],
})
export class NotificationComponent implements OnInit {
	constructor(private _toastr: ToastrService, private router: Router, private notificationService: NotificationService) {}

	notifications: any[]
	@ViewChild('table', { static: false }) table: any
	ngOnInit() {
		this.getList()
	}
	model: any = new NotificationObject()
	pageSize: number = 20
	pageIndex: number = 1
	totalRecords: any = 0
	idDelete: number

	listType: any = [
		{ value: 1, text: 'Bài viết' },
		{ value: 2, text: 'Phản ánh kiến nghị' },
		{ value: 3, text: 'Nhắc việc' },
	]

	getList() {
		let request = {
			title: this.model.title != null ? this.model.title : '',
			content: this.model.content != null ? this.model.content : '',
			type: this.model.type != null ? this.model.type : '',
			sendDate: this.model.sendDate != null ? this.model.sendDate : '',
			PageIndex: this.pageIndex,
			PageSize: this.pageSize,
		}
		this.notificationService.getListNotificationOnPageByReceiveId(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result != null) {
					this.notifications = response.result.syNotifications
					this.totalRecords = response.result.TotalCount
					this.notificationService.updateIsViewedNotification({}).subscribe()
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

	sendDateChange(newDate) {
		this.model.sendDate = newDate
		this.pageIndex = 1
		this.table.first = 0
		this.getList()
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

	preDelete(id: any) {
		this.idDelete = id
		$('#modalConfirmDelete').modal('show')
	}
	onDelete(id: any) {
		this.notificationService.deleteNotification({ id: id }).subscribe((res) => {
			$('#modalConfirmDelete').modal('hide')
			if (res.success == RESPONSE_STATUS.success) {
				this._toastr.success(MESSAGE_COMMON.DELETE_SUCCESS)
				this.getList()
			} else {
				this._toastr.error(MESSAGE_COMMON.DELETE_FAILED)
			}
		})
	}
}
