import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { FormsModule, ReactiveFormsModule } from '@angular/forms'
import { SharedModule } from 'src/app/shared/shared.module'
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker'
import { NgSelectModule } from '@ng-select/ng-select'
import { TableModule } from 'primeng/table'
import { MultiSelectModule } from 'primeng/multiselect'
import { TreeviewModule } from 'ngx-treeview'

import { InvitationRoutingModule } from './invitation-routing.module'
import { InvitationComponent } from './invitation.component'
import { InvitationCreateOrUpdateComponent } from './invitation-create-or-update/invitation-create-or-update.component'
import { InvitationDetailComponent } from './invitation-detail/invitation-detail.component'

@NgModule({
	declarations: [InvitationComponent, InvitationCreateOrUpdateComponent, InvitationDetailComponent],
	imports: [
		CommonModule,
		MultiSelectModule,
		TreeviewModule.forRoot(),
		InvitationRoutingModule,
		FormsModule,
		ReactiveFormsModule,
		SharedModule,
		TableModule,
		BsDatepickerModule.forRoot(),
		NgSelectModule,
	],
})
export class InvitationModule {}
