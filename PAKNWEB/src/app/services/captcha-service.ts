import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Api } from '../constants/api';
import { AppSettings } from '../constants/app-setting';

@Injectable({
  providedIn: 'root'
})
export class CaptchaService {

  constructor(private http: HttpClient) { }


  send(data): Observable<any> {
    //const options = {
    //  headers: new HttpHeaders({ 'Content-Type': 'application/json; charset=utf-8' })
    //};

    return this.http.get(AppSettings.API_ADDRESS + Api.ValidateCaptcha, {
      params: {
        CaptchaCode: data.CaptchaCode,
        MillisecondsCurrent: data.MillisecondsCurrent,
      }
    });
  }

  getImage(): Observable<any> {
    var data = {};

    return this.http.get(AppSettings.API_ADDRESS + Api.getImageCaptcha, data);
  }

}
