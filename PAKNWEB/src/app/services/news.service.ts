import { Injectable } from '@angular/core'
import { HttpClient, HttpHeaders } from '@angular/common/http'
import { ServiceInvokerService } from '../commons/service-invoker.service'
import { Observable } from 'rxjs'
import { AppSettings } from '../constants/app-setting'
import { Api } from '../constants/api'
// import { retry } from 'rxjs/operators';
// import { request } from 'http';
import { UserInfoStorageService } from '../commons/user-info-storage.service'
import { LOG_ACTION, LOG_OBJECT } from '../constants/CONSTANTS'

@Injectable({
	providedIn: 'root',
})
export class NewsService {
	constructor(private http: HttpClient, private serviceInvoker: ServiceInvokerService, private localStronageService: UserInfoStorageService) {}

	tempheaders = new HttpHeaders({
		ipAddress: this.localStronageService.getIpAddress() && this.localStronageService.getIpAddress() != 'null' ? this.localStronageService.getIpAddress() : '',
		macAddress: '',
		logAction: encodeURIComponent(LOG_ACTION.INSERT),
		logObject: encodeURIComponent(LOG_OBJECT.CA_FIELD),
	})

	getAllPagedList(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.NewsGetAllOnPage)
	}
	getById(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.NewsGetById)
	}
	create(data: any): Observable<any> {
		const httpPackage = {
			headers: this.tempheaders,
			reportProgress: true,
		}
		return this.http.post(AppSettings.API_ADDRESS + Api.NewsInsert, data, httpPackage)
	}
	update(data: any): Observable<any> {
		const httpPackage = {
			headers: this.tempheaders,
			reportProgress: true,
		}
		return this.http.post(AppSettings.API_ADDRESS + Api.NewsUpdate, data, httpPackage)
	}
	delete(data: any): Observable<any> {
		return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.NewsDelete)
	}
	getAllNewsRelates(data: any): Observable<any> {
		return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.NewsRelatesGetAll)
	}
	uploadFile(data: any): Observable<any> {
		return this.serviceInvoker.postfile(data, AppSettings.API_ADDRESS + Api.NewsUploadFile)
	}
}
