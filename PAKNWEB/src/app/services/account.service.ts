import { Injectable } from '@angular/core'
import { HttpClient, HttpHeaders } from '@angular/common/http'
import { ServiceInvokerService } from '../commons/service-invoker.service'
import { Observable } from 'rxjs'
import { AppSettings } from '../constants/app-setting'
import { Api } from '../constants/api'
import { UserInfoStorageService } from '../commons/user-info-storage.service'
import { LOG_ACTION, LOG_OBJECT } from '../constants/CONSTANTS'

@Injectable({
	providedIn: 'root',
})
export class AccountService {
	constructor(private http: HttpClient, private serviceInvoker: ServiceInvokerService, private localStronageService: UserInfoStorageService) {}

	tempheaders = new HttpHeaders({
		ipAddress: this.localStronageService.getIpAddress() && this.localStronageService.getIpAddress() != 'null' ? this.localStronageService.getIpAddress() : '',
		macAddress: '',
		logAction: encodeURIComponent(LOG_ACTION.INSERT),
		logObject: encodeURIComponent(LOG_OBJECT.CA_FIELD),
	})

	getUserInfo(): Observable<any> {
		let url = `${AppSettings.API_ADDRESS}${Api.AccountGetInfo}`
		return this.serviceInvoker.get({id : this.localStronageService.getUserId()}, url)
	}

	changePassword(body: any): Observable<any> {
		let url = `${AppSettings.API_ADDRESS}${Api.AccountChangePassword}`

		var form = new FormData()
		for (var b in body) {
			form.append(b, body[b])
		}
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.CHANGEPASSWORD),
			logObject: encodeURIComponent(LOG_OBJECT.NO_CONTENT),
		}
		return this.serviceInvoker.postwithHeaders(form, url,headers)
	}

	updateInfoIndividualCurrent(body: any) {
		
		return this.serviceInvoker.post(body, AppSettings.API_ADDRESS + Api.updateInfoIndividualCurrent)
	}

	updateInfoBusinessCurrent(body: any) {
		// var form = new FormData()
		// for (var b in body) {
		// 	form.append(b, body[b])
		// }

		return this.serviceInvoker.post(body, AppSettings.API_ADDRESS + Api.updateInfoBusinessCurrent)
	}


}
