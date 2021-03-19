import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ServiceInvokerService } from '../commons/service-invoker.service';
import { Observable } from 'rxjs';
import { AppSettings } from '../constants/app-setting';
import { Api } from '../constants/api';

@Injectable({
  providedIn: 'root'
})
export class DashBoardService {

  constructor(private http: HttpClient, private serviceInvoker: ServiceInvokerService) { }

  getDashBoard(): Observable<any> {
    var Request = {
    }
    return this.serviceInvoker.post(Request, AppSettings.API_ADDRESS + Api.GET_DASH_BOARD);
  }

  getListHocVien(): Observable<any> {
    var Request = {
    }
    return this.serviceInvoker.post(Request, AppSettings.API_ADDRESS + Api.GetListHocVien);
  }

  getNotifications(): Observable<any> {
    var Request = {

    }
    return this.serviceInvoker.post(Request, AppSettings.API_ADDRESS + Api.GET_NOTIFICATIONS);
  }

  updateIsViewRemind(data): Observable<any> {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.UpdateIsViewRemind);
  }

  dashBoardThongKeKienNghi(request): Observable<any> {
    return this.serviceInvoker.post(request, AppSettings.API_ADDRESS + Api.DashBoardThongKeKienNghi);
  }

  DashBoardGetList(request): Observable<any> {
    return this.serviceInvoker.get(request, AppSettings.API_ADDRESS + Api.DashBoardGetList);
  }

  dashBoardGetYear(request): Observable<any> {
    return this.serviceInvoker.get(request, AppSettings.API_ADDRESS + Api.DashBoardGetYear);
  }

  DashBoardCount(request): Observable<any> {
    return this.serviceInvoker.post(request, AppSettings.API_ADDRESS + Api.DashBoardCount);
  }
}


