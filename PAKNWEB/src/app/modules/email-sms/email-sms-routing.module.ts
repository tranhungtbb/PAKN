import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { EmailSmsComponent } from './email-sms.component'
// import { SMSManagementComponent } from './sms-management/sms-management.module#SMSModule'

const routes: Routes = [
	{
		path: '',
		component: EmailSmsComponent,
		children: [{ path: 'sms', loadChildren: './sms-management/sms-management.module#SMSModule' }],
	},
]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class EmailSmsRoutingModule {}
