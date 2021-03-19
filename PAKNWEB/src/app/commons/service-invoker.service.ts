/** angular importers**/
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { MatSnackBar } from '@angular/material';
/** local application importers**/
import { AppSettings } from '../constants/app-setting';
import { Observable, of } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import { UserInfoStorageService } from './user-info-storage.service';


const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})

export class ServiceInvokerService {
  userId: any;
  isSuperAdmin: any;
  isTongHop: any;
  deparmentId: any;
  unitId: any;
  accountId: any;
  ipAddress: any;

  constructor(private http: HttpClient,
    public snackBar: MatSnackBar,
    private storeageService: UserInfoStorageService) {
    this.userId = this.storeageService.getUserId();
    this.isSuperAdmin = this.storeageService.getIsSuperAdmin();
    this.deparmentId = this.storeageService.getDeparmentId();
    this.unitId = this.storeageService.getUnitId();
    this.accountId = this.storeageService.getAccountId();
  }

  /* Get array */
  get(element: any, url): Observable<any> {
    const httpPackage = {
      params: element
    };

    return this.http.get(url, httpPackage)
      .pipe(
        catchError(this.handleError<any>())
      );
  }

  /* Put */
  put<T>(element: T, successMessage: string, errorMessage: string): Observable<any> {
    return this.http.put(AppSettings.API_ADDRESS, element, httpOptions)
      .pipe(
        tap(_ => this.log(successMessage)),
        catchError(this.handleError<any>(errorMessage))
      );
  }

  /* Post */
  post(element: any, url: string): Observable<any> {
    if (element == undefined || element == '') {
      element = {};
    }

    element.UserId = this.storeageService.getUserId();
    element.IsSuperAdmin = this.storeageService.getIsSuperAdmin();
    element.UnitId = this.storeageService.getUnitId();
    element.AccountId = this.storeageService.getAccountId();
    element.DeparmentId = this.storeageService.getDeparmentId();
    element.IpAddress = this.storeageService.getIpAddress();
    element.Role = this.storeageService.getRole();
    const httpPackage = {
      params: element
    };
    var result = this.http.post(url, element, httpPackage)
      .pipe(
        catchError(this.handleError<any>())
      );
    return result;
  }

  postlogin(element: any, url: string): Observable<any> {
    if (element == undefined || element == '') {
      element = {};
    }

    const httpPackage = {
      params: element
    };

    return this.http.post(url, element, httpPackage)
      .pipe(
        catchError(this.handleError<any>())
      );
  }
  /*Delete*/
  delete<T>(id, successMessage: string, errorMessage: string): Observable<T> {
    const deleteApi = `${AppSettings.API_ADDRESS}/${id}`;
    return this.http.delete<T>(deleteApi, httpOptions)
      .pipe(
        tap((data: T) => this.log(successMessage)),
        catchError(this.handleError<T>(errorMessage))
      );
  }

  postfile(element: any, url: string): Observable<Blob> {
    if (element == undefined || element == '') {
      element = {};
    }
    element.UserId = this.userId;
    element.IsSuperAdmin = this.isSuperAdmin;
    element.IsTongHop = this.isTongHop;
    element.DeparmentId = this.deparmentId;
    element.UnitId = this.unitId;
    element.AccountId = this.accountId;

    const httpPackage = {
      params: element
    };
    return this.http.post(url, element, httpPackage)
      .pipe(
        catchError(this.handleError<any>())
      );
  }
  /** Log a message with the MessageService */
  private log(message: string) {
    this.snackBar.open(message, 'close', {
      duration: 2000,
    });
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
      console.error(error); // log to console instead

      // TODO: better job of transforming error for user consumption
      //this.log(`${operation} failed: ${error.message}`);

      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }
}
