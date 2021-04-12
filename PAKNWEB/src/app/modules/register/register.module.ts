import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { ReactiveFormsModule, FormsModule } from '@angular/forms'
import { NgSelectModule } from '@ng-select/ng-select'
import { SharedModule } from 'src/app/shared/shared.module'
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker'
import { MatDatepickerModule, MatInputModule, MatNativeDateModule } from '@angular/material'

import { RegisterRoutingModule } from './register-routing.module'
import { OrganizationComponent } from './organization/organization.component'
import { IndividualComponent } from './individual/individual.component'
import { RegisterComponent } from './register.component'
import { TabActiveDirective } from './tab-active.directive'
import { OrgFormAddressComponent } from './organization/org-form-address/org-form-address.component'
import { OrgRepreFormComponent } from './organization/org-repre-form/org-repre-form.component'

@NgModule({
	declarations: [OrganizationComponent, IndividualComponent, RegisterComponent, TabActiveDirective, OrgFormAddressComponent, OrgRepreFormComponent],
	imports: [CommonModule, RegisterRoutingModule, BsDatepickerModule.forRoot(), ReactiveFormsModule, FormsModule, NgSelectModule, SharedModule],
	providers: [],
})
export class RegisterModule {}
