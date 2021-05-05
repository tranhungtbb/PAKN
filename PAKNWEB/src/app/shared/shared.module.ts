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
import { ChangePipe } from 'src/app/pipes/unit-filter.pipe'
import { DisabledSpaceKeyDirective } from 'src/app/directives/sv-disabled-space-key.directive'
import { SvTextAreaTrimDirective } from '../directives/sv-textarea-trim.directive'
import { HtmlToPlaintextPipe } from '../pipes/html-filter.pipe'
import { UserCreateOrUpdateComponent } from '../modules/system-management/components/user/user-create-or-update/user-create-or-update.component'

@NgModule({
	declarations: [
		ConfirmClickDirective,
		SvBackButtonDirective,
		DisableControlDirective,
		SvTrimDirective,
		SvMoneyDirective,
		HasPermissionDirective,
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
	],
	exports: [
		ConfirmClickDirective,
		SvBackButtonDirective,
		DisableControlDirective,
		SvTrimDirective,
		SvMoneyDirective,
		HasPermissionDirective,
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
	],
})
export class SharedModule {}
