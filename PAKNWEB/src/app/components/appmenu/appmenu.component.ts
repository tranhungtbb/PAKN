import { AfterViewInit, Component, OnInit } from '@angular/core'
import { UserInfoStorageService } from '../../commons/user-info-storage.service'
import { HasPermission } from 'src/app/guards/has-permission.service'

declare var $: any
import { Router } from '@angular/router'

@Component({
	selector: 'app-appmenu',
	templateUrl: './appmenu.component.html',
	styleUrls: ['./appmenu.component.css'],
})
export class AppmenuComponent implements OnInit, AfterViewInit {
	isSuperAdmin: boolean = false
	constructor(private userStorage: UserInfoStorageService, private _router: Router, private hasPermission: HasPermission) {}
	isMainMenu: boolean = false
	isAdmin = this.userStorage.getIsAdmin()
	ngOnInit() {
		this.isMainMenu = this.userStorage.getIsUnitMain()
	}
	ngAfterViewInit() {
		this.loadScriptMenus('assets/dist/js/custom.min.js')
		this.loadScriptMenus('assets/dist/js/deznav-init.js')
	}
	public loadScriptMenus(url: string) {
		$('script[src="' + url + '"]').remove()
		$('<script>').attr('src', url).appendTo('body')
	}
}
