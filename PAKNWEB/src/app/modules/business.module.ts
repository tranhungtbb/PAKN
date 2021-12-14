import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { BusinessRoutingModule } from './business-routing.module'
import { BusinessComponent } from './business.component'
import { AppheaderComponent } from '../components/appheader/appheader.component'
import { AppfooterComponent } from '../components/appfooter/appfooter.component'
import { AppmenuComponent } from '../components/appmenu/appmenu.component'
import { ReactiveFormsModule, FormsModule } from '@angular/forms'
import { AngularDualListBoxModule } from 'angular-dual-listbox'
import { SharedModule } from '../shared/shared.module'
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker'
import { NgSelectModule } from '@ng-select/ng-select'
import { MatCheckboxModule } from '@angular/material/checkbox'
import { TableModule } from 'primeng/table'
import { ScrollPanelModule } from 'primeng/scrollpanel'
import { VirtualScrollerModule } from 'primeng/virtualscroller'
import { DxDropDownBoxModule, DxTreeViewModule, DxDataGridModule } from 'devextreme-angular'
import { DxReportViewerModule } from 'devexpress-reporting-angular'
import { TreeviewModule } from 'ngx-treeview'
import { ChartModule } from 'primeng/chart'

import { DashboardComponent } from './dash-board/dash-board.component'
import { NotificationComponent } from './notification/notification.component'
import { RecommnendationGetListComponent } from './dash-board/recommnendation-get-list/recommnendation-get-list.component'
import { ReportViewerComponent } from './report-view/report-viewcomponent'
import { UserViewInfoComponent } from './system-management/components/user/user-view-info/user-view-info.component'
import { UserCreateOrUpdateComponent } from './system-management/components/user/user-create-or-update/user-create-or-update.component'
import { LoginChatBoxComponent } from './chatbox/user/login/login.component'
import { DashboardChatBoxComponent } from './chatbox/dashboard/dashboard.component'
import { UserModule } from './chatbox/user/user.module'
import { DialogsComponent } from './chatbox/dashboard/dialogs/dialogs.component'
import { MessageComponent } from './chatbox/dashboard/messages/message.component'
import { CreateDialogComponent } from './chatbox/dashboard/create-dialog/create-dialog.component'
import { EditDialogComponent } from './chatbox/dashboard/edit-dialog/edit-dialog.component'
import { DeleteDialogComponent } from './chatbox/dashboard/delete-dialog/delete-dialog.component'
import { DropdownModule } from 'primeng/dropdown'
import { WeatherComponent } from './dash-board/weather/weather.component'
import { DashboardChatBotComponent } from './chatbot/chatbot.component'

@NgModule({
	imports: [
		CommonModule,
		BusinessRoutingModule,
		ReactiveFormsModule,
		FormsModule,
		AngularDualListBoxModule,
		SharedModule,
		BsDatepickerModule.forRoot(),
		NgSelectModule,
		MatCheckboxModule,
		TableModule,
		ScrollPanelModule,
		VirtualScrollerModule,
		DxTreeViewModule,
		DxDropDownBoxModule,
		DxDataGridModule,
		DxReportViewerModule,
		TreeviewModule.forRoot(),
		UserModule,
		DropdownModule,
		ChartModule,
	],
	declarations: [
		BusinessComponent,
		AppheaderComponent,
		AppfooterComponent,
		AppmenuComponent,
		DashboardComponent,
		NotificationComponent,
		RecommnendationGetListComponent,
		ReportViewerComponent,
		UserViewInfoComponent,
		UserCreateOrUpdateComponent,
		LoginChatBoxComponent,
		DashboardChatBoxComponent,
		DialogsComponent,
		MessageComponent,
		CreateDialogComponent,
		EditDialogComponent,
		DeleteDialogComponent,
		WeatherComponent,
		DashboardChatBotComponent
	],
	exports: [UserCreateOrUpdateComponent, UserViewInfoComponent],
})
export class BusinessModule { }
