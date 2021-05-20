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
import { RecommendationsByFieldComponent } from 'src/app/modules/statistic/recommendations-by-field/recommendations-by-field.component';
import { RecommendationsByGroupwordComponent } from './recommendations-by-groupword/recommendations-by-groupword.component'

@NgModule({
	declarations: [StatisticComponent, RecommendationsByUnitComponent, RecommendationsByFieldComponent, RecommendationsByGroupwordComponent],
	imports: [CommonModule, StatisticRoutingModule, FormsModule, MultiSelectModule, ReactiveFormsModule, SharedModule, TableModule, BsDatepickerModule.forRoot(), NgSelectModule],
})
export class StatisticModule {}
