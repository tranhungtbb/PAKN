import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { ServiceInvokerService } from '../commons/service-invoker.service'
import { Observable, of } from 'rxjs'
import { AppSettings } from '../constants/app-setting'
import { Api } from '../constants/api'
import { UserInfoStorageService } from '../commons/user-info-storage.service'
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
		return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.IndivialDelete)
	}

	individualRegister(data: any): Observable<any> {
		let form = new FormData()

		for (let item in data) {
			form.append(item, data[item])
		}
		return this.serviceInvoker.post(form, AppSettings.API_ADDRESS + Api.InvididualRegister)
	}

	invididualUpdate(data: any): Observable<any> {
		let form = new FormData()
		console.log('data', data)
		for (let item in data) {
			form.append(item, data[item])
		}
		return this.serviceInvoker.post(form, AppSettings.API_ADDRESS + Api.InvididualUpdate)
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

	businessGetList(request: any): Observable<any> {
		return this.serviceInvoker.get(request, AppSettings.API_ADDRESS + Api.BusinessGetAllOnPage)
	}

	businessChangeStatus(data: any): Observable<any> {
		return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.BusinessChangeStatus)
	}

	businessDelete(data: any): Observable<any> {
		return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.BusinessDelete)
	}

	businessRegister(data: any): Observable<any> {
		let form = new FormData()
		console.log('data', data)
		for (let item in data) {
			form.append(item, data[item])
		}

		return this.serviceInvoker.post(form, AppSettings.API_ADDRESS + Api.BusinessRegister)
	}

	businessGetByID(request: any): Observable<any> {
		return this.serviceInvoker.get(request, AppSettings.API_ADDRESS + Api.BusinessGetByID)
	}

	businessUpdate(request: any): Observable<any> {
		return this.serviceInvoker.post(request, AppSettings.API_ADDRESS + Api.BusinessUpdate)
	}

	businessImportFile(data: any): Observable<any> {
		return this.serviceInvoker.postfile(data, AppSettings.API_ADDRESS + Api.ImportDataBusiness)
	}
}
