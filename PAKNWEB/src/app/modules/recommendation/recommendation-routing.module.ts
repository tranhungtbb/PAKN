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
import { RecommendationComponent } from './recommendation.component'

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
		],
	},
]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class RecommendationRoutingModule {}
