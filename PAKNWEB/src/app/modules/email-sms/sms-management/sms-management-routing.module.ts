import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { RoleGuardService } from '../../../guards/role-guard.service'
import { SMSManagementComponent } from './sms-management.component'
import { SMSCreateOrUpdateComponent } from './sms-management-create-or-update/sms-create-or-update.component'

const routes: Routes = [
	{ 
		path: '',
		component: SMSManagementComponent,
		canActivate: [RoleGuardService],
		data: { role: 'B_II_3'},
 	},
	{
		path: 'them-moi', 
		component: SMSCreateOrUpdateComponent,
		canActivate: [RoleGuardService],
		data: { role: 'B_II_8'},
	},
	{ 
		path: 'cap-nhap/:id',
		component: SMSCreateOrUpdateComponent,
		canActivate: [RoleGuardService],
		data: { role: 'B_II_7'},
 	},
]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class SMSManagementRoutingModule {}
