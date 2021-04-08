import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'

import { NgSelectModule } from '@ng-select/ng-select'
import { ReactiveFormsModule, FormsModule } from '@angular/forms'
import { PublishRoutingModule } from './publish-routing.module'
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
import { ChangePipe } from 'src/app/pipes/unit-filter.pipe'
import { from } from 'rxjs'

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
		ChangePipe,
	],
	imports: [CommonModule, PublishRoutingModule,
		NgSelectModule,
		ReactiveFormsModule,
		FormsModule,
		SharedModule,
		BsDatepickerModule.forRoot(),
		TooltipModule],
})
export class PublishModule { }
