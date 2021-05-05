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
	constructor(private http: HttpClient, private serviceInvoker: ServiceInvokerService, private storeageService: UserInfoStorageService) {}

	public insert(body: any): Observable<any> {
		var form = new FormData()
		for (var item in body) {
			form.append(item, body[item])
		}
		return this.serviceInvoker.post(body, AppSettings.API_ADDRESS + Api.MRRecommendationCommentInsert)
	}

	public getAllOnPage(req: any): Observable<any> {
		return this.serviceInvoker.get(req, AppSettings.API_ADDRESS + Api.MRRecommendationCommentGetOnPage)
	}
}
