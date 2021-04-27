import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { FormsModule, ReactiveFormsModule } from '@angular/forms'
import { SharedModule } from 'src/app/shared/shared.module'
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker'
import { NgSelectModule } from '@ng-select/ng-select'
import { TableModule } from 'primeng/table'
import { RoleRoutingModule } from './role-routing.module'
import { RoleComponent } from './role.component'
import { RoleCreateOrUpdateComponent } from './role-create-or-update/role-create-or-update.component'

@NgModule({
	declarations: [RoleComponent, RoleCreateOrUpdateComponent],
	imports: [CommonModule, RoleRoutingModule, FormsModule, ReactiveFormsModule, SharedModule, TableModule, BsDatepickerModule.forRoot(), NgSelectModule],
})
export class RoleModule {}
