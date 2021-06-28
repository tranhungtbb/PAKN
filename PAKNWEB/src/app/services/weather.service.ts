// openweathermap user login
// duonglt2@thanglonginc.com / abcd@12345

import { Injectable } from '@angular/core'
import { HttpClient,HttpHeaders } from '@angular/common/http'
import { ServiceInvokerService } from '../commons/service-invoker.service'
import { Observable } from 'rxjs'
import { AppSettings } from '../constants/app-setting'
import { Api } from '../constants/api'
// import { retry } from 'rxjs/operators';
// import { request } from 'http';
import { UserInfoStorageService } from '../commons/user-info-storage.service'
import { LOG_ACTION, LOG_OBJECT } from '../constants/CONSTANTS'


declare var $ :any

@Injectable({
	providedIn: 'root',
})
export class WeatherService {
	constructor(private http: HttpClient, private serviceInvoker: ServiceInvokerService, 
    private localStronageService: UserInfoStorageService) {
      this.headers.append('Access-Control-Allow-Origin','*')
    }
    headers = new HttpHeaders();

    appid:string = '3203582b0e1e98b17f97b639bcef2350'

	getCurrent(loc:string = 'Nha trang,vn',appkey:string=null):Observable<any>{
    if(appkey) this.appid = appkey;

    return this.http.get(AppSettings.weatherApi,
      {
        headers:this.headers,
        params:{
          appid:this.appid,
          q:loc
        }
      });
  }
  getByGeographic (lat:any,long:any,appkey:string=null):Observable<any>{
    if(appkey) this.appid = appkey;
    if(!lat || !long) return this.getCurrent();

    return this.http.get(AppSettings.weatherApi,
      {
        headers:this.headers,
        params:{
          appid:this.appid,
          lat,lon:long
        }
      });
  }

  getByGeographic$(lat:any,long:any,appid:string=null):Observable<any>{
    return this.serviceInvoker.get({
      lat,lon:long
    },AppSettings.API_ADDRESS+Api.Weather);
  }
  
}
