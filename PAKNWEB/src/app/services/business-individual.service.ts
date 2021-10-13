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
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.UPDATESTATUS),
			logObject: encodeURIComponent(LOG_OBJECT.BI_INDIVIDUAL),
		}
		return this.serviceInvoker.postwithHeaders(data, AppSettings.API_ADDRESS + Api.IndivialChangeStatus, headers)
	}

	individualDelete(data: any, title: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.DELETE),
			logObject: encodeURIComponent(LOG_OBJECT.BI_INDIVIDUAL + ' ' + title),
		}
		return this.serviceInvoker.postwithHeaders(data, AppSettings.API_ADDRESS + Api.IndivialDelete, headers)
	}

	individualRegister(data: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.INSERT),
			logObject: encodeURIComponent(LOG_OBJECT.BI_INDIVIDUAL + ' ' + data.fullName),
		}
		return this.serviceInvoker.postwithHeaders(data, AppSettings.API_ADDRESS + Api.InvididualRegister, headers)
	}

	invididualUpdate(data: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.UPDATE),
			logObject: encodeURIComponent(LOG_OBJECT.BI_INDIVIDUAL + ' ' + data.fullName),
		}
		return this.serviceInvoker.postwithHeaders(data, AppSettings.API_ADDRESS + Api.InvididualUpdate, headers)
	}

	individualById(request: any): Observable<any> {
		return this.serviceInvoker.get(request, AppSettings.API_ADDRESS + Api.InvididualGetByID)
	}

	invididualImportFile(data: any): Observable<any> {
		return this.serviceInvoker.postfile(data, AppSettings.API_ADDRESS + Api.ImportDataInvididual)
	}

	individualCheckExists(params: any): Observable<any> {
		return this.serviceInvoker.get(params, AppSettings.API_ADDRESS + Api.IndividualCheckExists)
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
			logObject: encodeURIComponent(LOG_OBJECT.BI_BUSINESS + ' ' + data.business),
		}
		return this.serviceInvoker.postwithHeaders(data, AppSettings.API_ADDRESS + Api.BusinessDelete, headers)
	}

	businessRegister(data: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.INSERT),
			logObject: encodeURIComponent(LOG_OBJECT.BI_BUSINESS + ' ' + data.Business),
		}
		return this.serviceInvoker.postwithHeaders(data, AppSettings.API_ADDRESS + Api.BusinessRegister, headers)
	}

	businessGetByID(request: any): Observable<any> {
		return this.serviceInvoker.get(request, AppSettings.API_ADDRESS + Api.BusinessGetByID)
	}

	businessUpdate(request: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.UPDATE),
			logObject: encodeURIComponent(LOG_OBJECT.BI_BUSINESS + ' ' + request.Business),
		}
		return this.serviceInvoker.postwithHeaders(request, AppSettings.API_ADDRESS + Api.BusinessUpdate, headers)
	}

	businessImportFile(data: any): Observable<any> {
		return this.serviceInvoker.postfile(data, AppSettings.API_ADDRESS + Api.ImportDataBusiness)
	}
}
