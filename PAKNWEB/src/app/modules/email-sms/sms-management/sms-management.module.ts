import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { FormsModule, ReactiveFormsModule } from '@angular/forms'
import { SharedModule } from 'src/app/shared/shared.module'
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker'
import { NgSelectModule } from '@ng-select/ng-select'
import { TableModule } from 'primeng/table'
import { TreeviewModule } from 'ngx-treeview'
import { MultiSelectModule } from 'primeng/multiselect'

import { SMSManagementRoutingModule } from './sms-management-routing.module'
import { SMSManagementComponent } from './sms-management.component'
import { SMSCreateOrUpdateComponent } from './sms-management-create-or-update/sms-create-or-update.component'

@NgModule({
	declarations: [SMSManagementComponent, SMSCreateOrUpdateComponent],
	imports: [
		CommonModule,
		SMSManagementRoutingModule,
		TreeviewModule.forRoot(),
		FormsModule,
		ReactiveFormsModule,
		SharedModule,
		TableModule,
		BsDatepickerModule.forRoot(),
		NgSelectModule,
		MultiSelectModule,
	],
})
export class SMSModule {}
