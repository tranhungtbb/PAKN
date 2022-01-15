import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { ServiceInvokerService } from '../commons/service-invoker.service'
import { Observable } from 'rxjs'
import { AppSettings } from '../constants/app-setting'
import { Api } from '../constants/api'
import { LOG_ACTION, LOG_OBJECT } from '../constants/CONSTANTS'
import { UserInfoStorageService } from '../commons/user-info-storage.service'

const urlChatbot = 'http://14.177.236.88:8880/api'

@Injectable({
	providedIn: 'root',
})
export class ChatbotService {
	constructor(private http: HttpClient, private serviceInvoker: ServiceInvokerService, private localStronageService: UserInfoStorageService) { }

	UpdateChatbot(data: any): Observable<any> {
		const formData = new FormData()
		formData.append('ChatbotUpdateIN', JSON.stringify(data))
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.UPDATE),
			logObject: encodeURIComponent(LOG_OBJECT.SY_CHATBOX + ' ' + data.question),
		}
		return this.serviceInvoker.postwithHeaders(formData, AppSettings.API_ADDRESS + Api.ChatbotUpdate, headers)
	}
	chatbotGetList(request: any): Observable<any> {
		return this.serviceInvoker.get(request, AppSettings.API_ADDRESS + Api.ChatbotGetList)
	}
	chatbotDelete(request: any, title: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.DELETE),
			logObject: encodeURIComponent(LOG_OBJECT.SY_CHATBOX + title),
		}
		return this.serviceInvoker.postwithHeaders(request, AppSettings.API_ADDRESS + Api.ChatbotDelete, headers)
	}
	chatbotGetById(request: any): Observable<any> {
		return this.serviceInvoker.get(request, AppSettings.API_ADDRESS + Api.ChatbotGetById)
	}
	chatbotLibGetById(request: any): Observable<any> {
		return this.serviceInvoker.get(request, AppSettings.API_ADDRESS + Api.ChatbotLibGetById)
	}
	chatbotUpdateStatus(data: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.UPDATESTATUS),
			logObject: encodeURIComponent(LOG_OBJECT.SY_CHATBOX + ' ' + data.question),
		}
		return this.serviceInvoker.postwithHeaders(data, AppSettings.API_ADDRESS + Api.ChatbotUpdate, headers)
	}
	chatbotInsertQuestion(data: any): Observable<any> {
		const formData = new FormData()
		formData.append('data', JSON.stringify(data))
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.INSERT),
			logObject: encodeURIComponent(LOG_OBJECT.SY_CHATBOX + ' ' + data.question),
		}
		return this.serviceInvoker.postwithHeaders(formData, AppSettings.API_ADDRESS + Api.ChatbotInsertQuestion, headers)
	}
	getNewUserId() {
		return this.http.get(urlChatbot + '/Conversation/new')
	}
	sendToServer(kluid, data) {
		return this.http.post(urlChatbot + '/Conversation/' + kluid, data)
	}
	chatbotInsertData(data: any): Observable<any> {
		return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.ChatbotInsertData)
	}
	chatbotGetListHistory(request: any): Observable<any> {
		return this.serviceInvoker.get(request, AppSettings.API_ADDRESS + Api.ChatbotGetListHistory)
	}
	chatbotGetAllActive(request: any): Observable<any> {
		return this.serviceInvoker.get(request, AppSettings.API_ADDRESS + Api.ChatbotGetAllActive)
	}

	importExcel(request: any): Observable<any> {
		return this.serviceInvoker.postfile(request, AppSettings.API_ADDRESS + Api.ChatbotImportFile)
	}
}
