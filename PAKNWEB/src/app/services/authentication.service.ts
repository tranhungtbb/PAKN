import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { ServiceInvokerService } from '../commons/service-invoker.service'
import { Observable } from 'rxjs'
import { AppSettings } from '../constants/app-setting'
import { Api } from '../constants/api'
import { LOG_ACTION, LOG_OBJECT } from '../constants/CONSTANTS'

@Injectable({
	providedIn: 'root',
})
export class AuthenticationService {
	constructor(private http: HttpClient, private serviceInvoker: ServiceInvokerService) { }

	login(user: any): Observable<any> {
		user.logAction = LOG_ACTION.login
		user.logObject = LOG_OBJECT.login
		return this.serviceInvoker.postlogin(user, AppSettings.API_ADDRESS + Api.LOGIN)
	}

	logOut(data: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.logOut),
			logObject: encodeURIComponent(LOG_OBJECT.NO_CONTENT),
		}
		return this.serviceInvoker.postwithHeaders(data, AppSettings.API_ADDRESS + Api.logOut, headers)
	}

	register(user: any): Observable<any> {
		return this.serviceInvoker.post(user, AppSettings.API_ADDRESS + Api.REGISTER)
	}

	forgetpassword(user: any): Observable<any> {
		return this.serviceInvoker.post(user, AppSettings.API_ADDRESS + Api.FORGETPASSWORD)
	}

	getTokenByEmail(user: any): Observable<any> {
		return this.serviceInvoker.post(user, AppSettings.API_ADDRESS + Api.GetTokenByEmail)
	}

	chagepassword(user: any): Observable<any> {
		return this.serviceInvoker.post(user, AppSettings.API_ADDRESS + Api.CHANGEPASSWORD)
	}

	restoreAccount(user: any): Observable<any> {
		return this.serviceInvoker.post(user, AppSettings.API_ADDRESS + Api.RESTORE_ACCOUNT)
	}
}
