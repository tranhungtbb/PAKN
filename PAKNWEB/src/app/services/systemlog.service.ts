import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ServiceInvokerService } from '../commons/service-invoker.service';
import { Observable } from 'rxjs';
import { AppSettings } from '../constants/app-setting';
import { Api } from '../constants/api'; 

@Injectable({
  providedIn: 'root'
})

export class SystemLogService {
  constructor(private http: HttpClient, private serviceInvoker: ServiceInvokerService) { }

  getGridData(data:any): Observable<any> {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.SYSTEMLOGGETDATAGRID);
  }

  deleteLog(data: any): Observable<any> {

    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.SYSTEMLOGDELETEDATAGRID);
  }

  getTimeLineData(id: string, time: string): Observable<any> {
    var Request = {
      Uid: id,
      Time:time
    }
    return this.serviceInvoker.get(Request, AppSettings.API_ADDRESS + Api.SYSTEMLOGGETTIMELINE);
  }

  timlineusersearch(data:any): Observable<any> {

    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.SYSTEMLOGGETUSERTIMELINESEARCH);
  }

}
