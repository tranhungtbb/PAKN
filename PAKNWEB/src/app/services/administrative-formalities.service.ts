import { Injectable } from '@angular/core'
import { HttpClient, HttpHeaders } from '@angular/common/http'
import { ServiceInvokerService } from '../commons/service-invoker.service'
import { Observable } from 'rxjs'
import { AppSettings } from '../constants/app-setting'
import { Api } from '../constants/api'
// import { retry } from 'rxjs/operators';
// import { request } from 'http';
import { UserInfoStorageService } from '../commons/user-info-storage.service'
import { LOG_ACTION, LOG_OBJECT } from '../constants/CONSTANTS'

@Injectable({
	providedIn: 'root',
})
export class AdministrativeFormalitiesService {
	constructor(private http: HttpClient, private serviceInvoker: ServiceInvokerService, private storeageService: UserInfoStorageService) {}

	getDropdownByType(request: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.GETLIST),
			logObject: encodeURIComponent(LOG_OBJECT.CA_FIELD),
		}
		return this.serviceInvoker.getwithHeaders(request, AppSettings.API_ADDRESS + Api.AdministrativeFormalitiesGetDropdown, headers)
	}
	getCAFieldDAM(request: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.GETLIST),
			logObject: encodeURIComponent(LOG_OBJECT.CA_FIELD),
		}
		return this.serviceInvoker.getwithHeaders(request, AppSettings.API_ADDRESS + Api.AdministrativeFormalitiesCAFieldDAM, headers)
	}
	getList(request: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.GETLIST),
			logObject: encodeURIComponent(LOG_OBJECT.CA_FIELD),
		}
		return this.serviceInvoker.getwithHeaders(request, AppSettings.API_ADDRESS + Api.AdministrativeFormalitiesGetList, headers)
	}

	getListHomePage(request: any): Observable<any> {
		return this.serviceInvoker.get(request, AppSettings.API_ADDRESS + Api.AdministrativeFormalitiesGetListHomePage)
	}

	getById(request: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.GETINFO),
			logObject: encodeURIComponent(LOG_OBJECT.CA_FIELD),
		}
		return this.serviceInvoker.getwithHeaders(request, AppSettings.API_ADDRESS + Api.AdministrativeFormalitiesGetById, headers)
	}
	getUnitReceive(): Observable<any> {
		return this.serviceInvoker.getUrl({}, 'https://nguoidungkhapi.azurewebsites.net/api/v1/phongbans/GetCoQuanTiepNhan')
	}
	getByIdView(request: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.GETINFO),
			logObject: encodeURIComponent(LOG_OBJECT.CA_FIELD),
		}
		return this.serviceInvoker.getwithHeaders(request, AppSettings.API_ADDRESS + Api.AdministrativeFormalitiesGetByIdView, headers)
	}

	insert(request: any): Observable<any> {
		let tempheaders = new HttpHeaders({
			ipAddress: this.storeageService.getIpAddress() && this.storeageService.getIpAddress() != 'null' ? this.storeageService.getIpAddress() : '',
			macAddress: '',
			logAction: encodeURIComponent(LOG_ACTION.INSERT),
			logObject: encodeURIComponent(LOG_OBJECT.DAM_ADMINISTRATOR),
		})
		const form = new FormData()
		form.append('Data', JSON.stringify(request.Data))
		form.append('Files', JSON.stringify(request.Files))
		form.append('LstXoaFile', JSON.stringify(request.LstXoaFile))
		form.append('LstXoaFileForm', JSON.stringify(request.LstXoaFileForm))
		form.append('LstCompositionProfile', JSON.stringify(request.LstCompositionProfile))
		form.append('LstCharges', JSON.stringify(request.LstCharges))
		form.append('LstImplementationProcess', JSON.stringify(request.LstImplementationProcess))
		form.append('LstDelete', JSON.stringify(request.LstDelete))

		if (request.Files) {
			request.Files.forEach((item) => {
				form.append('File', item)
			})
		}

		if (request.LstCompositionProfile) {
			for (let index = 0; index < request.LstCompositionProfile.length; index++) {
				const element = request.LstCompositionProfile[index]
				element.files.forEach((item) => {
					form.append('Profile' + index, item)
				})
			}
		}
		const httpPackage = {
			headers: tempheaders,
			reportProgress: true,
		}
		return this.http.post(AppSettings.API_ADDRESS + Api.AdministrativeFormalitiesInsert, form, httpPackage)
	}

	update(request: any): Observable<any> {
		let tempheaders = new HttpHeaders({
			ipAddress: this.storeageService.getIpAddress() && this.storeageService.getIpAddress() != 'null' ? this.storeageService.getIpAddress() : '',
			macAddress: '',
			logAction: encodeURIComponent(LOG_ACTION.UPDATE),
			logObject: encodeURIComponent(LOG_OBJECT.DAM_ADMINISTRATOR),
		})
		const form = new FormData()
		form.append('Data', JSON.stringify(request.Data))
		form.append('Files', JSON.stringify(request.Files))
		form.append('LstXoaFile', JSON.stringify(request.LstXoaFile))
		form.append('LstXoaFileForm', JSON.stringify(request.LstXoaFileForm))
		form.append('LstCompositionProfile', JSON.stringify(request.LstCompositionProfile))
		form.append('LstCharges', JSON.stringify(request.LstCharges))
		form.append('LstImplementationProcess', JSON.stringify(request.LstImplementationProcess))
		form.append('LstDelete', JSON.stringify(request.LstDelete))

		if (request.Files) {
			request.Files.forEach((item) => {
				form.append('File', item)
			})
		}

		if (request.LstCompositionProfile) {
			for (let index = 0; index < request.LstCompositionProfile.length; index++) {
				const element = request.LstCompositionProfile[index]
				element.files.forEach((item) => {
					form.append('Profile' + index, item)
				})
			}
		}
		const httpPackage = {
			headers: tempheaders,
			reportProgress: true,
		}
		return this.http.post(AppSettings.API_ADDRESS + Api.AdministrativeFormalitiesUpdate, form, httpPackage)
	}
	updateShow(request: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.UPDATESTATUS),
			logObject: encodeURIComponent(LOG_OBJECT.DAM_ADMINISTRATOR),
		}
		return this.serviceInvoker.postwithHeaders(request, AppSettings.API_ADDRESS + Api.AdministrativeFormalitiesUpdateShow, headers)
	}
	forward(request: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.FORWARD),
			logObject: encodeURIComponent(LOG_OBJECT.DAM_ADMINISTRATOR),
		}
		return this.serviceInvoker.getwithHeaders(request, AppSettings.API_ADDRESS + Api.AdministrativeForward, headers)
	}

	delete(request: any): Observable<any> {
		let headers = {
			logAction: encodeURIComponent(LOG_ACTION.DELETE),
			logObject: encodeURIComponent(LOG_OBJECT.DAM_ADMINISTRATOR),
		}
		return this.serviceInvoker.postwithHeaders(request, AppSettings.API_ADDRESS + Api.AdministrativeFormalitiesDelete, headers)
	}
}
