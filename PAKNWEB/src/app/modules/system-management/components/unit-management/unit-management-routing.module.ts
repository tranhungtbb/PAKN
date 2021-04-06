import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {CreateOrUpdateUnitComponent} from './create-or-update-unit/create-or-update-unit.component';

const routes: Routes = [
  {
    path: 'create',
    component: CreateOrUpdateUnitComponent, 
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UnitManagementRoutingModule { }
