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
export class UserService {
	constructor(private http: HttpClient, private serviceInvoker: ServiceInvokerService, private localStronageService: UserInfoStorageService) {}

	getAllPagedList(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.UserGetPagedList)
	}
	getById(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.UserGetById)
	}
	insert(data: any, files:any = null): Observable<any> {

		let form = new FormData();

		for(let item in data){
			form.append(item,data[item])
		}
		if(files != null && files.length > 0){
			for(let item of files){
				form.append('files',item,item.name);
			}
		}

		return this.serviceInvoker.post(form, AppSettings.API_ADDRESS + Api.UserInsert)
	}
	update(data: any, files:any = null): Observable<any> {
		let form = new FormData();

		for(let key in data){
			form.append(key, data[key])
		}
		if(files != null && files.length > 0){
			for(let item of files){
				form.append('files', item, item.name);
			}
		}
		return this.serviceInvoker.post(form, AppSettings.API_ADDRESS + Api.UserUpdate)
	}
	delete(data: any): Observable<any> {
		return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.UserDelete)
	}

	getAvatar(id:number): Observable<any> {
		let url = AppSettings.API_ADDRESS + Api.UserGetAvatar + "/" +id;
		return this.serviceInvoker.get({}, url)
	}
}
