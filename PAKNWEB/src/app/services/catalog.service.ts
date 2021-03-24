import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { ServiceInvokerService } from '../commons/service-invoker.service'
import { Observable, of } from 'rxjs'
import { AppSettings } from '../constants/app-setting'
import { Api } from '../constants/api'
import { catchError, tap } from 'rxjs/operators'
import { LOG_ACTION, LOG_OBJECT } from '../constants/CONSTANTS'

@Injectable({
	providedIn: 'root',
})
export class CatalogService {
	private handleError<T>(operation = 'operation', result?: T) {
		return (error: any): Observable<T> => {
			console.error(error) // log to console instead
			return of(result as T)
		}
	}
	constructor(private http: HttpClient, private serviceInvoker: ServiceInvokerService) {}

	fieldGetList(request: any): Observable<any> {
		let body: any = {}
		body.logAction = LOG_ACTION.GETLIST
		body.logObject = LOG_OBJECT.CA_FIELD
		return this.serviceInvoker.getBody(request, body, AppSettings.API_ADDRESS + Api.FieldGetList)
	}

	fieldGetById(request: any): Observable<any> {
		request.logAction = LOG_ACTION.GETINFO
		request.logObject = LOG_OBJECT.CA_FIELD
		return this.serviceInvoker.get(request, AppSettings.API_ADDRESS + Api.FieldGetById)
	}

	fieldInsert(request: any): Observable<any> {
		request.logAction = LOG_ACTION.INSERT
		request.logObject = LOG_OBJECT.CA_FIELD
		return this.serviceInvoker.post(request, AppSettings.API_ADDRESS + Api.FieldInsert)
	}

	fieldUpdate(request: any): Observable<any> {
		request.logAction = LOG_ACTION.UPDATE
		request.logObject = LOG_OBJECT.CA_FIELD
		return this.serviceInvoker.post(request, AppSettings.API_ADDRESS + Api.FieldUpdate)
	}

	fieldUpdateStatus(request: any): Observable<any> {
		request.logAction = LOG_ACTION.UPDATESTATUS
		request.logObject = LOG_OBJECT.CA_FIELD
		return this.serviceInvoker.post(request, AppSettings.API_ADDRESS + Api.FieldUpdateStatus)
	}

	fieldDelete(request: any): Observable<any> {
		request.logAction = LOG_ACTION.DELETE
		request.logObject = LOG_OBJECT.CA_FIELD
		return this.serviceInvoker.post(request, AppSettings.API_ADDRESS + Api.FieldDelete)
	}

	fieldExportExcel(request): Observable<any> {
		request.logAction = LOG_ACTION.EXPORT
		request.logObject = LOG_OBJECT.CA_FIELD
		return this.http.get(AppSettings.API_ADDRESS + Api.FieldExport, { responseType: 'blob', params: request }).pipe(tap(), catchError(this.handleError<Blob>()))
	}
}
