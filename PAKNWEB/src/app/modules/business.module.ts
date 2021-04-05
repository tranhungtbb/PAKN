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
import { DashboardComponent } from './dash-board/dash-board.component'

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
	],
	declarations: [BusinessComponent, AppheaderComponent, AppfooterComponent, AppmenuComponent, DashboardComponent],
})
export class BusinessModule {}
