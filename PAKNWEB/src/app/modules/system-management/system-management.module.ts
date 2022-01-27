import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { NgSelectModule } from '@ng-select/ng-select'
import { CKEditorModule } from '@ckeditor/ckeditor5-angular'
import { ReactiveFormsModule, FormsModule } from '@angular/forms'
import { SystemManagementRoutingModule } from './system-management-routing.module'
import { SystemManagemenetComponent } from './system-managemenet.component'
import { SharedModule } from '../../shared/shared.module'
import { AngularDualListBoxModule } from 'angular-dual-listbox'
import { MatRadioModule } from '@angular/material/radio'
import { SystemLogComponent } from './components/system-log/system-log.component'
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker'
import { ContextMenuModule } from 'primeng/contextmenu'
import { TableModule } from 'primeng/table'
import { DropdownModule } from 'primeng/dropdown'
import { TreeModule } from 'primeng/tree'
import { CalendarModule } from 'primeng/calendar'
import { LazyLoadImageModule, scrollPreset } from 'ng-lazyload-image'
import { TooltipModule } from 'primeng/tooltip'
import { GMapModule } from 'primeng/gmap'
import { MatCheckboxModule } from '@angular/material/checkbox'
import { MatDialogModule } from '@angular/material/dialog'
import { EmailSettingComponent } from './components/email-setting/email-setting.component'
import { TimeSettingComponent } from './components/time-setting/time-setting.component'
import { SmsSettingComponent } from './components/sms-setting/sms-setting.component'
import { OrganizationalStructureComponent } from './components/organizational-structure/organizational-structure.component'
import { GroupUserComponent } from './components/group-user/group-user.component'
import { UserComponent } from './components/user/user.component'
import { UnitComponent } from './components/unit/unit.component'
import { UserCreateOrUpdateComponent } from './components/user/user-create-or-update/user-create-or-update.component'
import { UnitFilterPipe } from 'src/app/pipes/unit-filter.pipe'
import { ChatBotComponent } from './components/chat-bot/chat-bot.component'
import { HistoryChatBotComponent } from './components/history-chat-bot/history-chat-bot.component'
import { UserViewInfoComponent } from './components/user/user-view-info/user-view-info.component'
import { BusinessModule } from '../business.module'
import { IntroduceComponent } from './components/introduce/introduce.component'
import { IndexSettingComponent } from './components/index-setting/index-setting.component'
import { SystemConfigComponent } from './components/system-config/system-config.component'
import { SwitchboardSettingComponent } from './components/switchboard-setting/switchboard-setting.component'
import { UserSystemComponent } from './components/user-system/user-system.component'
import { NummerOfWarningSettingComponent } from './components/number-of-warning/number-of-warning.component'
import { SupportGalleryComponent } from './components/support-gallery/support-gallery.component'
import { ApplicationSettingComponent } from './components/application-setting/application-setting.component'
import { IndexTypeSettingComponent } from './components/index-type-setting/index-type-setting.component'
import { SyncSettingComponent } from './components/sync-setting/sync-setting.component'
import { PublishNotificationComponent } from './components/publish-notification/publish-notification.component';
import { StatisticAccessComponent } from './components/statistic-access/statistic-access.component'
import { ConfigRadiusComponent } from './components/config-radius/config-radius.component'
import { ConfigCommentComponent } from './components/config-comment/config-comment.component'
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
		CalendarModule,
		TooltipModule,
		GMapModule,
		MatCheckboxModule,
		CKEditorModule,
		MatDialogModule,
		BusinessModule,
		MatRadioModule,
		LazyLoadImageModule.forRoot({
			preset: scrollPreset,
		}),
	],
	declarations: [
		SystemManagemenetComponent,
		SystemLogComponent,
		EmailSettingComponent,
		TimeSettingComponent,
		SmsSettingComponent,
		OrganizationalStructureComponent,
		GroupUserComponent,
		UserComponent,
		UnitComponent,
		SystemConfigComponent,
		UnitFilterPipe,
		ChatBotComponent,
		HistoryChatBotComponent,
		IntroduceComponent,
		IndexSettingComponent,
		SwitchboardSettingComponent,
		UserSystemComponent,
		NummerOfWarningSettingComponent,
		SupportGalleryComponent,
		IndexTypeSettingComponent,
		SyncSettingComponent,
		PublishNotificationComponent,
		ApplicationSettingComponent,
		StatisticAccessComponent,
		ConfigRadiusComponent,
		ConfigCommentComponent
	],
	entryComponents: [UserCreateOrUpdateComponent, UserViewInfoComponent],
})
export class SystemManagementModule { }
