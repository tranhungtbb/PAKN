import { AfterViewInit, Directive, ElementRef, EventEmitter, forwardRef, Inject, Injectable, InjectionToken, Injector, Input, NgZone, OnInit, Output } from '@angular/core';
import { AbstractControl, ControlValueAccessor, FormControl, NG_VALUE_ACCESSOR, NgControl, Validators } from '@angular/forms';
import { Http } from '@angular/http';

declare const grecaptcha: any;

declare global {
  interface Window {
    grecaptcha: any;
    reCaptchaLoad: () => void
  }
}

export const RECAPTCHA_URL = new InjectionToken('RECAPTCHA_URL');

@Injectable()
export class ReCaptchaAsyncValidator {

  constructor(private http: Http, @Inject(RECAPTCHA_URL) private url) {
  }

  validateToken(token: string) {
    return (_: AbstractControl) => {
      return this.http.get(this.url, { params: { token } }).map(res => res.json()).map(res => {
        if (!res.success) {
          return { tokenInvalid: true }
        }
        return null;
      });
    }
  }
}

export interface ReCaptchaConfig {
  theme?: 'dark' | 'light';
  type?: 'audio' | 'image';
  size?: 'compact' | 'normal';
  tabindex?: number;
}

@Directive({
  selector: '[svRecaptcha]',
  exportAs: 'svRecaptcha',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => ReCaptchaDirective),
      multi: true
    },
    ReCaptchaAsyncValidator
  ]
})
export class ReCaptchaDirective implements OnInit, AfterViewInit, ControlValueAccessor {
  @Input() key: string;
  @Input() config: ReCaptchaConfig = {};
  @Input() lang: string;

  @Output() captchaResponse = new EventEmitter<string>();
  @Output() captchaExpired = new EventEmitter();

  private control: FormControl;
  private widgetId: number;

  private onChange: (value: string) => void;
  private onTouched: (value: string) => void;

  constructor(private element: ElementRef, private ngZone: NgZone, private injector: Injector, private reCaptchaAsyncValidator: ReCaptchaAsyncValidator) {
  }

  ngOnInit() {
    this.registerReCaptchaCallback();
    this.addScript();
  }

  registerReCaptchaCallback() {
    window.reCaptchaLoad = () => {
      const config = {
        ...this.config,
        'sitekey': this.key,
        'callback': this.onSuccess.bind(this),
        'expired-callback': this.onExpired.bind(this)
      };
      this.widgetId = this.render(this.element.nativeElement, config);
    };
  }

  ngAfterViewInit() {
    this.control = this.injector.get(NgControl).control;
    this.setValidators();
  }

  /**
   * Useful for multiple captcha
   * @returns {number}
   */
  getId() {
    return this.widgetId;
  }

  /**
   * Calling the setValidators doesn't trigger any update or value change event.
   * Therefore, we need to call updateValueAndValidity to trigger the update
   */
  private setValidators() {
    this.control.setValidators(Validators.required);
    this.control.updateValueAndValidity();
  }

  writeValue(obj: any): void {
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  /**
   * onExpired
   */
  onExpired() {
    this.ngZone.run(() => {
      this.captchaExpired.emit();
      this.onChange(null);
      this.onTouched(null);
    });
  }

  /**
   *
   * @param response
   */
  onSuccess(token: string) {
    this.ngZone.run(() => {
      this.verifyToken(token);
      this.captchaResponse.next(token);
      this.onChange(token);
      this.onTouched(token);
    });
  }

  /**
   *
   * @param token
   */
  verifyToken(token: string) {
    this.control.setAsyncValidators(this.reCaptchaAsyncValidator.validateToken(token))
    this.control.updateValueAndValidity();
  }

  /**
   * Renders the container as a reCAPTCHA widget and returns the ID of the newly created widget.
   * @param element
   * @param config
   * @returns {number}
   */
  private render(element: HTMLElement, config): number {
    return grecaptcha.render(element, config);
  }

  /**
   * Resets the reCAPTCHA widget.
   */
  reset(): void {
    if (!this.widgetId) return;
    grecaptcha.reset(this.widgetId);
    this.onChange(null);
  }

  /**
   * Gets the response for the reCAPTCHA widget.
   * @returns {string}
   */
  getResponse(): string {
    if (!this.widgetId)
      return grecaptcha.getResponse(this.widgetId);
  }

  /**
   * Add the script
   */
  addScript() {
    let script = document.createElement('script');
    const lang = this.lang ? '&hl=' + this.lang : '';
    script.src = `https://www.google.com/recaptcha/api.js?onload=reCaptchaLoad&render=explicit${lang}`;
    script.async = true;
    script.defer = true;
    document.body.appendChild(script);
  }

}
