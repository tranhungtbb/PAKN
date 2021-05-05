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
import { DepartmentGroupComponent } from './department-group/department-group.component'
import { PositionComponent } from './position/position.component'
import { MatCheckboxModule } from '@angular/material'
import { TreeTableModule } from 'primeng/treetable'
import { FieldComponent } from './field/field.component'
import { NewsTypeComponent } from './news-type/news-type.component'
import { WordLibraryComponent } from './word-library/word-library.component'
import { DepartmentComponent } from './department/department.component'
import { HashtagComponent } from './hashtag/hashtag.component'
// import { RemindComponent } from '../recommendation/remind/remind.component'
import { from } from 'rxjs';
import { GroupWordComponent } from './group-word/group-word.component'

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
		DepartmentGroupComponent,
		PositionComponent,
		FieldComponent,
		NewsTypeComponent,
		WordLibraryComponent,
		DepartmentComponent,
		HashtagComponent,
		GroupWordComponent,
		// RemindComponent,
	],
})
export class CatalogManagementModule {}
