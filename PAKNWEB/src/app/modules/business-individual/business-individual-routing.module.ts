import { NgModule } from '@angular/core'
import { RoleGuardService } from '../../guards/role-guard.service'
import { Routes, RouterModule } from '@angular/router'
import { BusinessIndividualComponent } from './business-individual.component'
import { IndividualComponent } from './components/individual/individual.component'
import { BusinessComponent } from './components/business/business.component'
import { CreateUpdBusinessComponent } from './components/create-upd-business/create-upd-business.component'

const routes: Routes = [
	{
		path: '',
		component: BusinessIndividualComponent,
		children: [
			{
				path: 'ca-nhan',
				component: IndividualComponent,
				canActivate: [RoleGuardService],
				data: { role: 'A_XVII_3' }
			},
			{ 
				path: 'doanh-nghiep',
				component: BusinessComponent,
				canActivate: [RoleGuardService],
				data: { role: 'A_XVII_3' }
			},
			{ 
				path: 'them-moi/:id',
				component: CreateUpdBusinessComponent,
				canActivate: [RoleGuardService],
				data: { role: 'A_XVII_3' }
			},
		],
	},
]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class BusinessIndividualRoutingModule {}
