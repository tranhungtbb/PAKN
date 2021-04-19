import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { BusinessComponent } from './business.component'
import { DashboardComponent } from './dash-board/dash-board.component'
import { NotificationComponent } from './notification/notification.component'

const routes: Routes = [
	{
		path: '',
		component: BusinessComponent,
		children: [
			{ path: '', redirectTo: 'ban-lam-viec' },
			{ path: 'ban-lam-viec', component: DashboardComponent },
			{ path: 'he-thong', loadChildren: './system-management/system-management.module#SystemManagementModule' },
			{ path: 'danh-muc', loadChildren: './catalog-management/catalog-management.module#CatalogManagementModule' },
			{ path: 'kien-nghi', loadChildren: './recommendation/recommendation.module#RecommendationModule' },
			{ path: 'hanh-chinh', loadChildren: './administration/administration.module#AdministrationModule' },
			{ path: 'tin-tuc', loadChildren: './news/news.module#NewsModule' },
			{ path: 'ca-nhan-doanh-nghiep', loadChildren: './business-individual/business-individual.module#BusinessIndividualModule' },
			{ path: 'thu-moi', loadChildren: './invitation/invitation.module#InvitationModule' },
			{ path: 'email-sms', loadChildren: './email-sms/email-sms.module#EmailSmsModule' },
			{ path: 'thong-ke', loadChildren: './statistic/statistic.module#StatisticModule' },
			{ path: 'bao-cao', loadChildren: './report/report.module#ReportModule' },
			{ path: 'thiet-lap-chung', loadChildren: './setting/setting.module#SettingModule' },
			{ path: 'ho-tro', loadChildren: './support/support.module#SupportModule' },
			{ path: 'thu-tuc-hanh-chinh', loadChildren: './administrative-formalities/administrative-formalities.module#AdministrativeFormalitiesModule' },
			{ path: 'thong-bao', component: NotificationComponent },
		],
	},
]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class BusinessRoutingModule {}
