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
import { GroupWordComponent } from './group-word/group-word.component'

const routes: Routes = [
	{
		path: '',
		component: CatalogManagementComponent,
		children: [
			{ 
				path: 'chuc-vu',
				component: PositionComponent,
				canActivate: [RoleGuardService],
				data: { role: 'C_I_0' }
			},
			{ 
				path: 'linh-vuc',
				component: FieldComponent,
				canActivate: [RoleGuardService],
				data: { role: 'C_IV_3' }
			},
			{ 
				path: 'loai-tin-tuc',
				component: NewsTypeComponent,
				canActivate: [RoleGuardService],
				data: { role: 'C_V_3' } 
			},
			{ 
				path: 'thu-vien-tu',
				component: WordLibraryComponent,
				canActivate: [RoleGuardService],
				data: { role: 'C_VI_3' } 
			},
			{ 
				path: 'so-nganh',
				component: DepartmentComponent,
				canActivate: [RoleGuardService],
				data: { role: 'C_III_3' }
			},
			{ 
				path: 'nhom-so-nganh',
				component: DepartmentGroupComponent ,
				canActivate: [RoleGuardService],
				data: { role: 'C_II_3' }
			},
			{ 
				path: 'hashtag', component: HashtagComponent,
				canActivate: [RoleGuardService],
				data: { role: 'C_VIII_3' }
			},
			{ 
				path: 'nhom-thu-vien-tu',
				component: GroupWordComponent ,
				canActivate: [RoleGuardService],
				data: { role: 'C_VII_3' }
			},
		],
	},
]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class CatalogManagementRoutingModule {}
