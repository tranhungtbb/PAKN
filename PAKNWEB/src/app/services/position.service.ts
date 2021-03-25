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
export class PositionService {
  constructor(private http: HttpClient, private serviceInvoker: ServiceInvokerService, private localStronageService: UserInfoStorageService) { }

  CreatePosition(data: any): Observable<any> {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.PositionInsert);
  }
  UpdatePosition(data: any): Observable<any> {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.PositionUpdate);
  }
  positionGetList(request: any): Observable<any> {
    return this.serviceInvoker.get(request, AppSettings.API_ADDRESS + Api.PositionGetList)
  }
  positionDelete(request: any): Observable<any> {
    return this.serviceInvoker.post(request, AppSettings.API_ADDRESS + Api.PositionDelete)
  }
  positionGetById(request: any): Observable<any> {
    return this.serviceInvoker.get(request, AppSettings.API_ADDRESS + Api.PositionGetById)
  }
}