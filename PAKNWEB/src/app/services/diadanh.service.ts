import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { ServiceInvokerService } from '../commons/service-invoker.service'
import { Observable } from 'rxjs'
import { AppSettings } from '../constants/app-setting'
import { Api } from '../constants/api'
// import { retry } from 'rxjs/operators';
// import { request } from 'http';
import { UserInfoStorageService } from '../commons/user-info-storage.service'

@Injectable({
	providedIn: 'root',
})
export class DiadanhService {
	constructor(private http: HttpClient, private serviceInvoker: ServiceInvokerService, private localStronageService: UserInfoStorageService) {}

	getAllProvince() {
		let url = AppSettings.API_ADDRESS + Api.ProvinceGetAll
		return this.serviceInvoker.get({}, url)
	}
	getAllDistrict(provinceId: any) {
		let url = AppSettings.API_ADDRESS + Api.DistrictGetAll
		return this.serviceInvoker.get({ provinceId }, url)
	}
	getAllVillage(provinceId: any, districtId: any) {
		let url = AppSettings.API_ADDRESS + Api.VillageGetAll
		return this.serviceInvoker.get({ provinceId, districtId }, url)
	}
}
