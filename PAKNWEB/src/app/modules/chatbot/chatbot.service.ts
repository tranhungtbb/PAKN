import { HttpHeaders } from '@angular/common/http'
import { EventEmitter, Injectable } from '@angular/core'
import { ServiceInvokerService } from 'src/app/commons/service-invoker.service'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { Api } from 'src/app/constants/api'
import { AppSettings } from 'src/app/constants/app-setting'
import { HttpClient } from '@angular/common/http'
import { Observable, of } from 'rxjs'

@Injectable({
	providedIn: 'root',
})
export class ChatBotService {
	constructor(private serviceInvoker: ServiceInvokerService, private storeageService: UserInfoStorageService, private http: HttpClient) { }

	getRooms(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.BotRooms)
	}

	getMessages(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.GetMessages)
	}

	createRoom(body: any) {
		return this.serviceInvoker.post(body, AppSettings.API_ADDRESS + Api.CreateRoom)
	}
	updateStatusRoom(body: any) {
		return this.serviceInvoker.post(body, AppSettings.API_ADDRESS + Api.UpdateStatusRoom)
	}
	getRoomForNotification(body: any) {
		return this.serviceInvoker.get(body, AppSettings.API_ADDRESS + Api.GetRoomForNotification)
	}
	getRoomById(body: any) {
		return this.serviceInvoker.get(body, AppSettings.API_ADDRESS + Api.GetRoomGetById)
	}


	clientSendFile(request: any): Observable<any> {
		let tempheaders = new HttpHeaders({
			ipAddress: this.storeageService.getIpAddress() && this.storeageService.getIpAddress() != 'null' ? this.storeageService.getIpAddress() : '',
			macAddress: '',
		})
		const form = new FormData()
		form.append('roomName', request.roomName)
		if (request.files) {
			request.files.forEach((item) => {
				form.append('QD', item)
			})
		}
		tempheaders.append('Content-Type', 'application/json')
		const httpPackage = {
			headers: tempheaders,
			reportProgress: true,
		}
		return this.http.post(AppSettings.API_ADDRESS + Api.UploadFileChatBot, form, httpPackage)
	}
}
