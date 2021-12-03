import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { RecommendationsByUnitComponent } from 'src/app/modules/statistic/recommendations-by-unit/recommendations-by-unit.component'
import { RecommendationsByFieldComponent } from 'src/app/modules/statistic/recommendations-by-field/recommendations-by-field.component'
import { RecommendationsByGroupwordComponent } from './recommendations-by-groupword/recommendations-by-groupword.component'
import { RecommendationsByGroupwordDetailComponent } from './recommendations-by-groupword-detail/recommendations-by-groupword-detail.component'
import { RecommendationsByUnitDetailComponent } from './recommendations-by-unit-detail/recommendations-by-unit-detail.component'
import { RecommendationsByFieldDetailComponent } from './recommendations-by-field-detail/recommendations-by-field-detail.component'
import { RoleGuardService } from 'src/app/guards/role-guard.service'
import { ProcessingStatusComponent } from './processing-status/processing-status.component'
import { ProcessingResultsComponent } from './processing-results/processing-results.component'
import { ProcessingResultsByTypeComponent } from './processing-results-by-type/processing-results-by-type.component'
import { ProcessingResultsByReceptionTypeComponent } from './processing-results-by-reception-type/processing-results-by-reception-type.component'
import { RecommendationsByTypeDetailComponent } from './recommendations-by-type-detail/recommendations-by-type-detail.component'

const routes: Routes = [
	{
		path: '',
		component: RecommendationsByUnitComponent,
		canActivate: [RoleGuardService],
		data: { role: 'D_I_0' },
	},
	{
		path: 'phan-anh-kien-nghi-theo-don-vi',
		component: RecommendationsByUnitComponent,
		canActivate: [RoleGuardService],
		data: { role: 'D_I_0' },
	},
	{
		path: 'phan-anh-kien-nghi-theo-don-vi-chi-tiet/:unitId/:fromDate/:toDate',
		component: RecommendationsByUnitDetailComponent,
		canActivate: [RoleGuardService],
		data: { role: 'D_I_1' },
	},
	{
		path: 'phan-anh-kien-nghi-theo-don-vi-chi-tiet/:unitId/:fromDate/:toDate/:status',
		component: RecommendationsByUnitDetailComponent,
		canActivate: [RoleGuardService],
		data: { role: 'D_I_1' },
	},
	{
		path: 'phan-anh-kien-nghi-theo-linh-vuc',
		component: RecommendationsByFieldComponent,
		canActivate: [RoleGuardService],
		data: { role: 'D_II_0' },
	},
	{
		path: 'phan-anh-kien-nghi-theo-linh-vuc-chi-tiet/:fieldId/:lstUnitId/:fromDate/:toDate/:status',
		component: RecommendationsByFieldDetailComponent,
		canActivate: [RoleGuardService],
		data: { role: 'D_II_1' },
	},
	{
		path: 'phan-anh-kien-nghi-theo-linh-vuc-chi-tiet/:fieldId/:lstUnitId/:fromDate/:toDate',
		component: RecommendationsByFieldDetailComponent,
		canActivate: [RoleGuardService],
		data: { role: 'D_II_1' },
	},
	{
		path: 'phan-anh-kien-nghi-theo-nhom-tu-ngu',
		component: RecommendationsByGroupwordComponent,
		canActivate: [RoleGuardService],
		data: { role: 'D_III_0' },
	},
	{
		path: 'phan-anh-kien-nghi-theo-nhom-tu-ngu-chi-tiet/:unitId/:groupWordId/:fromDate/:toDate',
		component: RecommendationsByGroupwordDetailComponent,
		canActivate: [RoleGuardService],
		data: { role: 'D_III_1' },
	},
	{
		path: 'tinh-trang-xu-ly-pakn',
		component: ProcessingStatusComponent,
		canActivate: [RoleGuardService],
		data: { role: 'D_III_0' },
	},
	{
		path: 'ket-qua-xu-ly-pakn',
		component: ProcessingResultsComponent,
		canActivate: [RoleGuardService],
		data: { role: 'D_III_0' },
	},
	{
		path: 'ket-qua-xu-ly-theo-loai-pakn',
		component: ProcessingResultsByTypeComponent,
		canActivate: [RoleGuardService],
		data: { role: 'D_III_0' },
	},
	{
		path: 'chi-tiet-ket-qua-xu-ly-theo-loai-pakn/:type/:fieldId/:unitId/:RecommendationType/:fromDate/:toDate',
		component: RecommendationsByTypeDetailComponent,
		canActivate: [RoleGuardService],
		data: { role: 'D_III_0' },
	},
	{
		path: 'ket-qua-xu-ly-theo-pttn',
		component: ProcessingResultsByReceptionTypeComponent,
		canActivate: [RoleGuardService],
		data: { role: 'D_III_0' },
	},
]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class StatisticRoutingModule { }
