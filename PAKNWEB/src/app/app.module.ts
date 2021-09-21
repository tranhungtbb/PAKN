import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http'
import { NgModule } from '@angular/core'
import { FormsModule, ReactiveFormsModule } from '@angular/forms' // <-- NgModel lives here
import { BrowserXhr, HttpModule } from '@angular/http'
import { MatDialogModule } from '@angular/material'
import { MatSnackBarModule } from '@angular/material/snack-bar'
import { BrowserModule } from '@angular/platform-browser'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { RouterModule } from '@angular/router'
import { EffectsModule } from '@ngrx/effects'
import { StoreModule } from '@ngrx/store'
import { StoreDevtoolsModule } from '@ngrx/store-devtools'
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker'
import { ToastrModule } from 'ngx-toastr'
import { AppRoutingModule } from './app-routing.module'
import { AppComponent } from './app.component'
import { CustomBrowserXhr } from './commons/CustomBrowserXhr'
import { ConfirmDialogComponent } from './directives/confirm-dialog/confirm-dialog.component'
import { SvFocusDirective } from './directives/sv-focus.directive'
import { ViewFileDialogComponent } from './directives/viewfile-dialog/viewfile-dialog.component'
import { CustomHttpInterceptor } from './http-interceptor/customHttpInterceptor'
import { ForbidenPageModule } from './modules/forbiden-page/forbiden-page.module'
import { LoginsModule } from './modules/logins/logins.module'
import { TreeModule } from 'primeng/tree'
import { CalendarModule } from 'primeng/calendar'
import { PdfJsViewerModule } from 'ng2-pdfjs-viewer'
import { ImageViewerModule } from 'ng2-image-viewer'
import { UnitFilterPipe } from './pipes/unit-filter.pipe'
import { DxReportViewerModule } from 'devexpress-reporting-angular'
import { TreeviewModule } from 'ngx-treeview';
import { CarouselModule } from 'ngx-owl-carousel-o';
import { TooltipModule  } from 'ngx-bootstrap/tooltip';

@NgModule({
	declarations: [AppComponent, SvFocusDirective, ConfirmDialogComponent, ViewFileDialogComponent],
	entryComponents: [ConfirmDialogComponent, ViewFileDialogComponent],
	imports: [
		BrowserModule,
		CalendarModule,
		BrowserAnimationsModule,
		FormsModule,
		ReactiveFormsModule.withConfig({ warnOnNgModelWithFormControl: 'never' }),
		HttpClientModule,
		HttpModule,
		BrowserAnimationsModule,
		MatSnackBarModule,
		LoginsModule,
		ForbidenPageModule,
		AppRoutingModule,
		RouterModule,
		ToastrModule.forRoot({
			timeOut: 2000,
			positionClass: 'toast-bottom-right',
			preventDuplicates: true,
		}),
		MatDialogModule,
		BsDatepickerModule.forRoot(),
		StoreDevtoolsModule.instrument({
			maxAge: 25, //  Retains last 25 states
		}),
		StoreModule.forRoot({}),
		EffectsModule.forRoot([]),
		PdfJsViewerModule,
		ImageViewerModule,
		TreeModule,
		DxReportViewerModule,
		TreeviewModule.forRoot(),
		CarouselModule,
		TooltipModule.forRoot()
	],
	providers: [
		{
			provide: HTTP_INTERCEPTORS,
			useClass: CustomHttpInterceptor,
			multi: true,
		},
		{ provide: BrowserXhr, useClass: CustomBrowserXhr },
	],
	bootstrap: [AppComponent],
})
export class AppModule {}
