import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { ServiceInvokerService } from '../commons/service-invoker.service'
import { Observable } from 'rxjs'
import { AppSettings } from '../constants/app-setting'
import { Api } from '../constants/api'
import { UserInfoStorageService } from '../commons/user-info-storage.service'

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
}
