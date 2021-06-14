import { NgModule, CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from '@angular/core'
import { CommonModule } from '@angular/common'

import { SharedModule } from '../../shared/shared.module'
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker'
import { TooltipModule } from 'primeng/tooltip'
import { TableModule } from 'primeng/table'
import { NgSelectModule } from '@ng-select/ng-select'

import { CallHistoryRoutingModule } from './call-history-routing.module'
import { CallHistoryListComponent } from './call-history-list/call-history-list.component'

@NgModule({
	declarations: [CallHistoryListComponent],
	imports: [CommonModule, CallHistoryRoutingModule, SharedModule, BsDatepickerModule, TooltipModule, TableModule, NgSelectModule],
	schemas: [CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA],
})
export class CallHistoryModule {}
