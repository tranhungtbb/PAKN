import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'

import { RoleGuardService } from '../../guards/role-guard.service'
import { SupportManagementComponent } from './support-management.component'
import { SupportListDocumentComponent } from './support-list-document/support-list-document.component'
import { SupportListVideoComponent } from './support-list-video/support-list-video.component'
import { SupportListPublicComponent } from './support-list-public/support-list-public.component'
import { SupportListPublicForAppComponent } from './support-list-public-for-app/support-list-public-for-app.component'

const routes: Routes = [
	{
		path: '',
		component: SupportManagementComponent,
		children: [
			{
				path: 'tai-lieu',
				component: SupportListDocumentComponent,
				canActivate: [RoleGuardService],
				data: { role: 'G_I_3' },
			},
			{
				path: 'video',
				component: SupportListVideoComponent,
				canActivate: [RoleGuardService],
				data: { role: 'G_II_3' },
			},
			{
				path: 'nguoi-dan-doanh-nghiep',
				component: SupportListPublicComponent,
				canActivate: [RoleGuardService],
				data: { role: 'G_II_3' },
			},

			{
				path: 'nguoi-dan-doanh-nghiep-app',
				component: SupportListPublicForAppComponent,
				canActivate: [RoleGuardService],
				data: { role: 'G_II_3' },
			},


		],
	},
]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class SupportManagementRoutingModule { }
