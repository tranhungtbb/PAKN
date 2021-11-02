import { Injectable } from '@angular/core'
import { ServiceInvokerService } from '../commons/service-invoker.service'
import { Observable } from 'rxjs'
import { AppSettings } from '../constants/app-setting'
import { Api } from '../constants/api'
import { LOG_ACTION, LOG_OBJECT } from '../constants/CONSTANTS'
import { iterator } from 'rxjs/internal-compatibility'
import { UserInfoStorageService } from '../commons/user-info-storage.service'
import { HttpClient, HttpHeaders } from '@angular/common/http'

@Injectable({
	providedIn: 'root',
})
export class SupportGalleryService {
	constructor(private http: HttpClient, private serviceInvoker: ServiceInvokerService, private storeageService: UserInfoStorageService) {}

	getAllSupportGallery(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.SYGalleryGetList)
	}
	insertSupportGallery(query: any): Observable<any> {
		let headers = new HttpHeaders({
			ipAddress: this.storeageService.getIpAddress() && this.storeageService.getIpAddress() != 'null' ? this.storeageService.getIpAddress() : '',
			macAddress: '',
			logAction: encodeURIComponent(LOG_ACTION.INSERT),
			logObject: encodeURIComponent(LOG_OBJECT.SY_GALLERY),
		})
		headers.append('Content-Type', 'application/json')
		const form = new FormData()
		if (query && query.files instanceof Array) {
			query.files.forEach((item) => {
				form.append('Files', item)
			})
		}
		console.log(form)
		let postHeaders = {
			headers: headers,
			reportProgress: true,
		}
		return this.http.post(AppSettings.API_ADDRESS + Api.SYGalleryInsert, form, postHeaders)
	}
	deleteSupportGallery(query: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.DELETE),
			logObject: encodeURIComponent(LOG_OBJECT.SY_GALLERY),
		}
		return this.serviceInvoker.postwithHeaders(query, AppSettings.API_ADDRESS + Api.SYGalleryDelete, headers)
	}
}
