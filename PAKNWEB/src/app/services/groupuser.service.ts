import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ServiceInvokerService } from '../commons/service-invoker.service';
import { Observable } from 'rxjs';
import { AppSettings } from '../constants/app-setting';
import { Api } from '../constants/api';
// import { GroupUserObject } from '../models/groupUserObject';
// import { RequestGroupUserModel } from '../models/requestGroupUserModel';

@Injectable({
  providedIn: 'root'
})
export class GroupUserService {

  constructor(private http: HttpClient, private serviceInvoker: ServiceInvokerService) { }

  
  SearchAll(data: any): Observable<any> {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.GET_GROUPUSER_LIST);
  }

  checkExistedCode(code: string): Observable<any> {
    var Request = {
      Code: code
    }

    return this.serviceInvoker.get(Request, AppSettings.API_ADDRESS + Api.CHECK_EXISTED_CODE);
  }

  CreateGroupUser(data: any): Observable<any> {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.CREATEGROUPUSER);
  }

  UpdateGroupUser(data: any): Observable<any> {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.UPDATEGROUPUSER);
  }

  DeleteGroupUser(data: any): Observable<any> {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.DELETEGROUPUSER);
  }

  GetGroupUserById(id: number): Observable<any> {
    var Request = {
      id: id
    }
    return this.serviceInvoker.get(Request, AppSettings.API_ADDRESS + Api.GROUPUSERBYID);
  }

  addUserToGroupUser(data: any): Observable<any> {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.ADDUSERTOGROUP);
  }

  addListUserToGroupUser(data: any): Observable<any> {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.ADDLISTUSERTOGROUP);
  }
  

  getCreateGroupUserDatas(data: any): Observable<any> {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.GETCREATEGROUPUSERDATAS);
  }

  getListUserByUnitIdAndFilter(data: any): Observable<any> {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.GETLISTUSERBYUNITIDANDFILTER);
  }

  getListUserByUnitIdAndPage(data: any): Observable<any> {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.GETLISTGROUPUSERBYUNITIDANDPAGE);
  }
  
  delUserFromGroupUser(data: any): Observable<any> {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.DELUSERFROMGROUPUSER);
  }
  
  
}


