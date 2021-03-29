import { Component, OnInit } from '@angular/core'
import * as ClassicEditor from '@ckeditor/ckeditor5-build-classic'
import { FormGroup, FormBuilder, Validators, FormArray, FormControl } from '@angular/forms'

import { NewsModel } from 'src/app/models/NewsObject'

@Component({
	selector: 'app-news-create-or-update',
	templateUrl: './news-create-or-update.component.html',
	styleUrls: ['./news-create-or-update.component.css'],
})
export class NewsCreateOrUpdateComponent implements OnInit {
	constructor(private formBuilder: FormBuilder) {}
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
			summary: ['', [Validators.required]],
			content: ['', [Validators.required]],
			newsType: [''],
			postType: [''],
			pushNotify: [''],
		})
	}

	get f() {
		return this.newsFrom.controls
	}
	onSave() {}

	public onReady(editor) {
		editor.ui.getEditableElement().parentElement.insertBefore(editor.ui.view.toolbar.element, editor.ui.getEditableElement())
	}
}
