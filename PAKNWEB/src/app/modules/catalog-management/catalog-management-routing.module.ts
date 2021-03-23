import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { RoleGuardService } from '../../guards/role-guard.service'
import { CatalogManagementComponent } from './catalog-management.component'
import { PositionComponent } from './position/position.component'
import { DepartmentComponent } from './department/department.component'
import { DepartmentGroupComponent } from './department-group/department-group.component'
import { FieldComponent } from './field/field.component'
import { NewsTypeComponent } from './news-type/news-type.component'
import { WordLibraryComponent } from './word-library/word-library.component'
import { HashtagComponent } from './hashtag/hashtag.component'

const routes: Routes = [
	{
		path: '',
		component: CatalogManagementComponent,
		children: [
			{ path: 'chuc-vu', component: PositionComponent },
			{ path: 'linh-vuc', component: FieldComponent },
			{ path: 'loai-tin-tuc', component: NewsTypeComponent },
			{ path: 'thu-vien-tu', component: WordLibraryComponent },
			{ path: 'so-nganh', component: DepartmentComponent },
			{ path: 'nhom-so-nganh', component: DepartmentGroupComponent },
			{ path: 'hashtag', component: HashtagComponent },
		],
	},
]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class CatalogManagementRoutingModule {}
