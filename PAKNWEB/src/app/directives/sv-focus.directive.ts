import { Directive, AfterViewInit, ElementRef, Renderer2 } from '@angular/core'

@Directive({
	selector: '[appSvFocus]',
})
export class SvFocusDirective implements AfterViewInit {
	constructor(public renderer: Renderer2, private el: ElementRef) {}

	ngAfterViewInit() {
		setTimeout(() => {
			this.renderer.insertBefore(this.el.nativeElement, 'focus', [])
		}, 500)
	}
}

// import { Directive, AfterViewInit, ElementRef } from '@angular/core'

// @Directive({
// 	selector: '[appSvFocus]',
// })
// export class SvFocusDirective implements AfterViewInit {
// 	constructor(private host: ElementRef) {}

// 	ngAfterViewInit() {
// 		window.setTimeout(() => {
// 			this.host.nativeElement.focus()
// 		})
// 	}
// }

// import { Directive, ElementRef, OnInit, AfterViewInit } from '@angular/core'

// @Directive({
// 	selector: '[appSvFocus]',
// })
// export class SvFocusDirective implements AfterViewInit {
// 	constructor(private el: ElementRef) {
// 		if (!el.nativeElement['focus']) {
// 			throw new Error('Element does not accept focus.')
// 		}
// 	}

// 	ngAfterViewInit(): void {
// 		const input: HTMLInputElement = this.el.nativeElement as HTMLInputElement
// 		input.focus()
// 		input.select()
// 	}
// }

// import { AfterContentInit, Directive, ElementRef, Input } from '@angular/core'

// @Directive({
// 	selector: '[appSvFocus]',
// })
// export class SvFocusDirective implements AfterContentInit {
// 	@Input() public appAutoFocus: boolean

// 	public constructor(private el: ElementRef) {}

// 	public ngAfterContentInit() {
// 		setTimeout(() => {
// 			this.el.nativeElement.focus()
// 		}, 500)
// 	}
// }
