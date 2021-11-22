import { Injectable } from '@angular/core'
import { ServiceInvokerService } from '../commons/service-invoker.service'
import { Observable } from 'rxjs'
import { AppSettings } from '../constants/app-setting'
import { Api } from '../constants/api'

@Injectable({
	providedIn: 'root',
})
export class PuRecommendationService {
	constructor(private serviceInvoker: ServiceInvokerService) {}

	getAllPagedList(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.PuRecommendationGetAllOnPage)
	}
	getHomePage(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.PuRecommendationHomePage)
	}

	getMyRecommentdation(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.MyRecommendationGetAllOnPage)
	}

	getListOrderByCountClick(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.PuRecomentdationGetListOrderByCountClick)
	}

	getById(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.PuRecommendationGetById)
	}
	changeSatisfaction(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.PuChangeSatisfaction)
	}

	countClick(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.PuRecommendationCountClick)
	}

	recommendationStatisticsGetByUserId(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.PURecommendationStatisticsGetByUserId)
	}

	recommendationStatisticsProvince(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.PURecommendationStatisticsProvince)
	}

	recommendationStatisticsByUnitParentId(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.StatisticsByUnitParentId)
	}

	recommendationStatisticsForChart(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.StatisticsForChart)
	}
}
