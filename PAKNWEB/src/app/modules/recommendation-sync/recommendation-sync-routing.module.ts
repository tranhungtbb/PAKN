import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router';
import { RoleGuardService } from '../../guards/role-guard.service'
import { CongThongTinDienTuTinhComponent } from './cong-thong-tin-dien-tu-tinh/cong-thong-tin-dien-tu-tinh.component';
import { CongThongTinDvHccComponent } from './cong-thong-tin-dv-hcc/cong-thong-tin-dv-hcc.component';
import { HeThongPaknChinhPhuComponent } from './he-thong-pakn-chinh-phu/he-thong-pakn-chinh-phu.component';
import { ListRequestComponent } from './list-recommendation-knct/list-recommendation-knct.component'
import { DetailRecommendationComponent } from '../recommendation-sync/detail-recommendation-knct/detail-recommendation-knct.component'
import {DetailHeThongPAKNChinhPhuComponent} from './detail-he-thong-pakn-chinh-phu/detail-he-thong-pakn-chinh-phu.component'


const routes: Routes = [
  {
    path:'cong-thong-tin-dien-tu-tinh',
    component:CongThongTinDienTuTinhComponent,
    canActivate: [RoleGuardService],
    data: { role: 'I_I_0' }
  },
  {
    path:'cong-thong-tin-dv-hcc',
    component:CongThongTinDvHccComponent,
    canActivate: [RoleGuardService],
    data: { role: 'I_IV_0' }
  },
  { path:'he-thong-quan-ly-kien-nghi-cu-tri',
    component:ListRequestComponent,
    canActivate: [RoleGuardService],
    data: { role: 'I_II_0' }
  },
  {
    path:'he-thong-pakn-chinh-phu',
    component:HeThongPaknChinhPhuComponent,
    canActivate: [RoleGuardService],
    data: { role: 'I_III_0' }
  },
  {
    path:'he-thong-pakn-chinh-phu/:id',
    component:DetailHeThongPAKNChinhPhuComponent,
    canActivate: [RoleGuardService],
    data: { role: 'I_III_1' }
  },
  
  { 
    path: 'he-thong-quan-ly-kien-nghi-cu-tri/:id',
    component: DetailRecommendationComponent,
    canActivate: [RoleGuardService],
    data: { role: 'E_XIII_1' }
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RecommendationSyncRoutingModule { }
