import { NgModule } from '@angular/core'
import { ConfirmClickDirective } from '../directives/confirm-click.directive'
import { SvBackButtonDirective } from '../directives/sv-back-button.directive'
import { DisableControlDirective } from '../directives/disable-control.directive'
import { SvTrimDirective } from '../directives/sv-trim.directive'
import { SvMoneyDirective } from '../directives/sv-money.directive'
import { HasPermissionDirective } from '../directives/sv-permission.directive'
import { ReportViewerComponent } from '../modules/report-view/report-viewcomponent'
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

@NgModule({
	declarations: [
		ConfirmClickDirective,
		SvBackButtonDirective,
		DisableControlDirective,
		SvTrimDirective,
		SvMoneyDirective,
		HasPermissionDirective,
		ReportViewerComponent,
		SvVoiceDirective,
		NumberOnlyDirective,
		ReCaptchaDirective,
		ViewFileDirective,
		SvScanDirective,
		NonSpecialcharDirective,
		InputCodeDirective,
		OnlyNumberDirective,
		StopLengthDirective,
		ChangePipe,
	],
	exports: [
		ConfirmClickDirective,
		SvBackButtonDirective,
		DisableControlDirective,
		SvTrimDirective,
		SvMoneyDirective,
		HasPermissionDirective,
		ReportViewerComponent,
		SvVoiceDirective,
		NumberOnlyDirective,
		ReCaptchaDirective,
		ViewFileDirective,
		SvScanDirective,
		NonSpecialcharDirective,
		InputCodeDirective,
		OnlyNumberDirective,
		StopLengthDirective,
		ChangePipe,
	],
})
export class SharedModule {}
