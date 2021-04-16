import { Injectable } from '@angular/core'
import { ServiceInvokerService } from '../commons/service-invoker.service'
import { Observable } from 'rxjs'
import { AppSettings } from '../constants/app-setting'
import { Api } from '../constants/api'

@Injectable({
	providedIn: 'root',
})
export class NotificationService {
	constructor(private serviceInvoker: ServiceInvokerService) {}

	insertNotificationTypeNews(query: any): Observable<any> {
		return this.serviceInvoker.post(query, AppSettings.API_ADDRESS + Api.NotificationInsertTypeNews)
	}

	getListOrderByCountClick(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.PuRecomentdationGetListOrderByCountClick)
	}

	getById(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.PuRecommendationGetById)
	}
}
