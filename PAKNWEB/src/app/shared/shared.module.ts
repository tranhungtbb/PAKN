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
	],
})
export class SharedModule {}
