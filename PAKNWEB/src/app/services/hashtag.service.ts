import { Injectable } from '@angular/core'
import { ServiceInvokerService } from '../commons/service-invoker.service'
import { Observable } from 'rxjs'
import { AppSettings } from '../constants/app-setting'
import { Api } from '../constants/api'
import { LOG_ACTION, LOG_OBJECT } from '../constants/CONSTANTS'

@Injectable({
	providedIn: 'root',
})
export class HashtagService {
	constructor(private serviceInvoker: ServiceInvokerService) {}

	getAllPagedList(query: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.GETLIST),
			logObject: encodeURIComponent(LOG_OBJECT.CA_HASHTAG),
		}
		return this.serviceInvoker.getwithHeaders(query, AppSettings.API_ADDRESS + Api.HashtagGetList, headers)
	}
	getAll(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.HashtagGetAll)
	}

	recommendationGetByHashtagAllOnPage(query: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.GETLIST),
			logObject: encodeURIComponent(LOG_OBJECT.MR_BY_HASHTAG),
		}
		return this.serviceInvoker.getwithHeaders(query, AppSettings.API_ADDRESS + Api.RecommendationGetByHashtagAllOnPage, headers)
	}

	getById(query: any): Observable<any> {
		let headers = new Headers()
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.HashtagGetById)
	}
	create(data: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.INSERT),
			logObject: encodeURIComponent(LOG_OBJECT.CA_HASHTAG + ' ' + data.name),
		}
		return this.serviceInvoker.postwithHeaders(data, AppSettings.API_ADDRESS + Api.HashtagInsert, headers)
	}

	update(data: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.UPDATE),
			logObject: encodeURIComponent(LOG_OBJECT.CA_HASHTAG + ' ' + data.name),
		}
		return this.serviceInvoker.postwithHeaders(data, AppSettings.API_ADDRESS + Api.HashtagUpdate, headers)
	}
	changeStatus(data: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.UPDATESTATUS),
			logObject: encodeURIComponent(LOG_OBJECT.CA_HASHTAG + ' ' + data.name),
		}
		return this.serviceInvoker.postwithHeaders(data, AppSettings.API_ADDRESS + Api.HashtagUpdate, headers)
	}

	delete(data: any, title: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.DELETE),
			logObject: encodeURIComponent(LOG_OBJECT.CA_HASHTAG + ' ' + title),
		}
		return this.serviceInvoker.postwithHeaders(data, AppSettings.API_ADDRESS + Api.HashtagDelete, headers)
	}
}
