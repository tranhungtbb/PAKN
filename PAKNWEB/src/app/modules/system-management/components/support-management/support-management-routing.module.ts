import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SupportManagementComponent } from './support-management.component';
import { SupportListComponent } from './support-list/support-list.component';
import { RoleGuardService } from '../../../../guards/role-guard.service';

const routes: Routes = [
  {
    path: '',
    component: SupportManagementComponent,
    children:
      [
        {
          path: 'support-list', component: SupportListComponent
        }
      ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SupportManagementRoutingModule { }
