import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { ServiceInvokerService } from '../commons/service-invoker.service'
import { Observable } from 'rxjs'
import { AppSettings } from '../constants/app-setting'
import { Api } from '../constants/api'
import { UserInfoStorageService } from '../commons/user-info-storage.service'
import { LOG_ACTION, LOG_OBJECT } from '../constants/CONSTANTS'

@Injectable({
	providedIn: 'root',
})
export class SystemconfigService {
	constructor(private http: HttpClient, private serviceInvoker: ServiceInvokerService, private storageService: UserInfoStorageService) {}
	getSystemEmail(): Observable<any> {
		return this.serviceInvoker.get({}, AppSettings.API_ADDRESS + Api.EmailGetFirstBase)
	}
	updateSystemEmail(query: any): Observable<any> {
		return this.serviceInvoker.post(query, AppSettings.API_ADDRESS + Api.EmailConfigSystemUpdate)
	}
	getSystemSMS(): Observable<any> {
		return this.serviceInvoker.get({}, AppSettings.API_ADDRESS + Api.SMSGetFirstBase)
	}
	updateSystemSMS(query: any): Observable<any> {
		return this.serviceInvoker.post(query, AppSettings.API_ADDRESS + Api.SMSConfigSystemUpdate)
	}
	getSystemTime(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.TimeConfigGetAllOnPage)
	}
	updateSystemTime(query: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.UPDATE),
			logObject: encodeURIComponent(LOG_OBJECT.SY_TIME),
		}
		return this.serviceInvoker.postwithHeaders(query, AppSettings.API_ADDRESS + Api.TimeConfigUpdate, headers)
	}
	insertSystemTime(query: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.INSERT),
			logObject: encodeURIComponent(LOG_OBJECT.SY_TIME),
		}
		return this.serviceInvoker.postwithHeaders(query, AppSettings.API_ADDRESS + Api.TimeConfigInsert, headers)
	}
	deleteSystemTime(query: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.DELETE),
			logObject: encodeURIComponent(LOG_OBJECT.SY_TIME),
		}
		return this.serviceInvoker.postwithHeaders(query, AppSettings.API_ADDRESS + Api.TimeConfigDelete, headers)
	}
	getSystemTimeById(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.TimeConfigGetById)
	}
	getSystemTimeDateActive(): Observable<any> {
		return this.serviceInvoker.get({}, AppSettings.API_ADDRESS + Api.TimeConfigGetDateActive)
	}
}
