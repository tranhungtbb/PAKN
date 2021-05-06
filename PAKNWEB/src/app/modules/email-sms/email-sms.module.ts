import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { NgSelectModule } from '@ng-select/ng-select'
import { ReactiveFormsModule, FormsModule } from '@angular/forms'
import { SharedModule } from '../../shared/shared.module'
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker'
import { TooltipModule } from 'primeng/tooltip'
import { TreeModule } from 'primeng/tree'
import { TableModule } from 'primeng/table'
import { ContextMenuModule } from 'primeng/contextmenu'
import { MatCheckboxModule } from '@angular/material'
import { TreeTableModule } from 'primeng/treetable'

import { EmailSmsRoutingModule } from './email-sms-routing.module'
import { EmailSmsComponent } from './email-sms.component'
import { SMSCreateOrUpdateComponent } from './sms-management/sms-management-create-or-update/sms-create-or-update.component'

@NgModule({
	declarations: [EmailSmsComponent],
	imports: [
		CommonModule,
		EmailSmsRoutingModule,
		CommonModule,
		ReactiveFormsModule,
		MatCheckboxModule,
		FormsModule,
		SharedModule,
		BsDatepickerModule.forRoot(),
		TooltipModule,
		TreeModule,
		TreeTableModule,
		TableModule,
		NgSelectModule,
		ContextMenuModule,
	],
})
export class EmailSmsModule {}
