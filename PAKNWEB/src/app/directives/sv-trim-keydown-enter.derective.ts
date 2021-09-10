import {Directive, ElementRef, ChangeDetectorRef} from "@angular/core";

@Directive({
  selector: 'input[trim]',
  host: { '(keydown.enter)' : 'onChange($event)'}
})

export class TrimDirective {

  constructor(private cdRef:ChangeDetectorRef, private el: ElementRef){}

  onChange($event:any) {
    let theEvent = $event || window.event;
    theEvent.target.value = theEvent.target.value.trim();
  }
}