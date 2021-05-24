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

import { DashboardComponent } from './dash-board/dash-board.component'
import { NotificationComponent } from './notification/notification.component'
import { RecommnendationGetListComponent } from './dash-board/recommnendation-get-list/recommnendation-get-list.component'
import { ReportViewerComponent } from './report-view/report-viewcomponent'
import { UserViewInfoComponent } from './system-management/components/user/user-view-info/user-view-info.component'
import { UserCreateOrUpdateComponent } from './system-management/components/user/user-create-or-update/user-create-or-update.component'
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
	],
	exports: [UserCreateOrUpdateComponent, UserViewInfoComponent],
})
export class BusinessModule {}
