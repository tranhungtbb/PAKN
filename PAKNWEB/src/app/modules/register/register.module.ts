import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { ReactiveFormsModule, FormsModule } from '@angular/forms'
import { NgSelectModule } from '@ng-select/ng-select'
import { SharedModule } from 'src/app/shared/shared.module'

import { RegisterRoutingModule } from './register-routing.module'
import { OrganizationComponent } from './organization/organization.component'
import { IndividualComponent } from './individual/individual.component'
import { RegisterComponent } from './register.component'
import { TabActiveDirective } from './tab-active.directive'

@NgModule({
	declarations: [OrganizationComponent, IndividualComponent, RegisterComponent, TabActiveDirective],
	imports: [CommonModule, RegisterRoutingModule, ReactiveFormsModule, FormsModule, NgSelectModule, SharedModule],
})
export class RegisterModule {}
