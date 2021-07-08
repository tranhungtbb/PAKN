import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared/shared.module'
import { NgSelectModule } from '@ng-select/ng-select'
import { TableModule } from 'primeng/table'
import { ScrollPanelModule } from 'primeng/scrollpanel'
import { VirtualScrollerModule } from 'primeng/virtualscroller'
import { ReactiveFormsModule, FormsModule } from '@angular/forms'

import { RecommendationSyncRoutingModule } from './recommendation-sync-routing.module';
import { CongThongTinDienTuTinhComponent } from './cong-thong-tin-dien-tu-tinh/cong-thong-tin-dien-tu-tinh.component';
import { CongThongTinDvHccComponent } from './cong-thong-tin-dv-hcc/cong-thong-tin-dv-hcc.component';
import { HeThongPaknChinhPhuComponent } from './he-thong-pakn-chinh-phu/he-thong-pakn-chinh-phu.component';
import { ListRequestComponent } from './list-recommendation-knct/list-recommendation-knct.component'
import { DetailRecommendationComponent } from '../recommendation-sync/detail-recommendation-knct/detail-recommendation-knct.component'
import {DetailHeThongPAKNChinhPhuComponent} from './detail-he-thong-pakn-chinh-phu/detail-he-thong-pakn-chinh-phu.component'

@NgModule({
  declarations: [CongThongTinDienTuTinhComponent, CongThongTinDvHccComponent, HeThongPaknChinhPhuComponent,ListRequestComponent, DetailRecommendationComponent,DetailHeThongPAKNChinhPhuComponent],
  imports: [
    CommonModule,
    RecommendationSyncRoutingModule,
    TableModule,
    ScrollPanelModule,
    VirtualScrollerModule,
    ReactiveFormsModule,
    FormsModule,
    SharedModule,
    NgSelectModule
  ]
})
export class RecommendationSyncModule { }
