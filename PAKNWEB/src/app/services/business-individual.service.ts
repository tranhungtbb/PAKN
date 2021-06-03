import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { ServiceInvokerService } from '../commons/service-invoker.service'
import { Observable, of } from 'rxjs'
import { AppSettings } from '../constants/app-setting'
import { Api } from '../constants/api'
import { UserInfoStorageService } from '../commons/user-info-storage.service'
import { LOG_ACTION, LOG_OBJECT } from '../constants/CONSTANTS'
@Injectable({
	providedIn: 'root',
})
export class BusinessIndividualService {
	constructor(private http: HttpClient, private serviceInvoker: ServiceInvokerService, private storeageService: UserInfoStorageService) {}
	individualGetList(request: any): Observable<any> {
		return this.serviceInvoker.get(request, AppSettings.API_ADDRESS + Api.IndividualGetAllOnPage)
	}

	individualChangeStatus(data: any): Observable<any> {
		return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.IndivialChangeStatus)
	}

	individualDelete(data: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.DELETE),
			logObject: encodeURIComponent(LOG_OBJECT.BI_INDIVIDUAL),
		}
		return this.serviceInvoker.postwithHeaders(data, AppSettings.API_ADDRESS + Api.IndivialDelete, headers)
	}

	individualRegister(data: any): Observable<any> {
		// let form = new FormData()

		// for (let item in data) {
		// 	form.append(item, data[item])
		// }
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.INSERT),
			logObject: encodeURIComponent(LOG_OBJECT.BI_INDIVIDUAL),
		}
		return this.serviceInvoker.postwithHeaders(data, AppSettings.API_ADDRESS + Api.InvididualRegister, headers)
	}

	invididualUpdate(data: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.UPDATE),
			logObject: encodeURIComponent(LOG_OBJECT.BI_INDIVIDUAL),
		}
		// let form = new FormData()
		// for (let item in data) {
		// 	form.append(item, data[item])
		// }
		return this.serviceInvoker.postwithHeaders(data, AppSettings.API_ADDRESS + Api.InvididualUpdate, headers)
	}

	individualById(request: any): Observable<any> {
		return this.serviceInvoker.get(request, AppSettings.API_ADDRESS + Api.InvididualGetByID)
	}

	// invididualUpdate_Old(data: any): Observable<any> {
	// 	return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.InvididualUpdate)
	// }

	invididualImportFile(data: any): Observable<any> {
		return this.serviceInvoker.postfile(data, AppSettings.API_ADDRESS + Api.ImportDataInvididual)
	}

	individualCheckExists(params: any): Observable<any> {
		return this.serviceInvoker.get(params, AppSettings.API_ADDRESS + Api.InvididualCheckExists)
	}

	businessGetList(request: any): Observable<any> {
		return this.serviceInvoker.get(request, AppSettings.API_ADDRESS + Api.BusinessGetAllOnPage)
	}

	businessChangeStatus(data: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.UPDATESTATUS),
			logObject: encodeURIComponent(LOG_OBJECT.BI_BUSINESS),
		}
		return this.serviceInvoker.postwithHeaders(data, AppSettings.API_ADDRESS + Api.BusinessChangeStatus, headers)
	}

	businessDelete(data: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.DELETE),
			logObject: encodeURIComponent(LOG_OBJECT.BI_BUSINESS),
		}
		return this.serviceInvoker.postwithHeaders(data, AppSettings.API_ADDRESS + Api.BusinessDelete, headers)
	}

	businessRegister(data: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.INSERT),
			logObject: encodeURIComponent(LOG_OBJECT.BI_BUSINESS),
		}
		// let form = new FormData()
		// for (let item in data) {
		// 	form.append(item, data[item])
		// }

		return this.serviceInvoker.postwithHeaders(data, AppSettings.API_ADDRESS + Api.BusinessRegister, headers)
	}

	businessGetByID(request: any): Observable<any> {
		return this.serviceInvoker.get(request, AppSettings.API_ADDRESS + Api.BusinessGetByID)
	}

	businessUpdate(request: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.UPDATE),
			logObject: encodeURIComponent(LOG_OBJECT.BI_BUSINESS),
		}
		return this.serviceInvoker.postwithHeaders(request, AppSettings.API_ADDRESS + Api.BusinessUpdate, headers)
	}

	businessImportFile(data: any): Observable<any> {
		return this.serviceInvoker.postfile(data, AppSettings.API_ADDRESS + Api.ImportDataBusiness)
	}
}
