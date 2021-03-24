import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'

import { TableModule } from 'primeng/table'

import { UnitManagementRoutingModule } from './unit-management-routing.module'
import { ListUnitComponent } from './list-unit/list-unit.component'

@NgModule({
	declarations: [ListUnitComponent],
	imports: [CommonModule, UnitManagementRoutingModule, TableModule],
})
export class UnitManagementModule {}
