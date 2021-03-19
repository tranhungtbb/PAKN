import {
  Directive,
  ElementRef,
  forwardRef,
  HostListener,
  OnInit,
  Renderer2
} from '@angular/core';
import { Input } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

const TRIM_VALUE_ACCESSOR = {
  provide: NG_VALUE_ACCESSOR,
  // tslint:disable-next-line:no-forward-ref
  useExisting: forwardRef(() => SvMoneyDirective),
  multi: true
};

@Directive({
  // tslint:disable-next-line:directive-selector
  selector: 'input[svMoney]',
  providers: [TRIM_VALUE_ACCESSOR]
})
export class SvMoneyDirective implements ControlValueAccessor, OnInit  {
  // tslint:disable-next-line:no-input-rename
  @Input('svMoney') onEvent: 'keyup' | 'focusout';
  _onChange(_: any) { }
  _onTouched() { }
  registerOnChange(fn: (value: any) => any): void {
    this._onChange = fn;
  }
  registerOnTouched(fn: () => any): void {
    this._onTouched = fn;
  }
  constructor(private _renderer: Renderer2, private _elementRef: ElementRef) { }

  writeValue(value: any): void {
    if (value !== undefined && value !== null) {
      this._renderer.setProperty(
        this._elementRef.nativeElement,
        'value',
        value
      );
    }
  }

  ngOnInit() {
    this.onEvent = this.onEvent || 'focusout';
  }

  @HostListener('keydown', ['$event'])
  keyDownEvent(event: KeyboardEvent) {
    if (event.key.length === 1 && (event.shiftKey || event.which < 48 || event.which > 57) && (event.which < 95 || event.which > 106)) {
      event.preventDefault();
    }
      //const element = <HTMLInputElement>event.target;
      //var trimValue = element.value.toString().replace(",", "");
      //const val = trimValue.replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,');
      //this.writeValue(val);
      //this._onChange(val);
  }

  @HostListener('keyup', ['$event'])
  _onKeyUp(event: Event) {
      const element = <HTMLInputElement>event.target;
      var trimValue = element.value.replace(/[^0-9\.]+/g, "");
      const val = trimValue.replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,');
      this.writeValue(val);
      this._onChange(val);
  }
}
