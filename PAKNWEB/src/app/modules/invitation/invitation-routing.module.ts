import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { RoleGuardService } from '../../guards/role-guard.service'
import { InvitationComponent } from './invitation.component'
import { InvitationCreateOrUpdateComponent } from './invitation-create-or-update/invitation-create-or-update.component'
import { InvitationDetailComponent } from './invitation-detail/invitation-detail.component'

const routes: Routes = [
	{ 
		path: '',
		component: InvitationComponent
 	},
	{ 
		path: 'them-moi',
		component: InvitationCreateOrUpdateComponent,
		canActivate: [RoleGuardService],
		data: { role: 'B_IV_0'}
	},
	{ 
		path: 'cap-nhap/:id',
		component: InvitationCreateOrUpdateComponent,
		canActivate: [RoleGuardService],
		data: { role: 'B_IV_1'}
	},
	{
		path: 'chi-tiet/:id',
		component: InvitationDetailComponent,
		canActivate: [RoleGuardService],
		data: { role: 'B_IV_8'}
	},
]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class InvitationRoutingModule {}
