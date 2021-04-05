import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'

import { NewsComponent } from './news.component'
import { NewsCreateOrUpdateComponent } from './news-create-or-update/news-create-or-update.component'

const routes: Routes = [
	{ path: '', component: NewsComponent },
	{ path: 'them-moi', component: NewsCreateOrUpdateComponent },
	{ path: 'chinh-sua/:id', component: NewsCreateOrUpdateComponent },
]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class NewsRoutingModule {}
