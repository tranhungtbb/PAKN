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
import { RecommendationComponent } from './recommendation.component'
import { ViewRecommendationComponent } from './view-recommendation/view-recommendation.component'
import { ListApproveDenyComponent } from './list-approve-deny/list-approve-deny.component'
import { ListApprovedComponent } from './list-approved/list-approved.component'
import { ListReactionaryWordComponent } from './list-reactionary-word/list-reactionary-word.component'
import { ListFakeImageComponent } from './list-fake-image/list-fake-image.component'
import { RoleGuardService } from '../../guards/role-guard.service'
import { ListRecommendationCommentComponent } from './list-recommendation-comment/list-recommendation-comment.component'
import { ListForwardComponent } from './list-forward/list-forward.component'
import { ListProcessDenyMainComponent } from './list-process-deny-main/list-process-deny-main.component'
import { ListCombinationComponent } from './list-combination/list-combination.component'
import { ViewCombineRecommendationComponent } from './view-combine-recommendation/view-combine-recommendation.component'

const routes: Routes = [
	{
		path: '',
		component: RecommendationComponent,
		children: [
			{
				path: 'them-moi/:id',
				component: CreateRecommendationComponent,
				canActivate: [RoleGuardService],
				data: { role: 'E_I_0' },
			},
			{
				path: 'them-moi/:id/:typeObject',
				component: CreateRecommendationComponent,
				canActivate: [RoleGuardService],
				data: { role: 'E_I_0' },
			},
			{
				path: 'danh-sach-tong-hop',
				component: ListGeneralComponent,
				canActivate: [RoleGuardService],
				data: { role: 'E_I_3' },
			},
			{
				path: 'cho-xu-ly',
				component: ListReceiveWaitComponent,
				canActivate: [RoleGuardService],
				data: { role: 'E_I_3' },
			},
			{
				path: 'tiep-nhan-xu-ly',
				component: ListReceiveApprovedComponent,
				canActivate: [RoleGuardService],
				data: { role: 'E_I_3' },
			},
			{
				path: 'tu-choi-tiep-nhan',
				component: ListReceiveDenyComponent,
				canActivate: [RoleGuardService],
				data: { role: 'E_I_3' },
			},
			{
				path: 'cho-giai-quyet',
				component: ListProcessWaitComponent,
				canActivate: [RoleGuardService],
				data: { role: 'E_I_3' },
			},
			{
				path: 'da-chuyen',
				component: ListForwardComponent,
				canActivate: [RoleGuardService],
				data: { role: 'E_I_3' },
			},
			{
				path: 'dang-giai-quyet',
				component: ListProcessingComponent,
				canActivate: [RoleGuardService],
				data: { role: 'E_I_3' },
			},
			{
				path: 'tu-choi-giai-quyet', // đơn vị xử lý lại
				component: ListProcessDenyComponent,
				canActivate: [RoleGuardService],
				data: { role: 'E_I_3' },
			},
			{
				path: 'tu-choi-giai-quyet-trung-tam', // tu choi giai quyet trung tam
				component: ListProcessDenyMainComponent,
				canActivate: [RoleGuardService],
				data: { role: 'E_I_3' },
			},
			{
				path: 'chi-tiet/:id',
				component: ViewRecommendationComponent,
				canActivate: [RoleGuardService],
				data: { role: 'E_I_4' },
			},

			{
				path: 'chi-tiet-pakn-phoi-hop/:id',
				component: ViewCombineRecommendationComponent,
				canActivate: [RoleGuardService],
				data: { role: 'E_I_4' },
			},


			{
				path: 'cho-phe-duyet',
				component: ListApproveWaitComponent,
				canActivate: [RoleGuardService],
				data: { role: 'E_I_3' },
			},
			{
				path: 'tu-choi-phe-duyet',
				component: ListApproveDenyComponent,
				canActivate: [RoleGuardService],
				data: { role: 'E_I_3' },
			},
			{
				path: 'da-giai-quyet',
				component: ListApprovedComponent,
				canActivate: [RoleGuardService],
				data: { role: 'E_I_3' },
			},

			{
				path: 'tham-muu-don-vi',
				component: ListCombinationComponent,
				canActivate: [RoleGuardService],
				data: { role: 'E_I_3' },
			},
			{
				path: 'chua-anh-gia',
				component: ListFakeImageComponent,
				canActivate: [RoleGuardService],
				data: { role: 'E_I_3' },
			},
			{
				path: 'chua-tu-ngu-bi-cam',
				component: ListReactionaryWordComponent,
				canActivate: [RoleGuardService],
				data: { role: 'E_XII_0' },
			},
			{
				path: 'binh-luan-pakn',
				component: ListRecommendationCommentComponent,
				canActivate: [RoleGuardService],
				data: { role: 'E_XII_0' },
			},
		],
	},
]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class RecommendationRoutingModule { }
