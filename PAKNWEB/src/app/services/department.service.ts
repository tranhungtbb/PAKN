import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ServiceInvokerService } from '../commons/service-invoker.service';
import { Observable } from 'rxjs';
import { AppSettings } from '../constants/app-setting';
import { Api } from '../constants/api';
// import { DepartmentObject } from '../models/departmentObject';
import { UserInfoStorageService } from '../commons/user-info-storage.service';

@Injectable({
  providedIn: 'root'
})
export class DepartmentService {

  constructor(private http: HttpClient,
    private serviceInvoker: ServiceInvokerService,
    private storeageService: UserInfoStorageService) { }

  getTreeDepartment(): Observable<any> {
    let e: any = {};
    return this.serviceInvoker.post(e, AppSettings.API_ADDRESS + Api.GET_DEPARTMENT_TREE);
  }

  LoadImage(data: any) {
    return this.http.get(AppSettings.API_ADDRESS + Api.downloadFileSupport, { responseType: 'blob', params: data }).pipe(file => { return file; }
    );
  }
  getTreeUnit(): Observable<any> {
    let e: any = {};
    return this.serviceInvoker.post(e, AppSettings.API_ADDRESS + Api.GET_UNIT_TREE);
  }
  getUnitName(ma: string): Observable<any> {
    var Request = {
      Ma: ma
    }
    return this.serviceInvoker.get(Request, AppSettings.API_ADDRESS + Api.GET_UNIT_NAME);
  }

  getHeadDepartment(loaiPhong: number, unitId: number): Observable<any> {
    var Request = {
      loaiPhong: loaiPhong,
      unitId: unitId
    }
    return this.serviceInvoker.get(Request, AppSettings.API_ADDRESS + Api.GetHeadDepartment);
  }

  checkExistedCode(code: string): Observable<any> {
    var Request = {
      Code: code
    }
    return this.serviceInvoker.get(Request, AppSettings.API_ADDRESS + Api.CHECK_EXISTED_CODE);
  }

  createDepartment(data: any): Observable<any> {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.CREATEDERPARTMENT);
  }

  UpdateDepartment(data: any): Observable<any> {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.UPDATEDERPARTMENT);
  }

  DeleteDepartment(id: number): Observable<any> {
    var UserId = this.storeageService.getUserId();
    var Request = {
      id: id,
      Account: UserId
    }
    return this.serviceInvoker.get(Request, AppSettings.API_ADDRESS + Api.DELETEDEPARTMENT);
  }

  RemoveUserFromDepartment(request: any): Observable<any> {
    return this.serviceInvoker.post(request, AppSettings.API_ADDRESS + Api.RemoveUserFromDepartment);
  }

  DepartmentGetbyId(id: number): Observable<any> {
    var Request = {
      id: id
    }
    return this.serviceInvoker.get(Request, AppSettings.API_ADDRESS + Api.DEPARTMENTGETBYID);
  }

  getDepartmentaByUnitId(data: any): Observable<any> {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.GETDEPARTMENTABYUNITID);
  }

  getDepartmentByUnit(data: any): Observable<any> {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.GetDepartmentByUnit);
  }

  getDepartmentForProcess(data: any): Observable<any> {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.GetDepartmentForProcess);
  }

  DepartmentGetlstUserbyId(data: any): Observable<any> {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.GETUSERBYUNITID);
  }

  DepartmentGetUserToAdd(data: any): Observable<any> {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.GETUSERTOADD);
  }

  DepartmentTreeAddUser(id: number): Observable<any> {
    var Request = {
      id: id
    }
    return this.serviceInvoker.get(Request, AppSettings.API_ADDRESS + Api.GET_TREE_ADD_USER);
  }

  AddDepartmentUser(data: any): Observable<any> {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.ADDDEPARTMENTUSER);
  }

  GetDepartments(data: any): Observable<any> {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.GetDepartments);
  }

  DeleteDepartmentUser(data: any): Observable<any> {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.DELETEDEPARTMENTUSER);
  }

  GetUserbyDepId(id: number): Observable<any> {
    var Request = {
      id: id
    }
    return this.serviceInvoker.get(Request, AppSettings.API_ADDRESS + Api.GETUSERBYDEPID)
  }

  UpdateRepresentative(data: any): Observable<any> {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.UPDATEREPRESENTATIVE);
  }

  getDropDown(id: number): Observable<any> {
    var UnintId = this.storeageService.getUnitId();
    var IsNguoiDaiDien = this.storeageService.getIsSuperAdmin();
    if (IsNguoiDaiDien) {
      UnintId = 0;
    }
    var Request = {
      id: id,
      UnintId: UnintId
    }
    return this.serviceInvoker.get(Request, AppSettings.API_ADDRESS + Api.GETDROPDOWNDEPARTMENT)
  }

  GetListUserOverDepartmentId(request): Observable<any> {
    return this.serviceInvoker.get(request, AppSettings.API_ADDRESS + Api.GetListUserOverDepartmentId);
  }

  DepartmentAddUser(request): Observable<any> {
    return this.serviceInvoker.post(request, AppSettings.API_ADDRESS + Api.DepartmentAddUser);
  }
}
