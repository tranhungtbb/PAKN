import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'

import { CallHistoryListComponent } from './call-history-list/call-history-list.component'

const routes: Routes = [{ path: 'danh-sach-cuoc-goi', component: CallHistoryListComponent }]

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class CallHistoryRoutingModule {}
