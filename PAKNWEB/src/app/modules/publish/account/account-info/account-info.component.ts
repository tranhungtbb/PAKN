import { Component, OnInit } from '@angular/core'
import { Router } from '@angular/router'
import { ToastrService } from 'ngx-toastr'
import { AccountService } from 'src/app/services/account.service'

import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { AuthenticationService } from 'src/app/services/authentication.service'
import { DataService } from 'src/app/services/sharedata.service'

import { UserInfoObject } from 'src/app/models/UserObject'
@Component({
	selector: 'app-account-info',
	templateUrl: './account-info.component.html',
	styleUrls: ['./account-info.component.css'],
})
export class AccountInfoComponent implements OnInit {
	constructor(
		private toast: ToastrService,
		private router: Router,
		private accountService: AccountService,
		private storageService: UserInfoStorageService,
		private authenService: AuthenticationService,
		private sharedataService: DataService
	) {}

	model: UserInfoObject = new UserInfoObject()

	ngOnInit() {
		this.getUserInfo()

		if (this.router.url.includes('thong-tin')) {
			this.viewVisiable = 'info'
		} else if (this.router.url.includes('thay-doi-mat-khau')) {
			this.viewVisiable = 'pwd'
		} else if (this.router.url.includes('chinh-sua-thong-tin')) {
			this.viewVisiable = 'edit'
		}
	}

	viewVisiable: string = 'info' //info, pwd, edit

	getUserInfo() {
		this.accountService.getUserInfo().subscribe((res) => {
			if (res.success != 'OK') {
				this.toast.error(res.message)
				return
			}
			this.model = res.result
		})
	}
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
