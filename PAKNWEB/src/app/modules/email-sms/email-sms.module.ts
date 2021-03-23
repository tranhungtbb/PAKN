import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { EmailSmsRoutingModule } from './email-sms-routing.module';
import { EmailSmsComponent } from './email-sms.component';

@NgModule({
  declarations: [EmailSmsComponent],
  imports: [
    CommonModule,
    EmailSmsRoutingModule
  ]
})
export class EmailSmsModule { }
