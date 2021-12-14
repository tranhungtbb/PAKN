import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { ServiceInvokerService } from '../commons/service-invoker.service'
import { Observable } from 'rxjs'
import { AppSettings } from '../constants/app-setting'
import { Api } from '../constants/api'
// import { retry } from 'rxjs/operators';
import { LOG_ACTION, LOG_OBJECT } from '../constants/CONSTANTS'
import { UserInfoStorageService } from '../commons/user-info-storage.service'

@Injectable({
	providedIn: 'root',
})
export class RoleService {
	constructor(private http: HttpClient, private serviceInvoker: ServiceInvokerService, private localStronageService: UserInfoStorageService) {}

	getAll(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.RoleGetAll)
	}
	getAllPagedList(query: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.GETLIST),
			logObject: encodeURIComponent(LOG_OBJECT.SY_ROLE),
		}
		return this.serviceInvoker.getwithHeaders(query, AppSettings.API_ADDRESS + Api.RoleGetAllOnPage, headers)
	}

	getRoleById(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.RoleGetById)
	}
	insert(data: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.INSERT),
			logObject: encodeURIComponent(LOG_OBJECT.SY_ROLE + ' ' + data.name),
		}
		return this.serviceInvoker.postwithHeaders(data, AppSettings.API_ADDRESS + Api.RoleInsert, headers)
	}
	update(data: any, isUpdateStatus: any = false): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(isUpdateStatus == false ? LOG_ACTION.UPDATE : LOG_ACTION.UPDATESTATUS),
			logObject: encodeURIComponent(LOG_OBJECT.SY_ROLE + ' ' + data.name),
		}
		return this.serviceInvoker.postwithHeaders(data, AppSettings.API_ADDRESS + Api.RoleUpdate, headers)
	}
	delete(data: any, title: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.DELETE),
			logObject: encodeURIComponent(LOG_OBJECT.SY_ROLE + ' ' + title),
		}
		return this.serviceInvoker.postwithHeaders(data, AppSettings.API_ADDRESS + Api.RoleDelete, headers)
	}

	getDataForCreate(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.RoleGetDataForCreate)
	}
	insertPermission(data: any): Observable<any> {
		return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.RoleInsertPermission)
	}
}
