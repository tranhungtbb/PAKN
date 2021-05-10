import { NgModule, Component } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { RoleGuardService } from '../../guards/role-guard.service'
import { SystemManagemenetComponent } from './system-managemenet.component'
import { SystemLogComponent } from './components/system-log/system-log.component'
import { EmailSettingComponent } from './components/email-setting/email-setting.component'
import { TimeSettingComponent } from './components/time-setting/time-setting.component'
import { SmsSettingComponent } from './components/sms-setting/sms-setting.component'
import { GroupUserComponent } from './components/group-user/group-user.component'
import { UserComponent } from './components/user/user.component'
import { UnitComponent } from './components/unit/unit.component'
import { UserCreateOrUpdateComponent } from './components/user/user-create-or-update/user-create-or-update.component'
import { ChatBotComponent } from './components/chat-bot/chat-bot.component'
import { HistoryChatBotComponent } from './components/history-chat-bot/history-chat-bot.component'
import { UserViewInfoComponent } from './components/user/user-view-info/user-view-info.component'
const routes: Routes = [
	{
		path: '',
		component: SystemManagemenetComponent,
		children: [
			{ path: 'cau-hinh-time', component: TimeSettingComponent },
			{ path: 'cau-hinh-email', component: EmailSettingComponent },
			{ path: 'cau-hinh-sms', component: SmsSettingComponent },
			// { path: 'vai-tro', component: GroupUserComponent },
			{ path: 'nguoi-dung', component: UserComponent },
			{ path: 'lich-su-he-thong', component: SystemLogComponent },
			{ path: 'co-cau-to-chuc', component: UnitComponent },
			{ path: 'nguoi-dung/them-moi', component: UserCreateOrUpdateComponent },
			{ path: 'quan-ly-chat-bot', component: ChatBotComponent },
			{ path: 'vai-tro', loadChildren: './components/role/role.module#RoleModule' },
			{ path: 'lich-su-chat-bot', component: HistoryChatBotComponent },
			{ path: 'nguoi-dung/thong-tin', component: UserViewInfoComponent },
		],
	},
]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class SystemManagementRoutingModule {}
