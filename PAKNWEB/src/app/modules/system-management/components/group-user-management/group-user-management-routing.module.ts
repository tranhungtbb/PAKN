import { NgModule, Component } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { GroupUserInfoComponent } from './group-user-info/group-user-info.component';
import { GroupUserManagementComponent } from './group-user-management.component';
import { CreateGroupUserComponent } from './create-group-user/create-group-user.component';
import { UpdateGroupUserComponent } from './update-group-user/update-group-user.component';
import { ViewGroupUserComponent } from './view-group-user/view-group-user.component';
import { RoleGuardService } from '../../../../guards/role-guard.service';


const routes: Routes = [
  {
    path: '',
    component: GroupUserInfoComponent,
    children:
      [
        {
          path: '', component: GroupUserManagementComponent,
          canActivate: [RoleGuardService],
          data: { role: 'A_III_0' } },
        {
          path: 'create-group-user/:id',
          component: CreateGroupUserComponent,
          canActivate: [RoleGuardService],
          data: { role: 'A_III_1' }
        },
        {
          path: 'update-group-user/:id',
          component: UpdateGroupUserComponent,
          canActivate: [RoleGuardService],
          data: { role: 'A_III_2' }
        },
        {
          path: 'view-group-user/:id',
          component: ViewGroupUserComponent,
          canActivate: [RoleGuardService],
          data: { role: 'A_III_3' }
        },
      ]
  }
];

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class GroupUserManagementRoutingModule { }
