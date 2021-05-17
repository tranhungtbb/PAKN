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

@NgModule({
	declarations: [StatisticComponent, RecommendationsByUnitComponent],
	imports: [CommonModule, StatisticRoutingModule, FormsModule, MultiSelectModule, ReactiveFormsModule, SharedModule, TableModule, BsDatepickerModule.forRoot(), NgSelectModule],
})
export class StatisticModule {}
