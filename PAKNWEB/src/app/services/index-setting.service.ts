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
export class IndexSettingService {
	constructor(private http: HttpClient, private serviceInvoker: ServiceInvokerService, private storeageService: UserInfoStorageService) { }

	Update(request: any): Observable<any> {
		let tempheaders = new HttpHeaders({
			ipAddress: this.storeageService.getIpAddress() && this.storeageService.getIpAddress() != 'null' ? this.storeageService.getIpAddress() : '',
			macAddress: '',
			logAction: encodeURIComponent(LOG_ACTION.UPDATE),
			logObject: encodeURIComponent(LOG_OBJECT.SY_INDEXSETTING),
		})

		const form = new FormData()
		form.append('model', JSON.stringify(request.model))

		if (request.fileBanner) {
			form.append('bannerMain', request.fileBanner)
		}
		if (request.logo) {
			form.append('logo', request.logo)
		}

		tempheaders.append('Content-Type', 'application/json')
		const httpPackage = {
			headers: tempheaders,
			reportProgress: true,
		}

		return this.http.post(AppSettings.API_ADDRESS + Api.SYIndexSettingUpdate, form, httpPackage)
	}

	GetInfo(request: any): Observable<any> {
		return this.serviceInvoker.get({}, AppSettings.API_ADDRESS + Api.SYIndexSettingGetInfo)
	}

	IndexWebsiteInsert(request: any): Observable<any> {
		let tempheaders = new HttpHeaders({
			ipAddress: this.storeageService.getIpAddress() && this.storeageService.getIpAddress() != 'null' ? this.storeageService.getIpAddress() : '',
			macAddress: '',
			logAction: encodeURIComponent(LOG_ACTION.UPDATE),
			logObject: encodeURIComponent(LOG_OBJECT.SY_INDEXSETTING),
		})

		const form = new FormData()
		form.append('model', JSON.stringify(request.model))
		if (request.file) {
			form.append('file', request.file)
		}
		tempheaders.append('Content-Type', 'application/json')
		const httpPackage = {
			headers: tempheaders,
			reportProgress: true,
		}

		return this.http.post(AppSettings.API_ADDRESS + Api.SYIndexWebsiteInsert, form, httpPackage)
	}

	IndexWebsiteUpdate(request: any): Observable<any> {
		let tempheaders = new HttpHeaders({
			ipAddress: this.storeageService.getIpAddress() && this.storeageService.getIpAddress() != 'null' ? this.storeageService.getIpAddress() : '',
			macAddress: '',
			logAction: encodeURIComponent(LOG_ACTION.UPDATE),
			logObject: encodeURIComponent(LOG_OBJECT.SY_INDEXSETTING),
		})

		const form = new FormData()
		form.append('model', JSON.stringify(request.model))
		if (request.file) {
			form.append('file', request.file)
		}
		tempheaders.append('Content-Type', 'application/json')
		const httpPackage = {
			headers: tempheaders,
			reportProgress: true,
		}

		return this.http.post(AppSettings.API_ADDRESS + Api.SYIndexWebsiteUpdate, form, httpPackage)
	}

	IndexWebsiteDelete(request: any): Observable<any> {
		return this.serviceInvoker.get(request, AppSettings.API_ADDRESS + Api.SYIndexWebsiteDelete)
	}

	SYIndexWebsiteGetAll(): Observable<any> {
		return this.serviceInvoker.get({}, AppSettings.API_ADDRESS + Api.SYIndexWebsiteGetAll)
	}


}
