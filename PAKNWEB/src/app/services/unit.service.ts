import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { ServiceInvokerService } from '../commons/service-invoker.service'
import { Observable } from 'rxjs'
import { AppSettings } from '../constants/app-setting'
import { Api } from '../constants/api'
// import { retry } from 'rxjs/operators';
// import { request } from 'http';
import { UserInfoStorageService } from '../commons/user-info-storage.service'

@Injectable({
	providedIn: 'root',
})
export class UnitService {
	constructor(private http: HttpClient, private serviceInvoker: ServiceInvokerService, private localStronageService: UserInfoStorageService) {}

	getAllPagedList(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.UnitGetPagedList)
	}
	getAll(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.UnitGetAll)
	}
	getChildrenDropdown(): Observable<any> {
		return this.serviceInvoker.get({}, AppSettings.API_ADDRESS + Api.UnitGetChildrenDropdown)
	}

	getById(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.UnitGetById)
	}
	create(data: any): Observable<any> {
		return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.UnitInsert)
	}
	// create(data: any): Observable<any> {
	// 	return this.http.post(AppSettings.API_ADDRESS + Api.UnitInsert, data)
	// }
	update(data: any): Observable<any> {
		return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.UnitUpdate)
	}
	delete(data: any): Observable<any> {
		return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.UnitDelete)
	}
	changeStatus(data: any): Observable<any> {
		return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.UnitChangeStatus)
	}
	checkExists(req: any): Observable<any> {
		let url = AppSettings.API_ADDRESS + Api.UnitCheckExists
		return this.serviceInvoker.get(req, url)
	}
}
