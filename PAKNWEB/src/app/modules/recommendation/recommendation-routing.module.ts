import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { CreateRecommendationComponent } from './create-recommendation/create-recommendation.component'
import { ListGeneralComponent } from './list-general/list-general.component'
import { ListProcessDenyComponent } from './list-process-deny/list-process-deny.component'
import { ListProcessWaitComponent } from './list-process-wait/list-process-wait.component'
import { ListProcessingComponent } from './list-processing/list-processing.component'
import { ListReceiveApprovedComponent } from './list-receive-approved/list-receive-approved.component'
import { ListReceiveDenyComponent } from './list-receive-deny/list-receive-deny.component'
import { ListReceiveWaitComponent } from './list-receive-wait/list-receive-wait.component'
import { ListApproveWaitComponent } from './list-approve-wait/list-approve-wait.component'
import { ListRequestComponent } from './list-request/list-request.component'
import { RecommendationComponent } from './recommendation.component'
import { ViewRecommendationComponent } from './view-recommendation/view-recommendation.component'
import { ListApproveDenyComponent } from './list-approve-deny/list-approve-deny.component'
import { ListApprovedComponent } from './list-approved/list-approved.component'
import { DetailRecommendationComponent } from './detail-recommendation/detail-recommendation.component'
import { ReportViewerComponent } from '../report-view/report-viewcomponent'

const routes: Routes = [
	{
		path: '',
		component: RecommendationComponent,
		children: [
			{ path: 'them-moi/:id', component: CreateRecommendationComponent },
			{ path: 'danh-sach-tong-hop', component: ListGeneralComponent },
			{ path: 'cho-xu-ly', component: ListReceiveWaitComponent },
			{ path: 'tiep-nhan-xu-ly', component: ListReceiveApprovedComponent },
			{ path: 'tu-choi-tiep-nhan', component: ListReceiveDenyComponent },
			{ path: 'cho-giai-quyet', component: ListProcessWaitComponent },
			{ path: 'dang-giai-quyet', component: ListProcessingComponent },
			{ path: 'tu-choi-giai-quyet', component: ListProcessDenyComponent },
			{ path: 'chi-tiet/:id', component: ViewRecommendationComponent },
			{ path: 'cho-phe-duyet', component: ListApproveWaitComponent },
			{ path: 'tu-choi-phe-duyet', component: ListApproveDenyComponent },
			{ path: 'da-giai-quyet', component: ListApprovedComponent },
			{ path: 'danh-sach-knct', component: ListRequestComponent },
			{ path: 'chi-tiet-knct/:id', component: DetailRecommendationComponent },
			{ path: 'xuat-file/:module', component: ReportViewerComponent },
		],
	},
]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class RecommendationRoutingModule {}
