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
import 'rxjs/add/operator/pairwise'
import { StoreLink } from '../constants/store-link'
import { environment } from '../../environments/environment'
import { UploadFileService } from '../services/uploadfiles.service'
import { saveAs as importedSaveAs } from 'file-saver'
import { DataService } from '../services/sharedata.service'
import { UserViewInfoComponent } from './system-management/components/user/user-view-info/user-view-info.component'
import { UserCreateOrUpdateComponent } from './system-management/components/user/user-create-or-update/user-create-or-update.component'

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

	constructor(
		private localeService: BsLocaleService,
		public userInfoService: UserInfoStorageService,
		public socketService: SocketService,
		private _router: Router,
		private sharedataService: DataService,
		private filesService: UploadFileService
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
						//this.sharedata.setobjectBack({});
					}
					environment.olderbacklink = e[0].url
					//var str: string = e[1].url;
					//this.sharedataService.setUrl(str);
				}
			})
	}

	@ViewChild(UserViewInfoComponent, { static: false }) child_UserViewInfoComponent: UserViewInfoComponent
	@ViewChild(UserCreateOrUpdateComponent, { static: false }) child_UserCreateOrUpdateComponent: UserCreateOrUpdateComponent

	ngOnInit() {
		this.localeService.use('vi')
		this.userInfoService.setReturnUrl('')

		// this.loadScript('assets/dist/vendor/global/global.min.js')
		this.loadScript('assets/dist/vendor/bootstrap-select/dist/js/bootstrap-select.min.js')
		this.loadScript('assets/dist/js/custom.min.js')
		this.loadScript('assets/dist/js/deznav-init.js')
		this.loadScript('assets/dist/vendor/waypoints/jquery.waypoints.min.js')
		this.loadScript('assets/dist/vendor/jquery.counterup/jquery.counterup.min.js')
		this.loadScript('assets/dist/vendor/apexchart/apexchart.js')
		this.loadScript('assets/dist/vendor/peity/jquery.peity.min.js')
		this.loadScript('assets/dist/js/plugins-init/piety-init.js')
		this.loadScript('assets/dist/js/dashboard/dashboard-1.js')
	}
	ngAfterViewInit() {
		$('#show-modal-account-info').click(() => {
			this.child_UserViewInfoComponent.openModal()
		})
		this.child_UserViewInfoComponent.parent_BusinessComponent = this
	}

	openModalEditInfo(userId: number) {
		this.child_UserCreateOrUpdateComponent.openModal(0, userId, true)
	}

	public loadScript(url: string) {
		$('script[src="' + url + '"]').remove()
		$('<script>').attr('src', url).appendTo('body')
	}
}
