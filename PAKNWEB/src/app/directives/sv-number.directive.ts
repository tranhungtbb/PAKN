import { Directive, ElementRef, HostListener, Input } from '@angular/core';
// import { NgControl } from '@angular/forms';

@Directive({
  selector: 'input[numbersOnly]'
})
export class NumberOnlyDirective {

  constructor(private _el: ElementRef) { }

  //@HostListener('input', ['$event']) onInputChange(event) {
  //  const initalValue = this._el.nativeElement.value;
  //  this._el.nativeElement.value = initalValue.replace(/[^0-9]*/g, '');
  //  if (initalValue !== this._el.nativeElement.value) {
  //    event.stopPropagation();
  //  }
  //}
  @HostListener('keydown', ['$event'])
  keyDownEvent(event: KeyboardEvent) {
    if (event.key.length === 1 && (event.shiftKey || event.which < 48 || event.which > 57) && (event.which < 95 || event.which > 106)) {
      event.preventDefault();
    }
  }

}
