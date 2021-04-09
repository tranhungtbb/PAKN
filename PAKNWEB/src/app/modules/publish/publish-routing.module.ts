import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { PublishComponent } from './publish.component'
import { IndexComponent } from './index/index.component'
import { IntroduceComponent } from './introduce/introduce.component'
import { AdministrativeProceduresComponent } from './administrative-procedures/administrative-procedures.component'
import { NewsComponent } from './news/news.component'
import { ReflectionsRecommendationsComponent } from './reflections-recommendations/reflections-recommendations.component'
import { ViewReflectionsRecommendationComponent } from './view-reflections-recommendation/view-reflections-recommendation.component'
import { SupportComponent } from './support/support.component'
import { CreateRecommendationComponent } from './user-create-recommendation/user-create-recommendation.component'
import { using } from 'rxjs'
import { MyRecommendationComponent } from './my-recommendation/my-recommendation.component'
import { ViewRecommendationPersonalComponent } from './view-recommendation-personal/view-recommendation-personal.component'

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
				path: 'tin-tuc-su-kien',
				component: NewsComponent,
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
				path: 'phan-anh-kien-nghi',
				component: ReflectionsRecommendationsComponent,
			},

			{
				path: 'phan-anh-kien-nghi/:id',
				component: ViewReflectionsRecommendationComponent,
			},
			{
				path: 'ho-tro',
				component: SupportComponent,
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
export class PublishRoutingModule { }
