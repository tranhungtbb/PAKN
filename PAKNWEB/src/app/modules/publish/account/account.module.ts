import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { FormsModule, ReactiveFormsModule } from '@angular/forms'
import { SharedModule } from 'src/app/shared/shared.module'
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker'
import { NgSelectModule } from '@ng-select/ng-select'

import { AccountRoutingModule } from './account-routing.module'
import { AccountInfoComponent } from './account-info/account-info.component'
import { ChangePasswordComponent } from './change-password/change-password.component'
import { AccountUpdateInfoComponent } from './account-update-info/account-update-info.component'

@NgModule({
	declarations: [AccountInfoComponent, ChangePasswordComponent, AccountUpdateInfoComponent],
	imports: [CommonModule, AccountRoutingModule, FormsModule, ReactiveFormsModule, SharedModule, BsDatepickerModule.forRoot(), NgSelectModule],
})
export class AccountModule {}
