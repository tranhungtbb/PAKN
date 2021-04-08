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
export class RegisterService {
	constructor(private http: HttpClient, private serviceInvoker: ServiceInvokerService, private localStronageService: UserInfoStorageService) {}

	getById(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.UserGetById)
	}
	registerIndividual(data: any): Observable<any> {
		let form = new FormData()

		for (let item in data) {
			form.append(item, data[item])
		}
		return this.serviceInvoker.post(form, AppSettings.API_ADDRESS + Api.RegisterIndividual)
	}
	registerOrganization(data: any): Observable<any> {
		let form = new FormData()

		for (let item in data) {
			form.append(item, data[item])
		}

		return this.serviceInvoker.post(form, AppSettings.API_ADDRESS + Api.RegisterOrganization)
	}
}
