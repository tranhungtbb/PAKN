import { NgModule, Component } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { RoleGuardService } from '../../guards/role-guard.service'
import { SystemManagemenetComponent } from './system-managemenet.component'
import { SystemLogComponent } from './components/system-log/system-log.component'
import { EmailSettingComponent } from './components/email-setting/email-setting.component'
import { SmsSettingComponent } from './components/sms-setting/sms-setting.component'
import { GroupUserComponent } from './components/group-user/group-user.component'
import { UserComponent } from './components/user/user.component'

const routes: Routes = [
	{
		path: '',
		component: SystemManagemenetComponent,
		children: [
			{ path: 'cau-hinh-email', component: EmailSettingComponent },
			{ path: 'cau-hinh-sms', component: SmsSettingComponent },
			{ path: 'vai-tro', component: GroupUserComponent },
			{ path: 'nguoi-dung', component: UserComponent },
			{ path: 'lich-su-he-thong', component: SystemLogComponent },
		],
	},
]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class SystemManagementRoutingModule {}
