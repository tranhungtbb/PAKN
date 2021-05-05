import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { NgSelectModule } from '@ng-select/ng-select'
import { CKEditorModule } from '@ckeditor/ckeditor5-angular'
import { ReactiveFormsModule, FormsModule } from '@angular/forms'
import { BusinessIndividualRoutingModule } from './business-individual-routing.module'
import { BusinessIndividualComponent } from './business-individual.component'
import { BusinessComponent } from './components/business/business.component'
import { IndividualComponent  } from './components/individual/individual.component'

@NgModule({
	imports: [CommonModule, BusinessIndividualRoutingModule],
	declarations: [BusinessIndividualComponent,BusinessComponent,IndividualComponent ],
})
export class BusinessIndividualModule {}
