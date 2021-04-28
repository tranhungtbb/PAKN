import { Component, ViewChild, AfterViewInit, Renderer2, Input, ElementRef, Inject, PLATFORM_ID, ViewEncapsulation } from '@angular/core'
import { isPlatformBrowser } from '@angular/common'
import * as ko from 'knockout'
// import { Html, DevExpress } from "devexpress-reporting/dx-web-document-viewer";
import { Html, DevExpress } from 'devexpress-reporting/dx-webdocumentviewer'
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
		//"../../../../node_modules/devexpress-reporting/css/web-document-viewer-light.min.css",
		'../../../../node_modules/devexpress-reporting/dist/css/dx-webdocumentviewer.css',
	],
})
export class ReportViewerComponent implements AfterViewInit {
	koReportUrl = ko.observable(null)
	_reportUrl
	model: any = {}
	@ViewChild('scripts', { static: false })
	scripts: ElementRef

	@ViewChild('control', { static: false })
	control: ElementRef

	backLinkLoc: boolean = false
	objectsearch: any = {}

	constructor(
		private renderer: Renderer2,
		@Inject(PLATFORM_ID) private platformId: Object,
		private route: ActivatedRoute,
		private localStorage: UserInfoStorageService,
		private router: Router,
		private shareData: DataService
	) {
		this.shareData.getobjectReport.subscribe((data) => {
			this.objectsearch = data
		})
	}

	ngAfterViewInit() {
		if (isPlatformBrowser(this.platformId)) {
			this.route.params.subscribe((params) => {
				this.model.module = params['module']
			})
			if (this.model.module == 'Recommendation_ListGeneral') {
				this.backLinkLoc = true
				var url = 'Recommendation_ListGeneral?' + JSON.stringify(this.objectsearch)
			} else if (this.model.module == 'ExportKienNghiCuTri') {
				this.backLinkLoc = true
				var url = 'ExportKienNghiCuTri?' + JSON.stringify(this.objectsearch)
			}
			const reportUrl = ko['observable'](url),
				host = AppSettings.API_DOWNLOADFILES,
				container = this.renderer.createElement('div')

			container.innerHTML = Html
			this.renderer.appendChild(this.scripts.nativeElement, container)
			ko.applyBindings(
				{
					reportUrl: reportUrl, // The URL of a report that is opened in the Document Viewer when the application starts.
					requestOptions: {
						// Options for processing requests from the Web Document Viewer.
						host: host, // URI of your backend project.
						invokeAction: '/DXXRDV', // The URI path of the controller action that processes requests.
					},
				},
				this.control.nativeElement
			)
		}
	}

	@Input()
	set reportUrl(reportUrl: string) {
		this._reportUrl = reportUrl
		this.koReportUrl(reportUrl)
	}
	get reportUrl() {
		return this._reportUrl
	}

	backLink() {
		if (!this.backLinkLoc) {
			var linked = this.router.url.substring(0, this.router.url.indexOf('/report-view'))
			this.router.navigate(['..' + linked])
		} else {
			window.history.back()
		}
	}
}
