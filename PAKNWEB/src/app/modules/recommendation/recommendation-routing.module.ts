import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { ListGeneralComponent } from './list-general/list-general.component'
import { RecommendationComponent } from './recommendation.component'

const routes: Routes = [
	{
		path: '',
		component: RecommendationComponent,
		children: [{ path: 'danh-sach-tong-hop', component: ListGeneralComponent }],
	},
]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class RecommendationRoutingModule {}
