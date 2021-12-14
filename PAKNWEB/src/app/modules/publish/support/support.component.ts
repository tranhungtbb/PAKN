import { Component, OnInit, AfterViewInit, ViewChild, ElementRef } from '@angular/core'
import { Router, ActivatedRoute, Params } from '@angular/router'
import { PuSupportService } from 'src/app/services/pu-support.service'
import { DomSanitizer, SafeUrl } from '@angular/platform-browser'

@Component({
	selector: 'app-support',
	templateUrl: './support.component.html',
	styleUrls: ['./support.component.css'],
})
export class SupportComponent implements OnInit, AfterViewInit {
	constructor(private router: Router, private activatedRoute: ActivatedRoute, private _PuSupportService: PuSupportService, private sanitizer: DomSanitizer) {
		
	}

	listDoc: any[] = []

	model: any = {
		src: '',
		date: new Date(),
	}
	contentType = 0
	type = 2

	@ViewChild('docView', { static: true }) docView: ElementRef

	ngOnInit() {
		this._PuSupportService.getData({}).subscribe((res) => {
			if (res) {
				this.listDoc = res.result.ListData
				this.loadDocView(this.contentType, this.type)
			}
		})
	}
	changeDoc() {
		this.loadDocView(this.contentType, this.type)
	}
	currentFileName: string = ''
	loadDocView(contentType: number, type: number) {
		this.type = type
		let item = this.listDoc.find((c) => c.category == contentType && c.type == type)
		this.model = { ...item }
		this.model.src = item.filePath
		let splString = item.filePath.split('/')
		this.currentFileName = splString[splString.length - 1]
	}
	safe(url: string) {
		return this.sanitizer.bypassSecurityTrustResourceUrl(url)
	}
	ngAfterViewInit() {
		//docView.src =
	}
}
