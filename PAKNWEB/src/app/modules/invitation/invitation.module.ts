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

import { NgxMatDatetimePickerModule, NgxMatTimepickerModule, NgxMatNativeDateModule } from '@angular-material-components/datetime-picker'

import { MatButtonModule } from '@angular/material/button'
import { MatCheckboxModule } from '@angular/material/checkbox'
import { MatDatepickerModule } from '@angular/material/datepicker'
import { MatInputModule } from '@angular/material/input'
import { MatRadioModule } from '@angular/material/radio'
import { MatSelectModule } from '@angular/material/select'

import { MomentDateModule } from '@angular/material-moment-adapter'
import { MAT_DATE_FORMATS } from '@angular/material/core'

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
		NgxMatDatetimePickerModule,
		NgxMatTimepickerModule,
		NgxMatNativeDateModule,
		MatButtonModule,
		MatCheckboxModule,
		MatDatepickerModule,
		MatInputModule,
		MatRadioModule,
		MatSelectModule,
		MomentDateModule,
	],
	// providers: [{ provide: MAT_DATE_FORMATS, useValue: MY_DATE_FORMATS }],
})
export class InvitationModule {}
