import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'

import { SupportManagementRoutingModule } from './support-management-routing.module'
import { SupportListComponent } from './support-list/support-list.component'
import { SupportManagementComponent } from './support-management.component'
import { ReactiveFormsModule } from '@angular/forms'
import { FormsModule } from '@angular/forms'
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker'
import { MatCheckboxModule, MatRadioModule } from '@angular/material'
import { SharedModule } from '../../shared/shared.module'
import { NgSelectModule } from '@ng-select/ng-select'
import { CalendarModule } from 'primeng/calendar'

@NgModule({
	imports: [
		CommonModule,
		SupportManagementRoutingModule,
		FormsModule,
		ReactiveFormsModule,
		BsDatepickerModule,
		NgSelectModule,
		MatRadioModule,
		MatCheckboxModule,
		SharedModule,
		CalendarModule,
	],
	declarations: [SupportListComponent, SupportManagementComponent],
})
export class SupportManagementModule {}
