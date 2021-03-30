import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { ServiceInvokerService } from '../commons/service-invoker.service'
import { Observable } from 'rxjs'
import { AppSettings } from '../constants/app-setting'
import { Api } from '../constants/api'
import { LOG_ACTION, LOG_OBJECT } from '../constants/CONSTANTS'

// import { retry } from 'rxjs/operators';
// import { request } from 'http';
import { UserInfoStorageService } from '../commons/user-info-storage.service'

@Injectable({
  providedIn: 'root',
})
export class PositionService {
  constructor(private http: HttpClient, private serviceInvoker: ServiceInvokerService, private localStronageService: UserInfoStorageService) { }


  CreatePosition(request: any): Observable<any> {
    let headers = {
      logAction: encodeURIComponent(LOG_ACTION.INSERT),
      logObject: encodeURIComponent(LOG_OBJECT.CA_FIELD),
    }
    return this.serviceInvoker.postwithHeaders(request, AppSettings.API_ADDRESS + Api.PositionInsert, headers)
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
  positionUpdateStatus(data: any): Observable<any> {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.PositionUpdate);
  }
}