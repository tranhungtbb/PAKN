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

@Injectable({
	providedIn: 'root',
})
export class WeatherService {
	constructor(private http: HttpClient, private serviceInvoker: ServiceInvokerService, 
    private localStronageService: UserInfoStorageService) {
      this.headers.append("Access-Control-Allow-Origin", "*")
      this.headers.append("Access-Control-Allow-Methods", "GET, POST")
      this.headers.append("Access-Control-Allow-Headers", "X-PINGOTHER, Content-Type")
      this.headers.append("Content-Type","application/json; charset=utf-8");

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
          lat,long
        }
      });

  }
}
