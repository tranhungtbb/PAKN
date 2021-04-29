import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { FormsModule, ReactiveFormsModule } from '@angular/forms'
import { SharedModule } from 'src/app/shared/shared.module'
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker'
import { NgSelectModule } from '@ng-select/ng-select'
import { TableModule } from 'primeng/table'

import { InvitationRoutingModule } from './invitation-routing.module'
import { InvitationComponent } from './invitation.component'
import { InvitationCreateOrUpdateComponent } from './invitation-create-or-update/invitation-create-or-update.component'

@NgModule({
	declarations: [InvitationComponent, InvitationCreateOrUpdateComponent],
	imports: [CommonModule, InvitationRoutingModule, FormsModule, ReactiveFormsModule, SharedModule, TableModule, BsDatepickerModule.forRoot(), NgSelectModule],
})
export class InvitationModule {}
