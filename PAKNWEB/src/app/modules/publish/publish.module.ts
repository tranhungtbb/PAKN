import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { MatCheckboxModule } from '@angular/material/checkbox'

import { NgSelectModule } from '@ng-select/ng-select'
import { ReactiveFormsModule, FormsModule } from '@angular/forms'
import { PublishRoutingModule } from './publish-routing.module'
import { PaginatorModule } from 'primeng/paginator'
import { CarouselModule } from 'primeng/carousel';
import { ChartsModule } from 'ng2-charts'
import { TreeTableModule } from 'primeng/treetable'
import { PublishComponent } from './publish.component'
import { SharedModule } from '../../shared/shared.module'
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker'
import { IndexComponent } from './index/index.component'
import { AdministrativeProceduresComponent } from './administrative-procedures/administrative-procedures.component'
import { NewsComponent } from './news/news.component'
import { ReflectionsRecommendationsComponent } from './reflections-recommendations/reflections-recommendations.component'
import { IntroduceComponent } from './introduce/introduce.component'
import { SupportComponent } from './support/support.component'
import { ViewReflectionsRecommendationComponent } from './view-reflections-recommendation/view-reflections-recommendation.component'
import { CreateRecommendationComponent } from './user-create-recommendation/user-create-recommendation.component'
import { MyRecommendationComponent } from './my-recommendation/my-recommendation.component'
import { AgmCoreModule } from '@agm/core'
import { EditorModule } from 'primeng/editor'
import { TableModule } from 'primeng/table'
import { CheckboxModule } from 'primeng/checkbox'
import { ViewRecommendationPersonalComponent } from './view-recommendation-personal/view-recommendation-personal.component'
import { ViewNewsComponent } from './view-news/view-news.component'
import { ViewAdministrativeProceduresComponent } from './view-administrative-procedures/view-administrative-procedures.component'
import { ChatbotComponent } from './chatbot/chatbot.component'
import { ViewNotificationComponent } from './view-notification/view-notification.component'
import { ListRecommendationKnct } from 'src/app/modules/publish/recommendations-sync/recommendation-knct/recommendation-knct.component'
import { DetailRecommendationKnctComponent } from './recommendations-sync/recommendation-knct-detail/recommendation-knct-detail.component'
import { RecommendationsDvhhcComponent } from 'src/app/modules/publish/recommendations-sync/recommendations-dvhhc/recommendations-dvhhc.component'
import { DetailRecommendationDvhhcComponent } from 'src/app/modules/publish/recommendations-sync/recommendations-dvhhc-detail/recommendations-dvhhc-detail.component'
import { RecommendationsCttdtComponent } from './recommendations-sync/recommendations-cttdt/recommendations-cttdt.component'
import { DetailRecommendationCttdtComponent } from './recommendations-sync/recommendations-cttdt-detail/recommendations-cttdt-detail.component'
import { RecommendationsPaknCPComponent } from './recommendations-sync/recommendation-pakn-cp/recommendation-pakn-cp.component'
import { DetailRecommendationPaknCPComponent } from './recommendations-sync/recommendation-pakn-cp-detail/recommendation-pakn-cp-detail.component'
import { StatisticsRightComponent } from './statistics-right/statistics-right.component'
import { GalleryComponent } from './gallery/gallery.component'
import { StatisticsComponent } from './statistics/statistics.component'
import { NewsHightLightComponent } from './news-hight-light/news-hight-light.component'
import { ReceiveDenyRecommendationsComponent } from './receive-deny-recommendations/receive-deny-recommendations.component'
import { InfomationPublicComponent } from './infomation-public/infomation-public.component'
import { UnitDissatisfactionRateComponent } from './unit-dissatisfaction-rate/unit-dissatisfaction-rate.component'
import { LateProcessingUnitComponent } from './late-processing-unit/late-processing-unit.component'
import { ProcessingRecommendationsComponent } from './processing-recommendations/processing-recommendations.component'
import { CommentModule } from './comment-recommendation/comment.module'
import { InfomationExchangeModule } from '../recommendation/infomation-exchange/infomation-exchange.module'
import { GovernmentNoticeComponent } from './government-notice/government-notice.component'
import { HightLightRecommendationsComponent } from './hight-light-recommendations/hight-light-recommendations.component'

@NgModule({
	declarations: [
		PublishComponent,
		IndexComponent,
		AdministrativeProceduresComponent,
		NewsComponent,
		ViewNewsComponent,
		ReflectionsRecommendationsComponent,
		IntroduceComponent,
		SupportComponent,
		ViewReflectionsRecommendationComponent,
		CreateRecommendationComponent,
		MyRecommendationComponent,
		ViewRecommendationPersonalComponent,
		ViewAdministrativeProceduresComponent,
		ChatbotComponent,
		ViewNotificationComponent,
		ListRecommendationKnct,
		DetailRecommendationKnctComponent,
		RecommendationsDvhhcComponent,
		DetailRecommendationDvhhcComponent,
		RecommendationsCttdtComponent,
		DetailRecommendationCttdtComponent,
		RecommendationsPaknCPComponent,
		DetailRecommendationPaknCPComponent,
		StatisticsRightComponent,
		GalleryComponent,
		StatisticsComponent,
		NewsHightLightComponent,
		ReceiveDenyRecommendationsComponent,
		InfomationPublicComponent,
		UnitDissatisfactionRateComponent,
		LateProcessingUnitComponent,
		ProcessingRecommendationsComponent,
		GovernmentNoticeComponent,
		HightLightRecommendationsComponent,
	],
	imports: [
		CommonModule,
		PublishRoutingModule,
		NgSelectModule,
		ReactiveFormsModule,
		FormsModule,
		SharedModule,
		CommentModule,
		InfomationExchangeModule,
		TableModule,
		CheckboxModule,
		PaginatorModule,
		BsDatepickerModule.forRoot(),
		EditorModule,
		CarouselModule,
		TreeTableModule,
		MatCheckboxModule,
		AgmCoreModule.forRoot({
			apiKey: 'AIzaSyBriVbWgmHEE8CGaEJM6V47Bem3VoYCi0Q',
			language: 'vi',
			libraries: ['places'],
		}),
		ChartsModule,
	],
})
export class PublishModule { }
