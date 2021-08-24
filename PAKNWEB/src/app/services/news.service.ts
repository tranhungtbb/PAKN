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

	getAllPagedList(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.NewsGetAllOnPage)
	}

	getListHomePage(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.NewsGetListHomePage)
	}

	getById(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.NewsGetById)
	}
	create(data: any, file: any = null): Observable<any> {
		let tempheaders = new HttpHeaders({
			ipAddress: this.localStronageService.getIpAddress() && this.localStronageService.getIpAddress() != 'null' ? this.localStronageService.getIpAddress() : '',
			macAddress: '',
			logAction: encodeURIComponent(LOG_ACTION.INSERT),
			logObject: encodeURIComponent(LOG_OBJECT.NE_NEWS),
		})
		const httpPackage = {
			headers: tempheaders,
			reportProgress: true,
		}
		let formData = new FormData()
		formData.append('data', JSON.stringify(data))
		if (file) formData.append('files', file, file.name)
		return this.http.post(AppSettings.API_ADDRESS + Api.NewsInsert, formData, httpPackage)
	}
	update(data: any, file: any = null): Observable<any> {
		let tempheaders = new HttpHeaders({
			ipAddress: this.localStronageService.getIpAddress() && this.localStronageService.getIpAddress() != 'null' ? this.localStronageService.getIpAddress() : '',
			macAddress: '',
			logAction: encodeURIComponent(LOG_ACTION.UPDATE),
			logObject: encodeURIComponent(LOG_OBJECT.NE_NEWS),
		})
		const httpPackage = {
			headers: tempheaders,
			reportProgress: true,
		}

		let formData = new FormData()
		formData.append('data', JSON.stringify(data))
		if (file) formData.append('files', file, file.name)

		return this.http.post(AppSettings.API_ADDRESS + Api.NewsUpdate, formData, httpPackage)
	}

	changeStatus(data: any, file: any = null): Observable<any> {
		let tempheaders = new HttpHeaders({
			ipAddress: this.localStronageService.getIpAddress() && this.localStronageService.getIpAddress() != 'null' ? this.localStronageService.getIpAddress() : '',
			macAddress: '',
			logAction: data.status == 1 ? encodeURIComponent(LOG_ACTION.PUBLIC) : encodeURIComponent(LOG_ACTION.WITHDRAW),
			logObject: encodeURIComponent(LOG_OBJECT.NE_NEWS),
		})
		const httpPackage = {
			headers: tempheaders,
			reportProgress: true,
		}

		let formData = new FormData()
		formData.append('data', JSON.stringify(data))
		if (file) formData.append('files', file, file.name)

		return this.http.post(AppSettings.API_ADDRESS + Api.NewsUpdate, formData, httpPackage)
	}

	delete(data: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.DELETE),
			logObject: encodeURIComponent(LOG_OBJECT.NE_NEWS),
		}
		return this.serviceInvoker.postwithHeaders(data, AppSettings.API_ADDRESS + Api.NewsDelete, headers)
	}
	getAllNewsRelates(data: any): Observable<any> {
		return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.NewsRelatesGetAll)
	}

	hisNewsCreate(data: any): Observable<any> {
		return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.HisNewsInsert)
	}

	getListHisNewsByNewsId(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.HisNewsGetListByNewsId)
	}

	getViewDetail(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.NewsGetViewDetail)
	}

	getViewDetailPublic(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.NewsGetViewDetailPublic)
	}

	

	getAllRelates(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.NewsGetAllRelates)
	}
}
