import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { ServiceInvokerService } from '../commons/service-invoker.service'
import { Observable } from 'rxjs'
import { AppSettings } from '../constants/app-setting'
import { Api } from '../constants/api'
import { LOG_ACTION, LOG_OBJECT } from '../constants/CONSTANTS'

import { UserInfoStorageService } from '../commons/user-info-storage.service'

@Injectable({
	providedIn: 'root',
})
export class ChatbotService {
	constructor(private http: HttpClient, private serviceInvoker: ServiceInvokerService, private localStronageService: UserInfoStorageService) {}

	UpdateChatbot(data: any): Observable<any> {
		return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.ChatbotUpdate)
	}
	chatbotGetList(request: any): Observable<any> {
		return this.serviceInvoker.get(request, AppSettings.API_ADDRESS + Api.ChatbotGetList)
	}
	chatbotDelete(request: any): Observable<any> {
		return this.serviceInvoker.post(request, AppSettings.API_ADDRESS + Api.ChatbotDelete)
	}
	chatbotGetById(request: any): Observable<any> {
		return this.serviceInvoker.get(request, AppSettings.API_ADDRESS + Api.ChatbotGetById)
	}
	chatbotUpdateStatus(data: any): Observable<any> {
		return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.ChatbotUpdate)
	}
	chatbotInsertQuestion(data: any): Observable<any> {
		return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.ChatbotInsertQuestion)
	}
}
