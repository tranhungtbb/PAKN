import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'

import { SMSManagementComponent } from './sms-management.component'
import { SMSCreateOrUpdateComponent } from './sms-management-create-or-update/sms-create-or-update.component'

const routes: Routes = [
	{ path: '', component: SMSManagementComponent },
	{ path: 'them-moi', component: SMSCreateOrUpdateComponent },
	{ path: 'cap-nhap/:id', component: SMSCreateOrUpdateComponent },
]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class SMSManagementRoutingModule {}
