import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'

import { InvitationComponent } from './invitation.component'
import { InvitationCreateOrUpdateComponent } from './invitation-create-or-update/invitation-create-or-update.component'
import { InvitationDetailComponent } from './invitation-detail/invitation-detail.component'

const routes: Routes = [
	{ path: '', component: InvitationComponent },
	{ path: 'them-moi', component: InvitationCreateOrUpdateComponent },
	{ path: 'cap-nhap/:id', component: InvitationCreateOrUpdateComponent },
	{ path: 'chi-tiet/:id', component: InvitationDetailComponent },
]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class InvitationRoutingModule {}
