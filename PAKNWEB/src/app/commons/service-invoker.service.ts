/** angular importers**/
import { Injectable } from '@angular/core'
import { HttpClient, HttpHeaders } from '@angular/common/http'
import { MatSnackBar } from '@angular/material'
/** local application importers**/
import { AppSettings } from '../constants/app-setting'
import { Observable, of } from 'rxjs'
import { tap, catchError } from 'rxjs/operators'
import { UserInfoStorageService } from './user-info-storage.service'
import { LOG_ACTION, LOG_OBJECT } from '../constants/CONSTANTS'

const httpOptions = {
	headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
}

@Injectable({
	providedIn: 'root',
})
export class ServiceInvokerService {
	userId: any
	isSuperAdmin: any
	isTongHop: any
	deparmentId: any
	unitId: any
	accountId: any
	ipAddress: any

	constructor(private http: HttpClient, public snackBar: MatSnackBar, private storeageService: UserInfoStorageService) {}

	/* Get array */
	get(element: any, url): Observable<any> {
		element.IpAddress = this.storeageService.getIpAddress()
		const httpPackage = {
			params: element,
		}

		return this.http.get(url, httpPackage).pipe(catchError(this.handleError<any>()))
	}

	/* Get array */
	getwithHeaders(element: any, url, headers: any): Observable<any> {
		let tempheaders = new HttpHeaders({
			ipAddress: this.storeageService.getIpAddress() && this.storeageService.getIpAddress() != 'null' ? this.storeageService.getIpAddress() : '',
			macAddress: '',
			logAction: headers.logAction,
			logObject: headers.logObject,
		})
		const httpPackage = {
			params: element,
			headers: tempheaders,
		}

		return this.http.get(url, httpPackage).pipe(catchError(this.handleError<any>()))
	}

	/* Get array */
	getNotLoading(element: any, url, headers: any): Observable<any> {
		let tempheaders = new HttpHeaders({
			ipAddress: this.storeageService.getIpAddress() && this.storeageService.getIpAddress() != 'null' ? this.storeageService.getIpAddress() : '',
			macAddress: '',
			logAction: headers.logAction,
			logObject: headers.logObject,
			isShowLoading: 'false',
		})
		const httpPackage = {
			params: element,
			headers: tempheaders,
		}

		return this.http.get(url, httpPackage).pipe(catchError(this.handleError<any>()))
	}

	/* Get array */
	getFilewithHeaders(element: any, url, headers: any): Observable<any> {
		let tempheaders = new HttpHeaders({
			ipAddress: this.storeageService.getIpAddress() && this.storeageService.getIpAddress() != 'null' ? this.storeageService.getIpAddress() : '',
			macAddress: '',
			logAction: headers.logAction,
			logObject: headers.logObject,
		})

		return this.http.get(url, { responseType: 'blob', params: element, headers: tempheaders }).pipe(tap(), catchError(this.handleError<Blob>()))
	}

	/* Put */
	put<T>(element: T, successMessage: string, errorMessage: string): Observable<any> {
		return this.http.put(AppSettings.API_ADDRESS, element, httpOptions).pipe(
			tap((_) => this.log(successMessage)),
			catchError(this.handleError<any>(errorMessage))
		)
	}

	/* Post */
	post(element: any, url: string): Observable<any> {
		if (element == undefined || element == '') {
			element = {}
		}

		element.IpAddress = this.storeageService.getIpAddress()
		element.Role = this.storeageService.getRole()
		const httpPackage = {
			params: element,
		}
		var result = this.http.post(url, element, httpPackage).pipe(catchError(this.handleError<any>()))
		return result
	}

	/* Post */
	postwithHeaders(element: any, url: string, headers: any): Observable<any> {
		if (element == undefined || element == '') {
			element = {}
		}
		let tempheaders = new HttpHeaders({
			ipAddress: this.storeageService.getIpAddress() && this.storeageService.getIpAddress() != 'null' ? this.storeageService.getIpAddress() : '',
			macAddress: '',
			logAction: headers.logAction,
			logObject: headers.logObject,
		})
		const httpPackage = {
			params: element,
			headers: tempheaders,
		}

		var result = this.http.post(url, element, httpPackage).pipe(catchError(this.handleError<any>()))
		return result
	}

	postlogin(element: any, url: string): Observable<any> {
		if (element == undefined || element == '') {
			element = {}
		}
		element.IpAddress = this.storeageService.getIpAddress()

		const httpPackage = {
			params: element,
		}

		return this.http.post(url, element, httpPackage).pipe(catchError(this.handleError<any>()))
	}
	/*Delete*/
	delete<T>(id, successMessage: string, errorMessage: string): Observable<T> {
		const deleteApi = `${AppSettings.API_ADDRESS}/${id}`
		return this.http.delete<T>(deleteApi, httpOptions).pipe(
			tap((data: T) => this.log(successMessage)),
			catchError(this.handleError<T>(errorMessage))
		)
	}

	postfile(element: any, url: string): Observable<Blob> {
		if (element == undefined || element == '') {
			element = {}
		}

		const httpPackage = {
			params: element,
		}
		return this.http.post(url, element, httpPackage).pipe(catchError(this.handleError<any>()))
	}
	/** Log a message with the MessageService */
	private log(message: string) {
		this.snackBar.open(message, 'close', {
			duration: 2000,
		})
	}

	/**
	 * Handle Http operation that failed.
	 * Let the app continue.
	 * @param operation - name of the operation that failed
	 * @param result - optional value to return as the observable result
	 */
	private handleError<T>(operation = 'operation', result?: T) {
		return (error: any): Observable<T> => {
			// TODO: send the error to remote logging infrastructure
			console.error(error) // log to console instead

			// TODO: better job of transforming error for user consumption
			//this.log(`${operation} failed: ${error.message}`);

			// Let the app keep running by returning an empty result.
			return of(result as T)
		}
	}
}
