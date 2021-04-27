import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { ReactiveFormsModule, FormsModule } from '@angular/forms'
import { SharedModule } from '../../shared/shared.module'
import { MatDialogModule } from '@angular/material/dialog'
import { TableModule } from 'primeng/table'

import { RoleRoutingModule } from './role-routing.module'
import { RoleComponent } from './role.component'
import { RoleCreateOrUpdateComponent } from './role-create-or-update/role-create-or-update.component'

@NgModule({
	declarations: [RoleComponent, RoleCreateOrUpdateComponent],
	imports: [CommonModule, RoleRoutingModule, ReactiveFormsModule, FormsModule, SharedModule, MatDialogModule, TableModule],
})
export class RoleModule {}
