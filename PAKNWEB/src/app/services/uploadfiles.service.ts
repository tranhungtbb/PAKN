import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { ServiceInvokerService } from '../commons/service-invoker.service'
import { Observable, of } from 'rxjs'
import { AppSettings } from '../constants/app-setting'
import { Api } from '../constants/api'
import { UserInfoStorageService } from '../commons/user-info-storage.service'
// import { ResponseContentType, RequestOptions, ResponseType } from '@angular/http';
import { tap, catchError } from 'rxjs/operators'
import { MatSnackBar } from '@angular/material'

@Injectable({
	providedIn: 'root',
})
export class UploadFileService {
	constructor(private http: HttpClient, private serviceInvoker: ServiceInvokerService, private localStorage: UserInfoStorageService, public snackBar: MatSnackBar) {}
}
