import { HttpClient, HttpHeaders } from '@angular/common/http'
import { Observable } from 'rxjs'
import { environment } from 'src/environments/environment'
import { Api } from '../constants/api'
import { AppSettings } from '../constants/app-setting'

export class UploadDocumentAdapter {
	public loader: any
	public url: string
	public xhr: XMLHttpRequest
	public token: string

	constructor(loader, private http: HttpClient, url: string) {
		this.loader = loader

		// change "environment.BASE_URL" key and API path
		this.url = url
		this.token = localStorage.getItem('accessToken')
	}

	upload() {
		return new Promise(async (resolve, reject) => {
			this.loader.file.then((file) => {
				this._initRequest()
				this._initListeners(resolve, reject, file)
			})
		})
	}

	abort() {
		if (this.xhr) {
			this.xhr.abort()
		}
	}

	_initRequest() { }

	_initListeners(resolve, reject, file) {
		this.uploadImageNews(file).subscribe((response) => {
			resolve({
				default: response.result.data.fullPaths,
			})
		})
	}

	uploadImageNews(file: any): Observable<any> {
		const form = new FormData()
		form.append('Files', file)
		const httpPackage = {
			reportProgress: true,
		}
		return this.http.post(this.url, form, httpPackage)
	}
}
