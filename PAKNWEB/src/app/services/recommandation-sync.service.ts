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
	getDichVuHCCPagedList(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.MrSyncCongThongTinDichVuHCCPagedList)
	}
	getHeThongPANKChinhPhuPagedList(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.MrSyncHeThongPANKChinhPhuPagedList)
	}
	getHeThongQuanLyKienNghiCuTriPagedList(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.MrSyncHeThongQuanLyKienNghiCuTriPagedList)
	}

	getAllPagedList(query:any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.PU_RecommandationSyncPagedList)
	}

	getDetail(query:any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.PU_RecommandationSyncGetDetail)
	}
	
}
