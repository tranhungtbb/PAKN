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
		],
	},
]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class PublishRoutingModule {}
