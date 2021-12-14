import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { EmailSmsComponent } from './email-sms.component'
import { SMSSentComponent } from './sms-sent/sms-sent.component'
import { EmailManagementComponent } from './email-management/email-management.component'
import { EmailCreateComponent } from './email-create/email-create.component'
import { RoleGuardService } from '../../guards/role-guard.service'
const routes: Routes = [
	{
		path: '',
		component: EmailSmsComponent,
		children: [
			{ path: 'sms', loadChildren: './sms-management/sms-management.module#SMSModule' },
			{
				path: 'sms-da-gui',
				component: SMSSentComponent,
				// canActivate: [RoleGuardService],
				// data: { role: 'B_II_3' },
			},
			{
				path: 'email',
				component: EmailManagementComponent,
				// canActivate: [RoleGuardService],
				// data: { role: 'B_I_2' },
			},
			{
				path: 'email/sent',
				component: EmailManagementComponent,
				// canActivate: [RoleGuardService],
				// data: { role: 'B_I_2' },
			},
			{
				path: 'email/create',
				component: EmailCreateComponent,
				// canActivate: [RoleGuardService],
				// data: { role: 'B_I_6' },
			},
			{
				path: 'email/edit/:id',
				component: EmailCreateComponent,
				// canActivate: [RoleGuardService],
				// data: { role: 'B_I_7' },
			},
		],
	},
]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class EmailSmsRoutingModule {}
