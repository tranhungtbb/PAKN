import { NgModule } from '@angular/core'
import { BrowserModule } from '@angular/platform-browser'
import { FormsModule } from '@angular/forms'
import { CommonModule } from '@angular/common'
import { InfomationExchangeComponent } from './infomation-exchange.component'
@NgModule({
  declarations: [InfomationExchangeComponent,
  ],
  imports: [CommonModule, FormsModule],
  exports: [InfomationExchangeComponent]
})
export class InfomationExchangeModule { }
