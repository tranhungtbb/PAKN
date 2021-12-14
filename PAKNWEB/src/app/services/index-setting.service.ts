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
	constructor(private http: HttpClient, private serviceInvoker: ServiceInvokerService, private storeageService: UserInfoStorageService) {}

	Update(request: any): Observable<any> {
		let tempheaders = new HttpHeaders({
			ipAddress: this.storeageService.getIpAddress() && this.storeageService.getIpAddress() != 'null' ? this.storeageService.getIpAddress() : '',
			macAddress: '',
			logAction: encodeURIComponent(LOG_ACTION.UPDATE),
			logObject: encodeURIComponent(LOG_OBJECT.SY_INDEXSETTING),
		})
		debugger
		const form = new FormData()
		form.append('model', JSON.stringify(request.model))
		form.append('ltsIndexWebsite', JSON.stringify(request.ltsIndexWebsite))
		form.append('lstRemoveBanner', JSON.stringify(request.lstRemoveBanner))

		if (request.fileBanner) {
			form.append('bannerMain', request.fileBanner)
		}

		if (request.lstInsertBanner) {
			request.lstInsertBanner.forEach((item) => {
				form.append('lstInsertBanner', item)
			})
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

	// IndexSettingWebsiteInsert(query: any): Observable<any> {
	// 	let headers = {
	// 		logAction: encodeURIComponent(LOG_ACTION.UPDATE),
	// 		logObject: encodeURIComponent(LOG_OBJECT.SY_INDEXSETTING),
	// 	}
	// 	return this.serviceInvoker.postwithHeaders(query, AppSettings.API_ADDRESS + Api.SY, headers)
	// }
}
