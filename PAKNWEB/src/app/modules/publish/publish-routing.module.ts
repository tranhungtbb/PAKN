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
import { ViewNotificationComponent } from './view-notification/view-notification.component'
import { DetailRecommendationKnctComponent } from './recommendations-sync/recommendation-knct-detail/recommendation-knct-detail.component'
import { ListRecommendationKnct } from 'src/app/modules/publish/recommendations-sync/recommendation-knct/recommendation-knct.component'
import { RecommendationsDvhhcComponent } from 'src/app/modules/publish/recommendations-sync/recommendations-dvhhc/recommendations-dvhhc.component'
import { DetailRecommendationDvhhcComponent } from 'src/app/modules/publish/recommendations-sync/recommendations-dvhhc-detail/recommendations-dvhhc-detail.component'
import { RecommendationsCttdtComponent } from './recommendations-sync/recommendations-cttdt/recommendations-cttdt.component'
import { DetailRecommendationCttdtComponent } from './recommendations-sync/recommendations-cttdt-detail/recommendations-cttdt-detail.component'
import { RecommendationsPaknCPComponent } from './recommendations-sync/recommendation-pakn-cp/recommendation-pakn-cp.component'
import { DetailRecommendationPaknCPComponent } from './recommendations-sync/recommendation-pakn-cp-detail/recommendation-pakn-cp-detail.component'
import { StatisticsComponent } from './statistics/statistics.component'
import { Index2Component } from './index2/index2.component'
import { ReceiveDenyRecommendationsComponent } from './receive-deny-recommendations/receive-deny-recommendations.component'
import { InfomationPublicComponent } from './infomation-public/infomation-public.component'
import { UnitDissatisfactionRateComponent } from './unit-dissatisfaction-rate/unit-dissatisfaction-rate.component'
import { LateProcessingUnitComponent } from './late-processing-unit/late-processing-unit.component'

const routes: Routes = [
	{
		path: '',
		component: PublishComponent,
		children: [
			{
				path: '',
				component: Index2Component,
			},
			{
				path: 'trang-chu',
				component: Index2Component,
			},
			{
				path: 'xem-truoc/trang-chu',
				component: Index2Component,
			},
			{
				path: 'gioi-thieu',
				component: IntroduceComponent,
			},
			{
				path: 'xem-truoc/gioi-thieu',
				component: IntroduceComponent,
			},
			{
				path: 'thu-tuc-hanh-chinh',
				component: AdministrativeProceduresComponent,
			},
			{
				path: 'thu-tuc-hanh-chinh/:id',
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
				path: 'phan-anh-kien-nghi/da-tra-loi',
				component: ReflectionsRecommendationsComponent,
			},
			{
				path: 'danh-sach-phan-anh-kien-nghi',
				component: ReflectionsRecommendationsComponent,
			},
			{
				path: 'danh-sach-phan-anh-kien-nghi/:field',
				component: ReflectionsRecommendationsComponent,
			},
			{
				path: 'danh-sach-phan-anh-kien-nghi/:field/:keysearch',
				component: ReflectionsRecommendationsComponent,
			},
			{
				path: 'phan-anh-kien-nghi-khong-tiep-nhan',
				component: ReceiveDenyRecommendationsComponent,
			},
			{
				path: 'phan-anh-kien-nghi/:id',
				component: ViewReflectionsRecommendationComponent,
			},
			{
				path: 'phan-anh-kien-nghi/sync/he-thong-cu-tri-khanh-hoa',
				component: ListRecommendationKnct,
			},
			{
				path: 'phan-anh-kien-nghi/sync/he-thong-cu-tri-khanh-hoa/:id',
				component: DetailRecommendationKnctComponent,
			},
			{
				path: 'phan-anh-kien-nghi/sync/cong-dv-hcc-tinh-khoanh-hoa',
				component: RecommendationsDvhhcComponent,
			},
			{
				path: 'phan-anh-kien-nghi/sync/cong-dv-hcc-tinh-khoanh-hoa/:id',
				component: DetailRecommendationDvhhcComponent,
			},
			{
				path: 'phan-anh-kien-nghi/sync/cong-ttdt-tinh-khanh-hoa',
				component: RecommendationsCttdtComponent,
			},
			{
				path: 'phan-anh-kien-nghi/sync/cong-ttdt-tinh-khanh-hoa/:id',
				component: DetailRecommendationCttdtComponent,
			},
			{
				path: 'phan-anh-kien-nghi/sync/he-thong-pakn-quoc-gia',
				component: RecommendationsPaknCPComponent,
			},
			{
				path: 'phan-anh-kien-nghi/sync/he-thong-pakn-quoc-gia/:id',
				component: DetailRecommendationPaknCPComponent,
			},
			{
				path: 'ho-tro',
				component: SupportComponent,
			},
			{
				path: 'tong-hop-so-lieu',
				component: StatisticsComponent,
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
				path: 'thong-tin-cong-bo',
				component: InfomationPublicComponent,
			},
			{
				path: 'ti-le-khong-hai-long',
				component: UnitDissatisfactionRateComponent,
			},
			{
				path: 'don-vi-xu-ly-tre-han',
				component: LateProcessingUnitComponent,
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
