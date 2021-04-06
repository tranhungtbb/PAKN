import { NgModule, Component } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UsersManagementComponent } from './users-management.component';
import { CreateUserComponent } from './create-user/create-user.component';
import { UsersInfoComponent } from './users-info.component';
import { EditUserComponent } from './edit-user/edit-user.component';
import { ViewUserComponent } from './view-user/view-user.component';
import { RoleGuardService } from '../../../../guards/role-guard.service';

const routes: Routes = [
  {
    path: '',
    component: UsersInfoComponent,
    children:
      [
        {
          path: '', component: UsersManagementComponent,
          canActivate: [RoleGuardService],
          data: { role: 'A_IV_0'}
        },
        {
          path: 'create-user/:id',
          component: CreateUserComponent,
          canActivate: [RoleGuardService],
          data: { role: 'A_IV_1' }
        },
        {
          path: 'edit-user/:id',
          component: EditUserComponent,
          canActivate: [RoleGuardService],
          data: { role: 'A_IV_2' }
        },
        {
          path: 'view-user/:id',
          component: ViewUserComponent,
          canActivate: [RoleGuardService],
          data: { role: 'A_IV_4' }
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
export class UsersManagementRoutingModule { }
