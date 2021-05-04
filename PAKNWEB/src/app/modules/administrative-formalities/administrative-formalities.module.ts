import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { ReactiveFormsModule, FormsModule } from '@angular/forms'
import { SharedModule } from '../../shared/shared.module'
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker'
import { NgSelectModule } from '@ng-select/ng-select'
import { CKEditorModule } from '@ckeditor/ckeditor5-angular'
import { MatDialogModule } from '@angular/material/dialog'
import { TableModule } from 'primeng/table'

import { AdministrativeFormalitiesRoutingModule } from './administrative-formalities-routing.module'
import { AdministrativeFormalitiesComponent } from './administrative-formalities.component'
import { CU_AdministrativeFormalitiesComponent } from './cu-administrative-formalities/cu-administrative-formalities.component'
import { ListAdministrativeFormalitiesComponent } from './list-administrative-formalities/list-administrative-formalities.component'
import { DetailAdministrativeFormalitiesComponent } from './detail-administrative-formalities/detail-administrative-formalities.component'

import { HtmlToPlaintextPipe } from 'src/app/pipes/html-filter.pipe'
@NgModule({
	declarations: [AdministrativeFormalitiesComponent, CU_AdministrativeFormalitiesComponent, ListAdministrativeFormalitiesComponent, DetailAdministrativeFormalitiesComponent],
	imports: [
		CommonModule,
		AdministrativeFormalitiesRoutingModule,
		ReactiveFormsModule,
		FormsModule,
		SharedModule,
		BsDatepickerModule,
		NgSelectModule,
		CKEditorModule,
		MatDialogModule,
		TableModule,
		HtmlToPlaintextPipe,
	],
})
export class AdministrativeFormalitiesModule {}
