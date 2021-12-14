import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { NgSelectModule } from '@ng-select/ng-select'
import { CKEditorModule } from '@ckeditor/ckeditor5-angular'
import { ReactiveFormsModule, FormsModule } from '@angular/forms'
import { BusinessIndividualRoutingModule } from './business-individual-routing.module'
import { BusinessIndividualComponent } from './business-individual.component'
import { SharedModule } from '../../shared/shared.module'
import { AngularDualListBoxModule } from 'angular-dual-listbox'
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker'
import { ContextMenuModule } from 'primeng/contextmenu'
import { TableModule } from 'primeng/table'
import { DropdownModule } from 'primeng/dropdown'
import { TreeModule } from 'primeng/tree'
import { TooltipModule } from 'primeng/tooltip'
import { GMapModule } from 'primeng/gmap'
import { MatCheckboxModule } from '@angular/material/checkbox'
import { MatDialogModule } from '@angular/material/dialog'
import { BusinessComponent } from './components/business/business.component'
import { IndividualComponent } from './components/individual/individual.component'
import { CreateUpdBusinessComponent } from './components/create-upd-business/create-upd-business.component'
import { OrgFormAddressComponent } from './components/create-upd-business/org-form-address/org-form-address.component'
import { OrgRepreFormComponent } from './components/create-upd-business/org-repre-form/org-repre-form.component'

@NgModule({
	imports: [
		CommonModule,
		NgSelectModule,
		BusinessIndividualRoutingModule,
		ReactiveFormsModule,
		FormsModule,
		AngularDualListBoxModule,
		SharedModule,
		BsDatepickerModule.forRoot(),
		ContextMenuModule,
		TableModule,
		DropdownModule,
		TreeModule,
		TooltipModule,
		GMapModule,
		MatCheckboxModule,
		CKEditorModule,
		MatDialogModule,
	],
	declarations: [BusinessIndividualComponent, BusinessComponent, IndividualComponent, CreateUpdBusinessComponent, OrgFormAddressComponent, OrgRepreFormComponent],
	providers: [],
})
export class BusinessIndividualModule {}
