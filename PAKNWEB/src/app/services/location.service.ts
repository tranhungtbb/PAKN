/** angular importers**/
import { Injectable } from '@angular/core'
import { HttpClient, HttpHeaders } from '@angular/common/http'
import { MatSnackBar } from '@angular/material'
/** local application importers**/
import { AppSettings } from '../constants/app-setting'
import { Observable, of } from 'rxjs'
// import { tap, catchError } from 'rxjs/operators'
import { catchError, map, tap } from 'rxjs/operators'

import { LocationURL, GoogleApiKey } from 'src/app/constants/CONSTANTS'
import { ServiceInvokerService } from '../commons/service-invoker.service'
import { RequestOptions, ResponseContentType } from '@angular/http'

@Injectable({
	providedIn: 'root',
})
export class LocationService {
	constructor(private http: HttpClient, private serviceInvoker: ServiceInvokerService) {}

	getPosition(): Promise<any> {
		return new Promise((resolve, reject) => {
			navigator.geolocation.getCurrentPosition(
				(resp) => {
					resolve({ lng: resp.coords.longitude, lat: resp.coords.latitude })
				},
				(err) => {
					reject(err)
				}
			)
		})
	}

	getPositionCurrent(): Observable<any> {
		const element = { key: GoogleApiKey }
		const httpPackage = {
			params: element,
		}
		return this.http.post(LocationURL, element, httpPackage).pipe(catchError(this.handleError<any>()))
	}

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
