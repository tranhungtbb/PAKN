import { Injectable } from '@angular/core'
import { ServiceInvokerService } from '../commons/service-invoker.service'
import { Observable } from 'rxjs'
import { AppSettings } from '../constants/app-setting'
import { Api } from '../constants/api'

@Injectable({
	providedIn: 'root',
})
export class HashtagService {
	constructor(private serviceInvoker: ServiceInvokerService) {}

	getAllPagedList(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.HashtagGetList)
	}
	getAll(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.HashtagGetAll)
	}
	getById(query: any): Observable<any> {
		let headers = new Headers()
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.HashtagGetById)
	}
	create(data: any): Observable<any> {
		let headers = new Headers()
		return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.HashtagInsert)
	}

	update(data: any): Observable<any> {
		let headers = new Headers()
		return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.HashtagUpdate)
	}
	delete(data: any): Observable<any> {
		let headers = new Headers()
		return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.HashtagDelete)
	}
}
