import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router, ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { UserInfoStorageService } from '../commons/user-info-storage.service';

@Injectable({
  providedIn: 'root'
})
export class DataGuardService implements CanActivate {

  constructor(private userService: UserInfoStorageService, private _router: Router, private route: ActivatedRoute) { }

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | boolean {


    var check = this.userService.getAccessToken();
    var returnUrl;
    if (check == '' || check == null || check == undefined) {
      returnUrl = state.url;
      this.userService.setReturnUrl(returnUrl);
    } else { 
      return true;
    }
   
  }

}
