import { Directive, ElementRef, HostListener, Input } from '@angular/core'

@Directive({
	selector: '[svDisabledSpaceKey]',
})
export class DisabledSpaceKeyDirective {
	constructor(private el: ElementRef) {}

	@HostListener('keydown', ['$event']) onKeyDown(event) {
		let e = <KeyboardEvent>event
		let native: any = e.target
		let val: string = native.value

		if (e.keyCode == 32) {
			e.preventDefault()
		}
	}
}
