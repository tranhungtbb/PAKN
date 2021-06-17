import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { ServiceInvokerService } from '../commons/service-invoker.service'
import { from, Observable } from 'rxjs'
import { AppSettings } from '../constants/app-setting'
import { Api } from '../constants/api'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
// import { GroupUserObject } from '../models/groupUserObject';
// import { RequestGroupUserModel } from '../models/requestGroupUserModel';

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

		return this.serviceInvoker.post(form, `${AppSettings.API_ADDRESS}/${Api.EmailManagementUpdate}?userId=${this.currentUserId}`)
	}

	public getById(id: number): Observable<any> {
		return this.serviceInvoker.get({ id }, `${AppSettings.API_ADDRESS}/${Api.EmailManagementGetById}?userId=${this.currentUserId}`)
	}
	public getPagedList(query: any): Observable<any> {
		return this.serviceInvoker.get(query, `${AppSettings.API_ADDRESS}/${Api.EmailManagementGetPagedList}?userId=${this.currentUserId}`)
	}
	public Delete(id:number): Observable<any> {
		return this.serviceInvoker.get({id}, `${AppSettings.API_ADDRESS}/${Api.EmailManagementDelete}?userId=${this.currentUserId}`)
	}
	public SendEmail(id:number): Observable<any> {
		return this.serviceInvoker.get({id}, `${AppSettings.API_ADDRESS}/${Api.EmailManagementSendEmail}?userId=${this.currentUserId}`)
	}
	public getHisPagedList(query:any): Observable<any> {
		return this.serviceInvoker.get(query, `${AppSettings.API_ADDRESS}/${Api.EmailManagementHisPagedList}?userId=${this.currentUserId}`)
	}
	
}
