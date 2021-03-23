import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { NgSelectModule } from '@ng-select/ng-select'
import { ReactiveFormsModule, FormsModule } from '@angular/forms'
import { CatalogManagementRoutingModule } from './catalog-management-routing.module'
import { CatalogManagementComponent } from './catalog-management.component'
import { SharedModule } from '../../shared/shared.module'
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker'
import { TooltipModule } from 'primeng/tooltip'
import { TreeModule } from 'primeng/tree'
import { TableModule } from 'primeng/table'
import { ContextMenuModule } from 'primeng/contextmenu'
import { GroupUsersComponent } from './group-users/group-users.component'
import { GroupUsersListComponent } from './group-users/group-users-list/group-users-list.component'
import { DepartmentGroupComponent } from './department-group/department-group.component'
import { MajorComponent } from './major/major.component'
import { StageComponent } from './stage/stage.component'
import { RecommendationsComponent } from './recommendations/recommendations.component'
import { RecommendationsTypeComponent } from './recommendations-type/recommendations-type.component'
import { RecommendationsFieldComponent } from './recommendations-field/recommendations-field.component'
import { ComplaintLetterComponent } from './complaint-letter/complaint-letter.component'
import { PositionComponent } from './position/position.component'
import { SessionComponent } from './session/session.component'
import { RegionCatalogComponent } from './region-catalog/region-catalog.component'
import { MatCheckboxModule } from '@angular/material'
import { NationComponent } from './nation/nation.component'
import { ResolutionTypeComponent } from './resolution-type/resolution-type.component'
import { TreeTableModule } from 'primeng/treetable'
import { RecommendationTypeTreeComponent } from './recommendation-type-tree/recommendation-type-tree.component'
import { UnitComponent } from './unit/unit.component'

@NgModule({
	imports: [
		CommonModule,
		ReactiveFormsModule,
		MatCheckboxModule,
		FormsModule,
		CatalogManagementRoutingModule,
		SharedModule,
		BsDatepickerModule.forRoot(),
		TooltipModule,
		TreeModule,
		TreeTableModule,
		TableModule,
		NgSelectModule,
		ContextMenuModule,
	],
	declarations: [
		CatalogManagementComponent,
		GroupUsersComponent,
		GroupUsersListComponent,
		DepartmentGroupComponent,
		MajorComponent,
		StageComponent,
		RecommendationsComponent,
		RecommendationsTypeComponent,
		RecommendationsFieldComponent,
		ComplaintLetterComponent,
		PositionComponent,
		SessionComponent,
		RegionCatalogComponent,
		NationComponent,
		ResolutionTypeComponent,
		RecommendationTypeTreeComponent,
		UnitComponent,
	],
})
export class CatalogManagementModule {}
