import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ServiceInvokerService } from '../commons/service-invoker.service';
import { Observable } from 'rxjs';
import { AppSettings } from '../constants/app-setting';
import { Api } from '../constants/api';
// import { retry } from 'rxjs/operators';
// import { request } from 'http';
import { UserInfoStorageService } from '../commons/user-info-storage.service';

@Injectable({
  providedIn: 'root'
})

export class UserService {
  constructor(private http: HttpClient,
    private serviceInvoker: ServiceInvokerService,
    private localStronageService: UserInfoStorageService) { }

  getUserByRegion(data: any): Observable<any> {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.GET_USER_BY_REGION);
  }

  getUserByDepartment(data: any): Observable<any> {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.GET_USER_BY_DEPARTMENT);
  }

  UserUpdateStatus(data: any): Observable<any> {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.UserUpdateStatus);
  }

  createUser(data: any): Observable<any> {
    const uploadData = new FormData();
    data.User.UserId = this.localStronageService.getUserId();
    data.User.AccountId = this.localStronageService.getAccountId();
    data.User.IpAddress = this.localStronageService.getIpAddress();

    if (data.listFile != null) {
      for (var i = 0; i < data.listFile.length; i++) {
        uploadData.append('DinhKemUser' + i, data.listFile[i], data.listFile[i].name);
      }
    }
    uploadData.append('User', JSON.stringify(data.User));
    uploadData.append('PermissionCategories', JSON.stringify(data.PermissionCategories));
    uploadData.append('GroupUsers', JSON.stringify(data.GroupUsers));
    uploadData.append('UserDepartments', JSON.stringify(data.UserDepartments));
    return this.http.post(AppSettings.API_ADDRESS + Api.CREATE_USER, uploadData);
  }

  getCreateUserDatas(data: any): Observable<any> {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.GET_CREATE_USER_DATAS);
  }

  getUserByGroupId(groupUserId: number): Observable<any> {
    var Request = {
      GroupUserId: groupUserId
    }
    return this.serviceInvoker.post(Request, AppSettings.API_ADDRESS + Api.GETUSERBYGROUPID);
  }

  editUser(data: any): Observable<any> {

    const uploadData = new FormData();
    data.User.UserId = this.localStronageService.getUserId();
    data.User.AccountId = this.localStronageService.getAccountId();
    data.User.IpAddress = this.localStronageService.getIpAddress();

    if (data.listFile != null) {
      for (var i = 0; i < data.listFile.length; i++) {
        uploadData.append('DinhKemUser' + i, data.listFile[i], data.listFile[i].name);
      }
    }
    uploadData.append('User', JSON.stringify(data.User));
    uploadData.append('PermissionCategories', JSON.stringify(data.PermissionCategories));
    uploadData.append('GroupUsers', JSON.stringify(data.GroupUsers));
    uploadData.append('UserDepartments', JSON.stringify(data.UserDepartments));
    return this.http.post(AppSettings.API_ADDRESS + Api.Edit_User, uploadData);

    //return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.Edit_User);
  }

  updateInfo(data: any) {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.UPDATE_INFO);
  }

  updateUserLogin(data: any, file: any): Observable<any> {
    data.User.UserId = this.localStronageService.getUserId();
    data.User.IsSuperAdmin = this.localStronageService.getIsSuperAdmin();
    data.User.UnitId = this.localStronageService.getUnitId();
    data.User.DeparmentId = this.localStronageService.getDeparmentId();
    data.User.AccountId = this.localStronageService.getAccountId();
    data.User.IpAddress = this.localStronageService.getIpAddress();
    var formData = new FormData();
    formData.append('User', JSON.stringify(data.User));
    formData.append('Files', file);
    return this.http.post(AppSettings.API_ADDRESS + Api.UPDATE_USER_LOGIN, formData);
  }

  lockUser(data: any): Observable<any> {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.LOCK_USER);
  }

  delete(data: any): Observable<any> {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.DELETE_USER);
  }

  logOut(data: any): Observable<any> {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.logOut);
  }

  getUserByUnitId(unitId: number): Observable<any> {
    var request = {
      unitId: unitId
    }
    return this.serviceInvoker.get(request, AppSettings.API_ADDRESS + Api.GET_USER_BY_UNIT);
  }
  getAllUserByUnitForForward(unitId: number, accountId: number): Observable<any> {
    var request = {
      unitId: unitId,
      accountId: accountId
    }
    return this.serviceInvoker.get(request, AppSettings.API_ADDRESS + Api.GetAllUserByUnitForForward);
  }

  GetUserByUnitAndQuery(unitId: number, query: string): Observable<any> {
    var request = {
      unitId: unitId,
      query: query
    }
    return this.serviceInvoker.get(request, AppSettings.API_ADDRESS + Api.GET_USER_BY_UNIT_AND_QUERY);
  }

  getUserByUnitRegion(data: any): Observable<any> {

    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.GET_USER_BY_REGION_UNIT);
  }

  isNguoiDaiDien(data: any): Observable<any> {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.IsNguoiDaiDien);
  }


  LoadImage(data: any) {
    return this.http.get(AppSettings.API_ADDRESS + Api.downloadFileSupport, { responseType: 'blob', params: data }).pipe(file => { return file; }
    );
  }


  getUserById(request: any): Observable<any> {
    return this.serviceInvoker.get(request, AppSettings.API_ADDRESS + Api.GetUserById);
  }

  GetListChucVuAndListPhongBan(request: any): Observable<any> {
    return this.serviceInvoker.post(request, AppSettings.API_ADDRESS + Api.GetListChucVuAndListPhongBan);
  }
}


