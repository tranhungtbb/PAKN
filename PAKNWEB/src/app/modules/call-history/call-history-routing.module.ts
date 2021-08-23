import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { RoleGuardService } from '../../guards/role-guard.service'

import { CallHistoryListComponent } from './call-history-list/call-history-list.component'

const routes: Routes = [{ path: 'danh-sach-cuoc-goi', component: CallHistoryListComponent ,canActivate: [RoleGuardService],
data: { role: 'A_XX_0'},}]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class CallHistoryRoutingModule {}
