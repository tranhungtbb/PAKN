import { Injectable } from '@angular/core'
import { HttpClient, HttpHeaders } from '@angular/common/http'
import { ServiceInvokerService } from '../commons/service-invoker.service'
import { Observable, of } from 'rxjs'
import { AppSettings } from '../constants/app-setting'
import { Api } from '../constants/api'
import { catchError, tap } from 'rxjs/operators'
import { LOG_ACTION, LOG_OBJECT } from '../constants/CONSTANTS'
import { UserInfoStorageService } from '../commons/user-info-storage.service'
import { RECOMMENDATION_STATUS } from 'src/app/constants/CONSTANTS'

@Injectable({
	providedIn: 'root',
})
export class RecommendationService {
	private handleError<T>(operation = 'operation', result?: T) {
		return (error: any): Observable<T> => {
			console.error(error) // log to console instead
			return of(result as T)
		}
	}
	constructor(private http: HttpClient, private serviceInvoker: ServiceInvokerService, private storeageService: UserInfoStorageService) {}

	recommendationGetDataForCreate(request: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.GETDATACREATE),
			logObject: encodeURIComponent(LOG_OBJECT.MR_RECOMMENDATION),
		}
		return this.serviceInvoker.getwithHeaders(request, AppSettings.API_ADDRESS + Api.RecommendationGetDataForCreate, headers)
	}

	recommendationGetDataForForward(request: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.GETDATACREATE),
			logObject: encodeURIComponent(LOG_OBJECT.MR_RECOMMENDATION),
		}
		return this.serviceInvoker.getwithHeaders(request, AppSettings.API_ADDRESS + Api.RecommendationGetDataForForward, headers)
	}

	recommendationGetDataForProcess(request: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.GETDATACREATE),
			logObject: encodeURIComponent(LOG_OBJECT.MR_RECOMMENDATION),
		}
		return this.serviceInvoker.getwithHeaders(request, AppSettings.API_ADDRESS + Api.RecommendationGetDataForProcess, headers)
	}

	recommendationGetList(request: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.GETLIST),
			logObject: encodeURIComponent(LOG_OBJECT.MR_RECOMMENDATION),
		}
		return this.serviceInvoker.getwithHeaders(request, AppSettings.API_ADDRESS + Api.RecommendationGetList, headers)
	}

	recommendationGetListProcess(request: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.GETLIST),
			logObject: encodeURIComponent(LOG_OBJECT.MR_RECOMMENDATION),
		}
		return this.serviceInvoker.getwithHeaders(request, AppSettings.API_ADDRESS + Api.RecommendationGetListProcess, headers)
	}

	recommendationGetListReactionaryWord(request: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.GETLIST),
			logObject: encodeURIComponent(LOG_OBJECT.MR_RECOMMENDATION),
		}
		return this.serviceInvoker.getwithHeaders(request, AppSettings.API_ADDRESS + Api.RecommendationGetListReactionaryWord, headers)
	}

	recommendationGetById(request: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.GETINFO),
			logObject: encodeURIComponent(LOG_OBJECT.MR_RECOMMENDATION),
		}
		return this.serviceInvoker.getwithHeaders(request, AppSettings.API_ADDRESS + Api.RecommendationGetById, headers)
	}

	recommendationGetByIdView(request: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.GETINFO),
			logObject: encodeURIComponent(LOG_OBJECT.MR_RECOMMENDATION),
		}
		return this.serviceInvoker.getwithHeaders(request, AppSettings.API_ADDRESS + Api.RecommendationGetByIdView, headers)
	}

	recommendationGetByIdViewPublic(request: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.GETINFO),
			logObject: encodeURIComponent(LOG_OBJECT.MR_RECOMMENDATION),
		}
		return this.serviceInvoker.getwithHeaders(request, AppSettings.API_ADDRESS + Api.RecommendationGetByIdViewPublic, headers)
	}

	recommendationGetHistories(request: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.GETINFO),
			logObject: encodeURIComponent(LOG_OBJECT.MR_HISTORIES),
		}
		return this.serviceInvoker.getwithHeaders(request, AppSettings.API_ADDRESS + Api.RecommendationGetHistories, headers)
	}

	recommendationInsert(request: any): Observable<any> {
		let tempheaders = new HttpHeaders({
			ipAddress: this.storeageService.getIpAddress() && this.storeageService.getIpAddress() != 'null' ? this.storeageService.getIpAddress() : '',
			macAddress: '',
			logAction: encodeURIComponent(LOG_ACTION.INSERT),
			logObject: encodeURIComponent(LOG_OBJECT.MR_RECOMMENDATION + ' ' + request.Data.title),
		})
		const form = new FormData()
		form.append('Data', JSON.stringify(request.Data))
		form.append('Hashtags', JSON.stringify(request.Hashtags))

		if (request.Files) {
			request.Files.forEach((item) => {
				form.append('QD', item)
			})
		}
		const httpPackage = {
			headers: tempheaders,
			reportProgress: true,
		}
		return this.http.post(AppSettings.API_ADDRESS + Api.RecommendationInsert, form, httpPackage)
	}

	recommendationUpdate(request: any): Observable<any> {
		let tempheaders = new HttpHeaders({
			ipAddress: this.storeageService.getIpAddress() && this.storeageService.getIpAddress() != 'null' ? this.storeageService.getIpAddress() : '',
			macAddress: '',
			logAction: encodeURIComponent(request.Data.status == RECOMMENDATION_STATUS.CREATED ? LOG_ACTION.UPDATE : LOG_ACTION.SEND),
			logObject: encodeURIComponent(LOG_OBJECT.MR_RECOMMENDATION + ' ' + request.Data.title),
		})
		const form = new FormData()
		form.append('Data', JSON.stringify(request.Data))
		form.append('Hashtags', JSON.stringify(request.Hashtags))
		form.append('LstXoaFile', JSON.stringify(request.LstXoaFile))

		if (request.Files) {
			request.Files.forEach((item) => {
				form.append('QD', item)
			})
		}
		const httpPackage = {
			headers: tempheaders,
			reportProgress: true,
		}
		return this.http.post(AppSettings.API_ADDRESS + Api.RecommendationUpdate, form, httpPackage)
	}

	recommendationForward(request: any, title: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.FORWARD),
			logObject: encodeURIComponent(LOG_OBJECT.MR_RECOMMENDATION + ' ' + title),
		}
		return this.serviceInvoker.postwithHeaders(request, AppSettings.API_ADDRESS + Api.RecommendationForward, headers)
	}

	recommendationProcess(request: any, title: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.PROCESSED),
			logObject: encodeURIComponent(LOG_OBJECT.MR_RECOMMENDATION + ' ' + title),
		}
		switch (request.RecommendationStatus) {
			case 2:
				headers.logAction = encodeURIComponent(LOG_ACTION.FORWARD_TT)
				break
			case 3:
				headers.logAction = encodeURIComponent(LOG_ACTION.RECEIVE_DENY)
				break
			case 4:
				headers.logAction = encodeURIComponent(LOG_ACTION.RECEIVE_APPROVED)
				break
			case 5:
				headers.logAction = encodeURIComponent(LOG_ACTION.PROCESS_SEND)
				break
			case 6:
				headers.logAction = encodeURIComponent(LOG_ACTION.PROCESS_DENY)
				break
			case 7:
				headers.logAction = encodeURIComponent(LOG_ACTION.PROCESSING)
				break
			case 9:
				headers.logAction = encodeURIComponent(LOG_ACTION.APPROVE_DENY)
				break
			case 10:
				headers.logAction = encodeURIComponent(LOG_ACTION.APPROVE)
				break
		}
		return this.serviceInvoker.postwithHeaders(request, AppSettings.API_ADDRESS + Api.RecommendationProcess, headers)
	}

	recommendationUpdateStatus(request: any, title: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.UPDATESTATUS),
			logObject: encodeURIComponent(LOG_OBJECT.MR_RECOMMENDATION + ' ' + title),
		}
		return this.serviceInvoker.postwithHeaders(request, AppSettings.API_ADDRESS + Api.RecommendationUpdateStatus, headers)
	}

	recommendationProcessConclusion(request: any): Observable<any> {
		let tempheaders = new HttpHeaders({
			ipAddress: this.storeageService.getIpAddress() && this.storeageService.getIpAddress() != 'null' ? this.storeageService.getIpAddress() : '',
			macAddress: '',
			logAction: encodeURIComponent(LOG_ACTION.APPROVE_SEND),
			logObject: encodeURIComponent(LOG_OBJECT.MR_RECOMMENDATION),
		})
		const form = new FormData()
		form.append('DataConclusion', JSON.stringify(request.DataConclusion))
		form.append('Hashtags', JSON.stringify(request.Hashtags))
		form.append('RecommendationStatus', JSON.stringify(request.RecommendationStatus))

		if (request.Files) {
			request.Files.forEach((item) => {
				form.append('QD', item)
			})
		}
		const httpPackage = {
			headers: tempheaders,
			reportProgress: true,
		}
		return this.http.post(AppSettings.API_ADDRESS + Api.RecommendationOnProcessConclusion, form, httpPackage)
	}

	recommendationDelete(request: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.DELETE),
			logObject: encodeURIComponent(LOG_OBJECT.MR_RECOMMENDATION),
		}
		return this.serviceInvoker.postwithHeaders(request, AppSettings.API_ADDRESS + Api.RecommendationDelete, headers)
	}

	recommendationGetSuggestCreate(request: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.GETLIST),
			logObject: encodeURIComponent(LOG_OBJECT.MR_RECOMMENDATION),
		}
		return this.serviceInvoker.getNotLoading(request, AppSettings.API_ADDRESS + Api.RecommendationGetSuggestCreate, headers)
	}

	recommendationGetSuggestReply(request: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.GETLIST),
			logObject: encodeURIComponent(LOG_OBJECT.MR_RECOMMENDATION),
		}
		return this.serviceInvoker.getNotLoading(request, AppSettings.API_ADDRESS + Api.RecommendationGetSuggestReply, headers)
	}

	recommendationExportExcel(request): Observable<any> {
		let headers = new HttpHeaders({
			logAction: encodeURIComponent(LOG_ACTION.EXPORT),
			logObject: encodeURIComponent(LOG_OBJECT.MR_RECOMMENDATION),
		})

		return this.serviceInvoker.getFilewithHeaders(request, AppSettings.API_ADDRESS + Api.FieldExport, headers)
	}

	getDataGraph(request): Observable<any> {
		let headers = new HttpHeaders({
			logAction: encodeURIComponent(LOG_ACTION.EXPORT),
			logObject: encodeURIComponent(LOG_OBJECT.MR_RECOMMENDATION),
		})

		return this.serviceInvoker.get(request, AppSettings.API_ADDRESS + Api.RecommendationGetDataGraph)
	}

	getSendUserDataGraph(request): Observable<any> {
		return this.serviceInvoker.get(request, AppSettings.API_ADDRESS + Api.RecommendationGetSendUserDataGraph)
	}

	recommendationGetByHashtagAllOnPage(request): Observable<any> {
		return this.serviceInvoker.get(request, AppSettings.API_ADDRESS + Api.RecommendationGetByHashtagAllOnPage)
	}

	addHashtagForRecommentdation(request): Observable<any> {
		return this.serviceInvoker.post(request, AppSettings.API_ADDRESS + Api.InsertHashtagForRecommentdation)
	}
	deleteHashtagForRecommentdation(request): Observable<any> {
		return this.serviceInvoker.post(request, AppSettings.API_ADDRESS + Api.DeleteHashtagForRecommentdation)
	}

	getDenyContent(request): Observable<any> {
		return this.serviceInvoker.get(request, AppSettings.API_ADDRESS + Api.RecommendationGetDenyContents)
	}

	get7DayDataGraph(request): Observable<any> {
		return this.serviceInvoker.get(request, AppSettings.API_ADDRESS + Api.MR_Recommendation7dayGraph)
	}
}
