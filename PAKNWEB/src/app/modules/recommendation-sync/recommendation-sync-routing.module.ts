import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router';

import { CongThongTinDienTuTinhComponent } from './cong-thong-tin-dien-tu-tinh/cong-thong-tin-dien-tu-tinh.component';
import { CongThongTinDvHccComponent } from './cong-thong-tin-dv-hcc/cong-thong-tin-dv-hcc.component';
import { HeThongQuanLyKienNghiCuTriComponent } from './he-thong-quan-ly-kien-nghi-cu-tri/he-thong-quan-ly-kien-nghi-cu-tri.component';
import { HeThongPaknChinhPhuComponent } from './he-thong-pakn-chinh-phu/he-thong-pakn-chinh-phu.component';


const routes: Routes = [
  {path:'cong-thong-tin-dien-tu-tinh',component:CongThongTinDienTuTinhComponent},
  {path:'cong-thong-tin-dv-hcc',component:CongThongTinDvHccComponent},
  {path:'he-thong-quan-ly-kien-nghi-cu-tri',component:HeThongQuanLyKienNghiCuTriComponent},
  {path:'he-thong-pakn-chinh-phu',component:HeThongPaknChinhPhuComponent},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RecommendationSyncRoutingModule { }
