import { Component, OnInit } from '@angular/core'
import * as ClassicEditor from '@ckeditor/ckeditor5-build-classic'
import { FormGroup, FormBuilder, Validators, FormArray, FormControl } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'
import { Router, ActivatedRoute, ParamMap } from '@angular/router'

import { NewsService } from 'src/app/services/news.service'

import { COMMONS } from 'src/app/commons/commons'
import { NewsModel } from 'src/app/models/NewsObject'

@Component({
	selector: 'app-news-create-or-update',
	templateUrl: './news-create-or-update.component.html',
	styleUrls: ['./news-create-or-update.component.css'],
})
export class NewsCreateOrUpdateComponent implements OnInit {
	constructor(private toast: ToastrService, private formBuilder: FormBuilder, private newsService: NewsService, private router: Router) {}
	public Editor = ClassicEditor
	model: NewsModel = new NewsModel()
	newsFrom: FormGroup
	listNewsCategories: Array<any>
	postTypes: any[] = [
		{ text: 'thường', value: true, checked: true },
		{ text: 'nổi bật', value: false, checked: false },
	]

	categoriesSelected: any[]
	ngOnInit() {
		this.newsFrom = this.formBuilder.group({
			title: ['', [Validators.required]],
			summary: [this.model.content, [Validators.required]],
			content: [''],
			newsType: [''],
			postType: [''],
			pushNotify: [''],
		})
	}

	get f() {
		return this.newsFrom.controls
	}

	submitted = false
	onSave(event) {
		this.submitted = true
		if (this.newsFrom.invalid) {
			return
		}

		if (this.model.id && this.model.id > 0) {
			this.newsService.create(this.model).subscribe((res) => {
				if (res.success != 'OK') {
					this.toast.error(COMMONS.ADD_FAILED)
					return
				}
				this.toast.success(COMMONS.ADD_SUCCESS)
				this.router.navigate(['/quan-tri/tin-tuc'])
			})
		} else {
			this.newsService.update(this.model).subscribe((res) => {
				if (res.success != 'OK') {
					this.toast.error(COMMONS.UPDATE_FAILED)
					return
				}
				this.toast.success(COMMONS.UPDATE_SUCCESS)
				this.router.navigate(['/quan-tri/tin-tuc'])
			})
		}
	}

	onPublished() {
		this.router.navigate(['/quan-tri/tin-tuc'])
	}

	public onChangeEditor({ editor }: any) {
		let data = editor.getData()
		this.model.content = data
	}
	public onReady(editor) {
		editor.ui.getEditableElement().parentElement.insertBefore(editor.ui.view.toolbar.element, editor.ui.getEditableElement())
	}
}
