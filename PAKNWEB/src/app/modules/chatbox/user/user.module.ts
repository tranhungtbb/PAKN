import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { UserServiceChatBox } from './user.service'
import { QBHelper } from '../helper/qbHelper'
import { Helpers } from '../helper/helpers'


@NgModule({
	declarations: [],
	imports: [CommonModule],
	providers: [UserServiceChatBox, QBHelper, Helpers],
})
export class UserModule {}
