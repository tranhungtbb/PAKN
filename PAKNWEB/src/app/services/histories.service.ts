import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ServiceInvokerService } from '../commons/service-invoker.service';
import { Observable } from 'rxjs';
import { AppSettings } from '../constants/app-setting';
import { Api } from '../constants/api';

@Injectable({
  providedIn: 'root'
})
export class HistoriesService {

  constructor(private http: HttpClient, private serviceInvoker: ServiceInvokerService) { }

  getHistories(data: any): Observable<any> {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.GetHistories);
  }

  // Set Class Icon
  setClassIcon(data: any) {
    switch (data.loai) {
      case 1:
        return {
          'fa-plus': true,
          'bg-blue': true
        };
      case 2:
        return {
          'fa-pen': true,
          'bg-blue': true
        };
      case 3:
        return {
          'fa-paper-plane': true,
          'bg-green': true
        };
      case 4:
        return {
          'fa-check': true,
          'bg-blue': true
        };
      case 5:
        return {
          'fa-times-circle': true,
          'bg-red': true
        };
      case 6:
        return {
          'fa-check': true,
          'bg-blue': true
        };
      case 7:
        return {
          'fa-trash': true,
          'bg-red': true
        };
      case 8:
        return {
          'fa-cloud-upload-alt': true,
          'bg-red': true
        };
      case 9:
        return {
          'fa-times-square': true,
          'bg-red': true
        };
    }
  };
  // Set Class Text
  setClassText(data: any) {
    switch (data.loai) {
      case 1:
        return {
          'text-blue': true
        };
      case 2:
        return {
          'text-blue': true
        };
      case 3:
        return {
          'text-green': true
        };
      case 4:
        return {
          'text-blue': true
        };
      case 5:
        return {
          'text-red': true
        };
      case 6:
        return {
          'text-blue': true
        };
      case 7:
        return {
          'text-red': true
        };
      case 8:
        return {
          'text-green': true
        };
      case 9:
        return {
          'text-red': true
        };
    }
  };
}
