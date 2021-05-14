import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { RecommendationsByUnitComponent } from 'src/app/modules/statistic/recommendations-by-unit/recommendations-by-unit.component'

const routes: Routes = [
	{ path: '', component: RecommendationsByUnitComponent },
	{ path: 'phan-anh-kien-nghi-theo-don-vi', component: RecommendationsByUnitComponent },
]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class StatisticRoutingModule {}
