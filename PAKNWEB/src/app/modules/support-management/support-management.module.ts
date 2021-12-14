import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'

import { SupportManagementRoutingModule } from './support-management-routing.module'
import { SupportManagementComponent } from './support-management.component'
import { ReactiveFormsModule } from '@angular/forms'
import { FormsModule } from '@angular/forms'
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker'
import { MatCheckboxModule, MatRadioModule } from '@angular/material'
import { SharedModule } from '../../shared/shared.module'
import { NgSelectModule } from '@ng-select/ng-select'
import { CalendarModule } from 'primeng/calendar'
import { TreeModule } from 'primeng/tree'
import { SupportListDocumentComponent } from './support-list-document/support-list-document.component'
import { SupportListVideoComponent } from './support-list-video/support-list-video.component'

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
		TreeModule,
	],
	declarations: [SupportManagementComponent, SupportListDocumentComponent, SupportListVideoComponent],
})
export class SupportManagementModule {}
