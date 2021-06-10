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
	constructor(private serviceInvoker: ServiceInvokerService, private storeageService: UserInfoStorageService, private http: HttpClient) {}

	insertNotificationTypeNews(query: any): Observable<any> {
		return this.serviceInvoker.post(query, AppSettings.API_ADDRESS + Api.NotificationInsertTypeNews)
	}

	insertNotificationTypeRecommendation(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.NotificationInsertTypeRecommendation)
	}

	getListNotificationOnPageByReceiveId(query: any): Observable<any> {
		let tempheaders = new HttpHeaders({
			ipAddress: this.storeageService.getIpAddress() && this.storeageService.getIpAddress() != 'null' ? this.storeageService.getIpAddress() : '',
			macAddress: '',
			logAction: encodeURIComponent(LOG_ACTION.INSERT),
			logObject: encodeURIComponent(LOG_OBJECT.CA_FIELD),
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

	updateIsViewedNotification(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.NotificationUpdateIsViewed)
	}

	updateIsReadedNotification(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.NotificationUpdateIsReaded)
	}

	deleteNotification(query: any): Observable<any> {
		return this.serviceInvoker.post(query, AppSettings.API_ADDRESS + Api.NotificationDelete)
	}
}
