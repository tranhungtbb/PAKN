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
export class InvitationService {
	constructor(private http: HttpClient, private serviceInvoker: ServiceInvokerService, private storeageService: UserInfoStorageService) {}

	invitationInsert(request: any): Observable<any> {
		let tempheaders = new HttpHeaders({
			ipAddress: this.storeageService.getIpAddress() && this.storeageService.getIpAddress() != 'null' ? this.storeageService.getIpAddress() : '',
			macAddress: '',
			logAction: encodeURIComponent(request.model.status == 1 ? LOG_ACTION.INSERT : LOG_ACTION.INSERT_AND_SEND),
			logObject: encodeURIComponent(LOG_OBJECT.SY_INVITATION + ' ' + request.model.title),
		})
		const form = new FormData()
		form.append('Model', JSON.stringify(request.model))
		form.append('InvitationUserMap', JSON.stringify(request.userMap))
		debugger
		if (request.Files) {
			request.Files.forEach((item) => {
				form.append('QD', item)
			})
		}
		tempheaders.append('Content-Type', 'application/json')
		const httpPackage = {
			headers: tempheaders,
			reportProgress: true,
		}
		return this.http.post(AppSettings.API_ADDRESS + Api.InnvitationInsert, form, httpPackage)
	}

	invitationGetById(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.InnvitationUpdate)
	}
	invitationDetail(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.InnvitationDetail)
	}

	invitationUpdate(request: any): Observable<any> {
		let tempheaders = new HttpHeaders({
			ipAddress: this.storeageService.getIpAddress() && this.storeageService.getIpAddress() != 'null' ? this.storeageService.getIpAddress() : '',
			macAddress: '',
			logAction: encodeURIComponent(request.model.status == 1 ? LOG_ACTION.UPDATE : LOG_ACTION.UPDATE_AND_SEND),
			logObject: encodeURIComponent(LOG_OBJECT.SY_INVITATION + ' ' + request.model.title),
		})
		const form = new FormData()
		form.append('Model', JSON.stringify(request.model))
		form.append('InvitationUserMap', JSON.stringify(request.userMap))
		form.append('LstFileDelete', JSON.stringify(request.lstFileDelete))

		if (request.Files) {
			request.Files.forEach((item) => {
				form.append('QD', item)
			})
		}
		tempheaders.append('Content-Type', 'application/json')
		const httpPackage = {
			headers: tempheaders,
			reportProgress: true,
		}
		return this.http.post(AppSettings.API_ADDRESS + Api.InnvitationUpdate, form, httpPackage)
	}

	invitationGetList(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.InvitationGetList)
	}

	userReadedInvitationGetList(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.UserReadedInvitationGetList)
	}

	GetListHisOnPage(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.InnvitationGetListHisOnPage)
	}

	delete(query: any, title: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.DELETE),
			logObject: encodeURIComponent(LOG_OBJECT.SY_INVITATION + ' ' + title),
		}
		return this.serviceInvoker.postwithHeaders(query, AppSettings.API_ADDRESS + Api.InvitationDelete, headers)
	}
}
