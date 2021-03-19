import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { UserInfoStorageService } from '../commons/user-info-storage.service';

@Injectable({
  providedIn: 'root'
})
export class RoleGuardService implements CanActivate  {

  constructor(private userService: UserInfoStorageService, private _router: Router) { }

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | boolean {


    var check = this.userService.getAccessToken();
    var returnUrl;
    if (check == '' || check == null || check == undefined) {
      returnUrl = state.url;
      this.userService.setReturnUrl(returnUrl);
    } else {
      let currentUserPermissions = this.userService.getPermissions().split(',');
      let isAdmin = this.userService.getIsSuperAdmin();
      if (isAdmin || currentUserPermissions.includes(next.data.role)) {
        return true;
      } else {
        this._router.navigate(['/forbidden']);
      }
    }
    return false;
  }

}
