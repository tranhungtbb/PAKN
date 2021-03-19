import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ForbidenPageRoutingModule } from './forbiden-page-routing.module';
import { PageForbiddenComponent } from './page-forbidden/page-forbidden.component';
import { SharedModule } from '../../shared/shared.module';

@NgModule({
  imports: [
    CommonModule,
    ForbidenPageRoutingModule,
    SharedModule
  ],
  declarations: [PageForbiddenComponent]
})
export class ForbidenPageModule { }
