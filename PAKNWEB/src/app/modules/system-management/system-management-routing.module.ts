import { NgModule, Component } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SystemManagemenetComponent } from './system-managemenet.component';
import { OrgnizationComponent } from './components/orgnization/orgnization.component';
import { SystemConfigComponent } from './components/system-config/system-config.component';
import { SystemLogComponent } from './components/system-log/system-log.component';
import { RoleGuardService } from '../../guards/role-guard.service';

const routes: Routes = [
  {
    path: '',
    component: SystemManagemenetComponent,
    children:
      [
        {
          path: 'organization',
          component: OrgnizationComponent,
          //canActivate: [RoleGuardService],
          //data: { role: 'A_II_0' }
        },
        {
          path: 'user',
          loadChildren: './components/users-management/users-management.module#UsersManagementModule'
        },
        {
          path: 'group-user-management',
          loadChildren: './components/group-user-management/group-user-management.module#GroupUserManagementModule',
        },
        {
          path: 'support-management',
          loadChildren: './components/support-management/support-management.module#SupportManagementModule'
        },
        {
          path: 'system-config',
          component: SystemConfigComponent,
        },
        {
          path: 'system-log',
          component: SystemLogComponent,
        }
      ]
  },
];

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class SystemManagementRoutingModule { }
