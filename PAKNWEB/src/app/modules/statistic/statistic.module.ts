import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { FormsModule, ReactiveFormsModule } from '@angular/forms'
import { SharedModule } from 'src/app/shared/shared.module'
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker'
import { NgSelectModule } from '@ng-select/ng-select'
import { TableModule } from 'primeng/table'
import { MultiSelectModule } from 'primeng/multiselect'

import { StatisticRoutingModule } from './statistic-routing.module'
import { StatisticComponent } from './statistic.component'
import { RecommendationsByUnitComponent } from './recommendations-by-unit/recommendations-by-unit.component'
import { RecommendationsByFieldComponent } from 'src/app/modules/statistic/recommendations-by-field/recommendations-by-field.component'
import { RecommendationsByGroupwordComponent } from './recommendations-by-groupword/recommendations-by-groupword.component'
import { RecommendationsByGroupwordDetailComponent } from './recommendations-by-groupword-detail/recommendations-by-groupword-detail.component'
import { RecommendationsByUnitDetailComponent } from './recommendations-by-unit-detail/recommendations-by-unit-detail.component'
import { RecommendationsByFieldDetailComponent } from './recommendations-by-field-detail/recommendations-by-field-detail.component'
import { ProcessingStatusComponent } from './processing-status/processing-status.component'
import { ProcessingResultsComponent } from './processing-results/processing-results.component'
import { ProcessingResultsByTypeComponent } from './processing-results-by-type/processing-results-by-type.component'
import { ProcessingResultsByReceptionTypeComponent } from './processing-results-by-reception-type/processing-results-by-reception-type.component'
import { RecommendationsByTypeDetailComponent } from './processing-results-by-type-detail/processing-results-by-type-detail.component'
import { RecommendationsByReceptionTypeDetailComponent } from './processing-results-by-reception-type-detail/processing-results-by-reception-type-detail.component'
import { RecommendationsProcessingResultDetailComponent } from './processing-results-detail/processing-results-detail.component'

@NgModule({
	declarations: [
		StatisticComponent,
		RecommendationsByUnitComponent,
		RecommendationsByUnitDetailComponent,
		RecommendationsByFieldComponent,
		RecommendationsByFieldDetailComponent,
		RecommendationsByGroupwordComponent,
		RecommendationsByGroupwordDetailComponent,
		ProcessingStatusComponent,
		ProcessingResultsComponent,
		ProcessingResultsByTypeComponent,
		ProcessingResultsByReceptionTypeComponent,
		RecommendationsByTypeDetailComponent,
		RecommendationsByReceptionTypeDetailComponent,
		RecommendationsProcessingResultDetailComponent
	],
	imports: [CommonModule, StatisticRoutingModule, FormsModule, MultiSelectModule, ReactiveFormsModule, SharedModule, TableModule, BsDatepickerModule.forRoot(), NgSelectModule],
})
export class StatisticModule { }
