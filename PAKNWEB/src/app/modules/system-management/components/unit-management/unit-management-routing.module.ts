import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'

import { ListUnitComponent } from './list-unit/list-unit.component'

const routes: Routes = [{ path: '', component: ListUnitComponent }]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class UnitManagementRoutingModule {}
