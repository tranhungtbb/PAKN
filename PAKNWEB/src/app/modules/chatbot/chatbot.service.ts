import { EventEmitter, Injectable } from '@angular/core'
import { Observable } from 'rxjs'
import { ServiceInvokerService } from 'src/app/commons/service-invoker.service'
import { Api } from 'src/app/constants/api'
import { AppSettings } from 'src/app/constants/app-setting'

@Injectable({
	providedIn: 'root',
})
export class ChatBotService {
	// public currentDialog: any = {}
	// public dialogs: any = {}
	// dialogsEvent: EventEmitter<any> = new EventEmitter()
	// currentDialogEvent: EventEmitter<any> = new EventEmitter()

	constructor(private serviceInvoker: ServiceInvokerService) {}

	getRooms(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.BotRooms)
	}
}
