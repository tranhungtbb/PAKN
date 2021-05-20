import { Injectable } from '@angular/core'
import { ServiceInvokerService } from '../commons/service-invoker.service'
import { Observable } from 'rxjs'
import { AppSettings } from '../constants/app-setting'
import { Api } from '../constants/api'

@Injectable({
	providedIn: 'root',
})
export class StatisticService {
	constructor(private serviceInvoker: ServiceInvokerService) {}

	getStatisticRecommendationByUnit(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.StatisticRecommendationByUnit)
	}

	getStatisticRecommendationByField(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.StatisticRecommendationByField)
	}

	getStatisticRecommendationByGroupWord(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.StatisticRecommendationByGroupWord)
	}

	getStatisticRecommendationByGroupWordDetail(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.StatisticRecommendationByGroupWordDetail)
	}
}
