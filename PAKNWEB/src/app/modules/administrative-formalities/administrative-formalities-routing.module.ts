import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'

import { ListAdministrativeFormalitiesComponent } from './list-administrative-formalities/list-administrative-formalities.component'
import { CU_AdministrativeFormalitiesComponent } from './cu-administrative-formalities/cu-administrative-formalities.component'
import { DetailAdministrativeFormalitiesComponent } from './detail-administrative-formalities/detail-administrative-formalities.component'
import { ListAdministrativeFormalitiesPublishComponent } from './list-administrative-formalities-publish/list-administrative-formalities-publish.component'

const routes: Routes = [
	{ path: '', redirectTo: 'danh-sach-tong-hop' },
	{ path: 'danh-sach-tong-hop', component: ListAdministrativeFormalitiesComponent },
	{ path: 'them-moi', component: CU_AdministrativeFormalitiesComponent },
	{ path: 'cap-nhat/:id', component: CU_AdministrativeFormalitiesComponent },
	{ path: 'chi-tiet/:id', component: DetailAdministrativeFormalitiesComponent },
	{ path: 'thu-tuc-hanh-chinh-da-cong-bo', component: ListAdministrativeFormalitiesPublishComponent },
]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class AdministrativeFormalitiesRoutingModule {}
