import { Directive, OnInit, ElementRef, TemplateRef, ViewContainerRef, Input } from '@angular/core';
import { UserInfoStorageService } from '../commons/user-info-storage.service';
import { RouterStateSnapshot, Router, RouterState } from '@angular/router';

@Directive({
  selector: '[hasNotPermission]'
})
export class HasNotPermissionDirective implements OnInit {
  private currentUserPermissions;
  private currentUserFuntions;
  private currentUserPermissionCategories;

  private permissions;
  private mode: string = "";

  constructor(
    private element: ElementRef,
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef,
    private userService: UserInfoStorageService,
    router: Router
  ) {
    const state: RouterState = router.routerState;
    const snapshot: RouterStateSnapshot = state.snapshot;


    //let isLogin = this.userService.getAccessToken();
    //if (isLogin == '' || isLogin == null || isLogin == undefined) {
    //  var returnUrl = snapshot.url;
    //  this.userService.setReturnUrl(returnUrl);
    //}

  }

  ngOnInit() {

  }

  @Input()
  set hasNotPermission(val) {
    if (val != null) {
      this.mode = val[0];
      this.permissions = val.slice(1);
      this.updateView();
    }
  }

  private updateView() {
    if (this.checkPermission()) {
      this.viewContainer.createEmbeddedView(this.templateRef);
    } else {
      this.viewContainer.clear();
    }
  }

  private checkPermission() {
    let hasNotPermission = false;
    let isAdmin = this.userService.getIsMain();
    let isLogin = this.userService.getAccessToken();
    if (isLogin == '' || isLogin == null || isLogin == undefined) {
    } else {
      if (this.mode === "1") // Category
      {
        this.currentUserPermissionCategories = this.userService.getPermissionCategories().split(',');
        for (let permission of this.permissions) {
          hasNotPermission = this.currentUserPermissionCategories.includes(permission);
          if (hasNotPermission) break;
        }
      } else if (this.mode === "2") {
        this.currentUserFuntions = this.userService.getFunctions().split(',');
        for (let permission of this.permissions) {
          hasNotPermission = this.currentUserFuntions.includes(permission);
          if (hasNotPermission) break;
        }
      } else if (this.mode === "3") {
        this.currentUserPermissions = this.userService.getPermissions().split(',');
        for (let permission of this.permissions) {
          hasNotPermission = this.currentUserPermissions.includes(permission);
          if (hasNotPermission) break;
        }
      }
    }


    // return isAdmin || hasNotPermission;
    
    if(isAdmin){
      return false
    } else{
      return !hasNotPermission;
    }
  }
}
