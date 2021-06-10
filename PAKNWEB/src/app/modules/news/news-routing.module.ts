import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'

import { NewsComponent } from './news.component'
import { NewsCreateOrUpdateComponent } from './news-create-or-update/news-create-or-update.component'
import { NewsDetailComponent } from './news-detail/news-detail.component'
import { NewsPuslishComponent } from './news-puslish/news-puslish.component'

const routes: Routes = [
	{ path: 'danh-sach-tong-hop', component: NewsComponent },
	{ path: 'them-moi', component: NewsCreateOrUpdateComponent },
	{ path: 'chinh-sua/:id', component: NewsCreateOrUpdateComponent },
	{ path: 'chi-tiet/:id', component: NewsDetailComponent },
	{ path: 'danh-sach-da-cong-bo', component: NewsPuslishComponent },
]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class NewsRoutingModule {}
