import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { ServiceInvokerService } from '../commons/service-invoker.service'
import { Observable } from 'rxjs'
import { AppSettings } from '../constants/app-setting'
import { Api } from '../constants/api'

@Injectable({
	providedIn: 'root',
})
export class RegionService {
	constructor(private http: HttpClient, private serviceInvoker: ServiceInvokerService) {}

	getRegionFirstNode(): Observable<any> {
		return this.serviceInvoker.get(null, AppSettings.API_ADDRESS + Api.GETREGIONFIRSTNODE)
	}

	getRegionChildNote(data): Observable<any> {
		return this.serviceInvoker.get(data, AppSettings.API_ADDRESS + Api.GETREGIONCHILDNODE)
	}

	createRegion(data): Observable<any> {
		return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.REGIONCREATE)
	}

	updateRegion(data): Observable<any> {
		return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.REGIONUPDATE)
	}

	deleteRegion(data): Observable<any> {
		return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.REGIONDELETE)
	}

	getRegionbyId(data): Observable<any> {
		return this.serviceInvoker.get(data, AppSettings.API_ADDRESS + Api.REGIONGETBYID)
	}

	getDropDown(): Observable<any> {
		return this.serviceInvoker.get({}, AppSettings.API_ADDRESS + Api.REGIONGETDropDown)
	}

	getDropDownQuanHuyen(data): Observable<any> {
		return this.serviceInvoker.get(data, AppSettings.API_ADDRESS + Api.RegionGetDropDownQuanHuyen)
	}

	getDropDownQuanHuyen_Group(): Observable<any> {
		return this.serviceInvoker.get(null, AppSettings.API_ADDRESS + Api.RegionGetDropDownQuanHuyen_Group)
	}

	getDropDownPhuongXa(data: any): Observable<any> {
		return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.RegionGetDropDownPhuongXa)
	}

	getDropDownQuanHuyen_All(): Observable<any> {
		return this.serviceInvoker.get(null, AppSettings.API_ADDRESS + Api.RegionGetDropDownQuanHuyen_All)
	}
	getDropDownTinhSeach(): Observable<any> {
		return this.serviceInvoker.get(null, AppSettings.API_ADDRESS + Api.RegionGetDropDownTinh_Search)
	}
}
