import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { PublishComponent } from './publish.component'
import { IndexComponent } from './index/index.component'
import { IntroduceComponent } from './introduce/introduce.component'
import { AdministrativeProceduresComponent } from './administrative-procedures/administrative-procedures.component'
import { NewsComponent } from './news/news.component'
import { ViewNewsComponent } from './view-news/view-news.component'
import { ReflectionsRecommendationsComponent } from './reflections-recommendations/reflections-recommendations.component'
import { ViewReflectionsRecommendationComponent } from './view-reflections-recommendation/view-reflections-recommendation.component'
import { SupportComponent } from './support/support.component'
import { CreateRecommendationComponent } from './user-create-recommendation/user-create-recommendation.component'
import { from, using } from 'rxjs'
import { MyRecommendationComponent } from './my-recommendation/my-recommendation.component'
import { ViewRecommendationPersonalComponent } from './view-recommendation-personal/view-recommendation-personal.component'
import { ViewAdministrativeProceduresComponent } from './view-administrative-procedures/view-administrative-procedures.component'
import  {ViewNotificationComponent} from './view-notification/view-notification.component'
import { RecommendationsSyncComponent } from './recommendations-sync/recommendations-sync.component'
import { ViewRecommendationsSyncComponent } from './view-recommendations-sync/view-recommendations-sync.component'

const routes: Routes = [
	{
		path: '',
		component: PublishComponent,
		children: [
			{
				path: '',
				component: IndexComponent,
			},
			{
				path: 'trang-chu',
				component: IndexComponent,
			},
			{
				path: 'gioi-thieu',
				component: IntroduceComponent,
			},
			{
				path: 'thu-tuc-hanh-chinh',
				component: AdministrativeProceduresComponent,
			},
			{
				path: 'chi-tiet/:id',
				component: ViewAdministrativeProceduresComponent,
			},
			{
				path: 'tin-tuc-su-kien',
				component: NewsComponent,
			},
			{
				path: 'tin-tuc-su-kien/xem-truoc/:id',
				component: ViewNewsComponent,
			},
			{
				path: 'tin-tuc-su-kien/:id',
				component: ViewNewsComponent,
			},
			{
				path: 'them-moi-kien-nghi',
				component: CreateRecommendationComponent,
			},
			{
				path: 'cap-nhat-kien-nghi/:id',
				component: CreateRecommendationComponent,
			},
			{
				path: 'chi-tiet-kien-nghi/:id',
				component: ViewRecommendationPersonalComponent,
			},
			{
				path: 'thong-bao/:id',
				component: ViewNotificationComponent,
			},
			{
				path: 'phan-anh-kien-nghi',
				component: ReflectionsRecommendationsComponent,
			},

			{
				path: 'phan-anh-kien-nghi/:id',
				component: ViewReflectionsRecommendationComponent,
			},
			{
				path: 'phan-anh-kien-nghi/sync/cong-ttdt-tinh-khanh-hoa',
				component: RecommendationsSyncComponent,
			},
			{
				path: 'phan-anh-kien-nghi/sync/cong-dv-hcc-tinh-khoanh-hoa',
				component: RecommendationsSyncComponent,
			},
			{
				path: 'phan-anh-kien-nghi/sync/he-thong-cu-tri-khanh-hoa',
				component: RecommendationsSyncComponent,
			},
			{
				path: 'phan-anh-kien-nghi/sync/he-thong-pakn-quoc-gia',
				component: RecommendationsSyncComponent,
			},
			{
				path: 'phan-anh-kien-nghi/sync/:type/:id',
				component: ViewRecommendationsSyncComponent,
			},
			// {
			// 	path: 'ho-tro/:type',
			// 	component: SupportComponent,
			// },
			{
				path: 'ho-tro',
				component: SupportComponent,
			},
			{
				path: 'phan-anh-kien-nghi-cua-toi/:id',
				component: MyRecommendationComponent,
			},
			{
				path: 'phan-anh-kien-nghi-cua-toi',
				component: MyRecommendationComponent,
			},

			{
				path: 'tai-khoan',
				loadChildren: './account/account.module#AccountModule', //() => import('./account/account.module').then((m) => m.AccountModule),
			},
		],
	},
]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class PublishRoutingModule {}
