import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UsersManagementRoutingModule } from './users-management-routing.module';
import { CreateUserComponent } from './create-user/create-user.component';
import { UsersManagementComponent } from './users-management.component';
import { UsersInfoComponent } from './users-info.component';
import { UsersListComponent } from './users-list/users-list.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { EditUserComponent } from './edit-user/edit-user.component';
// import { ConfirmClickDirective } from '../../../../directives/confirm-click.directive';
import { MatDialog, MatDialogModule, MatInputModule } from '@angular/material';
import { ViewUserComponent } from './view-user/view-user.component';
import { SharedModule } from '../../../../shared/shared.module';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { TooltipModule } from 'primeng/tooltip';
import { InputSwitchModule } from 'primeng/inputswitch';
import { TableModule } from 'primeng/table';
import { DropdownModule } from 'primeng/dropdown';
import { TreeModule } from 'primeng/tree';
import { DepartmentTreeComponent } from './department-tree/department-tree.component';
import { NgSelectModule } from '@ng-select/ng-select';
@NgModule({
  imports: [
    CommonModule,
    NgSelectModule,
    UsersManagementRoutingModule,
    ReactiveFormsModule,
    FormsModule,
    MatCheckboxModule,
    MatDialogModule,
    MatInputModule,
    SharedModule,
    BsDatepickerModule.forRoot(),
    TooltipModule,
    InputSwitchModule,
    TableModule,
    DropdownModule,
    TreeModule
  ],
  declarations: [
    CreateUserComponent,
    UsersManagementComponent,
    UsersInfoComponent,
    UsersListComponent,
    EditUserComponent,
    ViewUserComponent,
    DepartmentTreeComponent
  ]
})
export class UsersManagementModule { }
