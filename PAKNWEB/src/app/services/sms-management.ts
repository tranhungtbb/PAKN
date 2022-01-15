import { Injectable } from '@angular/core'
import { HttpClient, HttpHeaders } from '@angular/common/http'
import { ServiceInvokerService } from '../commons/service-invoker.service'
import { Observable, of } from 'rxjs'
import { AppSettings } from '../constants/app-setting'
import { Api } from '../constants/api'
import { LOG_ACTION, LOG_OBJECT } from '../constants/CONSTANTS'
import { UserInfoStorageService } from '../commons/user-info-storage.service'

@Injectable({
	providedIn: 'root',
})
export class SMSManagementService {
	constructor(private http: HttpClient, private serviceInvoker: ServiceInvokerService, private storeageService: UserInfoStorageService) { }

	Insert(request: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(request.model.status == 1 ? LOG_ACTION.INSERT : LOG_ACTION.INSERT_AND_SEND),
			logObject: encodeURIComponent(LOG_OBJECT.SMS_EMAIL + ' ' + request.model.title),
		}
		return this.serviceInvoker.postwithHeaders(request, AppSettings.API_ADDRESS + Api.SMSManagementInsert, headers)
	}

	InsertHisSMS(request: any): Observable<any> {
		return this.serviceInvoker.post(request, AppSettings.API_ADDRESS + Api.HISSMSInsert)
	}

	GetById(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.SMSManagementUpdate)
	}

	UpdateStatusSend(request: any, title: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.SEND),
			logObject: encodeURIComponent(LOG_OBJECT.SMS_EMAIL + ' ' + title),
		}
		return this.serviceInvoker.getwithHeaders(request, AppSettings.API_ADDRESS + Api.SMSManagementUpdateStatusSend, headers)
	}

	Update(request: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(request.model.status == 1 ? LOG_ACTION.UPDATE : LOG_ACTION.UPDATE_AND_SEND),
			logObject: encodeURIComponent(LOG_OBJECT.SMS_EMAIL + ' ' + request.model.title),
		}
		return this.serviceInvoker.postwithHeaders(request, AppSettings.API_ADDRESS + Api.SMSManagementUpdate, headers)
	}

	Delete(request: any, title: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.DELETE),
			logObject: encodeURIComponent(LOG_OBJECT.SMS_EMAIL + ' ' + title),
		}
		return this.serviceInvoker.postwithHeaders(request, AppSettings.API_ADDRESS + Api.SMSManagementDelete, headers)
	}

	GetListOnPage(query: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.GETLIST),
			logObject: encodeURIComponent(LOG_OBJECT.SMS_EMAIL),
		}
		return this.serviceInvoker.getwithHeaders(query, AppSettings.API_ADDRESS + Api.SMSManagementGetOnPage, headers)
	}

	GetListHisOnPage(query: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.GETLIST),
			logObject: encodeURIComponent(LOG_OBJECT.HIS_EMAIL),
		}
		return this.serviceInvoker.getwithHeaders(query, AppSettings.API_ADDRESS + Api.SMSManagementGetHisOnPage, headers)
	}

	GetListAdmintrative(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.AdministrativeUnits)
	}

	GetListIndividualAndBusinessByAdmintrativeUnitId(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.GetListIndividualAndBusinessByAdmintrativeUnitId)
	}
	GetListIndividualBusinessDrop(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.GetListIndividualBusinessDrop)
	}

	// delete(query: any): Observable<any> {
	// 	return this.serviceInvoker.post(query, AppSettings.API_ADDRESS + Api.InvitationDelete)
	// }
}
