import { Component, OnInit, AfterViewChecked, ChangeDetectorRef, HostListener } from '@angular/core'
import { UserInfoStorageService } from './commons/user-info-storage.service'
import { Router, RouterStateSnapshot } from '@angular/router'
import { environment } from '../environments/environment'

// // declare var jquery: any;
declare var $: any

@Component({
	selector: 'app-root',
	templateUrl: './app.component.html',
	styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit, AfterViewChecked {
	// @HostListener('window:unload', ['$event'])
	// unloadHandler(event) {
	// 	const s = this.storeageService.getSaveLogin()
	// 	if (s == false && this.first == 1) {
	// 		localStorage.clear()
	// 	}
	// 	this.first++
	// }

	// @HostListener('window:beforeunload', ['$event'])
	// beforeUnloadHander(event) {
	// 	const s = this.storeageService.getIsSession()
	// 	if (s == undefined) {
	// 		this.storeageService.setIsSession(true)
	// 	} else {
	// 		if (this.storeageService.getSaveLogin() == false && s == false) {
	// 			this.storeageService.clear()
	// 		} else {
	// 			this.storeageService.setIsSession(false)
	// 		}
	// 	}
	// }
	first: number = 0
	env = environment
	showLoader: boolean
	isLogin: boolean
	state: RouterStateSnapshot
	constructor(private cdRef: ChangeDetectorRef, private storeageService: UserInfoStorageService, private _router: Router) {
		var check = this.storeageService.getAccessToken()
		if (check == '' || check == null || check == undefined) {
			this.isLogin = false
		} else {
			this.isLogin = true
		}
	}

	ngOnInit() {
		var currentlink = location.toString()
		var check = this.storeageService.getAccessToken()
		var typeUserLogin = this.storeageService.getTypeObject()
		if (check == '' || check == null || check == undefined) {
			this.isLogin = false
		} else {
			this.isLogin = true
		}
		if (this.isLogin) {
			var returnlURL = this.storeageService.getReturnUrl()
			if (returnlURL != undefined && returnlURL != '' && returnlURL != null && returnlURL != 'undefined') {
				return
			}
			if (typeUserLogin == 1) {
				if (!currentlink.includes('quan-tri') && !currentlink.includes('cong-bo')) {
					this._router.navigate(['/quan-tri/ban-lam-viec'])
					return
				}
			} else {
				if (!currentlink.includes('cong-bo')) {
					this._router.navigate(['/cong-bo/trang-chu'])
					return
				}
			}
		} else {
			var returnlURL = this.storeageService.getReturnUrl()
			if ((returnlURL == undefined || returnlURL == '' || returnlURL == null || returnlURL == 'undefined') && !currentlink.includes('dang-nhap')) {
				var urlback = this._router.url
				this.storeageService.setReturnUrl(urlback)
			}

			if (currentlink.includes('dang-ky')) {
				this._router.navigate(['/dang-ky'])
				return
			}

			if (!currentlink.includes('cong-bo') && !currentlink.includes('dang-nhap') && !currentlink.includes('quen-mat-khau')) {
				this._router.navigate(['/cong-bo/trang-chu'])
			}
		}
	}

	ngAfterViewChecked(): void {
		this.cdRef.detectChanges()
	}
}
