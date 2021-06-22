import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared/shared.module'

import { TableModule } from 'primeng/table'
import { ScrollPanelModule } from 'primeng/scrollpanel'
import { VirtualScrollerModule } from 'primeng/virtualscroller'
import { ReactiveFormsModule, FormsModule } from '@angular/forms'

import { RecommendationSyncRoutingModule } from './recommendation-sync-routing.module';
import { CongThongTinDienTuTinhComponent } from './cong-thong-tin-dien-tu-tinh/cong-thong-tin-dien-tu-tinh.component';
import { CongThongTinDvHccComponent } from './cong-thong-tin-dv-hcc/cong-thong-tin-dv-hcc.component';
import { HeThongQuanLyKienNghiCuTriComponent } from './he-thong-quan-ly-kien-nghi-cu-tri/he-thong-quan-ly-kien-nghi-cu-tri.component';
import { HeThongPaknChinhPhuComponent } from './he-thong-pakn-chinh-phu/he-thong-pakn-chinh-phu.component';

@NgModule({
  declarations: [CongThongTinDienTuTinhComponent, CongThongTinDvHccComponent, HeThongQuanLyKienNghiCuTriComponent, HeThongPaknChinhPhuComponent],
  imports: [
    CommonModule,
    RecommendationSyncRoutingModule,
    TableModule,
    ScrollPanelModule,
    VirtualScrollerModule,
    ReactiveFormsModule,
    FormsModule,
    SharedModule
  ]
})
export class RecommendationSyncModule { }
