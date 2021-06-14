import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { from } from 'rxjs'
import { RecommendationsByUnitComponent } from 'src/app/modules/statistic/recommendations-by-unit/recommendations-by-unit.component'
import { RecommendationsByFieldComponent } from 'src/app/modules/statistic/recommendations-by-field/recommendations-by-field.component'
import { RecommendationsByGroupwordComponent } from './recommendations-by-groupword/recommendations-by-groupword.component'
import { RecommendationsByGroupwordDetailComponent } from './recommendations-by-groupword-detail/recommendations-by-groupword-detail.component'
import {RecommendationsByUnitDetailComponent} from './recommendations-by-unit-detail/recommendations-by-unit-detail.component'
import {RecommendationsByFieldDetailComponent} from './recommendations-by-field-detail/recommendations-by-field-detail.component'

const routes: Routes = [
	{ path: '', component: RecommendationsByUnitComponent },
	{ path: 'phan-anh-kien-nghi-theo-don-vi', component: RecommendationsByUnitComponent },
	{ path: 'phan-anh-kien-nghi-theo-don-vi-chi-tiet/:unitId/:fromDate/:toDate', component: RecommendationsByUnitDetailComponent },
	{ path: 'phan-anh-kien-nghi-theo-linh-vuc', component: RecommendationsByFieldComponent },
	{ path: 'phan-anh-kien-nghi-theo-linh-vuc-chi-tiet/:fieldId/:lstUnitId/:fromDate/:toDate', component: RecommendationsByFieldDetailComponent },
	{ path: 'phan-anh-kien-nghi-theo-nhom-tu-ngu', component: RecommendationsByGroupwordComponent },
	{ path: 'phan-anh-kien-nghi-theo-nhom-tu-ngu-chi-tiet/:unitId/:groupWordId/:fromDate/:toDate', component: RecommendationsByGroupwordDetailComponent },
]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class StatisticRoutingModule {}
