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
import { AgmCoreModule } from '@agm/core'

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
import { ListApproveDenyComponent } from './list-approve-deny/list-approve-deny.component'
import { ListApprovedComponent } from './list-approved/list-approved.component'
import { ListFakeImageComponent } from './list-fake-image/list-fake-image.component'
import { RemindComponent } from './remind/remind.component'

import { ListReactionaryWordComponent } from './list-reactionary-word/list-reactionary-word.component'
import { ListRecommendationCommentComponent } from './list-recommendation-comment/list-recommendation-comment.component'
import { CommentModule } from '../publish/comment-recommendation/comment.module'
import { InfomationExchangeModule } from './infomation-exchange/infomation-exchange.module'
import { ListForwardComponent } from './list-forward/list-forward.component'
import { ListProcessDenyMainComponent } from './list-process-deny-main/list-process-deny-main.component'
import { ListCombinationComponent } from './list-combination/list-combination.component'
import { ViewCombineRecommendationComponent } from './view-combine-recommendation/view-combine-recommendation.component'

@NgModule({
	imports: [
		CommonModule,
		RecommendationRoutingModule,
		ReactiveFormsModule,
		MatCheckboxModule,
		FormsModule,
		SharedModule,
		CommentModule,
		InfomationExchangeModule,
		BsDatepickerModule.forRoot(),
		TooltipModule,
		TreeModule,
		TreeTableModule,
		TableModule,
		NgSelectModule,
		ContextMenuModule,
		AgmCoreModule.forRoot({
			apiKey: 'AIzaSyBriVbWgmHEE8CGaEJM6V47Bem3VoYCi0Q',
			language: 'vi',
			libraries: ['places'],
		}),
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
		RemindComponent,
		ListReactionaryWordComponent,
		ListFakeImageComponent,
		ListRecommendationCommentComponent,
		ListForwardComponent, ListProcessDenyMainComponent, ListCombinationComponent, ViewCombineRecommendationComponent
	],
})
export class RecommendationModule { }
