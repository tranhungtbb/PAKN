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
import { ListGeneralComponent } from './list-general/list-general.component';
import { CreateRecommendationComponent } from './create-recommendation/create-recommendation.component';
import { ListReceiveWaitComponent } from './list-receive-wait/list-receive-wait.component';
import { ListReceiveDenyComponent } from './list-receive-deny/list-receive-deny.component';
import { ListReceiveApprovedComponent } from './list-receive-approved/list-receive-approved.component'

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
	declarations: [RecommendationComponent, ListGeneralComponent, CreateRecommendationComponent, ListReceiveWaitComponent, ListReceiveDenyComponent, ListReceiveApprovedComponent],
})
export class RecommendationModule {}
