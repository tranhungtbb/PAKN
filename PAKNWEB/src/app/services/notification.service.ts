import { Injectable } from '@angular/core'
import { ServiceInvokerService } from '../commons/service-invoker.service'
import { Observable } from 'rxjs'
import { AppSettings } from '../constants/app-setting'
import { Api } from '../constants/api'
import { HttpClient, HttpHeaders } from '@angular/common/http'
import { UserInfoStorageService } from '../commons/user-info-storage.service'
import { LOG_ACTION, LOG_OBJECT } from '../constants/CONSTANTS'

@Injectable({
	providedIn: 'root',
})
export class NotificationService {
	constructor(private serviceInvoker: ServiceInvokerService, private storeageService: UserInfoStorageService, private http: HttpClient) { }


	getListNotificationOnPageByReceiveId(query: any): Observable<any> {
		let tempheaders = new HttpHeaders({
			ipAddress: this.storeageService.getIpAddress() && this.storeageService.getIpAddress() != 'null' ? this.storeageService.getIpAddress() : '',
			macAddress: '',
		})
		const form = new FormData()
		form.append('model', JSON.stringify(query))
		tempheaders.append('Content-Type', 'application/json')
		const httpPackage = {
			headers: tempheaders,
			reportProgress: true,
		}
		return this.http.post(AppSettings.API_ADDRESS + Api.NotificationGetList, form, httpPackage)
	}
	getListNotificationGetById(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.NotificationGetById)
	}

	updateIsViewedNotification(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.NotificationUpdateIsViewed)
	}

	updateIsReadedNotification(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.NotificationUpdateIsReaded)
	}

	deleteNotification(query: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.DELETE),
			logObject: encodeURIComponent(LOG_OBJECT.SY_NOTIFICATION),
		}
		return this.serviceInvoker.postwithHeaders(query, AppSettings.API_ADDRESS + Api.NotificationDelete, headers)
	}
}
