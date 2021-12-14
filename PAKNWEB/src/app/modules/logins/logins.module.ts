import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'

import { LoginsRoutingModule } from './logins-routing.module'
import { LoginComponent } from './components/login/login.component'
import { ReactiveFormsModule, FormsModule } from '@angular/forms'
import { ForgetPasswordComponent } from './components/forget-password/forget-password.component'
import { ForgetPasswordUserComponent } from './components/forget-password-user/forget-password-user.component'
import { SharedModule } from 'src/app/shared/shared.module'
import { LoginAdminComponent } from './components/login-admin/login-admin.component'

@NgModule({
	imports: [CommonModule, LoginsRoutingModule, ReactiveFormsModule, FormsModule, SharedModule],
	declarations: [LoginComponent, ForgetPasswordComponent, ForgetPasswordUserComponent, LoginAdminComponent],
})
export class LoginsModule {}
