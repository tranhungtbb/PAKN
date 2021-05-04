import { Component, ViewChild, AfterViewInit, Renderer2, Input, ElementRef, Inject, PLATFORM_ID, ViewEncapsulation } from '@angular/core'
import { isPlatformBrowser } from '@angular/common'
import * as ko from 'knockout'
// import { Html, DevExpress } from "devexpress-reporting/dx-web-document-viewer";
import { Router, ActivatedRoute, ParamMap } from '@angular/router'
import { UserInfoStorageService } from '../../commons/user-info-storage.service'
import { AppSettings } from '../../constants/app-setting'
import { DataService } from '../../services/sharedata.service'

// declare var jquery: any;
declare var $: any

@Component({
	selector: 'report-viewer',
	encapsulation: ViewEncapsulation.None,
	templateUrl: './report-viewcomponent.html',
	styleUrls: [
		'../../../../node_modules/jquery-ui/themes/base/all.css',
		'../../../../node_modules/devextreme/dist/css/dx.common.css',
		'../../../../node_modules/devextreme/dist/css/dx.light.css',
		'../../../../node_modules/@devexpress/analytics-core/dist/css/dx-analytics.common.css',
		'../../../../node_modules/@devexpress/analytics-core/dist/css/dx-analytics.light.css',
		'../../../../node_modules/devexpress-reporting/dist/css/dx-webdocumentviewer.css',
	],
})
export class ReportViewerComponent {
	constructor(
		private renderer: Renderer2,
		@Inject(PLATFORM_ID) private platformId: Object,
		private route: ActivatedRoute,
		private localStorage: UserInfoStorageService,
		private router: Router,
		private shareData: DataService
	) {}
	@ViewChild('scripts', { static: false })
	scripts: ElementRef

	@ViewChild('control', { static: false })
	control: ElementRef

	backLinkLoc: boolean = false
	objectsearch: any = {}
	paramsAdded: boolean = false
	// The URL of a report to open in the Report Designer when the application starts.
	public reportUrl: string = this.shareData.sendReportUrl
	// URI of your backend project.
	hostUrl: string = AppSettings.API_DOWNLOADFILES + '/'

	// Use this line if you use an ASP.NET MVC backend
	// invokeAction: string = '/WebDocumentViewer/Invoke';
	// Uncomment this line if you use an ASP.NET Core backend
	invokeAction: string = 'DXXRDV'

	backLink() {
		if (!this.backLinkLoc) {
			var linked = this.router.url.substring(0, this.router.url.indexOf('/report-view'))
			this.router.navigate(['..' + linked])
		} else {
			window.history.back()
		}
	}
}
