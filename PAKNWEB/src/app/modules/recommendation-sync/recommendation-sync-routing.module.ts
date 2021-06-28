import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router';
import { RoleGuardService } from '../../guards/role-guard.service'
import { CongThongTinDienTuTinhComponent } from './cong-thong-tin-dien-tu-tinh/cong-thong-tin-dien-tu-tinh.component';
import { CongThongTinDvHccComponent } from './cong-thong-tin-dv-hcc/cong-thong-tin-dv-hcc.component';
import { HeThongQuanLyKienNghiCuTriComponent } from './he-thong-quan-ly-kien-nghi-cu-tri/he-thong-quan-ly-kien-nghi-cu-tri.component';
import { HeThongPaknChinhPhuComponent } from './he-thong-pakn-chinh-phu/he-thong-pakn-chinh-phu.component';


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
    component:HeThongQuanLyKienNghiCuTriComponent,
    canActivate: [RoleGuardService],
    data: { role: 'I_II_0' }
  },
  {
    path:'he-thong-pakn-chinh-phu',
    component:HeThongPaknChinhPhuComponent,
    canActivate: [RoleGuardService],
    data: { role: 'I_III_0' }
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RecommendationSyncRoutingModule { }
