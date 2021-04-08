import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { OrganizationComponent } from './organization/organization.component'
import { IndividualComponent } from './individual/individual.component'
import { RegisterComponent } from './register.component'

const routes: Routes = [
	{
		path: '',
		component: RegisterComponent,
		children: [
			{ path: '', redirectTo: 'ca-nhan' },
			{ path: 'ca-nhan', component: IndividualComponent },
			{ path: 'doanh-nghiep', component: OrganizationComponent },
		],
	},
]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class RegisterRoutingModule {}
