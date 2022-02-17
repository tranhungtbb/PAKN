import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ServiceInvokerService } from '../commons/service-invoker.service';
import { Api } from '../constants/api';
import { AppSettings } from '../constants/app-setting';
import { LOG_ACTION, LOG_OBJECT } from '../constants/CONSTANTS';

@Injectable({
  providedIn: 'root'
})
export class TemplateSmsService {

  constructor(private serviceInvoker: ServiceInvokerService) { }

  getList(data: any): Observable<any> {
    return this.serviceInvoker.get(data, AppSettings.API_ADDRESS + Api.TemplateSMSGetAll)
  }

  create(data: any): Observable<any> {
    let headers = {
      logAction: encodeURIComponent(LOG_ACTION.INSERT),
      logObject: encodeURIComponent(LOG_OBJECT.SY_TEAMPLALTE_SMS + ' ' + data.title),
    }
    return this.serviceInvoker.postwithHeaders(data, AppSettings.API_ADDRESS + Api.TemplateSMSInsert, headers)
  }
  update(data: any): Observable<any> {
    let headers = {
      logAction: encodeURIComponent(LOG_ACTION.UPDATE),
      logObject: encodeURIComponent(LOG_OBJECT.SY_TEAMPLALTE_SMS + ' ' + data.title),
    }
    return this.serviceInvoker.postwithHeaders(data, AppSettings.API_ADDRESS + Api.TemplateSMSUpdate, headers)
  }
  delete(data: any): Observable<any> {
    let headers = {
      logAction: encodeURIComponent(LOG_ACTION.DELETE),
      logObject: encodeURIComponent(LOG_OBJECT.SY_TEAMPLALTE_SMS + ' ' + data.title),
    }
    return this.serviceInvoker.postwithHeaders(data, AppSettings.API_ADDRESS + Api.TemplateSMSDelete, headers)
  }
}
