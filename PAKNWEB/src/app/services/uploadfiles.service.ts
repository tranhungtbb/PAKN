import { Injectable } from '@angular/core'
import { HttpClient, HttpHeaders } from '@angular/common/http'
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

	uploadImageNews(request: any): Observable<any> {
		let tempheaders = new HttpHeaders({
			ipAddress: this.localStorage.getIpAddress() && this.localStorage.getIpAddress() != 'null' ? this.localStorage.getIpAddress() : '',
			macAddress: '',
		})
		const form = new FormData()
		if (request.Files) {
			request.Files.forEach((item) => {
				form.append('Files', item)
			})
		}
		const httpPackage = {
			headers: tempheaders,
			reportProgress: true,
		}
		return this.http.post(AppSettings.API_ADDRESS + Api.UploadImageNews, form, httpPackage)
	}

	getEncryptedPath(data: any): Observable<any> {
		return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.GetEncryptedPath)
	}
	downloadApplication(data: any): Observable<Blob> {
		return this.http.get(AppSettings.API_ADDRESS + Api.DownloadApp, { responseType: 'blob', params: data }).pipe(tap())
	}
	downloadFile(data: any) {
		return this.http.get(AppSettings.API_ADDRESS + Api.download, { responseType: 'blob', params: data }).pipe(tap())
	}

	downloadFilebyId(data: any) {
		return this.http.get(AppSettings.API_ADDRESS + Api.DownloadFilebyId, { responseType: 'blob', params: data }).pipe(tap(), catchError(this.handleError<Blob>()))
	}

	downloadFileSupport(data: any) {
		return this.http.get(AppSettings.API_ADDRESS + Api.downloadFileSupport, { responseType: 'blob', params: data }).pipe(tap(), catchError(this.handleError<Blob>()))
	}

	checkFileWasExitsted(event: any, files: any[]) {
		if (event.target.files.length > 0) {
			for (var i = 0; i < event.target.files.length; i++) {
				if (event.target.files[i].size > 20485760) {
					return 3
				} else {
					if (files.length === 0) {
						return 1
					} else {
						for (var j = 0; j < files.length; j++) {
							if (files[j].name === event.target.files[i].name) {
								return 2
							}
						}
					}
				}
			}
			return 1
		}
	}

	checkFileWasExitstedThuMuc(event: any, files: any[]) {
		if (event.target.files.length > 0) {
			for (var i = 0; i < event.target.files.length; i++) {
				if (event.target.files[i].size > 20485760) {
					return 3
				} else {
					if (files.length === 0) {
						return 1
					} else {
						for (var j = 0; j < files.length; j++) {
							if (files[j].name === event.target.files[i].name || files[j].tenFile === event.target.files[i].name) {
								return 2
							}
						}
					}
				}
			}
			return 1
		}
	}

	deleteFileAttacth(files: Array<any>, index: number) {
		for (var i = files.length - 1; i >= 0; i--) {
			if (index == i) {
				files.splice(i, 1)
			}
		}
		return files
	}

	GetFileImage(data: any): Observable<Blob> {
		return this.http.get(AppSettings.API_ADDRESS + Api.getFileImage, { responseType: 'blob', params: data }).pipe(tap())
	}

	getInfoInputParamByFileId() {}
	private log(message: string) {
		this.snackBar.open(message, 'close', {
			duration: 2000,
		})
	}

	getFileSupport(data: any): Observable<any> {
		return this.serviceInvoker.get(data, AppSettings.API_ADDRESS + Api.getFileSupport)
	}
	private handleError<T>(operation = 'operation', result?: T) {
		return (error: any): Observable<T> => {
			// TODO: send the error to remote logging infrastructure
			console.error(error) // log to console instead

			// TODO: better job of transforming error for user consumption
			this.log(`${operation} failed: ${error.message}`)

			// Let the app keep running by returning an empty result.
			return of(result as T)
		}
	}
}
