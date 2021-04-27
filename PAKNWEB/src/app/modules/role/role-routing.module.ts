import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'

import { RoleComponent } from './role.component'
import { RoleCreateOrUpdateComponent } from './role-create-or-update/role-create-or-update.component'

const routes: Routes = [
	{ path: '', component: RoleComponent },
	{ path: 'them-moi', component: RoleCreateOrUpdateComponent },
	{ path: 'chinh-sua/:id', component: RoleCreateOrUpdateComponent },
]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class RoleRoutingModule {}
