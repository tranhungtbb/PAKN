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

	getStatisticRecommendationByUnitDetail(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.StatisticRecommendationByUnitDetail)
	}

	getStatisticRecommendationByField(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.StatisticRecommendationByField)
	}
	getStatisticRecommendationByFieldDetail(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.StatisticRecommendationByFieldDetail)
	}

	getStatisticRecommendationByGroupWord(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.StatisticRecommendationByGroupWord)
	}

	getStatisticRecommendationByGroupWordDetail(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.StatisticRecommendationByGroupWordDetail)
	}

	getProcessingStatus(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.StatisticRecommendationProcessingStatus)
	}
	getProcessingResult(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.StatisticRecommendationProcessingResults)
	}
	getProcessingResultByReceptionType(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.StatisticRecommendationProcessingResultsByReceptionType)
	}
	getProcessingResultByType(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.StatisticRecommendationProcessingResultsByType)
	}
	getProcessingResultByUnit(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.StatisticRecommendationProcessingResultsByUnit)
	}

	getProcessingResultByUnitAndReception(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.StatisticRecommendationProcessingResultsByUnitAndReception)
	}
	getStatisticRecommendationForMenu(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.StatisticRecommendationForMenu)
	}
}
