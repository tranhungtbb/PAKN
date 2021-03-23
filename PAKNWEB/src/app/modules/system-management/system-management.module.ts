import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgSelectModule } from '@ng-select/ng-select';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { SystemManagementRoutingModule } from './system-management-routing.module';
import { SystemManagemenetComponent } from './system-managemenet.component';
import { OrgnizationComponent } from './components/orgnization/orgnization.component';
import { DepartmentsListComponent } from './components/orgnization/departments-list/departments-list.component';
import { EmployeesListComponent } from './components/orgnization/employees-list/employees-list.component';
import { DepartmentsInfoComponent } from './components/orgnization/department-info/department-info.component';
import { SharedModule } from '../../shared/shared.module';
import { SystemConfigComponent } from './components/system-config/system-config.component';
import { SystemConfigListComponent } from './components/system-config/system-config-list/system-config-list.component';
import { AngularDualListBoxModule } from 'angular-dual-listbox';
import { SystemLogComponent } from './components/system-log/system-log.component';
import { SystemlogListComponent } from './components/system-log/system-log-list.component/system-log-list.component';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { ContextMenuModule } from 'primeng/contextmenu';
import { TableModule } from 'primeng/table';
import { DropdownModule } from 'primeng/dropdown';
import { TreeModule } from 'primeng/tree';
import { TooltipModule } from 'primeng/tooltip';
import { GMapModule } from 'primeng/gmap';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { DepartmentComponent } from './components/department/department.component';

@NgModule({
  imports: [
    CommonModule,
    NgSelectModule,
    SystemManagementRoutingModule,
    ReactiveFormsModule,
    FormsModule,
    AngularDualListBoxModule,
    SharedModule,
    BsDatepickerModule.forRoot(),
    ContextMenuModule,
    TableModule,
    DropdownModule,
    TreeModule,
    TooltipModule,
    GMapModule,
    MatCheckboxModule,
    CKEditorModule,
  ],
  declarations: [
    SystemManagemenetComponent,
    OrgnizationComponent,
    DepartmentsListComponent,
    EmployeesListComponent,
    DepartmentsInfoComponent,
    SystemConfigComponent,
    SystemConfigListComponent,
    SystemLogComponent,
    SystemlogListComponent,
    DepartmentComponent,
  ]
})
    export class SystemManagementModule { }
