import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { BusinessComponent } from './business.component'
import { LoginChatBoxComponent } from './chatbox/user/login/login.component'
import { DashboardComponent } from './dash-board/dash-board.component'
import { NotificationComponent } from './notification/notification.component'
import { ReportViewerComponent } from './report-view/report-viewcomponent'
import { DashboardChatBoxComponent } from './chatbox/dashboard/dashboard.component'
import { DashboardChatBotComponent } from './chatbot/chatbot.component'

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
			{ path: 'huong-dan-su-dung', loadChildren: './support-management/support-management.module#SupportManagementModule' },
			{ path: 'thu-tuc-hanh-chinh', loadChildren: './administrative-formalities/administrative-formalities.module#AdministrativeFormalitiesModule' },
			{ path: 'thong-bao', component: NotificationComponent },
			{ path: 'xuat-file', component: ReportViewerComponent },
			{ path: 'bao-cao', loadChildren: './statistic/statistic.module#StatisticModule' },
			{ path: 'cuoc-goi', loadChildren: 'src/app/modules/call-history/call-history.module#CallHistoryModule' },
			{ path: 'dong-bo-du-lieu', loadChildren: 'src/app/modules/recommendation-sync/recommendation-sync.module#RecommendationSyncModule' },
			{ path: 'tin-nhan', component: LoginChatBoxComponent },
			{ path: 'chatbox', component: DashboardChatBoxComponent },
			{ path: 'chat-bot', component: DashboardChatBotComponent },
			{ path: 'chat-bot/:roomId', component: DashboardChatBotComponent },
		],
	},
]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class BusinessRoutingModule {}
