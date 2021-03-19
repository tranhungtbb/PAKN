import { Directive, HostListener, ElementRef } from '@angular/core';
import { Location } from '@angular/common';

@Directive({
  selector: '[appSvBackButton]'
})
export class SvBackButtonDirective {

  constructor(private location: Location, private el: ElementRef) { }

  @HostListener("click", ["$event"])
  onClick($event: Event) {
    $event.preventDefault();
    $event.stopPropagation();
    this.location.back();
  }

}
