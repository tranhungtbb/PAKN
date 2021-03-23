import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { CatalogManagementComponent } from './catalog-management.component'
import { GroupUsersComponent } from './group-users/group-users.component'
import { DepartmentGroupComponent } from './department-group/department-group.component'
import { MajorComponent } from './major/major.component'
import { StageComponent } from './stage/stage.component'
import { RecommendationsComponent } from './recommendations/recommendations.component'
import { RecommendationTypeTreeComponent } from './recommendation-type-tree/recommendation-type-tree.component'
import { RecommendationsFieldComponent } from './recommendations-field/recommendations-field.component'
import { ComplaintLetterComponent } from './complaint-letter/complaint-letter.component'
import { PositionComponent } from './position/position.component'
import { SessionComponent } from './session/session.component'
import { RoleGuardService } from '../../guards/role-guard.service'
import { RegionCatalogComponent } from './region-catalog/region-catalog.component'
import { NationComponent } from './nation/nation.component'
import { ResolutionTypeComponent } from './resolution-type/resolution-type.component'
import { RecommendationsTypeComponent } from './recommendations-type/recommendations-type.component'
import { UnitComponent } from './unit/unit.component'

const routes: Routes = [
	{
		path: '',
		component: CatalogManagementComponent,
		children: [
			{
				path: 'chuc-vu',
				component: PositionComponent,
				canActivate: [RoleGuardService],
				data: { role: 'B_I_0' },
			},
			{
				path: 'group-users',
				component: GroupUsersComponent,
			},
			{
				path: 'department-group',
				component: DepartmentGroupComponent,
				canActivate: [RoleGuardService],
				data: { role: 'B_II_0' },
			},
			{
				path: 'major',
				component: MajorComponent,
				canActivate: [RoleGuardService],
				data: { role: 'B_III_0' },
			},
			{
				path: 'stage',
				component: StageComponent,
				canActivate: [RoleGuardService],
				data: { role: 'B_IV_0' },
			},
			{
				path: 'recommendations',
				component: RecommendationsComponent,
				canActivate: [RoleGuardService],
				data: { role: 'B_V_0' },
			},
			{
				path: 'recommendations-type',
				component: RecommendationsTypeComponent,
				canActivate: [RoleGuardService],
				data: { role: 'B_VI_0' },
			},
			{
				path: 'recommendations-field',
				component: RecommendationTypeTreeComponent,
				canActivate: [RoleGuardService],
				data: { role: 'B_VII_0' },
			},
			{
				path: 'complaint-letter',
				component: ComplaintLetterComponent,
				canActivate: [RoleGuardService],
				data: { role: 'B_VIII_0' },
			},
			{
				path: 'session',
				component: SessionComponent,
				canActivate: [RoleGuardService],
				data: { role: 'B_IX_0' },
			},
			{
				path: 'region',
				component: RegionCatalogComponent,
				canActivate: [RoleGuardService],
				data: { role: 'B_X_0' },
			},
			{
				path: 'nation',
				component: NationComponent,
				canActivate: [RoleGuardService],
				data: { role: 'B_XI_0' },
			},
			{
				path: 'resolution-type',
				component: ResolutionTypeComponent,
				canActivate: [RoleGuardService],
				data: { role: 'B_XII_0' },
			},
			{
				path: 'unit-ktnn',
				component: UnitComponent,
				canActivate: [RoleGuardService],
				data: { role: 'B_XIII_0' },
			},
		],
	},
]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class CatalogManagementRoutingModule {}
