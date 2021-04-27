import { Component, OnInit } from '@angular/core'

import { Router } from '@angular/router'
import { ToastrService } from 'ngx-toastr'
import { AccountService } from 'src/app/services/account.service'

import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { AuthenticationService } from 'src/app/services/authentication.service'
import { DataService } from 'src/app/services/sharedata.service'
import { DiadanhService } from 'src/app/services/diadanh.service'
import { UserInfoObject } from 'src/app/models/UserObject'

@Component({
	selector: 'app-account-side-left',
	templateUrl: './account-side-left.component.html',
	styleUrls: ['./account-side-left.component.css'],
})
export class AccountSideLeftComponent implements OnInit {
	constructor(
		private toast: ToastrService,
		private router: Router,
		private accountService: AccountService,
		private storageService: UserInfoStorageService,
		private authenService: AuthenticationService,
		private sharedataService: DataService,
		private diadanhService: DiadanhService
	) {}
	model: any = { userName: '' }
	ngOnInit() {}

	signOut(): void {
		this.authenService.logOut({}).subscribe((success) => {
			if (success.success == RESPONSE_STATUS.success) {
				this.sharedataService.setIsLogin(false)
				this.storageService.setReturnUrl('')
				this.storageService.clearStoreage()
				this.router.navigate(['/dang-nhap'])
				//location.href = "/dang-nhap";
			}
		})
	}
}
