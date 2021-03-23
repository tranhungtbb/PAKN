import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { BusinessComponent } from './business.component'
import { DashboardComponent } from './dash-board/dash-board.component'

const routes: Routes = [
	{
		path: '',
		component: BusinessComponent,
		children: [
			{ path: '', redirectTo: 'ban-lam-viec' },
			{ path: 'ban-lam-viec', component: DashboardComponent },
			{ path: 'he-thong', loadChildren: './system-management/system-management.module#SystemManagementModule' },
			{ path: 'danh-muc', loadChildren: './catalog-management/catalog-management.module#CatalogManagementModule' },
		],
	},
]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class BusinessRoutingModule {}
