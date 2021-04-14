import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'

import { NgSelectModule } from '@ng-select/ng-select'
import { ReactiveFormsModule, FormsModule } from '@angular/forms'
import { PublishRoutingModule } from './publish-routing.module'
import { PaginatorModule } from 'primeng/paginator'
//import { CarouselModule } from 'ngx-owl-carousel-o'

import { PublishComponent } from './publish.component'
import { SharedModule } from '../../shared/shared.module'
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker'
import { TooltipModule } from 'primeng/tooltip'
import { IndexComponent } from './index/index.component'
import { AdministrativeProceduresComponent } from './administrative-procedures/administrative-procedures.component'
import { NewsComponent } from './news/news.component'
import { ReflectionsRecommendationsComponent } from './reflections-recommendations/reflections-recommendations.component'
import { IntroduceComponent } from './introduce/introduce.component'
import { SupportComponent } from './support/support.component'
import { ViewReflectionsRecommendationComponent } from './view-reflections-recommendation/view-reflections-recommendation.component'
import { CreateRecommendationComponent } from './user-create-recommendation/user-create-recommendation.component'
import { MyRecommendationComponent } from './my-recommendation/my-recommendation.component'
import { from } from 'rxjs'
import { EditorModule } from 'primeng/editor'
import { ViewRecommendationPersonalComponent } from './view-recommendation-personal/view-recommendation-personal.component'

@NgModule({
	declarations: [
		PublishComponent,
		IndexComponent,
		AdministrativeProceduresComponent,
		NewsComponent,
		ReflectionsRecommendationsComponent,
		IntroduceComponent,
		SupportComponent,
		ViewReflectionsRecommendationComponent,
		CreateRecommendationComponent,
		MyRecommendationComponent,
		ViewRecommendationPersonalComponent,
	],
	imports: [
		CommonModule,
		PublishRoutingModule,
		NgSelectModule,
		ReactiveFormsModule,
		FormsModule,
		SharedModule,
		PaginatorModule,
		BsDatepickerModule.forRoot(),
		TooltipModule,
		//CarouselModule,
		EditorModule,
	],
})
export class PublishModule {}
