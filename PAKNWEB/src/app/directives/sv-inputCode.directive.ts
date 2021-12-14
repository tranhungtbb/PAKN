import { Directive, HostListener } from '@angular/core';

@Directive({
  selector: 'input[inputCode]'
  //selector: '[editable]'
})
export class InputCodeDirective {

  constructor() { }
  keysban: number[] = [186, 187, 219, 221, 191, 220, 106, 111];


  @HostListener('keydown', ['$event'])
  keyDownEvent(event: KeyboardEvent) {
    //if (event.key.length === 1 && (event.shiftKey || event.which < 48 || event.which > 57) && (event.which < 95 || event.which > 106)) {
    //  event.preventDefault();
    //}
    if (event.key.length === 1 && (event.shiftKey || !((event.which >= 48 && event.which <= 57)
      || (event.which >= 65 && event.which <= 90)
      || (event.which >= 97 && event.which <= 122))
      && event.which != 8 || event.ctrlKey)) {
      event.preventDefault();
    }
  }
}
