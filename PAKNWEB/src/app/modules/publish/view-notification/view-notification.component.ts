import { Component, OnInit, ViewChild } from '@angular/core'
import { Router, ActivatedRoute } from '@angular/router'
import { RESPONSE_STATUS , RECOMMENDATION_STATUS} from 'src/app/constants/CONSTANTS'
import {NotificationService} from 'src/app/services/notification.service'

@Component({
	selector: 'app-view-notification',
	templateUrl: './view-notification.component.html',
	styleUrls: ['./view-notification.component.css'],
})
export class ViewNotificationComponent implements OnInit {
	lstNotification: any[] = []
	notification: any
	id : any = 0

	constructor(private notificationService: NotificationService, private _router: Router, private activatedRoute: ActivatedRoute) {
		this.lstNotification = []
	}

	ngOnInit() {
		this.activatedRoute.params.subscribe((params) => {
			let suggest = params['id']
			if (suggest) {
				this.id = suggest
				this.notificationService.getListNotificationGetById({Id : suggest}).subscribe(res=>{
					if(res.success == RESPONSE_STATUS.success){
						this.notification = res.result.SYNotificationGetByID
						this.updateIsReadNotification(this.notification.dataId)
					}
				}), (error)=>{
					console.log(error)
				}
			}
		})
		// GET detail
		this.getListPaged()
	}

	getListPaged() {
		this.notificationService.getListNotificationOnPageByReceiveId({ PageSize: 7, PageIndex: 1 }).subscribe((res) => {
			if (res.success != RESPONSE_STATUS.success) {
				return
			}
			if (res.result.syNotifications.length > 0) {
				this.lstNotification = res.result.syNotifications
			}
			return
		})
	}
	redirectDetail(id: any) {
		this._router.navigate(['/cong-bo/thong-bao/' + id])
	}
	getTime(date : any){
		let result = ''
		let sendDate = new Date(date)
		let currentDate = new Date()
		if(sendDate.getFullYear() != currentDate.getFullYear()){
			result = String(sendDate.getMinutes()) +':'+ sendDate.getHours() + ' ' + sendDate.getDate() + '/' + sendDate.getMonth() + '/' + sendDate.getFullYear()
		}else{
			if(sendDate.getMonth() != currentDate.getMonth()){
				result = String(sendDate.getMinutes()) + ':'+ sendDate.getHours() + ' ' + sendDate.getDate() + '/' + sendDate.getMonth() + '/' + sendDate.getFullYear()
			}
			else{
				if(sendDate.getDate() != currentDate.getDate()){
					result = String(sendDate.getMinutes()) + ':'+ sendDate.getHours() + ' ' + sendDate.getDate() + '/' + sendDate.getMonth() + '/' + sendDate.getFullYear()
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
	checkDeny(status: any) {
		if (status == RECOMMENDATION_STATUS.PROCESS_DENY || status == RECOMMENDATION_STATUS.RECEIVE_DENY || status == RECOMMENDATION_STATUS.APPROVE_DENY) {
			return true
		}
		return false
	}
	updateIsReadNotification(dataId: any) {
		this.notificationService.updateIsReadedNotification({ ObjectId: dataId }).subscribe()
	}
}
