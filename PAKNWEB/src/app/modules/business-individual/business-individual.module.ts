import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BusinessIndividualRoutingModule } from './business-individual-routing.module';
import { BusinessIndividualComponent } from './business-individual.component';

@NgModule({
  declarations: [BusinessIndividualComponent],
  imports: [
    CommonModule,
    BusinessIndividualRoutingModule
  ]
})
export class BusinessIndividualModule { }
