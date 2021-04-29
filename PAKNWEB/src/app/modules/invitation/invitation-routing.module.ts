import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'

import { InvitationComponent } from './invitation.component'
import { InvitationCreateOrUpdateComponent } from './invitation-create-or-update/invitation-create-or-update.component'

const routes: Routes = [
	{ path: '', component: InvitationComponent },
	{ path: 'them-moi', component: InvitationCreateOrUpdateComponent },
	{ path: 'cap-nhap/:id', component: InvitationCreateOrUpdateComponent },
]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class InvitationRoutingModule {}
