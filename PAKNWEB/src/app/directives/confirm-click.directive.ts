import { Directive, Output, EventEmitter, HostListener, ElementRef, Renderer2, Input } from '@angular/core';
import { MatDialog } from '@angular/material';
import { ConfirmDialogComponent } from './confirm-dialog/confirm-dialog.component';

@Directive({
  selector: '[appConfirmClick]'
})
export class ConfirmClickDirective {
  @Output() appConfirmClick: EventEmitter<any> = new EventEmitter();
  @Input() validatebefore: boolean;
  @Output() invalidate: EventEmitter<any> = new EventEmitter();

  constructor(public dialog: MatDialog, private el: ElementRef, renderer: Renderer2) {
  }

  @HostListener('click', ['$event'])
  onClick(e) {

    if (this.validatebefore || this.validatebefore == undefined) {
      const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      });

      dialogRef.afterClosed().subscribe(result => {
        if (result !== true) {
          e.preventDefault();
          e.stopPropagation();
        } else {
          this.appConfirmClick.next(e);
        }
      });
    } else {
      this.invalidate.next(e);
    }

   
  }
}


