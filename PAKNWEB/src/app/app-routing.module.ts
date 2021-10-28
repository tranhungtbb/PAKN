import { NgModule, Component } from '@angular/core'
import { CommonModule } from '@angular/common'
import { RouterModule, Routes } from '@angular/router'
import { Index2Component } from 'src/app/modules/index2/index2.component'

const routes: Routes = [
	{ path: '', redirectTo: 'cong-bo', pathMatch: 'full' },
	{ path: 'login', loadChildren: './modules/logins/logins.module#LoginsModule' },
	{ path: 'quan-tri', loadChildren: './modules/business.module#BusinessModule' },
	{ path: 'cong-bo', loadChildren: './modules/publish/publish.module#PublishModule' },
	{ path: 'dang-ky', loadChildren: './modules/register/register.module#RegisterModule' },
	{
		path: 'cong-bo/trang-chu2',
		component: Index2Component,
	},
]

@NgModule({
	imports: [CommonModule, RouterModule.forRoot(routes)],
	declarations: [],
	exports: [RouterModule],
})
export class AppRoutingModule {}
