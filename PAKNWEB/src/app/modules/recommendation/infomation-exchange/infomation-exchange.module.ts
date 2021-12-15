import { NgModule } from '@angular/core'
import { BrowserModule } from '@angular/platform-browser'
import { FormsModule } from '@angular/forms'
import { CommonModule } from '@angular/common'
import { InfomationExchangeComponent } from './infomation-exchange.component'
import { InfomationExchangeReplyComponent } from '../infomation-exchange-reply/infomation-exchange-reply.component'

@NgModule({
  declarations: [InfomationExchangeComponent, InfomationExchangeReplyComponent
  ],
  imports: [CommonModule, FormsModule],
  exports: [InfomationExchangeComponent, InfomationExchangeReplyComponent]
})
export class InfomationExchangeModule { }
