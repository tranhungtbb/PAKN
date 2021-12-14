import { Injectable } from '@angular/core'
import { HttpClient, HttpHeaders } from '@angular/common/http'
import { ServiceInvokerService } from '../commons/service-invoker.service'
import { Observable, of } from 'rxjs'
import { AppSettings } from '../constants/app-setting'
import { Api } from '../constants/api'
// import { retry } from 'rxjs/operators';
// import { request } from 'http';
import { UserInfoStorageService } from '../commons/user-info-storage.service'
import { LOG_ACTION, LOG_OBJECT } from '../constants/CONSTANTS'
import { catchError, tap } from 'rxjs/operators'

@Injectable({
	providedIn: 'root',
})
export class UserService {
	private handleError<T>(operation = 'operation', result?: T) {
		return (error: any): Observable<T> => {

			console.error(error); // log to console instead
			return of(result as T);
		};
	}
	constructor(private http: HttpClient, private serviceInvoker: ServiceInvokerService, private localStronageService: UserInfoStorageService) { }

	getAllPagedList(query: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.GETLIST),
			logObject: encodeURIComponent(LOG_OBJECT.SY_USER),
		}
		return this.serviceInvoker.getwithHeaders(query, AppSettings.API_ADDRESS + Api.UserGetPagedList, headers)
	}

	getAllPagedListByUnitId(query: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.GETLIST),
			logObject: encodeURIComponent(LOG_OBJECT.SY_USER),
		}
		return this.serviceInvoker.getwithHeaders(query, AppSettings.API_ADDRESS + Api.UserGetPagedList, headers)
	}

	getAllPagedListForChat(query: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.GETLIST),
			logObject: encodeURIComponent(LOG_OBJECT.SY_USER),
		}
		return this.serviceInvoker.getwithHeaders(query, AppSettings.API_ADDRESS + Api.UserGetPagedListForChat, headers)
	}
	getAllByListIdQb(query: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.GETLIST),
			logObject: encodeURIComponent(LOG_OBJECT.SY_USER),
		}
		return this.serviceInvoker.getwithHeaders(query, AppSettings.API_ADDRESS + Api.UserGetAllByIdQb, headers)
	}

	getAllOnPagedList(query: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.GETLIST),
			logObject: encodeURIComponent(LOG_OBJECT.SY_USER),
		}
		return this.serviceInvoker.getwithHeaders(query, AppSettings.API_ADDRESS + Api.UserGetAllOnPagedList, headers)
	}

	getUserSystemAllOnPagedList(query: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.GETLIST),
			logObject: encodeURIComponent(LOG_OBJECT.SY_USER),
		}
		return this.serviceInvoker.getwithHeaders(query, AppSettings.API_ADDRESS + Api.UserSystemGetAllOnPagedList, headers)
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

	updateBQId(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.UserUpdateQBId)
	}

	// getIsSystem(query: any): Observable<any> {
	// 	return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.UserGetIsSystem)
	// }

	getIsNotRole(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.UserGetIsNotRole)
	}

	getIsSystem(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.UserGetIsSystem)
	}

	getIsSystemOrderByUnit(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.getIsSystemOrderByUnit)
	}

	insertMultiUserRole(query: any, titleRole: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.INSERT),
			logObject: encodeURIComponent(LOG_OBJECT.SY_USER_GET_BY_ROLE + ' ' + titleRole),
		}
		return this.serviceInvoker.postwithHeaders(query, AppSettings.API_ADDRESS + Api.InsertMultiUserRole, headers)
	}

	insert(data: any, files: any = null): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.INSERT),
			logObject: encodeURIComponent(LOG_OBJECT.SY_USER + ' ' + data.fullName),
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
			logObject: encodeURIComponent(LOG_OBJECT.SY_USER + ' ' + data.fullName),
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

	userSystemInsert(data: any, files: any = null): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.INSERT),
			logObject: encodeURIComponent(LOG_OBJECT.SY_USER + ' ' + data.fullName),
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

		return this.serviceInvoker.postwithHeaders(form, AppSettings.API_ADDRESS + Api.UserSystemCreate, headers)
	}

	userSystemUpdate(data: any, files: any = null): Observable<any> {
		let form = new FormData()

		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.UPDATE),
			logObject: encodeURIComponent(LOG_OBJECT.SY_USER + ' ' + data.fullName),
		}

		for (let key in data) {
			form.append(key, data[key])
		}
		if (files != null && files.length > 0) {
			for (let item of files) {
				form.append('files', item, item.name)
			}
		}
		return this.serviceInvoker.postwithHeaders(form, AppSettings.API_ADDRESS + Api.UserSystemUpdate, headers)
	}

	userUpdateProfile(data: any, files: any = null): Observable<any> {
		let form = new FormData()

		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.UPDATE),
			logObject: encodeURIComponent(LOG_OBJECT.SY_USER + ' ' + data.fullName),
		}

		for (let key in data) {
			form.append(key, data[key])
		}
		if (files != null && files.length > 0) {
			for (let item of files) {
				form.append('files', item, item.name)
			}
		}
		return this.serviceInvoker.postwithHeaders(form, AppSettings.API_ADDRESS + Api.UserUpdateProfile, headers)
	}

	delete(data: any, title: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.DELETE),
			logObject: encodeURIComponent(LOG_OBJECT.SY_USER + ' ' + title),
		}
		return this.serviceInvoker.postwithHeaders(data, AppSettings.API_ADDRESS + Api.UserDelete, headers)
	}
	changeStatus(data: any, title: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.UPDATESTATUS),
			logObject: encodeURIComponent(LOG_OBJECT.SY_USER + ' ' + title),
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

	deleteUserRole(data: any, titleRole: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.DELETE),
			logObject: encodeURIComponent(LOG_OBJECT.SY_USER_GET_BY_ROLE + ' ' + titleRole),
		}
		return this.serviceInvoker.postwithHeaders(data, AppSettings.API_ADDRESS + Api.DeleteUserRole, headers)
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
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.DELETE),
			logObject: encodeURIComponent(LOG_OBJECT.SY_SYSTEM),
		}
		return this.serviceInvoker.postwithHeaders(request, AppSettings.API_ADDRESS + Api.SystemLogDelete, headers)
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

	exportExcelSystemLog(request): Observable<any> {
		return this.http
			.get(AppSettings.API_ADDRESS + Api.ExportExcel, {
				responseType: "blob",
				params: request,
				headers: new HttpHeaders({
					"Access-Control-Allow-Origin": "*",
					Authorization: `Bearer ${this.localStronageService.getAccessToken()}`,
					"Content-Type": "application/json",
				}),
			})
			.pipe(tap(), catchError(this.handleError<Blob>()));
	}
}
