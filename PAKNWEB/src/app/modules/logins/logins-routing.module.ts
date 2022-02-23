import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { LoginComponent } from './components/login/login.component'
import { ForgetPasswordComponent } from './components/forget-password/forget-password.component'
import { ForgetPasswordUserComponent } from './components/forget-password-user/forget-password-user.component'
import { LoginAdminComponent } from './components/login-admin/login-admin.component'

const routes: Routes = [
	{
		path: 'dang-nhap',
		component: LoginComponent,
	},
	{
		path: 'dang-nhap-quan-tri',
		component: LoginAdminComponent,
	},

	{
		path: 'quen-mat-khau',
		component: ForgetPasswordComponent,
	},
	{
		path: 'quen-mat-khau-quan-tri',
		component: ForgetPasswordUserComponent,
	},
]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class LoginsRoutingModule { }
