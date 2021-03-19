import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';   
import { GroupUserInfoComponent } from './group-user-info/group-user-info.component';
import { CreateGroupUserComponent } from './create-group-user/create-group-user.component'; 
import { GroupUserManagementRoutingModule } from './group-user-management-routing.module';
import { GroupUserListComponent } from './group-user-info/group-user-list/group-user-list.component';
import { GroupUserManagementComponent } from './group-user-management.component';
import { UpdateGroupUserComponent } from './update-group-user/update-group-user.component';
import { MatFormFieldModule, MatInputModule, MatAutocompleteModule, MatButtonModule, MatProgressSpinnerModule } from '@angular/material';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { ViewGroupUserComponent } from './view-group-user/view-group-user.component';
import { SharedModule } from '../../../../shared/shared.module';
import { DePartmentTreeModule } from '../../../department-tree/department-tree-module';
import { TableModule } from 'primeng/table';
import { NgSelectModule } from '@ng-select/ng-select';
@NgModule({
  imports: [
    CommonModule, 
    ReactiveFormsModule,
    FormsModule, 
    GroupUserManagementRoutingModule, 
    MatInputModule,
    MatAutocompleteModule,
    MatFormFieldModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    NgSelectModule,
    MatCheckboxModule,
    SharedModule,
    DePartmentTreeModule,
    TableModule

  ],
  declarations: [
    GroupUserInfoComponent,
    CreateGroupUserComponent, 
    GroupUserListComponent,
    GroupUserManagementComponent,
    UpdateGroupUserComponent,
    ViewGroupUserComponent,
  ]
})
export class GroupUserManagementModule { }
