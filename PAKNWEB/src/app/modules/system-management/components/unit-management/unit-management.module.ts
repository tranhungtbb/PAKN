import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UnitManagementRoutingModule } from './unit-management-routing.module';
import { CreateOrUpdateUnitComponent } from './create-or-update-unit/create-or-update-unit.component';
import { ListUnitComponent } from './list-unit/list-unit.component';

@NgModule({
  declarations: [CreateOrUpdateUnitComponent, ListUnitComponent],
  imports: [
    CommonModule,
    UnitManagementRoutingModule
  ]
})
export class UnitManagementModule { }
