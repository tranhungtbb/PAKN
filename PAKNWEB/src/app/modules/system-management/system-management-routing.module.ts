import { NgModule, Component } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { SystemManagemenetComponent } from './system-managemenet.component'
import { OrgnizationComponent } from './components/orgnization/orgnization.component'
import { SystemConfigComponent } from './components/system-config/system-config.component'
import { SystemLogComponent } from './components/system-log/system-log.component'
import { RoleGuardService } from '../../guards/role-guard.service'

const routes: Routes = [
	{
		path: '',
		component: SystemManagemenetComponent,
		children: [
			{
				path: 'co-cau-to-chuc',
				component: OrgnizationComponent,
			},
			{
				path: 'nguoi-dung',
				loadChildren: './components/users-management/users-management.module#UsersManagementModule',
			},
			{
				path: 'vai-tro',
				component: OrgnizationComponent,
				//canActivate: [RoleGuardService],
				//data: { role: 'A_II_0' }
			},
			{
				path: 'support-management',
				loadChildren: './components/support-management/support-management.module#SupportManagementModule',
			},
		],
	},
]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class SystemManagementRoutingModule {}
