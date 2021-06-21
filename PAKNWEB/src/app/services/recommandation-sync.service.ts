import { Injectable } from '@angular/core'
import { ServiceInvokerService } from '../commons/service-invoker.service'
import { Observable } from 'rxjs'
import { AppSettings } from '../constants/app-setting'
import { Api } from '../constants/api'

@Injectable({
	providedIn: 'root',
})
export class RecommandationSyncService {
	constructor(private serviceInvoker: ServiceInvokerService) {}

	getCongThongTinDienTuTinhPagedList(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.MrSyncCongThongTinDienTuTinhPagedList)
	}
}
