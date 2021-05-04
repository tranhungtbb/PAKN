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

import { RecommendationRoutingModule } from './recommendation-routing.module'
import { RecommendationComponent } from './recommendation.component'
import { ListGeneralComponent } from './list-general/list-general.component'
import { CreateRecommendationComponent } from './create-recommendation/create-recommendation.component'
import { ListReceiveWaitComponent } from './list-receive-wait/list-receive-wait.component'
import { ListReceiveDenyComponent } from './list-receive-deny/list-receive-deny.component'
import { ListReceiveApprovedComponent } from './list-receive-approved/list-receive-approved.component'
import { ListProcessWaitComponent } from './list-process-wait/list-process-wait.component'
import { ListProcessDenyComponent } from './list-process-deny/list-process-deny.component'
import { ListProcessingComponent } from './list-processing/list-processing.component'
import { ViewRecommendationComponent } from './view-recommendation/view-recommendation.component'
import { ListApproveWaitComponent } from './list-approve-wait/list-approve-wait.component'
import { ListRequestComponent } from './list-request/list-request.component'
import { ListApproveDenyComponent } from './list-approve-deny/list-approve-deny.component'
import { ListApprovedComponent } from './list-approved/list-approved.component'

import { RemindComponent } from './remind/remind.component'

import { DetailRecommendationComponent } from './detail-recommendation/detail-recommendation.component'
// import { ChangePipe } from 'src/app/pipes/unit-filter.pipe'
@NgModule({
	imports: [
		CommonModule,
		RecommendationRoutingModule,
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
	declarations: [
		RecommendationComponent,
		ListGeneralComponent,
		CreateRecommendationComponent,
		ListReceiveWaitComponent,
		ListReceiveDenyComponent,
		ListReceiveApprovedComponent,
		ListProcessWaitComponent,
		ListProcessDenyComponent,
		ListProcessingComponent,
		ViewRecommendationComponent,
		ListApproveWaitComponent,
		ListApproveDenyComponent,
		ListApprovedComponent,
		ListRequestComponent,
		RemindComponent,
		// ChangePipe,
		DetailRecommendationComponent,
	],
})
export class RecommendationModule {}
