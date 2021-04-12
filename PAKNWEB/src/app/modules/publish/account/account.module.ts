import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'

import { AccountRoutingModule } from './account-routing.module';
import { AccountInfoComponent } from './account-info/account-info.component'

@NgModule({
	declarations: [AccountInfoComponent],
	imports: [CommonModule, AccountRoutingModule],
})
export class AccountModule {}
