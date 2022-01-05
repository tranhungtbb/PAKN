import { NgModule } from '@angular/core'
import { ConfirmClickDirective } from '../directives/confirm-click.directive'
import { SvBackButtonDirective } from '../directives/sv-back-button.directive'
import { DisableControlDirective } from '../directives/disable-control.directive'
import { SvTrimDirective } from '../directives/sv-trim.directive'
import { SvMoneyDirective } from '../directives/sv-money.directive'
import { HasPermissionDirective } from '../directives/sv-permission.directive'
import { SvVoiceDirective } from '../directives/sv-voice.directive'
import { NumberOnlyDirective } from '../directives/sv-number.directive'
import { ReCaptchaDirective } from '../directives/sv-reCaptcha.directive'
import { ViewFileDirective } from '../directives/view-file.directive'
import { NonSpecialcharDirective } from '../directives/sv-nonSpecialchar.directive'
import { InputCodeDirective } from '../directives/sv-inputCode.directive'
import { SvScanDirective } from '../directives/sv-scan.directive'
import { OnlyNumberDirective } from '../directives/only-number'
import { StopLengthDirective } from '../directives/stop-length.directive'
import { ChangePipe, RemoveTagPipe } from 'src/app/pipes/unit-filter.pipe'
import { DisabledSpaceKeyDirective } from 'src/app/directives/sv-disabled-space-key.directive'
import { SvTextAreaTrimDirective } from '../directives/sv-textarea-trim.directive'
import { HtmlToPlaintextPipe } from '../pipes/html-filter.pipe'
import { DiaDanhFilterPipe } from '../pipes/dia-danh-filter.pipi'
import { SanitizerUrlPipe, SafeUrlPipe } from '../pipes/sanitizer-url.pipe'
import { HasNotPermissionDirective } from 'src/app/directives/sv-notpermission.directive'
import { TrimDirective } from 'src/app/directives/sv-trim-keydown-enter.derective'
import { OembedPipe } from 'src/app/pipes/oembed.pipe'
import { SafeHtmlPipe } from '../pipes/safe-html.pipe'

@NgModule({
	declarations: [
		ConfirmClickDirective,
		SvBackButtonDirective,
		DisableControlDirective,
		SvTrimDirective,
		SvMoneyDirective,
		HasPermissionDirective,
		HasNotPermissionDirective,
		SvVoiceDirective,
		NumberOnlyDirective,
		ReCaptchaDirective,
		ViewFileDirective,
		SvScanDirective,
		NonSpecialcharDirective,
		InputCodeDirective,
		OnlyNumberDirective,
		StopLengthDirective,
		DisabledSpaceKeyDirective,
		ChangePipe,
		SvTextAreaTrimDirective,
		HtmlToPlaintextPipe,
		DiaDanhFilterPipe,
		SanitizerUrlPipe,
		SafeUrlPipe,
		TrimDirective,
		OembedPipe,
		RemoveTagPipe,
		SafeHtmlPipe
	],
	exports: [
		ConfirmClickDirective,
		SvBackButtonDirective,
		DisableControlDirective,
		SvTrimDirective,
		SvMoneyDirective,
		HasPermissionDirective,
		HasNotPermissionDirective,
		SvVoiceDirective,
		NumberOnlyDirective,
		ReCaptchaDirective,
		ViewFileDirective,
		SvScanDirective,
		NonSpecialcharDirective,
		InputCodeDirective,
		OnlyNumberDirective,
		StopLengthDirective,
		DisabledSpaceKeyDirective,
		ChangePipe,
		SvTextAreaTrimDirective,
		HtmlToPlaintextPipe,
		DiaDanhFilterPipe,
		SanitizerUrlPipe,
		SafeUrlPipe,
		TrimDirective,
		OembedPipe,
		RemoveTagPipe,
		SafeHtmlPipe
	],
})
export class SharedModule { }
