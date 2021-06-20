import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { EmailSmsComponent } from './email-sms.component'
import { SMSSentComponent } from './sms-sent/sms-sent.component'
import { EmailManagementComponent } from './email-management/email-management.component'
import { EmailCreateComponent } from './email-create/email-create.component'

const routes: Routes = [
	{
		path: '',
		component: EmailSmsComponent,
		children: [
			{ path: 'sms', loadChildren: './sms-management/sms-management.module#SMSModule' },
			{ path: 'sms-da-gui', component: SMSSentComponent },
			{ path: 'email', component: EmailManagementComponent },
			{ path: 'email/sent', component: EmailManagementComponent },
			{ path: 'email/create', component: EmailCreateComponent },
			{ path: 'email/edit/:id', component: EmailCreateComponent },
		],
	},
]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class EmailSmsRoutingModule {}
