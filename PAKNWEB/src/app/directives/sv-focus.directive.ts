import { Directive, AfterViewInit, ElementRef, Renderer } from '@angular/core'

@Directive({
	selector: '[appSvFocus]',
})
export class SvFocusDirective implements AfterViewInit {
	constructor(public renderer: Renderer, private el: ElementRef) {}

	ngAfterViewInit() {
		this.renderer.invokeElementMethod(this.el.nativeElement, 'focus', [])
	}
}

// import { Directive, AfterViewInit, ElementRef } from '@angular/core'

// @Directive({
// 	selector: '[appSvFocus]',
// })
// export class SvFocusDirective implements AfterViewInit {
// 	constructor(private host: ElementRef) {}

// 	ngAfterViewInit() {
// 			this.host.nativeElement.focus()
// 	}
// }
