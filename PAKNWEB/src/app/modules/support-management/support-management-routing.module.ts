import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { SupportManagementComponent } from './support-management.component'
import { SupportListComponent } from './support-list/support-list.component'
import { SupportListDocumentComponent } from './support-list-document/support-list-document.component'
import { SupportListVideoComponent } from './support-list-video/support-list-video.component'

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
			{
				path: 'tai-lieu',
				component: SupportListDocumentComponent,
			},
			{
				path: 'video',
				component: SupportListVideoComponent,
			},
		],
	},
]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class SupportManagementRoutingModule {}
