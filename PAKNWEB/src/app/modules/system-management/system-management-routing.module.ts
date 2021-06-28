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
import { IntroduceComponent } from './components/introduce/introduce.component'
import { IndexSettingComponent } from './components/index-setting/index-setting.component'
import {SystemConfigComponent} from './components/system-config/system-config.component'
import {SwitchboardSettingComponent} from './components/switchboard-setting/switchboard-setting.component'

const routes: Routes = [
	{
		path: '',
		component: SystemManagemenetComponent,
		children: [
			{
				path: 'cau-hinh-time',
				component: TimeSettingComponent,
				canActivate: [RoleGuardService],
				data: { role: 'A_XVIII_0' }
			},
			{ 
				path: 'cau-hinh-email/:id',
				component: EmailSettingComponent,
				canActivate: [RoleGuardService],
				data: { role: 'A_I_0' }
			},
			{ 
				path: 'cau-hinh-he-thong',
				component:  SystemConfigComponent,
				canActivate: [RoleGuardService],
				data: { role: 'A_I_0' }
			},
			{ 
				path: 'cau-hinh-sms/:id',
				component: SmsSettingComponent,
				canActivate: [RoleGuardService],
				data: { role: 'A_I_0' }
			},
			{ 
				path: 'cau-hinh-switchboard/:id', 
				component: SwitchboardSettingComponent,
				canActivate: [RoleGuardService],
				data: { role: 'A_I_0' }
			},
			{ 
				path: 'cau-hinh-trang-gioi-thieu', 
				component: IntroduceComponent,
				canActivate: [RoleGuardService],
				data: { role: 'A_XV_4' }
			},
			{ 
				path: 'cau-hinh-trang-chu',
				component: IndexSettingComponent,
				canActivate: [RoleGuardService],
				data: { role: 'A_XIX_0' }
			},
			{ 
				path: 'nguoi-dung',
				component: UserComponent,
				canActivate: [RoleGuardService],
				data: { role: 'C_I_0' }
			},
			{ 
				path: 'lich-su-he-thong',
				component: SystemLogComponent,
				canActivate: [RoleGuardService],
				data: { role: 'A_XIII_0' }
			},
			{ 
				path: 'co-cau-to-chuc',
				component: UnitComponent,
				canActivate: [RoleGuardService],
				data: { role: 'A_VII_0' }
			},
			// { 
			// 	path: 'nguoi-dung/them-moi', 
			// 	component: UserCreateOrUpdateComponent,
			// 	canActivate: [RoleGuardService],
			// 	data: { role: 'C_I_0' }
			// },
			{ 
				path: 'quan-ly-chat-bot',
				component: ChatBotComponent,
				canActivate: [RoleGuardService],
				data: { role: 'A_XI_3' }},
			{ 
				path: 'vai-tro',
				loadChildren: './components/role/role.module#RoleModule'
			},
			{ 
				path: 'lich-su-chat-bot',
				component: HistoryChatBotComponent,
				canActivate: [RoleGuardService],
				data: { role: 'A_XIV_0' }
			},
			// { 
			// 	path: 'nguoi-dung/thong-tin',
			// 	component: UserViewInfoComponent,
			// 	canActivate: [RoleGuardService],
			// 	data: { role: 'C_I_0' }
			// },
		],
	},
]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class SystemManagementRoutingModule {}
