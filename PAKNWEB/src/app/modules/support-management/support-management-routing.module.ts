import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { SupportManagementComponent } from './support-management.component'
import { SupportListComponent } from './support-list/support-list.component'

const routes: Routes = [
	{
		path: '',
		component: SupportManagementComponent,
		children: [
			{ path: '', component: SupportListComponent },
			{
				path: 'support-list',
				component: SupportListComponent,
			},
		],
	},
]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class SupportManagementRoutingModule {}
