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
		// this.activatedRoute.queryParams.subscribe((params: Params) => {
		// 	let type = params['type']
		// 	if (!type) {
		// 		this.router.navigate(['/cong-bo/ho-tro/2'])
		// 	}
		// })
	}

	listDoc: any[] = []

	model: any = {
		src: '',
		date: new Date(),
	}
	contentType = 0
	title = 'người dân'

	@ViewChild('docView', { static: true }) docView: ElementRef

	ngOnInit() {
		this._PuSupportService.getData({}).subscribe((res) => {
			if (res) {
				this.listDoc = res.result.ListData
				this.loadDocView(this.contentType, this.title)
			}
		})

		// this.model = {
		// 	createdDate: new Date(),
		// 	contents: `Lorem ipsum dolor sit amet consectetur adipisicing elit. Debitis vero earum a perspiciatis expedita, amet totam, cumque ullam tempore exercitationem
		//   alias deserunt quo nesciunt accusamus rerum obcaecati autem nisi quae.
		//   Lorem ipsum dolor sit amet consectetur adipisicing elit. Rerum fugit quisquam optio quo culpa saepe veritatis assumenda aperiam, repudiandae laudantium
		//   praesentium, debitis adipisci provident, minus inventore consectetur modi! Ipsum, animi.
		//   Lorem ipsum, dolor sit amet consectetur adipisicing elit. Animi explicabo voluptas a minima sit eveniet, dolorem fugit, dolorum itaque odit qui tenetur
		//   expedita nemo natus iste facere numquam quod repellat.`,
		// }
	}
	changeDoc() {
		this.loadDocView(this.contentType, this.title)
	}
	currentFileName: string = ''
	loadDocView(contentType: number, title: string) {
		let item = this.listDoc.find((c) => c.category == contentType && c.title.toLowerCase() == title.toLowerCase())
		this.model = { ...item }
		this.model.src = `http://14.177.236.88:6160/${item.filePath}`
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
