import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { ReactiveFormsModule, FormsModule } from '@angular/forms'
import { SharedModule } from '../../shared/shared.module'
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker'
import { NgSelectModule } from '@ng-select/ng-select'
import { CKEditorModule } from '@ckeditor/ckeditor5-angular'

import { NewsRoutingModule } from './news-routing.module'
import { NewsComponent } from './news.component'
import { NewsCreateOrUpdateComponent } from './news-create-or-update/news-create-or-update.component'

@NgModule({
	declarations: [NewsComponent, NewsCreateOrUpdateComponent],
	imports: [CommonModule, NewsRoutingModule, ReactiveFormsModule, FormsModule, SharedModule, BsDatepickerModule, NgSelectModule, CKEditorModule],
})
export class NewsModule {}
