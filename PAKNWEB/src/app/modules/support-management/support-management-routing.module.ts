import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'

import { RoleGuardService } from '../../guards/role-guard.service'
import { SupportManagementComponent } from './support-management.component'
import { SupportListComponent } from './support-list/support-list.component'
import { SupportListDocumentComponent } from './support-list-document/support-list-document.component'
import { SupportListVideoComponent } from './support-list-video/support-list-video.component'

const routes: Routes = [
	{
		path: '',
		component: SupportManagementComponent,
		children: [
			{
				path: 'tai-lieu',
				component: SupportListDocumentComponent,
				canActivate: [RoleGuardService],
				data: { role: 'G_I_3' }
			},
			{
				path: 'video',
				component: SupportListVideoComponent,
				canActivate: [RoleGuardService],
				data: { role: 'G_II_3' }
			},
		],
	},
]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class SupportManagementRoutingModule {}
