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
export class SupportService {
	constructor(private http: HttpClient, private serviceInvoker: ServiceInvokerService, private storeageService: UserInfoStorageService) { }

	GetList(request: any): Observable<any> {
		return this.serviceInvoker.get(request, AppSettings.API_ADDRESS + Api.SYSupportGetAllByCategory)
	}
	GetListByType(request: any): Observable<any> {
		return this.serviceInvoker.get(request, AppSettings.API_ADDRESS + Api.SYSupportGetAllByType)
	}



	Insert(request: any, isVideo: boolean = false): Observable<any> {
		let tempheaders = new HttpHeaders({
			ipAddress: this.storeageService.getIpAddress() && this.storeageService.getIpAddress() != 'null' ? this.storeageService.getIpAddress() : '',
			macAddress: '',
			logAction: encodeURIComponent(LOG_ACTION.INSERT),
			logObject: encodeURIComponent(isVideo == false ? LOG_OBJECT.SY_SUPPORT + ' ' + request.model.title : LOG_OBJECT.SY_SUPPORT_VIDEO + ' ' + request.model.title),
		})
		const form = new FormData()
		form.append('model', JSON.stringify(request.model))

		if (request.files) {
			request.files.forEach((item) => {
				form.append('file', item)
			})
		}
		tempheaders.append('Content-Type', 'application/json')
		const httpPackage = {
			headers: tempheaders,
			reportProgress: true,
		}
		return this.http.post(AppSettings.API_ADDRESS + Api.SYSupportInsert, form, httpPackage)
	}

	Update(request: any, isVideo: boolean = false): Observable<any> {
		let tempheaders = new HttpHeaders({
			ipAddress: this.storeageService.getIpAddress() && this.storeageService.getIpAddress() != 'null' ? this.storeageService.getIpAddress() : '',
			macAddress: '',
			logAction: encodeURIComponent(LOG_ACTION.UPDATE),
			logObject: encodeURIComponent(isVideo == false ? LOG_OBJECT.SY_SUPPORT + ' ' + request.model.title : LOG_OBJECT.SY_SUPPORT_VIDEO + ' ' + request.model.title),
		})
		const form = new FormData()
		form.append('model', JSON.stringify(request.model))

		if (request.files) {
			request.files.forEach((item) => {
				form.append('file', item)
			})
		}
		tempheaders.append('Content-Type', 'application/json')
		const httpPackage = {
			headers: tempheaders,
			reportProgress: true,
		}
		return this.http.post(AppSettings.API_ADDRESS + Api.SYSupportUpdate, form, httpPackage)
	}

	Delete(request: any, title: any, isVideo: boolean = false): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.DELETE),
			logObject: encodeURIComponent(isVideo == false ? LOG_OBJECT.SY_SUPPORT + ' ' + title : LOG_OBJECT.SY_SUPPORT_VIDEO + ' ' + title),
		}
		return this.serviceInvoker.postwithHeaders(request, AppSettings.API_ADDRESS + Api.SYSupportDelete, headers)
	}
}
