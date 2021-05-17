import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { EmailSmsComponent } from './email-sms.component'
import { SMSSentComponent } from './sms-sent/sms-sent.component'

const routes: Routes = [
	{
		path: '',
		component: EmailSmsComponent,
		children: [
			{ path: 'sms', loadChildren: './sms-management/sms-management.module#SMSModule' },
			{ path: 'sms-da-gui', component: SMSSentComponent },
		],
	},
]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class EmailSmsRoutingModule {}
