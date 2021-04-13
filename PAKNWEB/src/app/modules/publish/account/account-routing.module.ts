import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { AccountInfoComponent } from './account-info/account-info.component'

const routes: Routes = [
	{ path: 'thong-tin', component: AccountInfoComponent },
	{ path: 'thay-doi-mat-khau', component: AccountInfoComponent },
]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class AccountRoutingModule {}
