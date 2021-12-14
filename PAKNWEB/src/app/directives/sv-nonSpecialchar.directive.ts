import { Directive, HostListener } from '@angular/core'; 

@Directive({
  selector: '[nonSpecialchar]'
  //selector: '[editable]'
})
export class NonSpecialcharDirective {

  constructor() { }
  keysban: number[]=[186,187,219,221,191,220,106,111];

 
  @HostListener('keydown', ['$event'])
  keyDownEvent(event: KeyboardEvent) {
    if (event.key.length === 1 && (/[~`!@#$%\^&*()+=\-\[\]\\';/{}|\\":<>\?]/g.test(event.key))) {
      event.preventDefault();
    }
    //if (this.keysban) {
    //  if (event.key.length === 1 && (/[~`!@#$%\^&*()+=\-\[\]\\';,/{}|\\":<>\?]/g.test(event.key))) {
    //    event.preventDefault();
    //  }
    //}
  }

}
