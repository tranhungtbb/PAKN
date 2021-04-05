import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { ServiceInvokerService } from '../commons/service-invoker.service'
import { Observable } from 'rxjs'
import { AppSettings } from '../constants/app-setting'
import { Api } from '../constants/api'

@Injectable({
	providedIn: 'root',
})
export class DashBoardService {
	constructor(private http: HttpClient, private serviceInvoker: ServiceInvokerService) {}

	getDashBoard(): Observable<any> {
		var Request = {}
		return this.serviceInvoker.post(Request, AppSettings.API_ADDRESS + Api.GET_DASH_BOARD)
	}
}
