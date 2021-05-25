import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
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
export class UserService {
	constructor(private http: HttpClient, private serviceInvoker: ServiceInvokerService, private localStronageService: UserInfoStorageService) {}

	getAllPagedList(query: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.GETLIST),
			logObject: encodeURIComponent(LOG_OBJECT.SY_USER),
		}
		return this.serviceInvoker.getwithHeaders(query, AppSettings.API_ADDRESS + Api.UserGetPagedList, headers)
	}

	getAllOnPagedList(query: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.GETLIST),
			logObject: encodeURIComponent(LOG_OBJECT.SY_USER),
		}
		return this.serviceInvoker.getwithHeaders(query, AppSettings.API_ADDRESS + Api.UserGetAllOnPagedList, headers)
	}

	getDataForCreate(query: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.GETLIST),
			logObject: encodeURIComponent(LOG_OBJECT.SY_USER_GET_BY_ROLE),
		}
		return this.serviceInvoker.getwithHeaders(query, AppSettings.API_ADDRESS + Api.UsersGetDataForCreate, headers)
	}
	getById(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.UserGetById)
	}

	getByIdUpdate(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.UserGetByIdUpdate)
	}
	getByRoleIdOnPage(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.UserGetByRoleIdOnPage)
	}

	getByRoleId(query: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.GETLIST),
			logObject: encodeURIComponent(LOG_OBJECT.SY_USER_GET_BY_ROLE),
		}
		return this.serviceInvoker.getwithHeaders(query, AppSettings.API_ADDRESS + Api.UserGetByRoleId, headers)
	}

	getIsSystem(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.UserGetIsSystem)
	}

	getIsSystem2(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.UserGetIsSystem2)
	}

	getIsSystemOrderByUnit(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.getIsSystemOrderByUnit)
	}

	insertMultiUserRole(query: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.INSERT),
			logObject: encodeURIComponent(LOG_OBJECT.SY_USER_GET_BY_ROLE),
		}
		return this.serviceInvoker.postwithHeaders(query, AppSettings.API_ADDRESS + Api.InsertMultiUserRole, headers)
	}

	insert(data: any, files: any = null): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.INSERT),
			logObject: encodeURIComponent(LOG_OBJECT.SY_USER),
		}
		let form = new FormData()

		for (let item in data) {
			form.append(item, data[item])
		}
		if (files != null && files.length > 0) {
			for (let item of files) {
				form.append('files', item, item.name)
			}
		}

		return this.serviceInvoker.postwithHeaders(form, AppSettings.API_ADDRESS + Api.UserInsert, headers)
	}
	update(data: any, files: any = null): Observable<any> {
		let form = new FormData()

		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.UPDATE),
			logObject: encodeURIComponent(LOG_OBJECT.SY_USER),
		}

		for (let key in data) {
			form.append(key, data[key])
		}
		if (files != null && files.length > 0) {
			for (let item of files) {
				form.append('files', item, item.name)
			}
		}
		return this.serviceInvoker.postwithHeaders(form, AppSettings.API_ADDRESS + Api.UserUpdate, headers)
	}
	delete(data: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.DELETE),
			logObject: encodeURIComponent(LOG_OBJECT.SY_USER),
		}
		return this.serviceInvoker.postwithHeaders(data, AppSettings.API_ADDRESS + Api.UserDelete, headers)
	}
	changeStatus(data: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.UPDATESTATUS),
			logObject: encodeURIComponent(LOG_OBJECT.SY_USER),
		}
		return this.serviceInvoker.postwithHeaders(data, AppSettings.API_ADDRESS + Api.UserChangeStatus, headers)
	}

	changePasswordInManage(data: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.CHANGEPASSWORD),
			logObject: encodeURIComponent(LOG_OBJECT.SY_USER),
		}
		return this.serviceInvoker.postwithHeaders(data, AppSettings.API_ADDRESS + Api.UserChangePwdInManage, headers)
	}

	deleteUserRole(data: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.DELETE),
			logObject: encodeURIComponent(LOG_OBJECT.SY_USER_GET_BY_ROLE),
		}
		return this.serviceInvoker.postwithHeaders(data, AppSettings.API_ADDRESS + Api.DeleteUserRole, headers)
	}

	getAvatar(id: number): Observable<any> {
		let url = AppSettings.API_ADDRESS + Api.UserGetAvatar + '/' + id
		return this.serviceInvoker.get({}, url)
	}
	getSystemLogin(request: any): Observable<any> {
		let url = AppSettings.API_ADDRESS + Api.SystemLogin
		return this.serviceInvoker.get(request, url)
	}
	getSystemLoginAdmin(request: any): Observable<any> {
		let url = AppSettings.API_ADDRESS + Api.SystemLoginAdmin
		return this.serviceInvoker.get(request, url)
	}
	sysLogDelete(request: any): Observable<any> {
		let url = AppSettings.API_ADDRESS + Api.SystemLogDelete
		return this.serviceInvoker.post(request, url)
	}

	getCurrentUser(): Observable<any> {
		let url = AppSettings.API_ADDRESS + Api.SystemLogDelete
		return this.serviceInvoker.get({}, url)
	}
	getUserDropdown(): Observable<any> {
		let url = AppSettings.API_ADDRESS + Api.SystemGetUserDropDown
		return this.serviceInvoker.get({}, url)
	}

	checkExists(req: any): Observable<any> {
		let url = AppSettings.API_ADDRESS + Api.UserCheckExists
		return this.serviceInvoker.get(req, url)
	}
}
