import { NgModule, Component } from '@angular/core'
import { CommonModule } from '@angular/common'
import { RouterModule, Routes } from '@angular/router'

// template 2
import { ListRecommendation2Component } from 'src/app/modules/template-v2/list-recommendation/list-recommendation.component'
import { DetailRecommendation2Component } from 'src/app/modules/template-v2/detail-recommendation/detail-recommendation.component'
import { ListNewsComponent } from './modules/template-v2/list-news/list-news.component'
import { DetailNewsComponent } from './modules/template-v2/detail-news/detail-news.component'

const routes: Routes = [
	{ path: '', redirectTo: 'cong-bo', pathMatch: 'full' },
	{ path: 'login', loadChildren: './modules/logins/logins.module#LoginsModule' },
	{ path: 'quan-tri', loadChildren: './modules/business.module#BusinessModule' },
	{ path: 'cong-bo', loadChildren: './modules/publish/publish.module#PublishModule' },
	{ path: 'dang-ky', loadChildren: './modules/register/register.module#RegisterModule' },
]

@NgModule({
	imports: [CommonModule, RouterModule.forRoot(routes)],
	declarations: [],
	exports: [RouterModule],
})
export class AppRoutingModule {}
