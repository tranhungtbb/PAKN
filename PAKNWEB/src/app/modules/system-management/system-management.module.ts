import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { NgSelectModule } from '@ng-select/ng-select'
import { CKEditorModule } from '@ckeditor/ckeditor5-angular'
import { ReactiveFormsModule, FormsModule } from '@angular/forms'
import { SystemManagementRoutingModule } from './system-management-routing.module'
import { SystemManagemenetComponent } from './system-managemenet.component'
import { SharedModule } from '../../shared/shared.module'
import { AngularDualListBoxModule } from 'angular-dual-listbox'
import { SystemLogComponent } from './components/system-log/system-log.component'
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker'
import { ContextMenuModule } from 'primeng/contextmenu'
import { TableModule } from 'primeng/table'
import { DropdownModule } from 'primeng/dropdown'
import { TreeModule } from 'primeng/tree'
import { TooltipModule } from 'primeng/tooltip'
import { GMapModule } from 'primeng/gmap'
import { MatCheckboxModule } from '@angular/material/checkbox'
import { MatDialogModule } from '@angular/material/dialog'
import { EmailSettingComponent } from './components/email-setting/email-setting.component'
import { SmsSettingComponent } from './components/sms-setting/sms-setting.component'
import { OrganizationalStructureComponent } from './components/organizational-structure/organizational-structure.component'
import { GroupUserComponent } from './components/group-user/group-user.component'
import { UserComponent } from './components/user/user.component'
import { UnitComponent } from './components/unit/unit.component'
import { UserCreateOrUpdateComponent } from './components/user/user-create-or-update/user-create-or-update.component'
import { UnitFilterPipe } from 'src/app/pipes/unit-filter.pipe'
import { ChatBotComponent } from './components/chat-bot/chat-bot.component'
// import {} from './components/role/role.component'

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
		MatDialogModule,
	],
	declarations: [
		SystemManagemenetComponent,
		SystemLogComponent,
		EmailSettingComponent,
		SmsSettingComponent,
		OrganizationalStructureComponent,
		GroupUserComponent,
		UserComponent,
		UnitComponent,
		UserCreateOrUpdateComponent,
		UnitFilterPipe,
		ChatBotComponent,
	],
	entryComponents: [UserCreateOrUpdateComponent],
})
export class SystemManagementModule {}
