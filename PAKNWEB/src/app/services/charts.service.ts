import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ServiceInvokerService } from '../commons/service-invoker.service';
import { Observable } from 'rxjs';
import { AppSettings } from '../constants/app-setting';
import { Api } from '../constants/api'; 
import { UserInfoStorageService } from '../commons/user-info-storage.service';

@Injectable({
  providedIn: 'root'
})
export class ChartsService {

  constructor(private http: HttpClient, private serviceInvoker: ServiceInvokerService, private storeageService: UserInfoStorageService) { }
  
}
