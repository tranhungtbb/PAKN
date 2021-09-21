import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { AccountInfoComponent } from './account-info/account-info.component'
import { BusinessUpdateInfoComponent } from './business-update-info/business-update-info.component'

const routes: Routes = [
	{ path: 'thong-tin', component: AccountInfoComponent },
	{ path: 'thay-doi-mat-khau', component: AccountInfoComponent },
	{ path: 'chinh-sua-thong-tin', component: AccountInfoComponent },
	{ path: 'thong-tin-doanh-nghiep', component: BusinessUpdateInfoComponent },
	
]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class AccountRoutingModule {}
