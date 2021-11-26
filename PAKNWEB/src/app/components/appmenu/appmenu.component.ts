import { AfterViewInit, Component, OnInit } from '@angular/core'
import { UserInfoStorageService } from '../../commons/user-info-storage.service'
import { HasPermission } from 'src/app/guards/has-permission.service'
import { UnitService } from 'src/app/services/unit.service'

declare var $: any
import { Router } from '@angular/router'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'

@Component({
	selector: 'app-appmenu',
	templateUrl: './appmenu.component.html',
	styleUrls: ['./appmenu.component.css'],
})
export class AppmenuComponent implements OnInit, AfterViewInit {
	isSuperAdmin: boolean = false
	constructor(private userStorage: UserInfoStorageService, private _router: Router, private unitService : UnitService) {}
	isMainMenu: boolean = false
	isAdmin = this.userStorage.getIsAdmin()
	hasPermissionSMS : boolean = false
	ngOnInit() {
		this.isMainMenu = this.userStorage.getIsUnitMain()
		this.unitService.hasPermissionSMS({}).subscribe(res =>{
			if(res.success == RESPONSE_STATUS.success){
				this.hasPermissionSMS = res.result
			}
		})
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
