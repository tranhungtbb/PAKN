import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { ReactiveFormsModule, FormsModule } from '@angular/forms'
import { SharedModule } from '../../shared/shared.module'
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker'
import { NgSelectModule } from '@ng-select/ng-select'
import { CKEditorModule } from '@ckeditor/ckeditor5-angular'
import { MatDialogModule } from '@angular/material/dialog'
import { TableModule } from 'primeng/table'

import { NewsRoutingModule } from './news-routing.module'
import { NewsComponent } from './news.component'
import { NewsCreateOrUpdateComponent } from './news-create-or-update/news-create-or-update.component'
import { NewsRelateModalComponent } from './news-relate-modal/news-relate-modal.component'
import { NewsDetailComponent } from './news-detail/news-detail.component'
import { GetCatePipe } from './get-cate.pipe'
import { from } from 'rxjs'

@NgModule({
	declarations: [NewsComponent, NewsCreateOrUpdateComponent, NewsRelateModalComponent, GetCatePipe, NewsDetailComponent],
	imports: [CommonModule, NewsRoutingModule, ReactiveFormsModule, FormsModule, SharedModule, BsDatepickerModule, NgSelectModule, CKEditorModule, MatDialogModule, TableModule],
})
export class NewsModule {}
