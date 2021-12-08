import { Injectable } from '@angular/core'
import { HttpClient, HttpHeaders } from '@angular/common/http'
import { ServiceInvokerService } from '../commons/service-invoker.service'
import { Observable, of } from 'rxjs'
import { AppSettings } from '../constants/app-setting'
import { Api } from '../constants/api'
import { catchError, tap } from 'rxjs/operators'
import { LOG_ACTION, LOG_OBJECT } from '../constants/CONSTANTS'
import { UserInfoStorageService } from '../commons/user-info-storage.service'

@Injectable({
	providedIn: 'root',
})
export class RecommendationCommentService {
	constructor(private http: HttpClient, private serviceInvoker: ServiceInvokerService, private storeageService: UserInfoStorageService) { }

	public insert(body: any): Observable<any> {
		var form = new FormData()
		for (var item in body) {
			form.append(item, body[item])
		}
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.INSERT),
			logObject: encodeURIComponent(LOG_OBJECT.MR_COMMENT),
		}
		return this.serviceInvoker.postwithHeaders(body, AppSettings.API_ADDRESS + Api.MRRecommendationCommentInsert, headers)
	}

	public getAllOnPage(req: any): Observable<any> {
		return this.serviceInvoker.get(req, AppSettings.API_ADDRESS + Api.MRRecommendationCommentGetOnPage)
	}

	public getAllOnPageBase(req: any): Observable<any> {
		return this.serviceInvoker.get(req, AppSettings.API_ADDRESS + Api.MRRecommendationCommentGetOnPageBase)
	}

	public getAllByParentId(req: any): Observable<any> {
		return this.serviceInvoker.get(req, AppSettings.API_ADDRESS + Api.MRRecommendationCommentGetAllByParentId)
	}

	public getPageByParentId(req: any): Observable<any> {
		return this.serviceInvoker.get(req, AppSettings.API_ADDRESS + Api.MRRecommendationCommentGetPageByParent)
	}

	public updateStatus(req: any): Observable<any> {
		return this.serviceInvoker.post(req, AppSettings.API_ADDRESS + Api.MRRecommendationCommentUpdateStatus)
	}

	public delete(req: any): Observable<any> {
		return this.serviceInvoker.post(req, AppSettings.API_ADDRESS + Api.MRRecommendationCommentDelete)
	}


	// infomation Exchange

	public insertInfomationExchange(body: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.INSERT),
			logObject: encodeURIComponent(LOG_OBJECT.MR_INFOMATIONCHANGE),
		}
		return this.serviceInvoker.postwithHeaders(body, AppSettings.API_ADDRESS + Api.MRInfomationExchangeInsert, headers)
	}

	public getAllInfomationChangeOnPage(req: any): Observable<any> {
		return this.serviceInvoker.get(req, AppSettings.API_ADDRESS + Api.MRInfomationExchangeGetOnPage)
	}
}
