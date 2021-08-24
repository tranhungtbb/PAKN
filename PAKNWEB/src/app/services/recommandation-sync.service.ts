import { Injectable } from '@angular/core'
import { ServiceInvokerService } from '../commons/service-invoker.service'
import { Observable } from 'rxjs'
import { AppSettings } from '../constants/app-setting'
import { Api } from '../constants/api'
import { LOG_ACTION, LOG_OBJECT } from '../constants/CONSTANTS'

@Injectable({
	providedIn: 'root',
})
export class RecommandationSyncService {
	constructor(private serviceInvoker: ServiceInvokerService) {}

	getCongThongTinDienTuTinhPagedList(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.MrSyncCongThongTinDienTuTinhPagedList)
	}
	asyncCongThongTinDienTu(request : any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.ASYNC),
			logObject: encodeURIComponent(LOG_OBJECT.MR_RECOMMENDATION_GOPY),
		}
		return this.serviceInvoker.getwithHeaders(request, AppSettings.API_ADDRESS + Api.AsyncCongThongTinDienTu, headers)
	}

	getCongThongTinDienTuTinhGetById(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.MrSyncCongThongTinDienTuTinhGetById)
	}


	asyncDichVuHCC(request : any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.ASYNC),
			logObject: encodeURIComponent(LOG_OBJECT.MR_RECOMMENDATION_HOPTHUGOPY),
		}
		return this.serviceInvoker.getwithHeaders(request, AppSettings.API_ADDRESS + Api.AsyncCongThongTinDichVuHCC, headers)
	}

	getDichVuHCCPagedList(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.MrSyncCongThongTinDichVuHCCPagedList)
	}
	
	getDichVuHCCGetById(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.MrSyncCongThongTinDichVuHCCGetById)
	}

	getHeThongPANKChinhPhuPagedList(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.MrSyncHeThongPANKChinhPhuPagedList)
	}
	getHeThongPANKChinhPhuGetByObjectId(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.MrSyncHeThongPANKChinhPhuGetByObjectId)
	}

	asyncPAKNChinhPhu(): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.ASYNC),
			logObject: encodeURIComponent(LOG_OBJECT.MR_RECOMMENDATION_PAKN_CHINHPHU),
		}
		return this.serviceInvoker.getwithHeaders({}, AppSettings.API_ADDRESS + Api.AsyncMrSyncHeThongPANKChinhPhu, headers)
	}


	getHeThongQuanLyKienNghiCuTriPagedList(query: any): Observable<any> {
		return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.MrSyncHeThongQuanLyKienNghiCuTriPagedList)
	}

	// getAllPagedList(query:any): Observable<any> {
	// 	return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.PU_RecommandationSyncPagedList)
	// }

	// getDetail(query:any): Observable<any> {
	// 	return this.serviceInvoker.get(query, AppSettings.API_ADDRESS + Api.PU_RecommandationSyncGetDetail)
	// }
	
}
