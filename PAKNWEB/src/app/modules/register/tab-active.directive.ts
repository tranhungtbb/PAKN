import { Directive, OnInit, Input, ElementRef } from '@angular/core'
import { Router } from '@angular/router'

declare var $: any

@Directive({
	selector: '[appTabActive]',
})
export class TabActiveDirective implements OnInit {
	constructor(private router: Router, private el: ElementRef) {
		if (this.router.url.split('/').includes(this.appTabActive)) {
			$(this.el.nativeElement).addClass('active')
		} else {
			$(this.el.nativeElement).removeClass('active')
		}
	}

	@Input() appTabActive: string

	ngOnInit() {}
}
