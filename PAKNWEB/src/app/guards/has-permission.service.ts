import { Injectable } from '@angular/core'
import { UserInfoStorageService } from '../commons/user-info-storage.service';
@Injectable({
	providedIn: 'root',
})

export class HasPermission {
  constructor(private userService: UserInfoStorageService) {}

  checkPermission(lever, permissions) {
    let hasPermission = false;
    let isAdmin = this.userService.getIsMain();
    

    let isLogin = this.userService.getAccessToken();
    if (isLogin == '' || isLogin == null || isLogin == undefined) {
    } else {
      if (lever === "1") // Category
      {
        let currentUserPermissionCategories = this.userService.getPermissionCategories().split(',');
        for (let permission of permissions) {
          hasPermission = currentUserPermissionCategories.includes(permission);
          if (hasPermission) break;
        }
      } else if (lever === "2") {
        let currentUserFuntions = this.userService.getFunctions().split(',');
        for (let permission of permissions) {
          hasPermission = currentUserFuntions.includes(permission);
          if (hasPermission) break;
        }
      } else if (lever === "3") {
        let currentUserPermissions = this.userService.getPermissions().split(',');
        for (let permission of permissions) {
          hasPermission = currentUserPermissions.includes(permission);
          if (hasPermission) break;
        }
      }
    }


    return isAdmin || hasPermission;
  }

}