import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { RoleGuardService } from '../../guards/role-guard.service'
import { NewsComponent } from './news.component'
import { NewsCreateOrUpdateComponent } from './news-create-or-update/news-create-or-update.component'
import { NewsDetailComponent } from './news-detail/news-detail.component'
import { NewsPuslishComponent } from './news-puslish/news-puslish.component'

const routes: Routes = [
	{ 
		path: 'danh-sach-tong-hop',
		component: NewsComponent,
		canActivate: [RoleGuardService],
		data: { role: 'F_I_3'}
	},
	{ 
		path: 'them-moi',
		component: NewsCreateOrUpdateComponent,
		canActivate: [RoleGuardService],
		data: { role: 'F_I_0'} 
	},
	{ 
		path: 'chinh-sua/:id', 
		component: NewsCreateOrUpdateComponent,
		canActivate: [RoleGuardService],
		data: { role: 'F_I_1'}
	},
	{ 
		path: 'chi-tiet/:id',
		component: NewsDetailComponent,
		canActivate: [RoleGuardService],
		data: { role: 'F_I_4'}
	},
	{ 
		path: 'danh-sach-da-cong-bo',
		component: NewsPuslishComponent,
		canActivate: [RoleGuardService],
		data: { role: 'F_I_3'}
	},
]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class NewsRoutingModule {}

// *hasPermission="['3', 'F_I_0']"
