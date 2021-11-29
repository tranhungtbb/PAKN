import { HttpHeaders } from '@angular/common/http'
import { EventEmitter, Injectable } from '@angular/core'
import { ServiceInvokerService } from 'src/app/commons/service-invoker.service'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { Api } from 'src/app/constants/api'
import { AppSettings } from 'src/app/constants/app-setting'
import { HttpClient } from '@angular/common/http'
import { Observable, of } from 'rxjs'
import { tap, catchError } from 'rxjs/operators'

@Injectable({
	providedIn: 'root',
})
export class ChatBotService {
	constructor(private serviceInvoker: ServiceInvokerService, private storeageService: UserInfoStorageService, private http: HttpClient) {}

	getRooms(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.BotRooms)
	}

	getMessages(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.GetMessages)
	}

	createRoom(body: any) {
		return this.serviceInvoker.post(body, AppSettings.API_ADDRESS + Api.CreateRoom)
	}
}
