import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { ServiceInvokerService } from '../commons/service-invoker.service'
import { from, Observable } from 'rxjs'
import { AppSettings } from '../constants/app-setting'
import { Api } from '../constants/api'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { LOG_ACTION, LOG_OBJECT } from '../constants/CONSTANTS'

@Injectable({
	providedIn: 'root',
})
export class EmailManagementService {
	constructor(private http: HttpClient, private serviceInvoker: ServiceInvokerService, private userStorage: UserInfoStorageService) {}

	currentUserId = this.userStorage.getUserId()
	public createOrUpdate(body: any, files: Array<any>): Observable<any> {
		var form = new FormData()
		form.append(
			'ListAttachmentNew',
			JSON.stringify(
				files.map((c) => {
					return { name: c.name, fileType: c.fileType }
				})
			)
		)
		body.Data.userUpdateId = this.userStorage.getUserId()
		body.Data.userCreatedId = this.userStorage.getUserId()
		if (body) {
			for (let key in body) {
				form.append(key, JSON.stringify(body[key]))
			}
		}
		if (files) {
			for (let item of files) {
				form.append('files', item, item.name)
			}
		}
		let headers = {
			logAction: encodeURIComponent( body.Data.id == 0 ? LOG_ACTION.INSERT : LOG_ACTION.UPDATE),
			logObject: encodeURIComponent(LOG_OBJECT.EMAIL),
		}

		return this.serviceInvoker.postwithHeaders(form, `${AppSettings.API_ADDRESS}/${Api.EmailManagementUpdate}?userId=${this.currentUserId}`, headers)
	}

	public getById(id: number): Observable<any> {
		return this.serviceInvoker.get({ id }, `${AppSettings.API_ADDRESS}/${Api.EmailManagementGetById}?userId=${this.currentUserId}`)
	}
	public getPagedList(query: any): Observable<any> {
		return this.serviceInvoker.get(query, `${AppSettings.API_ADDRESS}/${Api.EmailManagementGetPagedList}?userId=${this.currentUserId}`)
	}
	public Delete(id:number): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.DELETE),
			logObject: encodeURIComponent(LOG_OBJECT.EMAIL),
		}
		return this.serviceInvoker.getwithHeaders({id}, `${AppSettings.API_ADDRESS}/${Api.EmailManagementDelete}?userId=${this.currentUserId}`, headers)
	}
	public SendEmail(id:number): Observable<any> {
		return this.serviceInvoker.get({id}, `${AppSettings.API_ADDRESS}/${Api.EmailManagementSendEmail}?userId=${this.currentUserId}`)
	}
	public getHisPagedList(query:any): Observable<any> {
		return this.serviceInvoker.get(query, `${AppSettings.API_ADDRESS}/${Api.EmailManagementHisPagedList}?userId=${this.currentUserId}`)
	}
	
}
