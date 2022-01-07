import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core'
// declare var jquery: any;
declare var $: any
import { BsDatepickerConfig, BsLocaleService } from 'ngx-bootstrap/datepicker'
import { defineLocale } from 'ngx-bootstrap/chronos'
import { viLocale } from 'ngx-bootstrap/locale'
import { UserInfoStorageService } from '../commons/user-info-storage.service'
import { SocketService } from '../services/socket.service'
import { Router, NavigationEnd } from '@angular/router'
import { filter } from 'rxjs/operators'
import { StoreLink } from '../constants/store-link'
import { environment } from '../../environments/environment'
import { MetaService } from '../services/tag-meta.service'
import { IndexSettingService } from '../services/index-setting.service'
import { RESPONSE_STATUS } from '../constants/CONSTANTS'

defineLocale('vi', viLocale)

declare var $: any
@Component({
	selector: 'app-business',
	templateUrl: './business.component.html',
	styleUrls: ['./business.component.css'],
})
export class BusinessComponent implements OnInit, AfterViewInit {
	userId: number
	url: string = ''
	currentRouter: string = ''
	isMain: any = this.userInfoService.getIsMain()

	constructor(
		private localeService: BsLocaleService,
		public userInfoService: UserInfoStorageService,
		public socketService: SocketService,
		private _router: Router,
		private metaService: MetaService,
		private indexSetting: IndexSettingService
	) {
		this._router.events
			.pipe(filter((event) => event instanceof NavigationEnd))
			.pairwise()
			.subscribe((e: any[]) => {
				if (e != undefined && e != null && e.length > 1) {
					var linkolder = environment.olderbacklink
					var backlink = false
					var currentlink = false

					for (let sp of StoreLink.ListBackLink) {
						if (e[0].url.includes(sp)) {
							backlink = true
						}
						if (e[1].url.includes(sp)) {
							currentlink = true
						}
					}
					if ((backlink && currentlink) || (environment.olderbacklink !== e[1].url && currentlink)) {
					}
					environment.olderbacklink = e[0].url
				}
			})
	}


	ngOnInit() {
		this.localeService.use('vi')
		this.userInfoService.setReturnUrl('')
		this.indexSetting.GetInfo({}).subscribe(res => {
			if (res.success == RESPONSE_STATUS.success) {
				let settingModel = res.result.model
				this.metaService.updateTitle(settingModel.metaTitle)
				this.metaService.updateDescription(settingModel.metaDescription)
			}
		})
		// this.loadScript('assets/dist/vendor/global/global.min.js')
		// this.loadScript('assets/dist/vendor/bootstrap-select/dist/js/bootstrap-select.min.js')
		//this.loadScript('assets/dist/js/custom.min.js')
		//this.loadScript('assets/dist/js/deznav-init.js')
		// this.loadScript('assets/dist/vendor/waypoints/jquery.waypoints.min.js')
		// this.loadScript('assets/dist/vendor/jquery.counterup/jquery.counterup.min.js')
		// this.loadScript('assets/dist/vendor/apexchart/apexchart.js')
		// this.loadScript('assets/dist/vendor/peity/jquery.peity.min.js')
		// this.loadScript('assets/dist/js/plugins-init/piety-init.js')
		// this.loadScript('assets/dist/js/dashboard/dashboard-1.js')
	}
	ngAfterViewInit() {
		this.currentRouter = this._router.url
		setTimeout(() => {
			var widthMenu = $(".header-right").width();
			var css = '.group-button-table { right: ' + (widthMenu + 25) + 'px; }',
				head = document.head || document.getElementsByTagName('head')[0],
				style = document.createElement('style');

			head.appendChild(style);
			style.appendChild(document.createTextNode(css));
		}, 500)
	}
	public loadScript(url: string) {
		$('script[src="' + url + '"]').remove()
		$('<script>').attr('src', url).appendTo('body')
	}
}
