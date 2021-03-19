import { Directive, OnInit, ElementRef, TemplateRef, ViewContainerRef, Input } from '@angular/core';
import { UserInfoStorageService } from '../commons/user-info-storage.service';
import { RouterStateSnapshot, Router, RouterState } from '@angular/router';

@Directive({
  selector: '[hasPermission]'
})
export class HasPermissionDirective implements OnInit {
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
  set hasPermission(val) {
    this.mode = val[0];
    this.permissions = val.slice(1);
    this.updateView();
  }

  private updateView() {
    var a = this.checkPermission();
    if (this.checkPermission()) {
      this.viewContainer.createEmbeddedView(this.templateRef);
    } else {
      this.viewContainer.clear();
    }
  }

  private checkPermission() {
    let hasPermission = false;
    let isAdmin = this.userService.getIsSuperAdmin();

    let isLogin = this.userService.getAccessToken();
    if (isLogin == '' || isLogin == null || isLogin == undefined) {
    } else {
      if (this.mode === "1") // Category
      {
        this.currentUserPermissionCategories = this.userService.getPermissionCategories().split(',');
        for (let permission of this.permissions) {
          hasPermission = this.currentUserPermissionCategories.includes(permission);
          if (hasPermission) break;
        }
      } else if (this.mode === "2") {
        this.currentUserFuntions = this.userService.getFunctions().split(',');
        for (let permission of this.permissions) {
          hasPermission = this.currentUserFuntions.includes(permission);
          if (hasPermission) break;
        }
      } else if (this.mode === "3") {
        this.currentUserPermissions = this.userService.getPermissions().split(',');
        for (let permission of this.permissions) {
          hasPermission = this.currentUserPermissions.includes(permission);
          if (hasPermission) break;
        }
      }
    }


    return isAdmin || hasPermission;
  }
}
