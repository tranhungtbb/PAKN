import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { RoleGuardService } from '../../guards/role-guard.service'

import { ListAdministrativeFormalitiesComponent } from './list-administrative-formalities/list-administrative-formalities.component'
import { CU_AdministrativeFormalitiesComponent } from './cu-administrative-formalities/cu-administrative-formalities.component'
import { DetailAdministrativeFormalitiesComponent } from './detail-administrative-formalities/detail-administrative-formalities.component'
import { ListAdministrativeFormalitiesPublishComponent } from './list-administrative-formalities-publish/list-administrative-formalities-publish.component'
import { ListAdministrativeForwardComponent } from './list-administrative-forward/list-administrative-forward.component'

const routes: Routes = [
	{ 
		path: '', redirectTo: 'danh-sach-tong-hop',
		canActivate: [RoleGuardService],
		data: { role: 'H_I_0' }
	},
	{ 
		path: 'danh-sach-tong-hop',
		component: ListAdministrativeFormalitiesComponent,
		canActivate: [RoleGuardService],
		data: { role: 'H_I_0' }
	},
	{ 
		path: 'them-moi',
		component: CU_AdministrativeFormalitiesComponent,
		// canActivate: [RoleGuardService],
		// data: { role: 'C_I_0' }
	},
	{ 
		path: 'cap-nhat/:id',
		component: CU_AdministrativeFormalitiesComponent,
		// canActivate: [RoleGuardService],
		// data: { role: 'C_I_0' }
	},
	{ 
		path: 'chi-tiet/:id',
		component: DetailAdministrativeFormalitiesComponent,
		canActivate: [RoleGuardService],
		data: { role: 'H_I_4' }
	},
	{ 
		path: 'thu-tuc-hanh-chinh-da-cong-bo',
		component: ListAdministrativeFormalitiesPublishComponent,
		canActivate: [RoleGuardService],
		data: { role: 'H_I_1' }
	},
	{ 
		path: 'thu-tuc-hanh-chinh-chuyen-tiep-tiep-nhan',
		component: ListAdministrativeForwardComponent,
		canActivate: [RoleGuardService],
		data: { role: 'H_I_2' }
	},
]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class AdministrativeFormalitiesRoutingModule {}
