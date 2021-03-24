import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { ServiceInvokerService } from '../commons/service-invoker.service'
import { Observable } from 'rxjs'
import { AppSettings } from '../constants/app-setting'
import { Api } from '../constants/api'
// import { GroupUserObject } from '../models/groupUserObject';
// import { RequestGroupUserModel } from '../models/requestGroupUserModel';

@Injectable({
	providedIn: 'root',
})
export class GroupUserService {
	constructor(private http: HttpClient, private serviceInvoker: ServiceInvokerService) {}
}
