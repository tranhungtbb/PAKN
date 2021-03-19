import { Injectable } from '@angular/core';
import { ServiceInvokerService } from '../commons/service-invoker.service';
import { AppSettings } from '../constants/app-setting';
import { Api } from '../constants/api';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DepartmentGroupService {

  constructor(private serviceInvoker: ServiceInvokerService) { }

  getList(request: any): Observable<any> {
    return this.serviceInvoker.post(request, AppSettings.API_ADDRESS + Api.DepartmentGroupGetList);
  }

  delete(request: any): Observable<any> {
    return this.serviceInvoker.post(request, AppSettings.API_ADDRESS + Api.DepartmentGroupDelete);
  }
}
